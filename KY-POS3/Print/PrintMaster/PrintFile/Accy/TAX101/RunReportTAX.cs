using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace PrintMaster.PrintFile.Accy.TAX101
{
    public class RunReportTAX
    {
        //CN101 CN

        public static TAX101 OpenReport(PrintData print_row,string typeprint) {
            var doc = JsonConvert.DeserializeObject<TAX101Set>(print_row.JsonData);

            TAX101 report0 = new TAX101(doc);
            report0.DisplayName = doc.head.TaxSubmitID;
            report0.CreateDocument();

            return report0;
        }

        public class TAX101Set
        {
            public OTaxSubmitHead head { get; set; }
            public List<vw_OTaxSubmitLine> line { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string lblDescInfo { get; set; }
        }
        public class OTaxSubmitHead
        {
            public int ID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string TaxSubmitID { get; set; }
            public string TaxType { get; set; }
            public string AccSide { get; set; }
            public int TaxYear { get; set; }
            public int TaxMonth { get; set; }
            public int TaxSeries { get; set; }
            public int CountINV { get; set; }
            public decimal Amt { get; set; }
            public decimal TotaVatlAmt { get; set; }
            public decimal TotaAmt { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }
        public class vw_OTaxSubmitLine
        {
            public int ID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string TaxSubmitID { get; set; }
            public string TaxType { get; set; }
            public int TaxYear { get; set; }
            public int TaxMonth { get; set; }
            public int TaxSeries { get; set; }
            public DateTime TaxDate { get; set; }
            public string AccSide { get; set; }
            public string INVID { get; set; }
            public string RCID { get; set; }
            public string TaxKeyID { get; set; }
            public DateTime? INVDate { get; set; }
            public string INVDocType { get; set; }
            public string CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string BrnDesc { get; set; }
            public string TaxID { get; set; }
            public decimal WHTTaxBase { get; set; }
            public string TaxRefNo { get; set; }
            public decimal TaxRate { get; set; }
            public decimal Amt { get; set; }
            public decimal TotaVatlAmt { get; set; }
            public decimal TotaAmt { get; set; }
            public bool IsActive { get; set; }
        }
    }
}