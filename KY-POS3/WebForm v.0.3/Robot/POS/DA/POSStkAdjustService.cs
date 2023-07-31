
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.POS.DA {
    public static class POSStkAdjustService {

        public class I_StkAdjustSet {
            public string Action { get; set; }
            public POS_StkAdjustHead head { get; set; }
            public List<POS_StkAdjustLine> line { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_StkAdjusFiterSet {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowActive { get; set; }
        }
        public static string PreviousPage { get { return (string)HttpContext.Current.Session["stkadjus_previouspage"]; } set { HttpContext.Current.Session["stkadjus_previouspage"] = value; } }

        public static string DetailPreviousPage { get { return (string)HttpContext.Current.Session["stkadjusdetail_previouspage"]; } set { HttpContext.Current.Session["stkadjusdetail_previouspage"] = value; } }
        public static string NewDocPreviousPage { get { return (string)HttpContext.Current.Session["stkadjusnewdoc_previouspage"]; } set { HttpContext.Current.Session["stkadjusnewdoc_previouspage"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }

        public static I_StkAdjustSet DocSet { get { return (I_StkAdjustSet)HttpContext.Current.Session["stkadjusdocset"]; } set { HttpContext.Current.Session["stkadjusdocset"] = value; } }
        public static List<POS_StkAdjustHead> DocList { get { return (List<POS_StkAdjustHead>)HttpContext.Current.Session["stkadjusdoc_list"]; } set { HttpContext.Current.Session["stkadjusdoc_list"] = value; } }
        public static I_StkAdjusFiterSet FilterSet { get { return (I_StkAdjusFiterSet)HttpContext.Current.Session["stkadjusfilter_set"]; } set { HttpContext.Current.Session["stkadjusfilter_set"] = value; } }

        #region Query Transaction
        public static void GetDocSetByID(int id ) {
            NewTransaction();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany;
            try {
                using (GAEntities db = new GAEntities()) {
                    DocSet.head = db.POS_StkAdjustHead.Where(o => o.ID == id ).FirstOrDefault();
                    DocSet.line = db.POS_StkAdjustLine.Where(o => o.AdjID == DocSet.head.AdjID && o.ComID == DocSet.head.ComID && o.RComID == DocSet.head.RComID).ToList();
                    DocSet.Log = TransactionInfoService.ListLogByDocID(DocSet.head.AdjID, DocSet.head.ComID, "STK_ADJUST");
                }
            } catch (Exception ex) {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void ListDoc() {
            var f = FilterSet;
            var uic = LoginService.LoginInfo.UserInCompany;
            using (GAEntities db = new GAEntities()) {

                DocList = db.POS_StkAdjustHead.Where(o =>
                                                        (o.ComID.Contains(f.SearchText)
                                                        || o.AdjID.Contains(f.SearchText)
                                                        || o.RComID.Contains(f.SearchText)
                                                        || o.ComID.Contains(f.SearchText)
                                                        || o.Description.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                        && o.AdjDate >= f.DateFrom && o.AdjDate <= f.DateTo
                                                        && (o.IsActive == f.ShowActive)
                                                        && uic.Contains(o.ComID)
                                                        && (o.Status == f.Status || f.Status == "")
                                                        ).OrderByDescending(o => o.CreatedDate).ToList();
                return;
            }
        }


        #endregion

        #region  Method GET

        public static POS_StkAdjustHead GetStkAdjust(string adjid) {
            POS_StkAdjustHead result = new POS_StkAdjustHead();
            using (GAEntities db = new GAEntities()) {
                result = db.POS_StkAdjustHead.Where(o => o.AdjID == adjid).FirstOrDefault();
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
            var h = DocSet.head;
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                CalDocSet();
                using (GAEntities db = new GAEntities()) {
                 
                    if (action == "insert") { 
                        checkDupID();
                        if (DocSet.OutputAction.Result == "fail") {
                            return;
                        }
                        db.POS_StkAdjustHead.Add(DocSet.head);
                        db.POS_StkAdjustLine.AddRange(DocSet.line);
                        db.SaveChanges();
                        IDRuunerService.GetNewID("ADJUST", h.ComID, true, "th", h.AdjDate);
                        CalStock();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.AdjID, TableID = "STK_ADJUST", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.ComID, Action = "INSERT NEW ADJUST" });
                    } else {

                        var b = db.POS_StkAdjustHead.Where(o => o.AdjID == DocSet.head.AdjID && o.ComID ==h.ComID && o.RComID==rcom).FirstOrDefault();

                        b.AdjDate = h.AdjDate;
                        b.AdjQty = h.AdjQty;
                        b.AdjAmt = h.AdjAmt;
                        b.Memo = h.Memo;
                        b.Description = h.Description;
                        b.Remark = h.Remark;
                        b.Status = h.Status;

                        db.POS_StkAdjustLine.RemoveRange(db.POS_StkAdjustLine.Where(o => o.AdjID == h.AdjID && o.RComID == h.RComID && o.ComID==h.ComID));
                        db.POS_StkAdjustLine.AddRange(DocSet.line);

                        db.SaveChanges();
                        CalStock();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.AdjID, TableID = "STK_ADJUST", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.ComID, Action = "UPDATE ADJUST" });
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

        public static void CalDocSet() {
            var h = DocSet.head;
            foreach (var l in DocSet.line) {
                l.AdjID = h.AdjID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.AdjDate = h.AdjDate;
                l.DocType = h.DocType;
                l.AdjQty =  Convert.ToDecimal(l.ActualQty)- Convert.ToDecimal(l.BeginQty);
                l.AdjAmt = l.Price * l.AdjQty;
                l.ActualAmt = l.Price * l.ActualQty;  
            }

            h.AdjQty = DocSet.line.Sum(o => o.AdjQty);
            h.AdjAmt = DocSet.line.Sum(o => o.AdjAmt);
            h.ActualQty = DocSet.line.Sum(o => o.ActualQty);
            h.ActualAmt = DocSet.line.Sum(o => o.ActualAmt);
        }

        public static I_BasicResult SetStatusCLOSE(string adjid)
        {
            //OPEN / CLOSED 
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = db.POS_StkAdjustHead.Where(o => o.AdjID == adjid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "CLOSED";
                    h.ApprovedBy = LoginService.LoginInfo.CurrentUser;
                    h.ApprovedDate = DateTime.Now;

                    db.SaveChanges();
                    CalStock();
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


        public static I_BasicResult NewLineByItem(POS_StkAdjustLine adjLine, string item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var oline = DocSet.line.Where(o => o.ItemID == item).FirstOrDefault();
                if (oline == null) {//add new stkadjline 
                    DocSet.line.Add(adjLine);
                } else {
                    //exist stkadjline
                    oline.ActualQty = adjLine.ActualQty;
                }
                CalDocSet();
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

        public static void checkDupID() {
            try {
                DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = DocSet.head;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_StkAdjustHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.AdjID == h.AdjID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GetNewID("ADJUST", h.ComID, true, "th", h.AdjDate);

                        h.AdjID = IDRuunerService.GetNewID("ADJUST", h.ComID, true, "th", h.AdjDate)[1];
                        get_id = db.POS_StkAdjustHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.AdjID == h.AdjID).FirstOrDefault();
                    }
                }
                CalDocSet();
            } catch (Exception ex) {
            }

        }



        #endregion

        #region Delete

        public static void DeleteDoc(string docid) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_StkAdjustHead.Where(o => o.AdjID == docid && o.RComID == rcom).FirstOrDefault();
                    var line = db.POS_StkAdjustLine.Where(o => o.AdjID == docid && o.RComID == rcom).ToList();
                    head.Status = "CLOSE";
                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    CalStock();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.AdjID, TableID = "STK_ADJUST", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.AdjID, Action = "DELETE DATA ADJUST" });
                }
            } catch (Exception ex) {

                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void DeleteLine(int linenum) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                DocSet.line.RemoveAll(o => o.LineNum == linenum);
            } catch (Exception ex) {

                DocSet.OutputAction.Result = "fail";
                DocSet.OutputAction.Message1 = ex.Message;
                if (ex.InnerException != null) {
                    DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " " + ex.InnerException.Message;
                }
            }
        }

        #endregion

        #region New transaction

        public static void NewFilterSet() {
            FilterSet = new I_StkAdjusFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date.AddDays(-7);
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.DocType = "";
            FilterSet.Status = "";
            FilterSet.SearchBy = "DOCDATE";
            FilterSet.SearchText = "";
            FilterSet.ShowActive = true;
        }

        public static void NewTransaction() {
            DocSet = new I_StkAdjustSet();
            DocSet.Action = "";
            DocSet.head = NewHead();
            DocSet.line = new List<POS_StkAdjustLine>();
            DocSet.NeedRunNextID = false;
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            IsNewDoc = false;
        }

        public static POS_StkAdjustHead NewHead() {
            POS_StkAdjustHead n = new POS_StkAdjustHead();

            n.ComID = "";
            n.RComID =  LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.AdjID = "";
            n.DocType = "ADJUST";
            n.AdjDate = DateTime.Now.Date;
            n.LocID = "";
            n.Description = "";
            n.Remark = "";
            n.Memo = "";
            n.AdjQty = 0;
            n.AdjAmt = 0;
            n.ActualQty = 0;
            n.ActualAmt = 0;
            n.ApprovedBy = "";
            n.ApprovedDate = DateTime.Now.Date;
            n.Status = "OPEN";
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static POS_StkAdjustLine NewLine() {
            POS_StkAdjustLine n = new POS_StkAdjustLine();

            n.ComID = "";
            n.RComID = "";
            n.AdjID = "";
            n.DocType = "";
            n.LineNum = GenLineNum();
            n.AdjDate = DateTime.Now.Date;
            n.LocID = "";
            n.ItemID = "";
            n.Name = "";
            n.Price = 0;
            n.BeginQty = 0;
            n.AdjQty = 0;
            n.AdjAmt = 0;
            n.ActualQty = 0;
            n.ActualAmt = 0;
            n.Unit = "";
            n.Memo = "";
            n.IsActive = true;
            return n;
        }
        public static POS_STKBal NewSTKBal() {
            POS_STKBal n = new POS_STKBal();

            n.ComID = "";
            n.RComID = "";
            n.LocID = "";
            n.SubLocID = "";
            n.ItemID = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.OrdQty = 0;
            n.InstQty = 0;
            n.BalQty = 0;
            n.RetQty = 0;
            n.Unit = "";
            n.IsActive = true;

            return n;
        }

        public static void STKBalConvert2AdjustLine(List<vw_POS_STKBal> input) {
            var line = DocSet.line;
            foreach (var i in input) {
                var chkExist = line.Where(o => o.ItemID == i.ItemID).FirstOrDefault();
                if (chkExist != null) {
                    continue;
                }
                POS_StkAdjustLine n = NewLine();
                n.ItemID = i.ItemID;
                n.Name = i.ItemName;
                n.BeginQty = i.BalQty;
                n.ActualQty = i.BalQty;
                n.Price = i.Price;
                n.LocID = i.LocID;
                n.Unit = i.UnitID;
                DocSet.line.Add(n);
            }
            CalDocSet();

        }


        public static int GenLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }

        public static void CalStock() {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var h = DocSet.head;
                using (GAEntities db = new GAEntities()) {
                    db.SP_CalStkAdjustMove(h.AdjID, h.DocType, h.RComID, h.ComID);
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

        #endregion

    }
}