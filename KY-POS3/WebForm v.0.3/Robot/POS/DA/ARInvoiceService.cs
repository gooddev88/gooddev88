using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.DataLayer;
using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Robot.POS.DA
{
    public static class ARInvoiceService {
        public class I_SOINVSet {
            public string Action { get; set; }
            public OINVHead Head { get; set; }
            public List<OINVLine> Line { get; set; }
            public OINVLine LineActive { get; set; }
            public List<DLDocLine> LineIndex { get; set; } 
            public List<String> BulkInvIds { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_SOINVFiterSet {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Company { get; set; }
            public String Customer { get; set; }
            public String Location { get; set; }
            public String Status { get; set; }
            public bool ShowClosed { get; set; }

            public bool ShowActive { get; set; }
        }

        public class Sort_INV {
            public int Sort { get; set; }
            public string INV_ID { get; set; }
            public string LockBy { get; set; }
            public string Remark { get; set; }
            public DateTime? LockDate { get; set; }
            public string LockDateText { get; set; }
        }


        public static string PreviousPage { get { return (string)HttpContext.Current.Session["arinv_previouspage"]; } set { HttpContext.Current.Session["arinv_previouspage"] = value; } }

        public static string PreviousBulkForecastDetailPage { get { return (string)HttpContext.Current.Session["arinv_bulk_forecast_datail_previouspage"]; } set { HttpContext.Current.Session["arinv_bulk_forecast_datail_previouspage"] = value; } }
        public static string PreviousPageSelectSHIP { get { return (string)HttpContext.Current.Session["arinv_previouspage_select_ship"]; } set { HttpContext.Current.Session["arinv_previouspage_select_ship"] = value; } }
        public static string PreviousPageSelectItem { get { return (string)HttpContext.Current.Session["arinv_previouspage_select_item"]; } set { HttpContext.Current.Session["arinv_previouspage_select_item"] = value; } }
        public static string PreviousSelectInvoicePage { get { return (string)HttpContext.Current.Session["arinvfcselectinv_previouspage"]; } set { HttpContext.Current.Session["arinvfcselectinv_previouspage"] = value; } }
        public static string PreviousPageNew { get { return (string)HttpContext.Current.Session["arinv_previouspage_newpage"]; } set { HttpContext.Current.Session["arinv_previouspage_newpage"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }

        //fc       
        public static DateTime InvDate { get { return HttpContext.Current.Session["newinvdate"] == null ? DateTime.Now.Date : (DateTime)HttpContext.Current.Session["newinvdate"]; } set { HttpContext.Current.Session["newinvdate"] = value; } }

        public static I_SOINVSet DocSet { get { return (I_SOINVSet)HttpContext.Current.Session["soinvdocset"]; } set { HttpContext.Current.Session["soinvdocset"] = value; } }
        public static List<vw_OINVHead> DocList { get { return (List<vw_OINVHead>)HttpContext.Current.Session["soinvdoc_list"]; } set { HttpContext.Current.Session["soinvdoc_list"] = value; } }        
        public static List<Sort_INV> NewInvoiceIDList { get { return (List<Sort_INV>)HttpContext.Current.Session["newinvid_list"]; } set { HttpContext.Current.Session["newinvid_list"] = value; } }    
        public static I_SOINVFiterSet FilterSet { get { return (I_SOINVFiterSet)HttpContext.Current.Session["soinvfilter_set"]; } set { HttpContext.Current.Session["soinvfilter_set"] = value; } }


        #region Query Transaction
        public static void GetDocSetByID(int id, bool isCopy) {

            NewTransaction();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var comId = LoginService.LoginInfo.CurrentCompany;
            try {
                using (GAEntities db = new GAEntities()) {
                    DocSet.Head = db.OINVHead.Where(o => o.ID == id).FirstOrDefault();
                    DocSet.Line = db.OINVLine.Where(o => o.SOINVID == DocSet.Head.SOINVID && o.CompanyID == DocSet.Head.CompanyID).ToList();
                    DocSet.Log = TransactionInfoService.ListLogByDocID(DocSet.Head.SOINVID);
                    ListDocLine();
                    if (isCopy) {
                        var h = DocSet.Head;
                        h.Status = "OPEN";
                        DocSet.Log = new List<TransactionLog>();
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


        public static OINVHead GetOINVHeadByID(string invid, string comid) {
            OINVHead result = new OINVHead();
            using (GAEntities db = new GAEntities()) {
                result = db.OINVHead.Where(o => o.SOINVID == invid && o.CompanyID == comid).FirstOrDefault();
            }
            return result;
        }

        public static OINVHead GetOINVHeadByID(int id) {
            OINVHead result = new OINVHead();
            using (GAEntities db = new GAEntities()) {
                result = db.OINVHead.Where(o => o.ID == id  ).FirstOrDefault();
            }
            return result;
        }


        public static List<OINVHead> ListOinvoiceBySOId(string soId) {
            List<OINVHead> result = new List<OINVHead>();
            var comId = LoginService.LoginInfo.CurrentCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.OINVHead.Where(o => o.SOID == soId && o.CompanyID==comId && o.IsActive).ToList();
            }
            return result;
        }

        public static void ListDoc() {

            var f = FilterSet;
            var uic = LoginService.LoginInfo.UserInCompany;
            using (GAEntities db = new GAEntities()) {
                if (f.SearchText != "") {
                    DocList = db.vw_OINVHead.Where(o => (
                                                    o.ShipID.Contains(f.SearchText)
                                                             || o.SOINVID.Contains(f.SearchText)
                                                             || o.SOID.Contains(f.SearchText)
                                                             || o.RCID.Contains(f.SearchText)
                                                             || o.BillingID.Contains(f.SearchText)
                                                             || o.CustomerID.Contains(f.SearchText)
                                                             || o.CustomerName.Contains(f.SearchText)
                                                             )
                                                             && uic.Contains(o.CompanyID)
                                                             && o.IsActive == f.ShowActive)
                                                            .OrderByDescending(o => o.CreatedDate).ToList();
                } else {
                    if (f.SearchBy == "invdate") {
                        DocList = db.vw_OINVHead.Where(o =>
                                                (o.SOINVDate >= f.DateFrom && o.SOINVDate <= f.DateTo)
                                                && uic.Contains(o.CompanyID)
                                                && o.IsActive == f.ShowActive
                                                ).OrderByDescending(o => o.CreatedDate).ToList();
                    }
                }
            }
        }
        public static void ListDocForForecast() {

            bool showforecast = FilterSet.ShowClosed;
            string doctype = FilterSet.DocType.ToLower();
            string status = FilterSet.Status.ToLower();
            string search = FilterSet.SearchText;
            DateTime dtbegin = FilterSet.DateFrom;
            DateTime dtend = FilterSet.DateTo;
            string currCom = LoginService.LoginInfo.CurrentCompany.CompanyID;

            int countfcline = FilterSet.ShowClosed ? 9999 : 1;
            using (GAEntities db = new GAEntities()) {
                if (search == "") {
                    DocList = db.vw_OINVHead.Where(o =>
                                            (o.SOINVDate >= dtbegin && o.SOINVDate <= dtend)
                                            && o.IsActive == true
                                            && o.CountFCLine < countfcline
                                            && (o.CompanyID == currCom)
                    ).OrderByDescending(o => o.CreatedDate).ToList();
                }
                if (search != "") {
                    DocList = db.vw_OINVHead.Where(o => (
                                                               o.SOINVID.Contains(search)
                                                             || o.SOID.Contains(search)
                                                             || o.ShipID.Contains(search)
                                                             || o.BillingID.Contains(search)
                                                             || o.CustomerID.Contains(search)
                                                             || o.CustomerName.Contains(search)
                                                             )
                                                            && o.IsActive == true
                                                            && (o.CompanyID == currCom)
                                                            )
                                                            .OrderByDescending(o => o.CreatedDate).ToList();
                }
            }
        }


        public static void ListDocLine() {
            DocSet.LineIndex = new List<DLDocLine>();
            int index = 0;
            foreach (var d in DocSet.Line) {
                var da = new DLDocLine();
                da.ID = d.ID;
                da.LineNum = d.LineNum;
                da.Key = d.SOINVID;
                da.Description = d.ItemID + " (" + d.ItemName + ")";
                da.Index = index;
                da.Type = "";
                da.CurrentIndex = false;
                if (index == 0) {
                    da.IsFirst = true;
                    if (DocSet.Line.Count == 1) {
                        da.IsLast = true;
                    }
                }
                if (index > 0 && DocSet.Line.Count() == index + 1) {
                    da.IsLast = true;
                }
                index++;
                DocSet.LineIndex.Add(da);
            }
        }
        public static void GetLineActive(int linenum) {
            DocSet.LineActive = DocSet.Line.Where(o => o.LineNum == linenum).FirstOrDefault();
        }

        #endregion

        #region Save

        public static I_BasicResult Save(string action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            //var comId = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var h = DocSet.Head;
            try {
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {
                        var rr = checkDupID(h.DocTypeID, h.CompanyID);
                        if (rr.Result == "fail") {
                            result = rr;
                            return result;
                        }

                        CalTax();
                        db.OINVHead.Add(DocSet.Head);
                        db.OINVLine.AddRange(DocSet.Line);

                        db.SaveChanges();
                        IDRuunerService.GetNewID("OINV", h.CompanyID, true, "th", h.CreatedDate);
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.SOINVID, TableID = "OINV", ParentID = "", TransactionDate = DateTime.Now, CompanyID = h.CompanyID, Action = "Create Sale Order Invoice" });
                       
                    }
                    if (action == "update") {
                        CalTax();
                        var hq = db.OINVHead.Where(o => o.SOINVID == h.SOINVID && o.CompanyID == h.CompanyID).FirstOrDefault();

                        hq.SOINVDate = DocSet.Head.SOINVDate;
                        hq.CustomerID = DocSet.Head.CustomerID;
                        hq.CompanyID = DocSet.Head.CompanyID;
                        hq.RCompanyID = DocSet.Head.RCompanyID;
                        hq.CustomerName = DocSet.Head.CustomerName;
                        hq.CustomerAddr1 = DocSet.Head.CustomerAddr1;
                        hq.CustomerAddr2 = DocSet.Head.CustomerAddr2;
                        hq.BillAddr1 = DocSet.Head.BillAddr1;
                        hq.BillAddr2 = DocSet.Head.BillAddr2;
                        hq.RefDocID = DocSet.Head.RefDocID;
                        hq.AccGroupID = DocSet.Head.AccGroupID;
                        hq.POID = DocSet.Head.POID;
                        hq.PODate = DocSet.Head.PODate;
                        hq.SOID = DocSet.Head.SOID;
                        hq.SODate = DocSet.Head.SODate;
                        hq.ShipID = DocSet.Head.ShipID;
                        hq.ShipDate = DocSet.Head.ShipDate;
                        hq.BillToCustomerID = DocSet.Head.BillToCustomerID;
                        hq.ShipFrLocID = DocSet.Head.ShipFrLocID;
                        hq.ShipFrSubLocID = DocSet.Head.ShipFrSubLocID;
                        hq.SalesID1 = DocSet.Head.SalesID1;
                        hq.SalesID2 = DocSet.Head.SalesID2;
                        hq.Currency = DocSet.Head.Currency;
                        hq.RateExchange = DocSet.Head.RateExchange;
                        hq.RateBy = DocSet.Head.RateBy;
                        hq.RateDate = DocSet.Head.RateDate;
                        hq.TermID = DocSet.Head.TermID;
                        hq.PayDueDate = DocSet.Head.PayDueDate;
                        hq.QtyInvoice = DocSet.Head.QtyInvoice;
                        hq.QtyOrder = DocSet.Head.QtyOrder;
                        hq.QtyShip = DocSet.Head.QtyShip;
                        hq.QtyReturn = DocSet.Head.QtyReturn;
                        hq.CustTaxID = DocSet.Head.CustTaxID;
                        hq.CustBrnID = DocSet.Head.CustBrnID;
                        hq.CustBrnName = DocSet.Head.CustBrnName;
                        hq.CustTaxID = DocSet.Head.CustTaxID;
                        hq.RCID = DocSet.Head.RCID;
                        hq.RCDate = DocSet.Head.RCDate;
                        hq.BillingID = DocSet.Head.BillingID;
                        hq.BillingDate = DocSet.Head.BillingDate;
                        hq.CountFCLine = DocSet.Head.CountFCLine;
                        hq.CountLine = DocSet.Head.CountLine;
                        hq.NetTotalAmt = DocSet.Head.NetTotalAmt;
                        hq.NetTotalVatAmt = DocSet.Head.NetTotalVatAmt;
                        hq.NetTotalAmtIncVat = DocSet.Head.NetTotalAmtIncVat;
                        hq.BaseNetTotalAmt = DocSet.Head.BaseNetTotalAmt;
                        hq.OntopDiscPer = DocSet.Head.OntopDiscPer;
                        hq.OntopDiscAmt = DocSet.Head.OntopDiscAmt;
                        hq.INVPendingAmt = DocSet.Head.INVPendingAmt;
                        hq.DiscCalBy = DocSet.Head.DiscCalBy;
                        hq.VatRate = DocSet.Head.VatRate;
                        hq.VatTypeID = DocSet.Head.VatTypeID;
                        hq.Remark1 = DocSet.Head.Remark1;
                        hq.Remark2 = DocSet.Head.Remark2;
                        hq.Remark3 = DocSet.Head.Remark3;
                        hq.PaymentMemo = DocSet.Head.PaymentMemo;
                        hq.Remark4 = DocSet.Head.Remark4;
                        hq.Remark5 = DocSet.Head.Remark5;
                        hq.ModifiedBy = DocSet.Head.ModifiedBy;
                        hq.ModifiedDate = DocSet.Head.ModifiedDate;

                        db.OINVLine.RemoveRange(db.OINVLine.Where(o => o.SOINVID == hq.SOINVID && o.CompanyID == hq.CompanyID));
                        db.OINVLine.AddRange(DocSet.Line);

                        db.SaveChanges();

                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Head.SOINVID, TableID = "OINV", ParentID = "", TransactionDate = DocSet.Head.CreatedDate, CompanyID = DocSet.Head.CompanyID, Action = "Edit Sale Order Invoice" });
                    }
                    RefreshSet(h.SOINVID);
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


        public static I_BasicResult checkDupID(string doctype, string comId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (GAEntities db = new GAEntities()) {
                    //var get_id = db.OINVHead.Where(o => o.SOINVID == DocSet.Head.SOINVID).FirstOrDefault();
                    //int i = 0;
                    //while (get_id != null) {

                    //    i++;
                    //    IDGeneratorServiceV2.GetNewID(doctype, comId, true, "th", Convert.ToDateTime( DocSet.Head.RCDate));

                    //    DocSet.Head.SOINVID = IDGeneratorServiceV2.GetNewID(ARInvoiceService.DocSet.Head.DocTypeID, ARInvoiceService.DocSet.Head.CompanyID, false, "th", Convert.ToDateTime(DocSet.Head.RCDate))[1];
                    //    get_id = db.OINVHead.Where(o => o.SOINVID == DocSet.Head.SOINVID).FirstOrDefault();
                    //}
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

        public static void checkDocSetNull() {
            if (DocSet == null) {
                NewTransaction();
            }
        }

        #endregion

        public static void AddLine() {
            ClearPendingLine();
            DocSet.Line.Add(NewLine());
            DocSet.LineActive = DocSet.Line.Where(o => o.Status == "new").OrderByDescending(o => o.LineNum).FirstOrDefault();
            ListDocLine();
        }
        #region Delete transaction
        public static I_BasicResult DeleteDoc(int id) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string invid = DocSet.Head.SOINVID;
                string comid = DocSet.Head.CompanyID;
                using (GAEntities db = new GAEntities()) {
                    var head = db.OINVHead.Where(o => o.ID == id).FirstOrDefault();
                    var line = db.OINVLine.Where(o => o.SOINVID == head.SOINVID).ToList();

                    DocSet.Head.IsActive = false;
                    foreach (var l in line) {
                        l.IsActive = false;
                    }
                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    db.SaveChanges();
                    RefreshSet(invid);
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
        public static void DeleteLine(int linenum) {
            try {
                DocSet.Line.RemoveAll(o => o.LineNum == linenum);
                DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                CalTax();
            } catch (Exception ex) {

                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        #endregion
        #region  New Transaction 
        public static void NewFilterSet() {
            FilterSet = new I_SOINVFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date.AddDays(-7);
            FilterSet.DateTo = DateTime.Now.Date.AddDays(4);
            FilterSet.DocType = "";
            FilterSet.SearchBy = "";
            FilterSet.SearchText = "";
            FilterSet.Company = "";
            FilterSet.Customer = "";
            FilterSet.Location = "";
            FilterSet.Status = "";
            FilterSet.ShowClosed = false;
            FilterSet.ShowActive = true;
        }
        public static void NewTransaction() {
            DocSet = new I_SOINVSet();
            DocSet.Action = "";

            DocSet.Head = NewHead();
            DocSet.Line = new List<OINVLine>();
            DocSet.LineActive = new OINVLine();
            DocSet.BulkInvIds = new List<string>();
            DocSet.LineIndex = new List<DLDocLine>();
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

        }
        public static OINVHead NewHead() {
            OINVHead n = new OINVHead();

            n.SOINVID = "";
            n.CompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.CompanyBillingID = "";
            n.CustBrnID = "";
            n.CustBrnName = "";
            n.SOINVDate = DateTime.Now.Date;
            n.DocTypeID = "";
            n.CustomerID = "";
            n.CustIDRefInCom = "";
            n.CustomerName = "";
            n.CustomerAddr1 = "";
            n.CustomerAddr2 = "";
            n.PaymentType = "";
            n.RefDocID = "";
            n.ShipID = "";
            n.ShipDate = DateTime.Now.Date;
            n.AccGroupID = "";
            n.CustTaxID = "";
            n.POID = "";
            n.PODate = null; ;
            n.SOID = "";
            n.SODate = DateTime.Now.Date;
            n.RCID = "";
            n.RCDate = null;
            n.RCStatus = "";
            n.BillingID = "";
            n.BillingDate = null;
            n.CountFCLine = 0;
            n.BillToCustomerID = "";
            n.BillAddr1 = "";
            n.BillAddr2 = "";
            n.ShipFrLocID = "";
            n.ShipFrSubLocID = "";
            n.SalesID1 = "";
            n.SalesID2 = "";
            n.Currency = "THB";
            n.RateExchange = 1;
            n.RateBy = "SB";
            n.RateDate = DateTime.Now.Date;
            n.TermID = "";
            n.PayDueDate = null;
            n.QtyOrder = 0;
            n.QtyShip = 0;
            n.QtyInvoice = 0;
            n.QtyReturn = 0;
            n.CountLine = 0;
            n.NetTotalAmt = 0;
            n.NetTotalVatAmt = 0;
            n.NetTotalAmtIncVat = 0;
            n.BaseNetTotalAmt = 0;
            n.OntopDiscPer = 0;
            n.OntopDiscAmt = 0;
            n.DiscCalBy = "A";
            n.VatRate = 0;
            n.VatTypeID = "";
            n.Remark1 = "";
            n.Remark2 = "";
            n.Remark3 = "";
            n.PaymentMemo = "";
            n.Remark4 = "";
            n.Remark5 = "";
            n.LastChatMessage = "";
            n.LastChatDate = null;
            n.LastStatementDate = null;
            n.IsPrint = false;
            n.PrintDate = null;
            n.IsLink = false;
            n.LinkDate = null;
            n.Status = "";

            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.IsReverse = false;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }
        public static OINVLine NewLine() {
            OINVLine n = new OINVLine();

            n.LineNum = GenLineLineNum();
            n.CompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
            n.DocTypeID = "";
            n.SOINVDate = DateTime.Now.Date;
            n.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.SOID = "";
            n.SOLineNum = 0;
            n.ShipID = "";
            n.ShipLineNum = 0;
            n.RefDocID = "";
            n.RefDocLineNum = 0;
            n.CustomerID = "";
            n.ItemID = "";
            n.ItemName = "";
            n.ItemTypeID = "";
            n.ItemCateID = "";
            n.ItemGroupID = "";
            n.ItemAccGroupID = "";
            n.IsStockItem = false;
            n.Weight = 0;
            n.WUnit = "";
            n.QtyOrder = 0;
            n.QtyShip = 0;
            n.QtyInvoice = 0;
            n.QtyReturn = 0;
            n.Unit = "";
            n.Packaging = "";
            n.Price = 0;
            n.PriceIncVat = 0;
            n.TotalAmt = 0;
            n.VatAmt = 0;
            n.TotalAmtIncVat = 0;
            n.VatRate = DocSet.Head.VatRate;
            n.VatTypeID = DocSet.Head.VatTypeID;
            n.BaseTotalAmt = 0;
            n.OntopDiscAmt = 0;
            n.OntopDiscPer = 0;
            n.DiscPer = 0;
            n.DiscAmt = 0;
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
            n.Status = "new";
            n.IsComplete = false;
            n.IsActive = true;
            return n;
        }

        public static void ClearPendingLine() {

            try {
                var r1 = DocSet.Line.RemoveAll(o => o.Status == "new");
                DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            } catch (Exception ex) {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }

        }
        public static int GenLineLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.Line.Max(o => o.LineNum);
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

        public static void CalTax() {
            var head = DocSet.Head;
            var line = DocSet.Line;

            foreach (var l in line) {
                //1.cal line base total amt
                l.BaseTotalAmt = l.QtyInvoice * l.Price;
                //2. cal disc amt
                if (l.DiscCalBy == "P") {
                    l.DiscAmt = (l.BaseTotalAmt * l.DiscPer) / 100;
                }
                //3. cal disc per
                if (l.DiscCalBy == "A") {
                    l.DiscPer = 0;
                    if (l.BaseTotalAmt != 0) {
                        l.DiscPer = (100 * l.DiscAmt) / l.BaseTotalAmt;
                    }
                }
            }

            //5. cal head base total amt
            var head_amt_aft_dis = line.Sum(o => o.BaseTotalAmt) - line.Sum(o => o.DiscAmt);
            head.BaseNetTotalAmt = head_amt_aft_dis;
            //6. cal head disc amt
            if (head.DiscCalBy == "P") {
                head.OntopDiscAmt = (head.BaseNetTotalAmt * head.OntopDiscPer) / 100;
            }
            //7. cal head disc per
            if (head.DiscCalBy == "A") {
                if (head.BaseNetTotalAmt != 0) {
                    head.OntopDiscPer = (100 * head.OntopDiscAmt) / head.BaseNetTotalAmt;
                }
            }

            foreach (var l in line) {
                //12. copy head to line
                l.SOINVID = head.SOINVID;
                l.RCompanyID = head.RCompanyID;
                l.CompanyID = head.CompanyID;
                l.DocTypeID = head.DocTypeID;
                l.SOINVDate = head.SOINVDate;
                l.CustomerID = head.CustomerID;

                //4 เผื่อกรณี โปรแกรม error ไม่มีค่า Vattype ในระดับ Line
                if (string.IsNullOrEmpty(l.VatTypeID)) {
                    l.VatRate = head.VatRate;
                    l.VatTypeID = head.VatTypeID;
                }

                //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
                l.OntopDiscPer = head.OntopDiscPer;
                l.OntopDiscAmt = Math.Round(head.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);

                //8.cal line disc weight ontop percent & amt

                if (head.BaseNetTotalAmt != 0) {
                    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * head.OntopDiscAmt) / head.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                }
                //9 cal line total amt 
                l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero); ;
                //10.cal line vat amt
                l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 2, MidpointRounding.AwayFromZero);
                //11.cal line nettotal amt inc vat
                l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 2, MidpointRounding.AwayFromZero);
            }
      
            //13.copy line to head
            head.NetTotalAmt = Math.Round(line.Sum(o => o.TotalAmt), 2, MidpointRounding.AwayFromZero);
            head.NetTotalVatAmt = Math.Round(line.Sum(o => o.VatAmt), 2, MidpointRounding.AwayFromZero);
            head.NetTotalAmtIncVat = Math.Round(line.Sum(o => o.TotalAmtIncVat), 2, MidpointRounding.AwayFromZero);
            head.QtyInvoice = line.Sum(o => o.QtyInvoice);

            if (head.RCID == "" && head.INVPendingAmt == 0)
            {
                head.INVPendingAmt = head.NetTotalAmtIncVat;
            }

            head.CountLine = line.Count();
        }


        #endregion

        #region Go to Record
        public static void GoPreviousDoc(int currID) {
            OINVHead query = new OINVHead();
            using (GAEntities db = new GAEntities()) {
                var curr = db.OINVHead.Where(o => o.ID == currID).FirstOrDefault();
                query = db.OINVHead.Where(o => o.ID < curr.ID && o.IsActive).FirstOrDefault();
                if (query != null) {
                    GetDocSetByID(query.ID, false);
                }
            }

        }

        public static void GoNextDoc(int currID) {
            OINVHead query = new OINVHead();
            using (GAEntities db = new GAEntities()) {
                var curr = db.OINVHead.Where(o => o.ID == currID).FirstOrDefault();
                query = db.OINVHead.Where(o => o.ID > curr.ID && o.IsActive).FirstOrDefault();

                if (query == null) {

                    GetDocSetByID(query.ID, false);
                }

            }

        }

        public static void GoLastDoc() {
            OINVHead query = new OINVHead();
            using (GAEntities db = new GAEntities()) {
                query = db.OINVHead.Where(o => o.IsActive).OrderByDescending(o => o.ID).FirstOrDefault();
                if (query != null) {
                    GetDocSetByID(query.ID, false);
                }
            }

        }


        #endregion

        #region Extend Fucntion

        public static List<POS_ORDERHead> ListSaleOrderHead()
        {
            
            List<POS_ORDERHead> result = new List<POS_ORDERHead>();
            var h = DocSet.Head;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var ords = ARInvoiceService.DocSet.Line.Select(o => (string)o.SOID).ToList();

            using (GAEntities db = new GAEntities())
            {
                result = db.POS_ORDERHead.Where(o =>   o.CustID == h.CustomerID
                                        && o.INVID==""
                                        && o.RComID==rcom
                                        && !ords.Contains(o.OrdID)
                                        && o.Status== "RECEIVED"

                                        && o.IsActive
                                        ).OrderByDescending(o=>o.OrdDate).ToList();
            }
            return result;
        }

        public static void ConvertOSaleOrderHead2Line(List<string> ordid)
        {

            using (GAEntities db = new GAEntities())
            {

                var oboml = db.POS_ORDERBomLine.Where(o => ordid.Contains(o.OrdID)).ToList();

                foreach (var l in oboml)
                {
                    OINVLine n = NewLine();

                    var itemfg = POSItemService.GetItem(l.FgItemID);

                    n.DocTypeID = DocSet.Head.DocTypeID;
                    n.CompanyID = DocSet.Head.CompanyID;
                    n.SOLineNum = l.LineNum;
                    n.ItemID = l.RmItemID;
                    n.ItemName = l.RmItemName;
                    n.ShipID = "";
                    n.ShipLineNum = l.LineNum;
                    n.SOID = l.OrdID;
                    n.IsStockItem = false;
                    n.WUnit = l.RmUnit;
                    n.QtyShip = l.RmQty;
                    n.QtyInvoice = l.RmQty;
                    n.QtyReturn = 0;
                    n.Unit = l.RmUnit;
                    n.Packaging = "";
                    n.Price = l.Price;
                    n.PriceIncVat = l.Price;
                    n.TotalAmt = l.RmAmt;
                    //n.VatAmt = l.VatAmt;
                    n.TotalAmtIncVat = l.RmAmt;
                    //n.VatRate = l.VatRate;
                    //n.VatTypeID = l.VatTypeID;
                    //n.BaseTotalAmt = l.BaseTotalAmt;
                    //n.OntopDiscAmt = l.OntopDiscAmt;
                    //n.OntopDiscPer = l.OntopDiscPer;
                    n.Remark1 = itemfg.Remark1;
                    n.Remark2 = itemfg.Remark2;
                    n.Status = l.OrdStatus;
                    DocSet.Line.Add(n);
                }

            }

        }

        #endregion


        #region Update  inv 2 so / inv 2 ship / inv 2 job / inv 2 picking / inv from another doc

        public static void RefreshSet(string invid) {
            UpdateInv2SO();
            //UpdateInv2BomLine();
        }
        public static I_BasicResult SetLink(string invid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var comId = LoginService.LoginInfo.CurrentCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {
                    var doc = db.OINVHead.Where(o => o.SOINVID == invid && o.CompanyID==comId).FirstOrDefault();
                    if (doc != null) {
                        doc.IsLink = true;
                        doc.LinkDate = DateTime.Now;
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.Head.SOINVID, TableID = "OINV", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.Head.CompanyID, Action = "Link OEINV" });
                    }
                }
            } catch (Exception ex) {

                result.Result = "fail";
                result.Message1 = ex.Message;
                if (ex.InnerException != null) {
                    result.Message1 = result.Message1 + " : " + ex.InnerException.Message;
                }
            }

            return result;
        }


        public static I_BasicResult UpdateInv2SO() {
            I_BasicResult result = new I_BasicResult();
            //var rcom = LoginService.LoginInfo.CurrentRootCompany;
            var h = DocSet.Head;
            //string invid = DocSet.Head.SOINVID;
            //string comid = DocSet.Head.CompanyID;
            using (GAEntities db = new GAEntities()) { //1.clear old id
                var clr_so = db.POS_ORDERHead.Where(o => o.INVID == h.SOINVID 
                                                        && o.ComID == h.CompanyID 
                                                        && o.RComID== h.RCompanyID
                                                        ).ToList();
                var clr_so_line = db.POS_ORDERLine.Where(o => o.INVID == h.SOINVID
                                                       && o.ComID == h.CompanyID
                                                       && o.RComID == h.RCompanyID
                                                       ).ToList();
                var clr_so_line_bom = db.POS_ORDERBomLine.Where(o => o.INVID == h.SOINVID
                                                    && o.ComID == h.CompanyID
                                                    && o.RComID == h.RCompanyID
                                                    ).ToList();
                foreach (var clr in clr_so) {
                    clr.INVID = "";
                }
                foreach (var clr in clr_so_line) {
                    clr.INVID = "";
                }
                foreach (var clr in clr_so_line_bom) {
                    clr.INVID = "";
                }
                //2.update invid to sohead
                var list_ids = db.OINVLine.Where(o => o.SOINVID == h.SOINVID
                                                    && o.CompanyID == h.CompanyID
                                                    && o.RCompanyID ==h.RCompanyID
                                                    && o.IsActive)
                                                    .Select(o => (string)o.SOID
                                                    ).ToList();
                var udp_so = db.POS_ORDERHead.Where(o => list_ids.Contains(o.OrdID) && o.RComID == h.RCompanyID).ToList();
                var udp_so_line = db.POS_ORDERLine.Where(o => list_ids.Contains(o.OrdID ) && o.RComID == h.RCompanyID).ToList();
                var udp_so_bom = db.POS_ORDERBomLine.Where(o => list_ids.Contains(o.OrdID) && o.RComID == h.RCompanyID).ToList();
                foreach (var udp in udp_so) {
                    udp.INVID = h.SOINVID;
                }
                foreach (var udp in udp_so_line) {
                    udp.INVID = h.SOINVID;
                }
                foreach (var udp in udp_so_bom) {
                    udp.INVID = h.SOINVID;
                }
                db.SaveChanges();
            }
            return result;
        }

        //public static I_BasicResult UpdateInv2BomLine()
        //{
        //    I_BasicResult result = new I_BasicResult();
        //    var h = DocSet.Head;
        //    string comid = DocSet.Head.CompanyID;
        //    //string invid = DocSet.Head.SOINVID;
        //    //string comid = DocSet.Head.CompanyID;
        //    using (GAEntities db = new GAEntities())
        //    {
        //        //1.get shid list from inv line
        //        var oinvs = db.OINVLine.Where(o => o.SOINVID == h.inv && o.CompanyID == comid && o.IsActive).Select(o => (string)o.SOID).ToList();

        //        //1.clear old id

        //        var clr_pk = db.POS_ORDERBomLine.Where(o => oinvs.Contains(o.OrdID) && o.ComID == comid && o.IsActive).ToList();
        //        foreach (var clr in clr_pk)
        //        {
        //            clr.INVID = "";
        //        }

        //        //2.update invid  to pk by shipid

        //        var pkhs = db.POS_ORDERBomLine.Where(o => oinvs.Contains(o.OrdID) && o.ComID == comid && o.IsActive).ToList();
        //        foreach (var l in pkhs)
        //        {
        //            l.INVID = DocSet.Head.SOINVID;
        //        }
        //        db.SaveChanges();
        //    }
        //    return result;
        //}

        #endregion


        public static I_BasicResult RefreshInvoice(string invid,string comid)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                //1.get inv head for update
                var inv_h = db.OINVHead.Where(o => o.SOINVID == invid && o.CompanyID == comid && o.RCompanyID == rcom).FirstOrDefault();
                //2.list receipt history
                var rc_lines = db.vw_ORCLine.Where(o => o.INVID == invid && o.CompanyID == comid && o.RCompanyID == rcom && o.IsActive).ToList();

                //2.3 get lasted receipt in this invoice
                var last_rc = rc_lines.OrderByDescending(o => o.PayNo).ThenByDescending(o => o.ID).FirstOrDefault();
                var last_rc_confirm = rc_lines.Where(o => o.RCStatus == "CONFIRM").OrderByDescending(o => o.RCDate).FirstOrDefault();

                List<string> pending_status = new List<string> { "OPEN", "ON-HAND", "DEPOSIT" };
                List<string> confirm_status = new List<string> { "CONFIRM" };//except REJECT status
                var not_ac_amt = rc_lines.Where(o => o.IsActive && pending_status.Contains(o.RCStatus)).Sum(o => o.PayAmt);
                var ac_amt = rc_lines.Where(o => o.IsActive && confirm_status.Contains(o.RCStatus)).Sum(o => o.PayAmt);

                //2.4 update last rc

                inv_h.RCDate = null;
                inv_h.RCLastPayNo = 0;
                inv_h.RCID = "";
                inv_h.RCStatus = "";
                inv_h.RCAmt = rc_lines.Sum(o => o.PayTotalAmt);
                inv_h.INVPendingAmt = inv_h.NetTotalAmtIncVat - inv_h.RCAmt;
                inv_h.LastStatementDate = null;
                if (last_rc != null)
                {
                    inv_h.RCDate = last_rc.RCDate;
                    inv_h.RCLastPayNo = last_rc.PayNo;
                    inv_h.RCID = last_rc.RCID;
                    inv_h.RCStatus = last_rc.RCStatus;
                }
                //2.5 update last statement date from confirm rc
                if (last_rc_confirm != null)
                {
                    inv_h.LastStatementDate = last_rc_confirm.RCDate;
                }
                if (inv_h.INVPendingAmt == 0 && inv_h.RCID != "")
                {
                    inv_h.Status = "CLOSED";
                }
                else
                {
                    inv_h.Status = "OPEN";
                }

                db.SaveChanges();

            }
            return result;
        }

    }


}