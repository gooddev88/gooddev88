using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess {
    public static class VendorInfoV2Service {
        #region Class

        public class VendorArea {
            public int ID { get; set; }
            public string AreaID { get; set; }
        }
        public class I_VendorSet {
            public string Action { get; set; }
            public VendorInfo Info { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_FiterSet {

            public String SearchText { get; set; }
            public bool ShowActive { get; set; }
        }

        #endregion


        #region Golbal var
        public static I_VendorSet DocSet { get { return (I_VendorSet)HttpContext.Current.Session["vendor_set"]; } set { HttpContext.Current.Session["vendor_set"] = value; } }
        public static List<vw_VendorInfo> DocList { get { return (List<vw_VendorInfo>)HttpContext.Current.Session["vw_vendor_list"]; } set { HttpContext.Current.Session["vw_vendor_list"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        public static string PreviouseDetailPage { get { return (string)HttpContext.Current.Session["ven_previous_page"]; } set { HttpContext.Current.Session["ven_previous_page"] = value; } }
        public static string PreviouseListPage { get { return (string)HttpContext.Current.Session["ven_list_previous_page"]; } set { HttpContext.Current.Session["ven_list_previous_page"] = value; } }
        public static I_FiterSet MyFilter { get { return (I_FiterSet)HttpContext.Current.Session["venfilter_set"]; } set { HttpContext.Current.Session["venfilter_set"] = value; } }
        public static bool NeedRunNext { get { return HttpContext.Current.Session["needrunnext"] == null ? false : (bool)HttpContext.Current.Session["needrunnext"]; } set { HttpContext.Current.Session["needrunnext"] = value; } }
        #endregion


        #region Method GET

        public static void GetDocSetByID(string docid, bool iscopy) {
            NewTransaction();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using(GAEntities db = new GAEntities()) {
                    DocSet.Info = db.VendorInfo.Where(o => o.VendorID == docid && o.IsActive).FirstOrDefault();
                    if(iscopy) {
                        //ReSetStatusCopyTransaction();
                    } else {
                        DocSet.Log = TransactionInfoService.ListLogByDocID(docid);
                    }
                }
            } catch(Exception ex) {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static VendorInfo GetVendorInfoByVendorId(string venid) {
            VendorInfo result = new VendorInfo();
            using(GAEntities db = new GAEntities()) {
                result = db.VendorInfo.Where(o => o.VendorID == venid && o.IsActive).FirstOrDefault();
            }
            return result;
        }

        public static VendorInfo GetVendorByCitizenID(string citizenId) {
            VendorInfo result = new VendorInfo();
            using(GAEntities db = new GAEntities()) {
                result = db.VendorInfo.Where(o => o.TaxID == citizenId).FirstOrDefault();
            }
            return result;
        }


        public static List<vw_VendorInfo> ListViewVendorByID(string venid) {
            List<vw_VendorInfo> result = new List<vw_VendorInfo>();
            using(GAEntities db = new GAEntities()) {
                result = db.vw_VendorInfo.Where(o => o.VendorID == venid || venid == "" && o.IsActive).ToList();
            }
            return result;
        }

        public static List<vw_VendorInfo> ListViewVendor(string search)
        {
            List<vw_VendorInfo> result = new List<vw_VendorInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_VendorInfo.Where(o => 
                o.VendorID.ToLower().Contains(search)
                || o.NameTh1.ToLower().Contains(search)
                || o.NameTh2.ToLower().Contains(search)
                || o.FullNameTh.ToLower().Contains(search)
                && o.IsActive).ToList();
            }
            return result;
        }

        public static void ListDoc(string search) {
            search = search.Trim().ToLower();
            using(GAEntities db = new GAEntities()) {
                DocList = db.vw_VendorInfo.Where(o =>
                                        (
                                            o.VendorID.ToLower().Contains(search)
                                            || o.CompanyID.ToLower().Contains(search)
                                            || o.NameTh1.ToLower().Contains(search)
                                            || o.NameTh2.ToLower().Contains(search)
                                            || o.TaxID.ToLower().Contains(search)
                                            || o.Mobile.Contains(search)
                                        )
                                        ).OrderBy(o => o.VendorID).ThenByDescending(o => o.CreatedDate).ToList();
            }

        }

        public static List<vw_VendorInfo> ListVendorInCom(string search, string currComId, bool isShowall) {
            search = search.Trim().ToLower();
            List<vw_VendorInfo> result = new List<vw_VendorInfo>();
            using(GAEntities db = new GAEntities()) {
                result = db.vw_VendorInfo.Where(o =>
                                        (
                                            o.VendorID.ToLower().Contains(search)
                                            || o.CompanyID.ToLower().Contains(search)
                                            || o.NameTh1.ToLower().Contains(search)
                                            || o.NameTh2.ToLower().Contains(search)
                                            || o.TaxID.ToLower().Contains(search)
                                            || o.Tel1.Contains(search)
                                            || o.Tel2.Contains(search)
                                        )
                                        //&& o.CompanyID == currComId
                                        && o.IsActive == true
                                        ).OrderBy(o => o.VendorID).ThenByDescending(o => o.CreatedDate).ToList();
            }
            return result;

        }
        public static List<VendorArea> ListProvinceFromVendor(string itemId) {
            List<VendorArea> result = new List<VendorArea>();
            using(GAEntities db = new GAEntities()) {
            //    var vids = db.VendorInItems.Where(o => o.ItemID == itemId).Select(o => o.VendorID).Distinct().ToList();
            //    var query = db.VendorInfo.Where(o => vids.Contains(o.VendorID)).Select(o => o.AddrProvince).Distinct().ToList();
            //    int i = 1;
            //    foreach(var q in query) {
            //        VendorArea n = new VendorArea();
            //        n.ID = i;
            //        n.AreaID = q;
            //        result.Add(n);
            //    }
            }
            return result;
        }

        #endregion Method GET

        #region Save

        public static I_BasicResult Save(VendorInfo info, string activeCompany, bool runNextId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using(GAEntities db = new GAEntities()) {
                    var h = db.VendorInfo.Where(o => o.VendorID == info.VendorID).FirstOrDefault();
                    if(h == null) {
                        db.VendorInfo.Add(info);
                        db.SaveChanges();
                        if(runNextId) {
                            IDRuunerService.GetNewID("VENDOR_INFO", h.CompanyID, true, "th", Convert.ToDateTime(h.CreatedDate));
                        }
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = info.VendorID, TableID = "VENDOR", ParentID = "", TransactionDate = DateTime.Now, CompanyID = activeCompany, Action = "Create new vendor" });
                    } else {

                        h.VendorCode = info.VendorCode;
                        h.TypeID = info.TypeID;
                        h.CompanyID = info.CompanyID;
                        h.ParentID = info.ParentID;
                        h.ShortCode = info.ShortCode;
                        h.RefID = info.RefID;
                        h.TitleTh = info.TitleTh;
                        h.NameTh1 = info.NameTh1;
                        h.NameTh2 = info.NameTh2;
                        h.FullNameTh = info.FullNameTh;
                        h.TitleEn = info.TitleEn;
                        h.NameEn1 = info.NameEn1;
                        h.NameEn2 = info.NameEn2;
                        h.FullNameEn = info.FullNameEn;
                        h.PersonTypeID = info.PersonTypeID;
                        h.GroupID = info.GroupID;
                        h.SubGroupID = info.SubGroupID;
                        h.TaxID = info.TaxID;
                        h.VatTypeID = info.VatTypeID;
                        h.BankCode = info.BankCode;
                        h.BankID = info.BankID;
                        h.PaymentTermID = info.PaymentTermID;
                        h.CreditLimit = info.CreditLimit;
                        h.PaymentTermID = info.PaymentTermID;
                        h.AddrFull = info.AddrFull;
                        h.AddrNo = info.AddrNo;
                        h.AddrMoo = info.AddrMoo;
                        h.AddrTumbon = info.AddrTumbon;
                        h.AddrAmphoe = info.AddrAmphoe;
                        h.AddrProvince = info.AddrProvince;
                        h.AddrPostCode = info.AddrPostCode;
                        h.AddrCountry = info.AddrCountry;
                        h.BillAddr1 = info.BillAddr1;
                        h.BillAddr2 = info.BillAddr2;
                        h.Tel1 = info.Tel1;
                        h.Tel2 = info.Tel2;
                        h.Mobile = info.Mobile;
                        h.Email = info.Email;
                        h.Fax = info.Fax;
                        h.AreaID = info.AreaID;
                        h.Remark1 = info.Remark1;
                        h.Remark2 = info.Remark2;
                        h.Remark3 = info.Remark3;
                        h.Remark4 = info.Remark4;
                        h.IsPartner = info.IsPartner;
                        h.IsActive = info.IsActive;
                        h.ModifiedBy = info.ModifiedBy;
                        h.ModifiedDate = info.ModifiedDate;
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = info.VendorID, TableID = "VENDOR", ParentID = "", TransactionDate = DateTime.Now, CompanyID = activeCompany, Action = "Update vendor" });
                    }
                }
            } catch(Exception ex) {
                result.Result = "fail";
                if(ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        #endregion  

        #region Delete

        public static void Delete() {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using(GAEntities db = new GAEntities()) {
                    var cus = db.VendorInfo.Where(o => o.VendorID == DocSet.Info.VendorID).FirstOrDefault();
                    cus.IsActive = false;

                    db.SaveChanges();
                }
            } catch(Exception ex) {
                DocSet.OutputAction.Result = "fail";
                DocSet.OutputAction.Message1 = ex.Message;
                if(ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " : " + ex.InnerException.Message;
                }
            }
        }
        public static void SetActive(bool active) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using(GAEntities db = new GAEntities()) {
                    var cus = db.VendorInfo.Where(o => o.VendorID == DocSet.Info.VendorID).FirstOrDefault();
                    cus.IsActive = active;

                    db.SaveChanges();
                }
            } catch(Exception ex) {
                DocSet.OutputAction.Result = "fail";
                DocSet.OutputAction.Message1 = ex.Message;
                if(ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " : " + ex.InnerException.Message;
                }
            }
        }

        #endregion  

        #region New data

        public static void NewTransaction() {
            DocSet = new I_VendorSet();
            DocSet.Action = "";
            DocSet.Info = NewHead();
            DocSet.Log = new List<TransactionLog>();
        }
        public static void NewFilter() {
            MyFilter = new I_FiterSet();
            MyFilter.SearchText = "";
            MyFilter.ShowActive = true;
        }
        public static VendorInfo NewHead() {
            VendorInfo n = new VendorInfo();
            n.VendorID = "";
            n.VendorCode = "";
            n.TypeID = "";
            n.ParentID = "";
            n.CompanyID = "";
            n.ShortCode = "";
            n.RefID = "";
            n.TitleTh = "";
            n.NameTh1 = "";
            n.NameTh2 = "";
            n.FullNameTh = "";
            n.TitleEn = "";
            n.NameEn1 = "";
            n.NameEn2 = "";
            n.FullNameEn = "";
            n.PersonTypeID = "";
            n.GroupID = "";
            n.SubGroupID = "";
            n.TaxID = "";
            n.VatTypeID = "";
            n.PaymentTermID = "";
            n.CreditLimit = 0;
            n.AddrFull = "";
            n.AddrNo = "";
            n.AddrMoo = "";
            n.AddrTumbon = "";
            n.AddrAmphoe = "";
            n.AddrProvince = "";
            n.AddrPostCode = "";
            n.AddrCountry = "";
            n.BillAddr1 = "";
            n.BillAddr2 = "";
            n.ContactPerson = "";
            n.Tel1 = "";
            n.Tel2 = "";
            n.Mobile = "";
            n.Email = "";
            n.Fax = "";
            n.AreaID = "";
            n.AccGroup = "";
            n.Currencry = "";
            n.RateBy = "";
            n.BankID = "";
            n.BankCode = "";
            n.Remark1 = "";
            n.Remark2 = "";
            n.Remark3 = "";
            n.Remark4 = "";
            n.Status = "OPEN";
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        #endregion

    }
}