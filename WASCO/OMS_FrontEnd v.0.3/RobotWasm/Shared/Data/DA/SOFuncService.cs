using RobotWasm.Shared.Data.GaDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Order;

namespace RobotWasm.Shared.Data.DA {
    public class SOFuncService {
       
        #region CalDocSet
        public static I_SODocSet CalDocSet(I_SODocSet input) {
            input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var head = input.Head;
                //var line = input.Line;
                //var lot = input.Lot;
                if (input.Line.Count > 0) {
                    foreach (var l in input.Line) {
                        //1.cal line base total amt
                        //l.BaseTotalAmt = l.Qty * l.Price;
                        l.Price = Math.Round((l.PriceIncVat * 100 / (100 + l.VatRate)), 3, MidpointRounding.AwayFromZero);
                        l.BaseTotalAmt = l.Qty * l.Price;
                        l.BaseTotalAmtIncVat = l.Qty * l.PriceIncVat;
                        //2. cal disc amt
                        if (l.DiscCalBy == "P") {
                            //l.DiscAmt = (l.BaseTotalAmt * l.DiscPer) / 100;
                            l.DiscPPU = Math.Round(((l.BaseTotalAmtIncVat * l.DiscPer) / 100) / l.Qty, 3, MidpointRounding.AwayFromZero); ;
                            //l.DiscAmtIncVat = (l.BaseTotalAmtIncVat * l.DiscPer) / 100;
                            l.DiscAmtIncVat = l.DiscPPU * l.Qty;
                            l.DiscAmt = Math.Round((l.DiscAmtIncVat * 100) / (100 + l.VatRate), 3, MidpointRounding.AwayFromZero);
                        }
                        //3. cal disc per
                        if (l.DiscCalBy == "A") {
                            l.DiscPer = 0;
                            //if (l.BaseTotalAmt != 0) {
                            //    l.DiscPer = (100 * l.DiscAmt) / l.BaseTotalAmt;
                            //}
                            if (l.BaseTotalAmtIncVat != 0) {
                                //l.DiscPPUIncVat = (l.DiscPPUIncVat * l.Qty);
                                l.DiscPPU = Math.Round((l.DiscPPUIncVat * 100) / (100 + l.VatRate), 3, MidpointRounding.AwayFromZero); ;
                                l.DiscAmtIncVat = (l.DiscPPUIncVat * l.Qty);
                                l.DiscAmt = Math.Round((l.DiscAmtIncVat * 100) / (100 + l.VatRate), 3, MidpointRounding.AwayFromZero);
                                l.DiscPer = Math.Round((100 * l.DiscAmtIncVat) / l.BaseTotalAmtIncVat, 3, MidpointRounding.AwayFromZero); ;
                            }
                        }
                    }
                }




                var line_v = input.Line.Where(o => o.ItemTypeID != "MISC");

                //5. cal head base total amt
                //var head_amt_aft_dis = line_v.Sum(o => o.BaseTotalAmt) - line_v.Sum(o => o.DiscAmt);
                //head.BaseNetTotalAmt = head_amt_aft_dis;

                //6. cal head disc amt
                if (head.DiscCalBy == "P") {
                    //head.OntopDiscAmt = (head.BaseNetTotalAmt * head.OntopDiscPer) / 100;
                    head.OntopDiscAmtIncVat = (head.BaseNetTotalAmtIncVat * head.OntopDiscPer) / 100;
                }
                //7. cal head disc per
                if (head.DiscCalBy == "A") {
                    //if (head.BaseNetTotalAmt != 0) {
                    //    head.OntopDiscPer = (100 * head.OntopDiscAmt) / head.BaseNetTotalAmt;
                    //}
                    if (head.BaseNetTotalAmtIncVat != 0) {
                        head.OntopDiscPer = (100 * head.OntopDiscAmtIncVat) / head.BaseNetTotalAmtIncVat;
                    }
                }
                if (input.Line.Count > 0) {
                    foreach (var l in input.Line) {
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
                        if (l.Status != "REJECT") {
                            l.OntopDiscPer = head.OntopDiscPer;
                            //l.OntopDiscAmt = Math.Round(head.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
                            l.OntopDiscAmtIncVat = Math.Round(head.OntopDiscAmtIncVat, 3, MidpointRounding.AwayFromZero);

                            //8.cal line disc weight ontop percent & amt

                            //if (head.BaseNetTotalAmt != 0) {
                            //    l.OntopDiscAmt = Math.Round(((l.BaseTotalAmt - l.DiscAmt) * head.OntopDiscAmt) / head.BaseNetTotalAmt, 3, MidpointRounding.AwayFromZero);
                            //    l.OntopDiscPer = (l.BaseTotalAmt - l.DiscAmt) == 0 ? 0 : (100 * l.OntopDiscAmt) / (l.BaseTotalAmt - l.DiscAmt);
                            //}
                            if (head.BaseNetTotalAmtIncVat != 0) {
                                l.OntopDiscAmtIncVat = Math.Round(((l.BaseTotalAmtIncVat - l.DiscAmtIncVat) * head.OntopDiscAmtIncVat) / head.BaseNetTotalAmtIncVat, 3, MidpointRounding.AwayFromZero);
                                l.OntopDiscPer = (l.BaseTotalAmtIncVat - l.DiscAmtIncVat) == 0 ? 0 : (100 * l.OntopDiscAmtIncVat) / (l.BaseTotalAmtIncVat - l.DiscAmtIncVat);
                            }
                        }
                        //9 cal line total amt 
                        //l.TotalAmt = Math.Round(l.BaseTotalAmt - l.DiscAmt - l.OntopDiscAmt, 3, MidpointRounding.AwayFromZero);
                        l.TotalAmtIncVat = Math.Round(l.BaseTotalAmtIncVat - l.DiscAmtIncVat - l.OntopDiscAmtIncVat, 3, MidpointRounding.AwayFromZero);
                        //10.cal line vat amt
                        //l.VatAmt = Math.Round((l.TotalAmt * l.VatRate) / 100, 2, MidpointRounding.AwayFromZero);
                        l.VatAmt = Math.Round((l.TotalAmtIncVat * l.VatRate) / (100 + l.VatRate), 2, MidpointRounding.AwayFromZero);
                        //11.cal line nettotal amt inc vat
                        //l.TotalAmtIncVat = Math.Round(l.TotalAmt + l.VatAmt, 2, MidpointRounding.AwayFromZero);
                        l.TotalAmt = Math.Round(l.TotalAmtIncVat - l.VatAmt, 2, MidpointRounding.AwayFromZero);
                        //12.set status discount approve

                        if (l.DiscAmtIncVat > 0) {
                            if (l.DiscApproveDate == null) {
                                l.Status = "WAIT";
                            }
                        } else {
                            l.Status = "OK";
                        }
                    }
                }
                if (input.Lot.Count > 0) {
                    foreach (var l in input.Lot) {
                        var ll = line_v.Where(o => o.LineNum == l.LineLineNum).FirstOrDefault();
                        if (ll != null) {
                            l.Status = ll.Status;
                            l.CustID = head.CustID;
                            l.OrdDate = head.OrdDate;
                            l.OrdID = head.OrdID;
                            l.DocTypeID = head.DocTypeID;
                            l.RComID = head.RComID;
                            l.ComID = head.ComID;
                        }


                    }
                }


                //13.copy line to head 
                head.ItemDiscAmt = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.DiscAmt), 2, MidpointRounding.AwayFromZero);
                head.ItemDiscAmtIncVat = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.DiscAmtIncVat), 2, MidpointRounding.AwayFromZero);
                head.BaseNetTotalAmtIncVat = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.BaseTotalAmtIncVat), 2, MidpointRounding.AwayFromZero);
                head.NetTotalAmt = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.TotalAmt), 2, MidpointRounding.AwayFromZero);
                head.NetTotalVatAmt = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.VatAmt), 2, MidpointRounding.AwayFromZero);
                head.NetTotalAmtIncVat = Math.Round(line_v.Where(o => o.Status != "REJECT").Sum(o => o.TotalAmtIncVat), 2, MidpointRounding.AwayFromZero);
                var count_pending_approve = line_v.Where(o => o.Status == "WAIT").Count();
                if (head.IsActive == false) {
                    head.Status = "CANCEL";
                } else {
                    if (count_pending_approve > 0) {
                        head.Status = "WAIT";
                    } else {
                        if (head.Status != "CLOSED") {
                            head.Status = "OPEN";
                        }
                    }
                }



                head.Qty = line_v.Sum(o => o.Qty);
                head.CountLine = line_v.Where(o => o.Status != "REJECT").Count();
            } catch (Exception ex) {
                input.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    input.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    input.OutputAction.Message1 = ex.Message;
                }
            }

            return input;
        }
        #endregion
        #region New transaction


        public static I_SODocSet NewTransaction(string rcom, string com, string doctype, string created_by, string created_fullname) {
            I_SODocSet n = new I_SODocSet();
            n.Head = NewHead(rcom, com, doctype, created_by, created_fullname);
            n.Line = new List<vw_OSOLine>();
            n.Lot = new List<vw_OSOLot>();
            n.LineActive = NewLine(rcom, com, n);
            n.LotActive = new vw_OSOLot();
            n.Log = new List<TransactionLog>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return n;
        }

        public static vw_OSOHead NewHead(string rcom, string com, string doctype, string created_by, string creaed_fullname) {
            vw_OSOHead n = new vw_OSOHead();
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
            n.CustName = "";
            n.CustAddr1 = "";
            n.CustAddr2 = "";
            n.CustMobile = "";
            n.CustEmail = "";
            n.BrandID = "";
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
            n.SalesID1 = created_by;
            n.SalesName = creaed_fullname;
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
            n.BaseNetTotalAmtIncVat = 0;
            n.ItemDiscAmt = 0;
            n.ItemDiscAmtIncVat = 0;
            n.OntopDiscPer = 0;
            n.OntopDiscAmt = 0;
            n.OntopDiscAmtIncVat = 0;
            n.DiscCalBy = "A";
            n.VatRate = 7;
            n.VatTypeID = "VAT7";
            n.Remark1 = "";
            n.Remark2 = "";

            n.PaymentMemo = "";

            n.IsPrint = false;
            n.PrintDate = null;
            n.IsLink = false;
            n.LinkDate = null;
            n.Status = "OPEN";
            n.Source = "BACK";
            n.CreatedBy = created_by;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static vw_OSOLine NewLine(string rcom, string com, I_SODocSet input) {
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
            n.BaseTotalAmtIncVat = 0;
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
            n.OntopDiscAmtIncVat = 0;
            n.DiscPer = 0;
            n.DiscAmt = 0;
            n.DiscAmtIncVat = 0;
            n.DiscCalBy = "A";
            n.LocID = "";
            n.SubLocID = "";
            n.PackagingNo = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.BatchNo = "";
            n.PointID = "";
            n.PointName = "";
            n.IsSpecialPrice = false;
            n.Remark1 = "";
            n.Remark2 = "";
            n.DiscApproveBy = "";
            n.DiscApproveDate = null;
            n.Status = "NEW";
            n.Sort = 1;
            n.ImageUrl = "";
            n.ProID = "";
            n.ProName = "";
            n.PatternID = "";
            n.PageBreak = false;
            n.IsComplete = false;
            n.IsActive = true;
            n.ImageUrl = "/img/watch2.png";
            return n;
        }

        public static vw_OSOLot NewLot(string rcom, string com, int line, I_SODocSet input) {
            vw_OSOLot n = new vw_OSOLot();
            n.RComID = "";
            n.ComID = "";
            n.OrdID = "";
            n.LineLineNum = line;
            n.LineNum = GenLotLineNum(input);
            n.DocTypeID = "";
            n.OrdDate = DateTime.Now.Date;
            n.CustID = "";
            n.ItemID = "";
            n.IsStockItem = true;
            n.Qty = 0;
            n.QtyBal = 0;
            n.Unit = "";
            n.LocID = "";
            n.LotNo = "";
            n.SerialNo = "";
            n.Status = "NEW";
            n.IsActive = true;
            return n;
        }
        public static OSOHead Convert2SOHhead(vw_OSOHead i) {
            OSOHead n = new OSOHead();
            n.ID = i.ID;
            n.RComID = i.RComID;
            n.ComID = i.ComID;
            n.OrdID = i.OrdID;
            n.OrdDate = i.OrdDate;
            n.DocTypeID = i.DocTypeID;
            n.INVID = i.INVID;
            n.INVDate = i.INVDate;
            n.CustID = i.CustID;
            n.CustName = i.CustName;
            n.CustAddr1 = i.CustAddr1;
            n.CustAddr2 = i.CustAddr2;
            n.CustTaxID = i.CustTaxID;
            n.CustBrnID = i.CustBrnID;
            n.CustBrnName = i.CustBrnName;
            n.CustMobile = i.CustMobile;
            n.CustEmail = i.CustEmail;
            n.BrandID = i.BrandID;
            n.POID = i.POID;
            n.PODate = i.PODate;
            n.RefDocID = i.RefDocID;
            n.AccGroupID = i.AccGroupID;
            n.BillToCustID = i.BillToCustID;
            n.BillAddr1 = i.BillAddr1;
            n.BillAddr2 = i.BillAddr2;
            n.ShipFrLocID = i.ShipFrLocID;
            n.ShipFrSubLocID = i.ShipFrSubLocID;
            n.SalesID1 = i.SalesID1;
            n.SalesID2 = i.SalesID2;
            n.Currency = i.Currency;
            n.RateExchange = i.RateExchange;
            n.RateBy = i.RateBy;
            n.RateDate = i.RateDate;
            n.TermID = i.TermID;
            n.PayDueDate = i.PayDueDate;
            n.PaymentType = i.PaymentType;
            n.PaymentMemo = i.PaymentMemo;
            n.CountLine = i.CountLine;
            n.Qty = i.Qty;
            n.QtyShip = i.QtyShip;
            n.QtyInvoice = i.QtyInvoice;
            n.QtyReturn = i.QtyReturn;
            n.QtyShipPending = i.QtyShipPending;
            n.QtyInvoicePending = i.QtyInvoicePending;
            n.AmtShipPending = i.AmtShipPending;
            n.AmtInvoice = i.AmtInvoice;
            n.AmtInvoicePending = i.AmtInvoicePending;
            n.BaseNetTotalAmt = i.BaseNetTotalAmt;
            n.BaseNetTotalAmtIncVat = i.BaseNetTotalAmtIncVat;
            n.NetTotalAmt = i.NetTotalAmt;
            n.NetTotalVatAmt = i.NetTotalVatAmt;
            n.NetTotalAmtIncVat = i.NetTotalAmtIncVat;
            n.ItemDiscAmtIncVat = i.ItemDiscAmtIncVat;
            n.ItemDiscAmt = i.ItemDiscAmt;
            n.OntopDiscPer = i.OntopDiscPer;
            n.OntopDiscAmt = i.OntopDiscAmt;
            n.OntopDiscAmtIncVat = i.OntopDiscAmtIncVat;
            n.DiscCalBy = i.DiscCalBy;
            n.VatRate = i.VatRate;
            n.VatTypeID = i.VatTypeID;
            n.Remark1 = i.Remark1;
            n.Remark2 = i.Remark2;
            n.PName=i.PName;
            n.AName = i.AName;
            n.TName=i.TName;
            n.IsPrint = i.IsPrint;
            n.PrintDate = i.PrintDate;
            n.ShipID = i.ShipID;
            n.ShipDate = i.ShipDate;
            n.IsLink = i.IsLink;
            n.LinkDate = i.LinkDate;
            n.DiscStatus = i.DiscStatus;
            n.IsLockPrice = i.IsLockPrice;
            n.Lat = i.Lat;
            n.Lon = i.Lon;
            n.Status = i.Status;
            n.Source = i.Source;
            n.CreatedBy = i.CreatedBy;
            n.CreatedDate = i.CreatedDate;
            n.ModifiedBy = i.ModifiedBy;
            n.ModifiedDate = i.ModifiedDate;
            n.IsActive = i.IsActive;
            return n;
        }
        public static List<OSOLine> Convert2SOLine(List<vw_OSOLine> input) {
            List<OSOLine> output = new List<OSOLine>();
            foreach (var i in input) {
                OSOLine n = new OSOLine();
                n.ID = i.ID;
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
                n.BaseTotalAmt = i.BaseTotalAmt;
                n.BaseTotalAmtIncVat = i.BaseTotalAmtIncVat;
                n.Price = i.Price;
                n.PriceIncVat = i.PriceIncVat;
                n.TotalAmt = i.TotalAmt;
                n.VatAmt = i.VatAmt;
                n.TotalAmtIncVat = i.TotalAmtIncVat;
                n.VatRate = i.VatRate;
                n.VatTypeID = i.VatTypeID;
                n.OntopDiscAmt = i.OntopDiscAmt;
                n.OntopDiscPer = i.OntopDiscPer;
                n.OntopDiscAmtIncVat = i.OntopDiscAmtIncVat;
                n.DiscPer = i.DiscPer;
                n.DiscAmt = i.DiscAmt;
                n.DiscAmtIncVat = i.DiscAmtIncVat;
                n.DiscCalBy = i.DiscCalBy;
                n.DiscPPU = i.DiscPPU;
                n.DiscPPUIncVat = i.DiscPPUIncVat;
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
                n.DiscApproveBy = i.DiscApproveBy;
                n.DiscApproveDate = i.DiscApproveDate;
                n.IsComplete = i.IsComplete;
                n.IsSpecialPrice = i.IsSpecialPrice;
                n.PatternID = i.PatternID;
                n.ProID= i.ProID;
                n.ProName= i.ProName;
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
                n.ID = i.ID;
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

        public static I_SODocSet AddLine(string rcom, string com, I_SODocSet input) {
            ClearPendingLine(input);
            input.Line.Add(NewLine(rcom, com, input));
            input.LineActive = input.Line.Where(o => o.Status == "NEW").OrderByDescending(o => o.LineNum).FirstOrDefault();
            return input;
        }

        public static I_SODocSet AddLot(string rcom, string com, I_SODocSet input) {
            input.Lot.Add(NewLot(rcom, com, input.LineActive.LineNum, input));
            return input;
        }

        public static void ClearPendingLine(I_SODocSet input) {
            try {
                var r1 = input.Line.RemoveAll(o => o.Status == "NEW");
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static void ClearPendingLot(I_SODocSet input) {
            try {
                var r1 = input.Lot.RemoveAll(o => o.Status == "NEW");
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
        }

        public static int GenLineNum(I_SODocSet input) {
            int result = 10;
            try {
                var max_linenum = input.Line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenLotLineNum(I_SODocSet input) {
            int result = 10;
            try {
                var max_linenum = input.Lot.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }
        public static int GenLineSort(I_SODocSet input) {
            int result = 1;
            try {
                var max_sort = input.Line.Max(o => o.Sort);
                result = Convert.ToInt32(max_sort) + 1;
            } catch { }
            return result;
        }

        #endregion





    }
}
