using RobotWasm.Server.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using Dapper;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.DA;
using System.Data;
using RobotWasm.Shared.Data.ML.Shared;
using static RobotWasm.Shared.Data.DA.SOFuncService;
using RobotWasm.Server.Data.DA.Line;
using RobotWasm.Shared.Data.ML.Promotion;
using RobotWasm.Shared.Data.ML.Order;

namespace RobotWasm.Server.Data.DA.Order.SO {
    public class SOService {

        #region Query Transaction
        public static I_SODocSet GetDocSet(string docid, string rcom, string com) {
            I_SODocSet doc = new I_SODocSet();
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.Head = db.vw_OSOHead.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    doc.Line = db.vw_OSOLine.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).ToList();
                    foreach (var n in doc.Line) {
                        if (string.IsNullOrEmpty(n.ImageUrl)) {
                            n.ImageUrl = @"/img/no_image.png";
                        } else {
                            n.ImageUrl = n.ImageUrl;
                        }
                    }

                    doc.Lot = db.vw_OSOLot.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).ToList();
                }
            } catch (Exception ex) {

            }
            return doc;
        }

        public static List<vw_OSOHead> ListDoc(I_SODocFiterSet f) {
            List<vw_OSOHead> result = new List<vw_OSOHead>();
            bool isLink = false;
            if (f.Status == "COMPLETED") {
                f.Status = "";
                  isLink = true;
            }

            using (GAEntities db = new GAEntities()) {
                if (!string.IsNullOrEmpty(f.SearchText)) {
                    result = db.vw_OSOHead.Where(o =>
                        (
                               o.OrdID.Contains(f.SearchText)
                            || o.ComID.Contains(f.SearchText)
                            || o.CustID.Contains(f.SearchText)
                            || f.SearchText == ""
                        )
                        && (o.SalesID1 == f.LockShowInSale || f.LockShowInSale == "")
                        && (o.DocTypeID == f.DocType || f.DocType == "")
                        && (o.Status == f.Status || f.Status == "")
                            && (o.IsActive == true)
                            ).OrderByDescending(o => o.CreatedDate).ToList();
                } else {
                    result = db.vw_OSOHead.Where(o =>
                                        (o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo
                                        && (o.DocTypeID == f.DocType || f.DocType == "")
                                        && (o.Status == f.Status || f.Status == "")
                                        && (o.SalesID1 == f.LockShowInSale || f.LockShowInSale == "")
                                        && o.IsActive == true)
                        ).OrderByDescending(o => o.CreatedDate).ToList();
                }
            }
            if ( isLink) {
                result = result.Where(o => o.IsLink == true).ToList();
            }
            return result;
        }

        public static int ListNotificationStatus(string username) {
            int result = 0;
            List<string> showStatus = new List<string> { "WAIT", "OPEN" };
            using (GAEntities db = new GAEntities()) {
                //result = db.OSOHead.Where(o => showStatus.Contains(o.Status)).Count();
                result = db.OSOHead.Where(o => o.IsActive == true && o.IsLink == false && o.SalesID1 == username).Count();
            }
            return result;
        }

        //public static vw_OSOLine GetLineActive(int linenum, List<vw_OSOLine> line) {
        //    vw_OSOLine result = new vw_OSOLine();
        //    result = line.Where(o => o.LineNum == linenum).FirstOrDefault();

        //    return result;
        //}
        //public static I_PromotionSet GetPromotion(string rcom, string com, string proId) {
        //    I_PromotionSet result = new I_PromotionSet();
        //    try {
        //        using (GAEntities db = new GAEntities()) {
        //            result.Promotion = db.Promotions.Where(o => o.RCompanyID == rcom
        //                                                    && o.CompanyID == com
        //                                                    && o.ProID == proId
        //                                                    && o.IsActive == true
        //                                                    ).FirstOrDefault();
        //            var items = db.vw_PromotionItemWithPattern.Where(o => o.RCompanyID == rcom
        //                                                    && o.CompanyID == com
        //                                                    && o.ProID == proId
        //                                                    && o.IsActive == true
        //                                                    ).ToList();
        //            //result.PromotionItems=db.PromotionItem.Where(o => o.RCompanyID == rcom
        //            //                                        && o.CompanyID == com
        //            //                                        && o.ProID == proId
        //            //                                        && o.IsActive == true
        //            //                                        ).ToList();
        //        }
        //    } catch (Exception ex) {
        //    }
        //    return result;
        //}
        public static I_PromotionSet ListSaleItem(string rcom, string com, string brandid, string cateid, string locid, string proid) {
            brandid = brandid == null ? "" : brandid;
            cateid = cateid == null ? "" : cateid;
            I_PromotionSet result = new I_PromotionSet();
            using (GAEntities db = new GAEntities()) {

                result.Promotion = db.Promotions.Where(o => o.RCompanyID == rcom
                                                         && o.CompanyID == com
                                                         && o.ProID == proid
                                                         && o.IsActive == true
                                                         ).FirstOrDefault();
                //  var group_Stkbal = Stkbal.GroupBy(x => x.ItemID).Select(g => g.First());

                if (result.Promotion.PatternID == "P000") {
                    var Stkbal = db.vw_ItemInfoWithPhotoAndStock.Where(o =>
                                                      (o.BrandID == brandid || brandid == "")
                                                      && (o.CateID == cateid || cateid == "")
                                                      && o.LocID == locid
                                                      ).ToList();
                    result.PromotionItems = new List<ItemDesplay>();
                    foreach (var l in Stkbal) {
                        ItemDesplay n = new ItemDesplay();
                        n.ItemID = l.ItemID;
                        n.ItemName = l.Name1;
                        n.LocID = l.LocID;
                        n.TypeID = l.TypeID;
                        n.CateID = l.CateID;
                        n.Price = l.Price;
                        n.PriceIncVat = l.PriceIncVat;
                        n.PriceProIncVat = l.PriceProIncVat;
                        n.IsSpecialPrice = l.IsSpecialPrice;
                        n.BrandID = l.BrandID;
                        n.Qty = 1;
                        if (string.IsNullOrEmpty(l.PhotoUrl)) {
                            n.ImageUrl = @"/img/no_image.png";
                        } else {
                            n.ImageUrl = l.PhotoUrl;
                        }
                        n.BalQty = l.BalQty;
                        n.DiscAmt = 0;
                        n.UnitID = l.UnitID;
                        n.ProID = "P000";
                        n.ProName = "ราคาปกติ";
                        n.PatternID = "PRO000";
                        result.PromotionItems.Add(n);
                    }
                }
                if (result.Promotion.PatternID == "P100") {
                    result.PromotionItems = new List<ItemDesplay>();
                    var items = db.vw_PromotionItemWithPattern.Where(o =>
                                                  (o.BrandID == brandid || brandid == "")
                                                  && (o.CateID == cateid || cateid == "")
                                                  && o.LocID == locid
                                                  && o.ProID == proid
                                                  ).ToList();
                    foreach (var l in items) {
                        ItemDesplay n = new ItemDesplay();
                        n.ItemID = l.ItemID;
                        n.ItemName = l.Name1;
                        n.LocID = l.LocID;
                        n.TypeID = l.TypeID;
                        n.CateID = l.CateID;
                        n.Price = l.Price;
                        n.PriceIncVat = 0;
                        n.PriceProIncVat = result.Promotion.YValue;
                        n.IsSpecialPrice = true;
                        n.BrandID = l.BrandID;
                        n.Qty = 1;
                        if (string.IsNullOrEmpty(l.PhotoUrl)) {
                            n.ImageUrl = @"/img/no_image.png";
                        } else {
                            n.ImageUrl = l.PhotoUrl;
                        }
                        n.BalQty = l.BalQty;
                        n.DiscAmt = 0;
                        n.UnitID = l.UnitID;
                        n.ProID = result.Promotion.ProID;
                        n.ProName = result.Promotion.ProDesc;
                        n.PatternID = result.Promotion.PatternID;
                        result.PromotionItems.Add(n);
                    }
                }

            }
            return result;
        }

        #endregion

        #region save

        public static I_BasicResult Save(I_SODocSet input, string action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //  input = CalDocSet(input);
                var head = input.Head;
                var chk_stock = CheckStock(input, action);
                if (chk_stock.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = chk_stock.Message1;
                    return result;
                }
                using (GAEntities db = new GAEntities()) {

                    if (action == "insert") {
                        if (head.OrdID == "") {

                            head.OrdID = GenDoc.IDRuunerService.GetNewIDV2(head.DocTypeID, head.RComID, input.Head.ComID, head.OrdDate, false, "th")[1];
                            result.Message2 = head.OrdID;
                            //    input = CalDocSet(input);
                        }
                        input = CalDocSet(input);
                        var sohead = SOFuncService.Convert2SOHhead(input.Head);
                        var solines = SOFuncService.Convert2SOLine(input.Line);
                        var solot = SOFuncService.Convert2SOLot(input.Lot);
                        db.OSOHead.Add(sohead);
                        db.OSOLine.AddRange(solines);
                        db.OSOLot.AddRange(solot);
                        db.SaveChanges();
                        GenDoc.IDRuunerService.GetNewIDV2(head.DocTypeID, head.RComID, input.Head.ComID, head.OrdDate, true, "th");
                        SendLine(sohead.RComID, sohead.ComID, sohead.OrdID,sohead.CustName, "new");
                        //TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.OrdID, TableID = "OORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "INSERT NEW ORDER" }, rcom, input.Head.ComID, user);
                    } else {
                        input = CalDocSet(input);
                        var uh = db.OSOHead.Where(o => o.OrdID == input.Head.OrdID && o.RComID == input.Head.RComID && o.ComID == input.Head.ComID).FirstOrDefault();
                        uh.OrdDate = input.Head.OrdDate;
                        uh.CustID = input.Head.CustID;
                        uh.ComID = input.Head.ComID;
                        uh.RComID = input.Head.RComID;
                        uh.CustName = input.Head.CustName;
                        uh.CustAddr1 = input.Head.CustAddr1;
                        uh.CustAddr2 = input.Head.CustAddr2;
                        uh.CustMobile = input.Head.CustMobile;
                        uh.CustEmail = input.Head.CustEmail;
                        uh.BillAddr1 = input.Head.BillAddr1;
                        uh.BillAddr2 = input.Head.BillAddr2;
                        uh.RefDocID = input.Head.RefDocID;
                        uh.AccGroupID = input.Head.AccGroupID;
                        uh.BrandID = input.Head.BrandID;
                        uh.POID = input.Head.POID;
                        uh.PODate = input.Head.PODate;
                        uh.ShipID = input.Head.ShipID;
                        uh.ShipDate = input.Head.ShipDate;
                        uh.BillToCustID = input.Head.BillToCustID;
                        uh.ShipFrLocID = input.Head.ShipFrLocID;
                        uh.ShipFrSubLocID = input.Head.ShipFrSubLocID;
                        uh.SalesID1 = input.Head.SalesID1;
                        uh.SalesID2 = input.Head.SalesID2;
                        uh.Currency = input.Head.Currency;
                        uh.RateExchange = input.Head.RateExchange;
                        uh.RateBy = input.Head.RateBy;
                        uh.RateDate = input.Head.RateDate;
                        uh.TermID = input.Head.TermID;
                        uh.PayDueDate = input.Head.PayDueDate;
                        uh.QtyInvoice = input.Head.QtyInvoice;
                        uh.Qty = input.Head.Qty;
                        uh.QtyShip = input.Head.QtyShip;
                        uh.QtyReturn = input.Head.QtyReturn;
                        uh.ItemDiscAmt = input.Head.ItemDiscAmt;
                        uh.ItemDiscAmtIncVat = input.Head.ItemDiscAmtIncVat;
                        uh.CustTaxID = input.Head.CustTaxID;
                        uh.CustBrnID = input.Head.CustBrnID;
                        uh.CustBrnName = input.Head.CustBrnName;
                        uh.QtyInvoice = input.Head.QtyInvoice;
                        uh.QtyInvoicePending = input.Head.QtyInvoicePending;
                        uh.QtyShipPending = input.Head.QtyShipPending;
                        uh.AmtInvoicePending = input.Head.AmtInvoicePending;
                        uh.AmtShipPending = input.Head.AmtShipPending;
                        uh.Status = input.Head.Status;
                        uh.CountLine = input.Head.CountLine;
                        uh.NetTotalAmt = input.Head.NetTotalAmt;
                        uh.NetTotalVatAmt = input.Head.NetTotalVatAmt;
                        uh.NetTotalAmtIncVat = input.Head.NetTotalAmtIncVat;
                        uh.BaseNetTotalAmt = input.Head.BaseNetTotalAmt;
                        uh.BaseNetTotalAmtIncVat = input.Head.BaseNetTotalAmtIncVat;
                        uh.OntopDiscPer = input.Head.OntopDiscPer;
                        uh.OntopDiscAmt = input.Head.OntopDiscAmt;
                        uh.OntopDiscAmtIncVat = input.Head.OntopDiscAmtIncVat;
                        uh.DiscCalBy = input.Head.DiscCalBy;
                        uh.VatRate = input.Head.VatRate;
                        uh.VatTypeID = input.Head.VatTypeID;
                        uh.Remark1 = input.Head.Remark1;
                        uh.Remark2 = input.Head.Remark2;
                        uh.TName = input.Head.TName;
                        uh.AName = input.Head.AName;
                        uh.PName = input.Head.PName;
                        uh.IsLockPrice=input.Head.IsLockPrice;
                        uh.PaymentMemo = input.Head.PaymentMemo;
                        uh.ModifiedBy = input.Head.ModifiedBy;
                        uh.ModifiedDate = DateTime.Now;
                        var sohead = SOFuncService.Convert2SOHhead(input.Head);
                        var solines = SOFuncService.Convert2SOLine(input.Line);
                        var solot = SOFuncService.Convert2SOLot(input.Lot);
                        db.OSOLine.RemoveRange(db.OSOLine.Where(o => o.OrdID == uh.OrdID && o.RComID == uh.RComID && o.ComID == uh.ComID));
                        db.OSOLine.AddRange(solines);
                        db.OSOLot.RemoveRange(db.OSOLot.Where(o => o.OrdID == uh.OrdID && o.RComID == uh.RComID && o.ComID == uh.ComID));
                        db.OSOLot.AddRange(solot);
                        db.SaveChanges();

                        //TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.OrdID, TableID = "OORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "UPDATE ORDER" }, rcom, input.Head.ComID, user);
                    }
                    var r_st = CalStock(head.OrdID, head.DocTypeID, head.RComID, head.ComID);
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        public static I_BasicResult CheckStock(I_SODocSet input, string action) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string rcom = input.Head.RComID;
                string com = input.Head.ComID;
                string soId = input.Head.OrdID;
                string locId = input.Head.ShipFrLocID;
                using (GAEntities db = new GAEntities()) {

                    if (action == "insert") {
                        List<string> itemIds = input.Line.Select(o => o.ItemID).Distinct().ToList();
                        var in_stock = db.STKBal.Where(o => o.LocID == locId && itemIds.Contains(o.ItemID)).ToList();
                        var in_doc = input.Line.GroupBy(p => p.ItemID)
                                       .Select(g => new {
                                           ItemID = g.Key,
                                           Qty = g.Sum(p => p.Qty)
                                       });
                        foreach (var doc in in_doc) {
                            var stk = in_stock.Where(o => o.ItemID == doc.ItemID).FirstOrDefault();
                            if (stk.BalQty < doc.Qty) {
                                r.Result = "fail";
                                r.Message1 = r.Message1 + stk.ItemID + " คงเหลือไม่พอในขณะนี้ <br>";
                            }
                        }
                    } else {
                        List<string> itemIds = input.Line.Select(o => o.ItemID).Distinct().ToList();
                        var in_stock = db.STKBal.Where(o => o.LocID == locId && itemIds.Contains(o.ItemID)).ToList();
                        var in_db = db.OSOLine.Where(o => o.RComID == rcom && o.ComID == com && o.OrdID == soId && o.IsActive == true)
                                               .GroupBy(p => p.ItemID) // Group by Category
                                               .Select(g => new {
                                                   ItemID = g.Key,
                                                   Qty = g.Sum(p => p.Qty)
                                               });
                        var in_doc = input.Line.GroupBy(p => p.ItemID)
                                               .Select(g => new {
                                                   ItemID = g.Key,
                                                   Qty = g.Sum(p => p.Qty)
                                               });
                        foreach (var st in in_stock) {
                            var d = in_db.Where(o => o.ItemID == st.ItemID).FirstOrDefault();
                            if (d != null) {
                                st.BalQty = st.BalQty + d.Qty;
                            }
                        }
                        foreach (var doc in in_doc) {
                            var stk = in_stock.Where(o => o.ItemID == doc.ItemID).FirstOrDefault();
                            if (stk.BalQty < doc.Qty) {
                                r.Result = "fail";
                                r.Message1 = r.Message1 + stk.ItemID + " คงเหลือไม่พอในขณะนี้" + Environment.NewLine;
                            }
                        }
                    }

                }


            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;

        }
        public static I_BasicResult CalStock(string docid, string doctype, string rcom, string com) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {
                //using (SqlConnection conn = new SqlConnection(Globals.GAEntitiesConn)) { 
                //    conn.Open();
                //    string sql =string.Format(@" exec SP_CalStkOrder  '{0}', '{1}', '{2}', '{3}' ", docid,doctype,rcom,com);
                //var result_command = conn.Execute(sql, commandType: CommandType.StoredProcedure); 
                //}

                using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                    var procedure = "[SP_CalStkOrder]";
                    var values = new { docid = docid, doctype = doctype, rcompany = rcom, company = com };
                    var query = connection.Query(procedure, values, commandType: CommandType.StoredProcedure).ToList();

                }


            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;

        }

        public static I_BasicResult LockOrder(int action) {
            //action =1=lock,0=unlock,2=getstatus
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "N" };
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {

                using (GAEntities db = new GAEntities()) {
                    var query = db.Config.Where(o => o.ConfigID == "lock_order").FirstOrDefault();
                    if (query != null) {
                        if (action == 1) {
                            query.ValueString1 = "Yes";
                            db.SaveChanges();
                        } else if (action == 0) {
                            query.ValueString1 = "No";
                            db.SaveChanges();
                        } else {
                            r.Message2 = query.ValueString1;
                        }

                    }

                }


            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;

        }

        //public static I_SODocSet CalDocSet(I_SODocSet input) {
        //    var head = input.Head;
        //    var line = input.Line;
        //    var lot = input.Lot;
        //    foreach (var l in line) {
        //        //1.cal line base total amt
        //        l.BaseTotalAmt = l.Qty * l.Price;
        //        //2. cal disc amt
        //        if (l.DiscCalBy == "P") {
        //            l.DiscAmt = (l.BaseTotalAmt * l.DiscPer) / 100;
        //        }
        //        //3. cal disc per
        //        if (l.DiscCalBy == "A") {
        //            l.DiscPer = 0;
        //            if (l.BaseTotalAmt != 0) {
        //                l.DiscPer = (100 * l.DiscAmt) / l.BaseTotalAmt;
        //            }
        //        }
        //    }

        //    var line_v = line.Where(o => o.ItemTypeID != "MISC");

        //    //5. cal head base total amt
        //    var head_amt_aft_dis = line_v.Sum(o => o.BaseTotalAmt) - line_v.Sum(o => o.DiscAmt);
        //    head.BaseNetTotalAmt = head_amt_aft_dis;
        //    //6. cal head disc amt
        //    if (head.DiscCalBy == "P") {
        //        head.OntopDiscAmt = (head.BaseNetTotalAmt * head.OntopDiscPer) / 100;
        //    }
        //    //7. cal head disc per
        //    if (head.DiscCalBy == "A") {
        //        if (head.BaseNetTotalAmt != 0) {
        //            head.OntopDiscPer = (100 * head.OntopDiscAmt) / head.BaseNetTotalAmt;
        //        }
        //    }

        //    foreach (var l in line) {
        //        //12. copy head to line
        //        l.OrdID = head.OrdID;
        //        l.OrdDate = head.OrdDate;
        //        l.INVID = head.INVID;
        //        l.INVDate = head.INVDate;
        //        l.RComID = head.RComID;
        //        l.ComID = head.ComID;
        //        l.DocTypeID = head.DocTypeID;
        //        l.CustID = head.CustID;

        //        //4 เผื่อกรณี โปรแกรม error ไม่มีค่า Vattype ในระดับ Line
        //        if (string.IsNullOrEmpty(l.VatTypeID)) {
        //            l.VatRate = head.VatRate;
        //            l.VatTypeID = head.VatTypeID;
        //        }

        //        //if (l.ItemTypeID != "MISC") {
        //        //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
        //        l.OntopDiscPer = head.OntopDiscPer;
        //        l.OntopDiscAmt = Math.Round(head.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);


        //        //8.cal line disc weight ontop percent & amt

        //        if (head.BaseNetTotalAmt != 0) {
        //            l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * head.OntopDiscAmt) / head.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
        //            l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
        //        }
        //        //9 cal line total amt 
        //        l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
        //        //10.cal line vat amt
        //        l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 2, MidpointRounding.AwayFromZero);
        //        //11.cal line nettotal amt inc vat


        //    }
        //    foreach (var l in lot) {
        //        l.CustID = head.CustID;
        //        l.OrdDate = head.OrdDate;
        //        l.OrdID = head.OrdID;
        //        l.DocTypeID = head.DocTypeID;
        //        l.RComID = head.RComID;
        //        l.ComID = head.ComID;

        //    }

        //    //13.copy line to head

        //    head.NetTotalAmt = Math.Round(line_v.Sum(o => o.TotalAmt), 2, MidpointRounding.AwayFromZero);
        //    head.NetTotalVatAmt = Math.Round(line_v.Sum(o => o.VatAmt), 2, MidpointRounding.AwayFromZero);
        //    head.NetTotalAmtIncVat = Math.Round(line_v.Sum(o => o.TotalAmtIncVat), 2, MidpointRounding.AwayFromZero);
        //    head.Qty = line_v.Sum(o => o.Qty);
        //    head.CountLine = line_v.Count();
        //    return input;
        //}

        //public static I_ORDSet checkDupID(I_ORDSet input, string rcom, string com) {
        //    try {
        //        input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //        using (GAEntities db = new GAEntities()) {
        //            var h = input.Head;
        //            var get_id = db.OSOHead.Where(o => o.OrdID == h.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
        //            if (input.NeedRunNextID) {
        //                int i = 0;
        //                while (get_id != null) {
        //                    if (i > 1000) {
        //                        input.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
        //                        break;
        //                    }
        //                    i++;

        //                    IDRuunerService.GetNewIDV2("OORDER", rcom, input.Head.ComID, input.Head.OrdDate, true, "th");
        //                    h.OrdID = IDRuunerService.GetNewIDV2("OORDER", rcom, input.Head.ComID, input.Head.OrdDate, false, "th")[1];
        //                    get_id = db.OSOHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.OrdID == h.OrdID).FirstOrDefault();
        //                }
        //            } else {
        //                if (get_id != null) {
        //                    input.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Duplication Order number " + h.OrdID, Message2 = "" };
        //                }
        //            }

        //        }
        //        input = CalDocSet(input);

        //    } catch (Exception ex) {
        //        input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
        //    }
        //    return input;
        //}

        #endregion

        #region Delete

        public static I_BasicResult DeleteDoc(string docId, string rcom, string com, string modified_by) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.OSOHead.Where(o => o.OrdID == docId && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    var line = db.OSOLine.Where(o => o.OrdID == docId && o.RComID == rcom && o.ComID == com).ToList();
                    var lot = db.OSOLot.Where(o => o.OrdID == docId && o.RComID == rcom && o.ComID == com).ToList();
                    head.Status = "CANCEL";
                    head.ModifiedBy = modified_by;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                        l.Status = "CANCEL";
                    }
                    foreach (var l in lot) {
                        l.IsActive = false;
                        l.Status = "CANCEL";
                    }
                    db.SaveChanges();
                    var r_st = CalStock(head.OrdID, head.DocTypeID, head.RComID, head.ComID);
                    SendLine(head.RComID, head.ComID, head.OrdID,head.CustName, "delete");
                    //TransactionService.SaveLog(new TransactionLog { TransactionID = head.OrdID, TableID = "OORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = head.ComID, Action = "DELETE ORDER" }, rcom, head.ComID, createby);
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

        //public static I_SODocSet DeleteLine(int linenum, I_SODocSet input) {
        //    try {
        //        input.Line.RemoveAll(o => o.LineNum == linenum);
        //        input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" }; 
        //    } catch (Exception ex) {
        //        input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
        //    } 
        //    return input;
        //}

        #endregion


        public static GeoThaiLand GetGeoThailand(decimal? lat_, decimal? lon_) {
            //หา ตำบล อำเภอ จังหวัดจาก gps
            GeoThaiLand result = new GeoThaiLand();
            try {
                using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                    var dynamicParameters = new DynamicParameters();
                    string sql = String.Format(@" exec [dbo].[sp_get_nearest_tambol]  {0},{1} ", lat_, lon_);
                    result = connection.Query<GeoThaiLand>(sql).FirstOrDefault();
                }
            } catch (Exception e) { }
            return result;
        }


        #region promotion
        public static List<Promotions> ListPromotion(string rcom, string com, DateTime tranDate) {
            List<Promotions> result = new List<Promotions>();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.Promotions.Where(o => o.RCompanyID == rcom
                                                            && o.CompanyID == com
                                                            && o.IsActive == true
                                                            && (tranDate >= o.DateBegin && tranDate <= o.DateEnd)
                                                            ).OrderBy(o => o.ProID).ToList();
                }
            } catch (Exception ex) {
            }
            return result;
        }

        #endregion



        #region Line Notify
        public static I_BasicResult SendLine(string rcom, string com, string docid, string cust,string msg_type) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.vw_OSOHead.Where(o => o.RComID == rcom && o.ComID == com && o.OrdID == docid).FirstOrDefault();
                    if (query != null) {
                        string msg = "";
                        if (msg_type.ToLower() == "new") {
                            msg = msg + query.SalesName + "\n เปิดออเดอร์เลข " + query.OrdID +"\n ให้ "+cust+ "\n เมื่อ " + query.CreatedDate.ToString("dd/MM/yyyy HH:mm");
                        }
                        if (msg_type.ToLower() == "delete") {
                            msg = msg + query.ModifiedBy + "\n ลบออเดอร์เลข " + query.OrdID + "\n ให้ " + cust + "\n เมื่อ " + Convert.ToDateTime(query.ModifiedDate).ToString("dd/MM/yyyy HH:mm");
                        }

                        result = LineNotify.SendLineGroupMsgOnly(rcom, msg, "OMS-ORDER");
                    }
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
        #endregion
    }
}
