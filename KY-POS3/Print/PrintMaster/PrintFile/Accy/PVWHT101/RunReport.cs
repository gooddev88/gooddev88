
using DevExpress.XtraReports;
using Newtonsoft.Json;
using PrintMaster.Data.DA;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PrintMaster.PrintFile.Accy.PVWHT101 {
    public class RunReport {

        public static PVWHT101 OpenReport(PrintData print_row, string typeprint) {
            var doc = JsonConvert.DeserializeObject<PVWHT101Set>(print_row.JsonData);

            PVWHT101 report1 = new PVWHT101(doc);
            report1.DisplayName = doc.ReportName;
            report1.CreateDocument();
            return report1;
           
        }

        public class PVWHT101Set {
            public string ReportName { get; set; }
            public List<vw_PVWHT> PVWHTs { get; set; }
        }
        public class vw_PVWHT {
            public int ID { get; set; }
            public string DocID { get; set; }
            public int LineNum { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string CompanyName { get; set; }
            public string ComTaxID { get; set; }
            public string ComCode { get; set; }
            public string DocType { get; set; }
            public DateTime DocDate { get; set; }
            public string WHTNo { get; set; }
            public string WHTLineTypeID { get; set; }
            public string WHTDesc { get; set; }
            public string WHTRemark { get; set; }
            public DateTime? WHTDate { get; set; }
            public string WHTTypeID { get; set; }
            public string WHTTypeName { get; set; }
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
            public string Building { get; set; }
            public string RoomNo { get; set; }
            public string FloorNo { get; set; }
            public string Village { get; set; }
            public string Soi { get; set; }
            public string Yaek { get; set; }
            public string Road { get; set; }
            public string HouseNo { get; set; }
            public string Postcode { get; set; }
            public string VendAddrMoo { get; set; }
            public string VendAddrSubDistrict { get; set; }
            public string VendAddrDistrict { get; set; }
            public string VendAddrProvince { get; set; }
            public string VendFullAddr1 { get; set; }
            public string VendFullAddr2 { get; set; }
            public decimal TaxBaseAmt { get; set; }
            public decimal TaxRate { get; set; }
            public decimal? TaxRateActual { get; set; }
            public decimal TaxBaseWithWhtAmt { get; set; }
            public string PayBy { get; set; }
            public string PayByType { get; set; }
            public string PayByDesc { get; set; }
            public decimal PayAmt { get; set; }
            public string VendBankCode { get; set; }
            public string VendBankName { get; set; }
            public string VendBankBranch { get; set; }
            public string PaymentRefNo { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }


    }
}
