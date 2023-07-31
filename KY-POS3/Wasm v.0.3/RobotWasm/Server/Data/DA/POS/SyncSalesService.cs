using RobotWasm.Server.Data.TFEDBF;
using RobotWasm.Shared.Data.ML.AssetDoc;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.TFEDBF;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Master.Location;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.POS;

namespace RobotWasm.Server.Data.DA.POS {
    public class SyncSalesService {

        public class I_FilterSet {
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string Table { get; set; }
            public string ShipTo { get; set; }
            public string MacNo { get; set; }
            public string Search { get; set; }
            public bool IsActive { get; set; }
        }
        public class I_FilterLatestTransaction {
            public DateTime LatestDate { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string MacNo { get; set; }
            public bool IsNoRecord { get; set; }
            public DateTime limitDate { get; set; }
        }

        public static List<I_POSSaleUploadDoc> UploadSaleTransaction(List<I_POSSaleUploadDoc> data) {
            try {
                DateTime begin = DateTime.Now.Date;
                DateTime end = DateTime.Now.Date;
                string rcom = "";
                string com = "";
                int forceRepeat = 0;
                if (data.Count > 0) {
                    begin = data.Min(o => o.Head.BillDate);
                    end = data.Max(o => o.Head.BillDate);
                    rcom = data.Min(o => o.Head.RComID);
                    com = data.Min(o => o.Head.ComID);
                } else {
                    return data;
                }

                using (GAEntities db = new GAEntities()) {
                    foreach (var d in data) {
                        d.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                        //   var h = d.Head;
                        //valid complete count line
                        // var chkNumOfLine = d.Line.Where(o=>o.Status!= "K-REJECT").Count();
                        var chkNumOfLine = d.Line.Count();
                        var chkNumOfPay = d.Payment.Count();
                        if ((d.Head.CountLine != chkNumOfLine) || (d.Head.PayMethodCount != chkNumOfPay)) {
                            d.Head.IsLink = false;
                            d.Head.LinkDate = null;
                            d.Head.LinkBy = "";
                            d.OutputAction.Result = "fail";
                            d.OutputAction.Message1 = "Incomplete line or payment data";
                            continue;
                        }
                        if (string.IsNullOrEmpty(d.Head.CreatedByApp)) {
                            d.Head.CreatedByApp = "androidv1";
                        }

                        try {
                            using (var transaction = db.Database.BeginTransaction()) {

                                var uh = db.POS_SaleHead.Where(o => o.BillID == d.Head.BillID && o.RComID == d.Head.RComID && o.ComID == d.Head.ComID).FirstOrDefault();
                                if (uh != null) {//has exist bill
                                                 //set islink  สำหรับ update กลับไปยัง client ว่า สำเร็จ
                                    d.Head.IsLink = true;
                                    d.Head.LinkDate = DateTime.Now;
                                    d.Head.LinkBy = "Mr.Auto";
                                    DateTime deviceMoDate = Convert.ToDateTime(d.Head.ModifiedDate).AddTicks(-(Convert.ToDateTime(d.Head.ModifiedDate).Ticks % 10000000));
                                    DateTime serverMoDate = Convert.ToDateTime(d.Head.ModifiedDate).AddTicks(-(Convert.ToDateTime(d.Head.ModifiedDate).Ticks % 10000000));
                                    if (deviceMoDate >= serverMoDate) {  //ถ้าใน client แก้ไขทีหลัง server ให้อัพเดทลง server
                                        if (uh.INVID != "" && d.Head.INVID == "") {//ถ้า server จ่ายเงินแล้ว แต่ client ยังไม่จ่ายจะไม่ให้อัพขึ้น server 
                                            continue;
                                        }
                                        uh.BillID = d.Head.BillID;
                                        uh.INVID = d.Head.INVID;
                                        uh.FINVID = d.Head.FINVID;
                                        uh.DocTypeID = d.Head.DocTypeID;
                                        uh.MacNo = d.Head.MacNo;
                                        uh.MacTaxNo = d.Head.MacTaxNo;
                                        uh.BillDate = d.Head.BillDate;
                                        uh.BillRefID = d.Head.BillRefID;
                                        uh.INVPeriod = d.Head.INVPeriod;
                                        uh.ReasonID = d.Head.ReasonID;
                                        uh.TableID = d.Head.TableID;
                                        uh.TableName = d.Head.TableName;
                                        uh.CustomerID = d.Head.CustomerID;
                                        uh.CustomerName = d.Head.CustomerName;
                                        uh.CustAccGroup = d.Head.CustAccGroup;
                                        uh.CustTaxID = d.Head.CustTaxID;
                                        uh.CustBranchID = d.Head.CustBranchID;
                                        uh.CustBranchName = d.Head.CustBranchName;
                                        uh.CustAddr1 = d.Head.CustAddr1;
                                        uh.CustAddr2 = d.Head.CustAddr2;
                                        uh.IsVatRegister = d.Head.IsVatRegister;
                                        uh.POID = d.Head.POID;
                                        uh.ShipToLocID = d.Head.ShipToLocID;
                                        uh.ShipToLocName = d.Head.ShipToLocName;
                                        uh.ShipToAddr1 = d.Head.ShipToAddr1;
                                        uh.ShipToAddr2 = d.Head.ShipToAddr2;
                                        uh.SalesID1 = d.Head.SalesID1;
                                        uh.SalesID2 = d.Head.SalesID2;
                                        uh.SalesID3 = d.Head.SalesID3;
                                        uh.SalesID4 = d.Head.SalesID4;
                                        uh.SalesID5 = d.Head.SalesID5;
                                        uh.ContactTo = d.Head.ContactTo;
                                        uh.Currency = d.Head.Currency;
                                        uh.RateExchange = d.Head.RateExchange;
                                        uh.RateBy = d.Head.RateBy;
                                        uh.RateDate = d.Head.RateDate;
                                        uh.CreditTermID = d.Head.CreditTermID;
                                        uh.Qty = d.Head.Qty;
                                        uh.BaseNetTotalAmt = d.Head.BaseNetTotalAmt;
                                        uh.NetTotalAmt = d.Head.NetTotalAmt;
                                        uh.NetTotalVatAmt = d.Head.NetTotalVatAmt;
                                        uh.NetTotalAmtIncVat = d.Head.NetTotalAmtIncVat;
                                        uh.OntopDiscPer = d.Head.OntopDiscPer;
                                        uh.OntopDiscAmt = d.Head.OntopDiscAmt;
                                        uh.ItemDiscAmt = d.Head.ItemDiscAmt;
                                        uh.DiscCalBy = d.Head.DiscCalBy;
                                        uh.LineDisc = d.Head.LineDisc;
                                        uh.VatRate = d.Head.VatRate;
                                        uh.VatTypeID = d.Head.VatTypeID;
                                        uh.IsVatInPrice = d.Head.IsVatInPrice;
                                        uh.NetTotalAfterRound = d.Head.NetTotalAfterRound;
                                        uh.NetDiff = d.Head.NetDiff;
                                        uh.PayByCash = d.Head.PayByCash;
                                        uh.PayByOther = d.Head.PayByOther;
                                        uh.PayTotalAmt = d.Head.PayTotalAmt;
                                        uh.PayMethodCount = d.Head.PayMethodCount;
                                        uh.Remark1 = d.Head.Remark1;
                                        uh.Remark2 = d.Head.Remark2;
                                        uh.LocID = d.Head.LocID;
                                        uh.SubLocID = d.Head.SubLocID;
                                        uh.CountLine = d.Head.CountLine;
                                        uh.LinkBy = d.Head.LinkBy;
                                        uh.Status = d.Head.Status;
                                        uh.IsPrint = d.Head.IsPrint;
                                        uh.PrintDate = d.Head.PrintDate;
                                        //uh.CreatedByApp = h.CreatedByApp;
                                        uh.ModifiedBy = d.Head.ModifiedBy;
                                        uh.ModifiedDate = d.Head.ModifiedDate;
                                        uh.RemarkCancel = d.Head.RemarkCancel;
                                        uh.IsLink = true;
                                        uh.LinkDate = DateTime.Now;
                                        uh.LinkBy = "Mr.Auto";
                                        uh.IsActive = d.Head.IsActive;

                                        //แก้ปัญหาเรื่อง client ไม่อัพเดทสถานะจากห้องครัวแล้วกลับมาอัพเดท
                                        var line_in_db = db.POS_SaleLine.Where(o => o.BillID == d.Head.BillID && o.RComID == d.Head.RComID && o.ComID == d.Head.ComID).ToList();
                                        foreach (var dl in d.Line) {
                                            var dbLine = line_in_db.Where(o => o.LineNum == dl.LineNum).FirstOrDefault();
                                            if (dbLine != null) {
                                                dl.Status = dbLine.Status;
                                            }
                                        }

                                        db.POS_SaleLine.RemoveRange(line_in_db);
                                        db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == d.Head.BillID && o.RComID == d.Head.RComID && o.ComID == d.Head.ComID));
                                        var nl = CopyLine(d.Line);
                                        var np = CopyPayment(d.Payment);

                                        db.POS_SaleLine.AddRange(nl);
                                        db.POS_SalePayment.AddRange(np);
                                        db.SaveChanges();
                                        transaction.Commit();
                                        var r2 = POSService.POS_SaleRefresh(uh);
                                        if (r2.Result == "fail") {
                                            d.OutputAction.Result = r2.Result;
                                            d.OutputAction.Message1 = r2.Message1;
                                        }
                                        //var rr = CalStock(h);
                                        //if (rr.Result == "fail") {
                                        //    d.OutputAction.Result = rr.Result;
                                        //}
                                    }

                                } else {//new in database
                                    d.Head.IsLink = true;
                                    d.Head.LinkDate = DateTime.Now;
                                    d.Head.LinkBy = "Mr.Auto";
                                    //d.Head.CreatedByApp = h.CreatedByApp;
                                    var nh = CopyHead(d.Head);
                                    db.POS_SaleHead.Add(nh);
                                    db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == d.Head.BillID && o.RComID == d.Head.RComID && o.ComID == d.Head.ComID));
                                    db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == d.Head.BillID && o.RComID == d.Head.RComID && o.ComID == d.Head.ComID));
                                    var nl = CopyLine(d.Line);
                                    var np = CopyPayment(d.Payment);
                                    db.POS_SaleLine.AddRange(nl);
                                    db.POS_SalePayment.AddRange(np);
                                    db.SaveChanges();
                                    transaction.Commit();
                                    var r2 = POSService.POS_SaleRefresh(nh);
                                    if (r2.Result == "fail") {
                                        d.OutputAction.Result = r2.Result;
                                        d.OutputAction.Message1 = r2.Message1;
                                    }
                                    //var rr = CalStock(h);
                                    //if (rr.Result == "fail") {
                                    //    d.OutputAction.Result = rr.Result;
                                    //    d.OutputAction.Message1 = rr.Message1;
                                    //}
                                }
                            }
                        } catch (Exception ex) {
                            d.Head.IsLink = false;
                            d.Head.LinkDate = null;
                            d.Head.LinkBy = "";
                            d.OutputAction.Result = "fail";
                            if (ex.InnerException != null) {
                                d.OutputAction.Message1 = ex.InnerException.ToString();
                            } else {
                                d.OutputAction.Message1 = ex.Message;
                            }
                        }
                    }
                }
                var rstk = POSService.CalStockEndDay(begin, end, rcom, com, 0);

            } catch (Exception ex) {

            }
            return data;
        }


        public static List<I_POSSaleUploadDoc> UploadSaleTransactionV2(List<I_POSSaleUploadDoc> bills) {

            try {
                DateTime begin = DateTime.Now.Date;
                DateTime end = DateTime.Now.Date;
                string rcom = "";
                string com = "";

                if (bills.Count > 0) {// อัพโหลด transaction ที่ไม่มีบิล
                    begin = bills.Min(o => o.Head.BillDate);
                    end = bills.Max(o => o.Head.BillDate);
                    rcom = bills.Min(o => o.Head.RComID);
                    com = bills.Min(o => o.Head.ComID);
                } else {
                    return bills;
                }


                using (GAEntities db = new GAEntities()) {
                    foreach (var d in bills) {//loop bill
                        d.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

                        var chkNumOfLine = d.Line.Count();
                        var chkNumOfPay = d.Payment.Count();
                        if ((d.Head.CountLine != chkNumOfLine) || (d.Head.PayMethodCount != chkNumOfPay)) {
                            d.Head.IsLink = false;
                            d.Head.LinkDate = null;
                            d.Head.LinkBy = "";
                            d.OutputAction.Result = "fail";
                            d.OutputAction.Message1 = "Incomplete line or payment data";
                            continue;
                        }
                        if (string.IsNullOrEmpty(d.Head.CreatedByApp)) {
                            d.Head.CreatedByApp = "androidv1";
                        }

                        try {

                            d.OutputAction = POSService.Save(d, false);
                            if (d.OutputAction.Result == "fail") {
                                d.Head.IsLink = false;
                                d.Head.LinkDate = null;
                                d.Head.LinkBy = "";
                            } else {
                                d.Head.IsLink = true;
                                d.Head.LinkDate = DateTime.Now;
                                d.Head.LinkBy = "Mr.Link";
                            }

                        } catch (Exception ex) {
                            d.Head.IsLink = false;
                            d.Head.LinkDate = null;
                            d.Head.LinkBy = "";
                            d.OutputAction.Result = "fail";
                            if (ex.InnerException != null) {
                                d.OutputAction.Message1 = ex.InnerException.ToString();
                            } else {
                                d.OutputAction.Message1 = ex.Message;
                            }
                        }
                    }
                }
                var rstk = POSService.CalStockEndDay(begin, end, rcom, com, 0);

            } catch (Exception ex) {

            }
            return bills;
        }
        public static List<IDGeneratorModel> UploadIDGenerator(List<IDGeneratorModel> data) {
            /*  I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" }*/
            ;
            try {
                using (GAEntities db = new GAEntities()) {

                    foreach (var d in data) {
                        try {
                            var query = db.IDGenerator.Where(o => o.Prefix == d.Prefix && o.Year == d.Year && o.Month == d.Month && o.MacNo == d.MacNo && o.FuncID == d.FuncID && o.RComID == d.RComID && o.ComID == d.ComID).FirstOrDefault();
                            if (query != null) {//มี record prefix นี้แล้ว ให้อัพเดท
                                if (Convert.ToInt32(query.DigitRunNumber) < Convert.ToInt32(d.DigitRunNumber)) {
                                    //ใน data server เลข running น้อยกว่าที่ upload ให้ update idgen
                                    query.DigitRunNumber = d.DigitRunNumber;
                                    query.LatestDate = d.LatestDate;
                                }
                            } else {//ยังไม่มี record prefix ให้ Insert
                                var ng = CopyModelToIDRunning(d);
                                db.IDGenerator.Add(ng);
                            }
                            db.SaveChanges();
                            d.IsUpload = true;
                        } catch (Exception ex) {
                            d.IsUpload = false;
                            //result.Result = "fail";
                            //if (ex.InnerException != null) {
                            //    result.Message1 = result.Message1 +" "+ex.InnerException.ToString();
                            //} else {
                            //    result.Message1 = result.Message1 + " " + ex.Message;
                            //}
                        }

                    }
                }
            } catch (Exception ex) {

            }
            return data;
        }


        #region Convert and copy
        public static POS_SaleHead CopyHead(POS_SaleHead i) {
            POS_SaleHead n = new POS_SaleHead();

            n.RComID = i.RComID;
            n.ComID = i.ComID;
            n.BillID = i.BillID;
            n.INVID = i.INVID;
            n.FINVID = i.FINVID;
            n.DocTypeID = i.DocTypeID;
            n.MacNo = i.MacNo;
            n.MacTaxNo = i.MacTaxNo;
            n.BillDate = i.BillDate;
            n.BillRefID = i.BillRefID;
            n.INVPeriod = i.INVPeriod;
            n.ReasonID = i.ReasonID;
            n.TableID = i.TableID;
            n.TableName = i.TableName;
            n.CustomerID = i.CustomerID;
            n.CustomerName = i.CustomerName;
            n.CustAccGroup = i.CustAccGroup;
            n.CustTaxID = i.CustTaxID;
            n.CustBranchID = i.CustBranchID;
            n.CustBranchName = i.CustBranchName;
            n.CustAddr1 = i.CustAddr1;
            n.CustAddr2 = i.CustAddr2;
            n.IsVatRegister = i.IsVatRegister;
            n.POID = i.POID;
            n.ShipToLocID = i.ShipToLocID;
            n.ShipToLocName = i.ShipToLocName;
            n.ShipToUsePrice = i.ShipToUsePrice;
            n.ShipToAddr1 = i.ShipToAddr1;
            n.ShipToAddr2 = i.ShipToAddr2;
            n.SalesID1 = i.SalesID1;
            n.SalesID2 = i.SalesID2;
            n.SalesID3 = i.SalesID3;
            n.SalesID4 = i.SalesID4;
            n.SalesID5 = i.SalesID5;
            n.ContactTo = i.ContactTo;
            n.Currency = i.Currency;
            n.RateExchange = i.RateExchange;
            n.RateBy = i.RateBy;
            n.RateDate = i.RateDate;
            n.CreditTermID = i.CreditTermID;
            n.Qty = i.Qty;
            n.BaseNetTotalAmt = i.BaseNetTotalAmt;
            n.NetTotalAmt = i.NetTotalAmt;
            n.NetTotalVatAmt = i.NetTotalVatAmt;
            n.NetTotalAmtIncVat = i.NetTotalAmtIncVat;
            n.OntopDiscPer = i.OntopDiscPer;
            n.OntopDiscAmt = i.OntopDiscAmt;
            n.ItemDiscAmt = i.ItemDiscAmt;
            n.DiscCalBy = i.DiscCalBy;
            n.LineDisc = i.LineDisc;
            n.VatRate = i.VatRate;
            n.VatTypeID = i.VatTypeID;
            n.IsVatInPrice = i.IsVatInPrice;
            n.NetTotalAfterRound = i.NetTotalAfterRound;
            n.NetDiff = i.NetDiff;
            n.PayByCash = i.PayByCash;
            n.PayByOther = i.PayByOther;
            n.PayTotalAmt = i.PayTotalAmt;
            n.PayMethodCount = i.PayMethodCount;
            n.Remark1 = i.Remark1;
            n.Remark2 = i.Remark2;
            n.LocID = i.LocID;
            n.SubLocID = i.SubLocID;
            n.CountLine = i.CountLine;
            n.IsLink = i.IsLink;
            n.LinkBy = i.LinkBy;
            n.LinkDate = i.LinkDate;
            n.Status = i.Status;
            n.IsPrint = i.IsPrint;
            n.PrintDate = i.PrintDate;
            n.CreatedByApp = i.CreatedByApp;
            n.CreatedBy = i.CreatedBy;
            n.CreatedDate = i.CreatedDate;
            n.ModifiedBy = i.ModifiedBy;
            n.ModifiedDate = i.ModifiedDate;
            n.IsActive = i.IsActive;

            return n;
        }
        public static List<POS_SaleLine> CopyLine(List<POS_SaleLine> line) {
            List<POS_SaleLine> r = new List<POS_SaleLine>();
            foreach (var i in line) {
                POS_SaleLine n = new POS_SaleLine();
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.MacNo = i.MacNo;
                n.LineNum = i.LineNum;
                n.RefLineNum = i.RefLineNum;
                n.BillID = i.BillID;
                n.INVID = i.INVID;
                n.FINVID = i.FINVID;
                n.BelongToLineNum = i.BelongToLineNum;
                n.DocTypeID = i.DocTypeID;
                n.BillDate = i.BillDate;
                n.BillRefID = i.BillRefID;
                n.CustID = i.CustID;
                n.TableID = i.TableID;
                n.TableName = i.TableName;
                n.Barcode = i.Barcode;
                n.ItemID = i.ItemID;
                n.ItemName = i.ItemName;
                n.ItemTypeID = i.ItemTypeID;
                n.ItemCateID = i.ItemCateID;
                n.ItemGroupID = i.ItemGroupID;
                n.IsStockItem = i.IsStockItem;
                n.Weight = i.Weight;
                n.WUnit = i.WUnit;
                n.Unit = i.Unit;
                n.Qty = i.Qty;
                n.Cost = i.Cost;
                n.Price = i.Price;
                n.PriceIncVat = i.PriceIncVat;
                n.BaseTotalAmt = i.BaseTotalAmt;
                n.TotalAmt = i.TotalAmt;
                n.VatAmt = i.VatAmt;
                n.TotalAmtIncVat = i.TotalAmtIncVat;
                n.VatRate = i.VatRate;
                n.VatTypeID = i.VatTypeID;
                n.OntopDiscAmt = i.OntopDiscAmt;
                n.OntopDiscPer = i.OntopDiscPer;
                n.DiscPer = i.DiscPer;
                n.DiscAmt = i.DiscAmt;
                n.DiscCalBy = i.DiscCalBy;
                n.IsFree = i.IsFree;
                n.ShareGpPer = i.ShareGpPer;
                n.IsOntopItem = i.IsOntopItem;
                n.PromotionID = i.PromotionID;
                n.PatternID = i.PatternID;
                n.PaternValue = i.PaternValue;
                n.MatchingNumber = i.MatchingNumber;
                n.ProPackCode = i.ProPackCode;
                n.IsProCompleted = i.IsProCompleted;
                n.LocID = i.LocID;
                n.SubLocID = i.SubLocID;
                n.LotNo = i.LotNo;
                n.SerialNo = i.SerialNo;
                n.BatchNo = i.BatchNo;
                n.Remark = i.Remark;
                n.Memo = i.Memo;
                n.ImgUrl = i.ImgUrl;
                n.Sort = i.Sort;
                n.Status = i.Status;
                n.CreatedDate = i.CreatedDate;
                n.ModifiedDate = i.ModifiedDate;
                n.IsActive = i.IsActive;
                r.Add(n);
            }
            return r;
        }

        public static List<POS_SalePayment> CopyPayment(List<POS_SalePayment> line) {
            List<POS_SalePayment> r = new List<POS_SalePayment>();
            foreach (var i in line) {
                POS_SalePayment n = new POS_SalePayment();
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.BillID = i.BillID;
                n.PaymentType = i.PaymentType;
                n.INVID = i.INVID;
                n.FINVID = i.FINVID;
                n.BillAmt = i.BillAmt;
                n.RoundAmt = i.RoundAmt;
                n.GetAmt = i.GetAmt;
                n.PayAmt = i.PayAmt;
                n.ChangeAmt = i.ChangeAmt;
                n.CreatedDate = i.CreatedDate;
                n.ModifiedDate = i.ModifiedDate;
                n.IsActive = i.IsActive;
                r.Add(n);
            }
            return r;
        }
        public static IDGenerator CopyModelToIDRunning(IDGeneratorModel i) {
            IDGenerator n = new IDGenerator();

            n.RComID = i.RComID;
            n.ComID = i.ComID;
            n.Prefix = i.Prefix;
            n.Year = i.Year;
            n.Month = i.Month;
            n.MacNo = i.MacNo;
            n.FuncID = i.FuncID;
            n.DigitRunNumber = i.DigitRunNumber;
            n.Description = i.Description;
            n.CreatedDate = i.CreatedDate;
            n.LatestDate = i.LatestDate;
            return n;
        }
        public static List<IDGeneratorModel> CopyIDRunningToModel(List<IDGenerator> data) {
            List<IDGeneratorModel> output = new List<IDGeneratorModel>();
            foreach (var i in data) {
                IDGeneratorModel n = new IDGeneratorModel();
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.Prefix = i.Prefix;
                n.Year = i.Year;
                n.Month = i.Month;
                n.MacNo = i.MacNo;
                n.FuncID = i.FuncID;
                n.DigitRunNumber = i.DigitRunNumber;
                n.Description = i.Description;
                n.CreatedDate = i.CreatedDate;
                n.LatestDate = i.LatestDate;
                n.IsUpload = true;
                output.Add(n);
            }

            return output;
        }
        #endregion
        #region stock managment
        public static I_BasicResult CalStock(POS_SaleHead h) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //var h = doc.Head;
                using (GAEntities db = new GAEntities()) {
                    if (h.INVID != "") {
                        using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                            //var dynamicParameters = new DynamicParameters();
                            //dynamicParameters.Add("BillID", h.BillID);
                            var strSQL = "exec [SP_CalStkSaleMove] @docid, @doctype, @rcompany, @company";
                            var values = new { docid = h.BillID, doctype = h.DocTypeID, rcompany = h.RComID, company = h.ComID };
                            var rquery = connection.Query(strSQL, values);

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

    }
}
