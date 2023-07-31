
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using Dapper;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.ML.Shared;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Data.DA.GenDoc;
using RobotWasm.Shared.Data.DA;
using static RobotWasm.Shared.Data.DA.POSFuncService;

namespace RobotWasm.Server.Data.DA.POS {
    public class POSService {

        #region Convert Class

        // Convert Head
        public static POS_SaleHeadModel ConvertHead2Model(POS_SaleHead input) {

            POS_SaleHeadModel h = new POS_SaleHeadModel();
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
            h.ImageUrlShipTo = "frontstore.png";

            var shipToInfo = POSFuncService.ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault();
            if (shipToInfo != null) {
                h.ImageUrlShipTo = shipToInfo.ImageUrl;
            }

            return h;
        }
        

        public static POS_SaleHead ConvertHead2DB(POS_SaleHeadModel input) {

            POS_SaleHead h = new POS_SaleHead();
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
            return h;
        }

        // End Convert Head

        // Convert Line

        public static List<POS_SaleLine> ConvertLine2DB(List<POS_SaleLineModel> input) {
            List<POS_SaleLine> result = new List<POS_SaleLine>();

            foreach (var l in input) {
                POS_SaleLine n = new POS_SaleLine();

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
                result.Add(n);
            }

            return result;
        }


        public static List<POS_SaleLineModel> ConvertLine2Model(List<POS_SaleLine> input) {

            List<POS_SaleLineModel> result = new List<POS_SaleLineModel>();

            foreach (var l in input) {
                POS_SaleLineModel n = new POS_SaleLineModel();

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

        // End Convert Line


        // Convert Payment
        public static List<POS_SalePayment> ConvertPayment2DB(List<POS_SalePaymentModel> input) {

            List<POS_SalePayment> result = new List<POS_SalePayment>();
            foreach (var l in input) {
                POS_SalePayment n = new POS_SalePayment();
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

        public static List<POS_SalePaymentModel> ConvertPayment2Model(List<POS_SalePayment> input) {

            List<POS_SalePaymentModel> result = new List<POS_SalePaymentModel>();
            foreach (var l in input) {
                POS_SalePaymentModel n = new POS_SalePaymentModel();
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

        // End Convert Payment

        #endregion

        #region Query Transaction

        public static I_POSSaleSet GetDocSet(string docid, string rcom) {
            I_POSSaleSet n = new I_POSSaleSet();
            using (GAEntities db = new GAEntities()) {
                var head = db.POS_SaleHead.Where(o => o.BillID == docid && o.RComID == rcom).FirstOrDefault();
                var line = db.POS_SaleLine.Where(o => o.BillID == docid && o.RComID == rcom).OrderBy(o => o.LineNum).ToList();
                var pay = db.POS_SalePayment.Where(o => o.BillID == docid && o.RComID == rcom).ToList();
                n.Head = ConvertHead2Model(head);
                n.Line = ConvertLine2Model(line);
                n.Payment = ConvertPayment2Model(pay);
            }
            return n;
        }

        public static POS_SaleHead GetBill(string billId) {
            POS_SaleHead result = new POS_SaleHead();
            using (GAEntities db = new GAEntities()) {
                result = db.POS_SaleHead.Where(o => o.BillID == billId).FirstOrDefault();
            }
            return result;
        }

        public static List<POS_SaleHeadModel> ListPendingCheckBill(string rcom, string com, string macno = "") {
            List<POS_SaleHeadModel> result = new List<POS_SaleHeadModel>();

            using (GAEntities db = new GAEntities()) {
                var query = db.POS_SaleHead.Where(o =>
                                                            o.ComID == com
                                                            && o.RComID == rcom
                                                            && o.INVID == ""
                                                            && (o.MacNo == macno || macno == "")
                                                            && o.IsActive == true
                                                            ).OrderBy(o => o.MacNo).OrderByDescending(o => o.CreatedDate).ToList();
                foreach (var q in query) {
                    var qq = ConvertHead2Model(q);
                    result.Add(qq);
                }
            }
            return result;
        }

        public static List<POS_SaleHead> GetSummaryDashBoard(string rcom, string com) {
            List<POS_SaleHead> result = new List<POS_SaleHead>();
            var today = DateTime.Now.Date;
            using (GAEntities db = new GAEntities()) {
                result = db.POS_SaleHead.Where(o =>
                                                            o.ComID == com
                                                            && o.RComID == rcom
                                                            && o.INVID != ""
                                                            && o.IsActive == true
                                                            && o.BillDate == today
                                                            ).OrderBy(o => o.MacNo).OrderByDescending(o => o.CreatedDate).ToList();
            }
            return result;
        }

        public static List<POS_SaleHeadModel> ListBill(I_BillFilterSet f) {
            List<POS_SaleHeadModel> result = new List<POS_SaleHeadModel>();

            using (GAEntities db = new GAEntities()) {
                var query = new List<POS_SaleHead>();

                if (f.Search != "") {//search แบบระบุคำเสริช
                    query = db.POS_SaleHead.Where(o =>
                                            (o.INVID.Contains(f.Search)
                                                || o.BillID.Contains(f.Search)
                                                || o.CustBranchName.Contains(f.Search)
                                                || o.CustBranchID.Contains(f.Search)
                                                || o.TableID.Contains(f.Search)
                                              )
                                                //&& (o.ComID == f.ComID)
                                                && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                && o.RComID == f.RComID
                                                && o.INVID != ""
                                                && f.comIDs.Contains(o.ComID)
                                                && o.IsActive == true
                                                ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                } else {//search แบบระบุวันที่
                    query = db.POS_SaleHead.Where(o =>
                                                        (o.BillDate >= f.Begin && o.BillDate <= f.End)
                                                        && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                        && o.RComID == f.RComID
                                                        && o.INVID != ""
                                                   //&& (o.ComID == f.ComID)
                                                   && f.comIDs.Contains(o.ComID)
                                                && o.IsActive == true
                                            ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                }

                foreach (var q in query) {
                    var qq = ConvertHead2Model(q);
                    result.Add(qq);
                }
            }
            return result;
        }

        //public static List<POS_SaleHeadModel> ListBill_Online(I_BillFilterSetMobile f) {
        //    List<POS_SaleHeadModel> result = new List<POS_SaleHeadModel>();

        //    using (GAEntities db = new GAEntities()) {
        //        var query = new List<POS_SaleHead>();

        //        if (f.Search != "") {//search แบบระบุคำเสริช
        //            query = db.POS_SaleHead.Where(o =>
        //                                    (o.INVID.Contains(f.Search)
        //                                        || o.BillID.Contains(f.Search)
        //                                        || o.CustBranchName.Contains(f.Search)
        //                                        || o.CustBranchID.Contains(f.Search)
        //                                        || o.TableID.Contains(f.Search)
        //                                      )
        //                                      && (o.MacNo == f.MacNo || f.MacNo == "")
        //                                        && (o.ComID == f.ComID)
        //                                        && (o.ShipToLocID == f.ShipTo || f.ShipTo == "X")
        //                                        && o.RComID == f.RComID
        //                                        && o.INVID != ""
        //                                        && o.IsActive == true
        //                                        ).Skip(f.Skip).Take(f.Take).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
        //        } else {//search แบบระบุวันที่
        //            query = db.POS_SaleHead.Where(o =>
        //                                                (o.BillDate >= f.Begin && o.BillDate <= f.End)
        //                                                && (o.ShipToLocID == f.ShipTo || f.ShipTo == "X")
        //                                                && o.RComID == f.RComID
        //                                                && (o.MacNo == f.MacNo || f.MacNo == "")
        //                                                 && o.INVID != ""
        //                                           && (o.ComID == f.ComID)
        //                                        && o.IsActive == true
        //                                    ).Skip(f.Skip).Take(f.Take).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
        //        }

        //        foreach (var q in query) {
        //            var qq = ConvertHead2Model(q);

        //            result.Add(qq);
        //        }
        //    }
        //    return result;
        //}

        public static List<POS_SaleLog> ListPOS_SaleLog(string rcom, string com, string billId) {
            List<POS_SaleLog> result = new List<POS_SaleLog>();

            using (GAEntities db = new GAEntities()) {

                result = db.POS_SaleLog.Where(o => o.RComID == rcom && o.ComID == com && o.BillID == billId
                                        ).OrderByDescending(o => o.SaveNo).ThenBy(o => o.LineNum).ToList();
                return result;
            }
        }

        //public List<POS_SaleLineModel> ListDocKitChen(string rcom, string com, int skip, int take, DateTime DateFr) {
        //    List<POS_SaleLineModel> result = new List<POS_SaleLineModel>();

        //    using (GAEntities db = new GAEntities()) {
        //        var query = new List<POS_SaleLine>();
        //        query = db.POS_SaleLine.Where(o =>
        //                                            (o.BillDate >= DateFr && o.BillDate <= DateTime.Now.Date)
        //                                            && o.RComID == rcom
        //                                       //&& comlist.Contains(o.ComID)
        //                                       && (o.ComID == com || com == "")
        //                                       && o.Status == "OK"
        //                                       && o.ItemTypeID != "DISCOUNT"
        //                                    && o.IsActive == true
        //                                ).OrderByDescending(o => o.BillDate).ThenBy(o => o.BillID).Skip(skip).Take(take).ToList();
        //        var qq = ConvertLine2Model(query);
        //        result.AddRange(qq);
        //    }
        //    return result;
        //}


        public List<vw_POS_SaleHead> ListCancelBIll(string rcom, string com, DateTime begain, DateTime end) {
            List<vw_POS_SaleHead> result = new List<vw_POS_SaleHead>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_SaleHead.Where(o => (o.ComID == com || com == "")
                                                             && o.RComID == rcom
                                                            && o.BillDate >= begain && o.BillDate <= end
                                                            && o.IsActive == false).ToList();
            }
            return result;
        }

        public List<vw_POS_SaleHead> ListDoc(I_BillFilterSet f) {
            List<vw_POS_SaleHead> result = new List<vw_POS_SaleHead>();

            using (GAEntities db = new GAEntities()) {
                if (f.Search != "") {//search แบบระบุคำเสริช
                    result = db.vw_POS_SaleHead.Where(o =>
                                            (o.INVID.Contains(f.Search)
                                                || o.BillID.Contains(f.Search)
                                                || o.CustBranchName.Contains(f.Search)
                                                || o.CustBranchID.Contains(f.Search)
                                                || o.TableID.Contains(f.Search)
                                              )
                                                && (o.ComID == f.ComID || f.ComID == "")
                                                && (o.MacNo == f.MacNo || f.MacNo == "")
                                                && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                && o.RComID == f.RComID
                                                && f.comIDs.Contains(o.ComID)
                                                && o.IsActive == f.ShowActive
                                                ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                } else {//search แบบระบุวันที่
                    result = db.vw_POS_SaleHead.Where(o =>
                                                        (o.BillDate >= f.Begin && o.BillDate <= f.End)
                                                        && o.RComID == f.RComID
                                                     && (o.MacNo == f.MacNo || f.MacNo == "")
                                                     && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                     && f.comIDs.Contains(o.ComID)
                                                   && (o.ComID == f.ComID || f.ComID == "")
                                                && o.IsActive == f.ShowActive
                                            ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();

                }
            }
            return result;
        }

        #endregion

        #region OrderForKitchen
        public static List<POS_SaleLineModel> ListOrderForKitchen(KitchenStatusParam input) {
            var result = new List<POS_SaleLineModel>();

            using (GAEntities db = new GAEntities()) {
                var query = db.POS_SaleLine.Where(o => (
                                                    o.RComID == input.rcom
                                                    && o.ComID == input.comid
                                                    && o.IsActive == true
                                                    && o.IsOntopItem == false
                                                    && o.IsLineActive == true
                                                    && o.Status.ToUpper() == "OK"
                                                    && input.cates.Contains(o.ItemCateID)
                                                )).OrderBy(o => o.ModifiedDate).Take(200).ToList();

                    result.AddRange(ConvertLine2Model(query));
            }
            return result;
        }

        public static I_BasicResult UpdatePOSLineStatus(KitchenStatusParam input) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_SaleHead.Where(o => o.BillID == input.billId && o.RComID == input.rcom && o.ComID == input.comid).FirstOrDefault();
                    head.IsLink = false;
                    head.ModifiedBy = input.Username;
                    head.ModifiedDate = DateTime.Now;
                    var line = db.POS_SaleLine.Where(o => o.BillID == input.billId && o.RComID == input.rcom && o.LineUnq == input.lineunq && o.ComID == input.comid).FirstOrDefault();
                    var ontop = db.POS_SaleLine.Where(o => o.BillID == input.billId && o.RComID == input.rcom && o.RefLineUnq == input.lineunq && o.ComID == input.comid).ToList();
                    if (line != null) {
                        if (line.Qty == 0) {
                            r.Result = "fail";
                            r.Message1 = "รายการถูกยกเลิกโดยผู้รับออเดอร์";
                            return r;
                        }
                        if (input.status.ToUpper() == "K-ACCEPT" || input.status.ToUpper() == "K-ACCEPT-ALL") {//"K-ACCPET-ALL" คือกดเสร็จทั้งหมด
                            if (input.status.ToUpper() == "K-ACCEPT-ALL") {
                                line.KitchenFinishCount = Convert.ToInt32(line.Qty);
                            } else {
                                line.KitchenFinishCount = line.KitchenFinishCount + 1;
                            }

                            if (line.Qty == line.KitchenFinishCount) {
                                line.Status = "K-ACCEPT";
                            }

                        } else {//"K-REJECT"
                            if (line.KitchenFinishCount > 0) {//ถ้าครัวทำเสร็จบางรายการแล้วมากดยกเลิก
                                line.Status = "K-ACCEPT";
                            } else {
                                line.Status = input.status.ToUpper();
                            }
                        }
                        line.ModifiedDate = DateTime.Now;
                        foreach (var o in ontop) {
                            o.Status = line.Status;
                            o.ModifiedDate = line.ModifiedDate;
                        }
                    }

                    db.SaveChanges();
                    var r2 = POSService.POS_SaleRefresh(head);

                    if (r2.Result == "fail") {
                        r.Result = r2.Result;
                        r.Message1 = r2.Message1;
                    }
                    //FireBaseService.Sendnotify(head.RComID, head.ComID);
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;

        }

        #endregion

        #region save

        public static I_POSSaleSet GenNumberOrderID(I_POSSaleSet doc) {
            try {
                var shortShiptoId = ListShipTo()?.Where(o => o.ShipToID == doc.Head.ShipToLocID)?.FirstOrDefault()?.ShortID ?? "";
                doc.Head.BillID = IDRuunerService.GenPOSSaleID("ORDER",doc.Head.RComID,doc.Head.ComID,doc.Head.MacNo,shortShiptoId,false,doc.Head.BillDate)[1];
                doc = CheckDupBillID(doc);
            } catch (Exception ex) {
            }
            return doc;
        }

        public static I_POSSaleSet CheckDupBillID(I_POSSaleSet doc) {
            try {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = doc.Head;

                using (GAEntities db = new GAEntities()) {
                    var shortShiptoId = ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault().ShortID;
                    //var comInfo = CompanyService.GetComInfoByComID(h.RComID, h.ComID);
                    var get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.BillID == h.BillID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            r = new I_BasicResult { Result = "fail", Message1 = "Cannot create bill no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GenPOSSaleID("ORDER", h.RComID, h.ComID, h.MacNo, shortShiptoId, true, h.BillDate);

                        h.BillID = IDRuunerService.GenPOSSaleID("ORDER", h.RComID, h.ComID, h.MacNo, shortShiptoId, false, h.BillDate)[1];
                        get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.BillID == h.BillID).FirstOrDefault();
                    }
                }
            } catch (Exception ex) {
            }
            return doc;
        }

        public static I_POSSaleSet GenNumberInvoiceID(I_POSSaleSet doc) {
            try {
                var shortShiptoId = ListShipTo()?.Where(o => o.ShipToID == doc.Head.ShipToLocID)?.FirstOrDefault()?.ShortID ?? "";
                doc.Head.INVID = IDRuunerService.GenPOSSaleID("INV", doc.Head.RComID, doc.Head.ComID, doc.Head.MacNo, shortShiptoId, false, doc.Head.BillDate)[1];
                doc = CheckDupInvoiceID(doc);
            } catch (Exception ex) {
            }
            return doc;
        }

        public static I_POSSaleSet CheckDupInvoiceID(I_POSSaleSet doc) {
            try {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = doc.Head;

                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.INVID == h.INVID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            r = new I_BasicResult { Result = "fail", Message1 = "Cannot create invoice no", Message2 = "" };
                            break;
                        }
                        i++;
                        var shortShiptoId = ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault().ShortID;
                        //var comInfo = CompanyService.GetComInfoByComID(h.RComID, h.ComID);
                        IDRuunerService.GenPOSSaleID("INV", h.RComID, h.ComID,"Z", shortShiptoId, true, h.BillDate);

                        h.INVID = IDRuunerService.GenPOSSaleID("INV", h.RComID, h.ComID, "Z", shortShiptoId, false, h.BillDate)[1];
                        get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.INVID == h.INVID).FirstOrDefault();
                    }
                }
            } catch (Exception ex) {
            }
            return doc;
        }

        public static I_BasicResult Save(I_POSSaleSet doc,string action) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.Head;
            doc = CalDocSet(doc);
            try {
                using (GAEntities db = new GAEntities()) {
                        if(action == "insert") {

                        if (string.IsNullOrEmpty(h.BillID)) {
                            var shortShiptoId = ListShipTo()?.Where(o => o.ShipToID == doc.Head.ShipToLocID)?.FirstOrDefault()?.ShortID ?? "";
                            doc.Head.BillID = IDRuunerService.GenPOSSaleID("ORDER", doc.Head.RComID, doc.Head.ComID, doc.Head.MacNo, shortShiptoId, false, doc.Head.BillDate)[1];
                            doc = CheckDupBillID(doc);
                            r.Message2 = doc.Head.BillID;
                        }
                        doc = CalDocSet(doc);

                        db.POS_SaleHead.Add(doc.Head);
                        db.POS_SaleLine.AddRange(doc.Line);
                        db.POS_SalePayment.AddRange(doc.Payment);
                        db.SaveChanges();

                        var rr = CalStock(h);
                        if (rr.Result == "fail") {
                            r.Result = rr.Result;
                            r.Message1 = rr.Message1;
                        }
                        var r2 = POS_SaleRefresh(doc.Head);
                        if (r2.Result == "fail") {
                            r.Result = r2.Result;
                            r.Message1 = r2.Message1;
                        }

                    } else {

                        if (string.IsNullOrEmpty(h.INVID) && doc.Payment.Count > 0) {
                            var shortShiptoId = ListShipTo()?.Where(o => o.ShipToID == doc.Head.ShipToLocID)?.FirstOrDefault()?.ShortID ?? "";
                            doc.Head.INVID = IDRuunerService.GenPOSSaleID("INV", doc.Head.RComID, doc.Head.ComID, doc.Head.MacNo, shortShiptoId, false, doc.Head.BillDate)[1];
                            doc = CheckDupInvoiceID(doc);
                            r.Message2 = doc.Head.INVID;
                        }

                        var uh = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID).FirstOrDefault();
                        uh.BillID = doc.Head.BillID;
                        uh.INVID = uh.INVID == "" ? doc.Head.INVID : uh.INVID;
                        uh.FINVID = doc.Head.FINVID;
                        uh.DocTypeID = doc.Head.DocTypeID;
                        uh.MacNo = doc.Head.MacNo;
                        uh.MacTaxNo = doc.Head.MacTaxNo;
                        uh.BillDate = doc.Head.BillDate;
                        uh.BillRefID = doc.Head.BillRefID;
                        uh.INVPeriod = doc.Head.INVPeriod;
                        uh.ReasonID = doc.Head.ReasonID;
                        uh.TableID = doc.Head.TableID;
                        uh.TableName = doc.Head.TableName;
                        uh.CustomerID = doc.Head.CustomerID;
                        uh.CustomerName = doc.Head.CustomerName;
                        uh.CustAccGroup = doc.Head.CustAccGroup;
                        uh.CustTaxID = doc.Head.CustTaxID;
                        uh.CustBranchID = doc.Head.CustBranchID;
                        uh.CustBranchName = doc.Head.CustBranchName;
                        uh.CustAddr1 = doc.Head.CustAddr1;
                        uh.CustAddr2 = doc.Head.CustAddr2;
                        uh.IsVatRegister = doc.Head.IsVatRegister;
                        uh.POID = doc.Head.POID;
                        uh.ShipToLocID = doc.Head.ShipToLocID;
                        uh.ShipToLocName = doc.Head.ShipToLocName;
                        uh.ShipToAddr1 = doc.Head.ShipToAddr1;
                        uh.ShipToAddr2 = doc.Head.ShipToAddr2;
                        uh.SalesID1 = doc.Head.SalesID1;
                        uh.SalesID2 = doc.Head.SalesID2;
                        uh.SalesID3 = doc.Head.SalesID3;
                        uh.SalesID4 = doc.Head.SalesID4;
                        uh.SalesID5 = doc.Head.SalesID5;
                        uh.ContactTo = doc.Head.ContactTo;
                        uh.Currency = doc.Head.Currency;
                        uh.RateExchange = doc.Head.RateExchange;
                        uh.RateBy = doc.Head.RateBy;
                        uh.RateDate = doc.Head.RateDate;
                        uh.CreditTermID = doc.Head.CreditTermID;
                        uh.Qty = doc.Head.Qty;
                        uh.BaseNetTotalAmt = doc.Head.BaseNetTotalAmt;
                        uh.NetTotalAmt = doc.Head.NetTotalAmt;
                        uh.NetTotalVatAmt = doc.Head.NetTotalVatAmt;
                        uh.NetTotalAmtIncVat = doc.Head.NetTotalAmtIncVat;
                        uh.OntopDiscPer = doc.Head.OntopDiscPer;
                        uh.OntopDiscAmt = doc.Head.OntopDiscAmt;
                        uh.ItemDiscAmt = doc.Head.ItemDiscAmt;
                        uh.DiscCalBy = doc.Head.DiscCalBy;
                        uh.LineDisc = doc.Head.LineDisc;
                        uh.VatRate = doc.Head.VatRate;
                        uh.VatTypeID = doc.Head.VatTypeID;
                        uh.IsVatInPrice = doc.Head.IsVatInPrice;
                        uh.NetTotalAfterRound = doc.Head.NetTotalAfterRound;
                        uh.NetDiff = doc.Head.NetDiff;
                        uh.PayByCash = doc.Head.PayByCash;
                        uh.PayByOther = doc.Head.PayByOther;
                        uh.PayByVoucher = doc.Head.PayByVoucher;
                        uh.PayByCredit = doc.Head.PayByCredit;
                        uh.PayTotalAmt = doc.Head.PayTotalAmt;
                        uh.PayMethodCount = doc.Head.PayMethodCount;
                        uh.Remark1 = doc.Head.Remark1;
                        uh.Remark2 = doc.Head.Remark2;
                        uh.RemarkCancel = doc.Head.RemarkCancel;
                        uh.LocID = doc.Head.LocID;
                        uh.SubLocID = doc.Head.SubLocID;
                        uh.CountLine = doc.Head.CountLine;
                        uh.IsLink = doc.Head.IsLink;
                        uh.LinkBy = doc.Head.LinkBy;
                        uh.LinkDate = doc.Head.LinkDate;
                        uh.Status = doc.Head.Status;
                        uh.IsPrint = doc.Head.IsPrint;
                        uh.PrintDate = doc.Head.PrintDate;
                        uh.ModifiedBy = doc.Head.ModifiedBy;
                        uh.ModifiedDate = DateTime.Now;

                        db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID));
                        db.POS_SaleLine.AddRange(doc.Line);
                        db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID));
                        db.POS_SalePayment.AddRange(doc.Payment);
                        db.SaveChanges();
                        var rr = CalStock(h);
                        if (rr.Result == "fail") {
                            r.Result = rr.Result;
                            r.Message1 = rr.Message1;
                        }
                        var r2 = POS_SaleRefresh(uh);
                        if (r2.Result == "fail") {
                            r.Result = r2.Result;
                            r.Message1 = r2.Message1;
                        }
                    }
                }
                Save_SaleLog(doc);
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        public static I_BasicResult Save_SaleLog(I_POSSaleSet doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            int saveNo = 1;
            try {
                using (GAEntities db = new GAEntities()) {
                    var getLatestSaveNo = db.POS_SaleLog.Where(o => o.RComID == doc.Head.RComID && o.ComID == doc.Head.ComID && o.BillID == doc.Head.BillID).OrderByDescending(o => o.SaveNo).FirstOrDefault();
                    if (getLatestSaveNo != null) {
                        saveNo = getLatestSaveNo.SaveNo + 1;
                    }
                    foreach (var l in doc.Line) {
                        POS_SaleLog n = new POS_SaleLog {
                            RComID = l.RComID,
                            SaveNo = saveNo,
                            LineUnq = l.LineUnq,
                            BillID = l.BillID,
                            ComID = l.ComID,
                            CreatedBy = l.CreatedBy,
                            CreatedByApp = "webv2",
                            CreatedDate = DateTime.Now,
                            IsActive = l.IsActive,
                            ItemID = l.ItemID,
                            ItemName = l.ItemName,
                            LineNum = l.LineNum,
                            MacNo = l.MacNo,
                            Price = l.Price,
                            Qty = l.Qty,
                            UploadedDate = DateTime.Now,
                            TotalAmt = l.TotalAmt
                        };
                        db.POS_SaleLog.Add(n);
                    }
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        public static I_BasicResult SaveTaxSlip(I_POSSaleSet doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var h = doc.Head;
            try {
                using (GAEntities db = new GAEntities()) {
                    doc = CalDocSet(doc);
                    var uh = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID).FirstOrDefault();
                    var shortShiptoId = ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault().ShortID;
                    if (string.IsNullOrEmpty(doc.Head.FINVID)) {
                        uh.FINVID = IDRuunerService.GenPOSSaleID("TAX", h.RComID, h.ComID,"Z", shortShiptoId, false, h.BillDate)[1];
                    }

                    uh.CustomerName = h.CustomerName;
                    uh.CustBranchName = h.CustBranchName;
                    uh.CustAddr1 = h.CustAddr1;
                    uh.CustAddr2 = h.CustAddr2;
                    uh.CustTaxID = h.CustTaxID;
                    uh.ModifiedBy = h.ModifiedBy;
                    uh.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    IDRuunerService.GenPOSSaleID("TAX", h.RComID, h.ComID,"Z", shortShiptoId, true, h.BillDate);
                    var r2 = POS_SaleRefresh(uh);
                    if (r2.Result == "fail") {
                        r.Result = r2.Result;
                        r.Message1 = r2.Message1;
                    } else {
                        r.Message2 = uh.FINVID;
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        public static I_BasicResult POS_SaleRefresh(POS_SaleHead data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    POS_SaleRefresh n = new POS_SaleRefresh();
                    n.BillID = data.BillID;
                    n.ComID = data.ComID;
                    n.RComID = data.RComID;
                    n.MacID = data.MacNo;
                    n.LatestModifiedDate = data.CreatedDate;
                    if (data.ModifiedDate != null) {
                        n.LatestModifiedDate = Convert.ToDateTime(data.ModifiedDate);
                    }
                    var exist = db.POS_SaleRefresh.Where(o => o.BillID == data.BillID && o.RComID == data.RComID).FirstOrDefault();
                    if (exist == null) {
                        db.POS_SaleRefresh.Add(n);
                    } else {
                        exist.LatestModifiedDate = n.LatestModifiedDate;
                    }
                    db.SaveChanges();
                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        public I_BasicResult UpdateStatus_SaleLine(int ID, string status) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    var ul = db.POS_SaleLine.Where(o => o.ID == ID && o.IsActive == true).FirstOrDefault();
                    ul.Status = status;
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        #endregion

        #region Delete

        public static I_BasicResult DeleteDoc(string docId,string rcom ,string modifiedby, string remark) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {

                    var head = db.POS_SaleHead.Where(o => o.BillID == docId && o.RComID == rcom).FirstOrDefault();

                    head.ModifiedBy = modifiedby;
                    head.ModifiedDate = DateTime.Now;
                    head.Remark1 = remark;
                    head.IsActive = false;
                    db.POS_SaleLine.Where(o => o.BillID == docId && o.RComID == rcom).ToList().ForEach(o => o.IsActive = false);
                    db.POS_SalePayment.Where(o => o.BillID == docId && o.RComID == rcom).ToList().ForEach(o => o.IsActive = false);
                    db.SaveChanges();
                    var r2 = POS_SaleRefresh(head);
                    CalStock(head);
                    //FireBaseService.Sendnotify(head.RComID, head.ComID);
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }


        public I_BasicResult DeletePermanent(LoginSet login, string storeId, string macno, string shiptoId, DateTime transDate, int billDeleteQty) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                var comInfo = login.CurrentCompany;
                var shortShiptoId = ListShipTo().Where(o => o.ShipToID == shiptoId).FirstOrDefault().ShortID;
                string year = DatetimeInfoService.Convert2ThaiYear(transDate).ToString();
                year = year.Substring(year.Length - 2);
                string month = transDate.Month.ToString("00");
                string day = transDate.Day.ToString("00");

                string prefix = shortShiptoId + comInfo.ShortCode + macno + year + month + day + "-";
                var rcom = login.CurrentRootCompany.CompanyID;

                using (GAEntities db = new GAEntities()) {

                    for (int i = 1; i <= billDeleteQty; i++) {
                        //var sh = db.POS_SaleHead.Where(o => o.ShipToLocID == shiptoId && o.MacNo == macno && o.BillDate == transDate && o.ComID == storeId && o.RComID == rcom && o.INVID!="" ).OrderByDescending(o => o.INVID).FirstOrDefault();
                        var sh = db.POS_SaleHead.Where(o => o.INVID.StartsWith(prefix) && o.ComID == storeId && o.RComID == rcom).OrderByDescending(o => o.INVID).FirstOrDefault();
                        if (sh == null) {
                            break;
                        } else {
                            if (!string.IsNullOrEmpty(sh.FINVID)) {
                                break;
                            } else {
                                db.POS_SaleHead.RemoveRange(db.POS_SaleHead.Where(o => o.BillID == sh.BillID && o.RComID == rcom && o.ComID == storeId));
                                db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == sh.BillID && o.RComID == rcom && o.ComID == storeId));
                                db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == sh.BillID && o.RComID == rcom && o.ComID == storeId));
                                db.SaveChanges();

                                var runing = Convert.ToInt32(sh.INVID.Substring(sh.INVID.Length - 3));
                                runing = (runing - 1) < 1 ? 1 : runing;
                                var idRecord = db.IDGenerator.Where(o => o.Prefix == prefix && o.ComID == sh.ComID && o.RComID == rcom).FirstOrDefault();
                                if (idRecord != null) {
                                    idRecord.DigitRunNumber = runing;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        #endregion

        #region RefreshBill        

        async public Task<I_BasicResult> RefreshBill(DateTime begin, DateTime end,LoginSet login) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = login.CurrentRootCompany.CompanyID;
                //var h = doc.Head;
                using (GAEntities db = new GAEntities()) {
                    var bills = db.POS_SaleHead.Where(o => o.IsActive == true
                                                             && o.INVID != ""
                                                             && (o.BillDate >= begin && o.BillDate <= begin)
                                                             && (o.NetTotalAfterRound != o.PayTotalAmt)
                                                             && o.RComID == rcom
                                                            ).Select(o => o.BillID).ToList();
                    r.Message2 = "Found Missing bill " + bills.Count().ToString("n0") + " for recalculate.";
                    foreach (var b in bills) {
                        var doc = await Task.Run(() => GetDocSet(b, rcom));
                        var rr = Save(doc,"");
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        public static I_BasicResult CalStock(POS_SaleHead h) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //var h = doc.Head;

                if (h.INVID != "") {
                    using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                        //var dynamicParameters = new DynamicParameters();
                        //dynamicParameters.Add("BillID", h.BillID);
                        var strSQL = "exec [SP_CalStkSaleMove] @docid, @doctype, @rcompany, @company";
                        var values = new { docid = h.BillID, doctype = h.DocTypeID, rcompany = h.RComID, company = h.ComID };
                        var rquery = connection.Query(strSQL, values);

                    }

                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        public static I_BasicResult CalStockEndDay(DateTime beign, DateTime end, string rcom, string com, int forceRepeat) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                    var strSQL = "exec [SP_CalStkEndDay] @beign, @end, @rcom, @com,@forceRepeat";
                    var values = new { beign = beign, end = end, rcom = rcom, com = com, forceRepeat = forceRepeat };
                    var rquery = connection.Query(strSQL, values);
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        #endregion

        #region ListData

        public static List<POSMenuItem> ListMenuItem(string rcom, string comId, string custId) {
            ///ImageStorage/BPR/ITEMS_PHOTO_PROFILE/Thumb/A001.jpg
          //  string rootappurl = GetRootApp(@"/ImageStorage/BPR/ITEMS_PHOTO_PROFILE/Thumb/");
            List<POSMenuItem> result = new List<POSMenuItem>();

            List<string> useType = new List<string> { "FG", "DISCOUNT" };
            var today = DateTime.Now.Date;
            using (GAEntities db = new GAEntities()) {
                var item_price = db.ItemPriceInfo.Where(o =>
                                                            (o.CompanyID == comId || o.CompanyID == "")
                                                            && o.CustID == custId
                                                            && o.RCompanyID == rcom
                                                            && o.DateBegin <= today && o.DateEnd >= today
                                                            && o.IsActive
                                                            ).ToList();

                var query = db.vw_ItemInfoWithPhoto.Where(o =>
                                          new List<string> { "FG", "DISCOUNT" }.Contains(o.TypeID)
                                            && o.RCompanyID == rcom
                                            && o.IsActive).OrderBy(o => o.Name1).ToList();
                foreach (var q in query) {
                    var price1 = item_price.Where(o => o.ItemID == q.ItemID).ToList();
                    if (price1.Count == 0 && q.TypeID != "DISCOUNT") {// ไม่มี config ราคาและ ไม่ใช่ส่วนลด ไม่ต้องเพิ่มในรายการสินค้า
                        continue;
                    }

                    var price = new ItemPriceInfo();
                    //Step1 get promotion price with this store
                    var chkProPirceThisStore = price1.Where(o => o.UseLevel == 1 && o.CompanyID != "").FirstOrDefault();
                    //Step2 get promotion price with all store
                    var chkProPirceAllStore = price1.Where(o => o.UseLevel == 1 && o.CompanyID == "").FirstOrDefault();
                    //Step3 get regular price with  this store
                    var chkRegPirceThisStore = price1.Where(o => o.UseLevel == 0 && o.CompanyID != "").FirstOrDefault();
                    //Step4 get regular price with  all store
                    var chkRegPirceAllStore = price1.Where(o => o.UseLevel == 0 && o.CompanyID == "").FirstOrDefault();

                    if (chkProPirceThisStore != null) {//pro price with this store ใช้อันนี้ก่อน
                        price = chkProPirceThisStore;
                    } else if (chkProPirceAllStore != null) {//ถ้าไม่มี pro price with this store ใช้ pro price with all store
                        price = chkProPirceAllStore;
                    } else if (chkRegPirceThisStore != null) {// ถ้าไม่มี pro price with all store ใช้  regular price with  this store
                        price = chkRegPirceThisStore;
                    } else {//ท้ายที่สุดไม่มีอะไรให้ใช้ราคา get regular price with  all store
                        price = chkRegPirceAllStore;
                    }
                    POSMenuItem n = new POSMenuItem();
                    n.ItemID = q.ItemID;
                    n.Name = q.Name1;
                    //n.GroupName = q.Group1Name;
                    n.CateID = q.CateID;
                    n.Company = q.CompanyID;
                    n.CustId = q.CateID;
                    n.TypeID = q.TypeID;
                    n.GroupID = q.Group1ID;
                    n.TypeName = q.TypeName;
                    n.IsStockItem = q.IsKeepStock;
                    n.AccGroup = q.GLGroupID;
                    n.SellQty = 0;
                    n.SellAmt = 0;
                    n.WUnit = q.UnitID;
                    n.Unit = q.UnitID;
                    n.Price = price == null ? 0 : price.Price;
                    n.PriceTaxCondType = price == null ? "INC VAT" : price.PriceTaxCondType;
                    //n.ImageUrl = "/SALE/assets/img/dogx.png";
                    //n.ImageUrl = @"/sale/ImageStorage/BPR/ITEMS_PHOTO_PROFILE/Thumb/" + q.ItemID + ".jpg";
                    n.ImageUrl = q.PhotoUrl == null || q.PhotoUrl == "" ? "/img/applogox.png" : q.PhotoUrl;
                    n.RefID = "";
                    n.IsActive = q.IsActive;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<MasterTypeLine> ListItemCate(string rcom) {
            List<string> exclude = new List<string> { "DISC PER", "DISC AMT" };
            return MastertypeService.ListType(rcom, "ITEM CATE", false).Where(o => !exclude.Contains(o.ValueTXT)).OrderBy(o => o.Sort).ToList();
        }

        #endregion

    }
}
