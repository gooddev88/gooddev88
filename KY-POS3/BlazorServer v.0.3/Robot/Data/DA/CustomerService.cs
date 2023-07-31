using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class CustomerService {


        public class I_CusSet {
            public CustomerInfo Info { get; set; }
            public List<XFilesRef> Files { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }


        public   I_CusSet DocSet { get; set; }

        public CustomerService() {
            DocSet = NewTransaction();
        }

        public class SelectOption {
            public string Value { get; set; }
            public string Desc { get; set; }

        }

        public I_CusSet GetDocSet(string docid) {
            I_CusSet n = NewTransaction();
            using (GAEntities db = new GAEntities()) {
                n.Info = db.CustomerInfo.Where(o => o.CustomerID == docid).FirstOrDefault();
            }
            return n;
        }

        public static CustomerInfo GetCustInfo(string custId) {
            CustomerInfo result = new CustomerInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o => o.CustomerID == custId).FirstOrDefault();
            }
            return result;
        }

        public static CustomerInfo GetMemberByMobile(string custId)
        {
            CustomerInfo result = new CustomerInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.CustomerInfo.Where(o => custId.Contains(o.CustomerID)).FirstOrDefault();
            }
            return result;
        }

        public vw_CustomerInfo GettViewCustInfo(string custId) {
            vw_CustomerInfo result = new vw_CustomerInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_CustomerInfo.Where(o => o.CustomerID == custId).FirstOrDefault();
            }
            return result;
        }

        public List<vw_CustomerInfo> ListViewCust() {
            List<vw_CustomerInfo> result = new List<vw_CustomerInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_CustomerInfo.OrderBy(o => o.CustomerID).ToList();
            }
            return result;
        }

        public List<ThaiPostAddress> SearchAddrThailand(string search)
        {
            List<ThaiPostAddress> result = new List<ThaiPostAddress>();
            using (GAEntities db = new GAEntities())
            {
                result = db.ThaiPostAddress.Where(o =>
                                                                (o.PROVINCE_NAME.Contains(search)
                                                           || o.DISTRICT_NAME.Contains(search)
                                                           || o.BORDER_NAME.Contains(search)
                                                           || o.DISTRICT_POSTAL_CODE.Contains(search)
                                                           || search == ""
                                                           )
                                                           )
                    .OrderBy(o => o.PROVINCE_NAME).ThenBy(o => o.BORDER_NAME).ThenBy(o => o.DISTRICT_NAME).Take(15).ToList();
            }
            return result;
        }


        public List<CustomerInfo> ListDocMember(string search, int skip, int take)
        {
            List<CustomerInfo> result = new List<CustomerInfo>();

            using (GAEntities db = new GAEntities())
            {
                result = db.CustomerInfo.Where(o =>
                                                    (o.CustomerID.Contains(search)
                                                        || o.Mobile.Contains(search)
                                                        || o.NameTh1.Contains(search)
                                                        || o.NameTh2.Contains(search)
                                                        || o.FullNameTh.Contains(search)
                                                        || search == ""
                                                    )
                                                    && o.IsActive == true
                                                    )
                                                    .OrderByDescending(o => o.CreatedDate).Take(30).ToList();
            }
            
            return result;
        }


        public I_BasicResult SaveCustomer(CustomerInfo doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) { 
                    var c = db.CustomerInfo.Where(o => o.CustomerID == doc.CustomerID).FirstOrDefault();
                    if (c == null) {
                        if (doc.Mobile == "") {
                            doc.Mobile = doc.CustomerID;
                        }
                        db.CustomerInfo.Add(doc);
                        db.SaveChanges();
                    } else {
                        c.CompanyID = doc.CompanyID;
                        c.NameTh1 = doc.NameTh1;
                        c.NameTh2 = doc.NameTh2;
                        c.TaxID = doc.TaxID;
                        c.BankCode = doc.BankCode;
                        c.SalePersonID = doc.SalePersonID;
                        c.BookIBankD = doc.BookIBankD;
                        c.AddrNo = doc.AddrNo;
                        c.AddrMoo = doc.AddrMoo;
                        c.AddrTumbon = doc.AddrTumbon;
                        c.AddrAmphoe = doc.AddrAmphoe;
                        c.AddrProvince = doc.AddrProvince;
                        c.AddrPostCode = doc.AddrPostCode;
                        c.AddrCountry = doc.AddrCountry;
                        if (doc.Mobile == "") {
                            c.Mobile = doc.CustomerID;
                        } else {
                            c.Mobile = doc.Mobile;
                        }
                        c.ModifiedBy = "ONLINE";
                        c.ModifiedDate = DateTime.Now;
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

        public I_CusSet NewTransaction() {
            I_CusSet n = new I_CusSet();
            n.Info = NewCustomer();
            n.Files = new List<XFilesRef>();
            return n;
        }
        public static CustomerInfo NewCustomer() {
            CustomerInfo n = new CustomerInfo();

            n.CustomerID = "";
            n.RCompanyID = "KY";
            n.CustCode = "";
            n.TypeID = "BRANCH";
            n.ParentID = "";
            n.CompanyID = "";
            n.ShortCode = "";
            n.BrnCode = "";
            n.BrnDesc = "";
            n.RefID = "";
            n.TitleTh = "";
            n.NameTh1 = "";
            n.NameTh2 = "";
            n.FullNameTh = "";
            n.TitleEn = "";
            n.NameEn1 = "";
            n.NameEn2 = "";
            n.FullNameEn = "";
            n.PersonTypeID = "NATURAL PERSION";
            n.GroupID = "ONETIME";
            n.SubGroupID = "";
            n.TaxID = "";
            n.VatTypeID = "VAT0";
            n.PaymentTermID = "CASH";
            n.PaymentGrade = "A";
            n.CreditLimit = 0;
            n.ContactPerson = "";
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
            n.Tel1 = "";
            n.Tel2 = "";
            n.Mobile = "";
            n.Email = "";
            n.Fax = "";
            n.SalePersonID = "";
            n.AreaID = "";
            n.BankCode = "";
            n.BookIBankD = "";
            n.Currency = "";
            n.CreatedBy = "ONLINE";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.Status = "OPEN";
            n.IsSysData = false;
            n.IsActive = true;
            return n;
        }



        public List<SelectOption> ListGender() {
            return new List<SelectOption>() {
                new SelectOption(){ Value= "Male", Desc="ชาย"},
                new SelectOption(){ Value= "Female", Desc="หญิง"}
            };
        }


        #region Company & customer
        public static CustomerInfo ConvertCompany2Customer(CompanyInfo com) {
            CustomerInfo cus = NewCustomer();
            using (GAEntities db = new GAEntities()) {
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

        public static I_BasicResult SaveCustomerByCompany(CustomerInfo info) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var c = db.CustomerInfo.Where(o => o.CustomerID == info.CustomerID && o.CompanyID == info.CompanyID && o.RCompanyID == info.RCompanyID).FirstOrDefault();
                    if (c == null) {
                        db.CustomerInfo.Add(info);
                        db.SaveChanges();
                        TransactionService.SaveLog(new TransactionLog { TransactionID = info.CustomerID, TableID = "CUSTOMER", ParentID = info.CompanyID, TransactionDate = DateTime.Now, CompanyID = "", Action = "Create new Customer" }, info.RCompanyID, info.CompanyID, info.CreatedBy);
                    } else {

                        db.CustomerInfo.RemoveRange(db.CustomerInfo.Where(o => o.CustomerID == info.CustomerID));
                        db.CustomerInfo.Add(info);
                        db.SaveChanges();

                        TransactionService.SaveLog(new TransactionLog { TransactionID = info.CustomerID, TableID = "CUSTOMER", ParentID = info.CompanyID, TransactionDate = DateTime.Now, CompanyID = "", Action = "Update Customer" }, info.RCompanyID, info.CompanyID, info.CreatedBy);
                    }
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

        #endregion

    }
}
