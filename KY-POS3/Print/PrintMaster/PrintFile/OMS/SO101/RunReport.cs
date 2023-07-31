using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PrintMaster.PrintFile.OMS.SO101 {
    public class RunReport {

        public static SO101 OpenReport(PrintData print_row, out string reportname) {
            reportname = "";
            var doc = JsonConvert.DeserializeObject<SO101Set>(print_row.JsonData);
            reportname = doc.Head.OrdID;
            List<int> print_list = new List<int> { 0, 1 };
            SO101 report0 = new SO101(doc, "0");
            report0.DisplayName = doc.Head.OrdID;
            report0.CreateDocument();
            SO101 report1 = new SO101(doc, "1");
            report1.CreateDocument();
            report0.Pages.AddRange(report1.Pages);
          //  report1.DisplayName = doc.Head.OrdID;
           // report0.DisplayName = doc.Head.OrdID;
            return report0;
        }

        public class SO101Set {
            public vw_OSOHead Head { get; set; }
            public List<vw_OSOLine> Line { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string SignnatureUrl { get; set; }
            public string SignnatureUrl1 { get; set; }
            public string SignnatureUrl2 { get; set; }

        }

        public  class vw_OSOHead {
            public int ID { get; set; }
            public string OrdID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public DateTime OrdDate { get; set; }
            public string DocTypeID { get; set; }
            public string DocTypeName { get; set; }
            public string INVID { get; set; }
            public DateTime? INVDate { get; set; }
            public string CustID { get; set; }
            public string CustName { get; set; }
            public string CustAddr1 { get; set; }
            public string CustAddr2 { get; set; }
            public string CustTaxID { get; set; }
            public string CustBrnID { get; set; }
            public string CustBrnName { get; set; }
            public string CustMobile { get; set; }
            public string CustEmail { get; set; }
            public string BrandID { get; set; }
            public string DepID { get; set; }
            public string AreaID { get; set; }
            public string PaymentTermID { get; set; }
            public string DeliveryBy { get; set; }
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
            public string SalesName { get; set; }
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
            public decimal QtyShip { get; set; }
            public decimal QtyInvoice { get; set; }
            public decimal QtyReturn { get; set; }
            public decimal QtyShipPending { get; set; }
            public decimal QtyInvoicePending { get; set; }
            public decimal AmtShipPending { get; set; }
            public decimal AmtInvoice { get; set; }
            public decimal AmtInvoicePending { get; set; }
            public int CountLine { get; set; }
            public decimal BaseNetTotalAmt { get; set; }
            public decimal BaseNetTotalAmtIncVat { get; set; }
            public decimal NetTotalAmt { get; set; }
            public decimal NetTotalVatAmt { get; set; }
            public decimal NetTotalAmtIncVat { get; set; }
            public decimal ItemDiscAmtIncVat { get; set; }
            public decimal ItemDiscAmt { get; set; }
            public decimal OntopDiscPer { get; set; }
            public decimal OntopDiscAmt { get; set; }
            public decimal OntopDiscAmtIncVat { get; set; }
            public string DiscCalBy { get; set; }
            public decimal VatRate { get; set; }
            public string VatTypeID { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string TName { get; set; }
            public string AName { get; set; }
            public string PName { get; set; }
            public bool IsPrint { get; set; }
            public DateTime? PrintDate { get; set; }
            public string ShipID { get; set; }
            public DateTime? ShipDate { get; set; }
            public bool IsLink { get; set; }
            public DateTime? LinkDate { get; set; }
            public string Status { get; set; }
            public string DiscStatus { get; set; }
            public string Source { get; set; }
            public decimal? Lat { get; set; }
            public decimal? Lon { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }
        public class vw_OSOLine {
            public int RN { get; set; }
            public int ID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string OrdID { get; set; }
            public int LineNum { get; set; }
            public string DocTypeID { get; set; }
            public DateTime OrdDate { get; set; }
            public string INVID { get; set; }
            public DateTime? INVDate { get; set; }
            public string RefDocID { get; set; }
            public int RefDocLineNum { get; set; }
            public string CustID { get; set; }
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
            public decimal Qty { get; set; }
            public decimal QtyShip { get; set; }
            public decimal QtyInvoice { get; set; }
            public decimal QtyShipPending { get; set; }
            public decimal QtyInvoicePending { get; set; }

            public decimal AmtShipPending { get; set; }
            public decimal AmtInvoicePending { get; set; }
            public string Unit { get; set; }
            public string Packaging { get; set; }
            public decimal Price { get; set; }
            public decimal PriceIncVat { get; set; }
            public decimal BaseTotalAmt { get; set; }
            public decimal BaseTotalAmtIncVat { get; set; }
            public decimal TotalAmt { get; set; }
            public decimal VatAmt { get; set; }
            public decimal TotalAmtIncVat { get; set; }
            public decimal VatRate { get; set; }
            public string VatTypeID { get; set; }
            public decimal OntopDiscAmt { get; set; }
            public decimal OntopDiscPer { get; set; }
            public decimal OntopDiscAmtIncVat { get; set; }
            public decimal DiscPer { get; set; }
            public decimal DiscAmt { get; set; }
            public decimal DiscAmtIncVat { get; set; }
            public string DiscCalBy { get; set; }
            public decimal DiscPPU { get; set; }
            public decimal DiscPPUIncVat { get; set; }
            public string LocID { get; set; }
            public string LocName { get; set; }
            public string SubLocID { get; set; }
            public string PackagingNo { get; set; }
            public string LotNo { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }
            public bool? IsSpecialPrice { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string PointID { get; set; }
            public string PointName { get; set; }
            public string ShipID { get; set; }
            public int ShipLineNum { get; set; }
            public string ImageUrl { get; set; }
            public string DiscApproveBy { get; set; }
            public DateTime? DiscApproveDate { get; set; }
            public string Status { get; set; }
            public bool IsComplete { get; set; }
            public int? Sort { get; set; }
            public string ProID { get; set; }
            public string ProName { get; set; }
            public string PatternID { get; set; }
            public bool? PageBreak { get; set; }
            public bool IsActive { get; set; }
        }
    }
}