using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PrintMaster.PrintFile.Accy.QO101 {
    public class RunReport {
        //A101 Qoutaion



        public static QO101 OpenReport(PrintData print_row, out string reportname) {
            reportname = "";
            var doc = JsonConvert.DeserializeObject<QO101Set>(print_row.JsonData);
            reportname = doc.head.QOID;
            List<int> print_list = new List<int> { 0, 1 };
            QO101 report0 = new QO101(doc, "0");
            report0.DisplayName = doc.head.QOID;
            report0.CreateDocument();
            QO101 report1 = new QO101(doc, "1");
            report1.CreateDocument();
            report0.Pages.AddRange(report1.Pages);
          //  report1.DisplayName = doc.head.QOID;
           // report0.DisplayName = doc.head.QOID;
            return report0;
        }

        public class QO101Set {
            public OQOHead head { get; set; }
            public List<OQOLine> line { get; set; }
            public string ComName { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string SignnatureUrl { get; set; }
            public string SignnatureUrl1 { get; set; }
            public string SignnatureUrl2 { get; set; }

        }
        public class OQOHead {
            public int ID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string QOID { get; set; }
            public DateTime QODate { get; set; }
            public string OrdID { get; set; }
            public DateTime OrdDate { get; set; }
            public string DocTypeID { get; set; }
            public string CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string CustomerAddr1 { get; set; }
            public string CustomerAddr2 { get; set; }
            public string CustTaxID { get; set; }
            public string CustBrnID { get; set; }
            public string CustBrnName { get; set; }
            public string RefDocID { get; set; }
            public string SalesID1 { get; set; }
            public string SalesID2 { get; set; }
            public string Currency { get; set; }
            public decimal RateExchange { get; set; }
            public string RateBy { get; set; }
            public DateTime? RateDate { get; set; }
            public string TermID { get; set; }
            public string PaymentType { get; set; }
            public string PaymentMemo { get; set; }
            public decimal Qty { get; set; }
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
            public decimal QtyOrder { get; set; }
            public decimal AmtOrder { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public bool IsPrint { get; set; }
            public DateTime ActiveDateFr { get; set; }
            public DateTime ActiveDateTo { get; set; }
            public DateTime? PrintDate { get; set; }
            public string ApproverName { get; set; }
            public bool IsLink { get; set; }
            public DateTime? LinkDate { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }
        public class OQOLine {
            public int RN { get; set; }
            public int ID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string QOID { get; set; }
            public int LineNum { get; set; }
            public string DocTypeID { get; set; }
            public DateTime QODate { get; set; }
            public string OrdID { get; set; }
            public DateTime OrdDate { get; set; }
            public string CustomerID { get; set; }
            public string ItemID { get; set; }
            public string ItemName { get; set; }
            public string ItemTypeID { get; set; }
            public string ItemTypeName { get; set; }
            public string ItemCateID { get; set; }
            public string ItemGroupID { get; set; }
            public decimal Weight { get; set; }
            public string WUnit { get; set; }
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
            public decimal QtyOrder { get; set; }
            public decimal AmtOrder { get; set; }
            public string LocID { get; set; }
            public string SubLocID { get; set; }
            public string PackagingNo { get; set; }
            public string LotNo { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string Status { get; set; }
            
            public int? Sort { get; set; }
            public bool? PageBreak { get; set; }
            public bool IsActive { get; set; }
        }
    }
}