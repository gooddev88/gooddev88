using Dapper;
using Robot.Data.DA.Document;
using Robot.Data.GADB.TT;
using static Robot.Data.ML.I_Result;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq;
using Robot.Data.ML.Shared;
using System.Data.SqlClient;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.SessionStorage;
using System.Threading.Tasks;
using Robot.Data.ML;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using System.Net.Http.Json;

namespace Robot.Data.DA.Order.SO {
    public class SOService {
        private readonly HttpClient _http;
        public static string SessionActive_BrandID = "session_active_brandid";
        public static string SessionActive_SOID = "session_active_soid";

        public static string SessionLat = "session_lat";
        public static string SessionLon = "session_lon";
        public static string SessionArea = "session_area";


        public I_SODocSet DocSet { get; set; }
        public List<ItemDesplay> ListProduct { get; set; } = new List<ItemDesplay>();
        public vw_OSOHead SelectOrder { get; set; } = new vw_OSOHead();
        public string PreviousPageUrl { get; set; }

        #region Query Transaction
        public static I_SODocSet GetDocSet(string docid, string rcom, string com) {
            I_SODocSet doc = new I_SODocSet();
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.Head = db.vw_OSOHead.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    doc.Line = db.vw_OSOLine.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).ToList();
                    doc.LineActive = db.vw_OSOLine.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).FirstOrDefault();
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
            using (GAEntities db = new GAEntities()) {
                if (!string.IsNullOrEmpty(f.SearchText)) {
                    result = db.vw_OSOHead.Where(o =>
                        (
                               o.OrdID.Contains(f.SearchText) 
                            || o.CustID.Contains(f.SearchText)
                            || o.CustName.Contains(f.SearchText) 
                        )
                        && (o.DocTypeID == f.DocType || f.DocType == "")
                        && (o.Status == f.Status || f.Status=="")
                           
                            && (o.IsActive == true)
                            ).OrderByDescending(o => o.CreatedDate).ToList();
                } else {
                    result = db.vw_OSOHead.Where(o =>
                                        (o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo
                                        && (o.DocTypeID == f.DocType || f.DocType == "")
                                        && (o.Status == f.Status || f.Status == "")
                                        && o.IsActive == true)
                        ).OrderByDescending(o => o.CreatedDate).ToList();
                }
            }
            return result;
        }

        public static List<vw_OSOHead> ListPrintOrder(I_SODocFiterSet f) {
            List<vw_OSOHead> result = new List<vw_OSOHead>();
            using (GAEntities db = new GAEntities()) {
                if (!string.IsNullOrEmpty(f.SearchText)) {
                    result = db.vw_OSOHead.Where(o =>
                        (
                               o.OrdID.Contains(f.SearchText)
                                || o.SalesName.Contains(f.SearchText)
                            || o.ComID.Contains(f.SearchText)
                            || o.CustID.Contains(f.SearchText)
                            || f.SearchText == ""
                        )
                        && (o.Status!= "WAIT" && o.Status!="DELETE")
                        && (o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo)
                            //&& (o.DocTypeID == f.DocType || f.DocType == "")
                            //&& o.Status == "WAIT"
                            && o.IsPrint == f.ShowIsPrint
                            && (o.IsActive == true)
                            ).OrderByDescending(o => o.CreatedDate).ToList();
                } else {
                    result = db.vw_OSOHead.Where(o =>
                                        (o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo
                                        //&& (o.DocTypeID == f.DocType || f.DocType == "")
                                        //&& o.Status == "WAIT"
                                        && o.IsPrint == f.ShowIsPrint
                                         && (o.Status != "WAIT" && o.Status != "DELETE")
                                        && o.IsActive == true)
                        ).OrderByDescending(o => o.CreatedDate).ToList();
                }
            }
            return result;
        }

        public static List<vw_OSOHead> ListCancelOrder(I_SODocFiterSet f) {
            List<vw_OSOHead> result = new List<vw_OSOHead>();
            using (GAEntities db = new GAEntities()) {
                if (!string.IsNullOrEmpty(f.SearchText)) {
                    result = db.vw_OSOHead.Where(o =>
                        (
                               o.OrdID.Contains(f.SearchText)
                                || o.SalesName.Contains(f.SearchText)
                            || o.ComID.Contains(f.SearchText)
                            || o.CustID.Contains(f.SearchText)
                            || f.SearchText == ""
                        )
                        && (o.ModifiedDate >= f.DateFrom && o.ModifiedDate <= f.DateTo)
                            //&& (o.DocTypeID == f.DocType || f.DocType == "")
                            //&& o.Status == "WAIT"
                            && (o.IsActive == false)
                            ).OrderByDescending(o => o.CreatedDate).ToList();
                } else {
                    result = db.vw_OSOHead.Where(o =>
                                        (o.ModifiedDate >= f.DateFrom && o.ModifiedDate <= f.DateTo
                                        //&& (o.DocTypeID == f.DocType || f.DocType == "")
                                        //&& o.Status == "WAIT"
                                        && o.IsActive == false)
                        ).OrderByDescending(o => o.CreatedDate).ToList();
                }
            }
            return result;
        }
        public static int ListNotificationStatus() {
            int result = 0;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_OSOHead.Where(o => o.Status == "OPEN").ToList().Count();
            }
            return result;
        }

        //public static vw_OSOLine GetLineActive(int linenum, List<vw_OSOLine> line) {
        //    vw_OSOLine result = new vw_OSOLine();
        //    result = line.Where(o => o.LineNum == linenum).FirstOrDefault();

        //    return result;
        //}

        public static List<SelectOption> ListDocStatus() {
            List<SelectOption> output = new List<SelectOption>();
            output.Add(new SelectOption { Sort = 1, Value = "", Description = "ALL" });
            output.Add(new SelectOption { Sort = 2, Value = "OPEN", Description = "OPEN" });
            output.Add(new SelectOption { Sort = 3, Value = "CANCLE", Description = "CANCEL" });
            output.Add(new SelectOption { Sort = 4, Value = "WAIT", Description = "WAIT" });
            //output.Add(new SelectOption { Sort = 3, Value = "PENDING", Description = "PENDING" });
            //output.Add(new SelectOption { Sort = 4, Value = "COMPLETED", Description = "COMPLETED" }); 
            return output;
        }
        public static List<ItemDesplay> ListSaleItem(string brandid, string cateid) {
            brandid = brandid == null ? "" : brandid;
            cateid = cateid == null ? "" : cateid;
            List<ItemDesplay> result = new List<ItemDesplay>();
            using (GAEntities db = new GAEntities()) {
                var Stkbal = db.vw_ItemInfoWithPhotoAndStock.Where(o =>
                                                        (o.BrandID == brandid || brandid == "")
                                                        && (o.CateID == cateid || cateid == "")
                                                        && o.LocID == "STORE"
                                                        ).ToList();

                //  var group_Stkbal = Stkbal.GroupBy(x => x.ItemID).Select(g => g.First());


                foreach (var l in Stkbal) {
                    ItemDesplay n = new ItemDesplay();
                    n.ItemID = l.ItemID;
                    n.ItemName = l.Name1;
                    n.LocID = l.LocID;
                    n.TypeID = l.TypeID;
                    n.CateID = l.CateID;
                    n.Price = l.Price;
                    n.PriceIncVat = l.PriceIncVat;
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

                    //if (result.Count() == 2 || result.Count() == 4 || result.Count() == 6) {
                    //    n.ImageUrl = "/img/watch2.png";
                    //}
                    result.Add(n);
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

                using (GAEntities db = new GAEntities()) {

                    if (action == "insert") {
                        if (head.OrdID == "") {
                            List<string>? list_docid = IDRuunerService.GetNewIDV2(head.DocTypeID, head.RComID, input.Head.ComID, head.OrdDate, true, "th");
                            head.OrdID = list_docid[1];
                            result.Message2 = head.OrdID;
                            //    input = CalDocSet(input);
                        }
                        var sohead = SOService.Convert2SOHhead(input.Head);
                        var solines = SOService.Convert2SOLine(input.Line);
                        var solot = SOService.Convert2SOLot(input.Lot);
                        db.OSOHead.Add(sohead);
                        db.OSOLine.AddRange(solines);
                        db.OSOLot.AddRange(solot);
                        db.SaveChanges();
                        IDRuunerService.GetNewIDV2(head.DocTypeID, head.RComID, input.Head.ComID, head.OrdDate, true, "th");

                        //TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.OrdID, TableID = "OORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "INSERT NEW ORDER" }, rcom, input.Head.ComID, user);
                    } else {
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

                        uh.PaymentMemo = input.Head.PaymentMemo;
                        uh.ModifiedBy = input.Head.ModifiedBy;
                        uh.ModifiedDate = DateTime.Now;
                        var sohead = SOService.Convert2SOHhead(input.Head);
                        var solines = SOService.Convert2SOLine(input.Line);
                        var solot = SOService.Convert2SOLot(input.Lot);
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
            } catch (Exception e) {

            }
            return result;
        }




        #region SOFucnService


        public class I_SODocSet {
            public vw_OSOHead Head { get; set; }
            public List<vw_OSOLine> Line { get; set; }
            public vw_OSOLine LineActive { get; set; }
            public List<vw_OSOLot> Lot { get; set; }
            public vw_OSOLot LotActive { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class I_SODocFiterSet {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowActive { get; set; }
            public bool ShowIsPrint { get; set; }
        }

        public class ItemDesplay {
            public string ItemID { get; set; }
            public string ItemName { get; set; }
            public string BrandID { get; set; }
            public string LocID { get; set; }
            public string TypeID { get; set; }
            public string CateID { get; set; }
            public decimal Price { get; set; }
            public decimal PriceIncVat { get; set; }
            public decimal Qty { get; set; }
            public decimal DiscAmt { get; set; }
            public decimal BalQty { get; set; }
            public string ImageUrl { get; set; }
            public string UnitID { get; set; }
        }

        ISessionStorageService sessionStorage;

        public SOService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;
        }
        #region CalDocSet
        public static I_SODocSet CalDocSet(I_SODocSet input) {
            input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var head = input.Head;
                //var line = input.Line;
                //var lot = input.Lot;
                if (input.Line.Count > 0) {
                    foreach (var l in input.Line) {
                        //1.cal line base total amt
                        //l.BaseTotalAmt = l.Qty * l.Price;
                        l.Price = Math.Round((l.PriceIncVat * 100 / (100 + l.VatRate)), 3, MidpointRounding.AwayFromZero);
                        l.BaseTotalAmt = l.Qty * l.Price;
                        l.BaseTotalAmtIncVat = l.Qty * l.PriceIncVat;
                        //2. cal disc amt
                        if (l.DiscCalBy == "P") {
                            //l.DiscAmt = (l.BaseTotalAmt * l.DiscPer) / 100;
                            l.DiscPPU = Math.Round(((l.BaseTotalAmtIncVat * l.DiscPer) / 100) / l.Qty, 3, MidpointRounding.AwayFromZero); ;
                            //l.DiscAmtIncVat = (l.BaseTotalAmtIncVat * l.DiscPer) / 100;
                            l.DiscAmtIncVat = l.DiscPPU * l.Qty;
                            l.DiscAmt = Math.Round((l.DiscAmtIncVat * 100) / (100 + l.VatRate), 3, MidpointRounding.AwayFromZero);
                        }
                        //3. cal disc per
                        if (l.DiscCalBy == "A") {
                            l.DiscPer = 0;
                            //if (l.BaseTotalAmt != 0) {
                            //    l.DiscPer = (100 * l.DiscAmt) / l.BaseTotalAmt;
                            //}
                            if (l.BaseTotalAmtIncVat != 0) {
                                //l.DiscPPUIncVat = (l.DiscPPUIncVat * l.Qty);
                                l.DiscPPU = Math.Round((l.DiscPPUIncVat * 100) / (100 + l.VatRate), 3, MidpointRounding.AwayFromZero); ;
                                l.DiscAmtIncVat = (l.DiscPPUIncVat * l.Qty);
                                l.DiscAmt = Math.Round((l.DiscAmtIncVat * 100) / (100 + l.VatRate), 3, MidpointRounding.AwayFromZero);
                                l.DiscPer = Math.Round((100 * l.DiscAmtIncVat) / l.BaseTotalAmtIncVat, 3, MidpointRounding.AwayFromZero); ;
                            }
                        }
                    }
                }




                var line_v = input.Line.Where(o => o.ItemTypeID != "MISC" );

                //5. cal head base total amt
                //var head_amt_aft_dis = line_v.Sum(o => o.BaseTotalAmt) - line_v.Sum(o => o.DiscAmt);
                //head.BaseNetTotalAmt = head_amt_aft_dis;

                //6. cal head disc amt
                if (head.DiscCalBy == "P") {
                    //head.OntopDiscAmt = (head.BaseNetTotalAmt * head.OntopDiscPer) / 100;
                    head.OntopDiscAmtIncVat = (head.BaseNetTotalAmtIncVat * head.OntopDiscPer) / 100;
                }
                //7. cal head disc per
                if (head.DiscCalBy == "A") {
                    //if (head.BaseNetTotalAmt != 0) {
                    //    head.OntopDiscPer = (100 * head.OntopDiscAmt) / head.BaseNetTotalAmt;
                    //}
                    if (head.BaseNetTotalAmtIncVat != 0) {
                        head.OntopDiscPer = (100 * head.OntopDiscAmtIncVat) / head.BaseNetTotalAmtIncVat;
                    }
                }
                if (input.Line.Count > 0) {
                    foreach (var l in input.Line) {
                        //12. copy head to line
                        l.OrdID = head.OrdID;
                        l.OrdDate = head.OrdDate;
                        l.INVID = head.INVID;
                        l.INVDate = head.INVDate;
                        l.RComID = head.RComID;
                        l.ComID = head.ComID;
                        l.DocTypeID = head.DocTypeID;
                        l.CustID = head.CustID;

                        //4 เผื่อกรณี โปรแกรม error ไม่มีค่า Vattype ในระดับ Line
                        if (string.IsNullOrEmpty(l.VatTypeID)) {
                            l.VatRate = head.VatRate;
                            l.VatTypeID = head.VatTypeID;
                        }

                        //if (l.ItemTypeID != "MISC") {
                        //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
                        if (l.Status != "REJECT") { 
                        l.OntopDiscPer = head.OntopDiscPer;
                        //l.OntopDiscAmt = Math.Round(head.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
                        l.OntopDiscAmtIncVat = Math.Round(head.OntopDiscAmtIncVat, 3, MidpointRounding.AwayFromZero);

                        //8.cal line disc weight ontop percent & amt

                        //if (head.BaseNetTotalAmt != 0) {
                        //    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * head.OntopDiscAmt) / head.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                        //    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                        //}
                        if (head.BaseNetTotalAmtIncVat != 0) {
                            l.OntopDiscAmtIncVat = Math.Round(((l.BaseTotalAmtIncVat - l.DiscAmtIncVat) * head.OntopDiscAmtIncVat) / head.BaseNetTotalAmtIncVat, 3, MidpointRounding.AwayFromZero);
                            l.OntopDiscPer = (l.BaseTotalAmtIncVat - l.DiscAmtIncVat) == 0 ? 0 : (100 * l.OntopDiscAmtIncVat) / (l.BaseTotalAmtIncVat - l.DiscAmtIncVat);
                        }
                        }
                        //9 cal line total amt 
                        //l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
                        l.TotalAmtIncVat = Math.Round(l.BaseTotalAmtIncVat - l.DiscAmtIncVat - l.OntopDiscAmtIncVat, 3, MidpointRounding.AwayFromZero);
                        //10.cal line vat amt
                        //l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 2, MidpointRounding.AwayFromZero);
                        l.VatAmt = Math.Round((l.TotalAmtIncVat * l.VatRate) / (100 + l.VatRate), 2, MidpointRounding.AwayFromZero);
                        //11.cal line nettotal amt inc vat
                        //l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 2, MidpointRounding.AwayFromZero);
                        l.TotalAmt = Math.Round(l.TotalAmtIncVat - l.VatAmt, 2, MidpointRounding.AwayFromZero);
                        //12.set status discount approve

                        if (l.DiscAmtIncVat > 0) {
                            if (l.DiscApproveDate == null) {
                                l.Status = "WAIT";
                            }  
                        } else {
                            l.Status = "OK";
                        }
                    }
                }
                if (input.Lot.Count > 0) {
                    foreach (var l in input.Lot) {
                        var ll = line_v.Where(o => o.LineNum == l.LineLineNum).FirstOrDefault();
                        if (ll!=null) {
                            l.Status = ll.Status;
                            l.CustID = head.CustID;
                            l.OrdDate = head.OrdDate;
                            l.OrdID = head.OrdID;
                            l.DocTypeID = head.DocTypeID;
                            l.RComID = head.RComID;
                            l.ComID = head.ComID;
                        }
                  

                    }
                }


                //13.copy line to head 
                head.ItemDiscAmt = Math.Round(line_v.Where(o=>o.Status!="REJECT").Sum(o => o.DiscAmt), 2, MidpointRounding.AwayFromZero);
                head.ItemDiscAmtIncVat = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.DiscAmtIncVat), 2, MidpointRounding.AwayFromZero);
                head.BaseNetTotalAmtIncVat = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.BaseTotalAmtIncVat), 2, MidpointRounding.AwayFromZero);
                head.NetTotalAmt = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.TotalAmt), 2, MidpointRounding.AwayFromZero);
                head.NetTotalVatAmt = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.VatAmt), 2, MidpointRounding.AwayFromZero);
                head.NetTotalAmtIncVat = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.TotalAmtIncVat), 2, MidpointRounding.AwayFromZero);
                var count_pending_approve = line_v.Where(o => o.Status == "WAIT").Count();
                if (head.IsActive == false) {
                    head.Status = "CANCEL";
                } else {
                    if (count_pending_approve > 0) {
                        head.Status = "WAIT";
                    } else {
                        if (head.Status != "CLOSED") {
                            head.Status = "OPEN";
                        }
                    }
                }



                head.Qty = line_v.Sum(o => o.Qty);
                head.CountLine = line_v.Where(o => o.Status != "REJECT").Count();
            } catch (Exception ex) {
                input.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    input.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    input.OutputAction.Message1 = ex.Message;
                }
            }

            return input;
        }
        #endregion
        #region New transaction


        public static I_SODocSet NewTransaction(string rcom, string com, string doctype, string created_by, string created_fullname) {
            I_SODocSet n = new I_SODocSet();
            n.Head = NewHead(rcom, com, doctype, created_by, created_fullname);
            n.Line = new List<vw_OSOLine>();
            n.Lot = new List<vw_OSOLot>();
            n.LineActive = NewLine(rcom, com, n);
            n.LotActive = new vw_OSOLot();
            n.Log = new List<TransactionLog>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return n;
        }

        public static vw_OSOHead NewHead(string rcom, string com, string doctype, string created_by, string creaed_fullname) {
            vw_OSOHead n = new vw_OSOHead();
            n.OrdID = "";
            n.OrdDate = DateTime.Now.Date;
            n.RComID = rcom;
            n.ComID = com;
            n.INVID = "";
            n.INVDate = null;
            n.CustBrnID = "";
            n.CustBrnName = "";
            n.DocTypeID = doctype;
            n.CustID = "";
            n.CustName = "";
            n.CustAddr1 = "";
            n.CustAddr2 = "";
            n.CustMobile = "";
            n.CustEmail = "";
            n.BrandID = "";
            n.PaymentType = "";
            n.RefDocID = "";
            n.ShipID = "";
            n.ShipDate = DateTime.Now.Date;
            n.AccGroupID = "";
            n.CustTaxID = "";
            n.POID = "";
            n.PODate = null;
            n.BillToCustID = "";
            n.BillAddr1 = "";
            n.BillAddr2 = "";
            n.ShipFrLocID = "";
            n.ShipFrSubLocID = "";
            n.SalesID1 = created_by;
            n.SalesName = creaed_fullname;
            n.SalesID2 = "";
            n.Currency = "THB";
            n.RateExchange = 1;
            n.RateBy = "SP";
            n.RateDate = DateTime.Now.Date;
            n.TermID = "";
            n.PayDueDate = null;
            n.Qty = 0;
            n.QtyShip = 0;
            n.QtyInvoice = 0;
            n.QtyReturn = 0;
            n.CountLine = 0;
            n.QtyInvoicePending = 0;
            n.AmtInvoicePending = 0;
            n.AmtShipPending = 0;
            n.NetTotalAmt = 0;
            n.NetTotalVatAmt = 0;
            n.NetTotalAmtIncVat = 0;
            n.BaseNetTotalAmt = 0;
            n.BaseNetTotalAmtIncVat = 0;
            n.ItemDiscAmt = 0;
            n.ItemDiscAmtIncVat = 0;
            n.OntopDiscPer = 0;
            n.OntopDiscAmt = 0;
            n.OntopDiscAmtIncVat = 0;
            n.DiscCalBy = "A";
            n.VatRate = 0;
            n.VatTypeID = "7";
            n.Remark1 = "";
            n.Remark2 = "";

            n.PaymentMemo = "";

            n.IsPrint = false;
            n.PrintDate = null;
            n.IsLink = false;
            n.LinkDate = null;
            n.Status = "OK";
            n.Source = "BACK";
            n.CreatedBy = created_by;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static vw_OSOLine NewLine(string rcom, string com, I_SODocSet input) {
            vw_OSOLine n = new vw_OSOLine();
            n.OrdID = "";
            n.OrdDate = DateTime.Now.Date;
            n.RComID = rcom;
            n.ComID = com;
            n.DocTypeID = "";
            n.LineNum = GenLineNum(input);
            n.Sort = GenLineSort(input);
            n.PageBreak = false;
            n.ShipID = "";
            n.ShipLineNum = 0;
            n.INVDate = null;
            n.RefDocID = "";
            n.RefDocLineNum = 0;
            n.CustID = "";
            n.ItemID = "";
            n.ItemName = "";
            n.ItemTypeID = "";
            n.ItemTypeName = "";
            n.ItemCateID = "";
            n.ItemGroupID = "";
            n.ItemAccGroupID = "";
            n.IsStockItem = false;
            n.Weight = 0;
            n.WUnit = "";
            n.Qty = 1;
            n.QtyShip = 0;
            n.QtyInvoice = 0;
            n.QtyShipPending = 0;
            n.QtyInvoicePending = 0;
            n.AmtShipPending = 0;
            n.AmtInvoicePending = 0;
            n.Unit = "";
            n.Packaging = "";
            n.BaseTotalAmtIncVat = 0;
            n.Price = 0;
            n.PriceIncVat = 0;
            n.TotalAmt = 0;
            n.VatAmt = 0;
            n.TotalAmtIncVat = 0;
            n.VatRate = input.Head.VatRate;
            n.VatTypeID = input.Head.VatTypeID;
            n.BaseTotalAmt = 0;
            n.OntopDiscAmt = 0;
            n.OntopDiscPer = 0;
            n.OntopDiscAmtIncVat = 0;
            n.DiscPer = 0;
            n.DiscAmt = 0;
            n.DiscAmtIncVat = 0;
            n.DiscCalBy = "A";
            n.LocID = "";
            n.SubLocID = "";
            n.PackagingNo = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.BatchNo = "";
            n.PointID = "";
            n.PointName = "";
            n.Remark1 = "";
            n.Remark2 = "";
            n.DiscApproveBy = "";
            n.DiscApproveDate = null;
            n.Status = "NEW";
            n.Sort = 1;
            n.ImageUrl = "";
            n.PageBreak = false;
            n.IsComplete = false;
            n.IsActive = true;
            n.ImageUrl = "/img/watch2.png";
            return n;
        }

        public static vw_OSOLot NewLot(string rcom, string com, int line, I_SODocSet input) {
            vw_OSOLot n = new vw_OSOLot();
            n.RComID = "";
            n.ComID = "";
            n.OrdID = "";
            n.LineLineNum = line;
            n.LineNum = GenLotLineNum(input);
            n.DocTypeID = "";
            n.OrdDate = DateTime.Now.Date;
            n.CustID = "";
            n.ItemID = "";
            n.IsStockItem = true;
            n.Qty = 0;
            n.QtyBal = 0;
            n.Unit = "";
            n.LocID = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.Status = "NEW";
            n.IsActive = true;
            return n;
        }
        public static OSOHead Convert2SOHhead(vw_OSOHead i) {
            OSOHead n = new OSOHead();
            n.ID = i.ID;
            n.RComID = i.RComID;
            n.ComID = i.ComID;
            n.OrdID = i.OrdID;
            n.OrdDate = i.OrdDate;
            n.DocTypeID = i.DocTypeID;
            n.INVID = i.INVID;
            n.INVDate = i.INVDate;
            n.CustID = i.CustID;
            n.CustName = i.CustName;
            n.CustAddr1 = i.CustAddr1;
            n.CustAddr2 = i.CustAddr2;
            n.CustTaxID = i.CustTaxID;
            n.CustBrnID = i.CustBrnID;
            n.CustBrnName = i.CustBrnName;
            n.CustMobile = i.CustMobile;
            n.CustEmail = i.CustEmail;
            n.BrandID = i.BrandID;
            n.POID = i.POID;
            n.PODate = i.PODate;
            n.RefDocID = i.RefDocID;
            n.AccGroupID = i.AccGroupID;
            n.BillToCustID = i.BillToCustID;
            n.BillAddr1 = i.BillAddr1;
            n.BillAddr2 = i.BillAddr2;
            n.ShipFrLocID = i.ShipFrLocID;
            n.ShipFrSubLocID = i.ShipFrSubLocID;
            n.SalesID1 = i.SalesID1;
            n.SalesID2 = i.SalesID2;
            n.Currency = i.Currency;
            n.RateExchange = i.RateExchange;
            n.RateBy = i.RateBy;
            n.RateDate = i.RateDate;
            n.TermID = i.TermID;
            n.PayDueDate = i.PayDueDate;
            n.PaymentType = i.PaymentType;
            n.PaymentMemo = i.PaymentMemo;
            n.CountLine = i.CountLine;
            n.Qty = i.Qty;
            n.QtyShip = i.QtyShip;
            n.QtyInvoice = i.QtyInvoice;
            n.QtyReturn = i.QtyReturn;
            n.QtyShipPending = i.QtyShipPending;
            n.QtyInvoicePending = i.QtyInvoicePending;
            n.AmtShipPending = i.AmtShipPending;
            n.AmtInvoice = i.AmtInvoice;
            n.AmtInvoicePending = i.AmtInvoicePending;
            n.BaseNetTotalAmt = i.BaseNetTotalAmt;
            n.BaseNetTotalAmtIncVat = i.BaseNetTotalAmtIncVat;
            n.NetTotalAmt = i.NetTotalAmt;
            n.NetTotalVatAmt = i.NetTotalVatAmt;
            n.NetTotalAmtIncVat = i.NetTotalAmtIncVat;
            n.ItemDiscAmtIncVat = i.ItemDiscAmtIncVat;
            n.ItemDiscAmt = i.ItemDiscAmt;
            n.OntopDiscPer = i.OntopDiscPer;
            n.OntopDiscAmt = i.OntopDiscAmt;
            n.OntopDiscAmtIncVat = i.OntopDiscAmtIncVat;
            n.DiscCalBy = i.DiscCalBy;
            n.VatRate = i.VatRate;
            n.VatTypeID = i.VatTypeID;
            n.Remark1 = i.Remark1;
            n.Remark2 = i.Remark2;
            n.IsPrint = i.IsPrint;
            n.PrintDate = i.PrintDate;
            n.ShipID = i.ShipID;
            n.ShipDate = i.ShipDate;
            n.IsLink = i.IsLink;
            n.LinkDate = i.LinkDate;
            n.DiscStatus = i.DiscStatus;
            n.Lat = i.Lat;
            n.Lon = i.Lon;
            n.Status = i.Status;
            n.Source = i.Source;
            n.CreatedBy = i.CreatedBy;
            n.CreatedDate = i.CreatedDate;
            n.ModifiedBy = i.ModifiedBy;
            n.ModifiedDate = i.ModifiedDate;
            n.IsActive = i.IsActive;
            return n;
        }
        public static List<OSOLine> Convert2SOLine(List<vw_OSOLine> input) {
            List<OSOLine> output = new List<OSOLine>();
            foreach (var i in input) {
                OSOLine n = new OSOLine();
                n.ID = i.ID;
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.OrdID = i.OrdID;
                n.LineNum = i.LineNum;
                n.DocTypeID = i.DocTypeID;
                n.OrdDate = i.OrdDate;
                n.INVID = i.INVID;
                n.INVDate = i.INVDate;
                n.RefDocID = i.RefDocID;
                n.RefDocLineNum = i.RefDocLineNum;
                n.CustID = i.CustID;
                n.ItemID = i.ItemID;
                n.ItemName = i.ItemName;
                n.ItemTypeID = i.ItemTypeID;
                n.ItemTypeName = i.ItemTypeName;
                n.ItemCateID = i.ItemCateID;
                n.ItemGroupID = i.ItemGroupID;
                n.ItemAccGroupID = i.ItemAccGroupID;
                n.IsStockItem = i.IsStockItem;
                n.Weight = i.Weight;
                n.WUnit = i.WUnit;
                n.Qty = i.Qty;
                n.QtyShip = i.QtyShip;
                n.QtyInvoice = i.QtyInvoice;
                n.QtyShipPending = i.QtyShipPending;
                n.QtyInvoicePending = i.QtyInvoicePending;
                n.AmtShipPending = i.AmtShipPending;
                n.AmtInvoicePending = i.AmtInvoicePending;
                n.Unit = i.Unit;
                n.Packaging = i.Packaging;
                n.BaseTotalAmt = i.BaseTotalAmt;
                n.BaseTotalAmtIncVat = i.BaseTotalAmtIncVat;
                n.Price = i.Price;
                n.PriceIncVat = i.PriceIncVat;
                n.TotalAmt = i.TotalAmt;
                n.VatAmt = i.VatAmt;
                n.TotalAmtIncVat = i.TotalAmtIncVat;
                n.VatRate = i.VatRate;
                n.VatTypeID = i.VatTypeID;
                n.OntopDiscAmt = i.OntopDiscAmt;
                n.OntopDiscPer = i.OntopDiscPer;
                n.OntopDiscAmtIncVat = i.OntopDiscAmtIncVat;
                n.DiscPer = i.DiscPer;
                n.DiscAmt = i.DiscAmt;
                n.DiscAmtIncVat = i.DiscAmtIncVat;
                n.DiscCalBy = i.DiscCalBy;
                n.DiscPPU = i.DiscPPU;
                n.DiscPPUIncVat = i.DiscPPUIncVat;
                n.LocID = i.LocID;
                n.SubLocID = i.SubLocID;
                n.PackagingNo = i.PackagingNo;
                n.LotNo = i.LotNo;
                n.SerialNo = i.SerialNo;
                n.BatchNo = i.BatchNo;
                n.Remark1 = i.Remark1;
                n.Remark2 = i.Remark2;
                n.PointID = i.PointID;
                n.PointName = i.PointName;
                n.ShipID = i.ShipID;
                n.ShipLineNum = i.ShipLineNum;
                n.Status = i.Status;
                n.DiscApproveBy = i.DiscApproveBy;
                n.DiscApproveDate = i.DiscApproveDate;
                n.IsComplete = i.IsComplete;
                n.Sort = i.Sort;
                n.PageBreak = i.PageBreak;
                n.IsActive = i.IsActive;

                output.Add(n);
            }

            return output;
        }
        public static List<OSOLot> Convert2SOLot(List<vw_OSOLot> input) {
            List<OSOLot> output = new List<OSOLot>();
            foreach (var i in input) {
                OSOLot n = new OSOLot();
                n.ID = i.ID;
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.OrdID = i.OrdID;
                n.LineLineNum = i.LineLineNum;
                n.LineNum = i.LineNum;
                n.DocTypeID = i.DocTypeID;
                n.OrdDate = i.OrdDate;
                n.CustID = i.CustID;
                n.ItemID = i.ItemID;
                n.IsStockItem = i.IsStockItem;
                n.Qty = i.Qty;
                n.Unit = i.Unit;
                n.LocID = i.LocID;
                n.LotNo = i.LotNo;
                n.Status = i.Status;
                n.SerialNo = i.SerialNo;
                n.IsActive = i.IsActive;

                output.Add(n);
            }

            return output;
        }

        public static I_SODocSet AddLine(string rcom, string com, I_SODocSet input) {
            ClearPendingLine(input);
            input.Line.Add(NewLine(rcom, com, input));
            input.LineActive = input.Line.Where(o => o.Status == "NEW").OrderByDescending(o => o.LineNum).FirstOrDefault();
            return input;
        }

        public static I_SODocSet AddLot(string rcom, string com, I_SODocSet input) {
            input.Lot.Add(NewLot(rcom, com, input.LineActive.LineNum, input));
            return input;
        }

        public static void ClearPendingLine(I_SODocSet input) {
            try {
                var r1 = input.Line.RemoveAll(o => o.Status == "NEW");
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void ClearPendingLot(I_SODocSet input) {
            try {
                var r1 = input.Lot.RemoveAll(o => o.Status == "NEW");
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static int GenLineNum(I_SODocSet input) {
            int result = 10;
            try {
                var max_linenum = input.Line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenLotLineNum(I_SODocSet input) {
            int result = 10;
            try {
                var max_linenum = input.Lot.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenLineSort(I_SODocSet input) {
            int result = 1;
            try {
                var max_sort = input.Line.Max(o => o.Sort);
                result = Convert.ToInt32(max_sort) + 1;
            } catch { }
            return result;
        }

        #endregion
        #endregion


        #region Filter
        public static I_SODocFiterSet NewFilterSet() {
            I_SODocFiterSet n = new I_SODocFiterSet();
            n.DateFrom = DateTime.Now.Date.AddMonths(-6);
            n.DateTo = DateTime.Now.Date.AddMinutes(1);
            n.DocType = "SO1";
            n.Status = "";
            n.SearchText = "";
            n.ShowIsPrint = false;
            n.ShowActive = true;
            return n;
        }
        async public void SetSessionFiterSet(I_SODocFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("so_Fiter", json);
        }

        async public Task<I_SODocFiterSet> GetSessionFiterSet() {
            I_SODocFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("so_Fiter");
            if (strdoc != null) {
                result = JsonSerializer.Deserialize<I_SODocFiterSet>(strdoc, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } else {
                result = NewFilterSet();
            }
            return result;
        }
        #endregion


        public static I_SODocSet DeleteLine(int linenum, I_SODocSet input) {

            try {
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                input.Line.RemoveAll(o => o.LineNum == linenum);
                input.Lot.RemoveAll(o => o.LineLineNum == linenum);
                input = SOService.CalDocSet(input);
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input;
        }
         

        public static I_BasicResult SaveSO(I_SODocSet input, string action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
              // input = CalDocSet(input);
                var head = input.Head;

                using (GAEntities db = new GAEntities()) {

                    if (action == "insert") {
                        if (head.OrdID == "") {
                            List<string>? list_docid = IDRuunerService.GetNewIDV2(head.DocTypeID, head.RComID, input.Head.ComID, head.OrdDate, true, "th");
                            head.OrdID = list_docid[1];
                            result.Message2 = head.OrdID;
                                input = CalDocSet(input);
                        }
                        var sohead = SOService.Convert2SOHhead(input.Head);
                        var solines = SOService.Convert2SOLine(input.Line);
                        var solot = SOService.Convert2SOLot(input.Lot);
                        db.OSOHead.Add(sohead);
                        db.OSOLine.AddRange(solines);
                        db.OSOLot.AddRange(solot);
                        db.SaveChanges();
                        IDRuunerService.GetNewIDV2(head.DocTypeID, head.RComID, input.Head.ComID, head.OrdDate, true, "th");

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

                        uh.PaymentMemo = input.Head.PaymentMemo;
                        uh.ModifiedBy = input.Head.ModifiedBy;
                        uh.ModifiedDate = DateTime.Now;
                        var sohead = SOService.Convert2SOHhead(input.Head);
                        var solines = SOService.Convert2SOLine(input.Line);
                        var solot = SOService.Convert2SOLot(input.Lot);
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


        public static I_BasicResult SetPrint(string rcom,string com, string ordId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var so = db.OSOHead.Where(o => o.OrdID == ordId && o.ComID == com && o.RComID == rcom).FirstOrDefault();
                    so.IsPrint = true;
                    so.PrintDate = DateTime.Now;
                    db.SaveChanges(); 
                }
            } catch (Exception ex) {
                result.Result = "ok";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
    }
}
