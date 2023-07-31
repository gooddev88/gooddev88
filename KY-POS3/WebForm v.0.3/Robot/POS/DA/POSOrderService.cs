
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
    public static class POSOrderService {

        public class I_ORDSet {
            public string Action { get; set; }
            public POS_ORDERHead head { get; set; }
            public List<vw_POS_ORDERLine> line { get; set; }
            public List<POS_ORDERBomLine> lineBom { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class I_ORDFiterSet {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowActive { get; set; }
        }
        public class I_ComboBinding {

            public String Desc { get; set; }
            public String Value { get; set; }
            public string Menu { get; set; }
            public List<string> ValueRelate { get; set; }

        }
        public static string PreviousPage { get { return (string)HttpContext.Current.Session["ord_previouspage"]; } set { HttpContext.Current.Session["ord_previouspage"] = value; } }

        public static string DetailPreviousPage { get { return (string)HttpContext.Current.Session["orddetail_previouspage"]; } set { HttpContext.Current.Session["orddetail_previouspage"] = value; } }
        public static string NewDocPreviousPage { get { return (string)HttpContext.Current.Session["ordnewdoc_previouspage"]; } set { HttpContext.Current.Session["ordnewdoc_previouspage"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }

        public static I_ORDSet DocSet { get { return (I_ORDSet)HttpContext.Current.Session["orddocset"]; } set { HttpContext.Current.Session["orddocset"] = value; } }
        public static List<vw_POS_ORDERHead> DocList { get { return (List<vw_POS_ORDERHead>)HttpContext.Current.Session["orddoc_list"]; } set { HttpContext.Current.Session["orddoc_list"] = value; } }
        public static I_ORDFiterSet FilterSet { get { return (I_ORDFiterSet)HttpContext.Current.Session["ordfilter_set"]; } set { HttpContext.Current.Session["ordfilter_set"] = value; } }

        #region Query Transaction
        public static void GetDocSetByID(string docid) {
            NewTransaction("ORDER");
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try {
                using (GAEntities db = new GAEntities()) {
                    DocSet.head = db.POS_ORDERHead.Where(o => o.OrdID == docid && o.RComID == rcom).FirstOrDefault();
                    DocSet.line = db.vw_POS_ORDERLine.Where(o => o.OrdID == docid && o.RComID == rcom).OrderByDescending(o => o.VendorID).ThenBy(o => o.ItemID).ToList();
                    DocSet.lineBom = db.POS_ORDERBomLine.Where(o => o.OrdID == docid && o.RComID == rcom).OrderByDescending(o => o.VendorID).ThenBy(o => o.FgItemID).ToList();
                    DocSet.Log = TransactionInfoService.ListLogByDocID(docid);
                }
            } catch (Exception ex) {
                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void ListDoc() {
            var f = FilterSet;
            //  string currCom = LoginService.LoginInfo.CurrentCompany;
            var uic = LoginService.LoginInfo.UserInCompany;
            //string doctype = FilterSet.DocType.ToLower();
            //string filterby = FilterSet.SearchBy.ToLower();
            //string search = FilterSet.SearchText;
            //bool isactive = FilterSet.ShowActive;
            //DateTime begin = FilterSet.DateFrom;
            //DateTime end = FilterSet.DateTo;
            using (GAEntities db = new GAEntities()) {

                DocList = db.vw_POS_ORDERHead.Where(o =>
                                                        (o.ComID.Contains(f.SearchText)
                                                        || o.OrdID.Contains(f.SearchText)
                                                        || o.RComID.Contains(f.SearchText)
                                                        || o.Description.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                        && o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo
                                                        //&& (o.ComID == currCom)
                                                        && uic.Contains(o.ComID)
                                                        && (o.IsActive == f.ShowActive)
                                                        && (o.Status == f.Status || f.Status == "")
                                                        ).OrderByDescending(o => o.CreatedDate).ToList();
                return;
            }
        }


        public static List<vw_POS_ORDER_RM> ListDoc_POSORDER_RM(RPT_POSORDER_RMList.FilterSet f) {
            List<vw_POS_ORDER_RM> result = new List<vw_POS_ORDER_RM>();
            using (GAEntities db = new GAEntities()) {

                result = db.vw_POS_ORDER_RM.Where(o =>
                                                        (o.VendorID.Contains(f.Search)
                                                        || o.RmItemID.Contains(f.Search)
                                                        || o.RmItemName.Contains(f.Search)
                                                        || f.Search == "")
                                                        ).OrderByDescending(o => o.RmItemID).ToList();
            }
            return result;
        }

        public static List<POS_ORDERHead> ListDocOrderMobile() {
            var f = FilterSet;
            var uic = LoginService.LoginInfo.UserInCompany;
            List<POS_ORDERHead> result = new List<POS_ORDERHead>();
            using (GAEntities db = new GAEntities()) {
                result = db.POS_ORDERHead.Where(o => (o.ComID.Contains(f.SearchText)
                                                        || o.OrdID.Contains(f.SearchText)
                                                        || o.RComID.Contains(f.SearchText)
                                                        || o.Description.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                        && uic.Contains(o.ComID)
                                                        && (o.Status == f.Status || f.Status == "")
                                                        && o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo
                                                        && o.IsActive
                                        ).OrderByDescending(o => o.CreatedDate).ToList();
            }
            return result;
        }


        #endregion

        #region  Method GET

        public static POS_ORDERHead GetOrderHeadByOrdID(string ordid) {
            POS_ORDERHead result = new POS_ORDERHead();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_POS_ORDERLine> ListViewOrderLineByOrdID(string ordid) {
            List<vw_POS_ORDERLine> result = new List<vw_POS_ORDERLine>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_ORDERLine.Where(o => o.OrdID == ordid && o.IsActive && o.RComID == rcom).OrderBy(o => o.Name).ToList();
            }
            return result;
        }

        public static List<vw_POS_ORDERLine> ListViewOrderLineByComId(string ComId) {
            List<vw_POS_ORDERLine> result = new List<vw_POS_ORDERLine>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_ORDERLine.Where(o => o.ComID == ComId && o.IsActive && o.RComID == rcom).OrderBy(o => o.Name).ToList();
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

        public static void Save(string action) {
            var h = DocSet.head;
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                CalDocSet();
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {

                        checkDupID();
                        if (DocSet.OutputAction.Result == "fail") {
                            return;
                        }
                        var ll = LineConvert2Table(DocSet.line);
                        db.POS_ORDERHead.Add(DocSet.head);
                        db.POS_ORDERLine.AddRange(ll);
                        db.POS_ORDERBomLine.AddRange(DocSet.lineBom);
                        db.SaveChanges();
                        //h.OrdID = IDGeneratorServiceV2.GenNewID("ORD", h.ComID, false, h.OrdDate, "th")[1];
                        IDRuunerService.GetNewID("ORD", h.ComID, true, "th", h.OrdDate);
                        CalStock();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.OrdID, TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.ComID, Action = "INSERT NEW ORDER" });
                    } else {

                        var uh = db.POS_ORDERHead.Where(o => o.OrdID == DocSet.head.OrdID).FirstOrDefault();
                        uh.OrdQty = h.OrdQty;
                        uh.OrdAmt = h.OrdAmt;
                        uh.ShipQty = h.ShipQty;
                        uh.ShipdAmt = h.ShipdAmt;
                        uh.GrQty = h.GrQty;
                        uh.GrAmt = h.GrAmt;
                        uh.Description = h.Description;
                        uh.Remark1 = h.Remark1;
                        uh.Status = h.Status;
                        uh.FrLocID = h.FrLocID;
                        uh.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        uh.ModifiedDate = DateTime.Now;
                        var ll = LineConvert2Table(DocSet.line);
                        db.POS_ORDERLine.RemoveRange(db.POS_ORDERLine.Where(o => o.OrdID == h.OrdID && o.RComID == h.RComID));
                        db.POS_ORDERLine.AddRange(ll);

                        db.POS_ORDERBomLine.RemoveRange(db.POS_ORDERBomLine.Where(o => o.OrdID == h.OrdID && o.RComID == h.RComID));
                        db.POS_ORDERBomLine.AddRange(DocSet.lineBom);

                        db.SaveChanges();
                        CalStock();
                        var rpo = Convert2PO(DocSet);//ให้สร้าง PO ซื้อของในจังหวะกด accept
                        if (rpo.Result == "fail") {
                            DocSet.OutputAction.Result = "fail";
                            DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " : " + rpo.Message1;
                        }
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.OrdID, TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.ComID, Action = "UPDATE ORDER" });
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

        public static I_BasicResult SetStatusAccepted(string ordid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "ACCEPTED";
                    h.AcceptedBy = LoginService.LoginInfo.CurrentUser;
                    h.AcceptedDate = DateTime.Now;

                    db.SaveChanges();
                    CalBom(h.OrdID);
                    POSOrderService.GetDocSetByID(h.OrdID);
                    var checkpo = db.POS_POHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    if (checkpo == null) {
                        var rpo = Convert2PO(DocSet);//ให้สร้าง PO ซื้อของในจังหวะกด accept
                        if (rpo.Result == "fail") {
                            DocSet.OutputAction.Result = "fail";
                            DocSet.OutputAction.Message1 = DocSet.OutputAction.Message1 + " : " + rpo.Message1;
                        }
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
        public static I_BasicResult SetStatusShipping(string ordid) {
            //OPEN / ACCEPTED / SHIPPING / RECEIVED / CANCEL
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "SHIPPING";
                    h.ShipBy = LoginService.LoginInfo.CurrentUser;
                    h.ShipDate = DateTime.Now;

                    db.SaveChanges();
                    //CalBom(h.OrdID);
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
        public static I_BasicResult SetStatusReceive(string ordid) {
            //OPEN / ACCEPTED / SHIPPING / RECEIVED / CANCEL
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "RECEIVED";
                    h.GRBy = LoginService.LoginInfo.CurrentUser;
                    h.GRDate = DateTime.Now;

                    db.SaveChanges();
                    //CalBom(h.OrdID);
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
        public static void CalDocSet() {
            var h = DocSet.head;
            foreach (var l in DocSet.line) {
                l.OrdID = h.OrdID;
                l.ComID = h.ComID;
                l.FrLocID = h.FrLocID;
                l.CustID = h.ComID;
                l.RComID = h.RComID;
                l.OrdDate = h.OrdDate;
                l.DocType = h.DocType;
                l.OrdAmt = l.Price * l.OrdQty;
                l.ShipdAmt = l.Price * l.ShipQty;
                l.GrAmt = l.Price * l.GrQty;
            }
            foreach (var l in DocSet.lineBom) {
                l.OrdID = h.OrdID;
                l.ComID = h.ComID;
                l.CustID = h.ComID;
                l.RComID = h.RComID;
                l.FrLocID = h.FrLocID;
                l.OrdDate = h.OrdDate;
                l.OrdStatus = h.Status;
                l.DocType = h.DocType;
                l.FgAmt = l.Price * l.FgQty;
                l.RmAmt = l.Price * l.RmQty;

            }
            h.OrdQty = DocSet.line.Sum(o => o.OrdQty);
            h.OrdAmt = DocSet.line.Sum(o => o.OrdAmt);
            h.ShipQty = DocSet.line.Sum(o => o.ShipQty);
            h.ShipdAmt = DocSet.line.Sum(o => o.ShipdAmt);
            h.GrQty = DocSet.line.Sum(o => o.GrQty);
            h.GrAmt = DocSet.line.Sum(o => o.GrAmt);

        }


        public static I_BasicResult NewLineByOrdLine(vw_POS_ORDERLine ordline, string item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                var oline = DocSet.line.Where(o => o.ItemID == item).FirstOrDefault();
                if (oline == null) {//add new ordline 
                    DocSet.line.Add(ordline);
                } else {
                    //exist ordline
                    oline.OrdQty = ordline.OrdQty;
                }
            } catch (Exception ex) {
                result.Result = "fail";
                result.Message1 = ex.Message;
                if (ex.InnerException != null) {
                    result.Message1 = result.Message1 + " " + ex.InnerException.Message;
                }
            }
            return result;
        }

        public static void checkDupID() {
            try {
                DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = DocSet.head;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_ORDERHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.OrdID == h.OrdID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GetNewID("ORD", h.ComID, true, "th", h.OrdDate);


                        h.OrdID = IDRuunerService.GetNewID("ORD", h.ComID, true, "th", h.OrdDate)[1];
                        get_id = db.POS_ORDERHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.OrdID == h.OrdID).FirstOrDefault();
                    }
                }
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
                    var head = db.POS_ORDERHead.Where(o => o.OrdID == docid && o.RComID == rcom).FirstOrDefault();
                    var line = db.POS_ORDERLine.Where(o => o.OrdID == docid && o.RComID == rcom).ToList();
                    head.Status = "CANCEL";
                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    CalBom(head.OrdID);
                    CalStock();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = DocSet.head.OrdID, TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = DocSet.head.OrdID, Action = "DELETE DATA ORDER" });
                }
            } catch (Exception ex) {

                DocSet.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void DeleteBOMLine(int linenum) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                DocSet.line.RemoveAll(o => o.LineNum == linenum);
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

        #region New transaction

        public static void NewFilterSet() {
            FilterSet = new I_ORDFiterSet();
            FilterSet.DateFrom = DateTime.Now.Date;
            FilterSet.DateTo = DateTime.Now.Date;
            FilterSet.DocType = "";
            FilterSet.Status = "";
            FilterSet.SearchBy = "DOCDATE";
            FilterSet.SearchText = "";
            FilterSet.ShowActive = true;
        }

        public static void NewTransaction(string doctype) {
            DocSet = new I_ORDSet();
            DocSet.Action = "";
            DocSet.head = NewHead(doctype);
            DocSet.line = new List<vw_POS_ORDERLine>();
            DocSet.lineBom = new List<POS_ORDERBomLine>();
            DocSet.NeedRunNextID = false;
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            IsNewDoc = false;
        }

        public static POS_ORDERHead NewHead(string doctype) {
            POS_ORDERHead n = new POS_ORDERHead();

            n.ComID = "";
            n.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.OrdID = "";
            n.DocType = doctype;
            n.OrdDate = DateTime.Now.Date;
            n.INVID = "";
            n.FrLocID = "INTER";
            n.CustID = "";
            n.CustName = "";
            n.Description = "";
            n.Remark1 = "";
            n.AcceptedMemo = "";
            n.Remark1 = "";
            n.OrdQty = 0;
            n.ShipQty = 0;
            n.GrQty = 0;
            n.OrdAmt = 0;
            n.ShipdAmt = 0;
            n.GRBy = "";
            n.GrAmt = 0;
            n.GRDate = null;
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

        public static vw_POS_ORDERLine NewLine() {
            vw_POS_ORDERLine n = new vw_POS_ORDERLine();

            n.ComID = "";
            n.RComID = "";
            n.OrdID = "";
            n.FrLocID = "INTER";
            n.LineNum = GenLineNum();
            n.OrdDate = DateTime.Now.Date;
            n.ItemID = "";
            n.Name = "";
            n.Price = 0;
            n.OrdQty = 0;
            n.ShipQty = 0;
            n.GrQty = 0;
            n.OrdAmt = 0;
            n.ShipdAmt = 0;
            n.BalQty = 0;
            n.BalQtyOrd = 0;
            n.OnOrdQty = 0;
            n.GrAmt = 0;
            n.GrUnit = "";
            n.Unit = "";
            n.INVID = "";
            n.VendorID = "";
            n.IsActive = true;
            return n;
        }
        public static POS_ORDERBomLine NewBomLine(vw_POS_ORDERLine i) {
            POS_ORDERBomLine n = new POS_ORDERBomLine();

            n.RComID = i.RComID;
            n.ComID = i.ComID;
            n.OrdID = i.OrdID;
            n.LineNum = GenLineLineNum();
            n.LineLineNum = i.LineNum;
            n.VendorID = i.VendorID;
            n.FrLocID = "INTER";
            n.CustID = i.CustID;
            n.OrdDate = i.OrdDate;
            n.DocType = i.DocType;
            n.FgItemID = i.ItemID;
            n.FgName = i.Name;
            n.RmItemID = i.ItemID;
            n.RmItemName = i.Name;
            n.Price = i.Price;
            n.FgQty = i.OrdQty;
            n.RmQty = i.OrdQty;
            n.FgAmt = i.OrdAmt;
            n.RmAmt = i.OrdQty;
            n.FgUnit = i.Unit;
            n.RmUnit = i.Unit;
            n.BomID = "";
            n.OrdStatus = "";
            n.INVID = "";
            n.IsActive = true;

            return n;
        }
        public static List<POS_ORDERLine> LineConvert2Table(List<vw_POS_ORDERLine> input) {
            List<POS_ORDERLine> t = new List<POS_ORDERLine>();
            foreach (var i in input) {
                POS_ORDERLine n = new POS_ORDERLine();
                n.ID = i.ID;
                n.RComID = i.RComID;
                n.ComID = i.ComID;
                n.OrdID = i.OrdID;
                n.CustID = i.CustID;
                n.LineNum = i.LineNum;
                n.OrdDate = i.OrdDate;
                n.DocType = i.DocType;
                n.FrLocID = i.FrLocID;
                n.ItemID = i.ItemID;
                n.Name = i.Name;
                n.Price = i.Price;
                n.OrdQty = i.OrdQty;
                n.ShipQty = i.ShipQty;
                n.GrQty = i.GrQty;
                n.BalQty = i.BalQtyOrd;
                n.OrdAmt = i.OrdAmt;
                n.ShipdAmt = i.ShipdAmt;
                n.GrAmt = i.GrAmt;
                n.Unit = i.Unit;
                n.GrUnit = i.GrUnit;
                n.INVID = i.INVID;
                n.IsActive = i.IsActive;
                t.Add(n);
            }
            return t;

        }
        public static int GenLineLineNum() {
            int result = 10;
            try {
                var max_linenum = DocSet.lineBom.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
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
                    db.SP_CalStkGrMove(h.OrdID, h.DocType, h.RComID, h.ComID);
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
        public static void CalBom(string ordId) {
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                GetDocSetByID(ordId);
                var h = DocSet.head;
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities()) {
                    DocSet.lineBom.Clear();
                    var fgItem = DocSet.line.Select(o => o.ItemID).Distinct().ToList();
                    var bom = db.POS_BOMLine.Where(o => o.IsActive && fgItem.Contains(o.ItemIDFG) && o.IsDefault && o.RComID == rcom && o.UserForModule == "KITCHEN").ToList();
                    var itemsId = bom.Select(o => o.ItemIDRM).Distinct().ToList();
                    var rmItem = db.ItemInfo.Where(o => o.RCompanyID == rcom && itemsId.Contains(o.ItemID)).ToList();

                    foreach (var l in DocSet.line) {
                        var bb = bom.Where(o => o.ItemIDFG == l.ItemID).ToList();
                        if (bb.Count > 0) {
                            foreach (var b in bb) {
                                var rm = rmItem.Where(o => o.ItemID == b.ItemIDRM).FirstOrDefault();
                                var r1 = NewBomLine(l);
                                r1.RmItemID = b.ItemIDRM;
                                r1.RmItemName = rm.Name1;
                                r1.BomID = b.BomID;
                                r1.RmUnit = rm.UnitID;
                                switch (h.Status) {
                                    case "ACCEPTED":
                                        r1.RmQty = (l.OrdQty * b.RmQty) / b.FgQty;
                                        //r1.FgQty = b.FgQty;
                                        break;
                                    case "SHIPPING":
                                        //   r1.RmQty = (l.ShipQty * b.RmQty) / b.FgQty;
                                        r1.FgQty = l.ShipQty;
                                        break;
                                    case "RECEIVED":
                                        //   r1.RmQty = (l.GrQty * b.RmQty) / b.FgQty;
                                        r1.FgQty = l.GrQty;
                                        break;
                                    default:
                                        break;
                                }

                                r1.Price = 0;
                                r1.RmAmt = 0;

                                if (rm != null) {

                                    r1.Price = rm.Price;
                                    r1.RmAmt = r1.RmQty * r1.Price;
                                }
                                DocSet.lineBom.Add(r1);
                            }

                            CalDocSet();

                        } else {
                            var r2 = NewBomLine(l);
                            DocSet.lineBom.Add(r2);
                        }
                    }
                    Save("update");
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


        #region Convert to po
        public static I_BasicResult Convert2PO(I_ORDSet doc) {
            var r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<string> work_status = new List<string> { "ACCEPTED" };
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                if (!work_status.Contains(doc.head.Status)) {
                    return r;
                }
                bool hasPO = false;
                using (GAEntities db = new GAEntities()) {
                    var po = db.POS_POHead.Where(o => o.RComID == rcom && o.OrdID == doc.head.OrdID && o.IsActive).FirstOrDefault();
                    if (po != null) {
                        hasPO = true;
                    }
          
                if (hasPO) {
                    return r;
                }
                POS_POService.NewTransaction("PO");
                var pdoc = POS_POService.DocSet;

                pdoc.head.ComID = doc.head.FrLocID;
                pdoc.head.OrdID = doc.head.OrdID;
                pdoc.head.PODate = doc.head.OrdDate;
                pdoc.head.DocType = "PO";
                pdoc.head.ToLocID = doc.head.FrLocID;

                foreach (var l in doc.line) {
                    //create po fg line
                    var pol = POS_POService.NewFGLine();
                    pol.LineNum = l.LineNum;
                    pol.ToLocID = l.ComID;
                    pol.VendorID = l.VendorID;
                    pol.FgItemID = l.ItemID;
                    pol.OrdID = l.OrdID;
                    pol.FgName = l.Name;
                    pol.Price = l.Price;
                    pol.OrdQty = l.OrdQty;
                    pol.FgQty = l.OrdQty;
                    pol.FgAmt = l.OrdAmt;
                    pol.FgUnit = l.Unit;
                    pol.IsActive = l.IsActive;
                    var r1 = POS_POService.NewFGLineByItem(pol, pol.FgItemID);
                    if (r1.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " : " + r1.Message1;
                    }
                }


                foreach (var l in doc.lineBom) { 
                    var pol = POS_POService.NewLine();
                    var item = POSItemService.GetItem(l.RmItemID); 
                    var ordLine = doc.line.Where(o => o.LineNum == l.LineLineNum).FirstOrDefault();
                    if (ordLine != null) {
                        pol.FGRefLineNum = ordLine.LineNum;
                        pol.FGItemID = ordLine.ItemID;
                        pol.FGName = ordLine.Name;
                        pol.FGOrdQty = ordLine.OrdQty;
                        pol.FGQty = ordLine.OrdQty;
                        pol.FGUnit = ordLine.Unit;
                    }
                    pol.ToLocID = l.ComID;
                    pol.ItemID = l.RmItemID;
                    pol.Name = l.RmItemName;
                    pol.Unit = l.RmUnit;
                    pol.Price = Convert.ToDecimal(item.Cost);
                    pol.Qty = l.RmQty;
                    pol.OrdID = l.OrdID;
                    pol.BalQty = 0;
                    pol.OnOrdQty = 0;
                    pol.FGItemID = l.FgItemID;
                    pol.FGQty = l.FgQty;
                    pol.FGUnit = l.FgUnit;
                    var itemInfo = db.ItemInfo.Where(o => o.RCompanyID == pdoc.head.RComID && o.ItemID == l.RmItemID).FirstOrDefault();
                    if (itemInfo != null) {
                        pol.VendorID = itemInfo.VendorID;
                    }
                    var r1 = POS_POService.NewLineByItem(pol, pol.ItemID);
                    if (r1.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " : " + r1.Message1;
                    }
                }
              
                pdoc.head.POID = IDRuunerService.GetNewID("PO", pdoc.head.ComID, false, "th", pdoc.head.PODate)[1];
                POS_POService.Save("insert");
                if (POS_POService.DocSet.OutputAction.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " : " + POS_POService.DocSet.OutputAction.Message1;
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