using RobotWasm.Server.Data.TFEDBF;
using RobotWasm.Shared.Data.ML.POS;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Master.Location;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Master.Company;
using RobotWasm.Shared.Data.ML.POS.ShipTo;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Shared.Data.ML.Shared;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Data.DA.GenDoc;

namespace RobotWasm.Server.Data.DA.POS {
    public class POSService {

        public I_POSSaleSet DocSet { get; set; }
        public List<POSMenuItem> Menu { get; set; }
        public List<MasterTypeLine> ItemCate { get; set; }
        public List<SelectOption> Tenders { get; set; }

        #region Convert Class

        public static I_POSSaleUploadDoc ConvertI_POSSaleSet2I_POSSaleUploadDoc(I_POSSaleSet input) {
            I_POSSaleUploadDoc ouptput = new I_POSSaleUploadDoc();
            ouptput.Head = ConvertHead2DB(input.Head);
            ouptput.Line = ConvertLine2DB(input.Line);
            ouptput.Payment = ConvertPayment2DB(input.Payment);

            return ouptput;
        }


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

            var shipToInfo = ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault();
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
                n.ImageUrl = "img/pear.png";

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

        public static I_POSSaleUploadDoc GetDocSetForUpload(string billId, string rcom) {
            I_POSSaleUploadDoc n = new I_POSSaleUploadDoc();
            using (GAEntities db = new GAEntities()) {
                n.Head = db.POS_SaleHead.Where(o => o.BillID == billId && o.RComID == rcom).FirstOrDefault();
                n.Line = db.POS_SaleLine.Where(o => o.BillID == billId && o.RComID == rcom).OrderBy(o => o.LineNum).ToList();
                n.Payment = db.POS_SalePayment.Where(o => o.BillID == billId && o.RComID == rcom).ToList();
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
                                                && (o.ComID == f.ComID)
                                                && (o.ShipToLocID == f.ShipTo || f.ShipTo == "X")
                                                && o.RComID == f.RComID
                                                && o.INVID != ""
                                                && o.IsActive == true
                                                ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                } else {//search แบบระบุวันที่
                    query = db.POS_SaleHead.Where(o =>
                                                        (o.BillDate >= f.Begin && o.BillDate <= f.End)
                                                        && (o.ShipToLocID == f.ShipTo || f.ShipTo == "X")
                                                        && o.RComID == f.RComID
                                                        && o.INVID != ""
                                                   && (o.ComID == f.ComID)
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

        public static List<POS_SaleHeadModel> ListBill_Online(I_BillFilterSetMobile f) {
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
                                              && (o.MacNo == f.MacNo || f.MacNo == "")
                                                && (o.ComID == f.ComID)
                                                && (o.ShipToLocID == f.ShipTo || f.ShipTo == "X")
                                                && o.RComID == f.RComID
                                                && o.INVID != ""
                                                && o.IsActive == true
                                                ).Skip(f.Skip).Take(f.Take).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                } else {//search แบบระบุวันที่
                    query = db.POS_SaleHead.Where(o =>
                                                        (o.BillDate >= f.Begin && o.BillDate <= f.End)
                                                        && (o.ShipToLocID == f.ShipTo || f.ShipTo == "X")
                                                        && o.RComID == f.RComID
                                                        && (o.MacNo == f.MacNo || f.MacNo == "")
                                                         && o.INVID != ""
                                                   && (o.ComID == f.ComID)
                                                && o.IsActive == true
                                            ).Skip(f.Skip).Take(f.Take).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                }

                foreach (var q in query) {
                    var qq = ConvertHead2Model(q);

                    result.Add(qq);
                }
            }
            return result;
        }
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

        #region save

        public static I_POSSaleSet CheckDupBillID(I_POSSaleSet doc) {
            try {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = doc.Head;

                using (GAEntities db = new GAEntities()) {
                    var shortShiptoId = ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault().ShortID;
                    var comInfo = CompanyService.GetComInfoByComID(h.RComID, h.ComID);
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

        public static I_POSSaleSet CheckDupInvoiceID(I_POSSaleSet doc, string macno) {
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
                        var comInfo = CompanyService.GetComInfoByComID(h.RComID, h.ComID);
                        IDRuunerService.GenPOSSaleID("INV", h.RComID, h.ComID, macno, shortShiptoId, true, h.BillDate);

                        h.INVID = IDRuunerService.GenPOSSaleID("INV", h.RComID, h.ComID, macno, shortShiptoId, false, h.BillDate)[1];
                        get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.INVID == h.INVID).FirstOrDefault();
                    }
                }
            } catch (Exception ex) {
            }
            return doc;
        }

        public static I_BasicResult Save(I_POSSaleUploadDoc client_doc, bool iswebSave) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = client_doc.Head;

            I_POSSaleUploadDoc merge_doc;
            try {
                using (GAEntities db = new GAEntities()) {
                    if (iswebSave) {
                        h.ModifiedDate = DateTime.Now;
                    }

                    var checkExist = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID).FirstOrDefault();
                    if (checkExist == null) {//insert new 

                        I_POSSaleUploadDoc server_doc = GetDocSetForUpload(h.BillID, h.RComID);
                        merge_doc = CompareDocument(server_doc, client_doc, iswebSave);
                        merge_doc.Head.IsLink = true;
                        db.POS_SaleHead.Add(merge_doc.Head);
                        db.POS_SaleLine.AddRange(merge_doc.Line);
                        db.POS_SalePayment.AddRange(merge_doc.Payment);
                        db.SaveChanges();

                        if (iswebSave) {
                            IDRuunerService.GenPOSSaleID("ORDER", h.RComID, h.ComID, h.MacNo, h.ShipToLocID, true, h.BillDate);
                        }

                        var rr = CalStock(h);
                        if (rr.Result == "fail") {
                            r.Result = rr.Result;
                            r.Message1 = rr.Message1;
                        }
                        var r2 = POS_SaleRefresh(merge_doc.Head);
                        if (r2.Result == "fail") {
                            r.Result = r2.Result;
                            r.Message1 = r2.Message1;
                        }

                    } else {

                        I_POSSaleUploadDoc server_doc = GetDocSetForUpload(h.BillID, h.RComID);
                        merge_doc = CompareDocument(server_doc, client_doc, iswebSave);
                        merge_doc.Head.IsLink = true;

                        var uh = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID).FirstOrDefault();
                        uh.BillID = merge_doc.Head.BillID;
                        uh.INVID = uh.INVID == "" ? merge_doc.Head.INVID : uh.INVID;
                        uh.FINVID = merge_doc.Head.FINVID;
                        uh.DocTypeID = merge_doc.Head.DocTypeID;
                        uh.MacNo = merge_doc.Head.MacNo;
                        uh.MacTaxNo = merge_doc.Head.MacTaxNo;
                        uh.BillDate = merge_doc.Head.BillDate;
                        uh.BillRefID = merge_doc.Head.BillRefID;
                        uh.INVPeriod = merge_doc.Head.INVPeriod;
                        uh.ReasonID = merge_doc.Head.ReasonID;
                        uh.TableID = merge_doc.Head.TableID;
                        uh.TableName = merge_doc.Head.TableName;
                        uh.CustomerID = merge_doc.Head.CustomerID;
                        uh.CustomerName = merge_doc.Head.CustomerName;
                        uh.CustAccGroup = merge_doc.Head.CustAccGroup;
                        uh.CustTaxID = merge_doc.Head.CustTaxID;
                        uh.CustBranchID = merge_doc.Head.CustBranchID;
                        uh.CustBranchName = merge_doc.Head.CustBranchName;
                        uh.CustAddr1 = merge_doc.Head.CustAddr1;
                        uh.CustAddr2 = merge_doc.Head.CustAddr2;
                        uh.IsVatRegister = merge_doc.Head.IsVatRegister;
                        uh.POID = merge_doc.Head.POID;
                        uh.ShipToLocID = merge_doc.Head.ShipToLocID;
                        uh.ShipToLocName = merge_doc.Head.ShipToLocName;
                        uh.ShipToAddr1 = merge_doc.Head.ShipToAddr1;
                        uh.ShipToAddr2 = merge_doc.Head.ShipToAddr2;
                        uh.SalesID1 = merge_doc.Head.SalesID1;
                        uh.SalesID2 = merge_doc.Head.SalesID2;
                        uh.SalesID3 = merge_doc.Head.SalesID3;
                        uh.SalesID4 = merge_doc.Head.SalesID4;
                        uh.SalesID5 = merge_doc.Head.SalesID5;
                        uh.ContactTo = merge_doc.Head.ContactTo;
                        uh.Currency = merge_doc.Head.Currency;
                        uh.RateExchange = merge_doc.Head.RateExchange;
                        uh.RateBy = merge_doc.Head.RateBy;
                        uh.RateDate = merge_doc.Head.RateDate;
                        uh.CreditTermID = merge_doc.Head.CreditTermID;
                        uh.Qty = merge_doc.Head.Qty;
                        uh.BaseNetTotalAmt = merge_doc.Head.BaseNetTotalAmt;
                        uh.NetTotalAmt = merge_doc.Head.NetTotalAmt;
                        uh.NetTotalVatAmt = merge_doc.Head.NetTotalVatAmt;
                        uh.NetTotalAmtIncVat = merge_doc.Head.NetTotalAmtIncVat;
                        uh.OntopDiscPer = merge_doc.Head.OntopDiscPer;
                        uh.OntopDiscAmt = merge_doc.Head.OntopDiscAmt;
                        uh.ItemDiscAmt = merge_doc.Head.ItemDiscAmt;
                        uh.DiscCalBy = merge_doc.Head.DiscCalBy;
                        uh.LineDisc = merge_doc.Head.LineDisc;
                        uh.VatRate = merge_doc.Head.VatRate;
                        uh.VatTypeID = merge_doc.Head.VatTypeID;
                        uh.IsVatInPrice = merge_doc.Head.IsVatInPrice;
                        uh.NetTotalAfterRound = merge_doc.Head.NetTotalAfterRound;
                        uh.NetDiff = merge_doc.Head.NetDiff;
                        uh.PayByCash = merge_doc.Head.PayByCash;
                        uh.PayByOther = merge_doc.Head.PayByOther;
                        uh.PayByVoucher = merge_doc.Head.PayByVoucher;
                        uh.PayByCredit = merge_doc.Head.PayByCredit;
                        uh.PayTotalAmt = merge_doc.Head.PayTotalAmt;
                        uh.PayMethodCount = merge_doc.Head.PayMethodCount;
                        uh.Remark1 = merge_doc.Head.Remark1;
                        uh.Remark2 = merge_doc.Head.Remark2;
                        uh.RemarkCancel = merge_doc.Head.RemarkCancel;
                        uh.LocID = merge_doc.Head.LocID;
                        uh.SubLocID = merge_doc.Head.SubLocID;
                        uh.CountLine = merge_doc.Head.CountLine;
                        uh.IsLink = merge_doc.Head.IsLink;
                        uh.LinkBy = merge_doc.Head.LinkBy;
                        uh.LinkDate = merge_doc.Head.LinkDate;
                        uh.Status = merge_doc.Head.Status;
                        uh.IsPrint = merge_doc.Head.IsPrint;
                        uh.PrintDate = merge_doc.Head.PrintDate;
                        uh.ModifiedBy = merge_doc.Head.ModifiedBy;
                        uh.ModifiedDate = merge_doc.Head.ModifiedDate;
                        uh.IsActive = merge_doc.Head.IsActive;

                        db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID));
                        db.POS_SaleLine.AddRange(merge_doc.Line);
                        db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID));
                        db.POS_SalePayment.AddRange(merge_doc.Payment);
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
                if (iswebSave) {
                    Save_SaleLog(merge_doc);
                }
               // FireBaseService.Sendnotify(h.RComID, h.ComID);
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

        public static I_BasicResult Save_SaleLog(I_POSSaleUploadDoc doc) {
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


        public static I_BasicResult SaveTaxSlip(I_POSSaleSet doc, string macno) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var h = doc.Head;
            try {
                using (GAEntities db = new GAEntities()) {
                    doc = CalDocSet(doc);
                    var uh = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID).FirstOrDefault();
                    var shortShiptoId = ListShipTo().Where(o => o.ShipToID == h.ShipToLocID).FirstOrDefault().ShortID;
                    var comInfo = CompanyService.GetComInfoByComID(h.RComID, h.ComID);
                    if (string.IsNullOrEmpty(doc.Head.FINVID)) {
                        uh.FINVID = IDRuunerService.GenPOSSaleID("TAX", h.RComID, h.ComID, macno, shortShiptoId, false, h.BillDate)[1];
                    }

                    uh.CustomerName = h.CustomerName;
                    uh.CustBranchName = h.CustBranchName;
                    uh.CustAddr1 = h.CustAddr1;
                    uh.CustAddr2 = h.CustAddr2;
                    uh.CustTaxID = h.CustTaxID;
                    uh.ModifiedBy = h.ModifiedBy;
                    uh.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    IDRuunerService.GenPOSSaleID("TAX", h.RComID, h.ComID, macno, shortShiptoId, true, h.BillDate);
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

        #region Add/Delete Line and Payment

        public static I_POSSaleSet AddItem(I_POSSaleSet doc, decimal qty, decimal discamt = 0) {

            var h = doc.Head;
            doc.LineActive = doc.Line.Where(o => o.ItemID == doc.SelectItem.ItemID && o.IsLineActive == true).FirstOrDefault();
            if (doc.LineActive == null && qty != -777) {//เพิ่มรหัสครั้งแรก /-777 คือการลบทีละรายการ
                doc.LineActive = NewLine(doc);
                doc.LineActive.Qty = 1;
                doc.Line.Add(doc.LineActive);
            } else {
                if (Math.Abs(qty) == 777) {//ส่ง 777 คือให้ระบบ บวกเพิ่มหรือลบ 1 ชิ้น(ตามเครื่องหมาย)                    
                    int x = qty < 0 ? -1 : 1;
                    doc.LineActive.Qty = doc.LineActive.Qty + x;
                } else {//ถ้าเป็นตัวเลขอื่นคือให้ replace ทับเลย
                    doc.LineActive.Qty = qty;
                }
                doc.LineActive.ModifiedDate = DateTime.Now;
            }

            if (doc.LineActive.Qty == 0) {
                //doc.Line.RemoveAll(o => o.LineUnq == doc.LineActive.LineUnq);
                var update_line = doc.Line.Where(o => o.LineUnq == doc.LineActive.LineUnq).FirstOrDefault();
                update_line.Qty = 0;
                update_line.IsLineActive = false;
                update_line.ModifiedDate = DateTime.Now;
                doc = CalDocSet(doc);
                return doc;
            }

            doc.LineActive.ItemID = doc.SelectItem.ItemID;
            doc.LineActive.ItemName = doc.SelectItem.Name;
            doc.LineActive.ItemTypeID = doc.SelectItem.TypeID;
            doc.LineActive.ItemCateID = doc.SelectItem.CateID;
            doc.LineActive.ItemGroupID = doc.SelectItem.GroupID;
            doc.LineActive.IsStockItem = doc.SelectItem.IsStockItem;
            doc.LineActive.Weight = doc.SelectItem.Weight;
            doc.LineActive.WUnit = doc.SelectItem.WUnit;
            doc.LineActive.Unit = doc.SelectItem.Unit;

            if (doc.SelectItem.PriceTaxCondType == "INC VAT") {
                doc.LineActive.Price = Math.Round(doc.SelectItem.Price * 100 / (100 + h.VatRate), 3, MidpointRounding.AwayFromZero);
                doc.LineActive.PriceIncVat = doc.SelectItem.Price;
            }
            if (doc.SelectItem.PriceTaxCondType == "EXC VAT") {
                doc.LineActive.Price = doc.SelectItem.Price;
                doc.LineActive.PriceIncVat = Math.Round(doc.SelectItem.Price * (100 + h.VatRate) / 100, 3, MidpointRounding.AwayFromZero);
            }


            if (doc.SelectItem.TypeID == "DISCOUNT") {
                doc.LineActive.Qty = 1;//ถ้าเป็น discount ให้ replace จำนวนเป็น 1
                doc.LineActive.LineNum = 9999;

                if (doc.LineActive.ItemID == "DISCPER01") {//ลดเป็น percent
                    doc.LineActive.ItemName = "ส่วนลด " + discamt + "%";

                    doc.LineActive.DiscCalBy = "P";
                    doc.LineActive.DiscPer = discamt;
                    doc.LineActive.DiscAmt = 0;
                    doc.LineActive.VatTypeID = "NONVAT";
                } else {//ลดเป็น amount
                    doc.LineActive.ItemName = "ส่วนลด " + "฿";
                    //doc.LineActive.BaseTotalAmt = discamt * -1;
                    doc.LineActive.DiscCalBy = "A";
                    doc.LineActive.DiscPer = 0;
                    doc.LineActive.DiscAmt = discamt;
                    doc.LineActive.Price = discamt;

                    //      doc.LineActive.Price = Math.Round(doc.SelectItem.Price * 100 / (100 + h.VatRate), 6);
                    //doc.LineActive.PriceIncVat = doc.SelectItem.Price;
                    doc.LineActive.VatTypeID = "NONVAT";
                }
            }
            doc = CalDocSet(doc);
            return doc;
        }

        public I_POSSaleSet DeleteItem(I_POSSaleSet doc, string lineunq) {
            var line_update = doc.Line.Where(o => o.LineUnq == lineunq).FirstOrDefault();
            line_update.IsLineActive = false;
            line_update.ModifiedDate = DateTime.Now;
            doc = CalDocSet(doc);
            return doc;
        }

        public static I_POSSaleSet AddPayment(I_POSSaleSet doc, string paymentType, decimal getAmt) {
            doc = CalDocSet(doc);
            //doc.Payment.RemoveAll(o => o.PaymentType == paymentType);
            doc.PaymentActive = doc.Payment.Where(o => o.PaymentType == paymentType).FirstOrDefault();
            if (doc.PaymentActive != null) {
                //doc.PaymentActive = NewPayment(paymentType);
                doc.PaymentActive.PaymentTypeName = paymentType;
                doc.PaymentActive.GetAmt = getAmt;
                doc.PaymentActive.ModifiedDate = DateTime.Now;
                doc.PaymentActive.IsLineActive = true;
                //doc.Payment.Add(doc.PaymentActive);
            } else {
                doc.PaymentActive = NewPayment(paymentType);
                doc.PaymentActive.PaymentTypeName = paymentType;
                doc.PaymentActive.GetAmt = getAmt;
                doc.PaymentActive.IsLineActive = true;
                doc.Payment.Add(doc.PaymentActive);
            }

            doc = CalDocSet(doc);
            return doc;

        }
        public I_POSSaleSet RemovePayment(I_POSSaleSet doc, string paymentType) {
            //doc.Payment.RemoveAll(o => o.PaymentType == paymentType);
            var update_payment = doc.Payment.Where(o => o.PaymentType == paymentType).FirstOrDefault();
            if (update_payment != null) {
                update_payment.IsLineActive = false;
                update_payment.ModifiedDate = DateTime.Now;
            }
            doc = CalDocSet(doc);
            return doc;
        }

        #endregion



        #region Cal Data & Refresh Data

        public static decimal CalDiscountInVat(I_POSSaleSet doc, decimal disc_input) {
            decimal discount = 0;
            if (doc.Head.IsVatRegister == true) {
                discount = (disc_input * 100) / (100 + doc.Head.VatRate);
                //discount = disc_input;
            } else {
                discount = disc_input;
            }
            return discount;
        }
        public static I_POSSaleSet CalDocSet(I_POSSaleSet doc) {
            var h = doc.Head;
            //copy head 2 line
            decimal baseDisc = doc.Line.Where(o => o.DocTypeID != "DISCOUNT" && o.IsLineActive == true && o.Status != "K-REJECT").Sum(o => o.BaseTotalAmt);
            foreach (var l in doc.Line) {
                //1.cal line base total amt 
                if (l.ItemTypeID == "DISCOUNT") {//  line ที่เป็น discount
                    //2. cal disc amt
                    if (l.DiscCalBy == "P") {
                        l.DiscAmt = Math.Round((baseDisc * l.DiscPer) / 100, 3, MidpointRounding.AwayFromZero);
                        //l.BaseTotalAmt = l.DiscAmt;

                    }
                    //3. cal disc per
                    if (l.DiscCalBy == "A") {
                        l.DiscPer = 0;
                        //l.BaseTotalAmt = l.DiscAmt;
                        if (baseDisc != 0) {
                            l.DiscPer = Math.Round((100 * l.DiscAmt) / baseDisc, 3, MidpointRounding.AwayFromZero);
                        }
                    }
                } else {//เป็นรายการขาย
                    l.BaseTotalAmt = Math.Round(l.Qty * l.Price, 3, MidpointRounding.AwayFromZero);
                    l.TotalAmt = l.BaseTotalAmt;
                }
                //4.assign head 2 line
                l.INVID = h.INVID;
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.BillDate = h.BillDate;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.MacNo = h.MacNo;
                l.DocTypeID = h.DocTypeID;
                l.CustID = h.CustomerID;
                l.TableID = h.TableID;
                l.TableName = h.TableName;
                l.ShipToLocID = h.ShipToLocID;
                l.ShipToLocName = h.ShipToLocName;
                l.VatRate = h.VatRate;
                l.VatTypeID = h.VatTypeID;
                l.CreatedBy = h.CreatedBy;
                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }

            //5. cal head base total amt            
            doc.Head.BaseNetTotalAmt = Math.Round(doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.BaseTotalAmt) - doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.DiscAmt), 2, MidpointRounding.AwayFromZero);
            //6. cal head disc amt
            if (doc.Head.DiscCalBy == "P") {
                doc.Head.OntopDiscAmt = Math.Round((doc.Head.BaseNetTotalAmt * doc.Head.OntopDiscPer) / 100, 2, MidpointRounding.AwayFromZero);
            }
            //7. cal head disc per
            if (doc.Head.DiscCalBy == "A") {
                if (doc.Head.BaseNetTotalAmt != 0) {
                    doc.Head.OntopDiscPer = Math.Round((100 * doc.Head.OntopDiscAmt) / doc.Head.BaseNetTotalAmt, 2, MidpointRounding.AwayFromZero);
                }
            }

            foreach (var l in doc.Line.Where(o => o.IsLineActive == true)) {
                //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
                l.OntopDiscPer = h.OntopDiscPer;
                l.OntopDiscAmt = Math.Round(h.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);

                //8.cal line disc weight ontop percent & amt

                if (h.BaseNetTotalAmt != 0) {
                    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * h.OntopDiscAmt) / h.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                }
                //9 cal line total amt 
                l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
                //10.cal line vat amt
                l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 3, MidpointRounding.AwayFromZero);
                //11.cal line nettotal amt inc vat
                l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 3, MidpointRounding.AwayFromZero);
            }

            //sum 2 head 
            h.ItemDiscAmt = doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.LineDisc = (doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt)) * -1;
            h.Qty = doc.Line.Where(o => o.ItemTypeID != "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.Qty);
            //h.CountLine = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT").Count();
            h.CountLine = doc.Line.Count();
            h.NetTotalAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt);
            h.NetTotalVatAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.VatAmt);
            h.NetTotalAmtIncVat = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.NetTotalAfterRound = h.NetTotalAmtIncVat - GetDecimalPart(h.NetTotalAmtIncVat);
            h.NetDiff = GetDecimalPart(h.NetTotalAmtIncVat);

            //copy head 2 line payment
            foreach (var l in doc.Payment) {
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.BillAmt = h.NetTotalAmtIncVat;
                l.RoundAmt = h.NetTotalAfterRound;
                if (l.GetAmt >= l.RoundAmt) {//รับมากกว่ายอดเต็ม
                    l.PayAmt = l.RoundAmt;
                } else {//รับน้อยกว่ายอดเต็ม
                    l.PayAmt = l.GetAmt;
                }
                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }

            //cal payment
            var change = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.GetAmt) - Convert.ToDecimal(h.NetTotalAfterRound);
            var payCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).FirstOrDefault();
            if (payCash != null) {
                payCash.ChangeAmt = change >= 0 ? change : 0;
            }

            //cal payby
            h.PayByOther = doc.Payment.Where(o => o.PaymentType != "CASH" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByVoucher = doc.Payment.Where(o => o.PaymentType == "VOUCHER" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayTotalAmt = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayMethodCount = doc.Payment.Count();
            doc.Line.OrderBy(o => o.LineNum);

            return doc;
        }

        public static I_POSSaleUploadDoc CalDocSet(I_POSSaleUploadDoc doc) {
            //เวลาแก้ไขสามารถ copy  CalDocset ที่ param เป็น I_POSSaleSet ได้ทั้งหมด โดยไม่ต้องแก้ไขไร
            var h = doc.Head;
            //copy head 2 line
            decimal baseDisc = doc.Line.Where(o => o.DocTypeID != "DISCOUNT" && o.IsLineActive == true).Sum(o => o.BaseTotalAmt);
            foreach (var l in doc.Line) {
                //1.cal line base total amt 
                if (l.ItemTypeID == "DISCOUNT") {//  line ที่เป็น discount
                    //2. cal disc amt
                    if (l.DiscCalBy == "P") {
                        l.DiscAmt = (baseDisc * l.DiscPer) / 100;
                        //l.BaseTotalAmt = l.DiscAmt;
                    }
                    //3. cal disc per
                    if (l.DiscCalBy == "A") {
                        l.DiscPer = 0;
                        //l.BaseTotalAmt = l.DiscAmt;
                        if (baseDisc != 0) {
                            l.DiscPer = (100 * l.DiscAmt) / baseDisc;
                        }
                    }
                } else {//เป็นรายการขาย
                    l.BaseTotalAmt = l.Qty * l.Price;
                    l.TotalAmt = l.BaseTotalAmt;
                }
                //4.assign head 2 line
                l.INVID = h.INVID;
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.BillDate = h.BillDate;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.MacNo = h.MacNo;
                l.DocTypeID = h.DocTypeID;
                l.CustID = h.CustomerID;
                l.TableID = h.TableID;
                l.TableName = h.TableName;
                l.VatRate = h.VatRate;
                l.VatTypeID = h.VatTypeID;
                l.CreatedBy = h.CreatedBy;
                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }

            //5. cal head base total amt            
            doc.Head.BaseNetTotalAmt = doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.BaseTotalAmt) - doc.Line.Where(o => o.IsLineActive == true).Sum(o => o.DiscAmt);
            //6. cal head disc amt
            if (doc.Head.DiscCalBy == "P") {
                doc.Head.OntopDiscAmt = (doc.Head.BaseNetTotalAmt * doc.Head.OntopDiscPer) / 100;
            }
            //7. cal head disc per
            if (doc.Head.DiscCalBy == "A") {
                if (doc.Head.BaseNetTotalAmt != 0) {
                    doc.Head.OntopDiscPer = (100 * doc.Head.OntopDiscAmt) / doc.Head.BaseNetTotalAmt;
                }
            }

            foreach (var l in doc.Line.Where(o => o.IsLineActive == true)) {
                //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
                l.OntopDiscPer = h.OntopDiscPer;
                l.OntopDiscAmt = Math.Round(h.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);

                //8.cal line disc weight ontop percent & amt

                if (h.BaseNetTotalAmt != 0) {
                    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * h.OntopDiscAmt) / h.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                }
                //9 cal line total amt 
                l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero); ;
                //10.cal line vat amt
                l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 3, MidpointRounding.AwayFromZero);
                //11.cal line nettotal amt inc vat
                l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 3, MidpointRounding.AwayFromZero);
            }

            //sum 2 head 
            h.ItemDiscAmt = doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.LineDisc = (doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt)) * -1;
            h.Qty = doc.Line.Where(o => o.ItemTypeID != "DISCOUNT" && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.Qty);
            //h.CountLine = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT").Count();
            h.CountLine = doc.Line.Count();
            h.NetTotalAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmt);
            h.NetTotalVatAmt = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.VatAmt);
            h.NetTotalAmtIncVat = doc.Line.Where(o => o.IsActive == true && o.Status != "K-REJECT" && o.IsLineActive == true).Sum(o => o.TotalAmtIncVat);
            h.NetTotalAfterRound = h.NetTotalAmtIncVat - GetDecimalPart(h.NetTotalAmtIncVat);
            h.NetDiff = GetDecimalPart(h.NetTotalAmtIncVat);


            //copy head 2 line payment
            foreach (var l in doc.Payment) {
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.BillAmt = h.NetTotalAmtIncVat;
                l.RoundAmt = h.NetTotalAfterRound;
                if (l.GetAmt >= l.RoundAmt) {//รับมากกว่ายอดเต็ม
                    l.PayAmt = l.RoundAmt;
                } else {//รับน้อยกว่ายอดเต็ม
                    l.PayAmt = l.GetAmt;
                }

                l.CreatedDate = l.CreatedDate == null ? DateTime.Now : l.CreatedDate;
                l.ModifiedDate = l.ModifiedDate == null ? l.CreatedDate : l.ModifiedDate;
            }

            //cal payment
            List<string> payVTC = new List<string> { "VOUCHER", "TRANSFER", "CREDIT" };
            var hasVoucherTranCredit = doc.Payment.Where(o => payVTC.Contains(o.PaymentType) && o.IsLineActive == true && o.IsActive).FirstOrDefault();
            var change = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.GetAmt) - Convert.ToDecimal(h.NetTotalAfterRound);
            var payCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).FirstOrDefault();
            if (payCash != null) {
                payCash.ChangeAmt = change >= 0 ? change : 0;
                if (hasVoucherTranCredit != null) {//มีชำระด้วย voucher ให้คำนวณ Paymat ของ cash ใหม่
                    payCash.PayAmt = payCash.GetAmt - payCash.ChangeAmt;
                }
            }

            //cal payby
            List<string> otherPayTypeInExcept = new List<string> { "CASH", "CREDIT" };  //ONLINE , TRANSFER , CREDIT , CASH , OTHER
            h.PayByOther = doc.Payment.Where(o => !otherPayTypeInExcept.Contains(o.PaymentType) && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByCash = doc.Payment.Where(o => o.PaymentType == "CASH" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByCredit = doc.Payment.Where(o => o.PaymentType == "CREDIT" && o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayByVoucher = doc.Payment.Where(o => o.PaymentType == "VOUCHER" && o.IsLineActive == true).Sum(o => o.PayAmt);

            h.PayTotalAmt = doc.Payment.Where(o => o.IsLineActive == true).Sum(o => o.PayAmt);
            h.PayMethodCount = doc.Payment.Count();
            doc.Line.OrderBy(o => o.LineNum);

            return doc;
        }

        public static decimal GetDecimalPart(decimal number) {
            decimal result = (number - Math.Truncate(number));
            return result;
        }

        public static I_BasicResult CalStock(POS_SaleHead h) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                if (h.INVID != "") {
                    using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                        //var dynamicParameters = new DynamicParameters();
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

        public static I_POSSaleUploadDoc CompareDocument(I_POSSaleUploadDoc server_doc, I_POSSaleUploadDoc client_doc, bool isWebSave) {
            I_POSSaleUploadDoc merge_doc = new I_POSSaleUploadDoc();
            try {
                server_doc.Head = server_doc.Head == null ? new POS_SaleHead() : server_doc.Head;
                server_doc.Line = server_doc.Line == null ? new List<POS_SaleLine>() : server_doc.Line;
                server_doc.Payment = server_doc.Payment == null ? new List<POS_SalePayment>() : server_doc.Payment;
                var head_db = server_doc.Head;
                var head_client = client_doc.Head;
                if (head_db != null) {
                    if (head_client.INVID != "" && head_db.INVID == "") {
                        head_db.INVID = head_client.INVID;
                        head_db.ModifiedDate = DateTime.Now;
                    }
                    if (DatetimeInfoService.RemoveMilliSecond(head_client.ModifiedDate) > DatetimeInfoService.RemoveMilliSecond(head_db.ModifiedDate)) {
                        head_db = head_client;
                        head_db.ID = 0;
                        head_db.ModifiedDate = DateTime.Now;
                    }
                } else {
                    head_db = head_client;
                    head_db.ID = 0;
                    head_db.ModifiedDate = DateTime.Now;
                }

                var line_db = server_doc.Line;
                var line_client = client_doc.Line;
                foreach (var l_ct in line_client) {
                    var chk_exist_db = line_db.Where(o => o.LineUnq == l_ct.LineUnq).FirstOrDefault();
                    if (chk_exist_db != null) {//has in db
                        l_ct.Status = chk_exist_db.Status;
                        l_ct.KitchenFinishCount = chk_exist_db.KitchenFinishCount;
                        if (!isWebSave) {
                            l_ct.IsLineActive = chk_exist_db.IsLineActive;
                        }
                        if (DatetimeInfoService.RemoveMilliSecond(l_ct.ModifiedDate) > DatetimeInfoService.RemoveMilliSecond(chk_exist_db.ModifiedDate)) {
                            line_db.Remove(chk_exist_db);
                            l_ct.ID = 0;
                            line_db.Add(l_ct);
                        }
                    } else {//not in db
                        l_ct.ID = 0;
                        line_db.Add(l_ct);
                    }
                }

                var pay_line_db = server_doc.Payment;
                var pay_line_client = client_doc.Payment;
                foreach (var l_ct in pay_line_client) {
                    var chk_exist_db = pay_line_db.Where(o => o.LineUnq == l_ct.LineUnq).FirstOrDefault();
                    if (chk_exist_db != null) {//has in db
                        if (DatetimeInfoService.RemoveMilliSecond(l_ct.ModifiedDate) > DatetimeInfoService.RemoveMilliSecond(chk_exist_db.ModifiedDate)) {
                            pay_line_db.Remove(chk_exist_db);
                            l_ct.ID = 0;
                            pay_line_db.Add(l_ct);
                        }
                    } else {//not in db
                        l_ct.ID = 0;
                        pay_line_db.Add(l_ct);
                    }
                }
                merge_doc.Head = head_db;
                merge_doc.Line = line_db;
                merge_doc.Payment = pay_line_db;
                merge_doc = CalDocSet(merge_doc);
            } catch (Exception) {

                throw;
            }
            return merge_doc;
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

        #region NewTransaction
        public I_POSSaleSet NewTransaction() {

            var doc = new I_POSSaleSet();
            doc.Head = new POS_SaleHeadModel();
            doc.Line = new List<POS_SaleLineModel>();
            doc.LineActive = new POS_SaleLineModel();
            doc.Payment = new List<POS_SalePaymentModel>();
            doc.PaymentActive = new POS_SalePaymentModel();
            doc.Log = new List<TransactionLog>();
            doc.OutputAction = new I_BasicResult();
            return doc;
        }

        public static I_POSSaleSet NewTransaction(LoginSet login, string version, string shipto) {
            var doc = new I_POSSaleSet();
            doc.Head = NewHead(login, version);
            doc.Line = new List<POS_SaleLineModel>();
            doc.LineActive = new POS_SaleLineModel();
            doc.Payment = new List<POS_SalePaymentModel>();
            doc.PaymentActive = new POS_SalePaymentModel();
            doc.Log = new List<TransactionLog>();
            if (string.IsNullOrEmpty(login.CurrentCompany.ShortCode)) {
                doc.OutputAction.Result = "fail";
                doc.OutputAction.Message1 = "ตั้งชื่อย่อสาขาก่อนทำรายการขาย";
                return doc;
            }

            doc.Head.ComID = login.CurrentCompany.CompanyID;
            doc.Head.IsVatRegister = login.CurrentCompany.IsWH;
            doc.Head.BillDate = Convert.ToDateTime(login.CurrentTransactionDate).Date;
            var shiptoInfo = ListShipTo().Where(o => o.ShipToID == shipto).FirstOrDefault();
            doc.Head.ShipToLocID = shipto;
            doc.Head.ShipToUsePrice = shiptoInfo.UsePrice; //ค่าว่างเป็นราคาหน้าร้าน 
            doc.Head.ShipToLocID = shiptoInfo.ShipToID;
            doc.Head.ShipToLocName = shiptoInfo.ShipToName;
            doc.Head.ImageUrlShipTo = shiptoInfo.ImageUrl;
            doc.Head.MacNo = login.CurrentMacNo;
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            doc.SelectItem = new POSMenuItem();
            //Menu = ListMenuItem(doc.Head.RComID, doc.Head.ComID, doc.Head.ShipToUsePrice);
            //ItemCate = ListItemCate(doc.Head.RComID);
            if (doc.Head.IsVatRegister == true) {
                doc.Head.VatRate = login.CurrentVatRate;
            }

            return doc;
        }

        public static POS_SaleHeadModel NewHead(LoginSet login, string version) {
            POS_SaleHeadModel n = new POS_SaleHeadModel();

            n.RComID = login.CurrentRootCompany.CompanyID;
            n.ComID = "";
            n.BillID = "";
            n.INVID = "";
            n.FINVID = "";
            n.DocTypeID = "SALE";
            n.MacNo = login.CurrentMacNo;
            n.MacTaxNo = "";
            n.BillDate = login.CurrentTransactionDate;
            n.BillRefID = "";
            n.INVPeriod = "";
            n.ReasonID = "";
            n.TableID = "T-000";
            n.TableName = "กลับบ้าน";
            n.CustomerID = "";
            n.CustomerName = "";
            n.CustAccGroup = "";
            n.CustTaxID = "";
            n.CustBranchID = "";
            n.CustBranchName = "";
            n.CustAddr1 = "";
            n.CustAddr2 = "";
            n.IsVatRegister = false;
            n.POID = "";
            n.ShipToLocID = "";
            n.ShipToLocName = "";
            n.ShipToUsePrice = "";
            n.ShipToAddr1 = "";
            n.ShipToAddr2 = "";
            n.SalesID1 = login.CurrentUser;
            n.SalesID2 = "";
            n.SalesID3 = "";
            n.SalesID4 = "";
            n.SalesID5 = "";
            n.ContactTo = "";
            n.Currency = "";
            n.RateExchange = 1;
            n.Currency = "THB";
            n.RateBy = "";
            n.RateDate = login.CurrentTransactionDate;
            n.CreditTermID = "";
            n.Qty = 0;
            n.BaseNetTotalAmt = 0;
            n.NetTotalAmt = 0;
            n.NetTotalVatAmt = 0;
            n.NetTotalAmtIncVat = 0;
            n.OntopDiscPer = 0;
            n.OntopDiscAmt = 0;
            n.ItemDiscAmt = 0;
            n.DiscCalBy = "";
            n.LineDisc = 0;
            n.VatRate = 0;
            n.VatTypeID = "";
            n.IsVatInPrice = false;
            n.NetTotalAfterRound = 0;
            n.NetDiff = 0;
            n.PayByCash = 0;
            n.PayByOther = 0;
            n.PayTotalAmt = 0;
            n.PayMethodCount = 0;
            n.PayByVoucher = 0;
            n.Remark1 = "";
            n.Remark2 = "";
            n.RemarkCancel = "";
            n.LocID = "";
            n.SubLocID = "";
            n.CountLine = 0;
            n.IsLink = true;
            n.LinkBy = "OnWeb";
            n.LinkDate = DateTime.Now;
            n.IsRecalStock = false;
            n.Status = "OPEN";
            n.IsPrint = false;
            n.PrintDate = null;
            n.CreatedByApp = version;
            n.CreatedBy = login.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = DateTime.Now;
            n.IsActive = true;


            return n;
        }

        public static POS_SaleLineModel NewLine(I_POSSaleSet doc) {
            POS_SaleLineModel n = new POS_SaleLineModel();
            n.RComID = "";
            n.ComID = "";
            n.MacNo = "";
            n.LineNum = GenLineNum(doc);
            n.LineUnq = Guid.NewGuid().ToString().Substring(0, 10);
            n.RefLineUnq = "";
            n.RefLineNum = 0;
            n.BillID = "";
            n.INVID = "";
            n.FINVID = "";
            n.BelongToLineNum = "";
            n.DocTypeID = "";
            n.BillDate = DateTime.Now.Date;
            n.BillRefID = "";
            n.CustID = "";
            n.TableID = "";
            n.TableName = "";
            n.Barcode = "";
            n.ItemID = "";
            n.ItemName = "";
            n.ItemTypeID = "";
            n.ItemCateID = "";

            n.ItemGroupID = "";
            n.IsStockItem = true;
            n.Weight = 0;
            n.WUnit = "";
            n.Unit = "";
            n.Qty = 0;
            n.Cost = 0;
            n.Price = 0;
            n.PriceIncVat = 0;
            n.BaseTotalAmt = 0;
            n.TotalAmt = 0;
            n.VatAmt = 0;
            n.TotalAmtIncVat = 0;
            n.VatRate = 0;
            n.VatTypeID = "";
            n.OntopDiscAmt = 0;
            n.OntopDiscPer = 0;
            n.DiscPer = 0;
            n.DiscAmt = 0;
            n.DiscCalBy = "";
            n.IsFree = false;
            n.ShareGpPer = 0;
            n.IsOntopItem = false;
            n.PromotionID = "";
            n.PatternID = "";
            n.PaternValue = "";
            n.MatchingNumber = 1;
            n.ProPackCode = "";
            n.IsProCompleted = false;
            n.LocID = "";
            n.SubLocID = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.BatchNo = "";
            n.Remark = "";
            n.Memo = "";
            n.ImgUrl = "img/applogox.png";
            n.ImageSource = "img/applogox.png";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedDate = DateTime.Now;
            n.KitchenFinishCount = 0;
            n.Sort = n.LineNum;
            n.Status = "OK";
            n.IsLineActive = true;
            n.IsActive = true;
            return n;
        }
        public static POS_SalePaymentModel NewPayment(string paymentType) {
            POS_SalePaymentModel n = new POS_SalePaymentModel();
            n.RComID = "";
            n.ComID = "";
            n.LineUnq = Guid.NewGuid().ToString().Substring(0, 10);
            n.BillID = "";
            n.PaymentType = paymentType;
            n.INVID = "";
            n.FINVID = "";
            n.BillAmt = 0;
            n.RoundAmt = 0;
            n.GetAmt = 0;
            n.PayAmt = 0;
            n.ChangeAmt = 0;
            n.CreatedDate = DateTime.Now;
            n.ModifiedDate = DateTime.Now;
            n.IsLineActive = true;
            n.IsActive = true;
            return n;
        }

        public static int GenLineNum(I_POSSaleSet doc) {
            var next = 0;
            if (doc.Line.Count > 0) {
                next = doc.Line.Where(o => o.ItemTypeID != "DISCOUNT").Max(o => o.LineNum);
            }
            next = next + 10;
            return next;
        }
        public static I_BillFilterSet NewFilterSet() {
            I_BillFilterSet n = new I_BillFilterSet();
            n.Begin = DateTime.Now.Date;
            n.End = DateTime.Now.Date;
            n.Search = "";
            n.ComID = "";
            n.RComID = "";
            n.ShipTo = "X";
            n.MacNo = "";
            n.Table = "";
            n.ShowActive = true;
            return n;
        }

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
                        var rr = Save(ConvertI_POSSaleSet2I_POSSaleUploadDoc(doc), true);
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

        #region ListData New Class

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

                var query = db.vw_ItemInfo.Where(o =>
                                          new List<string> { "FG", "DISCOUNT" }.Contains(o.TypeID)
                                            & o.RCompanyID == rcom
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
                    n.GroupName = q.Group1Name;
                    n.CateID = q.CateID;
                    n.Company = q.CompanyID;
                    n.CustId = q.CateID;
                    n.TypeID = q.TypeID;
                    n.GroupID = q.Group1ID;
                    n.TypeName = q.TypeName;
                    n.IsStockItem = q.IsKeepStock;
                    //n.AccGroup = q.GLGroupID;
                    n.SellQty = 0;
                    n.SellAmt = 0;
                    n.WUnit = q.UnitID;
                    n.Unit = q.UnitID;
                    n.Price = price == null ? 0 : price.Price;
                    n.PriceTaxCondType = price == null ? "INC VAT" : price.PriceTaxCondType;
                    //n.ImageUrl = "/SALE/assets/img/dogx.png";
                    n.ImageUrl = @"/sale/ImageStorage/BPR/ITEMS_PHOTO_PROFILE/Thumb/" + q.ItemID + ".jpg";
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

        public static List<SelectOption> ListTenderType() {
            return new List<SelectOption>() {
                new SelectOption(){ Description= "เงินสด", Value="CASH"},
                new SelectOption(){ Description= "โอนเงิน", Value="TRANSFER"},
                new SelectOption(){ Description= "บัตรเครดิต", Value="CREDIT"},
                new SelectOption(){ Description= "Voucher", Value="VOUCHER"},
            };
        }

        public List<SelectOption> ListDisCount() {
            return new List<SelectOption>() {
                new SelectOption(){ Description= "P", Value="เปอร์เซ็นต์ "},
                new SelectOption(){ Description= "D", Value="เงินสด"},
            };
        }

        public static List<ShipTo> ListShipTo() {
            List<ShipTo> shipList = new List<ShipTo>();
            shipList.Add(new ShipTo {
                ShipToID = "",
                ShipToName = "หน้าร้าน",
                ShortID = "",
                UsePrice = "",
                ImageUrl = "img/SALE/frontstore.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "GRAB",
                ShipToName = "Grab",
                ShortID = "G",
                UsePrice = "GRAB",
                ImageUrl = "img/SALE/grab.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "SHOPEE",
                ShipToName = "Shopee",
                ShortID = "J",
                UsePrice = "SHOPEE",
                ImageUrl = "img/SALE/shopee_logo.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "LINEMAN",
                ShipToName = "Line Man",
                ShortID = "L",
                UsePrice = "LINEMAN",
                ImageUrl = "img/SALE/lineman.png"
            });

            shipList.Add(new ShipTo {
                ShipToID = "PANDA",
                ShipToName = "Food Padda",
                ShortID = "P",
                UsePrice = "PANDA",
                ImageUrl = "img/SALE/padda.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "ROBINHOOD",
                ShipToName = "Robinhood",
                ShortID = "R",
                UsePrice = "ROBINHOOD",
                ImageUrl = "img/SALE/robinhood.png"
            });
            shipList.Add(new ShipTo {
                ShipToID = "ONLINE",
                ShipToName = "Online",
                ShortID = "O",
                UsePrice = "ONLINE",
                ImageUrl = "img/SALE/online.png"
            });
            return shipList;

        }
        #endregion

    }
}
