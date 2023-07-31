using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace PrintMaster.PrintFile.Accy.FrontEnvelop {
    public class RunReportFrontEnvelop
    {

        public static Envelop OpenReport(PrintData print_row,string typeprint) {
            var doc = JsonConvert.DeserializeObject<FrontEnvelopSet>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            Envelop report0 = new Envelop(doc, "", typeprint, "0");
            report0.DisplayName = doc.head.CustomerID;
            report0.CreateDocument();
          

            return report0;
        }

        public class FrontEnvelopSet {
            public CustomerInfo head { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComTel { get; set; }
            public string ComBrn { get; set; }
            //public string CusAddrFull { get; set; }
            //public string SignnatureUrl1 { get; set; }
            //public string SignnatureUrl2 { get; set; }
        }

        public partial class CustomerInfo {
            public int ID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string CustomerID { get; set; }
            public string CustCode { get; set; }
            public string OrgType { get; set; }
            public string TypeID { get; set; }
            public string ParentID { get; set; }
            public DateTime? Birthdate { get; set; }
            public string ShortCode { get; set; }
            public string BrnCode { get; set; }
            public string BrnDesc { get; set; }
            public string RefID { get; set; }
            public string TitleTh { get; set; }
            public string NameTh1 { get; set; }
            public string NameTh2 { get; set; }
            public string NameTh3 { get; set; }
            public string FullNameTh { get; set; }
            public string TitleEn { get; set; }
            public string NameEn1 { get; set; }
            public string NameEn2 { get; set; }
            public string NameEn3 { get; set; }
            public string FullNameEn { get; set; }
            public string PersonTypeID { get; set; }
            public string GroupID { get; set; }
            public string SubGroupID { get; set; }
            public string ProductGroupID { get; set; }
            public string TaxID { get; set; }
            public string Currency { get; set; }
            public DateTime? CardExpireDate { get; set; }
            public string CardIssue { get; set; }
            public string TaxTypeID { get; set; }
            public string PaymentTermID { get; set; }
            public string PaymentGrade { get; set; }
            public decimal CreditLimit { get; set; }
            public string ContactPerson { get; set; }
            public string AddrFull { get; set; }
            public string AddrNo { get; set; }
            public string AddrMoo { get; set; }
            public string Building { get; set; }
            public string RoomNo { get; set; }
            public string FloorNo { get; set; }
            public string Village { get; set; }
            public string Soi { get; set; }
            public string Yaek { get; set; }
            public string Road { get; set; }
            public string AddrTumbon { get; set; }
            public string AddrAmphoe { get; set; }
            public string AddrProvince { get; set; }
            public string AddrPostCode { get; set; }
            public string AddrCountry { get; set; }
            public string BillAddr1 { get; set; }
            public string BillAddr2 { get; set; }
            public string BillMemo { get; set; }
            public string BillContact { get; set; }
            public string PaymentMethod { get; set; }
            public string Tel1 { get; set; }
            public string Tel2 { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Fax { get; set; }
            public string LineID { get; set; }
            public string SalePersonID { get; set; }
            public string AreaID { get; set; }
            public string BankCode { get; set; }
            public string BookIBankID { get; set; }
            public string Occupation { get; set; }
            public string Gender { get; set; }
            public string Marriage { get; set; }
            public string Lang { get; set; }
            public string Race { get; set; }
            public string Nationality { get; set; }
            public string Status { get; set; }
            public bool IsHold { get; set; }
            public byte[] Photo { get; set; }
            public string Geolocation { get; set; }
            public decimal? Point { get; set; }
            public string AccID_Debtor { get; set; }
            public string AccID_Creditor { get; set; }
            public string AccID_Wht { get; set; }
            public string Acc_Side { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }


            public string NameDisplay
            {
                get
                {
                    return CustomerID.ToString() + " " +
                    NameTh1;
                }
            }


        }
    }
}