using Microsoft.EntityFrameworkCore;
using Robot.Data.DA.Login;
using Robot.Data.GADB.TT;
using Robot.Data.ML;
using Robot.Data.ML.DBD;
using Robot.Helper.AddrHep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.Master {
    public class CustomerService {

        public static string sessionActiveId = "activecustid";
        public class I_CusSet {
            public CustomerInfo Info { get; set; }
            public List<XFilesRef> Files { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class I_FiterSet {

            public String SearchText { get; set; }
            public bool ShowIsActive { get; set; }

        }

        public string PreviousPageUrl = "";
        public string PreviousSubPageUrl = "";

        public I_CusSet DocSet { get; set; }

        public CustomerService() {
            //DocSet = NewTransaction("","","");
        }

  

        public I_CusSet GetDocSet(string docid, string rcom,string comid) {
            I_CusSet n = NewTransaction(rcom, comid);
            using (GAEntities db = new GAEntities()) {
                n.Info = db.CustomerInfo.Where(o => o.CustomerID == docid && o.RCompanyID == rcom && o.CompanyID == comid).FirstOrDefault();
            }
            return n;
        }

        public static CustomerInfo GetCustInfo(string custId,string rcom,string com) {
            CustomerInfo result = new CustomerInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.CustomerInfo.Where(o => o.CustomerID == custId && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
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

        public static IEnumerable<vw_CustomerInfo> ListViewCustomer(I_FiterSet f, bool isLimitShow, LogInService.LoginSet login)
        {

            IEnumerable<vw_CustomerInfo> result;
            var rcom = login.CurrentRootCompany.CompanyID;
            var uic = login.UserInCompany;
            using (GAEntities db = new GAEntities())
            {
                if (isLimitShow)
                {
                    result = db.vw_CustomerInfo.Where(o =>
                                        (
                                            o.CustomerID.ToLower().Contains(f.SearchText)
                                            || o.CompanyID.ToLower().Contains(f.SearchText)
                                            || o.FullNameTh.ToLower().Contains(f.SearchText) 
                                            || o.TaxID.ToLower().Contains(f.SearchText)
                                            || o.Mobile.Contains(f.SearchText)
                                            || o.Tel1.Contains(f.SearchText)
                                        )
                                        && uic.Contains(o.CompanyID)
                                        && o.CompanyID == login.CurrentCompany.CompanyID
                                        && o.RCompany == rcom
                                        && (o.IsActive != f.ShowIsActive)
                                        ) 
                        .OrderByDescending(o => o.CreatedDate).AsNoTrackingWithIdentityResolution().ToArray(); ;
                }
                else
                {
                    result = db.vw_CustomerInfo.Where(o =>
                                 (
                                     o.CustomerID.ToLower().Contains(f.SearchText)
                                     || o.CompanyID.ToLower().Contains(f.SearchText)
                                     || o.FullNameTh.ToLower().Contains(f.SearchText) 
                                     || o.TaxID.ToLower().Contains(f.SearchText)
                                     || o.Mobile.Contains(f.SearchText)
                                     || o.Tel1.Contains(f.SearchText)
                                 )
                                 && uic.Contains(o.CompanyID)
                                 && o.CompanyID == login.CurrentCompany.CompanyID
                                 && o.RCompany == rcom
                                 && (o.IsActive != f.ShowIsActive)
                                 )
                 .OrderByDescending(o => o.CreatedDate).AsNoTrackingWithIdentityResolution().ToArray(); 
                }
                return result;
            }
        }

        public static List<CustomerInfo> ListSelectCustomer(string search,string rcom,string com)
        {
            List<CustomerInfo> result = new List<CustomerInfo>();
            search = search.Trim().ToLower();
            using (GAEntities db = new GAEntities())
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
                                      && o.CompanyID == com
                                      && o.IsActive == true
                                  )
                                .OrderBy(o => o.CustomerID).ThenByDescending(o => o.CreatedDate).ToList();
            }
            return result;
        }

     
        public static IEnumerable<CustomerInfo> ListCustomer(string rcom,string com)
        {
            IEnumerable<CustomerInfo> result = new List<CustomerInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.CustomerInfo.Where(o=>o.RCompanyID==rcom 
                                                   && o.CompanyID==com 
                                                        ).ToList();
            }
            return result;
        }

        public static I_BasicResult Save(CustomerInfo doc,LogInService.LoginSet login) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) { 
                    var c = db.CustomerInfo.Where(o => o.CustomerID == doc.CustomerID 
                            && o.RCompanyID == login.CurrentRootCompany.CompanyID 
                            && o.CompanyID == login.CurrentCompany.CompanyID).FirstOrDefault();
                    if (c == null) {
                        //if (doc.Mobile == "") {
                        //    doc.Mobile = doc.CustomerID;
                        //}
                        db.CustomerInfo.Add(doc);
                        db.SaveChanges();
                    } else {
                        c.RCompanyID = doc.RCompanyID;
                        c.CompanyID = doc.CompanyID;
                        c.CustomerID = doc.CustomerID;
                        c.CustCode = doc.CustCode;
                        c.OrgType = doc.OrgType;
                        c.TypeID = doc.TypeID;
                        c.ParentID = doc.ParentID;
                        c.Birthdate = doc.Birthdate;
                        c.ShortCode = doc.ShortCode;
                        c.BrnCode = doc.BrnCode;
                        c.BrnDesc = doc.BrnDesc;
                        c.RefID = doc.RefID;
                        c.TitleTh = doc.TitleTh;
                        c.NameTh1 = doc.NameTh1;
                        c.NameTh2 = doc.NameTh2;
                        c.NameTh3 = doc.NameTh3;
                        c.FullNameTh = doc.FullNameTh;
                        c.TitleEn = doc.TitleEn;
                        c.NameEn1 = doc.NameEn1;
                        c.NameEn2 = doc.NameEn2;
                        c.NameEn3 = doc.NameEn3;
                        c.FullNameEn = doc.FullNameEn;
                        c.PersonTypeID = doc.PersonTypeID;
                        c.GroupID = doc.GroupID;
                        c.SubGroupID = doc.SubGroupID;
                        c.ProductGroupID = doc.ProductGroupID;
                        c.TaxID = doc.TaxID;
                        c.Currency = doc.Currency;
                        c.CardExpireDate = doc.CardExpireDate;
                        c.CardIssue = doc.CardIssue;
                        c.TaxTypeID = doc.TaxTypeID;
                        c.PaymentTermID = doc.PaymentTermID;
                        c.PaymentGrade = doc.PaymentGrade;
                        c.CreditLimit = doc.CreditLimit;
                        c.ContactPerson = doc.ContactPerson;
                        c.AddrFull = doc.AddrFull;
                        c.AddrNo = doc.AddrNo;
                        c.AddrMoo = doc.AddrMoo;
                        c.Building = doc.Building;
                        c.RoomNo = doc.RoomNo;
                        c.FloorNo = doc.FloorNo;
                        c.Village = doc.Village;
                        c.Soi = doc.Soi;
                        c.Yaek = doc.Yaek;
                        c.Road = doc.Road;
                        c.AddrTumbon = doc.AddrTumbon;
                        c.AddrAmphoe = doc.AddrAmphoe;
                        c.AddrProvince = doc.AddrProvince;
                        c.AddrPostCode = doc.AddrPostCode;
                        c.AddrCountry = doc.AddrCountry;
                        c.BillAddr1 = doc.BillAddr1;
                        c.BillAddr2 = doc.BillAddr2;
                        c.BillMemo = doc.BillMemo;
                        c.BillContact = doc.BillContact;
                        c.PaymentMethod = doc.PaymentMethod;
                        c.Tel1 = doc.Tel1;
                        c.Tel2 = doc.Tel2;
                        c.Mobile = doc.Mobile;
                        c.Email = doc.Email;
                        c.Fax = doc.Fax;
                        c.LineID = doc.LineID;
                        c.SalePersonID = doc.SalePersonID;
                        c.AreaID = doc.AreaID;
                        c.BankCode = doc.BankCode;
                        c.BookIBankID = doc.BookIBankID;
                        c.Occupation = doc.Occupation;
                        c.Gender = doc.Gender;
                        c.Marriage = doc.Marriage;
                        c.Lang = doc.Lang;
                        c.Race = doc.Race;
                        c.Nationality = doc.Nationality;
                        c.Status = doc.Status;
                        c.IsHold = doc.IsHold;
                        c.Photo = doc.Photo;
                        c.Geolocation = doc.Geolocation;
                        c.Point = doc.Point;
                        c.IsLockPrice = doc.IsLockPrice;
                        c.AccID_Debtor = doc.AccID_Debtor;
                        c.AccID_Creditor = doc.AccID_Creditor;
                        c.AccID_Wht = doc.AccID_Wht;
                        c.Acc_Side = doc.Acc_Side;
                        c.ModifiedBy = login.CurrentUser;
                        c.ModifiedDate = DateTime.Now;
                        c.IsActive = doc.IsActive;
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

        public static I_CusSet NewTransaction(string rcom,string com) {
            I_CusSet n = new I_CusSet();
            n.Info = NewCustomer(rcom, com);
            n.Files = new List<XFilesRef>();
            return n;
        }
        public static CustomerInfo NewCustomer(string rcom, string com ) {
            CustomerInfo n = new CustomerInfo();

            n.CustomerID = "";
            n.RCompanyID = rcom;
            n.CustCode = "";
            n.OrgType = "BRANCH";
            n.TypeID = "CUSTOMER";
            n.ParentID = "";
            n.CompanyID = com;
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
            n.GroupID = "";
            n.SubGroupID = "";
            n.TaxID = "";
            n.TaxTypeID = "";
            n.PaymentTermID = "CASH";
            n.PaymentGrade = "A";
            n.CreditLimit = 0;
            n.ContactPerson = "";
            n.AddrFull = "";
            n.Currency = "THB";
            n.Building = "";
            n.RoomNo = "";
            n.FloorNo = "";
            n.Village = "";
            n.Soi = "";
            n.Yaek = "";
            n.Road = "";
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
            n.BillContact = "";
            n.AreaID = "";
            n.BankCode = "";
            n.BillMemo = "";
            n.BookIBankID = "";
            n.Lang = "";
            n.ProductGroupID = "";
            n.Gender = "";
            n.Occupation = "";
            n.CardIssue = "";
            n.IsLockPrice = true;
            n.AccID_Debtor = "";
            n.AccID_Creditor = "";
            n.AccID_Wht = "";
            n.Acc_Side = "";
            n.LineID = "";
            n.Race = "";
            n.Marriage = "";
            n.NameTh3 = "";
            n.NameEn3 = "";
            n.Geolocation = "";
            n.Point = 0;
            n.Nationality = "";
            n.PaymentMethod = "";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.Status = "OPEN";
            n.IsActive = true;
            return n;
        }


        public static I_FiterSet NewFilterSet() {
            I_FiterSet n = new I_FiterSet();
            n.SearchText = "";
            n.ShowIsActive = false;

            return n;
        }

        public List<SelectOption> ListGender() {
            return new List<SelectOption>() {
                new SelectOption(){ Value= "Male", Description="ชาย",Sort=1},
                new SelectOption(){ Value= "Female", Description="หญิง",Sort=2}
            };
        }

        public static List<SelectOption> ListDoctype()
        {
            return new List<SelectOption>() {
                new SelectOption(){ IsSelect = true ,Value= "บุคคลธรรมดา", Description="บุคคลธรรมดา" ,Sort = 1},
                new SelectOption(){ IsSelect = true ,Value = "บริษัทจํากัด", Description="บริษัทจํากัด", Sort = 2},
                new SelectOption(){ IsSelect = true ,Value = "บริษัทมหาชนจำกัด", Description="บริษัทมหาชนจำกัด", Sort = 3},
                new SelectOption(){ IsSelect = true ,Value = "ห้างหุ้นส่วนจำกัด", Description="ห้างหุ้นส่วนจำกัด", Sort = 4},
            };
        }



        public static List<SelectOption> ListCus_VenType() {
            return new List<SelectOption>() {
                new SelectOption(){ IsSelect = true ,Value= "CUSTOMER", Description="ลูกค้า" ,Sort = 1},
                new SelectOption(){ IsSelect = true ,Value = "VENDOR", Description="คู่ค้า", Sort = 2},
                
            };
        }
    
    

      
       async public static Task<I_CusSet>   GetDBDApiJuristicV1( I_CusSet doc) {
            JuristicData.JuristicInfo result = new JuristicData.JuristicInfo();
            try {
                doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    string dataEndpointUri = $"https://dataapi.moc.go.th/juristic?juristic_id={doc.Info.TaxID}";
                    HttpClient httpClient = new HttpClient();
                    var response = await httpClient.GetAsync(dataEndpointUri);
                    var jsonStr = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode.ToString() == "OK") {
                        result = JsonSerializer.Deserialize<JuristicData.JuristicInfo>(jsonStr);
                    if (result==null) {
                        doc.OutputAction.Result = "fail";
                        doc.OutputAction.Message1 = $"ไม่พบเลขผู้เสียภาษี {doc.Info.TaxID} ในระบบ DBD";
                        return doc;
                    }
                    doc.Info.OrgType = result.juristicType;
                    doc.Info.NameTh1 = result.juristicNameTH;
                    doc.Info.NameEn1 = result.juristicNameEN;
                    if (result.addressDetail!=null) {
                        doc.Info.BrnDesc = result.addressDetail.addressName==null?"": result.addressDetail.addressName;
                        doc.Info.Building = result.addressDetail.buildingName == null ? "" : result.addressDetail.buildingName.ToString();
                        doc.Info.RoomNo = result.addressDetail.roomNo == null ? "" : result.addressDetail.roomNo.ToString();
                        doc.Info.FloorNo = result.addressDetail.floor == null ? "" : result.addressDetail.floor.ToString();
                        doc.Info.Village = result.addressDetail.villageName == null ? "" : result.addressDetail.villageName.ToString();
                        doc.Info.AddrNo = result.addressDetail.houseNumber == null ? "" : result.addressDetail.houseNumber.ToString();
                        doc.Info.AddrMoo = result.addressDetail.moo == null ? "" : result.addressDetail.moo.ToString();
                        doc.Info.Soi = result.addressDetail.soi == null ? "" : result.addressDetail.soi.ToString();
                        doc.Info.Road = result.addressDetail.street == null ? "" : result.addressDetail.street.ToString();
                        doc.Info.AddrTumbon = result.addressDetail.subDistrict == null ? "" : result.addressDetail.subDistrict.ToString();
                        doc.Info.AddrAmphoe = result.addressDetail.district == null ? "" : result.addressDetail.district.ToString();
                        doc.Info.AddrProvince = result.addressDetail.province == null ? "" : result.addressDetail.province.ToString();
                    } else {
                        doc.Info.BrnDesc = "";
                        doc.Info.Building = "";
                        doc.Info.RoomNo = "";
                        doc.Info.FloorNo = "";
                        doc.Info.Village = "";
                        doc.Info.AddrNo = "";
                        doc.Info.AddrMoo = "";
                        doc.Info.Soi = "";
                        doc.Info.Road = "";
                        doc.Info.AddrTumbon = "";
                        doc.Info.AddrAmphoe = "";
                        doc.Info.AddrProvince = "";
                    } 
                } else {

                }
                
            } catch (Exception ex) {
                doc.OutputAction.Result = "fail";
                if (ex.InnerException!=null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            } 
                return doc;
            
        }


        public static I_CusSet CreateFullBillAddr(I_CusSet doc) {
            var i = doc.Info;
       i.BillAddr1=     AddrHelper.CreateFullAddr(i.AddrNo, i.Village, i.Building, i.RoomNo, i.FloorNo, i.Soi, i.Yaek, i.Road, i.AddrMoo, i.AddrTumbon, i.AddrAmphoe, i.AddrProvince, i.AddrPostCode);
            return doc;
        }
    }
}
