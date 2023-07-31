
using DevExpress.XtraReports;
using Newtonsoft.Json;
using PrintMaster.Data.DA;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PrintMaster.PrintFile.Accy.WHT101 {
    public class RunReportWHT
    {

        public static WHT101 OpenReport(PrintData print_row, string typeprint) {
            var doc = JsonConvert.DeserializeObject<WHT101Set>(print_row.JsonData);

            WHT101 report1 = new WHT101(doc,"1");
            report1.DisplayName = doc.head.WHTNo;
            report1.CreateDocument();
          

            WHT101 report2 = new WHT101(doc, "2");
            report2.CreateDocument();
            report1.Pages.AddRange(report2.Pages);

            WHT101 report3 = new WHT101(doc, "3");
            report3.CreateDocument();
            report1.Pages.AddRange(report3.Pages);

            WHT101 report4 = new WHT101(doc, "4");
            report4.CreateDocument();
            report1.Pages.AddRange(report4.Pages);
            return report1;
           
        }

        public class WHT101Set {
            public PVWHTHead head { get; set; }
            public List<PVPayLine> line { get; set; }
            public List<PVPayLineDisplay> lineDisplay { get; set; }
            public vw_UserInfo User { get; set; }
            public string UserAddress { get; set; }
            public string ComName { get; set; }
            public string addr_full { get; set; }
            
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string CusAddrFull { get; set; }
            public string WHTRemark { get; set; }

            public string SignnatureUrl1 { get; set; }
        }

        public class PVWHTHead {
            //public int ID { get; set; }
            //public string RComID { get; set; }
            //public string ComID { get; set; }
            //public string WHTNo { get; set; }
            //public string VendID { get; set; }
            //public DateTime? WHTDate { get; set; }
            //public string WHTTypeID { get; set; }
            //public string WHTTypeName { get; set; }
            //public string DocID { get; set; }
            //public string PayCondID { get; set; }
            //public string PayCondName { get; set; }
            //public string CustID { get; set; }
            //public string CustName { get; set; }
            //public string CustOrgName { get; set; }
            //public string CustBrn { get; set; }
            //public string CustPersonID { get; set; }
            //public string CustTaxID { get; set; }
            //public string CustAddr1 { get; set; }
            //public string CustAddr2 { get; set; }
            //public string CustAddrSubDistrict { get; set; }
            //public string CustAddrDistrict { get; set; }
            //public string CustAddrProvince { get; set; }
            //public string CustFullAddr1 { get; set; }
            //public string CustFullAddr2 { get; set; }
            //public string CreatedBy { get; set; }
            //public DateTime CreatedDate { get; set; }
            //public string ModifiedBy { get; set; }
            //public DateTime? ModifiedDate { get; set; }
            //public bool IsActive { get; set; }
            public int ID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string WHTNo { get; set; }
            public DateTime? WHTDate { get; set; }
            public string WHTTypeID { get; set; }
            public string WHTTypeName { get; set; }
            public string DocID { get; set; }
            public string PayCondID { get; set; }
            public string PayCondName { get; set; }
            public string VendTitle { get; set; }
            public string VendID { get; set; }
            public string VendName { get; set; }
            public string VendOrgName { get; set; }
            public string VendBrn { get; set; }

            public string VendTaxID { get; set; }
            public string VendAddr1 { get; set; }
            public string VendAddr2 { get; set; }
            public string Postcode { get; set; }
            public string Building { get; set; }
            public string RoomNo { get; set; }
            public string FloorNo { get; set; }
            public string Village { get; set; }
            public string Soi { get; set; }
            public string Yaek { get; set; }
            public string Road { get; set; }
            public string HouseNo { get; set; }
            public string VendAddrMoo { get; set; }
            public string VendAddrSubDistrict { get; set; }
            public string VendAddrDistrict { get; set; }
            public string VendAddrProvince { get; set; }
            public string VendFullAddr1 { get; set; }
            public string VendFullAddr2 { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }

        public partial class PVPayLine {
            //public int ID { get; set; }
            //public string DocID { get; set; }
            //public int LineNum { get; set; }
            //public string RCompanyID { get; set; }
            //public string CompanyID { get; set; }
            //public string DocType { get; set; }
            //public DateTime DocDate { get; set; }
            //public string CustID { get; set; }
            //public string CustName { get; set; }
            //public string WHTNo { get; set; }
            //public string WHTLineTypeID { get; set; }
            //public string WHTDesc { get; set; }
            //public decimal TaxBaseAmt { get; set; }
            //public decimal TaxRate { get; set; }
            //public decimal? TaxRateActual { get; set; }
            //public string PayBy { get; set; }
            //public string PayByType { get; set; }
            //public string PayByDesc { get; set; }
            //public string PayByCate { get; set; }
            //public string PayMemo { get; set; }
            //public decimal PayAmt { get; set; }
            //public string PayToBankCode { get; set; }
            //public string PayToBookID { get; set; }
            //public string PayToBookName { get; set; }
            //public DateTime PayDate { get; set; }
            //public DateTime? ClearingDate { get; set; }
            //public DateTime? StatementDate { get; set; }
            //public string CustBankCode { get; set; }
            //public string CustBankName { get; set; }
            //public string CustBankBranch { get; set; }
            //public string PaymentRefNo { get; set; }
            //public DateTime? ChqDate { get; set; }
            //public DateTime? ChqDepositDate { get; set; }
            //public DateTime? ChqExpired { get; set; }
            //public DateTime? ChqReturnDate { get; set; }
            //public string ChqReturnReason { get; set; }
            //public DateTime? CompletedDate { get; set; }
            //public string CompletedMemo { get; set; }
            //public string Currency { get; set; }
            //public decimal RateExchange { get; set; }
            //public string RateBy { get; set; }
            //public DateTime RateDate { get; set; }
            //public string DataSource { get; set; }
            //public string PVStatus { get; set; }
            //public string Status { get; set; }
            //public string CreatedBy { get; set; }
            //public DateTime CreatedDate { get; set; }
            //public string ModifiedBy { get; set; }
            //public DateTime? ModifiedDate { get; set; }
            //public bool IsActive { get; set; }
            public int ID { get; set; }
            public string DocID { get; set; }
            public int LineNum { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string DocType { get; set; }
            public DateTime DocDate { get; set; }
            public string VendID { get; set; }
            public string VendName { get; set; }
            public string WHTNo { get; set; }
            public string WHTLineTypeID { get; set; }
            public string WHTDesc { get; set; }
            public string WHTRemark { get; set; }
            public decimal TaxBaseAmt { get; set; }
            public decimal TaxRate { get; set; }
            public decimal? TaxRateActual { get; set; }
            public decimal TaxBaseWithWhtAmt { get; set; }
            public string PayBy { get; set; }
            public string PayByType { get; set; }
            public string PayByDesc { get; set; }
            public string PayByCate { get; set; }
            public string PayMemo { get; set; }
            public decimal PayAmt { get; set; }
            public string PayToBankCode { get; set; }
            public string PayToBookID { get; set; }
            public string PayToBookName { get; set; }
            public DateTime PayDate { get; set; }
            public DateTime? ClearingDate { get; set; }
            public DateTime? StatementDate { get; set; }
            public string VendBankCode { get; set; }
            public string VendBankName { get; set; }
            public string VendBankBranch { get; set; }
            public string PaymentRefNo { get; set; }
            public DateTime? ChqDate { get; set; }
            public DateTime? ChqDepositDate { get; set; }
            public DateTime? ChqExpired { get; set; }
            public DateTime? ChqReturnDate { get; set; }
            public string ChqReturnReason { get; set; }
            public DateTime? CompletedDate { get; set; }
            public string CompletedMemo { get; set; }
            public string Currency { get; set; }
            public decimal RateExchange { get; set; }
            public string RateBy { get; set; }
            public DateTime RateDate { get; set; }
            public string DataSource { get; set; }
            public string PVStatus { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }

        public partial class MasterTypeLine {
            public int ID { get; set; }
            public string RComID { get; set; }
            public string MasterTypeID { get; set; }
            public string ValueTXT { get; set; }
            public decimal ValueNUM { get; set; }
            public string Description1 { get; set; }
            public string Description2 { get; set; }
            public string Description3 { get; set; }
            public string Description4 { get; set; }
            public int Sort { get; set; }
            public string ParentID { get; set; }
            public string ParentValue { get; set; }
            public string RefID { get; set; }
            public string RefIDL2 { get; set; }
            public string RefIDL3 { get; set; }
            public bool? IsSysData { get; set; }
            public bool? IsActive { get; set; }
        }

        public partial class vw_UserInfo {
            public string Username { get; set; }
            public string Password { get; set; }
            public string EmpCode { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
            public string FirstName_En { get; set; }
            public string LastName_En { get; set; }
            public string FullName_En { get; set; }
            public string NickName { get; set; }
            public string Gender { get; set; }
            public string DepartmentID { get; set; }
            public string SubDepartmentID { get; set; }
            public string PositionID { get; set; }
            public string JobLevel { get; set; }
            public string JobType { get; set; }
            public bool IsProgramUser { get; set; }
            public bool IsNewUser { get; set; }
            public DateTime? JobStartDate { get; set; }
            public DateTime? ResignDate { get; set; }
            public string WorkAge { get; set; }
            public int? WorkYear { get; set; }
            public string AddrFull { get; set; }
            public string AddrNo { get; set; }
            public string AddrMoo { get; set; }
            public string AddrTumbon { get; set; }
            public string AddrAmphoe { get; set; }
            public string AddrProvince { get; set; }
            public string AddrPostCode { get; set; }
            public string AddrCountry { get; set; }
            public string Tel { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public DateTime? Birthdate { get; set; }
            public string MaritalStatus { get; set; }
            public string CitizenId { get; set; }
            public string BookBankNumber { get; set; }
            public string AuthenType { get; set; }
            public string ApproveBy { get; set; }
            public bool? UseTimeStamp { get; set; }
            public string ImageProfile { get; set; }
            public string LineToken { get; set; }
            public bool? IsSuperMan { get; set; }
            public string DefaultCompany { get; set; }
            public string UserType { get; set; }
            public string RelateID { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
            public string XStatus { get; set; }
        }

        public partial class PVPayLineDisplay : MasterTypeLine {
            public string DocID { get; set; }
            public int LineNum { get; set; }
            public string CompanyID { get; set; }
            public string DocType { get; set; }
            public string DocDate { get; set; }
           
            public string CustID { get; set; }
            public string CustName { get; set; }
            public string WHTNo { get; set; }
            public decimal TaxBaseAmt { get; set; }
            public decimal TaxRate { get; set; }
            public decimal PayAmt { get; set; }
        }

    }
}
