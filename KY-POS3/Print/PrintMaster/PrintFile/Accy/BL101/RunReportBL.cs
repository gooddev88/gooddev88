using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace PrintMaster.PrintFile.Accy.BL101
{
    public class RunReportBL
    {

        public static BL101 OpenReport(PrintData print_row,string typeprint) {
            var doc = JsonConvert.DeserializeObject<BL101Set>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            BL101 report0 = new BL101(doc, "", typeprint, "0");
            report0.DisplayName = doc.head.BillNo;
            report0.CreateDocument();
            BL101 report1 = new BL101(doc, "", typeprint, "1");
            report1.CreateDocument();
            report0.Pages.AddRange(report1.Pages);

            return report0;
        }

        public class BL101Set
        {
            public OBillingHead head { get; set; }
            public List<OBillingLine> line { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string CusAddrFull { get; set; }
            public string SignnatureUrl1 { get; set; }
            public string SignnatureUrl2 { get; set; }
        }
        public partial class OBillingHead
        {
            public int ID { get; set; }
            /// <summary>
            /// สาขา
            /// </summary>
            public string RComID { get; set; }
            /// <summary>
            /// สาขา
            /// </summary>
            public string ComID { get; set; }
            public string BillNo { get; set; }
            public DateTime BillDate { get; set; }
            public string RefNo { get; set; }
            public string BrnNo { get; set; }
            public string BillType { get; set; }
            public string CustID { get; set; }
            public string CustName { get; set; }
            public string CustTaxID { get; set; }
            public string CustBrn { get; set; }
            public string CustAddr1 { get; set; }
            public string CustAddr2 { get; set; }
            public string CustAddr3 { get; set; }
            public string CustAddr4 { get; set; }
            public string Memo { get; set; }
            /// <summary>
            /// สุทธิรวม VAT
            /// </summary>
            public decimal BillAmt { get; set; }
            public decimal? INVAmt { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public bool IsActive { get; set; }
        }
        public partial class OBillingLine
        {
            public int ID { get; set; }
            /// <summary>
            /// สาขา
            /// </summary>
            public string RComID { get; set; }
            /// <summary>
            /// สาขา
            /// </summary>
            public string ComID { get; set; }
            public string BillNo { get; set; }
            public int LineNum { get; set; }
            public string BillType { get; set; }
            public string Refno { get; set; }
            public DateTime BillDate { get; set; }
            public string BrnNo { get; set; }
            public string CustID { get; set; }
            public string CustName { get; set; }
            public string CustTaxID { get; set; }
            public string CustBrn { get; set; }
            public string INVID { get; set; }
            public DateTime? INVDate { get; set; }
            public DateTime? PayDueDate { get; set; }
            /// <summary>
            /// สุทธิรวม VAT
            /// </summary>
            public decimal BillAmt { get; set; }
            public decimal? INVAmt { get; set; }
            public bool IsActive { get; set; }
        }
    }
}