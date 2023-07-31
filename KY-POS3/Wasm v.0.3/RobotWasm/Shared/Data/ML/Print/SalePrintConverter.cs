using RobotWasm.Shared.Data.DA;
using RobotWasm.Shared.Data.GaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.DA.POSFuncService;

namespace RobotWasm.Shared.Data.ML.Print {
    public class SalePrintConverter {

        public static I_POSSaleSetX Convert2Print(I_POSSaleSet input) {
            I_POSSaleSetX doc = new I_POSSaleSetX();
            doc.Head = ConvertHead2Model(input.Head);
            doc.Line = ConvertLine2Model(input.Line);
            doc.Payment = ConvertPayment2Model(input.Payment);
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

        public static POS_SaleHeadModelX ConvertHead2Model(POS_SaleHeadModel input) {

            POS_SaleHeadModelX h = new POS_SaleHeadModelX();
            h.RComID = input.RComID;
            h.ComID = input.ComID;
            h.BillID = input.BillID;
            h.INVID = input.INVID;
            h.FINVID = input.FINVID;
            h.DocTypeID = input.DocTypeID;
            h.MacNo = input.MacNo;
            h.MacTaxNo = input.MacTaxNo;
            h.BillDate = input.BillDate;
            h.BillRefID = input.BillRefID;
            h.INVPeriod = input.INVPeriod;
            h.ReasonID = input.ReasonID;
            h.TableID = input.TableID;
            h.TableName = input.TableName;
            h.CustomerID = input.CustomerID;
            h.CustomerName = input.CustomerName;
            h.CustAccGroup = input.CustAccGroup;
            h.CustTaxID = input.CustTaxID;
            h.CustBranchID = input.CustBranchID;
            h.CustBranchName = input.CustBranchName;
            h.CustAddr1 = input.CustAddr1;
            h.CustAddr2 = input.CustAddr2;
            h.IsVatRegister = input.IsVatRegister;
            h.POID = input.POID;
            h.ShipToLocID = input.ShipToLocID;
            h.ShipToLocName = input.ShipToLocName;
            h.ShipToUsePrice = input.ShipToUsePrice;
            h.ShipToAddr1 = input.ShipToAddr1;
            h.ShipToAddr2 = input.ShipToAddr2;
            h.SalesID1 = input.SalesID1;
            h.SalesID2 = input.SalesID2;
            h.SalesID3 = input.SalesID3;
            h.SalesID4 = input.SalesID4;
            h.SalesID5 = input.SalesID5;
            h.ContactTo = input.ContactTo;
            h.Currency = input.Currency;
            h.RateExchange = input.RateExchange;
            h.RateBy = input.RateBy;
            h.RateDate = input.RateDate;
            h.CreditTermID = input.CreditTermID;
            h.Qty = input.Qty;
            h.BaseNetTotalAmt = input.BaseNetTotalAmt;
            h.NetTotalAmt = input.NetTotalAmt;
            h.NetTotalVatAmt = input.NetTotalVatAmt;
            h.NetTotalAmtIncVat = input.NetTotalAmtIncVat;
            h.OntopDiscPer = input.OntopDiscPer;
            h.OntopDiscAmt = input.OntopDiscAmt;
            h.ItemDiscAmt = input.ItemDiscAmt;
            h.DiscCalBy = input.DiscCalBy;
            h.LineDisc = input.LineDisc;
            h.VatRate = input.VatRate;
            h.VatTypeID = input.VatTypeID;
            h.IsVatInPrice = input.IsVatInPrice;
            h.NetTotalAfterRound = input.NetTotalAfterRound;
            h.NetDiff = input.NetDiff;
            h.PayByCash = input.PayByCash;
            h.PayByCredit = input.PayByCredit;
            h.PayByOther = input.PayByOther;
            h.PayByVoucher = input.PayByVoucher;
            h.PayTotalAmt = input.PayTotalAmt;
            h.Remark1 = input.Remark1;
            h.Remark2 = input.Remark2;
            h.RemarkCancel = input.RemarkCancel;
            h.LocID = input.LocID;
            h.SubLocID = input.SubLocID;
            h.CountLine = input.CountLine;
            h.PayMethodCount = input.PayMethodCount;
            h.CountPay = input.CountPay;
            h.IsLink = input.IsLink;
            h.LinkBy = input.LinkBy;
            h.LinkDate = input.LinkDate;
            h.Status = input.Status;
            h.IsPrint = input.IsPrint;
            h.PrintDate = input.PrintDate;
            h.IsRecalStock = input.IsRecalStock;
            h.CreatedByApp = input.CreatedByApp;
            h.CreatedBy = input.CreatedBy;
            h.CreatedDate = input.CreatedDate;
            h.ModifiedBy = input.ModifiedBy;
            h.ModifiedDate = input.ModifiedDate;
            h.IsActive = input.IsActive;

            var shipToInfo = POSFuncService.ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault();
            if (shipToInfo != null) {
                h.ImageUrlShipTo = shipToInfo.ImageUrl;
            }

            return h;
        }


        public static List<POS_SaleLineModelX> ConvertLine2Model(List<POS_SaleLineModel> input) {

            List<POS_SaleLineModelX> result = new List<POS_SaleLineModelX>();

            foreach (var l in input) {
                POS_SaleLineModelX n = new POS_SaleLineModelX();

                n.RComID = l.RComID;
                n.ComID = l.ComID;
                n.MacNo = l.MacNo;
                n.LineNum = l.LineNum;
                n.BillID = l.BillID;
                n.RefLineNum = l.RefLineNum;
                n.RefLineUnq = l.RefLineUnq;
                n.INVID = l.INVID;
                n.FINVID = l.FINVID;
                n.LineUnq = l.LineUnq;
                n.BelongToLineNum = l.BelongToLineNum;
                n.DocTypeID = l.DocTypeID;
                n.BillDate = l.BillDate;
                n.BillRefID = l.BillRefID;
                n.CustID = l.CustID;
                n.TableID = l.TableID;
                n.TableName = l.TableName;
                n.ShipToLocID = l.ShipToLocID;
                n.ShipToLocName = l.ShipToLocName;
                n.Barcode = l.Barcode;
                n.ItemID = l.ItemID;
                n.ItemName = l.ItemName;
                n.ItemTypeID = l.ItemTypeID;
                n.ItemCateID = l.ItemCateID;
                n.ItemGroupID = l.ItemGroupID;
                n.IsStockItem = l.IsStockItem;
                n.Weight = l.Weight;
                n.WUnit = l.WUnit;
                n.Unit = l.Unit;
                n.Qty = l.Qty;
                n.Cost = l.Cost;
                n.Price = l.Price;
                n.PriceIncVat = l.PriceIncVat;
                n.BaseTotalAmt = l.BaseTotalAmt;
                n.TotalAmt = l.TotalAmt;
                n.VatAmt = l.VatAmt;
                n.TotalAmtIncVat = l.TotalAmtIncVat;
                n.VatRate = l.VatRate;
                n.VatTypeID = l.VatTypeID;
                n.OntopDiscAmt = l.OntopDiscAmt;
                n.OntopDiscPer = l.OntopDiscPer;
                n.DiscPer = l.DiscPer;
                n.DiscAmt = l.DiscAmt;
                n.DiscCalBy = l.DiscCalBy;
                n.IsFree = l.IsFree;
                n.ShareGpPer = l.ShareGpPer;
                n.IsOntopItem = l.IsOntopItem;
                n.PromotionID = l.PromotionID;
                n.PatternID = l.PatternID;
                n.PaternValue = l.PaternValue;
                n.MatchingNumber = l.MatchingNumber;
                n.ProPackCode = l.ProPackCode;
                n.IsProCompleted = l.IsProCompleted;
                n.LocID = l.LocID;
                n.SubLocID = l.SubLocID;
                n.LotNo = l.LotNo;
                n.SerialNo = l.SerialNo;
                n.BatchNo = l.BatchNo;
                n.Remark = l.Remark;
                n.Memo = l.Memo;
                n.ImgUrl = l.ImgUrl;
                n.Sort = l.Sort;
                n.KitchenFinishCount = l.KitchenFinishCount;
                n.Status = l.Status;
                n.CreatedBy = l.CreatedBy;
                n.CreatedDate = l.CreatedDate;
                n.ModifiedDate = l.ModifiedDate;
                n.IsLineActive = l.IsLineActive;
                n.IsActive = l.IsActive;

                n.ImageSource = "ghost.png";
                n.ImageUrl = n.ImageUrl;

                switch (l.Status.ToLower()) {
                    case "ok":
                        n.IsEditAble = true;
                        n.KitchenMessageLogo = "";
                        break;
                    case "k-accept":
                        n.IsEditAble = false;
                        if (Convert.ToInt32(l.Qty) - Convert.ToInt32(l.KitchenFinishCount) == 0) {
                            //ครัวทำเสร็จครบตาม order
                            n.KitchenMessageLogo = "/img/success01.png";
                        } else {
                            //ครัวทำเสร็จบางรายการ
                            n.KitchenMessageLogo = "/img/warning01.png";
                        }
                        break;
                    case "k-reject":
                        n.IsEditAble = false;
                        n.KitchenMessageLogo = "/img/delete01.png";
                        break;
                }
                result.Add(n);
            }

            return result;
        }

        public static List<POS_SalePaymentModelX> ConvertPayment2Model(List<POS_SalePaymentModel> input) {

            List<POS_SalePaymentModelX> result = new List<POS_SalePaymentModelX>();
            foreach (var l in input) {
                POS_SalePaymentModelX n = new POS_SalePaymentModelX();
                n.RComID = l.RComID;
                n.ComID = l.ComID;
                n.BillID = l.BillID;
                n.PaymentType = l.PaymentType;
                n.LineUnq = l.LineUnq;
                n.INVID = l.INVID;
                n.FINVID = l.FINVID;
                n.BillAmt = l.BillAmt;
                n.RoundAmt = l.RoundAmt;
                n.GetAmt = l.GetAmt;
                n.PayAmt = l.PayAmt;
                n.ChangeAmt = l.ChangeAmt;
                n.CreatedDate = l.CreatedDate;
                n.ModifiedDate = l.ModifiedDate;
                n.IsLineActive = l.IsLineActive;
                n.IsActive = l.IsActive;
                result.Add(n);
            }

            return result;
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
            public decimal? PayByCredit { get; set; }
            public decimal PayByOther { get; set; }
            public decimal? PayByVoucher { get; set; }
            public decimal PayTotalAmt { get; set; }
            public string Remark1 { get; set; }
            public string Remark2 { get; set; }
            public string RemarkCancel { get; set; }
            public string LocID { get; set; }
            public string SubLocID { get; set; }
            public int CountLine { get; set; }
            public int PayMethodCount { get; set; }
            public int? CountPay { get; set; }
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
            public bool IsActive { get; set; }
            public String KitchenMessageLogo { get; set; }
            public bool IsEditAble { get; set; }
            public String ImageUrl { get; set; }
            public string ImageSource { get; set; }

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
