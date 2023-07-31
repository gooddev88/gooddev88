using AutoMapper;
using System;
using System.Collections.Generic;
using static Robot.Data.DA.POSSY.POSSaleConverterService;
using static Robot.Data.DA.POSSY.POSService;  

namespace Robot.PrintOut.CreatePrintData {
    public class SalePrintConverter {

        public static I_POSSaleSetX Convert2Print(I_POSSaleSet input) {
            I_POSSaleSetX doc = new I_POSSaleSetX();
            doc.Head = ConvertHeadModel2X(input.Head);
            doc.Line = ConvertSaleModel2X(input.Line);
            doc.Payment = ConvertPaymentModel2X(input.Payment);
            doc.Username = "";
            doc.ComName = "";
            doc.ComBranch = "";
            doc.ComAddress = "";
            doc.ComImage64 = "";
            doc.ComTax = "";
            doc.ComBrn = "";
            doc.IsVatRegister = false;
            return doc;
        }


        #region Convert
        public static POS_SaleHeadModelX ConvertHeadModel2X(POS_SaleHeadModel input) {
            var destination = new POS_SaleHeadModelX();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleHeadModel, POS_SaleHeadModelX>();
            });
            IMapper iMapper = config.CreateMapper();
            destination= iMapper.Map<POS_SaleHeadModel, POS_SaleHeadModelX>(input);
            return destination;
        }

        public static List<POS_SaleLineModelX> ConvertSaleModel2X(List<POS_SaleLineModel> input) {
            var destination = new List<POS_SaleLineModelX>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SaleLineModel, POS_SaleLineModelX>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                destination.Add(iMapper.Map<POS_SaleLineModel, POS_SaleLineModelX>(i));
            }
            return destination;
        }
        public static List<POS_SalePaymentModelX> ConvertPaymentModel2X(List<POS_SalePaymentModel> input) {
            var destination = new List<POS_SalePaymentModelX>();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<POS_SalePaymentModel, POS_SalePaymentModelX>();
            });
            IMapper iMapper = config.CreateMapper();
            foreach (var i in input) {
                destination.Add(iMapper.Map<POS_SalePaymentModel, POS_SalePaymentModelX>(i));
            }
            return destination;
        }

        #endregion

        #region  class

        public class I_POSSaleSetX {
            public POS_SaleHeadModelX Head { get; set; }
            public List<POS_SaleLineModelX> Line { get; set; }
            public List<POS_SalePaymentModelX> Payment { get; set; }
            public string Username { get; set; }
            public string ComName { get; set; }
            public string ComBranch { get; set; }
            public string ComAddress { get; set; }
            public string ComImage64 { get; set; }
            public string ComTax { get; set; }
            public string ComBrn { get; set; }
            public string QrPaymentData { get; set; }
            public string PriceTaxCon { get; set; }
            public bool IsVatRegister { get; set; }
        }



        public class POS_SaleHeadModelX {
            public int ID { get; set; }
            public string ImageUrlShipTo { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string BillID { get; set; }
            public string INVID { get; set; }
            public string FINVID { get; set; }
            public string DocTypeID { get; set; }
            public string MacNo { get; set; }
            public string MacTaxNo { get; set; }
            public DateTime BillDate { get; set; }
            public string BillRefID { get; set; }
            public string INVPeriod { get; set; }
            public string ReasonID { get; set; }
            public string TableID { get; set; }
            public string TableName { get; set; }
            public string CustomerID { get; set; }
            public string CustomerName { get; set; }
            public string CustAccGroup { get; set; }
            public string CustTaxID { get; set; }
            public string CustBranchID { get; set; }
            public string CustBranchName { get; set; }
            public string CustAddr1 { get; set; }
            public string CustAddr2 { get; set; }
            public bool? IsVatRegister { get; set; }
            public string POID { get; set; }
            public string ShipToLocID { get; set; }
            public string ShipToLocName { get; set; }
            public string ShipToUsePrice { get; set; }
            public string ShipToAddr1 { get; set; }
            public string ShipToAddr2 { get; set; }
            public string SalesID1 { get; set; }
            public string SalesID2 { get; set; }
            public string SalesID3 { get; set; }
            public string SalesID4 { get; set; }
            public string SalesID5 { get; set; }
            public string ContactTo { get; set; }
            public string Currency { get; set; }
            public decimal RateExchange { get; set; }
            public string RateBy { get; set; }
            public DateTime? RateDate { get; set; }
            public string CreditTermID { get; set; }
            public decimal Qty { get; set; }
            public decimal BaseNetTotalAmt { get; set; }
            public decimal NetTotalAmt { get; set; }
            public decimal NetTotalVatAmt { get; set; }
            public decimal NetTotalAmtIncVat { get; set; }
            public decimal OntopDiscPer { get; set; }
            public decimal OntopDiscAmt { get; set; }
            public decimal ItemDiscAmt { get; set; }
            public string DiscCalBy { get; set; }
            public decimal LineDisc { get; set; }
            public decimal VatRate { get; set; }
            public string VatTypeID { get; set; }
            public bool IsVatInPrice { get; set; }
            public decimal NetTotalAfterRound { get; set; }
            public decimal NetDiff { get; set; }
            public decimal PayByCash { get; set; }
            public decimal PayByOther { get; set; }
            public Nullable<decimal> PayByCredit { get; set; }
            public decimal PayTotalAmt { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string RemarkCancel { get; set; }
            public string LocID { get; set; }
            public string SubLocID { get; set; }
            public int CountLine { get; set; }
            public int PayMethodCount { get; set; }
            public bool IsLink { get; set; }
            public string LinkBy { get; set; }
            public DateTime? LinkDate { get; set; }
            public string Status { get; set; }
            public bool IsPrint { get; set; }
            public DateTime? PrintDate { get; set; }
            public bool? IsRecalStock { get; set; }
            public string CreatedByApp { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool IsActive { get; set; }
        }


        public class POS_SaleLineModelX {
            public int ID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string MacNo { get; set; }
            public int LineNum { get; set; }
            public string BillID { get; set; }
            public int? RefLineNum { get; set; }
            public string RefLineUnq { get; set; }
            public string INVID { get; set; }
            public string FINVID { get; set; }
            public string LineUnq { get; set; }
            public string BelongToLineNum { get; set; }
            public string DocTypeID { get; set; }
            public DateTime BillDate { get; set; }
            public string BillRefID { get; set; }
            public string CustID { get; set; }
            public string TableID { get; set; }
            public string TableName { get; set; }
            public string ShipToLocID { get; set; }
            public string ShipToLocName { get; set; }
            public string Barcode { get; set; }
            public string ItemID { get; set; }
            public string ItemName { get; set; }
            public string ItemTypeID { get; set; }
            public string ItemCateID { get; set; }
            public string ItemGroupID { get; set; }
            public bool IsStockItem { get; set; }
            public decimal Weight { get; set; }
            public string WUnit { get; set; }
            public string Unit { get; set; }
            public decimal Qty { get; set; }
            public decimal Cost { get; set; }
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
            public bool IsFree { get; set; }
            public decimal ShareGpPer { get; set; }
            public bool IsOntopItem { get; set; }
            public string PromotionID { get; set; }
            public string PatternID { get; set; }
            public string PaternValue { get; set; }
            public int MatchingNumber { get; set; }
            public string ProPackCode { get; set; }
            public bool IsProCompleted { get; set; }
            public string LocID { get; set; }
            public string SubLocID { get; set; }
            public string LotNo { get; set; }
            public string SerialNo { get; set; }
            public string BatchNo { get; set; }
            public string Remark { get; set; }
            public string Memo { get; set; }
            public string ImgUrl { get; set; }
            public int Sort { get; set; }
            public int? KitchenFinishCount { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool? IsLineActive { get; set; }
            public String KitchenMessageLogo { get; set; }
            public bool IsEditAble { get; set; }
            public String ImageUrl { get; set; }
            public string ImageSource { get; set; }
            public bool IsActive { get; set; }


        }
        public class POS_SalePaymentModelX {

            public int ID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string BillID { get; set; }
            public string PaymentType { get; set; }
            public string PaymentTypeName { get; set; }
            public string LineUnq { get; set; }
            public string INVID { get; set; }
            public string FINVID { get; set; }
            public decimal BillAmt { get; set; }
            public decimal RoundAmt { get; set; }
            public decimal GetAmt { get; set; }
            public decimal PayAmt { get; set; }
            public decimal ChangeAmt { get; set; }
            public DateTime? CreatedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public bool? IsLineActive { get; set; }
            public bool IsActive { get; set; }
        }

        public partial class PrintData {
            public int ID { get; set; }
            public string PrintID { get; set; }
            public string FormPrintID { get; set; }
            public string JsonData { get; set; }
            public System.DateTime PrintDate { get; set; }
            public string AppID { get; set; }
        }
        #endregion
    }
}
