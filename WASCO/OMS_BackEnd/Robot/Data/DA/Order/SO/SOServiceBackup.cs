using Robot.Data.GADB.TT;
using Robot.Data.ML;
using Robot.Data.DA.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using Robot.Data.DA.Document;
using System.Text.Json;
using Newtonsoft.Json;
using Blazored.SessionStorage;
using Microsoft.EntityFrameworkCore;

namespace Robot.Data.DA.Order.SO {

    public class Dogs {
        public string Name { get; set; }
        public string Race { get; set; }
        public int Leg { get; set; }
    }
    public class SOServiceBackup {
     

        

        public static string sessionActiveId = "activesoid";
        public static string sessionDocType = "sotype";
        public static string sessionSubBackUrl = "supbackurl";
        public class I_ORDSet {
            public OSOHead Head { get; set; }
            public List<vw_OSOLine> Line { get; set; }
            public vw_OSOLine LineActive { get; set; }
            public List<vw_OSOLot> Lot { get; set; }
            public vw_OSOLot LotActive { get; set; }

            public List<vw_OINVLine> InvoiceHistory { get; set; }
            public List<vw_XFilesRef> files { get; set; }
            public List<TransactionLog> Log { get; set; }
            public List<DLDocLine> LineIndex { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class I_ORDFiterSet {
            public String Rcom { get; set; }
            public String Com { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowActive { get; set; }
            public bool ShowBillingPending { get; set; }
        }

        public class I_ComboBinding {
            public String Desc { get; set; }
            public String Value { get; set; }
            public string Menu { get; set; }
            public List<string> ValueRelate { get; set; }

        }
        public string PreviousPageUrl = "";
        public string PreviousSubPageUrl = "";

        public I_ORDSet DocSet { get; set; }
        ISessionStorageService sessionStorage;

        public SOServiceBackup(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;

        }

        #region Query Transaction

        public I_ORDSet GetDocSet(string docid, string rcom, string com, bool isCopy = false) {
            I_ORDSet n = NewTransaction("ORDER", rcom, com);
            try {
                using (GAEntities db = new GAEntities()) {
                    n.Head = db.OSOHead.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    n.Line = db.vw_OSOLine.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).ToList();
                    n.Lot = db.vw_OSOLot.Where(o => o.OrdID == docid && o.RComID == rcom && o.ComID == com).ToList();
                    n.InvoiceHistory = db.vw_OINVLine.Where(o => o.SOID == docid && o.RCompanyID == rcom && o.CompanyID == com && o.IsActive).OrderByDescending(o => o.INVDate).ToList();
                    n.files=db.vw_XFilesRef.Where(o=>o.DocID==docid && o.RCompanyID==rcom && o.CompanyID==com && o.DocType== "SO_DOCUMENT" && o.IsActive).ToList();  
                    n.Log = TransactionService.ListLogByDocID(docid, rcom, com);

                    if (isCopy) {
                        var h = DocSet.Head;
                        h.ID = 0;
                        h.OrdID = "";
                        h.OrdDate = DateTime.Now.Date;
                        h.INVDate = DateTime.Now.Date;
                        h.CreatedBy = "COPY";
                        h.CreatedDate = DateTime.Now;
                        h.ModifiedBy = "";
                        h.ModifiedDate = null;
                        h.QtyInvoicePending = 0;
                        h.QtyInvoice = 0;
                        h.AmtInvoice = 0;
                        h.INVID = "";
                        h.INVDate = null;
                        h.Status = "OPEN";
                        n.Head = h;
                        n.Log = new List<TransactionLog>();
                        n.InvoiceHistory = new List<vw_OINVLine>();
                        foreach (var l in n.Line) {
                            l.ID = 0;
                        }
                        n = CalDocSet(n);
                    }

                }
            } catch (Exception ex) {
                n.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return n;
        }
        public static I_ORDSet GetLatestFiles(I_ORDSet doc) { 
            try {
                using (GAEntities db = new GAEntities()) {
                    doc.files = db.vw_XFilesRef.Where(o => o.DocID == doc.Head.OrdID && o.RCompanyID == doc.Head.RComID && o.CompanyID == doc.Head.ComID && o.DocType == "SO_DOCUMENT" && o.IsActive).ToList();

                }
            } catch (Exception ex) {
             
            }
            return doc;
        }
        public static List<OSOHead> ListDocOrder(I_ORDFiterSet f) {
            List<OSOHead> result = new List<OSOHead>();
            try {
                using (GAEntities db = new GAEntities()) {
                    if (string.IsNullOrEmpty(f.SearchText)) {
                        result = db.OSOHead.Where(o =>
                                                         (o.Status == f.Status || f.Status == "")
                                                      && o.OrdDate >= f.DateFrom && o.OrdDate <= f.DateTo
                                                      && o.ComID == f.Com
                                                      && o.RComID == f.Rcom
                                                       && o.DocTypeID == f.DocType
                                                      && o.IsActive == true
                                      ).OrderByDescending(o => o.CreatedDate).ToList();
                    } else {
                        result = db.OSOHead.Where(o => (
                                                                      o.OrdID.Contains(f.SearchText)
                                                                  || o.CustID.Contains(f.SearchText)
                                                                  || o.CustName.Contains(f.SearchText)
                                                                  || f.SearchText == ""
                                                      )
                                                      && (o.Status == f.Status || f.Status == "")
                                                       && o.DocTypeID == f.DocType
                                                      && o.ComID == f.Com
                                                      && o.RComID == f.Rcom
                                                      && o.IsActive == true
                                      ).OrderByDescending(o => o.CreatedDate).ToList();
                    }


                    if (f.ShowBillingPending == true) {
                        result = result.Where(o => o.AmtInvoicePending != 0).OrderByDescending(o => o.CreatedDate).ToList();

                    } else {
                        //result = result.Where(o => o.AmtInvoicePending != 0).OrderByDescending(o => o.CreatedDate).ToList();
                    }
                }
            } catch (Exception) {
            }
            return result;
        }

        public static IEnumerable<OSOHead> ListOSOHead(string rcom, string com) {
            IEnumerable<OSOHead> result = new List<OSOHead>();
            using (GAEntities db = new GAEntities()) {
                result = db.OSOHead.Where(o => o.RComID == rcom
                                                        && o.ComID == com
                                                        && o.IsActive == true).AsNoTrackingWithIdentityResolution().ToArray();
            }
            return result;
        }

        public static List<LocationInfo> ListLocationInfo(string rcom, string com) {
            List<LocationInfo> result = new List<LocationInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.LocationInfo.Where(o => o.RCompanyID == rcom
                                                        && o.CompanyID == com
                                                        && o.IsActive == true).ToList();
            }
            return result;
        }

        public static List<DLDocLine> ListDocLine(List<vw_OSOLine> line) {
            List<DLDocLine> dline = new List<DLDocLine>();
            int index = 1;
            foreach (var d in line) {
                var da = new DLDocLine();
                da.ID = d.ID;
                da.LineNum = d.LineNum;
                da.Key = d.LineNum.ToString("n0");//d.OrdID;
                da.Description = index + ". " + d.ItemID + " (" + d.ItemName + ")";
                da.Index = index;
                da.Type = "";
                da.CurrentIndex = false;
                if (index == 1) {
                    da.IsFirst = true;
                    if (line.Count == 1) {
                        da.IsLast = true;
                    }
                }
                if (index > 1 && line.Count() == index + 1) {
                    da.IsLast = true;
                }

                index++;
                dline.Add(da);
            }
            return dline;
        }

        public static vw_OSOLine GetLineActive(int linenum, List<vw_OSOLine> line) {
            vw_OSOLine result = new vw_OSOLine();
            result = line.Where(o => o.LineNum == linenum).FirstOrDefault();

            return result;
        }


        public static OSOHead GetORDER(string ordid, string rcom, string com) {
            OSOHead result = new OSOHead();
            using (GAEntities db = new GAEntities()) {
                result = db.OSOHead.Where(o => o.OrdID == ordid && o.RComID == rcom && o.ComID == com).FirstOrDefault();
            }
            return result;
        }

        public static decimal GetItemVatRate(string itemId) {
            decimal result = 0;
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.ItemInfo.Where(o => o.ItemID == itemId).FirstOrDefault();
                    var vat = db.TaxInfo.Where(o => o.TaxTypeID == query.TaxTypeID && o.Type == "SALE").FirstOrDefault();
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

        public static I_BasicResult Save(string action , I_ORDSet input, string rcom, string com, string user , bool ischecklock) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                input = CalDocSet(input);
                var head = input.Head;
                //                var v = CheckEditableSO(head.OrdID, rcom, com);
                //if (v.Result == "fail") {
                //    result = v;
                //    return result;
                //}

                using (GAEntities db = new GAEntities()) {

                    if (action == "insert") {//new transaction
                        input = checkDupID(input, rcom, com);
                        if (input.OutputAction.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = input.OutputAction.Message1;
                        }

                        var solines = Convert2SOLine(input.Line);
                        var solot = Convert2SOLot(input.Lot);
                        db.OSOHead.Add(input.Head);
                        db.OSOLine.AddRange(solines);
                        db.OSOLot.AddRange(solot);
                        db.SaveChanges();
                        if (input.NeedRunNextID) {
                            IDRuunerService.GetNewIDV2("OORDER", rcom, input.Head.ComID, input.Head.OrdDate, true, "th");
                            input.NeedRunNextID = false;
                        }

                        TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.OrdID, TableID = "OORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "INSERT NEW ORDER" }, rcom, input.Head.ComID, user);
                    } else {
                        var uh = db.OSOHead.Where(o => o.OrdID == input.Head.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
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

                        uh.CustTaxID = input.Head.CustTaxID;
                        uh.CustBrnID = input.Head.CustBrnID;
                        uh.CustBrnName = input.Head.CustBrnName;
                        uh.QtyInvoice = input.Head.QtyInvoice;
                        uh.QtyInvoicePending = input.Head.QtyInvoicePending;
                        uh.QtyShipPending = input.Head.QtyShipPending;
                        uh.AmtInvoicePending = input.Head.AmtInvoicePending;
                        uh.AmtShipPending = input.Head.AmtShipPending;

                        uh.CountLine = input.Head.CountLine;
                        uh.NetTotalAmt = input.Head.NetTotalAmt;
                        uh.NetTotalVatAmt = input.Head.NetTotalVatAmt;
                        uh.NetTotalAmtIncVat = input.Head.NetTotalAmtIncVat;
                        uh.BaseNetTotalAmt = input.Head.BaseNetTotalAmt;
                        uh.OntopDiscPer = input.Head.OntopDiscPer;
                        uh.OntopDiscAmt = input.Head.OntopDiscAmt;
                        uh.DiscCalBy = input.Head.DiscCalBy;
                        uh.VatRate = input.Head.VatRate;
                        uh.VatTypeID = input.Head.VatTypeID;
                        uh.Remark1 = input.Head.Remark1;
                        uh.Remark2 = input.Head.Remark2;

                        uh.PaymentMemo = input.Head.PaymentMemo;

                        uh.ModifiedBy = user;
                        uh.ModifiedDate = DateTime.Now;
                        var solines = Convert2SOLine(input.Line);
                        var solot = Convert2SOLot(input.Lot);
                        db.OSOLine.RemoveRange(db.OSOLine.Where(o => o.OrdID == uh.OrdID && o.RComID == uh.RComID && o.ComID == uh.ComID));
                        db.OSOLine.AddRange(solines);
                        db.OSOLot.RemoveRange(db.OSOLot.Where(o => o.OrdID == uh.OrdID && o.RComID == uh.RComID && o.ComID == uh.ComID));
                        db.OSOLot.AddRange(solot);
                        db.SaveChanges();

                        TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.OrdID, TableID = "OORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "UPDATE ORDER" }, rcom, input.Head.ComID, user);
                    }
                }
                RefreshSet(head.OrdID, rcom, head.ComID);
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

        public static I_ORDSet CalDocSet(I_ORDSet input) {
            var head = input.Head;
            var line = input.Line;
            var lot = input.Lot;
            var invHis = input.InvoiceHistory;
            foreach (var l in line) {
                //1.cal line base total amt
                l.BaseTotalAmt = l.Qty * l.Price;
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

            var line_v = line.Where(o => o.ItemTypeID != "MISC");

            //5. cal head base total amt
            var head_amt_aft_dis = line_v.Sum(o => o.BaseTotalAmt) - line_v.Sum(o => o.DiscAmt);
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
                l.OntopDiscPer = head.OntopDiscPer;
                l.OntopDiscAmt = Math.Round(head.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);


                //8.cal line disc weight ontop percent & amt

                if (head.BaseNetTotalAmt != 0) {
                    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * head.OntopDiscAmt) / head.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                }
                //9 cal line total amt 
                l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
                //10.cal line vat amt
                l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 2, MidpointRounding.AwayFromZero);
                //11.cal line nettotal amt inc vat
                l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 2, MidpointRounding.AwayFromZero);
                //l.AmtInvoicePending = l.TotalAmtIncVat;
                //}

            }
            foreach (var l in lot) {
                l.CustID = head.CustID;
                l.OrdDate=head.OrdDate;
                l.OrdID = head.OrdID;
                l.DocTypeID = head.DocTypeID;
                l.RComID = head.RComID;
                l.ComID = head.ComID;
                
            }

                //13.copy line to head

                head.NetTotalAmt = Math.Round(line_v.Sum(o => o.TotalAmt), 2, MidpointRounding.AwayFromZero);
            head.NetTotalVatAmt = Math.Round(line_v.Sum(o => o.VatAmt), 2, MidpointRounding.AwayFromZero);
            head.NetTotalAmtIncVat = Math.Round(line_v.Sum(o => o.TotalAmtIncVat), 2, MidpointRounding.AwayFromZero);
            head.Qty = line_v.Sum(o => o.Qty);
            // head.AmtInvoicePending = line_v.Sum(o => o.AmtInvoicePending);
            head.CountLine = line_v.Count();
            return input;
        }

        public static I_ORDSet checkDupID(I_ORDSet input, string rcom, string com) {
            try {
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                using (GAEntities db = new GAEntities()) {
                    var h = input.Head;
                    var get_id = db.OSOHead.Where(o => o.OrdID == h.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    if (input.NeedRunNextID) {
                        int i = 0;
                        while (get_id != null) {
                            if (i > 1000) {
                                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
                                break;
                            }
                            i++;

                            IDRuunerService.GetNewIDV2("OORDER", rcom, input.Head.ComID, input.Head.OrdDate, true, "th");
                            h.OrdID = IDRuunerService.GetNewIDV2("OORDER", rcom, input.Head.ComID, input.Head.OrdDate, false, "th")[1];
                            get_id = db.OSOHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.OrdID == h.OrdID).FirstOrDefault();
                        }
                    } else {
                        if (get_id != null) {
                            input.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Duplication Order number " + h.OrdID, Message2 = "" };
                        }
                    }

                }
                input = CalDocSet(input);

            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input;


        }

        public static bool checkHasLine(string ordId, string rcom, string com) {
            bool result = false;
            try {

                using (GAEntities db = new GAEntities()) {
                    var lines = db.OSOLine.Where(o => o.OrdID == ordId && o.RComID == rcom && o.ComID == com).Count();
                    if (lines > 0) {
                        result = true;
                    }

                }

            } catch (Exception ex) {

            }
            return result;
        }


        public static I_BasicResult CheckEditableSO(string soId, string rcom, string com) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {

                    var invls = db.OINVLine.Where(o => o.SOID == soId && o.CompanyID == com && o.RCompanyID == rcom && o.IsActive==true).ToList();

                    if (invls.Count > 0) {
                        result.Result = "fail";
                        result.Message1 = "ออเดอร์นี้เปิดอินวอยซ์แล้ว ไม่สามารถแก้ไขหรือลบได้";
                        return result;
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

        #endregion

        #region Delete

        public static I_BasicResult DeleteDoc(string soId, string rcom, string com, string createby) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var v = CheckEditableSO(soId, rcom, com);
                if (v.Result == "fail") {
                    result = v;
                    return result;
                }
                using (GAEntities db = new GAEntities()) {
                    var head = db.OSOHead.Where(o => o.OrdID == soId && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    var line = db.OSOLine.Where(o => o.OrdID == soId && o.RComID == rcom && o.ComID == com).ToList();
                    head.Status = "CANCEL";
                    head.ModifiedBy = createby;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                        l.Status = "CANCEL";
                    }

                    db.SaveChanges();
                    TransactionService.SaveLog(new TransactionLog { TransactionID = head.OrdID, TableID = "OORDER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = head.ComID, Action = "DELETE ORDER" }, rcom, head.ComID, createby);
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

        public static I_ORDSet DeleteLine(int linenum, I_ORDSet input) {
            try {
                input.Line.RemoveAll(o => o.LineNum == linenum);
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                input = CalDocSet(input);
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }

            return input;
        }

        #endregion

        #region New transaction

        async public void SetSessionORDFiterSet(I_ORDFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("so_Fiter", json);
        }
        async public Task<I_ORDFiterSet> GetSessionORDFiterSet() {
            I_ORDFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("so_Fiter");
            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_ORDFiterSet>(strdoc);
            } else {
                return null;
            }

        }

        public static I_ORDFiterSet NewFilterSet() {
            I_ORDFiterSet n = new I_ORDFiterSet();

            n.Rcom = "";
            n.Com = "";
            n.DateFrom = DateTime.Now.Date.AddMonths(-6);
            n.DateTo = DateTime.Now.Date.AddMinutes(1);
            n.DocType = "";
            n.Status = "";
            n.SearchBy = "DOCDATE";
            n.SearchText = "";
            n.ShowActive = true;
            n.ShowBillingPending = false;

            return n;
        }

        public static I_ORDSet NewTransaction( string rcom, string com, string doctype) {
            I_ORDSet n = new I_ORDSet();
            //ORDERDetailBase.txtord_id = "";
            n.Head = NewHead(rcom, com, doctype);
            n.Line = new List<vw_OSOLine>();
            n.Lot = new List<vw_OSOLot>();
            n.InvoiceHistory = new List<vw_OINVLine>();
            n.NeedRunNextID = false;
            n.LineActive = NewLine(rcom, com, n);
            n.files = new List<vw_XFilesRef>();
            n.Log = new List<TransactionLog>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return n;
        }

        public static OSOHead NewHead( string rcom, string com, string doctype) {
            OSOHead n = new OSOHead();

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
            //n.CustIDRefInCom = "";
            n.CustName = "";
            n.CustAddr1 = "";
            n.CustAddr1 = "";
            n.CustMobile = "";
            n.CustEmail = "";
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
            n.SalesID1 = "";
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
            n.OntopDiscPer = 0;
            n.OntopDiscAmt = 0;
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
            n.Status = "OPEN";
            n.Source = "BACK";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static vw_OSOLine NewLine(string rcom, string com, I_ORDSet input) {
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
            n.Status = "NEW";
            n.Sort = 1;
            n.ImageUrl = "";
            n.PageBreak = false;
            n.IsComplete = false;
            n.IsActive = true;
            return n;
        }

        public static vw_OSOLot NewLot(string rcom, string com,int line, I_ORDSet input) {
            vw_OSOLot n = new vw_OSOLot(); 
            n.RComID = "";
            n.ComID = "";
            n.OrdID = "";
            n.LineLineNum = line;
            n.LineNum = GenLotLineNum(input);
            n.LineLineNum = line;
            n.LineNum = line;
            n.DocTypeID = "";
            n.OrdDate = DateTime.Now.Date;
            n.CustID = "";
            n.ItemID = "";
            n.IsStockItem = true;
            n.Qty = 0;
            n.Unit = "";
            n.LocID = "";
            n.LotNo = "";
            n.Status = "OPEN";
            n.SerialNo = "";
            n.IsActive = true;
            return n;
        }

        public static List<OSOLine> Convert2SOLine(List<vw_OSOLine> input) {
            List<OSOLine> output = new List<OSOLine>();
            foreach (var i in input) {
                OSOLine n = new OSOLine();
            
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
                n.Price = i.Price;
                n.PriceIncVat = i.PriceIncVat;
                n.BaseTotalAmt = i.BaseTotalAmt;
                n.TotalAmt = i.TotalAmt;
                n.VatAmt = i.VatAmt;
                n.TotalAmtIncVat = i.TotalAmtIncVat;
                n.VatRate = i.VatRate;
                n.VatTypeID = i.VatTypeID;
                n.OntopDiscAmt = i.OntopDiscAmt;
                n.OntopDiscPer = i.OntopDiscPer;
                n.DiscPer = i.DiscPer;
                n.DiscAmt = i.DiscAmt;
                n.DiscCalBy = i.DiscCalBy;
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

        public static void AddLine(string rcom, string com, I_ORDSet input) {
            ClearPendingLine(input);
            input.Line.Add(NewLine(rcom, com, input));
            input.LineActive = input.Line.Where(o => o.Status == "NEW").OrderByDescending(o => o.LineNum).FirstOrDefault();
            //ListDocLine();
        }

        public static void ClearPendingLine(I_ORDSet input) {
            try {
                var r1 = input.Line.RemoveAll(o => o.Status == "NEW");
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static int GenLineNum(I_ORDSet input) {
            int result = 10;
            try {
                var max_linenum = input.Line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenLotLineNum(I_ORDSet input) {
            int result = 10;
            try {
                var max_linenum = input.Lot.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenLineSort(I_ORDSet input) {
            int result = 1;
            try {
                var max_sort = input.Line.Max(o => o.Sort);
                result = Convert.ToInt32(max_sort) + 1;
            } catch { }
            return result;
        }
        #endregion

        #region Refresh doc

        //refresh main document
        public static void RefreshSet(string soId, string rcom, string comId) {
            UpdateSOFromInvoice(soId, rcom, comId);
        }



        public static I_BasicResult UpdateSOFromInvoice(string soId, string rcom, string comId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    //1.get so head info
                    var soh = db.OSOHead.Where(o => o.OrdID == soId && o.ComID == comId && o.RComID == rcom).FirstOrDefault();
                    //2.get so lin info
                    var sols = db.OSOLine.Where(o => o.OrdID == soId && o.ComID == comId && o.RComID == rcom && o.IsActive == true).ToList();

                    //4.list invoice line 
                    List<string> cn_type = new List<string> { "OCN1", "OCN2" };
                    List<string> inv_type = new List<string> { "OINV1", "OINV2", "ODN1", "ODN2" };

                    var amtinv = db.OINVLine.Where(o => o.SOID == soId && o.CompanyID == comId && o.RCompanyID == rcom && o.IsActive ==true && o.ItemTypeID != "MISC" && inv_type.Contains(o.DocTypeID)).ToList().Sum(o => o.TotalAmt);
                    var amtcn = db.OINVLine.Where(o => o.SOID == soId && o.CompanyID == comId && o.RCompanyID == rcom && o.IsActive == true  && o.ItemTypeID != "MISC" && cn_type.Contains(o.DocTypeID)).ToList().Sum(o => o.TotalAmt);

                    soh.AmtInvoice = amtinv - (amtcn * -1);

                    soh.AmtInvoicePending = soh.NetTotalAmt - soh.AmtInvoice;
                    var invLine = db.OINVLine.Where(o => o.SOID == soId && o.CompanyID == comId && o.RCompanyID == rcom && o.IsActive == true).ToList();
                    var lastInvLine = invLine.OrderByDescending(o => o.INVDate).ThenByDescending(o => o.INVID).FirstOrDefault();

                    //var inv = invLine.FirstOrDefault();
                    //5. update invoice qty + amt pending in so head

                    foreach (var sol in sols.Where(o => o.ItemTypeID != "MISC")) {

                        var sum_qtyInv = invLine.Where(o => o.SOID == sol.OrdID && o.SOLineNum == sol.LineNum && inv_type.Contains(o.DocTypeID)).ToList().Sum(o => o.Qty);
                        var sum_qtycn = invLine.Where(o => o.SOID == sol.OrdID && o.SOLineNum == sol.LineNum && cn_type.Contains(o.DocTypeID)).ToList().Sum(o => o.Qty);
                        var sum_amtInv = invLine.Where(o => o.SOID == sol.OrdID && o.SOLineNum == sol.LineNum && inv_type.Contains(o.DocTypeID)).ToList().Sum(o => o.TotalAmt);
                        var sum_amtcn = invLine.Where(o => o.SOID == sol.OrdID && o.SOLineNum == sol.LineNum && cn_type.Contains(o.DocTypeID)).ToList().Sum(o => o.TotalAmt);
                        sol.QtyInvoice = sum_qtyInv - (sum_qtycn * -1);
                        sol.QtyInvoicePending = sol.Qty - (sol.QtyInvoice + sum_amtcn * -1);
                    }


                    //6. update InvID pending in soline
                    if (lastInvLine != null) {
                        soh.INVID = lastInvLine.INVID;
                        soh.INVDate = lastInvLine.INVDate;
                    } else {
                        soh.INVID = "";
                        soh.INVDate = null;
                    }
                    soh.QtyInvoice = sols.Where(o => o.ItemTypeID != "MISC").Sum(o => o.QtyInvoice);
                    soh.QtyInvoicePending = sols.Where(o => o.ItemTypeID != "MISC").Sum(o => o.QtyInvoicePending);


                    db.SaveChanges();

                }
            } catch (Exception ex) {
                result.Result = "fail";

                if (ex.InnerException == null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }

            }

            return result;
        }

        #endregion

      


        public static List<SelectOption> ListDiscBy() {
            return new List<SelectOption>() {
                new SelectOption(){ IsSelect = true ,Value= "P", Description="%" ,Sort = 1},
                new SelectOption(){ IsSelect = true ,Value = "A", Description="$", Sort = 2}
            };
        }

        #region Go to Record
        public static OSOHead GoPreviousDoc(int currID, string rcom, string com) {
            OSOHead query = new OSOHead();
            using (GAEntities db = new GAEntities()) {
                var curr = db.OSOHead.Where(o => o.ID == currID).FirstOrDefault();
                query = db.OSOHead.Where(o => o.ID < curr.ID && o.ComID == com && o.RComID == rcom && o.IsActive == true).FirstOrDefault();
                if (query != null) {
                    query = db.OSOHead.Where(o => o.OrdID == query.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                } else {//go to first
                    var last = db.OSOHead.Where(o => o.ComID == com && o.RComID == rcom).OrderByDescending(o => o.OrdID).FirstOrDefault();
                    if (last != null) {
                        query = db.OSOHead.Where(o => o.OrdID == last.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    }
                }
            }

            return query;
        }

        public static OSOHead GoNextDoc(int currID, string rcom, string com) {
            OSOHead query = new OSOHead();
            using (GAEntities db = new GAEntities()) {
                var curr = db.OSOHead.Where(o => o.ID == currID).FirstOrDefault();
                query = db.OSOHead.Where(o => o.ID > curr.ID && o.ComID == com && o.RComID == rcom && o.IsActive == true).FirstOrDefault();

                if (query != null) {
                    query = db.OSOHead.Where(o => o.OrdID == query.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                } else {//go to first
                    var first = db.OSOHead.Where(o => o.ComID == com && o.RComID == rcom).OrderBy(o => o.OrdID).FirstOrDefault();
                    if (first != null) {
                        query = db.OSOHead.Where(o => o.OrdID == first.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    }
                }

            }
            return query;
        }

        public static OSOHead GoLastDoc(string rcom, string com) {
            OSOHead query = new OSOHead();
            using (GAEntities db = new GAEntities()) {
                query = db.OSOHead.Where(o => o.ComID == com && o.RComID == rcom && o.IsActive ==true).OrderByDescending(o => o.OrdID).FirstOrDefault();
                if (query != null) {
                    query = db.OSOHead.Where(o => o.OrdID == query.OrdID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                }
            }
            return query;
        }


        #endregion

    }
}
