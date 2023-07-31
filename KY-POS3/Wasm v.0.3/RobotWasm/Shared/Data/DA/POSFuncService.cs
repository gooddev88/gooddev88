using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.DA {
    public class POSFuncService {

        #region class

        public class I_POSSaleSet {
            public POS_SaleHeadModel Head { get; set; }
            public List<POS_SaleLineModel> Line { get; set; }
            public POS_SaleLineModel LineActive { get; set; }
            public POSMenuItem SelectItem { get; set; }
            public List<POS_SalePaymentModel> Payment { get; set; }
            public POS_SalePaymentModel PaymentActive { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class POS_SaleHeadModel : POS_SaleHead {
            public String ImageUrlShipTo { get; set; } = "frontstore.png";
        }
        public class POS_SaleLineModel : POS_SaleLine {

            public String KitchenMessageLogo { get; set; }
            public bool IsEditAble { get; set; }
            public String ImageUrl { get; set; }
            public string ImageSource { get; set; }

        }
        public class POS_SalePaymentModel : POS_SalePayment {
            public string PaymentTypeName { get; set; }
        }

        public class POSMenuItem {
            public string ItemID { get; set; }
            public string Name { get; set; }
            public string Company { get; set; }
            public string CustId { get; set; }
            public string TypeName { get; set; }
            public string GroupName { get; set; }
            public string CateID { get; set; }
            public string GroupID { get; set; }
            public string TypeID { get; set; }
            public string AccGroup { get; set; }
            public decimal SellQty { get; set; }
            public decimal SellAmt { get; set; }
            public string ImageUrl { get; set; }
            public decimal Price { get; set; }
            public string PriceTaxCondType { get; set; }
            public decimal Weight { get; set; }
            public string RefID { get; set; }
            public string WUnit { get; set; }
            public string Unit { get; set; }
            public bool IsStockItem { get; set; }
            public bool IsActive { get; set; }
        }

        public class ShipTo {
            public String ShipToID { get; set; }
            public String ShipToName { get; set; }
            public String ShortID { get; set; }
            public String UsePrice { get; set; }
            public String ImageUrl { get; set; }
        }

        public class POS_TableModel : POS_Table {
            public string Image { get; set; }
        }

        #region Class Filter
        public class I_BillFilterSet {
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
            public string ComID { get; set; }
            public string RComID { get; set; }
            public string Table { get; set; }
            public string ShipTo { get; set; }
            public string MacNo { get; set; }
            public string Search { get; set; }
            public bool ShowActive { get; set; }
            public List<string> comIDs { get; set; }
        }

        public class KitchenStatusParam {

            public string rcom { get; set; }
            public string comid { get; set; }
            public string billId { get; set; }
            public int linenum { get; set; }
            public string lineunq { get; set; }
            public string status { get; set; }
            public string Username { get; set; }
            public string RoomID { get; set; }
            public List<string> cates { get; set; }

        }
        #endregion

        #endregion


        #region CalDocSet

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
                    if (l.IsLineActive == true) {
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

        public static decimal GetDecimalPart(decimal number) {
            decimal result = (number - Math.Truncate(number));
            return result;
        }

        #endregion

        #region Add Item & Delete Item

        public static I_POSSaleSet AddItem(I_POSSaleSet doc, decimal qty, decimal discamt = 0) {

            var h = doc.Head;
            doc.LineActive = doc.Line.Where(o => o.ItemID == doc.SelectItem.ItemID && o.Qty > o.KitchenFinishCount && o.IsLineActive == true).FirstOrDefault();
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

                    //doc.LineActive.Price = Math.Round(doc.SelectItem.Price * 100 / (100 + h.VatRate), 6);
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

        #endregion

        #region Add Payment & Delete Payment
        public static I_POSSaleSet AddPayment(I_POSSaleSet doc, string paymentType, decimal getAmt) 
        {
            doc = CalDocSet(doc);
            doc.PaymentActive = doc.Payment.Where(o => o.PaymentType == paymentType).FirstOrDefault();
            if (doc.PaymentActive != null) {
                doc.PaymentActive.PaymentTypeName = paymentType;
                doc.PaymentActive.GetAmt = getAmt;
                doc.PaymentActive.ModifiedDate = DateTime.Now;
                doc.PaymentActive.IsLineActive = true;
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
        public static I_POSSaleSet RemovePayment(I_POSSaleSet doc, string paymentType) {
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

        #region New transaction
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
            doc.Head.IsVatRegister = login.CurrentCompany.IsVatRegister;
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

            // ไปดึงข้อมูลทีหลัง
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
            n.DocTypeID = "ORDER";
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
            n.Begin = DateTime.Now.AddDays(-7);
            n.End = DateTime.Now.Date;
            n.Search = "";
            n.ComID = "";
            n.RComID = "";
            n.ShipTo = "";
            n.MacNo = "";
            n.Table = "";
            n.ShowActive = true;
            return n;
        }

        public static KitchenStatusParam NewFilterKitchen() {
            KitchenStatusParam n = new KitchenStatusParam();
            n.rcom = "";
            n.comid = "";
            n.billId = "";
            n.linenum = 0;
            n.lineunq = "";
            n.status = "";
            n.Username = "";
            n.RoomID = "";
            n.cates = new List<string>();
            return n;
        }       

        #endregion

        #region ListData

        public static List<SelectOption> ListTenderType() {
            return new List<SelectOption>() {
                new SelectOption(){ Description= "เงินสด", Value="CASH"},
                new SelectOption(){ Description= "โอนเงิน", Value="TRANSFER"},
                new SelectOption(){ Description= "บัตรเครดิต", Value="CREDIT"},
                new SelectOption(){ Description= "Voucher", Value="VOUCHER"},
            };
        }

        public static List<SelectOption> ListDisCount() {
            return new List<SelectOption>() {
                new SelectOption(){ Description= "เปอร์เซ็นต์", Value="DISCPER01"},
                new SelectOption(){ Description= "เงินสด", Value="DISCAMT01"},
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

        #region function
        public static bool IsOverPayable(I_POSSaleSet doc) {
            bool result = false;
            var hasCash = doc.Payment.Where(o => o.PaymentType == "CASH").ToList().Count;
            var hasVoucher = doc.Payment.Where(o => o.PaymentType == "VOUCHER").ToList().Count;
            var hastransfer = doc.Payment.Where(o => o.PaymentType == "TRANSFER").ToList().Count;
            var hascredit = doc.Payment.Where(o => o.PaymentType == "CREDIT").ToList().Count;
            if (hasCash > 0 && hasVoucher > 0) {
                result = true;
            }
            if (hasCash > 0 && hastransfer > 0) {
                result = true;
            }
            if (hasCash > 0 && hascredit > 0) {
                result = true;
            }
            return result;
        }
        #endregion

    }
}
