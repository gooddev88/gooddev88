using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using static Robot.Data.BL.I_Result;

using static Robot.Master.DA.POSItemService;

namespace Robot.POSC.DA {
    public class POSSaleService {
        public static I_POSSaleSet DocSet { get { return (I_POSSaleSet)HttpContext.Current.Session["possale_docset"]; } set { HttpContext.Current.Session["possale_docset"] = value; } }
        //public static bool IsRunOnMobile { get { return HttpContext.Current.Session["isrunonmobile"] == null ? false : (bool)HttpContext.Current.Session["isrunonmobile"]; } set { HttpContext.Current.Session["isrunonmobile"] = value; } }
        public static List<I_ShipTo> ShipTo { get { return (List<I_ShipTo>)HttpContext.Current.Session["pos_shipto"]; } set { HttpContext.Current.Session["pos_shipto"] = value; } }

        public static List<POSMenuItem> Menu { get { return (List<POSMenuItem>)HttpContext.Current.Session["possale_menu"]; } set { HttpContext.Current.Session["possale_menu"] = value; } }

        public class I_FilterSet {
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
            public string Company { get; set; }
            public string Table { get; set; }
            public string ShipTo { get; set; }
            public string MacNo { get; set; }
            public string Search { get; set; }
            public bool ShowActive { get; set; }
        }


        public class I_ShipTo {
            public string ShipToID { get; set; }
            public string ShipToName { get; set; }
        }


        public class I_POSSaleSet {
            public POS_SaleHead Head { get; set; }
            public List<POS_SaleLine> Line { get; set; }
            public POS_SaleLine LineActive { get; set; }
            public POSMenuItem SelectItem { get; set; }
            public List<POS_SalePayment> Payment { get; set; }
            public POS_SalePayment PaymentActive { get; set; }
            public List<TransactionLog> Log { get; set; }
        }



        public static POS_SaleHead GetBill(string billId, string rcom) {
            POS_SaleHead result = new POS_SaleHead();
            using (GAEntities db = new GAEntities()) {
                result = db.POS_SaleHead.Where(o => o.BillID == billId && o.RComID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static I_POSSaleSet GetDocSet(string billId) {
            I_POSSaleSet n = NewTransaction();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                n.Head = db.POS_SaleHead.Where(o => o.BillID == billId && o.RComID == rcom).FirstOrDefault();
                n.Line = db.POS_SaleLine.Where(o => o.BillID == billId && o.RComID == rcom).OrderBy(o => o.LineNum).ToList();
                n.Payment = db.POS_SalePayment.Where(o => o.BillID == billId && o.RComID == rcom).ToList();
                n.Log = TransactionInfoService.ListLogByDocID(billId, rcom, "SALE");
            }
            return n;
        }



        public static List<vw_POS_SaleHead> ListPendinbBill(I_FilterSet f) {
            List<vw_POS_SaleHead> result = new List<vw_POS_SaleHead>();
            var comlist = LoginService.LoginInfo.UserInCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_SaleHead.Where(o =>
                                                            o.ComID == f.Company
                                                            && o.RComID == rcom
                                                            && comlist.Contains(o.ComID)
                                                            && o.INVID == ""
                                                            && o.IsActive == true
                                                            ).OrderBy(o => o.BillID).ToList();
            }
            return result;
        }
        public static List<vw_POS_SaleHead> ListCancelBIll(string com, DateTime begain, DateTime end) {
            List<vw_POS_SaleHead> result = new List<vw_POS_SaleHead>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_POS_SaleHead.Where(o => (o.ComID == com || com == "")
                                                             && o.RComID == rcom
                                                            && o.BillDate >= begain && o.BillDate <= end
                                                            && o.IsActive == false).ToList();
            }
            return result;
        }
        public static List<vw_POS_SaleHead> ListDoc(I_FilterSet f) {
            List<vw_POS_SaleHead> result = new List<vw_POS_SaleHead>();
            var comlist = LoginService.LoginInfo.UserInCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                if (f.Search != "") {//search แบบระบุคำเสริช
                    result = db.vw_POS_SaleHead.Where(o =>
                                            (o.INVID.Contains(f.Search)
                                                || o.BillID.Contains(f.Search)
                                                || o.CustBranchName.Contains(f.Search)
                                                || o.CustBranchID.Contains(f.Search)
                                                || o.TableID.Contains(f.Search)
                                              )
                                                && (o.ComID == f.Company || f.Company == "")
                                                && (o.MacNo == f.MacNo || f.MacNo == "")
                                                && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                && o.RComID == rcom
                                                && comlist.Contains(o.ComID)
                                                && o.IsActive == f.ShowActive
                                                ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                } else {//search แบบระบุวันที่
                    result = db.vw_POS_SaleHead.Where(o =>
                                                        (o.BillDate >= f.Begin && o.BillDate <= f.End)
                                                        && o.RComID == rcom
                                                     && (o.MacNo == f.MacNo || f.MacNo == "")
                                                     && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                       && comlist.Contains(o.ComID)
                                                   && (o.ComID == f.Company || f.Company == "")
                                                && o.IsActive == f.ShowActive
                                            ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();

                }
            }
            return result;

        }
        public static List<vw_POS_SaleHead> ListCancelOrder(I_FilterSet f) {
            List<vw_POS_SaleHead> result = new List<vw_POS_SaleHead>();
            var comlist = LoginService.LoginInfo.UserInCompany;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                if (f.Search != "") {//search แบบระบุคำเสริช
                    result = db.vw_POS_SaleHead.Where(o =>
                                            ( 
                                                   o.BillID.Contains(f.Search)
                                                || o.CustBranchName.Contains(f.Search)
                                                || o.CustBranchID.Contains(f.Search)
                                                || o.TableID.Contains(f.Search)
                                              )
                                                && (o.ComID == f.Company || f.Company == "")
                                                && (o.MacNo == f.MacNo || f.MacNo == "")
                                                && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                && o.RComID == rcom
                                                && comlist.Contains(o.ComID)
                                                && o.INVID==""
                                                && o.IsActive == false
                                                ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();
                } else {//search แบบระบุวันที่
                    result = db.vw_POS_SaleHead.Where(o =>
                                                        (o.BillDate >= f.Begin && o.BillDate <= f.End)
                                                        && o.RComID == rcom
                                                     && (o.MacNo == f.MacNo || f.MacNo == "")
                                                     && (o.ShipToLocID == f.ShipTo || f.ShipTo == "")
                                                       && comlist.Contains(o.ComID)
                                                   && (o.ComID == f.Company || f.Company == "")
                                                   && o.INVID == ""
                                                && o.IsActive == false
                                            ).OrderByDescending(o => o.CreatedDate).ThenBy(o => o.ComID).ToList();

                }
            }
            return result;

        }



        #region Save / Delete

        public static I_BasicResult DeleteDoc(string docId) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try {
                using (GAEntities db = new GAEntities()) {

                    var head = db.POS_SaleHead.Where(o => o.BillID == docId && o.RComID == rcom).FirstOrDefault();

                    head.ModifiedBy = LoginService.LoginInfo.CurrentUser; ;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;
                    db.POS_SaleLine.Where(o => o.BillID == docId && o.RComID == rcom).ToList().ForEach(o => o.IsActive = false);
                    db.POS_SalePayment.Where(o => o.BillID == docId && o.RComID == rcom).ToList().ForEach(o => o.IsActive = false);
                    db.SaveChanges();
                    CalStock(head);
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = docId, TableID = "SALE", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "ลบรายการ" });
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

        //public static I_BasicResult DeletePermanent(string billId) {
        //    I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

        //    try {
        //        using (GAEntities db = new GAEntities()) {
        //            var rcom = LoginService.LoginInfo.CurrentRootCompany;

        //            var uh = db.POS_SaleHead.Where(o => o.BillID == billId && o.RComID== rcom).FirstOrDefault();
        //            if (uh.FINVID != "") {
        //                r.Result = "fail";
        //                r.Message1 = "ไม่สามารถลบบิลที่ออกใบกำกับภาษีแล้ว";
        //                return r;
        //            }
        //            //CompanyInfo com = CompanyInfoService.GetDataByComID(uh.ComID);
        //            //var comInfo = CompanyService.GetCompanyInfo(uh.ComID);
        //            db.POS_SaleHead.Remove(uh);
        //            db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == billId && o.RComID==rcom).ToList());
        //            db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == billId && o.RComID == rcom).ToList());
        //            db.SaveChanges();
        //            IDGeneratorServiceV2.RunBackPOSSaleID( uh.ComID, uh.MacNo, uh.ShipToLocID, uh.BillDate,1);
        //           // ApplicationService.RunBackPOSSaleID("POSA_SO", com, uh.BillID);
        //            TransactionInfoService.SaveLog(new TransactionLog { TransactionID = billId, ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "Delete permanent data" });

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
        public static I_BasicResult DeletePermanent(string storeId, string macno, string shiptoId, DateTime transDate, int billDeleteQty) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                var comInfo = LoginService.LoginInfo.CurrentCompany;
                var shortShiptoId = ShipToService.ListShipTo().Where(o => o.ShipToID == shiptoId).FirstOrDefault().ShortID;
                string year = DatetimeInfoService.Convert2ThaiYear(transDate).ToString();
                year = year.Substring(year.Length - 2);
                string month = transDate.Month.ToString("00");
                string day = transDate.Day.ToString("00");

                string prefix = shortShiptoId + comInfo.ShortCode + macno + year + month + day + "-";
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;


                using (GAEntities db = new GAEntities()) {

                    for (int i = 1; i <= billDeleteQty; i++) {



                        //var sh = db.POS_SaleHead.Where(o => o.ShipToLocID == shiptoId && o.MacNo == macno && o.BillDate == transDate && o.ComID == storeId && o.RComID == rcom && o.INVID!="" ).OrderByDescending(o => o.INVID).FirstOrDefault();
                        var sh = db.POS_SaleHead.Where(o => o.INVID.StartsWith(prefix) && o.ComID == storeId && o.RComID == rcom).OrderByDescending(o => o.INVID).FirstOrDefault();
                        if (sh == null) {
                            break;
                        } else {
                            if (!string.IsNullOrEmpty(sh.FINVID)) {
                                break;
                            } else {


                                db.POS_SaleHead.RemoveRange(db.POS_SaleHead.Where(o => o.BillID == sh.BillID && o.RComID == rcom && o.ComID == storeId));
                                db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == sh.BillID && o.RComID == rcom && o.ComID == storeId));
                                db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == sh.BillID && o.RComID == rcom && o.ComID == storeId));
                                db.SaveChanges();

                                var runing = Convert.ToInt32(sh.INVID.Substring(sh.INVID.Length - 3));
                                runing = (runing - 1) < 1 ? 1 : runing;
                                var idRecord = db.IDGenerator.Where(o => o.Prefix == prefix && o.ComID == sh.ComID && o.RComID == rcom).FirstOrDefault();
                                if (idRecord != null) {
                                    idRecord.DigitRunNumber = runing;
                                    db.SaveChanges();
                                }
                            }
                        }

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
        public static I_BasicResult Save(I_POSSaleSet doc, bool isNew) {

            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var rcom = LoginService.LoginInfo.CurrentRootCompany;
            var com = LoginService.LoginInfo.CurrentCompany;
            var h = doc.Head;


            try {
                using (GAEntities db = new GAEntities()) {


                    if (isNew) {//insert new
                        doc = checkDupBillID(doc);
                        doc = CalDocSet(doc);
                        db.POS_SaleHead.Add(doc.Head);
                        db.POS_SaleLine.AddRange(doc.Line);
                        db.POS_SalePayment.AddRange(doc.Payment);
                        db.SaveChanges();
                        IDRuunerService.GenPOSSaleID("ORDER", h.ComID, h.MacNo, h.ShipToLocID, true, h.BillDate);
                        var rr = CalStock(h);
                        if (rr.Result == "fail") {
                            r.Result = rr.Result;
                            r.Message1 = rr.Message1;
                        }
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.INVID, ParentID = "", TableID = "SALE", TransactionDate = h.CreatedDate, CompanyID = h.ComID, Action = "สร้างรายการ" });
                    } else {
                        doc = CalDocSet(doc);
                        var uh = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID).FirstOrDefault();
                        uh.BillID = h.BillID;
                        uh.INVID = h.INVID;
                        uh.FINVID = h.FINVID;
                        uh.DocTypeID = h.DocTypeID;
                        uh.MacNo = h.MacNo;
                        uh.MacTaxNo = h.MacTaxNo;
                        uh.BillDate = h.BillDate;
                        uh.BillRefID = h.BillRefID;
                        uh.INVPeriod = h.INVPeriod;
                        uh.ReasonID = h.ReasonID;
                        uh.TableID = h.TableID;
                        uh.CustomerID = h.CustomerID;
                        uh.CustomerName = h.CustomerName;
                        uh.CustAccGroup = h.CustAccGroup;
                        uh.CustTaxID = h.CustTaxID;
                        uh.CustBranchID = h.CustBranchID;
                        uh.CustBranchName = h.CustBranchName;
                        uh.CustAddr1 = h.CustAddr1;
                        uh.CustAddr2 = h.CustAddr2;
                        uh.IsVatRegister = h.IsVatRegister;
                        uh.POID = h.POID;
                        uh.ShipToLocID = h.ShipToLocID;
                        uh.ShipToLocName = h.ShipToLocName;
                        uh.ShipToAddr1 = h.ShipToAddr1;
                        uh.ShipToAddr2 = h.ShipToAddr2;
                        uh.SalesID1 = h.SalesID1;
                        uh.SalesID2 = h.SalesID2;
                        uh.SalesID3 = h.SalesID3;
                        uh.SalesID4 = h.SalesID4;
                        uh.SalesID5 = h.SalesID5;
                        uh.ContactTo = h.ContactTo;
                        uh.Currency = h.Currency;
                        uh.RateExchange = h.RateExchange;
                        uh.RateBy = h.RateBy;
                        uh.RateDate = h.RateDate;
                        uh.CreditTermID = h.CreditTermID;
                        uh.Qty = h.Qty;
                        uh.BaseNetTotalAmt = h.BaseNetTotalAmt;
                        uh.NetTotalAmt = h.NetTotalAmt;
                        uh.NetTotalVatAmt = h.NetTotalVatAmt;
                        uh.NetTotalAmtIncVat = h.NetTotalAmtIncVat;
                        uh.OntopDiscPer = h.OntopDiscPer;
                        uh.OntopDiscAmt = h.OntopDiscAmt;
                        uh.ItemDiscAmt = h.ItemDiscAmt;
                        uh.DiscCalBy = h.DiscCalBy;
                        uh.LineDisc = h.LineDisc;
                        uh.VatRate = h.VatRate;
                        uh.VatTypeID = h.VatTypeID;
                        uh.IsVatInPrice = h.IsVatInPrice;
                        uh.NetTotalAfterRound = h.NetTotalAfterRound;
                        uh.NetDiff = h.NetDiff;
                        uh.PayByCash = h.PayByCash;
                        uh.PayByOther = h.PayByOther;
                        uh.PayByCredit = h.PayByCredit;
                        uh.PayTotalAmt = h.PayTotalAmt;
                        uh.PayMethodCount = h.PayMethodCount;
                        uh.Remark1 = h.Remark1;
                        uh.Remark2 = h.Remark2;
                        uh.LocID = h.LocID;
                        uh.SubLocID = h.SubLocID;
                        uh.CountLine = h.CountLine;
                        uh.IsLink = h.IsLink;
                        uh.LinkBy = h.LinkBy;
                        uh.LinkDate = h.LinkDate;
                        uh.Status = h.Status;
                        uh.IsPrint = h.IsPrint;
                        uh.PrintDate = h.PrintDate;
                        uh.ModifiedBy = h.ModifiedBy;
                        uh.ModifiedDate = h.ModifiedDate;
                        //db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == h.BillID));
                        //db.POS_SaleLine.AddRange(doc.Line);
                        //db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == h.BillID  ));
                        //db.POS_SalePayment.AddRange(doc.Payment);


                        db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID));
                        db.POS_SaleLine.AddRange(doc.Line);
                        db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == h.BillID && o.RComID == h.RComID && o.ComID == h.ComID));
                        db.POS_SalePayment.AddRange(doc.Payment);
                        db.SaveChanges();
                        var rr = CalStock(h);
                        if (rr.Result == "fail") {
                            r.Result = rr.Result;
                            r.Message1 = rr.Message1;
                        }
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.INVID, ParentID = "", TableID = "SALE", TransactionDate = h.BillDate, CompanyID = h.ComID, Action = "แก้ไขรายการ" });
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

        public static I_POSSaleSet checkDupBillID(I_POSSaleSet doc) {
            try {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = doc.Head;

                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.BillID == h.BillID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            r = new I_BasicResult { Result = "fail", Message1 = "Cannot create bill no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GenPOSSaleID("ORDER", h.ComID, h.MacNo, h.ShipToLocID, true, h.BillDate);

                        h.BillID = IDRuunerService.GenPOSSaleID("ORDER", h.ComID, h.MacNo, h.ShipToLocID, false, h.BillDate)[1];
                        get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.BillID == h.BillID).FirstOrDefault();
                    }
                }
            } catch (Exception ex) {
            }
            return doc;
        }

        public static I_POSSaleSet checkDupInvoiceID(I_POSSaleSet doc) {
            try {
                I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = doc.Head;

                using (GAEntities db = new GAEntities()) {
                    var get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.INVID == h.INVID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            r = new I_BasicResult { Result = "fail", Message1 = "Cannot create invoice no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GenPOSSaleID("INV", h.ComID, h.MacNo, h.ShipToLocID, true, h.BillDate);

                        h.INVID = IDRuunerService.GenPOSSaleID("INV", h.ComID, h.MacNo, h.ShipToLocID, false, h.BillDate)[1];
                        get_id = db.POS_SaleHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.INVID == h.INVID).FirstOrDefault();
                    }
                }
            } catch (Exception ex) {
            }
            return doc;
        }
        public static I_BasicResult SaveTaxSlip(I_POSSaleSet doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var h = doc.Head;
            try {
                using (GAEntities db = new GAEntities()) {
                    doc = CalDocSet(doc);
                    var uh = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID).FirstOrDefault();
                    if (string.IsNullOrEmpty(doc.Head.FINVID)) {

                        uh.FINVID = IDRuunerService.GenPOSSaleID("TAX", h.ComID, h.MacNo, h.ShipToLocID, false, h.BillDate)[1];
                    }

                    uh.CustomerName = h.CustomerName;
                    uh.CustBranchName = h.CustBranchName;
                    uh.CustAddr1 = h.CustAddr1;
                    uh.CustAddr2 = h.CustAddr2;
                    uh.CustTaxID = h.CustTaxID;
                    db.SaveChanges();
                    IDRuunerService.GenPOSSaleID("TAX", h.ComID, h.MacNo, h.ShipToLocID, true, h.BillDate);
                    //  ApplicationService.Get_NewID("POSA_INV", comdata);//run next id in idgenerator table
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



        public static I_BasicResult SaveInvoice(I_POSSaleSet doc) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };


            var h = doc.Head;

            try {
                using (GAEntities db = new GAEntities()) {
                    if (!string.IsNullOrEmpty(doc.Head.INVID)) {
                        return r;
                    }

                    h.INVID = IDRuunerService.GenPOSSaleID("INV", h.ComID, h.MacNo, h.ShipToLocID, false, h.BillDate)[1];
                    doc = checkDupInvoiceID(doc);
                    var uh = db.POS_SaleHead.Where(o => o.BillID == h.BillID && o.RComID == h.RComID).FirstOrDefault();
                    uh.INVID = h.INVID;
                    uh.Status = "CLOSED";
                    doc = CalDocSet(doc);
                    db.POS_SaleLine.RemoveRange(db.POS_SaleLine.Where(o => o.BillID == h.BillID && o.RComID == h.RComID));
                    db.POS_SaleLine.AddRange(doc.Line);
                    db.POS_SalePayment.RemoveRange(db.POS_SalePayment.Where(o => o.BillID == h.BillID && o.RComID == h.RComID));
                    db.POS_SalePayment.AddRange(doc.Payment);
                    db.SaveChanges();
                    IDRuunerService.GenPOSSaleID("INV", h.ComID, h.MacNo, h.ShipToLocID, true, h.BillDate);
                    r = CalStock(doc.Head);
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




        public static I_POSSaleSet CalDocSet(I_POSSaleSet doc) {
            var h = doc.Head;
            //copy head 2 line
            decimal baseDisc = doc.Line.Where(o => o.DocTypeID != "DISCOUNT").Sum(o => o.BaseTotalAmt);
            foreach (var l in doc.Line) {
                //1.cal line base total amt

                if (l.ItemTypeID == "DISCOUNT") {//  line ที่เป็น discount
                    //2. cal disc amt
                    if (l.DiscCalBy == "P") {
                        l.DiscAmt = (baseDisc * l.DiscPer) / 100;
                        //l.BaseTotalAmt = l.DiscAmt;

                    }
                    //3. cal disc per
                    if (l.DiscCalBy == "A") {
                        l.DiscPer = 0;
                        //l.BaseTotalAmt = l.DiscAmt;
                        if (baseDisc != 0) {
                            l.DiscPer = (100 * l.DiscAmt) / baseDisc;
                        }
                    }
                } else {//เป็นรายการขาย
                    l.BaseTotalAmt = l.Qty * l.Price;
                    l.TotalAmt = l.BaseTotalAmt;
                }
                //4.assign head 2 line'
                l.TableID = h.TableID;
                l.TableName = h.TableName;
                l.ShipToLocID = h.ShipToLocID;
                l.ShipToLocName = h.ShipToLocName;


                l.INVID = h.INVID;
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.BillDate = h.BillDate;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.MacNo = h.MacNo;
                l.DocTypeID = h.DocTypeID;
                l.CustID = h.CustomerID;
                l.VatRate = h.VatRate;
                l.VatTypeID = h.VatTypeID;
            }

            //5. cal head base total amt            
            var baseTotalAmt = doc.Line.Where(o => o.IsActive && o.Status != "K-REJECT").Sum(o => o.BaseTotalAmt);
            var discAmt = doc.Line.Where(o => o.IsActive && o.Status != "K-REJECT").Sum(o => o.DiscAmt);
            doc.Head.BaseNetTotalAmt = baseTotalAmt - discAmt;
            //6. cal head disc amt
            if (doc.Head.DiscCalBy == "P") {
                doc.Head.OntopDiscAmt = (doc.Head.BaseNetTotalAmt * doc.Head.OntopDiscPer) / 100;
            }
            //7. cal head disc per
            if (doc.Head.DiscCalBy == "A") {
                if (doc.Head.BaseNetTotalAmt != 0) {
                    doc.Head.OntopDiscPer = (100 * doc.Head.OntopDiscAmt) / doc.Head.BaseNetTotalAmt;
                }
            }

            foreach (var l in doc.Line) {
                //7.ตั้งต้น Ontop discount จาก Header ก่อนนำไปคำนวณใน Line
                l.OntopDiscPer = h.OntopDiscPer;
                l.OntopDiscAmt = Math.Round(h.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);

                //8.cal line disc weight ontop percent & amt

                if (h.BaseNetTotalAmt != 0) {
                    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * h.OntopDiscAmt) / h.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                }
                //9 cal line total amt 
                l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero); ;
                //10.cal line vat amt
                l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 2, MidpointRounding.AwayFromZero);
                //11.cal line nettotal amt inc vat
                l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 2, MidpointRounding.AwayFromZero);
            }

            //sum 2 head 
            h.ItemDiscAmt = doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.IsActive && o.Status != "K-REJECT").Sum(o => o.TotalAmtIncVat);
            h.LineDisc = (doc.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.IsActive && o.Status != "K-REJECT").Sum(o => o.TotalAmt)) * -1;
            h.Qty = doc.Line.Where(o => o.ItemTypeID != "DISCOUNT" && o.IsActive && o.Status != "K-REJECT").Sum(o => o.Qty);
            h.CountLine = doc.Line.Where(o => o.IsActive && o.Status != "K-REJECT").Count();
            h.NetTotalAmt = doc.Line.Where(o => o.IsActive && o.Status != "K-REJECT").Sum(o => o.TotalAmt);
            h.NetTotalVatAmt = doc.Line.Where(o => o.IsActive && o.Status != "K-REJECT").Sum(o => o.VatAmt);
            h.NetTotalAmtIncVat = doc.Line.Sum(o => o.TotalAmtIncVat);
            h.NetTotalAfterRound = h.NetTotalAmtIncVat - GetDecimalPart(h.NetTotalAmtIncVat);
            h.NetDiff = GetDecimalPart(h.NetTotalAmtIncVat);


            //copy head 2 line payment
            foreach (var l in doc.Payment) {
                l.BillID = h.BillID;
                l.INVID = h.INVID;
                l.FINVID = h.FINVID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.BillAmt = h.NetTotalAmtIncVat;
                l.RoundAmt = h.NetTotalAfterRound;
                if (l.GetAmt >= l.RoundAmt) {//รับมากกว่ายอดเต็ม
                    l.PayAmt = l.RoundAmt;
                } else {//รับน้อยกว่ายอดเต็ม
                    l.PayAmt = l.GetAmt;
                }
            }




            //cal payment
            var change = doc.Payment.Sum(o => o.GetAmt) - Convert.ToDecimal(h.NetTotalAfterRound);
            var payCash = doc.Payment.Where(o => o.PaymentType == "CASH").FirstOrDefault();
            if (payCash != null) {
                payCash.ChangeAmt = change >= 0 ? change : 0;
            }



            //cal payby
            List<string> otherPayTypeInExcept = new List<string> { "CASH", "CREDIT" };  //ONLINE , TRANSFER , CREDIT , CASH , OTHER
            h.PayByOther = doc.Payment.Where(o => !otherPayTypeInExcept.Contains(o.PaymentType)).Sum(o => o.PayAmt);
            h.PayByCash = doc.Payment.Where(o => o.PaymentType == "CASH").Sum(o => o.PayAmt);
            h.PayByCredit = doc.Payment.Where(o => o.PaymentType == "CREDIT").Sum(o => o.PayAmt);
            h.PayTotalAmt = doc.Payment.Sum(o => o.PayAmt);


            return doc;
        }

        public static decimal GetDecimalPart(decimal number) {
            decimal result = (number - Math.Truncate(number));
            return result;
        }




        public static I_POSSaleSet AddItem(I_POSSaleSet doc, decimal qty, decimal discamt = 0) {

            var h = doc.Head;
            if (doc.SelectItem.TypeID == "DISCOUNT") {
                doc.Line.RemoveAll(o => o.ItemTypeID == "DISCOUNT");
            }


            doc.LineActive = doc.Line.Where(o => o.ItemID == doc.SelectItem.ItemID).FirstOrDefault();
            if (doc.LineActive == null) {//เพิ่มรหัสครั้งแรก
                doc.LineActive = NewLine(POSSaleService.DocSet);
                doc.LineActive.Qty = 1;
                doc.Line.Add(doc.LineActive);
            } else {
                if (qty == -1) {//ส่ง -1 คือให้ระบบ บวกเพิ่ม 1 ชิ้น
                    doc.LineActive.Qty = doc.LineActive.Qty + 1;
                } else {//ถ้าเป็นตัวเลขอื่นคือให้ replace ทับเลย
                    doc.LineActive.Qty = qty;
                }

            }

            doc.LineActive.ItemID = doc.SelectItem.ItemID;
            doc.LineActive.ItemName = doc.SelectItem.Name;
            doc.LineActive.ItemTypeID = doc.SelectItem.TypeID;
            doc.LineActive.ItemCateID = doc.SelectItem.CateID;
            doc.LineActive.ItemGroupID = doc.SelectItem.GroupID;
            doc.LineActive.IsStockItem = doc.SelectItem.IsStockItem;
            doc.LineActive.Weight = doc.SelectItem.Weight;
            doc.LineActive.WUnit = doc.SelectItem.WUnit;
            doc.LineActive.Unit = doc.SelectItem.Unit;

            if (doc.SelectItem.PriceTaxCondType == "INC VAT") {
                doc.LineActive.Price = Math.Round(doc.SelectItem.Price * 100 / (100 + h.VatRate), 6);
                doc.LineActive.PriceIncVat = doc.SelectItem.Price;
            }
            if (doc.SelectItem.PriceTaxCondType == "EXC VAT") {
                doc.LineActive.Price = doc.SelectItem.Price;
                doc.LineActive.PriceIncVat = Math.Round(doc.SelectItem.Price * (100 + h.VatRate) / 100, 6);
            }



            if (doc.SelectItem.TypeID == "DISCOUNT") {
                doc.LineActive.Qty = 1;//ถ้าเป็น discount ให้ replace จำนวนเป็น 1
                doc.LineActive.LineNum = 9999;

                if (doc.LineActive.ItemID == "DISCPER01") {//ลดเป็น percent
                    doc.LineActive.ItemName = "ส่วนลด " + discamt + "%";

                    doc.LineActive.DiscCalBy = "P";
                    doc.LineActive.DiscPer = discamt;
                    doc.LineActive.DiscAmt = 0;
                    doc.LineActive.VatTypeID = "NONVAT";
                } else {//ลดเป็น amount
                    doc.LineActive.ItemName = "ส่วนลด " + discamt + "฿";
                    //doc.LineActive.BaseTotalAmt = discamt * -1;
                    doc.LineActive.DiscCalBy = "A";
                    doc.LineActive.DiscPer = 0;
                    doc.LineActive.DiscAmt = discamt;
                    doc.LineActive.Price = discamt;
                    doc.LineActive.VatTypeID = "NONVAT";
                }
            }



            doc = POSSaleService.CalDocSet(doc);

            return doc;
        }

        public static I_POSSaleSet DeleteItem(I_POSSaleSet doc, int lineNum) {
            doc.Line.RemoveAll(o => o.LineNum == lineNum);
            doc = CalDocSet(doc);
            return doc;
        }

        public static I_POSSaleSet AddPayment(I_POSSaleSet doc, string paymentType, decimal getAmt) {

            doc = CalDocSet(doc);
            doc.Payment.RemoveAll(o => o.PaymentType == paymentType);
            doc.PaymentActive = NewPayment(paymentType);
            doc.PaymentActive.GetAmt = getAmt;
            doc.Payment.Add(doc.PaymentActive);
            doc = CalDocSet(doc);
            return doc;
        }
        public static void NewBillFromPreviousBill() {
            string comId = POSSaleService.DocSet.Head.ComID;
            string shiptoLoc = POSSaleService.DocSet.Head.ShipToLocID;
            string macno = POSSaleService.DocSet.Head.MacNo;
            bool isVatRegister = Convert.ToBoolean(POSSaleService.DocSet.Head.IsVatRegister);
            DateTime tranDate = POSSaleService.DocSet.Head.BillDate;

            POSSaleService.DocSet = POSSaleService.NewTransaction();
            POSSaleService.DocSet.Head.ComID = comId;
            POSSaleService.DocSet.Head.IsVatRegister = isVatRegister;
            POSSaleService.DocSet.Head.ShipToLocID = "";
            POSSaleService.DocSet.Head.BillDate = tranDate;
            POSSaleService.DocSet.Head.MacNo = macno;
            POSSaleService.Menu = POSItemService.ListMenuItem(POSSaleService.DocSet.Head.ComID, POSSaleService.DocSet.Head.ShipToUsePrice);

            if (POSSaleService.DocSet.Head.IsVatRegister == true) {
                POSSaleService.DocSet.Head.VatRate = LoginService.LoginInfo.CurrentVatRate;
            }
        }
        public static I_POSSaleSet NewTransaction() {
            I_POSSaleSet n = new I_POSSaleSet();
            n.Head = NewHead();
            n.Line = new List<POS_SaleLine>();
            n.LineActive = new POS_SaleLine();
            n.Payment = new List<POS_SalePayment>();
            n.PaymentActive = new POS_SalePayment();
            n.Log = new List<TransactionLog>();
            return n;
        }

        public static POS_SaleHead NewHead() {
            POS_SaleHead n = new POS_SaleHead();

            n.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            n.ComID = "";
            n.BillID = "";
            n.INVID = "";
            n.FINVID = "";
            n.DocTypeID = "SALE";
            n.MacNo = LoginService.LoginInfo.CurrentMacNo;
            n.MacTaxNo = "";
            n.BillDate = LoginService.LoginInfo.CurrentTransactionDate;
            n.BillRefID = "";
            n.INVPeriod = "";
            n.ReasonID = "";
            n.TableID = "T-000";
            n.TableName = "กลับบ้าน";
            n.CustomerID = "";
            n.CustomerName = "";
            n.CustAccGroup = "";
            n.CustTaxID = "";
            n.CustBranchID = "";
            n.CustBranchName = "";
            n.CustAddr1 = "";
            n.CustAddr2 = "";
            n.IsVatRegister = false;
            n.POID = "";
            n.ShipToLocID = "";
            n.ShipToLocName = "";
            n.ShipToUsePrice = "";
            n.ShipToAddr1 = "";
            n.ShipToAddr2 = "";
            n.SalesID1 = LoginService.LoginInfo.CurrentUser;
            n.SalesID2 = "";
            n.SalesID3 = "";
            n.SalesID4 = "";
            n.SalesID5 = "";
            n.ContactTo = "";
            n.Currency = "";
            n.RateExchange = 1;
            n.Currency = "THB";
            n.RateBy = "";
            n.RateDate = LoginService.LoginInfo.CurrentTransactionDate;
            n.CreditTermID = "";
            n.Qty = 0;
            n.BaseNetTotalAmt = 0;
            n.NetTotalAmt = 0;
            n.NetTotalVatAmt = 0;
            n.NetTotalAmtIncVat = 0;
            n.OntopDiscPer = 0;
            n.OntopDiscAmt = 0;
            n.ItemDiscAmt = 0;
            n.DiscCalBy = "";
            n.LineDisc = 0;
            n.VatRate = 0;
            n.VatTypeID = "";
            n.IsVatInPrice = false;
            n.NetTotalAfterRound = 0;
            n.NetDiff = 0;
            n.PayByCash = 0;
            n.PayByOther = 0;
            n.PayTotalAmt = 0;
            n.PayMethodCount = 0;
            n.CountPay = 0;
            n.Remark1 = "";
            n.Remark2 = "";
            n.RemarkCancel = "";
            n.LocID = "";
            n.SubLocID = "";
            n.CountLine = 0;
            n.IsLink = true;
            n.LinkBy = "OnWeb";
            n.LinkDate = DateTime.Now;
            n.IsRecalStock = false;
            n.Status = "OPEN";
            n.IsPrint = false;
            n.PrintDate = null;
            n.CreatedByApp = "webv1";
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;


            return n;
        }

        public static POS_SaleLine NewLine(I_POSSaleSet doc) {
            POS_SaleLine n = new POS_SaleLine();


            n.RComID = "";
            n.ComID = "";
            n.MacNo = "";
            n.LineNum = GenLineNum(doc);
            n.LineUnq = Guid.NewGuid().ToString().Substring(0, 10);

            n.RefLineUnq = "";
            n.RefLineNum = 0;
            n.BillID = "";
            n.INVID = "";
            n.FINVID = "";
            n.BelongToLineNum = "";
            n.DocTypeID = "";
            n.BillDate = DateTime.Now.Date;
            n.BillRefID = "";
            n.CustID = "";
            n.Barcode = "";
            n.ItemID = "";
            n.ItemName = "";
            n.ItemTypeID = "";
            n.ItemCateID = "";
            n.ItemGroupID = "";
            n.IsStockItem = true;
            n.TableID = "";
            n.TableName = "";
            n.ShipToLocID = "";
            n.ShipToLocName = "";
            n.KitchenFinishCount = 0;
            n.Weight = 0;
            n.WUnit = "";
            n.Unit = "";
            n.Qty = 0;
            n.Cost = 0;
            n.Price = 0;
            n.PriceIncVat = 0;
            n.BaseTotalAmt = 0;
            n.TotalAmt = 0;
            n.VatAmt = 0;
            n.TotalAmtIncVat = 0;
            n.VatRate = 0;
            n.VatTypeID = "";
            n.OntopDiscAmt = 0;
            n.OntopDiscPer = 0;
            n.DiscPer = 0;
            n.DiscAmt = 0;
            n.DiscCalBy = "";
            n.IsFree = false;
            n.ShareGpPer = 0;
            n.IsOntopItem = false;
            n.PromotionID = "";
            n.PatternID = "";
            n.PaternValue = "";
            n.MatchingNumber = 1;
            n.ProPackCode = "";
            n.IsProCompleted = false;
            n.LocID = "";
            n.SubLocID = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.BatchNo = "";
            n.Remark = "";
            n.Memo = "";
            n.ImgUrl = "";
            n.Sort = n.LineNum;
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedDate = null;
            n.IsLineActive = true;
            n.Status = "OK";
            n.IsActive = true;
            return n;
        }
        public static POS_SalePayment NewPayment(string paymentType) {
            POS_SalePayment n = new POS_SalePayment();
            n.RComID = "";
            n.ComID = "";
            n.BillID = "";
            n.PaymentType = paymentType;
            n.LineUnq = Guid.NewGuid().ToString().Substring(0, 10);

            n.INVID = "";
            n.FINVID = "";
            n.BillAmt = 0;
            n.RoundAmt = 0;
            n.GetAmt = 0;
            n.PayAmt = 0;
            n.ChangeAmt = 0;
            n.CreatedDate = DateTime.Now;
            n.ModifiedDate = null;
            n.IsLineActive = true;
            n.IsActive = true;
            return n;
        }

        public static int GenLineNum(I_POSSaleSet doc) {
            var next = 0;
            if (doc.Line.Count > 0) {
                next = doc.Line.Max(o => o.LineNum);
            }

            next = next + 10;
            return next;
        }
        public static I_FilterSet NewFilterSet() {
            I_FilterSet n = new I_FilterSet();
            n.Begin = DateTime.Now.Date;
            n.End = DateTime.Now.Date;
            n.Search = "";
            n.Company = "";
            n.ShipTo = "";
            n.MacNo = "";
            n.Table = "";
            n.ShowActive = true;
            return n;
        }


        public static I_BasicResult CalStock(POS_SaleHead h) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //var h = doc.Head;
                using (GAEntities db = new GAEntities()) {
                    if (h.INVID != "") {
                        db.SP_CalStkSaleMove(h.BillID, h.DocTypeID, h.RComID, h.ComID);
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

        public static I_BasicResult RefreshBill(DateTime begin, DateTime end) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                //var h = doc.Head;
                using (GAEntities db = new GAEntities()) {
                    var bills = db.POS_SaleHead.Where(o => o.IsActive
                                                             && o.INVID != ""
                                                             && (o.BillDate >= begin && o.BillDate <= begin)
                                                             && (o.NetTotalAfterRound != o.PayTotalAmt)
                                                             && o.RComID == rcom
                                                            ).Select(o => o.BillID).ToList();
                    r.Message2 = "Found Missing bill " + bills.Count().ToString("n0") + " for recalculate.";
                    foreach (var b in bills) {
                        var doc = GetDocSet(b);
                        var rr = POSSaleService.Save(doc, false);

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



    }
}