using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Master.DA
{
    public static class CompanyService
    {
        #region Class
        public class CompanyInfoList
        {
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
        public class I_CompanySet
        {
            public string Action { get; set; }
            public CompanyInfo Info { get; set; }
            public List<LocationInfo> Location { get; set; }
            public List<POS_Table> Table { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_FiterSet
        {
            public String SearchText { get; set; }
            public bool ShowActive { get; set; }
        }
        #endregion


        #region Golbal var
        public static I_CompanySet DocSet { get { return (I_CompanySet)HttpContext.Current.Session["company_set"]; } set { HttpContext.Current.Session["company_set"] = value; } }
        public static List<vw_CompanyInfo> DocList { get { return (List<vw_CompanyInfo>)HttpContext.Current.Session["vw_company_list"]; } set { HttpContext.Current.Session["vw_company_list"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        public static string NewDocPreviousPage { get { return (string)HttpContext.Current.Session["comnewdoc_previouspage"]; } set { HttpContext.Current.Session["comnewdoc_previouspage"] = value; } }
        public static string PreviouseDetailPage { get { return (string)HttpContext.Current.Session["com_previous_page"]; } set { HttpContext.Current.Session["com_previous_page"] = value; } }
        public static string PreviouseListPage { get { return (string)HttpContext.Current.Session["com_list_previous_page"]; } set { HttpContext.Current.Session["com_list_previous_page"] = value; } }
        public static I_FiterSet MyFilter { get { return (I_FiterSet)HttpContext.Current.Session["comfilter_set"]; } set { HttpContext.Current.Session["comfilter_set"] = value; } }
        public static bool NeedRunNext { get { return HttpContext.Current.Session["needrunnext"] == null ? false : (bool)HttpContext.Current.Session["needrunnext"]; } set { HttpContext.Current.Session["needrunnext"] = value; } }
        #endregion

        #region Method GET
        public static void GetDocSetByID(string docid, bool iscopy)
        {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            NewTransaction();

            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    DocSet.Info = db.CompanyInfo.Where(o => o.CompanyID == docid && o.RCompanyID == rcom && o.IsActive).FirstOrDefault();
                    DocSet.Location = db.LocationInfo.Where(o => o.CompanyID == DocSet.Info.CompanyID && o.RCompany == rcom && o.IsActive).ToList();
                    DocSet.Table = db.POS_Table.Where(o => o.ComID == DocSet.Info.CompanyID && o.RComID == rcom && o.IsActive).OrderBy(o => o.Sort).ToList();
                    if (iscopy)
                    {
                        //ReSetStatusCopyTransaction();
                    }
                    else
                    {
                        DocSet.Log = TransactionInfoService.ListLogByDocID(docid);
                    }
                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static List<vw_UserInRCom> ListRComByUser(string username)
        {
            List<vw_UserInRCom> uirc = new List<vw_UserInRCom>();

            using (GAEntities db = new GAEntities())
            {
                uirc = db.vw_UserInRCom.Where(o => o.UserName == username).ToList();
            }
            return uirc;
        }
        public static List<CompanyInfoList> ListCompanyInfoUIC(string type, bool addShowAll)
        {
            var comlist = LoginService.LoginInfo.UserInCompany; ;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            List<CompanyInfoList> result = new List<CompanyInfoList>();
            using (GAEntities db = new GAEntities())
            {
                var query = db.vw_CompanyInfo.Where(o =>
                                                    (o.TypeID == type || type == "")
                                                     && comlist.Contains(o.CompanyID)
                                                     && o.RCompanyID == rcom

                                                     && o.IsActive
                                                ).ToList();
                foreach (var q in query)
                {
                    CompanyInfoList n = new CompanyInfoList();
                    n.CompanyID = q.CompanyID;
                    n.Name = q.Name1 + " " + q.Name2 + " (" + q.CompanyID + ")";
                    n.TypeName = q.TypeName;
                    n.TypeID = q.TypeID;
                    n.FullAddr = q.AddrFull;
                    n.TaxID = q.TaxID;
                    n.IsWH = Convert.ToBoolean(q.IsWH);
                    result.Add(n);
                }
                if (addShowAll)
                {
                    CompanyInfoList blank = new CompanyInfoList { ComCode = "", CompanyID = "", Name = "ทุกสาขา", TypeID = "", TaxID = "", FullAddr = "" };
                    result.Insert(0, blank);
                }

            }
            return result;
        }



      



        public static List<CompanyInfo> ListBranch(bool isShowBlank)
        {

            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            List<CompanyInfo> result = new List<CompanyInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.CompanyInfo.Where(o => o.IsActive
                                                        && o.RCompanyID == rcom
                                                        && o.TypeID == "BRANCH"
                                                        ).ToList();
                foreach (var q in result) {
                    q.Name1 = q.Name1 + " " + q.CompanyID;
                }
            }
            if (isShowBlank)
            {
                CompanyInfo blank = new CompanyInfo { ComCode = "", CompanyID = "", Name1 = "ทุกสาขา", TypeID = "", TaxID = "", AddrFull = "" };
                result.Insert(0, blank);
            }
            return result;
        }
        public static List<vw_CompanyInfo> ListCompanyByWH(string rcom)
        {
            List<vw_CompanyInfo> result = new List<vw_CompanyInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_CompanyInfo.Where(o => o.IsWH == true 
                                                        && o.RCompanyID==rcom
                                                          && o.IsActive
                                                            ).OrderBy(o => o.CreatedDate).ToList();
            }
            return result;
        }

        public static CompanyInfo GetCompanyInfo(string comId)
        {
            CompanyInfo result = new CompanyInfo();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                result = db.CompanyInfo.Where(o => o.CompanyID == comId && o.RCompanyID == rcom).FirstOrDefault();
            }
            return result;
        }
        public static CompanyInfo GetCompanyInfo(string rcom,string comId) {
            CompanyInfo result = new CompanyInfo();
  
            using (GAEntities db = new GAEntities()) {
                result = db.CompanyInfo.Where(o => o.CompanyID == comId && o.RCompanyID == rcom).FirstOrDefault();
            }
            return result;
        }
        public static CompanyInfo GetRCompanyInfo(string rcomId) {
            CompanyInfo result = new CompanyInfo();
   
            using (GAEntities db = new GAEntities()) {
                result = db.CompanyInfo.Where(o => o.CompanyID == rcomId && o.TypeID=="COMPANY").FirstOrDefault();
            }

            return result;
        }

        public static List<CompanyInfo> ListCompany(string search, bool isLimitShow)
        {
            List<CompanyInfo> result = new List<CompanyInfo>();
            search = search.Trim().ToLower();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var uic = LoginService.LoginInfo.UserInCompany;
            //var com = LoginService.LoginInfo.CurrentCompany;
            using (GAEntities db = new GAEntities())
            {
                if (isLimitShow)
                {
                    result = db.CompanyInfo.Where(o =>
                                                      (
                                                          o.ComCode.ToLower().Contains(search)
                                                          || o.CompanyID.ToLower().Contains(search)
                                                          || o.Name1.ToLower().Contains(search)
                                                          || o.Name2.ToLower().Contains(search)
                                                          || o.TaxID.ToLower().Contains(search)
                                                          || o.Mobile.Contains(search)
                                                          || o.Tel1.Contains(search)
                                                      )
                                                      && uic.Contains(o.CompanyID)
                                                      && o.RCompanyID == rcom
                                                      //&& o.CompanyID == com
                                                      )
                                                .Take(100)
                                                .OrderBy(o => o.CompanyID).ThenByDescending(o => o.CreatedDate).ToList();
                }
                else
                {
                    result = db.CompanyInfo.Where(o =>
                                  (
                                      o.ComCode.ToLower().Contains(search)
                                      || o.CompanyID.ToLower().Contains(search)
                                      || o.Name1.ToLower().Contains(search)
                                      || o.Name2.ToLower().Contains(search)
                                      || o.TaxID.ToLower().Contains(search)
                                      || o.Mobile.Contains(search)
                                      || o.Tel1.Contains(search)
                                  )
                                      && o.RCompanyID == rcom
                                  //&& o.CompanyID == com
                                  )
                                .OrderBy(o => o.CompanyID).ThenByDescending(o => o.CreatedDate).ToList();
                }

            }
            return result;
        }

        public static void ListDoc(string search)
        {
            search = search.Trim().ToLower();
           var rcom= LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                DocList = db.vw_CompanyInfo.Where(o =>
                                        (
                                               o.BrnCode.ToLower().Contains(search)
                                            || o.CompanyID.ToLower().Contains(search)
                                            || o.Name1.ToLower().Contains(search)
                                            || o.Name2.ToLower().Contains(search)
                                            || o.TaxID.ToLower().Contains(search)
                                            || o.Mobile.Contains(search)
                                        )
                                        && o.RCompanyID==rcom
                                        && o.TypeID == "BRANCH"
                                        ).OrderBy(o => o.CompanyID).ThenByDescending(o => o.CreatedDate).ToList();
            }

        }

        public static List<vw_CompanyInfo> ListCompanyInUserLogIn(string searchtext)
        {
            List<vw_CompanyInfo> result = new List<vw_CompanyInfo>();
            using (GAEntities db = new GAEntities())
            {

                var CominUserlogin = LoginService.LoginInfo.UserInCompany;
                result = db.vw_CompanyInfo.Where(o => (
                                                o.CompanyID.Contains(searchtext)
                                                         || o.Mobile.Contains(searchtext)
                                                         || o.Name1.Contains(searchtext)
                                                         || o.Name2.Contains(searchtext)
                                                         || o.Tel1.Contains(searchtext)
                                                         || o.TaxID.Contains(searchtext)
                                                         || searchtext == "")
                                                          && o.IsActive == true
                                                          && CominUserlogin.Contains(o.CompanyID)
                                                          )
                                                        .OrderBy(o => o.CompanyID).ToList();
            }

            return result;
        }

        public static List<vw_CompanyInfo> ListCompanyMain()
        {
            List<vw_CompanyInfo> result = new List<vw_CompanyInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_CompanyInfo.Where(o => o.TypeID == "COMPANY" && o.IsActive).OrderBy(o => o.CreatedDate).ToList();
            }
            return result;
        }

        #endregion Method GET

        #region Save

        public static I_BasicResult Save(CompanyInfo info, string activeCompany, bool runNextId)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    //var c = db.CompanyInfo.Where(o => o.CompanyID == info.CompanyID && o.RCompanyID==rcom).FirstOrDefault();
                    var c = db.CompanyInfo.Where(o => o.CompanyID == info.CompanyID).FirstOrDefault();
                    if (c == null)
                    {
                        db.CompanyInfo.Add(info);
                        db.SaveChanges();
                        if (runNextId)
                        {
                            IDRuunerService.GetNewID("COMPANY", c.CompanyID, true, "th", DateTime.Now.Date);
                        }
                        CreateDefaultLocation(info.CompanyID);
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = info.CompanyID, TableID = "COMPANY", ParentID = "", TransactionDate = DateTime.Now, CompanyID = activeCompany, Action = "Create new Company" });
                    }
                    else
                    {
                        c.GroupCode = info.GroupCode;
                        c.ComCode = info.ComCode;
                        c.BrnCode = info.BrnCode;
                        c.BankCode = info.BankCode;
                        c.BookBankNo = info.BookBankNo;
                        //c.RCompanyID = info.RCompanyID;
                        c.TypeID = info.TypeID;
                        c.ParentID = info.ParentID;
                        c.ShortCode = info.ShortCode;
                        c.Name1 = info.Name1;
                        c.Name2 = info.Name2;
                        c.TaxID = info.TaxID;
                        c.AddrFull = info.AddrFull;
                        c.AddrNo = info.AddrNo;
                        c.AddrTanon = info.AddrTanon;
                        c.AddrTumbon = info.AddrTumbon;
                        c.AddrAmphoe = info.AddrAmphoe;
                        c.AddrProvince = info.AddrProvince;
                        c.AddrPostCode = info.AddrPostCode;
                        c.AddrCountry = info.AddrCountry;
                        c.BillAddr1 = info.BillAddr1;
                        c.BillAddr2 = info.BillAddr2;
                        c.Tel1 = info.Tel1;
                        c.Tel2 = info.Tel2;
                        c.Mobile = info.Mobile;
                        c.Email = info.Email;
                        c.Fax = info.Fax;
                        c.SalePersonID = info.SalePersonID;
                        c.StockType = info.StockType;
                        c.BankCode = info.BankCode;
                        c.BookBankNo = info.BookBankNo;
                        c.BookBankName = info.BookBankName;
                        c.PromptPay = info.PromptPay;
                        c.PromptPayAccType = info.PromptPayAccType;
                        c.QrPaymentData = info.QrPaymentData;
                        c.Remark1 = info.Remark1;
                        c.Remark2 = info.Remark2;
                        c.IsWH = info.IsWH;
                        c.IsVatRegister = info.IsVatRegister;
                        c.PriceTaxCondType = info.PriceTaxCondType;
                        c.IsActive = info.IsActive;
                        c.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        c.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        CreateDefaultLocation(info.CompanyID);
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = info.CompanyID, TableID = "COMPANY", ParentID = "", TransactionDate = DateTime.Now, CompanyID = activeCompany, Action = "Update Company" });

                    }
                }
            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        public static I_BasicResult SaveCustomer(CustomerInfo info)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var c = db.CustomerInfo.Where(o => o.CustomerID == info.CustomerID).FirstOrDefault();
                    if (c == null)
                    {
                        db.CustomerInfo.Add(info);
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = info.CompanyID, TableID = "CUSTOMER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = info.CompanyID, Action = "Create new Customer" });
                    }
                    else
                    {

                        info.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        info.ModifiedDate = DateTime.Now;

                        db.CustomerInfo.RemoveRange(db.CustomerInfo.Where(o => o.CustomerID == info.CustomerID));
                        db.CustomerInfo.Add(info);

                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = info.CompanyID, TableID = "CUSTOMER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = info.CompanyID, Action = "Update Customer" });
                    }
                }
            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        public static I_BasicResult AddTable(POS_Table data)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = DocSet.Info;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var table = db.POS_Table.Where(o => o.TableID == data.TableID && o.RComID == rcom && o.ComID == data.ComID).FirstOrDefault();

                    if (table == null)
                    {//add new 
                        db.POS_Table.Add(data);
                        db.SaveChanges();
                    }
                    else
                    {
                        result.Result = "fail";
                        result.Message1 = "มีรหัสโต๊ะนี้ในสาขา " + data.ComID + " นี้แล้ว";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public static void checkDupID()
        {
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.Info;
                    var get_id = db.CompanyInfo.Where(o => o.CompanyID == DocSet.Info.CompanyID && o.RCompanyID == rcom).FirstOrDefault();

                    int i = 0;
                    while (get_id != null)
                    {
                        if (i > 1000)
                        {
                            DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GetNewID("COMPANY", h.CompanyID, true, "th", DateTime.Now.Date);//run next id 
                        DocSet.Info.CompanyID = IDRuunerService.GetNewID("COMPANY", h.CompanyID, false, "th", DateTime.Now.Date)[1];
                        get_id = db.CompanyInfo.Where(o => o.CompanyID == DocSet.Info.CompanyID).FirstOrDefault();//check new id exist 
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }

        public static bool checkIsDupShortID(string comId, string shortId)
        {
            bool isdup = false;

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.Info;
                    var dup = db.CompanyInfo.Where(o => o.CompanyID != DocSet.Info.CompanyID && o.ShortCode == shortId).FirstOrDefault();
                    if (dup != null)
                    {
                        isdup = true;
                    }

                }


            }
            catch (Exception ex)
            {
            }
            return isdup;
        }
        #endregion  

        #region Delete

        public static void Delete()
        {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var com = db.CompanyInfo.Where(o => o.CompanyID == DocSet.Info.CompanyID && o.RCompanyID == rcom).FirstOrDefault();
                    com.IsActive = false;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "fail";

                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.Message;
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }
        public static void SetActive(bool active)
        {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var com = db.CompanyInfo.Where(o => o.CompanyID == DocSet.Info.CompanyID).FirstOrDefault();
                    com.IsActive = active;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "fail";
                DocSet.OutputAction.Message1 = ex.Message;
                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " : " + ex.InnerException.Message;
                }
            }
        }

        public static void DeleteTable(string docid,string comid)
        {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.POS_Table.Remove(db.POS_Table.Where(o => o.RComID == rcom && o.TableID == docid && o.ComID == comid).FirstOrDefault());
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                DocSet.OutputAction.Result = "fail";

                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }


        #endregion  

        #region New tranaction

        public static void NewTransaction()
        {
            DocSet = new I_CompanySet();
            DocSet.Action = "";
            DocSet.Info = NewHead();
            DocSet.Location = new List<LocationInfo>();
            DocSet.Table = new List<POS_Table>();
            DocSet.Log = new List<TransactionLog>();
        }
        public static void NewFilter()
        {
            MyFilter = new I_FiterSet();
            MyFilter.SearchText = "";
            MyFilter.ShowActive = true;
        }
        public static CompanyInfo NewHead()
        {
            CompanyInfo n = new CompanyInfo();
            n.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.CompanyID = "";
            n.GroupCode = "";
            n.ComCode = "";
            n.BrnCode = "";
            n.ParentID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.ShortCode = "";
            n.TypeID = "BRANCH";
            n.Name1 = "";
            n.Name2 = "";
            n.TaxID = "";
            n.BillAddr1 = "";
            n.BillAddr2 = "";
            n.AddrFull = "";
            n.AddrFull2 = "";
            n.AddrNo = "";
            n.AddrTanon = "";
            n.AddrTumbon = "";
            n.AddrAmphoe = "";
            n.AddrProvince = "";
            n.AddrPostCode = "";
            n.AddrCountry = "";
            n.Tel1 = "";
            n.Tel2 = "";
            n.Mobile = "";
            n.Email = "";
            n.Fax = "";
            n.BankCode = "";
            n.BookBankNo = "";
            n.BookBankName = "";
            n.PromptPay = "";
            n.PromptPayAccType = "";
            n.StockType = "";
            n.SalePersonID = "";
            n.QrPaymentData = "";
            n.Remark1 = "";
            n.Remark2 = "";
            n.IsWH = false;
            n.IsVatRegister = false;
            n.PriceTaxCondType = "";
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }
        public static LocationInfo NewLocation()
        {
            LocationInfo n = new LocationInfo();

            n.RCompany = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.CompanyID = "";
            n.LocID = "";
            n.LocCode = "";
            n.LocTypeID = "";
            n.ParentID = "";
            n.Name = "";
            n.Remark = "";
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;

        }

        public static POS_Table NewTable()
        {
            POS_Table n = new POS_Table();

            n.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.ComID = "";
            n.TableID = "";
            n.TableName = "";
            n.Sort = 0;
            n.IsActive = true;
            return n;

        }

        #endregion


        #region Location
        public static I_BasicResult CreateDefaultLocation(string comId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var comInfo =  LoginService.LoginInfo.CurrentCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {

                    var activeCom = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == comId).FirstOrDefault();
                    // remove consing loc
                    db.LocationInfo.RemoveRange(db.LocationInfo.Where(o => o.RCompany == rcom && o.LocTypeID == "CONSIGN" && o.CompanyID == comId));
                    // remove store loc
                    db.LocationInfo.RemoveRange(db.LocationInfo.Where(o => o.RCompany == rcom && o.LocTypeID == "STORE" && o.CompanyID == comId));


                    if (activeCom.IsWH == false)
                    {
                        // add store  loc
                        var store_loc = NewLocation();
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
                        var intransit_loc = NewLocation();
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
                        var return_loc = NewLocation();
                        return_loc.RCompany = rcom;
                        return_loc.CompanyID = comId;
                        return_loc.LocID = "RETURN";
                        return_loc.LocCode = "RETURN";
                        return_loc.LocTypeID = "STORE";
                        return_loc.Name = "RETURN";
                        var chk_returnExist = db.LocationInfo.Where(o => o.RCompany == rcom && o.CompanyID == comId && o.LocID == return_loc.LocID).FirstOrDefault();

                        db.LocationInfo.Add(return_loc);
                        db.SaveChanges();

                    }
                    else
                    {

                        // add consign loc
                        var list_store = db.CompanyInfo.Where(o => o.IsActive && o.IsWH == false).OrderBy(o => o.CompanyID).ToList();
                   
                        var ownLoc = NewLocation();
                        ownLoc.RCompany = rcom;
                        ownLoc.CompanyID = comId;
                        ownLoc.LocID = comId;
                        ownLoc.LocCode = comId;
                        ownLoc.LocTypeID = "STORE";
                        ownLoc.Name = activeCom.Name1 + " " + activeCom.Name2;
                        db.LocationInfo.Add(ownLoc);
                        foreach (var c in list_store)
                        {
                            var consign_loc = NewLocation();
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

            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }


        #endregion


        #region Get Image 

        public static string GetRComLogoThumb(string rcomId)
        {
            string result = "";
            using (GAEntities db = new GAEntities())
            {
                var query = db.CompanyRoot.Where(o => o.RComID == rcomId).FirstOrDefault();
                if (query != null)
                {
                    result = query.ImageThumb;
                }
            }
            return result;

        }
        #endregion

    }
}