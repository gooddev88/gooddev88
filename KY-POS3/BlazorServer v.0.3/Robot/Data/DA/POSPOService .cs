
using Blazored.SessionStorage;
using BlazorPro.Spinkit;
using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Robot.Data.DA;
using Robot.Data.GADB.TT;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using static OfficeOpenXml.ExcelErrorValue;
using static Robot.Data.ML.I_Result;
using static Robot.POS.DA.POSOPOService;

namespace Robot.POS.DA {
    public class POSOPOService {
        public static string sessionActiveId = "activecomid";
        public static string sessionDocType = "doctype";
        public class I_POSet {
            public string Action { get; set; }
            public POS_POHead head { get; set; }
            public List<vw_POS_POLine> line { get; set; }
            public List<vw_POS_POFGLine> FGLine { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }

        }
        public class I_POFiterSet {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public String Vendor { get; set; }
            public bool ShowActive { get; set; }

        }
        public class I_ComboBinding {

            public String Desc { get; set; }
            public String Value { get; set; }
            public string Menu { get; set; }
            public List<string> ValueRelate { get; set; }

        }



        ISessionStorageService sessionStorage;

        public string PreviousPageUrl = "";
        public string PreviousSubPageUrl = "";

        public POSOPOService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;

        }

        public I_POSet DocSet { get; set; }


        #region Query Transaction
        public static I_POSet GetDocSetByID(string docid, string rcom) {

            I_POSet doc = new I_POSet();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    doc.head = db.POS_POHead.Where(o => o.POID == docid && o.RComID == rcom).FirstOrDefault();

                    doc.line = db.vw_POS_POLine.Where(o => (o.POID == docid)).ToList();
                    doc.FGLine = db.vw_POS_POFGLine.Where(o => (o.POID == docid)).ToList();
                    doc.Log = db.TransactionLog.Where(o => (o.TransactionID == docid && o.RCompanyID == rcom)).ToList();
                }
            } catch (Exception ex) {
                doc.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return doc;
        }

        public static List<vw_POS_POHead> ListDoc(LogInService login, I_POFiterSet f) {
            List<vw_POS_POHead> result = new List<vw_POS_POHead>();
            var uic = login.LoginInfo.UserInCompany;

            using (GAEntities db = new GAEntities()) {

                result = db.vw_POS_POHead.Where(o =>
                                                        (o.ComID.Contains(f.SearchText)
                                                        || o.POID.Contains(f.SearchText)
                                                        || o.RComID.Contains(f.SearchText)
                                                        || o.Description.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                        && o.PODate >= f.DateFrom && o.PODate <= f.DateTo
                                                        //&& uic.Contains(o.ComID)
                                                        && (o.IsActive == f.ShowActive)
                                                        && (o.Status == f.Status || f.Status == "")
                                                        ).OrderByDescending(o => o.CreatedDate).ToList();
                if (!f.ShowActive) {
                    result = result.Where(o => o.IsActive).ToList();
                }
            }
            return result;
        }



        public static List<vw_POS_ORDER_RM> ListDoc_POSORDER_RM(I_POFiterSet f) {
            List<vw_POS_ORDER_RM> result = new List<vw_POS_ORDER_RM>();
            using (GAEntities db = new GAEntities()) {

                result = db.vw_POS_ORDER_RM.Where(o =>
                                                        (o.VendorID.Contains(f.SearchText)
                                                        || o.RmItemID.Contains(f.SearchText)
                                                        || o.RmItemName.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                        ).OrderByDescending(o => o.RmItemID).ToList();
            }
            return result;
        }

        public static List<POS_POHead> ListDocOrderMobile(LogInService login, I_POFiterSet f) {
            List<POS_POHead> result = new List<POS_POHead>();
            var uic = login.LoginInfo.UserInCompany;

            using (GAEntities db = new GAEntities()) {
                result = db.POS_POHead.Where(o => (o.ComID.Contains(f.SearchText)
                                                        || o.POID.Contains(f.SearchText)
                                                        || o.RComID.Contains(f.SearchText)
                                                        || o.Description.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                        && uic.Contains(o.ComID)
                                                        && (o.Status == f.Status || f.Status == "")
                                                        && o.PODate >= f.DateFrom && o.PODate <= f.DateTo
                                                        && o.IsActive
                                        ).OrderByDescending(o => o.CreatedDate).ToList();
            }
            return result;
        }


        #endregion

        #region  Method GET

        public static POS_POHead GetOrderHeadByOrdID(string poid, string rcom) {
            POS_POHead result = new POS_POHead();
            using (GAEntities db = new GAEntities()) {
                result = db.POS_POHead.Where(o => o.POID == poid && o.RComID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_POS_POLine> ListViewPOLineByOrdID(string poid, string rcom) {
            List<vw_POS_POLine> result = new List<vw_POS_POLine>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_POLine.Where(o => o.POID == poid && o.IsActive && o.RComID == rcom).OrderBy(o => o.Name).ToList();
            }
            return result;
        }

        public static List<vw_POS_POLine> ListViewPOLineByComId(string ComId, string rcom) {
            List<vw_POS_POLine> result = new List<vw_POS_POLine>();

            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_POLine.Where(o => o.ComID == ComId && o.IsActive && o.RComID == rcom).OrderBy(o => o.Name).ToList();
            }
            return result;
        }

        public static List<I_ComboBinding> ListStatus(string menu) {
            List<I_ComboBinding> n = new List<I_ComboBinding>();
            n.Add(new I_ComboBinding { Value = "", Desc = "ALL", Menu = "2009", ValueRelate = new List<string> { "OPEN", "ACCEPTED", "SHIPPING", "RECEIVED" } });
            n.Add(new I_ComboBinding { Value = "OPEN", Desc = "OPEN", Menu = "2001", ValueRelate = new List<string> { "OPEN", "ACCEPTED", "SHIPPING", "RECEIVED" } }); //2001 เบิกสินค้า
            n.Add(new I_ComboBinding { Value = "ACCEPTED", Desc = "ACCEPTED", Menu = "2002", ValueRelate = new List<string> { "OPEN", "ACCEPTED" } });// 2002 จัดเตรียมสินค้า
            n.Add(new I_ComboBinding { Value = "SHIPPING", Desc = "SHIPPING", Menu = "2003", ValueRelate = new List<string> { "ACCEPTED", "SHIPPING" } });// 2003 ส่งสินค้า
            n.Add(new I_ComboBinding { Value = "RECEIVED", Desc = "RECEIVED", Menu = "2004", ValueRelate = new List<string> { "SHIPPING", "RECEIVED" } }); //2004 รับสินค้า
            var queryMenu = n.Where(o => o.Menu == menu).FirstOrDefault();
            n = n.Where(o => queryMenu.ValueRelate.Contains(o.Value)).ToList();
            return n;
        }
        public static decimal GetItemVatRate(string itemId) {
            decimal result = 0;
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.ItemInfo.Where(o => o.ItemID == itemId).FirstOrDefault();
                    var vat = db.TaxInfo.Where(o => o.TaxID == query.VatTypeID && o.Type == "SALE").FirstOrDefault();
                    if (vat != null) {
                        result = vat.TaxValue;
                    }

                }
            } catch (Exception) {

                throw;
            }

            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(string action, I_POSet doc, string user) {
            var r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.head;
            var rcom = doc.head.RComID;

            var createby = user;
            try {
                doc = CalDocSet(doc);
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {

                        doc = checkDupID(doc);
                        if (doc.OutputAction.Result == "fail") {
                            return doc.OutputAction;
                        }
                        var ll = LineConvert2Table(doc.line);
                        var ff = FGLineConvert2Table(doc.FGLine);
                        db.POS_POHead.Add(doc.head);
                        db.POS_POLine.AddRange(ll);
                        db.POS_POFGLine.AddRange(ff);
                        db.SaveChanges();
                        IDRuunerService.GetNewIDV2("ORD", h.RComID, h.ComID, h.PODate, true, "th");
                        var r_stk = CalStock(h.POID, h.DocType, h.RComID, h.ComID);
                        if (r_stk.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = r.Message1 + " " + r_stk.Message1;
                        }
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "INSERT NEW PO" }, rcom, doc.head.ComID, createby);

                    } else {

                        var uh = db.POS_POHead.Where(o => o.POID == doc.head.POID).FirstOrDefault();
                        uh.Description = h.Description;
                        uh.Remark1 = h.Remark1;
                        uh.Status = h.Status;
                        uh.OrdID = h.OrdID;
                        uh.ToLocID = h.ToLocID;
                        uh.Qty = h.Qty;
                        uh.Amt = h.Amt;
                        uh.GrQty = h.GrQty;
                        uh.GrAmt = h.GrAmt;
                        uh.ShipdAmt = h.ShipdAmt;
                        uh.ShipQty = h.ShipQty;
                        //uh.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        //uh.ModifiedDate = DateTime.Now;

                        var ll = LineConvert2Table(doc.line);
                        db.POS_POLine.RemoveRange(db.POS_POLine.Where(o => o.POID == h.POID && o.RComID == h.RComID));
                        db.POS_POLine.AddRange(ll);
                        var ff = FGLineConvert2Table(doc.FGLine);
                        db.POS_POFGLine.RemoveRange(db.POS_POFGLine.Where(o => o.POID == h.POID && o.RComID == h.RComID));
                        db.POS_POFGLine.AddRange(ff);

                        db.SaveChanges();
                        var r_stk = CalStock(h.POID, h.DocType, h.RComID, h.ComID);
                        if (r_stk.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = r.Message1 + " " + r_stk.Message1;
                        }
                        
                        //TransactionInfoService.SaveLog(new TransactionLog { TransactionID = doc.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "UPDATE PO" });
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "UPDATE PO" }, rcom, doc.head.ComID, createby);
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
        public static I_BasicResult SetStatusReceive(I_POSet doc, string poid, string rcom, string user) {
            //OPEN / ACCEPTED / SHIPPING / RECEIVED / CANCEL
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_POHead.Where(o => o.POID == poid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    var line = db.POS_POLine.Where(o => o.POID == poid && o.RComID == rcom && o.IsActive).ToList();
                    h.Status = "RECEIVED";
                    h.AcceptedBy = user;
                    h.AcceptedDate = DateTime.Now;
                    foreach (var l in line) {
                        l.GrQty = l.Qty;
                        l.GrAmt = l.Amt;
                    }
                    db.SaveChanges();
                    CalStock(h.POID, h.DocType, h.RComID, h.ComID);
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

        public static I_BasicResult SetStatusFinishFG(I_POSet doc, string poid, string rcom, string user) {
            //OPEN / ACCEPTED / SHIPPING / RECEIVED / CANCEL
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_POHead.Where(o => o.POID == poid && o.RComID == rcom).FirstOrDefault();
                    h.FinishFGDate = DateTime.Now;
                    db.SaveChanges();
                    CalStock(h.POID, h.DocType, h.RComID, h.ComID);
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
        public I_BasicResult SetStatusAccepted(string ordid, string rcom, string user) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (GAEntities db = new GAEntities()) {
                    var h = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "ACCEPTED";
                    h.ModifiedBy = user;
                    h.ModifiedDate = DateTime.Now;
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

        //public static I_BasicResult SetCostItemByLastPriceInPO(I_POSet doc,string rcom, List<POS_POLine> poline) {
        //    I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        using (GAEntities db = new GAEntities()) {
        //            List<string> iIds = poline.Select(o => o.ItemID).Distinct().ToList();
        //            var itemInfo = db.ItemInfo.Where(o => iIds.Contains(o.ItemID) && o.RCompanyID == rcom && o.IsActive).ToList();
        //            foreach (var l in itemInfo) {
        //                var exsit = poline.Where(o => o.ItemID == l.ItemID).FirstOrDefault();
        //                if (exsit != null) {
        //                    l.Cost = exsit.Price;
        //                }
        //                db.SaveChanges();
        //            }
        //        }
        //    } catch (Exception ex) {
        //        result.Result = "fail";
        //        if (ex.InnerException != null) {
        //            result.Message1 = ex.InnerException.ToString();
        //        } else {
        //            result.Message1 = ex.Message;
        //        }
        //    }
        //    return result;
        //}


        public static I_POSet CalDocSet(I_POSet doc) {
            var h = doc.head;
            var line = doc.line;




            //1. copy head to fg line
            foreach (var l in doc.FGLine) {

                if (h.Status == "OPEN") {
                    var rmline = doc.line.Where(o => o.FGRefLineNum == l.LineNum).FirstOrDefault();
                    if (rmline != null) {
                        l.FgQty = rmline.Qty;
                    }
                }
                l.POID = h.POID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.OrdID = h.OrdID;
                l.DocType = h.DocType;
                //l.ToLocID = h.ToLocID;
                l.PODate = h.PODate;
                l.FgAmt = l.Price * l.FgQty;
            }
            //1. copy head to line
            foreach (var l in doc.line) {
                var fgline = doc.FGLine.Where(o => o.LineNum == l.FGRefLineNum).FirstOrDefault();
                if (fgline != null) {
                    l.FGItemID = fgline.FgItemID;
                    l.FGName = fgline.FgName;
                    l.FGOrdQty = fgline.OrdQty == null ? 0 : Convert.ToDecimal(fgline.OrdQty);
                    l.FGQty = fgline.FgQty;
                    l.FGUnit = fgline.FgUnit;
                }
                l.POID = h.POID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.OrdID = h.OrdID;
                //l.ToLocID = h.ToLocID;
                l.PODate = h.PODate;
                l.Amt = l.Price * l.Qty;
                l.ShipdAmt = l.Price * l.ShipQty;
                l.GrAmt = l.Price * l.GrQty;
            }

            h.Qty = doc.line.Sum(o => o.Qty);
            h.Amt = doc.line.Sum(o => o.Amt);
            h.GrQty = doc.line.Sum(o => o.GrQty);
            h.GrAmt = doc.line.Sum(o => o.GrAmt);
            h.ShipQty = doc.line.Sum(o => o.ShipQty);
            h.ShipdAmt = doc.line.Sum(o => o.ShipdAmt);
            return doc;
        }

        public static void CalRevertBom(I_POSet doc,int poline_linenum, string rcom) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var h = doc.head;
                var a = doc.line.Where(o => o.LineNum == poline_linenum).FirstOrDefault();


                using (GAEntities db = new GAEntities()) {
                    var bom = db.vw_POS_BOMLine.Where(o => o.IsActive && o.ItemIDRM == a.ItemID && o.IsDefault && o.RComID == rcom && o.UserForModule == "KITCHEN").FirstOrDefault();
                    var pol = POS_POService.NewFGLine(doc);
                    if (bom == null) {
                        var bal = db.POS_STKBal.Where(o => o.RComID == a.RComID && o.ComID == h.ComID && o.LocID == a.ToLocID && o.ItemID == a.ItemID).FirstOrDefault();
                        a.FGItemID = a.ItemID;
                        a.FGRefLineNum = pol.LineNum;
                        a.FGName = a.Name;
                        a.FGQty = a.Qty;
                        if (bal != null) {
                            a.BalQty = bal.BalQty;
                        }
                        a.FGUnit = a.Unit;

                    } else {
                        var bal = db.POS_STKBal.Where(o => o.RComID == a.RComID && o.ComID == h.ComID && o.LocID == a.ToLocID && o.ItemID == bom.ItemIDFG).FirstOrDefault();
                        a.FGItemID = bom.ItemIDFG;
                        a.FGRefLineNum = pol.LineNum;
                        a.FGName = bom.ItemIDFGName;
                        if (bom.RmQty == 0) {
                            a.FGQty = 0;
                        } else {
                            a.FGQty = Math.Round(a.Qty * bom.FgQty / bom.RmQty, 2, MidpointRounding.AwayFromZero);
                        }
                        if (bal != null) {
                            a.BalQty = bal.BalQty;
                        }
                        a.FGUnit = bom.FgUnit;
                    }

                    pol.FgItemID = a.FGItemID;
                    pol.FgName = a.FGName;
                    pol.FGQtyRemainAfterOrder = 0;
                    pol.ToLocID = a.ToLocID;
                    pol.FgUnit = bom == null ? a.Unit : bom.FgUnit;
                    var item = POSItemService.GetItem(pol.FgItemID,rcom);
                    pol.Price = Convert.ToDecimal(item.Cost);
                    pol.VendorID = item.VendorID;
                    var r = POS_POService.NewPoFGLineByItem(doc,pol, pol.FgItemID);
                    CalDocSet(doc);

                }
            } catch (Exception ex) {
                doc.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            }
        }
        public static I_BasicResult CalStock(string docid, string doctype, string rcom, string com) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                    var strSQL = $"exec [SP_CalStkPOMove] {docid}, {doctype}, {rcom}, {com}";
                    var rquery = connection.Execute(strSQL);
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
        public static I_POSet NewLineByItem(I_POSet doc, vw_POS_POLine poline, string item) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pline = doc.line.Where(o => o.ItemID == item).FirstOrDefault();
                if (pline == null) {//add new ordline 
                    doc.line.Add(poline);
                } else {//exist ordline
                    pline.Qty = pline.Qty + poline.Qty;
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

        public static I_POSet NewPoFGLineByItem(I_POSet doc, vw_POS_POLine pofgline, string item) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pofgactive = doc.FGLine.Where(o => o.FgItemID == item).FirstOrDefault();
                if (pofgactive == null) {//add new pofgline 
                    doc.FGLine.Add(pofgline);
                } else {
                    //exist pofgline
                    pofgactive.FgQty = pofgactive.FgQty + pofgline.FgQty;
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

        public static I_POSet NewFGLineByItem(I_POSet doc, vw_POS_POLine pofgline, string item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pline = doc.FGLine.Where(o => o.FgItemID == item).FirstOrDefault();
                if (pline == null) {//add new ordline 
                    doc.FGLine.Add(pofgline);
                } else {
                    //exist ordline
                    pline.FgQty = pline.FgQty + pofgline.FgQty;
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

        public static I_POSet checkDupID(I_POSet doc) {
            bool result = false;
            try {
                doc.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Duplicate PO Code", Message2 = "" };
                string poid = doc.head.POID;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_POHead.Where(o => o.POID == poid).FirstOrDefault();

                    if (get_id == null) {
                        result = true;
                        doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    }
                }
            } catch (Exception ex) {
            }
            return result;
        }



        #endregion

        #region Delete

        public static I_BasicResult DeleteDoc(string docid, string rcom, string user) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var createby = user;
            try {

                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_POHead.Where(o => o.POID == docid && o.RComID == rcom).FirstOrDefault();
                    var line = db.POS_POLine.Where(o => o.POID == docid && o.RComID == rcom).ToList();
                    head.Status = "CANCEL";
                    head.ModifiedBy = rcom;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    var r_stk = CalStock(head.POID, head.DocType, head.RComID, head.ComID);
                    if (r_stk.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " " + r_stk.Message1;
                    }
                    TransactionService.SaveLog(new TransactionLog { TransactionID = head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = head.ComID, Action = "DELETE DATA PO" }, rcom, head.ComID, createby);


                }
            } catch (Exception ex) {

                r = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return r;
        }

        public static void DeleteBOMLine(I_POSet doc, int linenum) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var getpoline = doc.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                //var getFgline = DocSet.FGLine.Where(o => o.LineNum == getpoline.FGRefLineNum).ToList(); 

                doc.line.RemoveAll(o => o.LineNum == linenum);
                doc.FGLine.RemoveAll(o => o.LineNum == getpoline.FGRefLineNum);
                CalDocSet(doc);
            } catch (Exception ex) {

                doc.OutputAction.Result = "fail";
                doc.OutputAction.Message1 = ex.Message;
                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = doc.OutputAction.Message1 + " " + ex.InnerException.Message;
                }
            }
        }

        #endregion

        #region New transaction

        public static I_POFiterSet NewFilterSet() {
            I_POFiterSet n = new I_POFiterSet();

            n = new I_POFiterSet();
            n.DateFrom = DateTime.Now.Date.AddDays(-2);
            n.DateTo = DateTime.Now.Date;
            n.DocType = "";
            n.SearchBy = "DOCDATE";
            n.SearchText = "";
            n.Status = "OPEN";
            n.Vendor = "";
            n.ShowActive = true;

            return n;
        }

        public static I_POSet NewTransaction(string doctype, string rcom) {
            I_POSet n = new I_POSet();

            n.Action = "";
            n.head = NewHead(doctype, rcom);
            n.line = new List<vw_POS_POLine>();
            n.NeedRunNextID = false;
            n.FGLine = new List<vw_POS_POFGLine>();
            n.Log = new List<TransactionLog>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            //IsNewDoc = false;
            return n;
        }

        public static POS_POHead NewHead(string doctype, string rcom) {
            POS_POHead n = new POS_POHead();


            n.RComID = "";
            n.ComID = "";
            n.POID = "";
            n.PODate = DateTime.Now;
            n.DocType = "";
            n.VendorID = "";
            n.VendorName = "";
            n.Description = "";
            n.OrdID = "";
            n.ToLocID = "";
            n.Remark1 = "";
            n.AcceptedMemo = "";
            n.Qty = 0;
            n.ShipQty = 0;
            n.GrQty = 0;
            n.Amt = 0;
            n.ShipdAmt = 0;
            n.GrAmt = 0;
            n.AcceptedBy = "";
            n.AcceptedDate = DateTime.Now;
            n.ShipBy = "";
            n.ShpiDate = DateTime.Now;
            n.FinishFGDate = DateTime.Now;
            n.Status = "";
            n.CreatedDate = DateTime.Now;
            n.CreatedBy = "";
            n.ModifiedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.IsActive = true;

            return n;
        }

        public static vw_POS_POLine NewLine(I_POSet doc, string rcom) {
            vw_POS_POLine n = new vw_POS_POLine();

            n.ComID = "";
            n.RComID = rcom;
            n.VendorID = "";
            n.POID = "";
            n.ToLocID = "";
            n.DocType = "PO";
            n.OrdID = "";
            n.LineNum = GenLineNum(doc);
            n.PODate = DateTime.Now.Date;
            n.ItemID = "";
            n.Name = "";
            n.Price = 0;
            n.Qty = 0;
            n.ShipQty = 0;
            n.GrQty = 0;
            n.Amt = 0;
            n.ShipdAmt = 0;
            n.GrAmt = 0;
            n.BalQty = 0;
            n.OnOrdQty = 0;
            n.Unit = "";
            n.FGName = "";
            n.FGOrdQty = 0;
            n.FGBalQty = 0;
            n.FGRefLineNum = 0;
            n.FGItemID = "";
            n.Name = "";
            n.FGOrdQty = 0;
            n.FGQty = 0;
            n.FGUnit = "";
            n.FGQtyRemainAfterOrder = 0;
            n.IsActive = true;
            return n;
        }

        public static vw_POS_POFGLine NewFGLine(I_POSet doc, string rcom) {
            vw_POS_POFGLine n = new vw_POS_POFGLine();
            n.ID = 0;
            n.RComID = "";
            n.ComID = "";
            n.POID = "";
            n.LineNum = GenFGLineNum(doc);
            n.OrdID = "";
            n.VenderID = "";
            n.ToLocID = "";
            n.VendorID = "";
            n.PODate = DateTime.Now.Date;
            n.DocType = "";
            n.FgItemID = "";
            n.FgName = "";
            n.Price = 0;
            n.OrdQty = 0;
            n.FgQty = 0;
            n.FgAmt = 0;
            n.FgUnit = "";
            n.BalQty = 0;
            n.FGQtyRemainAfterOrder = 0;
            n.IsActive = true;
            return n;
        }
        public static List<POS_POLine> LineConvert2Table(List<vw_POS_POLine> input) {
            List<POS_POLine> t = new List<POS_POLine>();
            foreach (var i in input) {
                POS_POLine n = new POS_POLine();
                n.ID = i.ID;
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.POID = i.POID;
                n.LineNum = i.LineNum;
                n.VendorID = i.VendorID;
                n.ToLocID = i.ToLocID;
                n.PODate = i.PODate;
                n.OrdID = i.OrdID;
                n.DocType = i.DocType;
                n.ItemID = i.ItemID;
                n.Name = i.Name;
                n.Price = i.Price;
                n.Qty = i.Qty;
                n.ShipQty = i.ShipQty;
                n.GrQty = i.GrQty;
                n.Amt = i.Amt;
                n.ShipdAmt = i.ShipdAmt;
                n.GrAmt = i.GrAmt;
                n.Unit = i.Unit;
                n.FGRefLineNum = i.FGRefLineNum;
                n.FGItemID = i.FGItemID;
                n.FGName = i.FGName;
                n.FGOrdQty = i.FGOrdQty;
                n.FGQty = i.FGQty;
                n.FGUnit = i.FGUnit;
                n.IsActive = i.IsActive;

                t.Add(n);
            }
            return t;
        }
        public static List<POS_POFGLine> FGLineConvert2Table(List<vw_POS_POFGLine> input) {
            List<POS_POFGLine> t = new List<POS_POFGLine>();
            foreach (var i in input) {
                POS_POFGLine n = new POS_POFGLine();
                n.ID = i.ID;
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.POID = i.POID;
                n.LineNum = i.LineNum;
                n.VenderID = i.VenderID;
                n.ToLocID = i.ToLocID;
                n.VendorID = i.VendorID;
                n.PODate = i.PODate;
                n.DocType = i.DocType;
                n.OrdID = i.OrdID;
                n.FgItemID = i.FgItemID;
                n.FgName = i.FgName;
                n.Price = i.Price;
                n.OrdID = i.OrdID;
                n.OrdQty = i.OrdQty;
                n.FgQty = i.FgQty;
                n.FgAmt = i.FgAmt;
                n.FgUnit = i.FgUnit;
                n.IsActive = i.IsActive;

                t.Add(n);
            }
            return t;
        }
        public static int GenFGLineNum(I_POSet doc) {
        int result = 10;
        try {
            var max_linenum = doc.FGLine.Max(o => o.LineNum);
            result = max_linenum + 10;
        } catch { }
        return result;
    }
    public static int GenLineNum(I_POSet doc) {
        int result = 10;
        try {
            var max_linenum = doc.line.Max(o => o.LineNum);
            result = max_linenum + 10;
        } catch { }
        return result;
    }
    //public static void CalStock(I_POSet doc) {
    //    doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
    //    try {
    //        var h = doc.head;
    //        using (GAEntities db = new GAEntities()) {
    //            db.SP_CalStkGrMove(h.POID, h.DocType, h.RComID, h.ComID);
    //        }
    //    } catch (Exception ex) {
    //        doc.OutputAction.Result = "fail";
    //        if (ex.InnerException != null) {
    //            doc.OutputAction.Message1 = ex.InnerException.ToString();
    //        } else {
    //            doc.OutputAction.Message1 = ex.Message;
    //        }


    //    }
    //}

    //public static  I_BasicResult CalBom(string  doc_id ,string rcom,string user) {
    //    I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
    //    try {

    //     var doc=   GetDocSetByID(doc_id, rcom);
    //        var h = doc.head;

    //        using (GAEntities db = new GAEntities()) {
    //            doc.lineBom.Clear();
    //            var fgItem = doc.line.Select(o => o.ItemID).Distinct().ToList();
    //            var bom = db.POS_BOMLine.Where(o => o.IsActive && fgItem.Contains(o.ItemIDFG) && o.IsDefault && o.RComID == rcom && o.UserForModule == "KITCHEN").ToList();
    //            var itemsId = bom.Select(o => o.ItemIDRM).Distinct().ToList();
    //            var rmItem = db.ItemInfo.Where(o => o.RCompanyID == rcom && itemsId.Contains(o.ItemID)).ToList();

    //            foreach (var l in doc.line) {
    //                var bb = bom.Where(o => o.ItemIDFG == l.ItemID).ToList();
    //                if (bb.Count > 0) {
    //                    foreach (var b in bb) {
    //                        var rm = rmItem.Where(o => o.ItemID == b.ItemIDRM).FirstOrDefault();
    //                        var r1 = NewBomLine(l,doc);
    //                        r1.RmItemID = b.ItemIDRM;
    //                        r1.RmItemName = rm.Name1;
    //                        r1.BomID = b.BomID;
    //                        r1.RmUnit = rm.UnitID;
    //                        switch (h.Status) {
    //                            case "ACCEPTED":
    //                                r1.RmQty = (l.OrdQty * b.RmQty) / b.FgQty;
    //                                //r1.FgQty = b.FgQty;
    //                                break;
    //                            case "SHIPPING":
    //                                //   r1.RmQty = (l.ShipQty * b.RmQty) / b.FgQty;
    //                                r1.FgQty = l.ShipQty;
    //                                break;
    //                            case "RECEIVED":
    //                                //   r1.RmQty = (l.GrQty * b.RmQty) / b.FgQty;
    //                                r1.FgQty = l.GrQty;
    //                                break;
    //                            default:
    //                                break;
    //                        }

    //                        r1.Price = 0;
    //                        r1.RmAmt = 0;

    //                        if (rm != null) {

    //                            r1.Price = rm.Price;
    //                            r1.RmAmt = r1.RmQty * r1.Price;
    //                        }
    //                        doc.lineBom.Add(r1);
    //                    }

    //                    CalDocSet(doc);

    //                } else {
    //                    var r2 = NewBomLine(l,doc);
    //                    doc.lineBom.Add(r2);
    //                }
    //            }
    //            Save("update",doc,user);
    //        }
    //    } catch (Exception ex) {
    //        r.Result = "fail";
    //        if (ex.InnerException != null) {
    //            r.Message1 = ex.InnerException.ToString();
    //        } else {
    //            r.Message1 = ex.Message;
    //        }


    //    }
    //    return r;
    //}
    #endregion

    #region function move data management
    public static POS_BOMHead GetPreviousData(string currID) {
        POS_BOMHead result = new POS_BOMHead();
        using (GAEntities db = new GAEntities()) {
            var curr = db.POS_BOMHead.Where(o => o.BomID == currID).FirstOrDefault();
            result = db.POS_BOMHead.Where(o => o.ID < curr.ID && o.IsActive).FirstOrDefault();
        }
        return result;
    }
    public static POS_BOMHead GetLastData() {
        POS_BOMHead result = new POS_BOMHead();
        using (GAEntities db = new GAEntities()) {
            result = db.POS_BOMHead.OrderByDescending(o => o.ID).FirstOrDefault();
        }
        return result;
    }

    public static POS_BOMHead GetNextData(string currID) {
        POS_BOMHead result = new POS_BOMHead();
        using (GAEntities db = new GAEntities()) {
            var curr = db.POS_BOMHead.Where(o => o.BomID == currID).FirstOrDefault();
            result = db.POS_BOMHead.Where(o => o.ID > curr.ID && o.IsActive).FirstOrDefault();
        }
        return result;
    }

    #endregion







    async public Task<I_POFiterSet> GetSessionPOSOrderFiterSet() {
        I_POFiterSet result = NewFilterSet();
        var strdoc = await sessionStorage.GetItemAsync<string>("POSPO_Fiter");
        if (strdoc == null) {
            return null;
        } else {
            return JsonConvert.DeserializeObject<I_POFiterSet>(strdoc);
        }

    }


    async public void SetSessionPOSOrderFiterSet(I_POFiterSet data) {
        JsonSerializerOptions jso = new JsonSerializerOptions();
        jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
        await sessionStorage.SetItemAsync("POSPO_Fiter", json);
    }

}
}