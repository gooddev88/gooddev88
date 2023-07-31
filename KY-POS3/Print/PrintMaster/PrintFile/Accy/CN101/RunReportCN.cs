using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace PrintMaster.PrintFile.Accy.CN101
{
    public class RunReportCN {
        //CN101 CN

        public static CN101 OpenReport(PrintData print_row,string typeprint) {
            var doc = JsonConvert.DeserializeObject<CN101Set>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            CN101 report0 = new CN101(doc, "ลูกค้า", typeprint, "0");
            report0.DisplayName = doc.head.INVID;
            report0.CreateDocument();
            CN101 report1 = new CN101(doc, "ลูกค้า", typeprint, "1");
            report1.CreateDocument();
            report0.Pages.AddRange(report1.Pages);
            CN101 report2 = new CN101(doc, "บัญชี", typeprint, "1");
            report2.CreateDocument();
            report0.Pages.AddRange(report2.Pages);
            CN101 report3 = new CN101(doc, "การเงิน", typeprint, "1");
            report3.CreateDocument();
            report0.Pages.AddRange(report3.Pages);

            return report0;
        }

        public class CN101Set
        {
            public OINVHead head { get; set; }
            public List<OINVLine> line { get; set; }
            public OINVLine DescSOInfo { get; set; }
            public OINVHead RefDocInv { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string SignnatureUrl1 { get; set; }
            public string SignnatureUrl2 { get; set; }
        }
        public class OINVHead
        {
            public int ID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string INVID { get; set; }
            public string DocTypeID { get; set; }
            public DateTime INVDate { get; set; }
            public string CustID { get; set; }
            public string CustName { get; set; }
            public string CustAddr1 { get; set; }
            public string CustAddr2 { get; set; }
            public string CustTaxID { get; set; }
            public string CustBrnID { get; set; }
            public string CustBrnName { get; set; }
            public string POID { get; set; }
            public DateTime? PODate { get; set; }
            public string RefDocID { get; set; }
            public string AccGroupID { get; set; }
            public string BillToCustID { get; set; }
            public string BillAddr1 { get; set; }
            public string BillAddr2 { get; set; }
            public string ShipFrLocID { get; set; }
            public string ShipFrSubLocID { get; set; }
            public string SalesID1 { get; set; }
            public string SalesID2 { get; set; }
            public string Currency { get; set; }
            public decimal RateExchange { get; set; }
            public string RateBy { get; set; }
            public DateTime? RateDate { get; set; }
            public string TermID { get; set; }
            public DateTime? PayDueDate { get; set; }
            public string PaymentType { get; set; }
            public string PaymentMemo { get; set; }
            public decimal Qty { get; set; }
            public decimal QtyOrder { get; set; }
            public decimal QtyShip { get; set; }
            public int CountLine { get; set; }
            public decimal BaseNetTotalAmt { get; set; }
            public decimal NetTotalAmt { get; set; }
            public decimal NetTotalVatAmt { get; set; }
            public decimal NetTotalAmtIncVat { get; set; }
            public decimal ItemDiscAmt { get; set; }
            public decimal OntopDiscPer { get; set; }
            public decimal OntopDiscAmt { get; set; }
            public string DiscCalBy { get; set; }
            public decimal VatRate { get; set; }
            public string VatTypeID { get; set; }
            public string SOInfo { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string RemarkRC { get; set; }
            public bool IsPrint { get; set; }
            public DateTime? PrintDate { get; set; }
            public string SOID { get; set; }
            public DateTime? SODate { get; set; }
            public string ShipID { get; set; }
            public DateTime? ShipDate { get; set; }
            public string RCID { get; set; }
            public DateTime? RCDate { get; set; }
            public string BillingID { get; set; }
            public DateTime? BillingDate { get; set; }
            public string RCStatus { get; set; }
            public decimal RCAmt { get; set; }
            public decimal INVPendingAmt { get; set; }
            public int RCLastPayNo { get; set; }
            public bool IsLink { get; set; }
            public DateTime? LinkDate { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsReverse { get; set; }
            public bool IsActive { get; set; }
        }
        public class OINVLine
        {
            public int ID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string INVID { get; set; }
            public int LineNum { get; set; }
            public string DocTypeID { get; set; }
            public DateTime INVDate { get; set; }
            public string RefDocID { get; set; }
            public int RefDocLineNum { get; set; }
            public string CustomerID { get; set; }
            public string ItemID { get; set; }
            public string ItemName { get; set; }
            public string ItemTypeID { get; set; }
            public string ItemTypeName { get; set; }
            public string ItemCateID { get; set; }
            public string ItemGroupID { get; set; }
            public string ItemAccGroupID { get; set; }
            public bool IsStockItem { get; set; }
            public decimal Weight { get; set; }
            public string WUnit { get; set; }
            public decimal QtyOrder { get; set; }
            public decimal QtyShip { get; set; }
            public decimal Qty { get; set; }
            public string Unit { get; set; }
            public string Packaging { get; set; }
            public decimal Price { get; set; }
            public decimal PriceIncVat { get; set; }
            public decimal BaseTotalAmt { get; set; }
            public decimal TotalAmt { get; set; }
            public decimal VatAmt { get; set; }
            public decimal TotalAmtIncVat { get; set; }
            public decimal VatRate { get; set; }
            public string VatTypeID { get; set; }
            public decimal OntopDiscAmt { get; set; }
            public decimal OntopDiscPer { get; set; }
            public decimal DiscPer { get; set; }
            public decimal DiscAmt { get; set; }
            public string DiscCalBy { get; set; }
            public string LocID { get; set; }
            public string SubLocID { get; set; }
            public string PackagingNo { get; set; }
            public string LotNo { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string PointID { get; set; }
            public string PointName { get; set; }
            public string SOID { get; set; }
            public int SOLineNum { get; set; }
            public string ShipID { get; set; }
            public int ShipLineNum { get; set; }
            public string Status { get; set; }
            public bool IsComplete { get; set; }
            public bool IsActive { get; set; }
        }
    }
}