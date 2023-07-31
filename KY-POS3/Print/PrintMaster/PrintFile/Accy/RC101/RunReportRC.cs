using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace PrintMaster.PrintFile.Accy.RC101
{
    public class RunReportRC
    {
        //RC101 พิมพ์ใบเสร็จรับเงินขายสินค้า


        public static RC101 OpenReport(PrintData print_row,string typeprint) {
            var doc = JsonConvert.DeserializeObject<RC101Set>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            RC101 report = null;

            if (typeprint == "RC101")
            {
                RC101 report0 = new RC101(doc, "", typeprint, "0");
                report0.DisplayName = doc.head.RCID;
                report0.CreateDocument();
                RC101 report1 = new RC101(doc, "", typeprint, "1");
                report1.CreateDocument();
                report0.Pages.AddRange(report1.Pages);
                report = report0;
            }
            else
            {
                RC101 report0 = new RC101(doc, "ลูกค้า", typeprint, "0");
                report0.DisplayName = doc.head.RCID;
                report0.CreateDocument();
                RC101 report1 = new RC101(doc, "ลูกค้า", typeprint, "1");
                report1.CreateDocument();
                report0.Pages.AddRange(report1.Pages);
                RC101 report2 = new RC101(doc, "บัญชี", typeprint, "1");
                report2.CreateDocument();
                report0.Pages.AddRange(report2.Pages);
                RC101 report3 = new RC101(doc, "การเงิน", typeprint, "1");
                report3.CreateDocument();
                report0.Pages.AddRange(report3.Pages);
                report = report0;
            }

            return report;
        }

        public class RC101Set
        {
            public ORCHead head { get; set; }
            public List<ORCLine> line { get; set; }
            public List<ORCPayLine> payline { get; set; }
            public List<RCPrintLine> linePrint { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string BillAddr1 { get; set; }
            public string BillAddr2 { get; set; }
            public string SignnatureUrl { get; set; }
            public string SignnatureUrl1 { get; set; }
            public string SignnatureUrl2 { get; set; }
        }
        public class ORCHead
        {
            public int ID { get; set; }
            public string RCID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string DocType { get; set; }
            public DateTime RCDate { get; set; }
            public string CustID { get; set; }
            public string CustName { get; set; }
            public string CustTaxID { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string PayBy { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string Status { get; set; }
            public string RCStatus { get; set; }
            public DateTime? LinkDate { get; set; }
            public string LinkBy { get; set; }
            public string PayMemo { get; set; }
            public string LinkRefID { get; set; }
            public int CountPaymentLine { get; set; }
            public int CountInvLine { get; set; }
            public decimal PayINVAmt { get; set; }
            public decimal PayINVVatAmt { get; set; }
            public decimal PayINVTotalAmt { get; set; }
            public decimal PayTotalAmt { get; set; }
            public decimal PayTotalDiffINVAmt { get; set; }
            public string PayToBookID { get; set; }
            public string PayToBookName { get; set; }
            public string PayToBankCode { get; set; }
            public DateTime PayDate { get; set; }
            public DateTime? ClearingDate { get; set; }
            public DateTime? StatementDate { get; set; }
            public string CustBankCode { get; set; }
            public string CustBankName { get; set; }
            public string CustBankBranch { get; set; }
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
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }
        public class ORCLine
        {
            public int ID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string RCID { get; set; }
            public int LineNum { get; set; }
            public string DocType { get; set; }
            public DateTime RCDate { get; set; }
            public string CustID { get; set; }
            public string SOID { get; set; }
            public string ShipID { get; set; }
            public string BillID { get; set; }
            public int PayNo { get; set; }
            public string INVID { get; set; }
            public string INVDocTypeID { get; set; }
            public DateTime INVDate { get; set; }
            public DateTime InvDueDate { get; set; }
            public decimal InvTotalAmt { get; set; }
            public decimal InvPreviousAmt { get; set; }
            public decimal PayVatAmt { get; set; }
            public decimal PayAmt { get; set; }
            public decimal PayTotalAmt { get; set; }
            public decimal VatRate { get; set; }
            public string Currency { get; set; }
            public decimal InvRateExchange { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string Remark3 { get; set; }
            public string Status { get; set; }
            public string RCStatus { get; set; }
            public bool IsActive { get; set; }
        }

        public class ORCPayLine
        {
            public int ID { get; set; }
            public string RCID { get; set; }
            public int LineNum { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string DocType { get; set; }
            public DateTime RCDate { get; set; }
            public string CustID { get; set; }
            public string CustName { get; set; }
            public string WHTRefNo { get; set; }
            public decimal TaxBaseAmt { get; set; }
            public decimal TaxRate { get; set; }
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
            public string CustBankCode { get; set; }
            public string CustBankName { get; set; }
            public string CustBankBranch { get; set; }
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
            public string RCStatus { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }

        public class RCPrintLine
        {
            public string ItemID { get; set; }
            public string ItemName { get; set; }
            public string RCType { get; set; }
            public string Doctype { get; set; }
            public decimal Qty { get; set; }
            public decimal TotalAmt { get; set; }
            public decimal TotalAmtIntVat { get; set; }
            public decimal InvoiceTotalAmtVat { get; set; }
        }

    }
}