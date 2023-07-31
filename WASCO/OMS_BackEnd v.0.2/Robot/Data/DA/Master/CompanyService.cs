using Robot.Data.DA.Login;
using Robot.Data.GADB.TT;
using Robot.Service.FileGo;
using System;
using System.Collections.Generic;
using System.Linq;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.Master {
    public class CompanyService {
        public static string sessionActiveId = "activecomid";
        public class CompanyInfoList {
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string ComCode { get; set; }
            public string ParentID { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string TypeName { get; set; }
            public string TypeID { get; set; }
            public string Phone { get; set; }
            public string TaxID { get; set; }
            public string FullAddr { get; set; }
            public bool IsWH { get; set; }
        }

        public class I_ComSet {
            public CompanyInfo ComInfo { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
            public List<vw_XFilesRef> files { get; set; }
            public bool NeedRunNextID { get; set; }
        }


        public I_ComSet DocSet { get; set; }

       
        public static I_ComSet RefreshFile(I_ComSet doc) {
            try {
                var h = doc.ComInfo;
                doc.files = FileGo.ListFilesRef(h.RCompanyID, h.CompanyID, FileGo.Type_CompanySignatureAll, h.CompanyID);
            } catch (Exception ex) {
            }
            return doc;
        }
        #region Query Transaction



        public I_ComSet GetDocSet(string comid, string rcom) {
            I_ComSet n = NewTransaction(rcom);
            using (GAEntities db = new GAEntities()) {
                n.ComInfo = db.CompanyInfo.Where(o => o.CompanyID == comid && o.RCompanyID == rcom).FirstOrDefault();
                n.files = db.vw_XFilesRef.Where(o => o.IsActive == true && o.RCompanyID == rcom && o.CompanyID == comid && o.DocID==comid && o.DocType == FileGo.Type_CompanySignatureAll).ToList();
                n.Log = db.TransactionLog.Where(o => o.TransactionID == comid && o.TableID == "COMPANY").OrderBy(o => o.CreatedDate).ToList();
            }
            return n;
        }

        public static CompanyInfo GetComInfoByComID(string comId) {
            CompanyInfo com = new CompanyInfo();
            using (GAEntities db = new GAEntities()) {
                com = db.CompanyInfo.Where(o => o.CompanyID == comId).FirstOrDefault();
            }
            return com;
        }

        public static CompanyInfo GetComInfoByComID(string rcom, string comId) {
            CompanyInfo com = new CompanyInfo();
            using (GAEntities db = new GAEntities()) {
                com = db.CompanyInfo.Where(o => o.CompanyID == comId && o.RCompanyID == rcom).FirstOrDefault();
            }

            return com;
        }
        public static CompanyInfo GetCompanyInfo(string rcom, string comId) {
            CompanyInfo result = new CompanyInfo();

            using (GAEntities db = new GAEntities()) {
                result = db.CompanyInfo.Where(o => o.CompanyID == comId && o.RCompanyID == rcom).FirstOrDefault();
            }
            return result;
        }
        public static CompanyInfo GetRCompanyInfo(string rcomId) {
            CompanyInfo result = new CompanyInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.CompanyInfo.Where(o => o.CompanyID == rcomId && o.TypeID == "COMPANY").FirstOrDefault();
            }
            return result;
        }

        public static List<CompanyInfo> ListCompany(List<string> uirc ) {
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities()) {
                
                result = db.CompanyInfo.Where(o => o.IsActive == true
                                                        && uirc.Contains(o.CompanyID) 
                                                        && o.TypeID == "COMPANY"
                                                        ).ToList();
            }
            return result;
        }
        public static List<CompanyInfo> ListBranch(/*List<string> uic, string group*/) {
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities()) {

                result = db.CompanyInfo.Where(o => o.IsActive == true
                                                        //&& uic.Contains(o.CompanyID)
                                                        //&& (o.GroupCode == group || group == "")
                                                        && o.TypeID == "BRANCH"
                                                        ).ToList();
            }
            return result;
        }

        public static List<CompanyInfo> ListCompanyGroup(List<string> uic,List<string> uirc) {
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities()) {
                if (uic==null) {
                    result = db.CompanyInfo.Where(o => o.TypeID == "GROUP" 
                                                && uirc.Contains(o.RCompanyID)
                                                && o.IsActive == true).ToList();
                } else {
                    result = db.CompanyInfo.Where(o => o.TypeID == "GROUP"
                                                && uic.Contains(o.CompanyID)
                                                && uirc.Contains(o.RCompanyID)
                                                && o.IsActive == true).ToList();
                }
            
            }
            return result;
        }
        public static List<CompanyInfoList> ListCompanyInfoUIC(LogInService.LoginSet login, string type, bool addShowAll) {
            var comlist = login.UserInCompany;
            var rcom = login.CurrentRootCompany.CompanyID;
            List<CompanyInfoList> result = new List<CompanyInfoList>();
            using (GAEntities db = new GAEntities()) {
                var query = db.vw_CompanyInfo.Where(o =>
                                                    (o.TypeID == type || type == "")
                                                     && comlist.Contains(o.CompanyID)
                                                     && o.RCompanyID == rcom

                                                     && o.IsActive
                                                ).ToList();
                foreach (var q in query) {
                    CompanyInfoList n = new CompanyInfoList();
                    n.RCompanyID = q.RCompanyID;
                    n.CompanyID = q.CompanyID;
                    n.Name = q.Name1 + " " + q.Name2 + " (" + q.CompanyID + ")";
                    n.TypeName = q.TypeName;
                    n.TypeID = q.TypeID;
                    n.FullAddr = q.AddrFull;
                    n.TaxID = q.TaxID;
                    n.IsWH = Convert.ToBoolean(q.IsWH);
                    result.Add(n);
                }
                if (addShowAll) {
                    CompanyInfoList blank = new CompanyInfoList { ComCode = "", CompanyID = "", Name = "ทุกสาขา", TypeID = "", TaxID = "", FullAddr = "" };
                    result.Insert(0, blank);
                }

            }
            return result;
        }

        public static List<CompanyInfo> ListCompanyByUserPermission(string rcom, string username) {
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities()) {
                var coms = db.vw_UIC.Where(o => o.RCompanyID == rcom && o.UserName == username).Select(o => o.CompanyID).ToList();
                result = db.CompanyInfo.Where(o => o.IsActive == true
                                                                && o.RCompanyID == rcom
                                                                && coms.Contains(o.CompanyID)
                                                                ).ToList();
            }
            return result;
        }

        public static List<vw_CompanyInfo> ListCom(string Search, string rcom, bool ShowActive) {
            List<vw_CompanyInfo> result = new List<vw_CompanyInfo>();
            using (GAEntities db = new GAEntities()) {

                result = db.vw_CompanyInfo.Where(o =>
                                                        (o.CompanyID.Contains(Search)
                                                        || o.Name1.Contains(Search)
                                                        || o.Name2.Contains(Search)
                                                        || o.Tel1.Contains(Search)
                                                        || o.Mobile.Contains(Search)
                                                        || o.Email.Contains(Search)
                                                        || o.TypeID.Contains(Search)
                                                        || o.TypeName.Contains(Search)
                                                        || Search == "")
                                                        && o.TypeID=="BRANCH"
                                                        && o.RCompanyID == rcom
                                                        && o.IsActive == ShowActive
                                                        ).OrderByDescending(o => o.CreatedDate).ToList();

            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_ComSet doc,bool isnew ) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var h = doc.ComInfo;
                using (GAEntities db = new GAEntities()) {
                    

                    if (isnew) {
                        bool checkdup = checkDupID(doc.ComInfo);
                        if (checkdup == false) {
                            result.Result = "fail";
                            result.Message1 = $"Duplicate company code : {h.CompanyID}";
                            return result;
                        }

                        db.CompanyInfo.Add(doc.ComInfo);
                        db.SaveChanges();

                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.ComInfo.CompanyID, TableID = "COMPANY", ParentID = h.CompanyID, TransactionDate = DateTime.Now, CompanyID = "", Action = "INSERT NEW COMPANY" }, h.RCompanyID, h.CompanyID, h.CreatedBy);
                    } else {
                        var n = db.CompanyInfo.Where(o => o.CompanyID == doc.ComInfo.CompanyID && o.RCompanyID == h.RCompanyID).FirstOrDefault();
                        n.RCompanyID = doc.ComInfo.RCompanyID;
                        n.GroupCode = doc.ComInfo.GroupCode;
                        n.ComCode = doc.ComInfo.ComCode;
                        n.BrnCode = doc.ComInfo.BrnCode;
                        n.ParentID = doc.ComInfo.ParentID;
                        n.ShortCode = doc.ComInfo.ShortCode;
                        n.TypeID = doc.ComInfo.TypeID;
                        n.Name1 = doc.ComInfo.Name1;
                        n.Name2 = doc.ComInfo.Name2;
                        n.BillAddr1 = doc.ComInfo.BillAddr1;
                        n.BillAddr2 = doc.ComInfo.BillAddr2;
                        n.PrintHeader1=doc.ComInfo.PrintHeader1;
                        n.PrintHeader2 = doc.ComInfo.PrintHeader2;
                        n.TaxID = doc.ComInfo.TaxID;
                        n.AddrFull = doc.ComInfo.AddrFull;
                        n.AddrFull2 = doc.ComInfo.AddrFull2;
                        n.AddrNo = doc.ComInfo.AddrNo;
                        n.AddrTanon = doc.ComInfo.AddrTanon;
                        n.AddrTumbon = doc.ComInfo.AddrTumbon;
                        n.AddrAmphoe = doc.ComInfo.AddrAmphoe;
                        n.AddrProvince = doc.ComInfo.AddrProvince;
                        n.AddrPostCode = doc.ComInfo.AddrPostCode;
                        n.AddrCountry = doc.ComInfo.AddrCountry;
                        n.Currency = doc.ComInfo.Currency;
                        n.Tel1 = doc.ComInfo.Tel1;
                        n.Tel2 = doc.ComInfo.Tel2;
                        n.Mobile = doc.ComInfo.Mobile;
                        n.Email = doc.ComInfo.Email;
                        n.Fax = doc.ComInfo.Fax;
                        n.SalePersonID = doc.ComInfo.SalePersonID;
                        n.IsWH = false;
                        n.Remark1 = doc.ComInfo.Remark1;
                        n.Remark2 = doc.ComInfo.Remark2;
                        n.ModifiedDate = DateTime.Now;
                        n.ModifiedBy = doc.ComInfo.ModifiedBy;

                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.ComInfo.CompanyID, TableID = "COMPANY", ParentID = h.CompanyID, TransactionDate = DateTime.Now, CompanyID = "", Action = "UPDATE COMPANY" }, h.RCompanyID, h.CompanyID, h.ModifiedBy);

                    }
                }
                var r = InitData.NewMiscBeginOfNewCompany(h.RCompanyID, doc.ComInfo.CompanyID);
                //var r2 = InitData.CreateDefaultGroupForAllRCom();
                var r3 = InitData.NewDefaultPayment(h.RCompanyID, doc.ComInfo.CompanyID);
                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + " " + r.Message1;
                }
                //if (r2.Result == "fail")
                //{
                //    result.Result = "fail";
                //    result.Message1 = result.Message1 + " " + r2.Message1;
                //}
                if (r3.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + " " + r3.Message1;
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        public static bool checkDupID(CompanyInfo com) {
            bool result = false;
            try {
                string comid = com.CompanyID;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.CompanyInfo.Where(o => o.CompanyID == comid && o.RCompanyID == com.RCompanyID).FirstOrDefault();

                    if (get_id == null) {
                        result = true;
                    }


                }
            } catch (Exception ex) {
            }
            return result;
        }

        #endregion

        #region Delete


        public static I_BasicResult Delete(string comid, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var com = db.CompanyInfo.Where(o => o.CompanyID == comid && o.RCompanyID == rcom).FirstOrDefault();
                    com.ModifiedBy = "ONLINE";
                    com.ModifiedDate = DateTime.Now;
                    com.IsActive = false;

                    db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";

                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }


        #endregion
 


        #region New transaction

        public I_ComSet NewTransaction(string rcom) {
            I_ComSet n = new I_ComSet();

            n.ComInfo = NewHead(rcom);
            n.files = new List<vw_XFilesRef>();
            n.Log = new List<TransactionLog>();
            n.NeedRunNextID = false;

            return n;
        }

        public static CompanyInfo NewHead(string rcom) {
            CompanyInfo n = new CompanyInfo();

            n.CompanyID = "";
            n.RCompanyID = rcom;
            n.GroupCode = "";
            n.ComCode = "";
            n.BrnCode = "";
            n.ParentID = "";
            n.ShortCode = "";
            n.TypeID = "BRANCH";
            n.Name1 = "";
            n.Name2 = "";
            n.BillAddr1 = "";
            n.BillAddr2 = "";
            n.TaxID = "";
            n.AddrFull = "";
            n.AddrFull2 = "";
            n.AddrNo = "";
            n.AddrTanon = "";
            n.AddrTumbon = "";
            n.AddrAmphoe = "";
            n.AddrProvince = "";
            n.AddrPostCode = "";
            n.AddrCountry = "";
            n.Currency = "THB";
            n.Tel1 = "";
            n.Tel2 = "";
            n.Mobile = "";
            n.Email = "";
            n.Fax = "";
            n.SalePersonID = "";
            n.IsWH = false;

            n.Remark1 = "";
            n.Remark2 = "";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        #endregion


        public static List<LocationInfo> ListLocationInfo(string rcom, string com) {
            List<LocationInfo> result = new List<LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.LocationInfo.Where(o => o.RCompanyID == rcom
                                                        && o.CompanyID == com
                                                        && o.IsActive == true).ToList();
            }
            return result;
        }

    }
}
