using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
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
            public List<LocationInfo> Location { get; set; }
            public List<POS_Table> Table { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
            public bool NeedRunNextID { get; set; }
        }

        public string PreviousPageUrl = "";
        public I_ComSet DocSet { get; set; }

        public CompanyService() {

        }

        #region Query Transaction



        public I_ComSet GetDocSet(string comid, string rcom) {
            I_ComSet n = NewTransaction(rcom);
            using (GAEntities db = new GAEntities()) {
                n.ComInfo = db.CompanyInfo.Where(o => o.CompanyID == comid && o.RCompanyID == rcom).FirstOrDefault();
                n.Location = db.LocationInfo.Where(o => o.CompanyID == comid && o.RCompany == rcom && o.IsActive).ToList();
                n.Table = db.POS_Table.Where(o => o.ComID == comid && o.RComID == rcom && o.IsActive).OrderBy(o => o.TableName).ToList();
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

        public static List<CompanyInfo> ListCompany(List<string> uirc) {
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities()) {

                result = db.CompanyInfo.Where(o => o.IsActive == true
                                                        && uirc.Contains(o.CompanyID)
                                                        && o.TypeID == "COMPANY"
                                                        ).ToList();
            }
            return result;
        }
        public static List<CompanyInfo> ListBranch(List<string> uic, string group) {
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities()) {

                result = db.CompanyInfo.Where(o => o.IsActive == true
                                                        && uic.Contains(o.CompanyID)
                                                        && (o.GroupCode == group || group == "")
                                                        && o.TypeID == "BRANCH"
                                                        ).ToList();
            }
            return result;
        }

        public static List<CompanyInfo> ListCompanyGroup(List<string> uic, List<string> uirc) {
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities()) {
                if (uic == null) {
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
                                                        && o.TypeID == "BRANCH"
                                                        && o.RCompanyID == rcom
                                                        && o.IsActive == ShowActive
                                                        ).OrderByDescending(o => o.CreatedDate).ToList();

            }
            return result;
        }

        public static bool checkIsDupShortID(string comId, string shortId) {
            bool isdup = false;
            try {
                using (GAEntities db = new GAEntities()) {
                    var dup = db.CompanyInfo.Where(o => o.CompanyID != comId && o.ShortCode == shortId).FirstOrDefault();
                    if (dup != null) {
                        isdup = true;
                    }
                }
            } catch (Exception ex) {
            }
            return isdup;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_ComSet doc, bool isnew) {
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
                        CreateDefaultLocation(doc.ComInfo.RCompanyID, doc.ComInfo.CompanyID, doc.ComInfo.CreatedBy);
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
                        //n.Currency = doc.ComInfo.Currency;
                        n.Tel1 = doc.ComInfo.Tel1;
                        n.Tel2 = doc.ComInfo.Tel2;
                        n.Mobile = doc.ComInfo.Mobile;
                        n.Email = doc.ComInfo.Email;
                        n.Fax = doc.ComInfo.Fax;
                        n.SalePersonID = doc.ComInfo.SalePersonID;
                        n.BankCode = doc.ComInfo.BankCode;
                        n.BookBankNo = doc.ComInfo.BookBankNo;
                        n.BookBankName = doc.ComInfo.BookBankName;
                        n.PromptPay = doc.ComInfo.PromptPay;
                        n.PromptPayAccType = doc.ComInfo.PromptPayAccType;
                        n.QrPaymentData = doc.ComInfo.QrPaymentData;
                        n.PriceTaxCondType = doc.ComInfo.PriceTaxCondType;
                        n.IsVatRegister = doc.ComInfo.IsVatRegister;
                        n.IsWH = doc.ComInfo.IsWH;
                        n.Remark1 = doc.ComInfo.Remark1;
                        n.Remark2 = doc.ComInfo.Remark2;
                        n.ModifiedDate = DateTime.Now;
                        n.ModifiedBy = doc.ComInfo.ModifiedBy;
                        db.SaveChanges();
                        CreateDefaultLocation(doc.ComInfo.RCompanyID,doc.ComInfo.CompanyID,doc.ComInfo.ModifiedBy);
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.ComInfo.CompanyID, TableID = "COMPANY", ParentID = h.CompanyID, TransactionDate = DateTime.Now, CompanyID = "", Action = "UPDATE COMPANY" }, h.RCompanyID, h.CompanyID, h.ModifiedBy);

                    }
                }
                //var r = InitData.NewMiscBeginOfNewCompany(h.RCompanyID, doc.ComInfo.CompanyID);
                //var r3 = InitData.NewDefaultPayment(h.RCompanyID, doc.ComInfo.CompanyID);
                //if (r.Result == "fail") {
                //    result.Result = "fail";
                //    result.Message1 = result.Message1 + " " + r.Message1;
                //}
                //if (r3.Result == "fail") {
                //    result.Result = "fail";
                //    result.Message1 = result.Message1 + " " + r3.Message1;
                //}
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

        #region Table
        public static I_BasicResult AddTable(POS_Table data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var table = db.POS_Table.Where(o => o.TableID == data.TableID && o.RComID == data.RComID && o.ComID == data.ComID).FirstOrDefault();

                    if (table == null) {//add new 
                        db.POS_Table.Add(data);
                        db.SaveChanges();
                    } else {
                        result.Result = "fail";
                        result.Message1 = "มีรหัสโต๊ะนี้ในสาขา " + data.ComID + " นี้แล้ว";
                    }
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

        public static I_BasicResult DeleteTable(POS_Table data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.POS_Table.Remove(db.POS_Table.Where(o => o.RComID == data.RComID && o.TableID == data.TableID && o.ComID == data.ComID).FirstOrDefault());
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

        #region Location
        public static I_BasicResult CreateDefaultLocation(string rcom,string comId,string createby) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {

                    var activeCom = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == comId).FirstOrDefault();
                    // remove consing loc
                    db.LocationInfo.RemoveRange(db.LocationInfo.Where(o => o.RCompany == rcom && o.LocTypeID == "CONSIGN" && o.CompanyID == comId));
                    // remove store loc
                    db.LocationInfo.RemoveRange(db.LocationInfo.Where(o => o.RCompany == rcom && o.LocTypeID == "STORE" && o.CompanyID == comId));

                    if (activeCom.IsWH == false) {
                        // add store  loc
                        var store_loc = NewLocation(rcom,createby);
                        store_loc.RCompany = rcom;
                        store_loc.CompanyID = comId;
                        store_loc.LocID = "STORE";
                        store_loc.LocCode = "STORE";
                        store_loc.LocTypeID = "STORE";
                        store_loc.Name = "STORE";
                        var chk_storeExist = db.LocationInfo.Where(o => o.RCompany == rcom && o.CompanyID == comId && o.LocID == store_loc.LocID).FirstOrDefault();

                        db.LocationInfo.Add(store_loc);
                        db.SaveChanges();

                        // add intransit   loc
                        var intransit_loc = NewLocation(rcom, createby);
                        intransit_loc.RCompany = rcom;
                        intransit_loc.CompanyID = comId;
                        intransit_loc.LocID = "INTRANSIT";
                        intransit_loc.LocCode = "INTRANSIT";
                        intransit_loc.LocTypeID = "STORE";
                        intransit_loc.Name = "INTRANSIT";
                        var chk_instransitExist = db.LocationInfo.Where(o => o.RCompany == rcom && o.CompanyID == comId && o.LocID == intransit_loc.LocID).FirstOrDefault();

                        db.LocationInfo.Add(intransit_loc);
                        db.SaveChanges();

                        // add return   loc
                        var return_loc = NewLocation(rcom, createby);
                        return_loc.RCompany = rcom;
                        return_loc.CompanyID = comId;
                        return_loc.LocID = "RETURN";
                        return_loc.LocCode = "RETURN";
                        return_loc.LocTypeID = "STORE";
                        return_loc.Name = "RETURN";
                        var chk_returnExist = db.LocationInfo.Where(o => o.RCompany == rcom && o.CompanyID == comId && o.LocID == return_loc.LocID).FirstOrDefault();

                        db.LocationInfo.Add(return_loc);
                        db.SaveChanges();

                    } else {
                        // add consign loc
                        var list_store = db.CompanyInfo.Where(o => o.IsActive == true && o.RCompanyID==rcom && o.TypeID== "BRANCH" && o.IsWH == false).OrderBy(o => o.CompanyID).ToList();

                        var ownLoc = NewLocation(rcom, createby);
                        ownLoc.RCompany = rcom;
                        ownLoc.CompanyID = comId;
                        ownLoc.LocID = comId;
                        ownLoc.LocCode = comId;
                        ownLoc.LocTypeID = "STORE";
                        ownLoc.Name = activeCom.Name1 + " " + activeCom.Name2;
                        db.LocationInfo.Add(ownLoc);
                        foreach (var c in list_store) {
                            var consign_loc = NewLocation(rcom, createby);
                            consign_loc.RCompany = rcom;
                            consign_loc.CompanyID = comId;
                            consign_loc.LocID = c.CompanyID;
                            consign_loc.LocCode = c.CompanyID;
                            consign_loc.LocTypeID = "CONSIGN";
                            consign_loc.Name = c.Name1 + " " + c.Name2;
                            db.LocationInfo.Add(consign_loc);
                        }
                        db.SaveChanges();
                    }
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
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
            n.Location = new List<LocationInfo>();
            n.Table = new List<POS_Table>();
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
            //n.Currency = "THB";
            n.Tel1 = "";
            n.Tel2 = "";
            n.Mobile = "";
            n.Email = "";
            n.Fax = "";
            n.SalePersonID = "";
            n.IsWH = false;

            n.BankCode = "";
            n.BookBankNo = "";
            n.BookBankName = "";
            n.PromptPay = "";
            n.PromptPayAccType = "";
            n.QrPaymentData = "";
            n.PriceTaxCondType = "";
            n.IsVatRegister = false;

            n.Remark1 = "";
            n.Remark2 = "";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static LocationInfo NewLocation(string rcom,string createby) {
            LocationInfo n = new LocationInfo();

            n.RCompany = rcom;
            n.CompanyID = "";
            n.LocID = "";
            n.LocCode = "";
            n.LocTypeID = "";
            n.ParentID = "";
            n.Name = "";
            n.Remark = "";
            n.CreatedBy = createby;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;

        }

        public static POS_Table NewTable(string rcom,string com) {
            POS_Table n = new POS_Table();

            n.RComID = rcom;
            n.ComID = com;
            n.TableID = "";
            n.TableName = "";
            n.Sort = 0;
            n.IsActive = true;
            return n;

        }

        #endregion

    }
}
