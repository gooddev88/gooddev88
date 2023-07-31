
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.FileStore;
using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.POS.DA {
    public static class POS_POService {

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

        public static string PreviousPage { get { return (string)HttpContext.Current.Session["po_previouspage"]; } set { HttpContext.Current.Session["po_previouspage"] = value; } }

        public static string DetailPreviousPage { get { return (string)HttpContext.Current.Session["podetail_previouspage"]; } set { HttpContext.Current.Session["podetail_previouspage"] = value; } }
        public static string NewDocPreviousPage { get { return (string)HttpContext.Current.Session["ponewdoc_previouspage"]; } set { HttpContext.Current.Session["ponewdoc_previouspage"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }

        public static I_POSet DocSet { get { return (I_POSet)HttpContext.Current.Session["podocset"]; } set { HttpContext.Current.Session["podocset"] = value; } }
        public static List<vw_POS_POHead> DocList { get { return (List<vw_POS_POHead>)HttpContext.Current.Session["podoc_list"]; } set { HttpContext.Current.Session["podoc_list"] = value; } }
        public static I_POFiterSet FilterSet { get { return (I_POFiterSet)HttpContext.Current.Session["pofilter_set"]; } set { HttpContext.Current.Session["pofilter_set"] = value; } }

        #region Query Transaction
        public static void GetDocSetByID(string docid) {
            NewTransaction("");
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    DocSet.head = db.POS_POHead.Where(o => o.POID == docid).FirstOrDefault();
                    DocSet.line = db.vw_POS_POLine.Where(o => o.POID == docid && o.IsActive).ToList();
                    DocSet.FGLine = db.vw_POS_POFGLine.Where(o => o.POID == docid && o.IsActive).ToList();
                    DocSet.Log = TransactionInfoService.ListLogByDocID(docid);
                }
            } catch (Exception ex) {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }
       
        public static void ListDoc() {
            var f = FilterSet;
            var uic = LoginService.LoginInfo.UserInCompany;
            using (GAEntities db = new GAEntities()) {
                if (f.SearchText.Trim()=="") {
                    DocList = db.vw_POS_POHead.Where(o => 
                                          o.PODate >= f.DateFrom && o.PODate <= f.DateTo
                                        && uic.Contains(o.ComID)
                                        && (o.IsActive == f.ShowActive)
                                        && (o.Status == f.Status || f.Status == "")
                                        ).OrderByDescending(o => o.CreatedDate).ToList();
                } else {
                    DocList = db.vw_POS_POHead.Where(o =>
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
                                                            && (o.Status == f.Status || f.Status == "")
                                                            ).OrderByDescending(o => o.CreatedDate).ToList();
                }

                return;
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

        public static void Save(string action) {
            CalDocSet();
            var h = DocSet.head;
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {
                        checkDupID();
                        if (DocSet.OutputAction.Result == "fail") {
                            return;
                        }
                        var ll = LineConvert2Table(DocSet.line);
                        var ff = FGLineConvert2Table(DocSet.FGLine);
                        db.POS_POHead.Add(DocSet.head);
                        db.POS_POLine.AddRange(ll);
                        db.POS_POFGLine.AddRange(ff);
                        db.SaveChanges();

                        IDRuunerService.GetNewID("PO", h.ComID, true, "th", h.PODate);
                        SetCostItemByLastPriceInPO(DocSet.head.RComID, ll);
                        CalStock();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.ComID, Action = "INSERT NEW PO" });

                    } else {
                        var uh = db.POS_POHead.Where(o => o.POID == DocSet.head.POID).FirstOrDefault();
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
                        uh.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        uh.ModifiedDate = DateTime.Now;

                        var ll = LineConvert2Table(DocSet.line);
                        db.POS_POLine.RemoveRange(db.POS_POLine.Where(o => o.POID == h.POID && o.RComID == h.RComID));
                        db.POS_POLine.AddRange(ll);
                        var ff = FGLineConvert2Table(DocSet.FGLine);
                        db.POS_POFGLine.RemoveRange(db.POS_POFGLine.Where(o => o.POID == h.POID && o.RComID == h.RComID));
                        db.POS_POFGLine.AddRange(ff);

                        db.SaveChanges();
                        SetCostItemByLastPriceInPO(DocSet.head.RComID, ll);
                        CalStock();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.ComID, Action = "UPDATE PO" });
                    }

                }
            } catch (Exception ex) {
                DocSet.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }

        public static I_BasicResult SetStatusReceive(string poid,string rcom) {
            //OPEN / RECEIVED / CANCEL
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_POHead.Where(o => o.POID == poid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    var line = db.POS_POLine.Where(o => o.POID == poid && o.RComID == rcom && o.IsActive).ToList();
                    h.Status = "RECEIVED";
                    h.AcceptedBy = LoginService.LoginInfo.CurrentUser;
                    h.AcceptedDate = DateTime.Now;
                    foreach (var l in line) {
                        l.GrQty = l.Qty;
                        l.GrAmt = l.Amt;
                    }
                    db.SaveChanges();
                    CalStock();
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
        public static I_BasicResult SetStatusFinishFG(string poid,string rcom) { 
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_POHead.Where(o => o.POID == poid && o.RComID == rcom ).FirstOrDefault();
                    h.FinishFGDate = DateTime.Now;
                    db.SaveChanges();
                    CalStock();
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
        public static I_BasicResult SetStatusAccepted(string rcom, string ordid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var h = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "ACCEPTED";
                    h.ModifiedBy = LoginService.LoginInfo.CurrentUser;
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
                    var itemInfo = db.ItemInfo.Where(o => iIds.Contains(o.ItemID) && o.RCompanyID == rcom && o.IsActive).ToList();
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

        public static void CalDocSet() {
            var h = DocSet.head;

     
            //1. copy head to fg line
            foreach (var l in DocSet.FGLine) {
                
                if (h.Status=="OPEN") {
                    var rmline = DocSet.line.Where(o => o.FGRefLineNum == l.LineNum ).FirstOrDefault();
                    if (rmline!=null) {
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
            foreach (var l in DocSet.line) {
                var fgline = DocSet.FGLine.Where(o => o.LineNum == l.FGRefLineNum).FirstOrDefault();
                if (fgline!=null) {
                    l.FGItemID = fgline.FgItemID;
                    l.FGName = fgline.FgName;
                    l.FGOrdQty = fgline.OrdQty==null?0:Convert.ToDecimal(fgline.OrdQty);
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

            h.Qty = DocSet.line.Sum(o => o.Qty);
            h.Amt = DocSet.line.Sum(o => o.Amt);
            h.GrQty = DocSet.line.Sum(o => o.GrQty);
            h.GrAmt = DocSet.line.Sum(o => o.GrAmt);
            h.ShipQty = DocSet.line.Sum(o => o.ShipQty);
            h.ShipdAmt = DocSet.line.Sum(o => o.ShipdAmt);

        }
        public static void CalRevertBom(int poline_linenum ) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var h = DocSet.head;
                var a = DocSet.line.Where(o => o.LineNum == poline_linenum).FirstOrDefault();

                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;

                using (GAEntities db = new GAEntities()) { 
                    var bom = db.vw_POS_BOMLine.Where(o => o.IsActive && o.ItemIDRM == a.ItemID && o.IsDefault && o.RComID == rcom && o.UserForModule == "KITCHEN").FirstOrDefault();
                    var pol = POS_POService.NewFGLine();
                    if (bom==null) {
                      var bal = db.POS_STKBal.Where(o => o.RComID == a.RComID && o.ComID == h.ComID && o.LocID == a.ToLocID && o.ItemID == a.ItemID).FirstOrDefault();
                        a.FGItemID =  a.ItemID;
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
                        if (bom.RmQty==0) {
                            a.FGQty = 0;
                        } else {
                            a.FGQty = Math.Round(a.Qty * bom.FgQty / bom.RmQty, 2, MidpointRounding.AwayFromZero);
                        }
                        if (bal!=null) {
                            a.BalQty = bal.BalQty;
                        }
                        a.FGUnit = bom.FgUnit;
                    }
                     
                    pol.FgItemID = a.FGItemID;
                    pol.FgName = a.FGName;
                    pol.FGQtyRemainAfterOrder = 0;
                    pol.ToLocID = a.ToLocID;
                    pol.FgUnit = bom == null ? a.Unit : bom.FgUnit;
                    var item=    POSItemService.GetItem(pol.FgItemID); 
                    pol.Price = Convert.ToDecimal(item.Cost);
                    pol.VendorID = item.VendorID;
                    var r = POS_POService.NewPoFGLineByItem(pol, pol.FgItemID);
                    CalDocSet();

                }
            } catch (Exception ex) {
                DocSet.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    DocSet.OutputAction.Message1 = ex.Message;
                } 
            }
        }
        public static void CalStock() {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var h = DocSet.head;
                using (GAEntities db = new GAEntities()) {
                    db.SP_CalStkPOMove(h.POID, h.DocType, h.RComID, h.ComID);
                }
            } catch (Exception ex) {
                DocSet.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
        }

        public static I_BasicResult NewLineByItem(vw_POS_POLine poline, string item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pline = DocSet.line.Where(o => o.ItemID == item).FirstOrDefault();
                if (pline == null) {//add new ordline 
                    DocSet.line.Add(poline);
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

        public static I_BasicResult NewPoFGLineByItem(vw_POS_POFGLine pofgline, string item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pofgactive = DocSet.FGLine.Where(o => o.FgItemID == item).FirstOrDefault();
                if (pofgactive == null) {//add new pofgline 
                    DocSet.FGLine.Add(pofgline);
                } else {
                    //exist pofgline
                    pofgactive.FgQty = pofgactive.FgQty + pofgline.FgQty;
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

        public static I_BasicResult NewFGLineByItem(vw_POS_POFGLine pofgline, string item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var pline = DocSet.FGLine.Where(o => o.FgItemID == item).FirstOrDefault();
                if (pline == null) {//add new ordline 
                    DocSet.FGLine.Add(pofgline);
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

        public static bool checkDupID() {
            bool result = false;
            try {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Duplicate PO Code", Message2 = "" };
                string poid = DocSet.head.POID;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_POHead.Where(o => o.POID == poid).FirstOrDefault();

                    if (get_id == null) {
                        result = true;
                        DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                    }
                }
            } catch (Exception ex) {
            }
            return result;
        }

        #endregion

        #region Delete

        public static void DeleteDoc(string docid) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_POHead.Where(o => o.POID == docid && o.IsActive == true).FirstOrDefault();
                    var line = db.POS_POLine.Where(o => o.POID == docid && o.IsActive == true).ToList();
                    head.Status = "CANCEL";
                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    CalStock();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.POID, TableID = "PO", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.POID, Action = "DELETE DATA PO" });
                }
            } catch (Exception ex) {

                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void DeletePOLine(int linenum) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                 var getpoline = DocSet.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                //var getFgline = DocSet.FGLine.Where(o => o.LineNum == getpoline.FGRefLineNum).ToList(); 
                 
                DocSet.line.RemoveAll(o => o.LineNum == linenum);
                DocSet.FGLine.RemoveAll(o=>o.LineNum==getpoline.FGRefLineNum);
                CalDocSet();
            } catch (Exception ex) {

                DocSet.OutputAction.Result = "fail";
                DocSet.OutputAction.Message1 = ex.Message;
                if (ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " " + ex.InnerException.Message;
                }
            }
        }


        public static I_BasicResult DeletePOFGLine(int linenum)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var po = db.POS_POFGLine.Where(o => o.RComID == rcom && o.LineNum == linenum).FirstOrDefault();

                    db.POS_POFGLine.Remove(po);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                result.Result = "fail";

                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }

        #endregion

        #region New transaction

        public static void NewFilterSet() {
            FilterSet = new I_POFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date.AddDays(-2);
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.DocType = "";
            FilterSet.SearchBy = "DOCDATE";
            FilterSet.SearchText = "";
            FilterSet.Status = "OPEN";
            FilterSet.Vendor = "";
            FilterSet.ShowActive = true;
        }

        public static void NewTransaction(string doctype) {
            DocSet = new I_POSet();
            DocSet.Action = "";
            DocSet.head = NewHead();
            DocSet.line = new List<vw_POS_POLine>();
            DocSet.NeedRunNextID = false;
            DocSet.FGLine = new List<vw_POS_POFGLine>();
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            IsNewDoc = false;
        }

        public static POS_POHead NewHead() {
            POS_POHead n = new POS_POHead();

            n.ComID = "";
            n.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static vw_POS_POLine NewLine() {
            vw_POS_POLine n = new vw_POS_POLine();

            n.ComID = "";
            n.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.VendorID = "";
            n.POID = "";
            n.ToLocID = "";
            n.DocType = "PO";
            n.OrdID = "";
            n.LineNum = GenLineNum();
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

        public static vw_POS_POFGLine NewFGLine() {
            vw_POS_POFGLine n = new vw_POS_POFGLine();
            n.ID = 0;
            n.RComID = "";
            n.ComID = "";
            n.POID = "";
            n.LineNum = GenFGLineNum();
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

        public static int GenLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenFGLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.FGLine.Max(o => o.LineNum);
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

        #endregion

    }
}