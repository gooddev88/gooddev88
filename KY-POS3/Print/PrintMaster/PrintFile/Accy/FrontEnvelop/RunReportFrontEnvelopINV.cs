using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 

namespace PrintMaster.PrintFile.Accy.FrontEnvelop {
    public class RunReportFrontEnvelopINV
    {

        public static EnvelopINV OpenReport(PrintData print_row,string typeprint) {
            var doc = JsonConvert.DeserializeObject<FrontEnvelopINVSet>(print_row.JsonData);
            List<int> print_list = new List<int> { 0, 1 };

            EnvelopINV report0 = new EnvelopINV(doc, "", typeprint, "0");
            report0.DisplayName = doc.head.CustID;
            report0.CreateDocument();
          

            return report0;
        }

        public class FrontEnvelopINVSet {
            public OINVHead head { get; set; }
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

        public class OINVHead {
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
            public string Mobile { get; set; }
            public string Email { get; set; }
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
    }
}