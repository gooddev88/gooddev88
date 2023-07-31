using Robot.Data;
using Robot.Data.DataAccess;

using Robot.Data.FileStore;
using Robot.OMASTER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;


namespace Robot.POS.DA {
    public static class ORCService {
        public class I_RCSet {
            public string Action { get; set; }

            public ORCHead Head { get; set; }
            public List<ORCLine> Line { get; set; }
            public ORCLine LineActive { get; set; }
            public List<ORCPayLine> PaymentLine { get; set; }
            public ORCPayLine PaymentLineActive { get; set; }
            public List<ORCAdjust> AdjustLine { get; set; }
            public List<vw_XFilesRef> DocFiles { get; set; }
            public List<TransactionLog> Log { get; set; }

            public Data.BL.I_Result.I_BasicResult OutputAction { get; set; }
        }
        public class I_RCFiterSet {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String SearchTextForSelectCust { get; set; }
            public String Company { get; set; }
            public String Customer { get; set; }
            public String Location { get; set; }
            public String Status { get; set; }
            public bool ShowClosed { get; set; }
            public bool ShowActive { get; set; }
        }

        //public class I_STrackingFiterSet {
        //    public String ChqReturn { get; set; }
        //    public String Payment { get; set; }
        //    public String Status { get; set; }
        //    public String SearchBy { get; set; }
        //    public String SearchText { get; set; }
        //}

        public class I_StatementStatus {
            public String Code { get; set; }
            public String Desc { get; set; }
        }

        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        public static I_RCSet DocSet { get { return (I_RCSet)HttpContext.Current.Session["rcdocset"]; } set { HttpContext.Current.Session["rcdocset"] = value; } }
        public static I_RCFiterSet FilterSet { get { return (I_RCFiterSet)HttpContext.Current.Session["rcfilter_set"]; } set { HttpContext.Current.Session["rcfilter_set"] = value; } }

        #region Query Transaction
        public static I_BasicResult GetDocSetByID(int id) {

            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
                NewTransaction();
                using (GAEntities db = new GAEntities()) {
                    DocSet.Head = db.ORCHead.Where(o => o.ID == id).FirstOrDefault();
                    DocSet.Line = db.ORCLine.Where(o => o.RCID == DocSet.Head.RCID && o.RCompanyID == rcom).ToList();
                    DocSet.AdjustLine = db.ORCAdjust.Where(o => o.RCID == DocSet.Head.RCID && o.CompanyID == com && o.RCompanyID == rcom).ToList();
                    DocSet.PaymentLine = db.ORCPayLine.Where(o => o.RCID == DocSet.Head.RCID && o.CompanyID == com && o.RCompanyID == rcom).ToList();
                    DocSet.LineActive = DocSet.Line.FirstOrDefault();
                    DocSet.DocFiles = new List<vw_XFilesRef>();
                    DocSet.Log = TransactionInfoService.ListLogByDocID(DocSet.Head.RCID, DocSet.Head.CompanyID);

                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            DocSet.OutputAction = result;
            return result;
        }


        public static List<vw_OINVHead> ListInvoice(DateTime begin, DateTime end) {
            List<vw_OINVHead> result = new List<vw_OINVHead>();
            var comid = ORCService.DocSet.Head.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var custid = ORCService.DocSet.Head.CustomerID;

            var invs = ORCService.DocSet.Line.Select(o => (string)o.INVID).ToList();

            using (GAEntities db = new GAEntities()) {
                result = db.vw_OINVHead.Where(o =>

                                              (o.SOINVDate >= begin && o.SOINVDate <= end)
                                             && o.CompanyID == comid 
                                             && !invs.Contains(o.SOINVID)
                                             && o.RCompanyID == rcom
                                             && o.CustomerID == custid
                                             && o.INVPendingAmt != 0
                                             && o.IsActive
                ).OrderBy(o => o.SOINVID).ThenBy(o => o.SOINVDate).ToList();
            }

            return result;
        }


        public static ORCHead GetRCIDHeadByID(string rcid) {
            ORCHead result = new ORCHead();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.ORCHead.Where(o => o.RCID == rcid && o.CompanyID == com && o.RCompanyID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static ORCHead GetRCIDHeadByID(int id) {
            ORCHead result = new ORCHead();
            using (GAEntities db = new GAEntities()) {
                result = db.ORCHead.Where(o => o.ID == id).FirstOrDefault();
            }
            return result;
        }

        public static List<ORCLine> GetViewRCIDLineByID(string rcid) {
            List<ORCLine> result = new List<ORCLine>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.ORCLine.Where(o => o.RCID == rcid && o.CompanyID == com && o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }

        public static List<GLAccount> ListGLAccountLineByID(string comId)
        {
            List<GLAccount> result = new List<GLAccount>();
            //var comId = LoginService.LoginInfo.CurrentCompany;
            using (GAEntities db = new GAEntities())
            {
                result = db.GLAccount.Where(o => o.IsActive && o.CompanyID == comId).ToList();
            }
            return result;
        }



        public static List<vw_ORCHead> ListDoc() {
            List<vw_ORCHead> result = new List<vw_ORCHead>();
            var f = FilterSet;
            var uic = LoginService.LoginInfo.UserInCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            //var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                if (f.SearchText != "") {
                    result = db.vw_ORCHead.Where(o => (
                                                              o.RCID.Contains(f.SearchText)
                                                             || o.CompanyID.Contains(f.SearchText)
                                                             || o.CustomerID.Contains(f.SearchText)
                                                             || o.CustomerName.Contains(f.SearchText)
                                                             || o.RCStatus.Contains(f.SearchText)
                                                             || o.PayBy.Contains(f.SearchText)
                                                            )
                                                            //&& o.CompanyID == com
                                                            && uic.Contains(o.CompanyID)
                                                            && o.RCompanyID == rcom
                                                            && o.IsActive == f.ShowActive
                                                            )
                                                            .OrderByDescending(o => o.CreatedDate).ToList();

                } else {
                    result = db.vw_ORCHead.Where(o =>
                        (o.RCDate >= f.DateFrom && o.RCDate <= f.DateTo)
                                        //&& o.CompanyID == com
                                        && uic.Contains(o.CompanyID)
                                        && o.RCompanyID == rcom
                                        && o.IsActive == f.ShowActive
                                        ).OrderByDescending(o => o.CreatedDate).ToList();
                }



            }
            return result;
        }
 
        public static void GetLineActive(int linenum) {

            DocSet.LineActive = DocSet.Line.Where(o => o.LineNum == linenum).FirstOrDefault();
        }

        #endregion

        #region Save

        public static I_BasicResult Save(string action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try {
                using (GAEntities db = new GAEntities()) {

                    ORCService.DocSet = CalDocSet(ORCService.DocSet);
                    var h = DocSet.Head;
             
                    if (action == "insert") {
                        //checkDupID();
                        db.ORCHead.Add(DocSet.Head);
                        db.ORCLine.AddRange(DocSet.Line);
                        db.ORCPayLine.AddRange(DocSet.PaymentLine);
                        db.ORCAdjust.AddRange(DocSet.AdjustLine);
                        db.SaveChanges();
                        IDRuunerService.GetNewID("ORC", h.CompanyID, true, "th", h.RCDate);
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.RCID, TableID = "ORC", ParentID = "", TransactionDate = h.CreatedDate, CompanyID = h.CompanyID, Action = "Create O Receipt" });


                    }
                    if (action == "update") {

                        var hq = db.ORCHead.Where(o => o.RCID == DocSet.Head.RCID && o.CompanyID == com && o.RCompanyID == rcom).FirstOrDefault();

                        hq.RCDate = DocSet.Head.RCDate;
                        hq.PayDate = DocSet.Head.PayDate;
                        hq.CustomerID = DocSet.Head.CustomerID;
                        hq.CustomerName = DocSet.Head.CustomerName;
                        hq.CustTaxID = DocSet.Head.CustTaxID;
                        hq.DocType = DocSet.Head.DocType;
                        hq.PayINVAmt = DocSet.Head.PayINVAmt;
                        hq.PayINVVatAmt = DocSet.Head.PayINVVatAmt;
                        hq.PayINVTotalAmt = DocSet.Head.PayINVTotalAmt;
                        hq.PayTotalAmt = DocSet.Head.PayTotalAmt;
                        hq.PayTotalDiffINVAmt = DocSet.Head.PayTotalDiffINVAmt; 

                        hq.Remark1 = DocSet.Head.Remark1;
                        hq.Remark2 = DocSet.Head.Remark2;
                        hq.Status = DocSet.Head.Status;
                        hq.RCStatus = DocSet.Head.RCStatus;
                        hq.CompletedDate = DocSet.Head.CompletedDate;
                        hq.PayBy = DocSet.Head.PayBy;
                        hq.PayToBookName = DocSet.Head.PayToBookName;
                     
                        hq.ClearingDate = DocSet.Head.ClearingDate;
                        hq.StatementDate = DocSet.Head.StatementDate;
                        hq.CustBankCode = DocSet.Head.CustBankCode;
                        hq.CustBankBranch = DocSet.Head.CustBankBranch;
                        hq.CustBankName = DocSet.Head.CustBankName;

                        hq.PaymentRefNo = DocSet.Head.PaymentRefNo;
                        hq.ChqDate = DocSet.Head.ChqDate;
                        hq.ChqDepositDate = DocSet.Head.ChqDepositDate;
                        hq.ChqExpired = DocSet.Head.ChqExpired;
                        hq.ChqReturnDate = DocSet.Head.ChqReturnDate;
                        hq.ChqReturnReason = DocSet.Head.ChqReturnReason;
                        hq.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        hq.ModifiedDate = DateTime.Now;


                        db.ORCPayLine.RemoveRange(db.ORCPayLine.Where(o => o.RCID == hq.RCID && o.CompanyID == com && o.RCompanyID == rcom));
                        db.ORCPayLine.AddRange(DocSet.PaymentLine);

                        db.ORCLine.RemoveRange(db.ORCLine.Where(o => o.RCID == hq.RCID && o.CompanyID == com && o.RCompanyID == rcom));
                        db.ORCLine.AddRange(DocSet.Line);
                        db.ORCAdjust.RemoveRange(db.ORCAdjust.Where(o => o.RCID == hq.RCID && o.CompanyID == com && o.RCompanyID == rcom));
                        db.ORCAdjust.AddRange(DocSet.AdjustLine);

                        db.SaveChanges();

                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Head.RCID, TableID = "ORC", ParentID = "", TransactionDate = DocSet.Head.CreatedDate, CompanyID = DocSet.Head.CompanyID, Action = "Edit O Receipt" });


                    }
                    RefreshSet(DocSet.Head.RCID);
                }
            } catch (Exception ex) {
                result.Result = "fail";

                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }

            DocSet.OutputAction = result;
            return result;
        }


        public static void checkDupID() {
            try {
                DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = ORCService.DocSet.Head;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.ORCHead.Where(o => o.RCID == h.RCID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 20) {
                            DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot cread document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GetNewID("ORC", h.CompanyID, true, "th", h.RCDate);
                        DocSet.Head.RCID = IDRuunerService.GetNewID("ORC", h.CompanyID, false, "th", h.RCDate)[1];
                        get_id = db.ORCHead.Where(o => o.RCID == DocSet.Head.RCID).FirstOrDefault();



                    }
                }
            } catch (Exception ex) {
            }
        }


        #endregion

        public static void AddLine() {


        }
        public static void AddPaymentLine() {
            ClearPaymentLinePending();
            DocSet.PaymentLine.Add(NewPaymentLine());
            DocSet.PaymentLineActive = DocSet.PaymentLine.Where(o => o.Status == "NEW").OrderByDescending(o => o.LineNum).FirstOrDefault();
            //ListDocLine();

        }

        #region Delete transaction
        public static I_BasicResult Delete(string docId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.ORCHead.Where(o => o.RCID == docId && o.CompanyID == com && o.RCompanyID==rcom).FirstOrDefault();
                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;
                    db.ORCLine.Where(o => o.RCID == docId && o.CompanyID == com && o.RCompanyID == rcom).ToList().ForEach(o => o.IsActive = false);
                    db.ORCAdjust.Where(o => o.RCID == docId && o.CompanyID == com && o.RCompanyID == rcom).ToList().ForEach(o => o.IsActive = false);
                    db.SaveChanges();
                    RefreshSet(head.RCID);

                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = head.RCID, TableID = "ORC", ParentID = "", TransactionDate = head.CreatedDate, CompanyID = head.CompanyID, Action = "Delete O Receipt" });


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

        public static I_BasicResult DeleteLine(int linenum) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                DocSet.Line.RemoveAll(o => o.LineNum == linenum);
                ORCService.DocSet = CalDocSet(ORCService.DocSet);

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
        public static I_BasicResult DeleteLineAdjust(int linenum) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                DocSet.AdjustLine.RemoveAll(o => o.LineNum == linenum);
                ORCService.DocSet = CalDocSet(ORCService.DocSet);

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
        #region  New Transaction 
        public static void NewFilterSet() {
            FilterSet = new I_RCFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date.AddMonths(-1);
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.DocType = "";
            FilterSet.SearchBy = "DOCDATE";
            FilterSet.SearchText = "";
            FilterSet.Company = "";
            FilterSet.SearchTextForSelectCust = "";
            FilterSet.Customer = "";
            FilterSet.Location = "";
            FilterSet.Status = "OPEN";
            FilterSet.ShowClosed = false;
            FilterSet.ShowActive = true;
        }

        //public static void NewSTrackingFiterSet() {
        //    STrackingFiterSet = new I_STrackingFiterSet();
        //    STrackingFiterSet.SearchBy = "DOCDATE";
        //    STrackingFiterSet.SearchText = "";
        //    STrackingFiterSet.Status = "";
        //    STrackingFiterSet.Payment = "";
        //    STrackingFiterSet.ChqReturn = "";
        //}


        public static void NewTransaction() {
            DocSet = new I_RCSet();
            DocSet.Action = "";
            DocSet.Head = NewHead();
            DocSet.Line = new List<ORCLine>();
            DocSet.AdjustLine = new List<ORCAdjust>();
            DocSet.PaymentLine = new List<ORCPayLine>();
            DocSet.PaymentLineActive = new ORCPayLine();
            DocSet.Log = new List<TransactionLog>();
            DocSet.LineActive = new ORCLine();
            DocSet.DocFiles = new List<vw_XFilesRef>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

        }
        public static ORCHead NewHead() {
            ORCHead n = new ORCHead();

            n.RCID = "";
            n.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.CompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.DocType = "";
            n.RCDate = DateTime.Now.Date;
            n.CustomerID = "";
            n.CustomerName = "";
            n.Currency = "THB";
            n.RateExchange = 1;
            n.RateBy = "AR";
            n.RateDate = DateTime.Now.Date;

            n.PayINVAmt = 0;
            n.PayINVVatAmt = 0;
            n.PayINVTotalAmt = 0;
            n.PayTotalAmt = 0;
            n.PayTotalDiffINVAmt = 0;
            n.Remark1 = "";
            n.Remark2 = "";

            n.LinkDate = null;
            n.LinkBy = "";
            n.PayBy = "";
            n.PayMemo = "";
            n.PayToBankCode = "";
            n.PayToBankCode = "";
            n.PayToBookID = "";
            n.PayToBookName = "";
            n.PayToBankCode = "";
            n.PayDate = DateTime.Now.Date;
            n.ClearingDate = null;
            n.StatementDate = null;
            n.CustBankCode = "";
            n.CustBankBranch = "";
            n.CustBankName = "";
            n.DataSource = "ACCOUNTING";
            n.PaymentRefNo = "";
            n.CountInvLine = 0;
            n.CountPaymentLine = 0;
            n.ChqDate = null;
            n.Status = "OPEN";
            n.RCStatus = "OPEN";
            n.ChqDepositDate = null;
            n.ChqExpired = null;
            n.ChqReturnDate = null;
            n.ChqReturnReason = "";
            n.CompletedDate = null;
            n.CompletedMemo = "";
            n.LinkRefID = "";
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null; ;
            n.IsActive = true;
            return n;
        }
        public static ORCLine NewLine() {
            ORCLine n = new ORCLine();
            n.RCompanyID = "";
            n.LineNum = GenLineNum();
            n.CompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.CompanyBillingID = "";
            n.INVID = "";
            n.INVDate = DateTime.Now.Date;
            n.InvDueDate = DateTime.Now.Date;
            n.ShipID = "";
            n.SOID = "";
            n.BillID = "";
            n.PayNo = 0;
            n.CustomerID = "";
            n.InvPreviousAmt = 0;
            n.VatRate = 0;
            n.PayVatAmt = 0;
            n.InvTotalAmt = 0;
            n.PayAmt = 0;
            n.PayTotalAmt = 0; 

            n.RCStatus = "PENDING";
            n.Status = "NEW";

            n.Remark1 = "";
            n.Remark2 = "";
            n.Remark3 = "";
            n.IsActive = true;
            return n;
        }
        public static ORCPayLine NewPaymentLine() {
            ORCPayLine n = new ORCPayLine();
            n.RCID = "";
            n.CompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.RCompanyID = "";
            n.DocType = "";
            n.LineNum = GenPaymentLineNum();
            n.RCDate = DateTime.Now.Date;
            n.CustomerID = "";
            n.CustomerName = "";
            n.WHTRefNo = "";
            n.TaxBaseAmt = 0;
            n.TaxRate = 0;           
            n.PayBy = "CASH";
            n.PayByType = "";
            n.PayByCate = "";
         
            n.PayMemo = "";
            n.PayAmt = 0;
            n.PayToBankCode = "";
            n.PayToBookID = "";
            n.PayToBookName = "";
            n.PayDate = DateTime.Now.Date;
            n.ClearingDate = null;
            n.StatementDate = null;
            n.CustBankCode = "";
            n.CustBankName = "";
            n.CustBankBranch = "";
            n.PaymentRefNo = "";
            n.ChqDate = null;
            n.ChqDepositDate = null;
            n.ChqExpired = null;
            n.ChqReturnDate = null;
            n.ChqReturnReason = "";
            n.CompletedDate = null;
            n.CompletedMemo = "";
            n.Currency = "THB";
            n.RateExchange = 1;
            n.RateBy = "AR";
            n.RateDate = DateTime.Now.Date;
            n.DataSource = "KYPOS";
            n.RCStatus = "OPEN";
            n.Status = "NEW";
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;

            return n;
        }
        public static ORCAdjust NewGLAdust() {
            ORCAdjust n = new ORCAdjust();

            n.LineNum = GenAdjustLineNum();
            n.CompanyID = "";
            n.RCompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.RCID = "";
            n.GLCode = "";
            n.RCDate = DateTime.Now.Date;
            n.GLNam = "";
            n.GLCate = "";
            n.Sort = n.LineNum;
            n.CrAmt = 0;
            n.DrAmt = 0;
            n.Currency = "";
            n.Rate = 0;
            n.Remark1 = "";
            n.Remark2 = "";
            n.IsActive = true;
            return n;
        }
        public static void NewAdustByGlID(string gl_code,string comId)
        {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            GLAccount result = new GLAccount();
            //var comId = LoginService.LoginInfo.CurrentCompany;
            using (GAEntities db = new GAEntities())
            {
                var gl = db.GLAccount.Where(o => o.Code == gl_code && o.CompanyID == comId && o.IsActive).FirstOrDefault();
                if (gl == null)
                {
                    DocSet.OutputAction.Result = "fail";
                    DocSet.OutputAction.Message1 = "GL Account not found";
                    return;
                }
                var chk_exit_gl = DocSet.AdjustLine.Where(o => o.GLCode == gl_code && o.CompanyID == comId).FirstOrDefault();
                if (chk_exit_gl != null)
                {
                    DocSet.OutputAction.Result = "fail";
                    DocSet.OutputAction.Message1 = "Duplicate GL Account";
                    return;
                }

                var n = NewGLAdust();

                n.GLCode = gl.Code;
                n.GLNam = gl.Name;
                n.GLCate = gl.Cate;
                n.Remark1 = gl.Remark;
                ORCService.DocSet.AdjustLine.Add(n);
            }
        }

        public static I_BasicResult ClearPaymentLinePending() {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var r1 = DocSet.PaymentLine.RemoveAll(o => o.Status == "NEW");
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
        public static int GenLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.Line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenPaymentLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.PaymentLine.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenAdjustLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.AdjustLine.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenNextID(string table) {
            int result = 1000000;
            try {
                int max_id = 1000000;

                if (table.ToLower() == "line") {
                    max_id = DocSet.Line.Max(o => o.ID);
                }

                result = max_id + 1;
            } catch { }
            return result;
        }

        #endregion

        #region Calculator

        public static I_RCSet CalDocSet(I_RCSet input) {

            var head = input.Head;
            var line = input.Line;
            var payline = input.PaymentLine;
            var adjustline = input.AdjustLine;

            var first_payline = payline.OrderByDescending(o => o.LineNum).FirstOrDefault();

            foreach (var l in line) {   //12. copy head to line
                l.RCID = head.RCID;
                l.RCDate = head.RCDate;
                l.DocType = head.DocType;
                l.CompanyID = head.CompanyID;
                l.RCompanyID = head.RCompanyID;
                l.CustomerID = head.CustomerID;
                l.RCStatus = head.RCStatus;
                l.Status = head.Status;
                
                l.PayVatAmt = Math.Round((l.PayTotalAmt * l.VatRate) / (100 + l.VatRate), 2, MidpointRounding.AwayFromZero);
                //l.PayVatAmt = Math.Round( (l.PayTotalAmt * l.VatRate) * (l.VatRate + 100),3,MidpointRounding.AwayFromZero);            
                l.PayAmt = l.PayTotalAmt-l.PayVatAmt;             
            }



            //09 copy head 2 payment line
            foreach (var l in payline) {
                l.RCID = head.RCID;
                l.RCDate = head.RCDate;
                l.CompanyID = head.CompanyID;
                l.RCompanyID = head.RCompanyID;
                l.DocType = head.DocType;
                l.PayDate = head.PayDate;
                l.RateExchange = head.RateExchange;
                l.RateDate = head.RateDate;
            } 
            //10 copy line 2 adjus
            foreach (var l in adjustline) {
                l.RCID = head.RCID;
                l.RCDate = head.RCDate;
                l.CompanyID = head.CompanyID;
                l.RCompanyID = head.RCompanyID;          
            }


            //update head for first payment method
            if (first_payline != null) {
                head.RCompanyID = first_payline.RCompanyID;
                head.RCStatus = first_payline.RCStatus;
                head.PayMemo = first_payline.PayMemo;
                head.PayToBookID = first_payline.PayToBookID;
                head.PayToBookName = first_payline.PayToBookName;
                head.PayToBankCode = first_payline.PayToBankCode;
                head.ClearingDate = first_payline.ClearingDate;
                head.StatementDate = first_payline.StatementDate;
                head.CustBankCode = first_payline.CustBankCode;
                head.CustBankName = first_payline.CustBankName;
                head.CustBankBranch = first_payline.CustBankBranch;
                head.PaymentRefNo = first_payline.PaymentRefNo;
                head.ChqDate = first_payline.ChqDate;
                head.ChqDepositDate = first_payline.ChqDepositDate;
                head.ChqExpired = first_payline.ChqExpired;
                head.ChqReturnDate = first_payline.ChqReturnDate;
                head.ChqReturnReason = first_payline.ChqReturnReason;
                head.CompletedDate = first_payline.CompletedDate;
                head.CompletedMemo = first_payline.CompletedMemo;

            }

            //13.copy line to head
            head.CountPaymentLine = payline.Count;
            head.CountInvLine = line.Count;
            head.PayINVAmt = line.Sum(o => o.PayAmt);
            head.PayINVVatAmt = line.Sum(o => o.PayVatAmt);
            head.PayINVTotalAmt = line.Sum(o => o.PayTotalAmt);
            head.PayTotalAmt = payline.Sum(o => o.PayAmt);
            head.PayTotalDiffINVAmt =  head.PayINVTotalAmt- head.PayTotalAmt;

            //ถ้า payment method confirm ครบจะถือว่า การรับชำระสมบูรณ์และต้องด้วยเงื่อนไขว่ามีการรับชำระแล้วด้วย
            var countPaymethodNotComfirm = payline.Where(o => o.RCStatus != "CONFIRM").ToList().Count;
            if (countPaymethodNotComfirm > 0 && payline.Count > 0) {
                head.Status = "OPEN";
            } else {
                head.Status = "CLOSED";
            }
            return input;
        }

        #endregion




        public static I_BasicResult DeleteAdjust(int linenum) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                ORCService.DocSet.AdjustLine.RemoveAll(o => o.LineNum == linenum);
                ORCService.DocSet = CalDocSet(ORCService.DocSet);
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

        public static I_BasicResult DeletePayment(int linenum) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                ORCService.DocSet.PaymentLine.RemoveAll(o => o.LineNum == linenum);
                ORCService.DocSet = CalDocSet(ORCService.DocSet);
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


        public static I_BasicResult AddRCLineFromInv(List<string> invIds) {

            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = ORCService.DocSet.Head;
            //var comId = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try {
                using (GAEntities db = new GAEntities()) {
                    var exist_invid = DocSet.Line.Select(o => o.INVID).ToList();
                    var inv_list = db.OINVHead.Where(o => invIds.Contains(o.SOINVID) && o.RCompanyID == rcom && o.CompanyID == h.CompanyID && o.INVPendingAmt != 0).OrderBy(o => o.SOINVID).ToList();
                    inv_list = inv_list.Where(o => !exist_invid.Contains(o.SOINVID)).ToList();
                    var rcs = db.ORCLine.Where(o => invIds.Contains(o.INVID) && o.IsActive).ToList();

                    foreach (var i in inv_list) {
                        ORCLine n = NewLine();
                        var lastPayNo = rcs.Count == 0 ? 0 : rcs.Where(o => o.INVID == i.SOINVID).Max(o => o.PayNo);

                        n.INVID = i.SOINVID;
                        n.CompanyID = i.CompanyID;
                        n.RCompanyID = i.RCompanyID;
                        n.CompanyBillingID = i.CompanyBillingID;
                        n.BillID = i.BillingID;
                        n.INVDocTypeID = i.DocTypeID;
                        n.InvDueDate = Convert.ToDateTime(i.PayDueDate);
                        n.SOID = i.SOID;
                        n.ShipID = i.ShipID;
                        n.InvPreviousAmt = i.INVPendingAmt;
                        n.InvTotalAmt = i.NetTotalAmtIncVat;
                        n.PayNo = lastPayNo + 1; ;
                        n.PayTotalAmt = Convert.ToDecimal(i.INVPendingAmt);
                        n.VatRate = i.VatRate;
                        n.CustomerID = i.CustomerID;
                        n.INVDate = i.SOINVDate;
                        n.Currency = i.Currency;
                        n.InvRateExchange = i.RateExchange;
                        n.Remark1 = i.Remark1;
                        n.Remark2 = i.Remark2;

                        DocSet.Line.Add(n);
                    }
                }
                ORCService.DocSet = CalDocSet(ORCService.DocSet);
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




        #region Update  rc 2 inv

        public static void RefreshSet(string rcId) {
            UpdateRC2Inv(ListInvoiceIDFormRCID(rcId));
        }

        public static List<string> ListInvoiceIDFormRCID(string rcId) {
            List<string> invidS = new List<string>();
            try {
                var comId = LoginService.LoginInfo.CurrentCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {
                    var ii = db.OINVHead.Where(o => o.RCID == rcId && o.CompanyID == comId).Select(o => o.SOINVID).Distinct().ToList();
                    var rr = db.ORCLine.Where(o => o.RCID == rcId && o.CompanyID == comId).Select(o => o.INVID).Distinct().ToList();
                    invidS = ii.Concat(rr).ToList();
                }
            } catch (Exception ex) {
            }
            return invidS;
        }

        public static I_BasicResult UpdateRC2Inv(List<string> invs) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = ORCService.DocSet.Head;
            try {

                foreach (var i in invs) {
                    ARInvoiceService.RefreshInvoice(i,h.CompanyID);
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.Message.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }




        #endregion
    }
}