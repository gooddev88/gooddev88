using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class CustomerInfoService {
        #region Class
        public class SelectListCustomer {
            public string CustomerID { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string TypeName { get; set; }
            public string GroupName { get; set; }
            public string SubGroupName { get; set; }
            public string Phone { get; set; }
            public string TaxID { get; set; }
            public string FullAddr { get; set; }

        }
        #endregion


        #region  Method GET
        public static List<SelectListCustomer> MiniSelectList(string type, string group) {
            var comlist = LoginService.LoginInfo.UserInCompany;
            List<SelectListCustomer> result = new List<SelectListCustomer>();
            using (GAEntities db = new GAEntities()) {
                var query = db.vw_CustomerInfo.Where(o => (o.GroupID == group || group == "")
                                                     && (o.TypeID == type || type == "")
                                                     && comlist.Contains(o.CompanyID)
                                                     && o.IsActive
                                                ).ToList();
                foreach (var q in query) {
                    SelectListCustomer n = new SelectListCustomer();
                    n.CustomerID = q.CustomerID;
                    n.Name = q.FullNameTh = " (" + q.CustomerID + ")";
                    n.GroupName = q.GroupName;
                    n.TypeName = q.TypeName;
                    result.Add(n);
                }
            }
            return result;
        }

        public static CustomerInfo GetDataByID(string CustID) {
            CustomerInfo result = new CustomerInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o => o.CustomerID == CustID).FirstOrDefault();
            }
            return result;
        }
        public static vw_CustomerInfo GetViewDataByID(string CustID) {
            vw_CustomerInfo result = new vw_CustomerInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_CustomerInfo.Where(o => o.CustomerID == CustID).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_CustomerInfo> ListViewByID()
        {
            List<vw_CustomerInfo> result = new List<vw_CustomerInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_CustomerInfo.OrderBy(o => o.CustomerID).ToList();
            }
            return result;
        }

        public static List<CustomerInfo> ListSearch(string search) {
            var comlist = LoginService.LoginInfo.UserInCompany;
            List<CustomerInfo> result = new List<CustomerInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o => (o.CustomerID.Contains(search)
                                                 || o.FullNameTh.Contains(search)
                                                 || o.GroupID.Contains(search)
                                                 || o.TypeID.Contains(search)
                                                 || search == "")
                                                 && comlist.Contains(o.CompanyID)).ToList();
            }
            return result;
        }
        public static List<vw_CustomerInfo> ListViewSearch(string search, bool showInActive) {

            List<vw_CustomerInfo> result = new List<vw_CustomerInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_CustomerInfo.Where(o => (o.CustomerID.Contains(search)
                                            || o.FullNameTh.Contains(search)
                                            || o.GroupID.Contains(search)
                                            || o.GroupName.Contains(search)
                                            || o.TypeID.Contains(search)
                                            )
                                            && o.IsSysData == false
                                            && (o.IsActive || showInActive)

                                            ).ToList();
            }
            return result;
        }


        public static List<vw_CustomerInfo> ListViewHeadSearch(string search) {


            List<vw_CustomerInfo> result = new List<vw_CustomerInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_CustomerInfo.Where(o => (o.CustomerID.ToLower().Contains(search)
                                            || o.NameDisplay.ToLower().Contains(search)
                                            || o.NameTh1.ToLower().Contains(search)
                                            || o.NameTh2.ToLower().Contains(search)
                                            || o.Mobile.Contains(search)
                                            )
                                           ).OrderBy(o => o.NameTh1).ToList();
            }
            return result;
        }

        public static List<CustomerInfo> ListCust(string search, bool isLimitShow)
        {
            List<CustomerInfo> result = new List<CustomerInfo>();
            search = search.Trim().ToLower();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var uic = LoginService.LoginInfo.UserInCompany;
            //var com = LoginService.LoginInfo.CurrentCompany;
            using (GAEntities db = new GAEntities())
            {
                if (isLimitShow)
                {
                    result = db.CustomerInfo.Where(o =>
                                                      (
                                                          o.CustomerID.ToLower().Contains(search)
                                                          || o.CompanyID.ToLower().Contains(search)
                                                          || o.NameTh1.ToLower().Contains(search)
                                                          || o.NameTh2.ToLower().Contains(search)
                                                          || o.TaxID.ToLower().Contains(search)
                                                          || o.Mobile.Contains(search)
                                                          || o.Tel1.Contains(search)
                                                      )
                                                      && uic.Contains(o.CompanyID)
                                                      && o.RCompanyID == rcom
                                                      //&& o.CompanyID == com
                                                      )
                                                .Take(100)
                                                .OrderBy(o => o.CustomerID).ThenByDescending(o => o.CreatedDate).ToList();
                }
                else
                {
                    result = db.CustomerInfo.Where(o =>
                                  (
                                      o.CustomerID.ToLower().Contains(search)
                                      || o.CompanyID.ToLower().Contains(search)
                                      || o.NameTh1.ToLower().Contains(search)
                                      || o.NameTh2.ToLower().Contains(search)
                                      || o.TaxID.ToLower().Contains(search)
                                      || o.Mobile.Contains(search)
                                      || o.Tel1.Contains(search)
                                  )
                                      && o.RCompanyID == rcom
                                      //&& o.CompanyID == com
                                  )
                                .OrderBy(o => o.CustomerID).ThenByDescending(o => o.CreatedDate).ToList();
                }

            }
            return result;
        }

        public static List<CustomerInfo> ListCustomer() {

            List<CustomerInfo> result = new List<CustomerInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o => o.IsActive).ToList();
            }
            return result;
        }

        #endregion

        #region  Save
        public static List<string> Save(CustomerInfo cus, string action) {
            //string comlist = HttpContext.Current.Session["userincompany"].ToString();
            List<string> result = new List<string> { "", "" };//RESULT ACTION R0=ERROR:R1=SUCCESS 

            try {
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {
                        cus.Status = "OPEN";
                        cus.CreatedBy = LoginService.LoginInfo.CurrentUser;
                        cus.CreatedDate = DateTime.Now;
                        db.CustomerInfo.Add(cus);
                        db.SaveChanges();
                        
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = cus.CustomerID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = cus.CompanyID, Action = "INSERT DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }

                    if (action == "update") {
                        var c = db.CustomerInfo.Where(o => o.CustomerID == cus.CustomerID).FirstOrDefault();
                        c.CustCode = cus.CustCode;
                        c.TypeID = cus.TypeID;
                        c.CompanyID = cus.CompanyID;
                        c.ParentID = cus.ParentID;
                        c.ShortCode = cus.ShortCode;
                        c.BrnCode = cus.BrnCode;
                        c.BrnDesc = cus.BrnDesc;
                        c.RefID = cus.RefID;
                        c.TitleTh = cus.TitleTh;
                        c.NameTh1 = cus.NameTh1;
                        c.NameTh2 = cus.NameTh2;
                        c.TitleEn = cus.TitleEn;
                        c.NameEn1 = cus.NameEn1;
                        c.NameEn2 = cus.NameEn2;

                        c.PersonTypeID = cus.PersonTypeID;
                        c.GroupID = cus.GroupID;
                        c.SubGroupID = cus.SubGroupID;
                        c.TaxID = cus.TaxID;
                        c.VatTypeID = cus.VatTypeID;
                        c.PaymentTermID = cus.PaymentTermID;
                        c.CreditLimit = cus.CreditLimit;
                        c.PaymentTermID = cus.PaymentTermID;
                        c.PaymentGrade = cus.PaymentGrade;
                        c.ContactPerson = cus.ContactPerson;
                        c.AddrFull = cus.AddrFull;
                        c.AddrNo = cus.AddrNo;
                        c.AddrMoo = cus.AddrMoo;
                        c.AddrTumbon = cus.AddrTumbon;
                        c.AddrAmphoe = cus.AddrAmphoe;
                        c.AddrProvince = cus.AddrProvince;
                        c.AddrPostCode = cus.AddrPostCode;
                        c.AddrCountry = cus.AddrCountry;
                        c.BillAddr1 = cus.BillAddr1;
                        c.BillAddr2 = cus.BillAddr2;
                        c.Tel1 = cus.Tel1;
                        c.Tel2 = cus.Tel2;
                        c.Mobile = cus.Mobile;
                        c.Email = cus.Email;
                        c.Fax = cus.Fax;
                        c.BankCode = cus.BankCode;
                        c.BookIBankD = cus.BookIBankD;
                        c.AreaID = cus.AreaID;
                        //c.Remark1 = cus.Remark1;
                        //c.Remark2 = cus.Remark2;
                        //c.Remark3 = cus.Remark3;
                        //c.Remark4 = cus.Remark4;
                        c.ModifiedBy = LoginService.LoginInfo.CurrentUser; 
                        c.ModifiedDate = DateTime.Now;
                      
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = cus.CustomerID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = cus.CompanyID, Action = "UPDATE DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }

                }

                return result;
            } catch (Exception ex) {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;

        }

        public static List<string> SaveCus(CustomerInfo cus, string action)
        {
            //string comlist = HttpContext.Current.Session["userincompany"].ToString();
            List<string> result = new List<string>();

            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    if (action == "insert")
                    {
                        cus.CreatedBy = LoginService.LoginInfo.CurrentUser;
                        cus.CreatedDate = DateTime.Now;
                        db.CustomerInfo.Add(cus);
                        db.SaveChanges();
                        //CustomerAddrInfoService.CreateDefualtAddr(cus.CustomerID);
                        //TransactionInfoService.SaveLog(new TransactionLog { TransactionID = cus.CustomerID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = cus.CompanyID, Action = "INSERT DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }

                    if (action == "update")
                    {
                        var c = db.CustomerInfo.Where(o => o.CustomerID == cus.CustomerID).FirstOrDefault();
                        c.CompanyID = cus.CompanyID;
                        c.NameTh1 = cus.NameTh1;
                        c.NameTh2 = cus.NameTh2;
                        c.TaxID = cus.TaxID;
                        c.AddrNo = cus.AddrNo;
                        c.AddrMoo = cus.AddrMoo;
                        c.AddrTumbon = cus.AddrTumbon;
                        c.AddrAmphoe = cus.AddrAmphoe;
                        c.AddrProvince = cus.AddrProvince;
                        c.AddrPostCode = cus.AddrPostCode;
                        c.AddrCountry = cus.AddrCountry;
                        c.Tel1 = cus.Tel1;
                        c.Mobile = cus.Mobile;
                        c.Email = cus.Email;
                        c.Birthdate = cus.Birthdate;
                        c.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        c.ModifiedDate = DateTime.Now;
                        //CustomerAddrInfoService.CreateDefualtAddr(c.CustomerID);
                        db.SaveChanges();
                        //TransactionInfoService.SaveLog(new TransactionLog { TransactionID = cus.CustomerID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = cus.CompanyID, Action = "UPDATE DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }

                }

                return result;
            }
            catch (Exception ex)
            {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;

        }



        #endregion

        #region Delete
        public static List<string> Delete(string cus_id) {
            List<string> result = new List<string>();
            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");
            try {
                using (GAEntities db = new GAEntities()) {
                    var cus = db.CustomerInfo.Where(o => o.CustomerID == cus_id).FirstOrDefault();

                    //cus.Status = "IN-ACTIVE";
                    cus.IsActive = false;

                    db.SaveChanges();
                    //CustomerAddrInfoService.DeleteByCustID(cus.CustomerID);
                    //TransactionInfoService.SaveLog(new TransactionLog { TransactionID = cus.CustomerID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = cus.CompanyID, Action = "DELETE DATA" });

                    result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = "";


                }
                return result;
            } catch (Exception ex) {

                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;
        }
        #endregion

        #region New data
        public static CustomerInfo NewCusHead()
        {
            CustomerInfo newdata = new CustomerInfo();

            newdata.CustomerID = "";
            newdata.CompanyID = "";
            newdata.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            newdata.CustCode = "";
            newdata.TypeID = "";
            newdata.ParentID = "";
            newdata.ShortCode = "";
            newdata.BrnCode = "";
            newdata.BrnDesc = "";
            newdata.RefID = "";
            newdata.TitleTh = "";
            newdata.NameTh1 = "";
            newdata.NameTh2 = "";
            newdata.FullNameTh = "";
            newdata.TitleEn = "";
            newdata.NameEn1 = "";
            newdata.NameEn2 = "";
            newdata.FullNameEn = "";
            newdata.PersonTypeID = "";
            newdata.GroupID = "";
            newdata.SubGroupID = "";
            newdata.ProductGroupID = "";
            newdata.TaxID = "";
            newdata.VatTypeID = "";
            newdata.PaymentTermID = "";
            newdata.InteralTermID = "";
            newdata.PaymentGrade = "";
            newdata.CreditLimit = 0;
            newdata.ContactPerson = "";
            newdata.AddrFull = "";
            newdata.AddrNo = "";
            newdata.AddrMoo = "";
            newdata.AddrTumbon = "";
            newdata.AddrAmphoe = "";
            newdata.AddrProvince = "";
            newdata.AddrPostCode = "";
            newdata.AddrCountry = "";
            newdata.BillAddr1 = "";
            newdata.BillAddr2 = "";
            newdata.BillTime = "";
            newdata.BillMemo = "";
            newdata.BIllPrintRemark = "";
            newdata.BillContact = "";
            newdata.BillInShip = true;
            newdata.SOMemo = "";
            newdata.ChqAddr1 = "";
            newdata.ChqAddr2 = "";
            newdata.ChqTime = "";
            newdata.ChqPrintRemark = "";
            newdata.ChqMemo = "";
            newdata.ShipPrintRemark = "";
            newdata.InvPrintRemark = "";
            newdata.InvSignPrintRemark = "";
            newdata.PaymentMethod = "";
            newdata.LimitOverDue = 0;
            newdata.IsUseQO = true;
            newdata.Tel1 = "";
            newdata.Tel2 = "";
            newdata.Mobile = "";
            newdata.Email = "";
            newdata.Fax = "";
            newdata.SalePersonID = "";
            newdata.AreaID = "";
            newdata.BankCode = "";
            newdata.BookIBankD = "";
            newdata.Currency = "";
            newdata.CrmPoint = 0;
            newdata.Status = "OPEN";
            newdata.IsSysData = null;
            newdata.IsHold = false;
            newdata.CreatedBy = LoginService.LoginInfo.CurrentUser;
            newdata.CreatedDate = DateTime.Now;
            newdata.ModifiedBy = "";
            newdata.ModifiedDate = null;
            newdata.IsActive = true;
            return newdata;
        }
        #endregion


        public static CustomerInfo ConvertCompany2Customer(CompanyInfo com)
        {
            CustomerInfo cus = NewCusHead();
            using (GAEntities db = new GAEntities())
            {
                cus.CustomerID = com.CompanyID;
                cus.CompanyID = com.CompanyID;
                cus.RCompanyID = com.RCompanyID;
                cus.NameTh1 = com.Name1;
                cus.NameTh2 = com.Name2;
                cus.ShortCode = com.ShortCode;
                cus.TaxID = com.TaxID;
                cus.Mobile = com.Mobile;
                cus.Email = com.Email;
                cus.AddrNo = com.AddrNo;
                cus.AddrMoo = com.AddrTanon;
                cus.AddrTumbon = com.AddrTumbon;
                cus.AddrAmphoe = com.AddrAmphoe;
                cus.AddrProvince = com.AddrProvince;
                cus.AddrPostCode = com.AddrPostCode;
                cus.BillAddr1 = com.BillAddr1;
                cus.BillAddr2 = com.BillAddr2;
                cus.Status = "OPEN";
            }

            return cus;

        }


    }
}