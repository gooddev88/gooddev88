
using Blazored.SessionStorage;
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
using static Robot.POS.DA.POSOrderService;

namespace Robot.POS.DA {
    public   class POSOrderService {
        public static string sessionActiveId = "activecomid";
        public static string sessionDocType = "doctype";
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
            public String RCom { get; set; }
            public String Com { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchText { get; set; }
            public String Search { get; set; }
            public String Status { get; set; }

            public bool ShowActive { get; set; }

        }
        public class I_ComboBinding {

            public String Desc { get; set; }
            public String Value { get; set; }
            public string Menu { get; set; }
            public List<string> ValueRelate { get; set; }

        }

        

        

        public string PreviousPageUrl = "";
        public string PreviousSubPageUrl = "";

        public POSOrderService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;

        }

        ISessionStorageService sessionStorage;

        public I_ORDSet DocSet { get; set; }
   
   
        #region Query Transaction
        public static   I_ORDSet GetDocSetByID(string docid,string rcom) {
        
            I_ORDSet doc = new I_ORDSet();
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.head = db.POS_ORDERHead.Where(o => o.OrdID == docid && o.RComID == rcom).FirstOrDefault();
                     
                    doc.line = db.vw_POS_ORDERLine.Where(o => o.OrdID == docid && o.RComID == rcom).OrderByDescending(o => o.VendorID).ThenBy(o => o.ItemID).ToList();
                    doc.lineBom = db.POS_ORDERBomLine.Where(o => o.OrdID == docid && o.RComID == rcom).OrderByDescending(o => o.VendorID).ThenBy(o => o.FgItemID).ToList();
                    doc.Log =  db.TransactionLog.Where(o => (o.TransactionID == docid && o.RCompanyID == rcom  )).ToList();
                }
            } catch (Exception ex) {
                doc.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return doc;
        }

        public static List<vw_POS_ORDERHead> ListDoc(LogInService login, I_ORDFiterSet f) {
            List<vw_POS_ORDERHead> result = new List<vw_POS_ORDERHead>();
            var uic = login.LoginInfo.UserInCompany;

            using (GAEntities db = new GAEntities()) {

                result = db.vw_POS_ORDERHead.Where(o =>
                                                        (o.ComID.Contains(f.SearchText)
                                                        || o.OrdID.Contains(f.SearchText)
                                                        || o.RComID.Contains(f.SearchText)
                                                        || o.Description.Contains(f.SearchText)
                                                        || f.SearchText == "")
                                                        && o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo
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
        


        public static List<vw_POS_ORDER_RM> ListDoc_POSORDER_RM(I_ORDFiterSet f) {
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

        public static List<POS_ORDERHead> ListDocOrderMobile(LogInService login, I_ORDFiterSet f) {
        List<POS_ORDERHead> result = new List<POS_ORDERHead>();
        var uic = login.LoginInfo.UserInCompany;
            
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

        public static POS_ORDERHead GetOrderHeadByOrdID(string ordid,string rcom) {
            POS_ORDERHead result = new POS_ORDERHead(); 
            using (GAEntities db = new GAEntities()) {
                result = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_POS_ORDERLine> ListViewOrderLineByOrdID(string ordid,string rcom) {
            List<vw_POS_ORDERLine> result = new List<vw_POS_ORDERLine>(); 
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_ORDERLine.Where(o => o.OrdID == ordid && o.IsActive && o.RComID == rcom).OrderBy(o => o.Name).ToList();
            }
            return result;
        }

        public static List<vw_POS_ORDERLine> ListViewOrderLineByComId(string ComId,string rcom) {
            List<vw_POS_ORDERLine> result = new List<vw_POS_ORDERLine>();
    
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

        public static I_BasicResult Save(string action, I_ORDSet doc, string user) {
            var r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.head;
            var rcom = doc.head.RComID;

            var createby = user;
            try {
                doc= CalDocSet(doc);
                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {

                        doc= checkDupID( doc);
                        if (doc.OutputAction.Result == "fail") {
                            return doc.OutputAction;
                        }
                        var ll = LineConvert2Table(doc.line);
                        db.POS_ORDERHead.Add(doc.head);
                        db.POS_ORDERLine.AddRange(ll);
                        db.POS_ORDERBomLine.AddRange(doc.lineBom);
                        db.SaveChanges(); 
                        IDRuunerService.GetNewIDV2("ORD",h.RComID, h.ComID, h.OrdDate, true, "th");
                    var r_stk=   CalStock(h.OrdID,h.DocType,h.RComID,h.ComID);
                        if (r_stk.Result=="fail") {
                            r.Result = "fail";
                            r.Message1 = r.Message1+" "+ r_stk.Message1;
                        }
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.head.OrdID, TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "INSERT NEW ORDER" }, rcom, doc.head.ComID, createby);

                    } else {

                        var uh = db.POS_ORDERHead.Where(o => o.OrdID == doc.head.OrdID).FirstOrDefault();
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
                        //uh.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        //uh.ModifiedDate = DateTime.Now;
                        var ll = LineConvert2Table(doc.line);
                        db.POS_ORDERLine.RemoveRange(db.POS_ORDERLine.Where(o => o.OrdID == h.OrdID && o.RComID == h.RComID));
                        db.POS_ORDERLine.AddRange(ll);

                        db.POS_ORDERBomLine.RemoveRange(db.POS_ORDERBomLine.Where(o => o.OrdID == h.OrdID && o.RComID == h.RComID));
                        db.POS_ORDERBomLine.AddRange(doc.lineBom);

                        db.SaveChanges();
                        var r_stk = CalStock(h.OrdID, h.DocType, h.RComID, h.ComID);
                        if (r_stk.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = r.Message1 + " " + r_stk.Message1;
                        }
                        var rpo = Convert2PO(doc,rcom,user);//ให้สร้าง PO ซื้อของในจังหวะกด accept
                        if (rpo.Result == "fail") {
                            r.Result = "fail";
                           r.Message1 = r.Message1 + " : " + rpo.Message1;
                        }
                        //TransactionInfoService.SaveLog(new TransactionLog { TransactionID = doc.head.OrdID, TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "UPDATE ORDER" });
                        TransactionService.SaveLog(new TransactionLog { TransactionID = doc.head.OrdID, TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.head.ComID, Action = "UPDATE ORDER" }, rcom, doc.head.ComID, createby);
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

        public  I_BasicResult SetStatusAccepted(string doc_id,string rcom,string user) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_ORDERHead.Where(o => o.OrdID == doc_id && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "ACCEPTED";
                    h.AcceptedBy = user;
                    h.AcceptedDate = DateTime.Now;

                    db.SaveChanges();
                    var r_bom=CalBom(doc_id, rcom,user);
                    if (r_bom.Result=="fail") {
                        result.Result = "fail";
                        result.Message1 = result.Message1 + " " + r_bom.Message1;
                    }
                var doc=    GetDocSetByID(doc_id, rcom);
                    var checkpo = db.POS_POHead.Where(o => o.OrdID == doc_id && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    if (checkpo == null) {
                        var rpo = Convert2PO(doc ,rcom,user);//ให้สร้าง PO ซื้อของในจังหวะกด accept
                        if (rpo.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = doc.OutputAction.Message1 + " : " + rpo.Message1;
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
        public static I_BasicResult SetStatusShipping(I_ORDSet doc,string ordid,string rcom, string user) {
            //OPEN / ACCEPTED / SHIPPING / RECEIVED / CANCEL
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "SHIPPING";
                    h.ShipBy = user;
                    h.ShipDate = DateTime.Now;

                    db.SaveChanges();
                    //CalBom(h.OrdID);
                    var r_stk = CalStock(h.OrdID, h.DocType, h.RComID, h.ComID);
                    if (r_stk.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " " + r_stk.Message1;
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
        public static I_BasicResult SetStatusReceive(I_ORDSet doc ,string ordid, string rcom, string user) {
            //OPEN / ACCEPTED / SHIPPING / RECEIVED / CANCEL
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
               
                using (GAEntities db = new GAEntities()) {

                    var h = db.POS_ORDERHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.IsActive).FirstOrDefault();
                    h.Status = "RECEIVED";
                    h.GRBy = user;
                    h.GRDate = DateTime.Now;

                    db.SaveChanges();
                    //CalBom(h.OrdID);
                    var r_stk = CalStock(h.OrdID, h.DocType, h.RComID, h.ComID);
                    if (r_stk.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " " + r_stk.Message1;
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
        public static I_ORDSet CalDocSet(I_ORDSet doc) {
            var h = doc.head;
            var line = doc.line;


            foreach (var l in doc.line) {
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
            foreach (var l in doc.lineBom) {
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
            h.OrdQty = doc.line.Sum(o => o.OrdQty);
            h.OrdAmt = doc.line.Sum(o => o.OrdAmt);
            h.ShipQty = doc.line.Sum(o => o.ShipQty);
            h.ShipdAmt = doc.line.Sum(o => o.ShipdAmt);
            h.GrQty = doc.line.Sum(o => o.GrQty);
            h.GrAmt = doc.line.Sum(o => o.GrAmt);
            return doc;
        }


        public static I_ORDSet NewLineByOrdLine(I_ORDSet doc, vw_POS_ORDERLine ordline) {
          
            try {
                var oline = doc.line.Where(o => o.ItemID == ordline.ItemID).FirstOrDefault();
                if (oline == null) {//add new ordline 
                    doc.line.Add(ordline);
                } else {
                    //exist ordline
                    oline.OrdQty = ordline.OrdQty;
                }
            } catch (Exception ex) {
                
                }
            
            return doc;
        }

        public static I_ORDSet checkDupID(I_ORDSet doc) {
            try {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = doc.head;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_ORDERHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.OrdID == h.OrdID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            doc.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GetNewIDV2("ORD",h.RComID, h.ComID, h.OrdDate, true, "th");


                        h.OrdID = IDRuunerService.GetNewIDV2("ORD",h.RComID, h.ComID, h.OrdDate, true, "th")[1];
                        get_id = db.POS_ORDERHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.OrdID == h.OrdID).FirstOrDefault();
                    }
                }
            } catch (Exception ex) {
            }
            return doc;
        }



        #endregion

        #region Delete

        public static I_BasicResult DeleteDoc(  string docid ,string rcom ,string user) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var createby = user;
            try {
                
                using (GAEntities db = new GAEntities()) {
                    var head = db.POS_ORDERHead.Where(o => o.OrdID == docid && o.RComID == rcom).FirstOrDefault();
                    var line = db.POS_ORDERLine.Where(o => o.OrdID == docid && o.RComID == rcom).ToList();
                    head.Status = "CANCEL";
                    head.ModifiedBy = rcom;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    var r_bom = CalBom(head.OrdID, rcom,user);
                    if (r_bom.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " " + r_bom.Message1;
                    }
                    var r_stk = CalStock(head.OrdID, head.DocType, head.RComID, head.ComID);
                    if (r_stk.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " " + r_stk.Message1;
                    }
                    TransactionService.SaveLog(new TransactionLog { TransactionID = head.OrdID, TableID = "ORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = head.ComID, Action = "DELETE DATA ORDER" }, rcom, head.ComID, createby);

                    
                }
            } catch (Exception ex) {

               r = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return r;
        }

        public static void DeleteBOMLine(I_ORDSet doc, int linenum) {
            doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                doc.line.RemoveAll(o => o.LineNum == linenum);
            } catch (Exception ex) {
                doc.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    doc.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    doc.OutputAction.Message1 = ex.Message;
                }
            }
        }

        #endregion

        #region New transaction

        public static I_ORDFiterSet NewFilterSet() {
            I_ORDFiterSet n = new I_ORDFiterSet();
            n = new I_ORDFiterSet();
            n.RCom = "";
            n.Com = "";
            n.DateFrom = DateTime.Now.Date;
            n.DateTo = DateTime.Now.Date;
        n.DocType = "";
        n.Status = "";
            n.Search = "";
            n.SearchText = "";
            n.ShowActive = true;

            return n;
        }

        public static I_ORDSet NewTransaction(string doctype,string rcom) {
            I_ORDSet n = new I_ORDSet(); 
            n.Action = "";
            n.head = NewHead(doctype,rcom);
            n.line = new List<vw_POS_ORDERLine>();
            
            n.lineBom = new List<POS_ORDERBomLine>();
            n.NeedRunNextID = false;
            n.Log = new List<TransactionLog>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            //IsNewDoc = false;
            return n;
        }

        public static POS_ORDERHead NewHead(string doctype,string rcom) {
            POS_ORDERHead n = new POS_ORDERHead();

            n.ComID = "";
            n.RComID = rcom;
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
            n.CreatedBy = rcom;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static vw_POS_ORDERLine NewLine(I_ORDSet doc) {
            vw_POS_ORDERLine n = new vw_POS_ORDERLine();

            n.ComID = "";
            n.RComID = "";
            n.OrdID = "";
            n.FrLocID = "INTER";
            n.LineNum = GenLineNum(doc);
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
        public static POS_ORDERBomLine NewBomLine(vw_POS_ORDERLine i , I_ORDSet doc) {
            POS_ORDERBomLine n = new POS_ORDERBomLine();

            n.RComID = i.RComID;
            n.ComID = i.ComID;
            n.OrdID = i.OrdID;
            n.LineNum = GenLineLineNum(doc);
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
        public static int GenLineLineNum(I_ORDSet doc) {
            int result = 10;
            try {
                var max_linenum = doc.lineBom.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenLineNum(I_ORDSet doc) {
            int result = 10;
            try {
                var max_linenum = doc.line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        //public static void CalStock(I_ORDSet doc) {
        //    doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        var h = doc.head;
        //        using (GAEntities db = new GAEntities()) {
        //            db.SP_CalStkGrMove(h.OrdID, h.DocType, h.RComID, h.ComID);
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
        public static I_BasicResult CalStock(string docid,string doctype,string rcom,string com) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try { 
                    using (var connection = new SqlConnection(Globals.GAEntitiesConn)) {
                        var strSQL = $"exec [SP_CalStkGrMove] {docid}, {doctype}, {rcom}, {com}";
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
        public static  I_BasicResult CalBom(string  doc_id ,string rcom,string user) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                
             var doc=   GetDocSetByID(doc_id, rcom);
                var h = doc.head;

                using (GAEntities db = new GAEntities()) {
                    doc.lineBom.Clear();
                    var fgItem = doc.line.Select(o => o.ItemID).Distinct().ToList();
                    var bom = db.POS_BOMLine.Where(o => o.IsActive && fgItem.Contains(o.ItemIDFG) && o.IsDefault && o.RComID == rcom && o.UserForModule == "KITCHEN").ToList();
                    var itemsId = bom.Select(o => o.ItemIDRM).Distinct().ToList();
                    var rmItem = db.ItemInfo.Where(o => o.RCompanyID == rcom && itemsId.Contains(o.ItemID)).ToList();

                    foreach (var l in doc.line) {
                        var bb = bom.Where(o => o.ItemIDFG == l.ItemID).ToList();
                        if (bb.Count > 0) {
                            foreach (var b in bb) {
                                var rm = rmItem.Where(o => o.ItemID == b.ItemIDRM).FirstOrDefault();
                                var r1 = NewBomLine(l,doc);
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
                                doc.lineBom.Add(r1);
                            }

                            CalDocSet(doc);

                        } else {
                            var r2 = NewBomLine(l,doc);
                            doc.lineBom.Add(r2);
                        }
                    }
                    Save("update",doc,user);
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
        public static I_BasicResult Convert2PO(I_ORDSet doc,string rcom,string user) {
            var r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<string> work_status = new List<string> { "ACCEPTED" };
                
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
              var pdoc=      POS_POService.NewTransaction("PO",rcom, user);
            

                    pdoc.head.ComID = doc.head.FrLocID;
                    pdoc.head.OrdID = doc.head.OrdID;
                    pdoc.head.PODate = doc.head.OrdDate;
                    pdoc.head.DocType = "PO";
                    pdoc.head.ToLocID = doc.head.FrLocID;

                    foreach (var l in doc.line) {
                        //create po fg line
                        var pol = POS_POService.NewFGLine(pdoc);
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
                        pdoc = POS_POService.NewFGLineByItem(pdoc,pol, pol.FgItemID);
                      
                    }


                    foreach (var l in doc.lineBom) {
                        var pol = POS_POService.NewLine(pdoc,user);
                        var item = POSItemService.GetItem(l.RmItemID,rcom);
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
                          pdoc = POS_POService.NewLineByItem(pdoc,pol, pol.ItemID);
                        if (pdoc.OutputAction.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = r.Message1 + " : " + pdoc.OutputAction.Message1;
                        }
                    }

                    pdoc.head.POID = IDRuunerService.GetNewIDV2("PO", pdoc.head.RComID, pdoc.head.ComID, pdoc.head.PODate, false, "th")[1];
                 var r_po_save=   POS_POService.Save("insert",pdoc,user);

                    if (r_po_save.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = r.Message1 + " : " + r_po_save.Message1;
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



        async public Task<I_ORDFiterSet> GetSessionPOSOrderFiterSet() {
            I_ORDFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("POSOrder_Fiter");
            if (strdoc == null) {
                return null;
            } else {
                return JsonConvert.DeserializeObject<I_ORDFiterSet>(strdoc);
            }

        }


        async public void SetSessionPOSOrderFiterSet(I_ORDFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("POSOrder_Fiter", json);
        }


        
        #endregion
    }
}