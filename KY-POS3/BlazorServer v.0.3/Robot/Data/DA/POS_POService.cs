


using Blazored.SessionStorage;
using Dapper;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Robot.Data.DA;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using static Robot.POS.DA.POS_POService;
using static Robot.POS.DA.POSOrderService;

namespace Robot.POS.DA {
    public   class POS_POService {
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

        public class I_StatusBinding {
            public String Value { get; set; }
            public String Desc { get; set; }
        }
        public class I_POFiterSet {
            public String RCom { get; set; }
            public String Com { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public String Vendor { get; set; }
            public bool ShowActive { get; set; }
        }

        public string PreviousPageUrl = "";
        public string PreviousSubPageUrl = "";

        public POS_POService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;

        }

        ISessionStorageService sessionStorage;

        public I_POSet DocSet { get; set; }
        public I_POFiterSet FilterSet = new I_POFiterSet();


        public static List<I_StatusBinding> ListStatus() {
            List<I_StatusBinding> result = new List<I_StatusBinding>();
            result.Add(new I_StatusBinding { Value = "OPEN", Desc = "OPEN" });
            result.Add(new I_StatusBinding { Value = "RECEIVED", Desc = "RECEIVED" });
            return result;
        }


        public static List<LocationInfo> ListStockLocation(string type, string comId, string rcom) {
            List<LocationInfo> result = new List<LocationInfo>();

            using (GAEntities db = new GAEntities()) {

                result = db.LocationInfo.Where(o =>
                                              (o.LocTypeID == type || type == "")
                                               && (o.CompanyID == comId || comId == "")
                                               && o.RCompany == rcom
                                               && o.IsActive
                                          ).ToList();


                //if (isShowEmpty) {
                //    LocationInfo blank = new LocationInfo { LocID = "X", CompanyID = "", Name = "ไม่ระบุที่เก็บ", LocTypeID = "", LocCode = "ไม่ระบุที่เก็บ", ParentID = "" };
                //    result.Insert(0, blank);
                //}

            }
            return result;
        }
        //public static List<LocationInfo> ListStockLocation(string type, string comId, bool isShowEmpty)
        //{
        //    List<LocationInfo> result = new List<LocationInfo>();
        //    using (GAEntities db = new GAEntities())
        //    {
           
        //            result = db.LocationInfo.Where(o =>
        //                                          (o.LocTypeID == type || type == "")
        //                                           && (o.CompanyID == comId || comId == "")
        //                                           && o.IsActive
        //                                      ).ToList();
            
             
        //        if (isShowEmpty)
        //        {
        //            LocationInfo blank = new LocationInfo { LocID = "X", CompanyID = "", Name = "ไม่ระบุที่เก็บ", LocTypeID = "", LocCode = "ไม่ระบุที่เก็บ", ParentID = "" };
        //            result.Insert(0, blank);
        //        }

        //    }
        //    return result;
        //}
        #region Query Transaction
        public static I_POSet GetDocSetByID(string docid,string rcom) {
            I_POSet doc = new I_POSet();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.head = db.POS_POHead.Where(o => o.POID == docid && o.RComID==rcom).FirstOrDefault();
                    doc.line = db.vw_POS_POLine.Where(o => o.POID == docid && o.RComID == rcom && o.IsActive).ToList();
                    doc.FGLine = db.vw_POS_POFGLine.Where(o => o.POID == docid && o.RComID == rcom && o.IsActive).ToList();
                    doc.Log = db.TransactionLog.Where(o => (o.TransactionID == docid && o.RCompanyID == rcom)).ToList();
                }
            } catch (Exception ex) {
                doc.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return doc;
        }
        //public static void GetDocSetByID(string docid) {
        //    NewTransaction("ORDER");
        //    DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
        //    try
        //    {
        //        using (GAEntities db = new GAEntities())
        //        {
        //            DocSet.head = db.POS_ORDERHead.Where(o => o.OrdID == docid && o.RComID == rcom).FirstOrDefault();
        //            DocSet.line = db.vw_POS_ORDERLine.Where(o => o.OrdID == docid && o.RComID == rcom).OrderByDescending(o => o.VendorID).ThenBy(o => o.ItemID).ToList();
        //            DocSet.lineBom = db.POS_ORDERBomLine.Where(o => o.OrdID == docid && o.RComID == rcom).OrderByDescending(o => o.VendorID).ThenBy(o => o.FgItemID).ToList();
        //            DocSet.Log = TransactionInfoService.ListLogByDocID(docid);
        //        }
        //    } catch (Exception ex)
        //    {
        //        DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
        //    }
        //}
        //public static void ListDoc() {
        //    string doctype = FilterSet.DocType.ToLower();
        //    string filterby = FilterSet.SearchBy.ToLower();
        //    string search = FilterSet.SearchText;
        //    bool isactive = FilterSet.ShowActive;
        //    DateTime begin = FilterSet.DateFrom;
        //    DateTime end = FilterSet.DateTo;
        //    using (GAEntities db = new GAEntities()) {

        //        DocList = db.POS_POHead.Where(o =>
        //                                                (o.ComID.Contains(search)
        //                                                || o.POID.Contains(search)
        //                                                || o.RComID.Contains(search)
        //                                                || o.Description.Contains(search)
        //                                                || search == "")
        //                                                && (o.IsActive == isactive)
        //                                                ).OrderByDescending(o => o.CreatedDate).ToList();
        //        return;
        //    }
        //}

        public static List<vw_POS_POHead> ListDoc(I_POFiterSet f, LogInService login) {
            List<vw_POS_POHead> output = new List<vw_POS_POHead>();
                  var uic = login.LoginInfo.UserInCompany;
            using (GAEntities db = new GAEntities()) {
                if (f.SearchText.Trim() == "") {
                    output = db.vw_POS_POHead.Where(o =>
                                          o.PODate >= f.DateFrom && o.PODate <= f.DateTo
                                        && uic.Contains(o.ComID)
                                        && (o.IsActive == f.ShowActive)
                                                                                                    && (o.VendorID == f.Vendor || f.Vendor == "")
                                        && (o.Status == f.Status || f.Status == "")
                                        ).OrderByDescending(o => o.CreatedDate).ToList();
                } else {
                    output = db.vw_POS_POHead.Where(o =>
                                                            (o.ComID.Contains(f.SearchText)
                                                            || o.POID.Contains(f.SearchText)
                                                            || o.OrdID.Contains(f.SearchText)
                                                            || o.RComID.Contains(f.SearchText)
                                                            || o.VendorID.Contains(f.SearchText)
                                                            || o.VendorName.Contains(f.SearchText)
                                                            || o.Description.Contains(f.SearchText)
                                                            || f.SearchText == "")
                                                            && uic.Contains(o.ComID)
                                                            && (o.IsActive == f.ShowActive)
                                                                                                                        && (o.VendorID == f.Vendor || f.Vendor == "")
                                                            && (o.Status == f.Status || f.Status == "")
                                                            ).OrderByDescending(o => o.CreatedDate).ToList();
                }

                return output;
            }
        }

        #endregion

        #region  Method GET

        public static POS_POHead GetPO(string poid) {
            POS_POHead result = new POS_POHead();
            using (GAEntities db = new GAEntities()) {
                result = db.POS_POHead.Where(o => o.POID == poid).FirstOrDefault();
            }
            return result;
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

        public static I_BasicResult Save(string action, I_POSet doc,string user) {
            var r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            doc= CalDocSet(doc);
            var h = doc.head;
            var createby = user;
            try {
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {
                        doc= checkDupID(doc);
                        if (doc.OutputAction.Result == "fail") {
                            r = doc.OutputAction;
                            return r;
                        }
                        var ll = LineConvert2Table(doc.line);
                        var ff = FGLineConvert2Table(doc.FGLine);
                        db.POS_POHead.Add(doc.head);
                        db.POS_POLine.AddRange(ll);
                        db.POS_POFGLine.AddRange(ff);
                        db.SaveChanges();
                        IDRuunerService.GetNewIDV2("PO", h.RComID, h.ComID, h.PODate, true, "th");
                       
                        SetCostItemByLastPriceInPO(doc.head.RComID, ll);
                        var r_stk = CalStock(h.POID, h.DocType, h.RComID, h.ComID);
                        if (r_stk.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = r.Message1 + " " + r_stk.Message1;
                        }
                   
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "INSERT NEW PO" }, doc.head.RComID, doc.head.ComID, createby);


                    } else {
                        var uh = db.POS_POHead.Where(o => o.POID ==h.POID).FirstOrDefault();
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
                        uh.ModifiedBy = h.ModifiedBy;
                        uh.ModifiedDate = DateTime.Now;
                        

                        var ll = LineConvert2Table(doc.line);
                        db.POS_POLine.RemoveRange(db.POS_POLine.Where(o => o.POID == h.POID && o.RComID == h.RComID));
                        db.POS_POLine.AddRange(ll);
                        var ff = FGLineConvert2Table(doc.FGLine);
                        db.POS_POFGLine.RemoveRange(db.POS_POFGLine.Where(o => o.POID == h.POID && o.RComID == h.RComID));
                        db.POS_POFGLine.AddRange(ff);

                        db.SaveChanges();
                        SetCostItemByLastPriceInPO(doc.head.RComID, ll);
                        var r_stk = CalStock(h.OrdID, h.DocType, h.RComID, h.ComID);
                        if (r_stk.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = r.Message1 + " " + r_stk.Message1;
                        }

                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "UPDATE PO" }, doc.head.RComID, doc.head.ComID, createby);


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

        public static I_BasicResult SetStatusReceive(string poid, string rcom,string user) {
            //OPEN / RECEIVED / CANCEL
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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
                    var r_stk = CalStock(h.OrdID, h.DocType, h.RComID, h.ComID);
                    if (r_stk.Result == "fail") {
                        result.Result = "fail";
                        result.Message1 = result.Message1 + " " + r_stk.Message1;
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
        public static I_BasicResult SetStatusFinishFG(string poid, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_POHead.Where(o => o.POID == poid && o.RComID == rcom).FirstOrDefault();
                    h.FinishFGDate = DateTime.Now;
                    db.SaveChanges();
                    var r_stk = CalStock(h.OrdID, h.DocType, h.RComID, h.ComID);
                    if (r_stk.Result == "fail") {
                        result.Result = "fail";
                        result.Message1 = result.Message1 + " " + r_stk.Message1;
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
        public static I_BasicResult SetStatusAccepted(string rcom, string poid, string user) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var h = db.POS_POHead.Where(o => o.POID == poid && o.RComID == rcom && o.IsActive).FirstOrDefault();
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

        public static I_BasicResult SetCostItemByLastPriceInPO(string rcom, List<POS_POLine> poline) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    List<string> iIds = poline.Select(o => o.ItemID).Distinct().ToList();
                    var itemInfo = db.ItemInfo.Where(o => iIds.Contains(o.ItemID) && o.RCompanyID == rcom && o.IsActive==true).ToList();
                    foreach (var l in itemInfo) {
                        var exsit = poline.Where(o => o.ItemID == l.ItemID).FirstOrDefault();
                        if (exsit != null) {
                            l.Cost = exsit.Price;
                        }
                        db.SaveChanges();
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

        public static I_POSet CalDocSet(I_POSet doc) {
            var h = doc.head;


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
        public static I_POSet CalRevertBom(int poline_linenum, I_POSet doc) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var h = doc.head;
                var a = doc.line.Where(o => o.LineNum == poline_linenum).FirstOrDefault();

                var rcom = h.RComID;

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
                        a.Amt = a.Qty * a.Price;
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
                    pol.OrdQty = a.Qty;
                    pol.ToLocID = a.ToLocID;
                    
                    pol.FgUnit = bom == null ? a.Unit : bom.FgUnit;
                    var item = POSItemService.GetItem(pol.FgItemID,h.RComID);
                    pol.Price = Convert.ToDecimal(item.Cost);
                    pol.FgAmt = pol.FgQty* item.Cost;
                    pol.VendorID = item.VendorID;
                    var r = POS_POService.NewPoFGLineByItem(doc,pol, pol.FgItemID);
                    doc= CalDocSet(doc);

                }
            } catch (Exception ex) {
                doc.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            }
            return doc;
        }
    
        public static I_BasicResult CalStock(string docid , string doctype , string rcom , string com) {
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

        public static I_POSet NewLineByItem(I_POSet doc,vw_POS_POLine poline, string item) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pline = doc.line.Where(o => o.ItemID == item).FirstOrDefault();
                if (pline == null) {//add new ordline 
                    doc.line.Add(poline);
                } else {//exist ordline
                    pline.Qty = pline.Qty + poline.Qty;
                }
            } catch (Exception ex) {
                doc.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            }
            return doc;
        }

        public static I_POSet NewPoFGLineByItem(I_POSet doc, vw_POS_POFGLine pofgline, string item) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pofgactive = doc.FGLine.Where(o => o.FgItemID == item).FirstOrDefault();
                if (pofgactive == null) {//add new pofgline 
                    doc.FGLine.Add(pofgline);
                } else {
                    //exist pofgline
                    pofgactive.FgQty = pofgactive.FgQty + pofgline.FgQty;
                }
            } catch (Exception ex) {
                doc.OutputAction.Result = "fail";

                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            }
            return doc;
        }

        public static I_POSet NewFGLineByItem(I_POSet doc, vw_POS_POFGLine pofgline, string item) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pline = doc.FGLine.Where(o => o.FgItemID == item).FirstOrDefault();
                if (pline == null) {//add new ordline 
                    doc.FGLine.Add(pofgline);
                } else {
                    //exist ordline
                    pline.FgQty = pline.FgQty + pofgline.FgQty;
                }
            } catch (Exception ex) {
                doc.OutputAction.Result = "fail";

                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            }
            return doc;
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
            return doc;
        }

        #endregion

        #region Delete

        public static I_BasicResult DeleteDoc(string docid,string user) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var createby = user;
            try {

                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_POHead.Where(o => o.POID == docid && o.IsActive == true).FirstOrDefault();
                    var line = db.POS_POLine.Where(o => o.POID == docid && o.IsActive == true).ToList();
                    head.Status = "CANCEL";
                    head.ModifiedBy = user;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    var r_stk = CalStock(head.OrdID, head.DocType, head.RComID, head.ComID);
                    if (r_stk.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " " + r_stk.Message1;
                    }

                    TransactionService.SaveLog(new TransactionLog { TransactionID = head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = head.ComID, Action = "DELETE DATA PO" }, head.RComID, head.ComID, createby);

                }
            } catch (Exception ex) { 
                r = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return r;
        }

        public static I_POSet DeletePOLine(I_POSet doc ,int linenum) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var getpoline = doc.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                //var getFgline = DocSet.FGLine.Where(o => o.LineNum == getpoline.FGRefLineNum).ToList(); 

                doc.line.RemoveAll(o => o.LineNum == linenum);
                doc.FGLine.RemoveAll(o => o.LineNum == getpoline.FGRefLineNum);
                doc = CalDocSet(doc);
            } catch (Exception ex) {

                doc.OutputAction.Result = "fail";
                doc.OutputAction.Message1 = ex.Message;
                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = doc.OutputAction.Message1 + " " + ex.InnerException.Message;
                }
            }
            return doc;
        }


        public static I_BasicResult DeletePOFGLine(string doc_id,string rcom ,int linenum) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
           
            try {
                using (GAEntities db = new GAEntities()) {
                    var po = db.POS_POFGLine.Where(o =>o.POID==doc_id && o.RComID == rcom && o.LineNum == linenum).FirstOrDefault();

                    db.POS_POFGLine.Remove(po);
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

        #endregion

        #region New transaction

        public static I_POFiterSet NewFilterSet() {
            I_POFiterSet f = new I_POFiterSet();
           f = new I_POFiterSet();
            f.DateFrom = DateTime.Now.Date.AddDays(-2);
            f.DateTo = DateTime.Now.Date;
            f.DocType = "";
            f.SearchBy = "DOCDATE";
            f.SearchText = "";
            f.Status = "OPEN";
            f.Vendor = "";
            f.ShowActive = true;
            return f;
        }

        public static I_POSet NewTransaction(string doctype, string rcom, string user) {
          var  doc = new I_POSet();
            doc.Action = "";
            doc.head = NewHead(doctype,rcom,user);
            doc.line = new List<vw_POS_POLine>();
            doc.NeedRunNextID = false;
            doc.FGLine = new List<vw_POS_POFGLine>();
            doc.Log = new List<TransactionLog>();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return doc;
        }

        public static POS_POHead NewHead(string doctype, string rcom, string user) {
            POS_POHead n = new POS_POHead();

            n.ComID = "";
            n.RComID = rcom;
            n.POID = "";
            n.ToLocID = "";
            n.OrdID = "";
            n.VendorID = "";
            n.VendorName = "";
            n.DocType = "PO";
            n.PODate = DateTime.Now.Date;
            n.ToLocID = "";
            n.Description = "";
            n.Remark1 = "";
            n.AcceptedMemo = "";
            n.Remark1 = "";
            n.Qty = 0;
            n.ShipQty = 0;
            n.GrQty = 0;
            n.Amt = 0;
            n.ShipdAmt = 0;
            n.GrAmt = 0;
            n.AcceptedBy = "";
            n.ShipBy = "";
            n.Status = "OPEN";
            n.CreatedBy =user;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static vw_POS_POLine NewLine(I_POSet doc, string user) {
            vw_POS_POLine n = new vw_POS_POLine();

            n.ComID = "";
            n.RComID = doc.head.RComID;
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
            n.FGQty = 0;
            n.FGUnit = "";
            n.FGQtyRemainAfterOrder = 0;
            n.IsActive = true;
            return n;


        }

        public static vw_POS_POFGLine NewFGLine(I_POSet doc) {
            vw_POS_POFGLine n = new vw_POS_POFGLine();
            n.ID = 0;
            n.RComID = doc.head.RComID;
            n.ComID =doc.head.ComID;
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

        public static int GenLineNum(I_POSet doc) {
            int result = 10;
            try {
                var max_linenum = doc.line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenFGLineNum(I_POSet doc) {
            int result = 10;
            try {
                var max_linenum = doc.FGLine.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
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



        async public Task<I_POFiterSet> GetSessionPOSPOFiterSet() {
            I_POFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("POSPO_Fiter");
            if (strdoc == null) {
                return null;
            } else {
                return JsonConvert.DeserializeObject<I_POFiterSet>(strdoc);
            }

        }


        async public void SetSessionPOSPOFiterSet(I_POFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("POSPO_Fiter", json);
        }

        #endregion




    }
}