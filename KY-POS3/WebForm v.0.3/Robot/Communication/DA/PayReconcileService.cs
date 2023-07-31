using Robot.Data;
using Robot.Data.DataAccess;
using Robot.OMS.THAIPOST.BL;
using Robot.OMS.THAIPOST.DA;
using Robot.OMSReports;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static Robot.Data.BL.I_Result;
using static Robot.Data.DataAccess.XFilesService;

namespace Robot.Communication.DA {
    public static class PayReconcileService
    {
        #region Class
        public class I_PayRCSet {
            public string Action { get; set; }
            public PayReconcile Head { get; set; }
            public List<PayReconcile> Line { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class I_PayRCFiterSet
        {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Company { get; set; }
            public bool ShowClosed { get; set; }
        }


        #endregion

        #region Global var

        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        public static I_PayRCSet DocSet { get { return (I_PayRCSet)HttpContext.Current.Session["payrcset_docset"]; } set { HttpContext.Current.Session["payrcset_docset"] = value; } }
        public static I_PayRCFiterSet FilterSet { get { return (I_PayRCFiterSet)HttpContext.Current.Session["payrcfilter_set"]; } set { HttpContext.Current.Session["payrcfilter_set"] = value; } }


        #endregion

        #region Query Transaction

        public static I_PayRCSet GetDocSetByID(string docid) {
            var doc = NewTransaction();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.Head = db.PayReconcile.Where(o => o.PayID == docid).FirstOrDefault();
                    doc.Log = TransactionInfoService.ListLogByDocID(docid);
                    
                }
            } catch (Exception ex) {
                doc.OutputAction.Result = "ok";

                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            }
            return doc;
        }


        #endregion        

        #region new transaction

        public static void NewFilterSet() {
            FilterSet = new I_PayRCFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date;
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.DocType = "";
            FilterSet.SearchBy = "DOCDATE";
            FilterSet.SearchText = "";
            FilterSet.Company = "";
            FilterSet.ShowClosed = false;
        }

        public static I_PayRCSet NewTransaction() {
            var doc = new I_PayRCSet();
            doc.Head = NewHead();
            doc.Line = new List<PayReconcile>();
            doc.Log = new List<TransactionLog>();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            return doc;
        }

        public static I_BasicResult Save() {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                var h = DocSet.Head;
                using (GAEntities db = new GAEntities()) {
                    var hh = db.PayReconcile.Where(o => o.PayID == h.PayID).FirstOrDefault();

                    if (hh == null) {
                        //checkDupID();
                        db.PayReconcile.Add(DocSet.Head);
                        db.SaveChanges();
                        IDGeneratorServiceV2.GetNewID("PAY", h.CompanyID, true, "th");

                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.PayID, TableID = "PAY", ParentID = "", TransactionDate = h.CreatedDate, CompanyID = h.CompanyID, Action = "CREATE_PAYMENT", ChangeValue = "จ่ายเงินใบคุม" });
                    } else {                        
                        hh.CompanyID = h.CompanyID;
                        hh.PayType = h.PayType;
                        hh.PayDesc = h.PayDesc;
                        hh.PayByStoreID = h.PayByStoreID;
                        hh.PayByPersonID = h.PayByPersonID;
                        hh.PayByBankCode = h.PayByBankCode;
                        hh.PayMethod = h.PayMethod;
                        hh.PayToBookID = h.PayToBookID;
                        hh.PayAmt = h.PayAmt;
                        hh.PayToBookBackCode = h.PayToBookBackCode;
                        hh.PackingID = h.PackingID;
                        hh.OrdID = h.OrdID;
                        hh.Memo = h.Memo;
                        hh.ReconcileDate = h.ReconcileDate;                        

                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.PayID, TableID = "PAY", ParentID = "", TransactionDate = h.CreatedDate, CompanyID = h.CompanyID, Action = "EDIT_PAYMENT", ChangeValue = "แก้ไข้ จ่ายเงินใบคุม" });
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

        public static void checkDupID() {
            try {
                var h = DocSet.Head;
                DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.PayReconcile.Where(o => o.PayID == h.PayID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 20) {
                            DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot cread document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDGeneratorServiceV2.GetNewID("PAY", h.CompanyID, true, "th");
                        h.PayID = IDGeneratorServiceV2.GetNewID("PAY", h.CompanyID, false, "th")[1];
                        get_id = db.PayReconcile.Where(o => o.PayID == h.PayID).FirstOrDefault();
                    }
                }
            } catch (Exception ex) {
            }
        }

        public static PayReconcile NewHead() {
            PayReconcile n = new PayReconcile();
            n.RCompanyID = "BS";
            n.CompanyID = "";
            n.PayID = "";
            n.PayType = "";
            n.PayDesc = "";
            n.PayByStoreID = "";
            n.PayByPersonID = "";
            n.PayByBankCode = "";
            n.PayMethod = "";
            n.PayAmt = 0;
            n.PayToBookID = "";
            n.PayToBookBackCode = "";
            n.PackingID = "";
            n.OrdID = "";
            n.Memo = "";
            n.ReconcileDate = DateTime.Now.Date;
            n.CreatedBy = LoginService.GetLoginInfo().CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.IsActive = true;
            return n;
        }

       

        public static I_BasicResult DeleteDoc(string docid, string remark) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (GAEntities db = new GAEntities()) {
                    var head = db.OrderHead.Where(o => o.ORDID == docid).FirstOrDefault();
                    var line = db.OrderLine.Where(o => o.ORDID == docid).ToList();
                    var addr = db.OrderShipAddr.Where(o => o.ORDID == docid).ToList();
                    var status = db.OrderStatus.Where(o => o.OrdID == docid).ToList();

                    head.ModifiedBy = LoginService.GetLoginInfo().CurrentUser;
                    head.ModifiedDate = DateTime.Now;

                    head.Remark1 = remark;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }
                    foreach (var l in addr) {
                        l.IsActive = false;
                    }
                    foreach (var l in status) {
                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = head.ORDID, ActionType = "DELETE_ORDER", TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = head.CompanyID, ChangeValue = "ลบออเดอร์ด้วยเหตุผล " + remark });
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