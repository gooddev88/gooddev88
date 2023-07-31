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
using Robot.Data.DA.Login;
using System.Data.SqlClient;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Telerik.SvgIcons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using Robot.Tools;
using System.IO.Pipelines;
using System.Globalization;
using System.Net.Sockets;
using System.Drawing;
using OfficeOpenXml.Style;
using Robot.Data.DA.HR;

namespace Robot.Data.DA.Stock {

    public class StkTransferService {

        public static string sessionActiveId = "activeadjid";
        public static string sessionSubBackUrl = "supbackurl";
        public class I_StkTransferSet {
            public StkTransferHead Head { get; set; }
            public List<StkTransferLine> Line { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class I_StkTransferFiterSet {
            public String Rcom { get; set; }
            public String Com { get; set; }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String DocType { get; set; }
            public String SearchBy { get; set; }
            public String SearchText { get; set; }
            public String Status { get; set; }
            public bool ShowActive { get; set; }
        }

        public string PreviousPageUrl = "";
        public string PreviousSubPageUrl = "";

        public I_StkTransferSet DocSet { get; set; }
        ISessionStorageService sessionStorage;

        public StkTransferService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;
        }

        #region Query Transaction

        public I_StkTransferSet GetDocSet(string docid, string rcom, string com) {
            I_StkTransferSet n = NewTransaction(rcom, com);
            try {
                using (GAEntities db = new GAEntities()) {
                    n.Head = db.StkTransferHead.Where(o => o.TRID == docid && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                    n.Line = db.StkTransferLine.Where(o => o.TRID == docid && o.RComID == rcom && o.ComID == com).ToList();
                    n.Log = TransactionService.ListLogByDocID(docid, rcom, com);
                }
            } catch (Exception ex) {
                n.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return n;
        }

        public static List<StkTransferHead> ListDoc(I_StkTransferFiterSet f) {
            List<StkTransferHead> result = new List<StkTransferHead>();
            try {
                f.Status = f.Status == null ? "" : f.Status;
                using (GAEntities db = new GAEntities()) {
                    if (string.IsNullOrEmpty(f.SearchText)) {
                        result = db.StkTransferHead.Where(o => o.TRDate >= f.DateFrom && o.TRDate <= f.DateTo
                                                      && o.ComID == f.Com
                                                      && o.RComID == f.Rcom
                                                      && (o.Status == f.Status || f.Status == "")
                                                      && o.IsActive == true
                                      ).OrderByDescending(o => o.CreatedDate).ToList();
                    } else {
                        result = db.StkTransferHead.Where(o => (
                                                        o.ComID.Contains(f.SearchText)
                                                        || o.TRID.Contains(f.SearchText)
                                                        || o.RComID.Contains(f.SearchText)
                                                        || o.ComID.Contains(f.SearchText)
                                                        || o.Description.Contains(f.SearchText)
                                                        || f.SearchText == ""
                                                      )
                                                      && (o.Status == f.Status || f.Status == "")
                                                      && o.ComID == f.Com
                                                      && o.RComID == f.Rcom
                                                      && o.IsActive == true
                                      ).OrderByDescending(o => o.CreatedDate).ToList();
                    }
                }
            } catch (Exception) {
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

        #region stock bal
        public static vw_STKBal GetStkBalByLoc(string rcom, string comId, string itemId, string locId) {
            vw_STKBal r = new vw_STKBal();
            using (GAEntities db = new GAEntities()) {
                r = db.vw_STKBal.Where(o =>
                o.RComID == rcom
                && o.ItemID == itemId
                && o.ComID == comId
                && (o.LocID == locId || locId == "")
                ).FirstOrDefault();
                return r;
            }
        }



        #endregion

        #region Save

        public static I_BasicResult Save(I_StkTransferSet input, string rcom, string com, string action) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                input = CalDocSet(input);
                var head = input.Head;

                using (GAEntities db = new GAEntities()) {
                    if (action == "insert") {//new transaction
                        input = checkDupID(input, rcom, com);
                        if (input.OutputAction.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = input.OutputAction.Message1;
                        }

                        db.StkTransferHead.Add(input.Head);
                        db.StkTransferLine.AddRange(input.Line);
                        db.SaveChanges();
                        CalStock(input);
                        if (input.NeedRunNextID) {
                            IDRuunerService.GetNewIDV2("TRANSFER", rcom, input.Head.ComID, input.Head.TRDate, true, "th");
                            input.NeedRunNextID = false;
                        }
                        TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.TRID, TableID = "STK_TRANSFER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "INSERT NEW STOCK TRANSFER" }, rcom, input.Head.ComID, input.Head.CreatedBy);
                    } else {
                        var uh = db.StkTransferHead.Where(o => o.TRID == input.Head.TRID && o.RComID == rcom && o.ComID == com).FirstOrDefault();
                        uh.TRDate = head.TRDate;
                        uh.Qty = head.Qty;
                        uh.Amt = head.Amt;
                        uh.Memo = head.Memo;
                        uh.BrandID = head.BrandID;
                        uh.LocIDFr = head.LocIDFr;
                        uh.LocIDTo = head.LocIDTo;
                        uh.Description = head.Description;
                        uh.Remark = head.Remark;
                        uh.Status = head.Status;
                        uh.ModifiedBy = head.ModifiedBy;
                        uh.ModifiedDate = DateTime.Now;

                        db.StkTransferLine.RemoveRange(db.StkTransferLine.Where(o => o.TRID == uh.TRID && o.RComID == uh.RComID && o.ComID == uh.ComID));
                        db.StkTransferLine.AddRange(input.Line);
                        db.SaveChanges();
                        CalStock(input);
                        TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.TRID, TableID = "STK_TRANSFER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "UPDATE STOCK TRANSFER" }, rcom, input.Head.ComID, input.Head.ModifiedBy);
                    }
                }
                //RefreshSet(head.OrdID, rcom, head.ComID);
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

        public static I_StkTransferSet CalDocSet(I_StkTransferSet input) {
            var h = input.Head;
            var line = input.Line;
            foreach (var l in line) {
                l.TRID = h.TRID;
                l.ComID = h.ComID;
                l.RComID = h.RComID;
                l.TRDate = h.TRDate;
                l.DocType = h.DocType;
                l.LocIDFr = h.LocIDFr;
                l.LocIDTo = h.LocIDTo;
                l.Qty = l.Qty;
                l.Amt = l.Price * l.Amt;
            }

            h.Qty = line.Sum(o => o.Qty);
            h.Amt = line.Sum(o => o.Amt);

            return input;
        }

        public static I_StkTransferSet checkDupID(I_StkTransferSet input, string rcom, string com) {
            try {
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = input.Head;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.StkTransferHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.TRID == h.TRID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            input.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GetNewIDV2("TRANSFER", rcom, input.Head.ComID, input.Head.TRDate, true, "th");
                        h.TRID = IDRuunerService.GetNewIDV2("TRANSFER", rcom, input.Head.ComID, input.Head.TRDate, false, "th")[1];
                        get_id = db.StkTransferHead.Where(o => o.RComID == h.RComID && o.ComID == h.ComID && o.TRID == h.TRID).FirstOrDefault();
                    }
                }
                CalDocSet(input);
            } catch (Exception ex) {
            }
            return input;
        }

        public static I_BasicResult NewLineByItem(I_StkTransferSet input, StkTransferLine adjLine, string item) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var oline = input.Line.Where(o => o.ItemID == item).FirstOrDefault();
                if (oline == null) {//add new stkadjline 
                    input.Line.Add(adjLine);
                } else {
                    //exist stkadjline
                    oline.Qty = adjLine.Qty;
                }
                CalDocSet(input);
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

        public static I_BasicResult SetStatusClose(I_StkTransferSet input, string updateby) {
            //OPEN / CLOSED 
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var h = db.StkTransferHead.Where(o => o.TRID == input.Head.TRID && o.RComID == input.Head.RComID && o.ComID == input.Head.ComID && o.IsActive).FirstOrDefault();
                    h.Status = "CLOSED";
                    h.ApprovedBy = updateby;
                    h.ApprovedDate = DateTime.Now;

                    db.SaveChanges();
                    CalStock(input);
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

        #region Delete / Closed
        public static I_BasicResult ClosedDoc(I_StkTransferSet input, string updateby) {
            input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.StkTransferHead.Where(o => o.TRID == input.Head.TRID && o.RComID == input.Head.RComID).FirstOrDefault();
                    var line = db.StkTransferLine.Where(o => o.TRID == input.Head.TRID && o.RComID == input.Head.RComID).ToList();
                    head.Status = "CONFIRM";
                    head.ApprovedDate = DateTime.Now;
                    head.ApprovedBy = updateby;
                    head.ModifiedBy = updateby;
                    head.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    CalStock(input);
                    TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.TRID, TableID = "STK_TRANSFER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "CONFIRM STOCK TRANSFER" }, input.Head.RComID, input.Head.ComID, updateby);
                }
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input.OutputAction;
        }

        public static I_BasicResult DeleteDoc(I_StkTransferSet input, string updateby) {
            input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.StkTransferHead.Where(o => o.TRID == input.Head.TRID && o.RComID == input.Head.RComID).FirstOrDefault();
                    var line = db.StkTransferLine.Where(o => o.TRID == input.Head.TRID && o.RComID == input.Head.RComID).ToList();
                    head.Status = "DELETED";
                    head.ModifiedBy = updateby;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {
                        l.IsActive = false;
                    }
                    db.SaveChanges();
                    CalStock(input);
                    TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.TRID, TableID = "STK_TRANSFER", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.ComID, Action = "DELETE STOCK TRANSFER" }, input.Head.RComID, input.Head.ComID, updateby);
                }
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input.OutputAction;
        }

        public static I_StkTransferSet DeleteLine(int linenum, I_StkTransferSet input) {
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

        async public void SetSessionFiterSet(I_StkTransferFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("stkadjus_fiter", json);
        }
        async public Task<I_StkTransferFiterSet> GetSessionFiterSet() {
            I_StkTransferFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("stkadjus_fiter");
            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_StkTransferFiterSet>(strdoc);
            } else {
                return null;
            }
        }

        public static I_StkTransferFiterSet NewFilterSet() {
            I_StkTransferFiterSet n = new I_StkTransferFiterSet();
            n.Rcom = "";
            n.Com = "";
            n.DateFrom = DateTime.Now.Date.AddDays(-7);
            n.DateTo = DateTime.Now.Date;
            n.Status = "";
            n.SearchBy = "DOCDATE";
            n.SearchText = "";
            n.ShowActive = true;

            return n;
        }

        public static I_StkTransferSet NewTransaction(string rcom, string com) {
            I_StkTransferSet n = new I_StkTransferSet();
            n.Head = NewHead(rcom, com);
            n.Line = new List<StkTransferLine>();
            n.NeedRunNextID = false;
            n.Log = new List<TransactionLog>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return n;
        }

        public static StkTransferHead NewHead(string rcom, string com) {
            StkTransferHead n = new StkTransferHead();

            n.TRID = "";
            n.DocType = "TRANSFER";
            n.TRDate = DateTime.Now.Date;
            n.RComID = rcom;
            n.ComID = com;
            n.LocIDFr = "";
            n.LocIDTo = "";
            n.BrandID = "";
            n.Description = "";
            n.Remark = "";
            n.Memo = "";
            n.Qty = 0;
            n.Amt = 0;
            n.ApprovedBy = "";
            n.ApprovedDate = DateTime.Now.Date;
            n.Status = "OPEN";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static StkTransferLine NewLine(I_StkTransferSet input) {
            StkTransferLine n = new StkTransferLine();
            n.ComID = input.Head.ComID;
            n.RComID = input.Head.RComID;
            n.TRID = "";
            n.DocType = "";
            n.LineNum = GenLineNum(input);
            n.TRDate = DateTime.Now.Date;
            n.LocIDFr = "";
            n.LocIDTo = "";
            n.ItemID = "";
            n.Name = "";
            n.Price = 0;
            n.Qty = 0;
            n.Amt = 0;
            n.Unit = "";
            n.Memo = "";
            n.IsActive = true;
            return n;
        }

        public static STKBal NewSTKBal() {
            STKBal n = new STKBal();
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

        public static List<vw_STKBal> ListSTKBalByBrandAndLocation(string rcom, string com, string brandid, string loc) {
            List<vw_STKBal> result = new List<vw_STKBal>();
            using (GAEntities db = new GAEntities()) {
                brandid = brandid == null ? "" : brandid;
                result = db.vw_STKBal.Where(o => o.RComID == rcom
                                                                && o.ComID == com
                                                                && (o.BrandID == brandid || brandid == "")
                                                                 && (o.LocID == loc || loc == "")
                                                                && o.IsActive == true
                                                                ).ToList();
            }
            return result;
        }

        public static List<vw_STKBal> ListSTKBalByLocation(string rcom, string com, string locid) {
            List<vw_STKBal> result = new List<vw_STKBal>();
            using (GAEntities db = new GAEntities()) {
                locid = locid == null ? "" : locid;
                result = db.vw_STKBal.Where(o => o.RComID == rcom
                                                                && o.ComID == com
                                                                && (o.LocID == locid || locid == "")
                                                                && o.IsActive
                                                                ).ToList();
            }
            return result;
        }

        public static List<StkTransferLine> STKBalConvert2AdjustLine(List<vw_STKBal> STKBal, I_StkTransferSet input) {
            var line = input.Line;
            foreach (var i in STKBal) {
                var chkExist = line.Where(o => o.ItemID == i.ItemID).FirstOrDefault();
                if (chkExist != null) {
                    continue;
                }
                StkTransferLine n = NewLine(input);
                n.ItemID = i.ItemID;
                n.Name = i.ItemName;
                n.Qty = i.BalQty;
                n.Price = i.Price;
                n.LocIDFr = i.LocID;
                n.LocIDTo = i.LocID;

                n.Unit = i.UnitID;
                input.Line.Add(n);
            }
            CalDocSet(input);

            return input.Line;
        }

        public static int GenLineNum(I_StkTransferSet input) {
            int result = 10;
            try {
                var max_linenum = input.Line.Max(o => o.LineNum);
                result = max_linenum + 10;
            } catch { }
            return result;
        }



        public static void CalStock(I_StkTransferSet input) {
            input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var h = input.Head;
                string conStr = Globals.GAEntitiesConn;
                using (var conn = new SqlConnection(conStr)) {
                    var sql = "exec [SP_CalStkTransferMove] @docid, @doctype, @rcompany, @company";
                    var values = new { docid = h.TRID, doctype = h.DocType, rcompany = h.RComID, company = h.ComID };
                    var results = conn.Query(sql, values);
                }
            } catch (Exception ex) {
                input.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    input.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    input.OutputAction.Message1 = ex.Message;
                }
            }
        }

        #endregion

        public static List<SelectOption> ListStatus() {
            return new List<SelectOption>() {
                new SelectOption(){ IsSelect = true ,Value= "OPEN", Description="OPEN" ,Sort = 1},
                new SelectOption(){ IsSelect = true ,Value = "CONFIRM", Description="CONFIRM", Sort = 2},
                new SelectOption(){ IsSelect = true ,Value = "DELETED", Description="DELETED", Sort = 3}
            };
        }

        public static MemoryStream DownItemToExcel(string rcom, string com) {
            var memoryStream = new MemoryStream();
            var ItemBal = ListSTKBalByLocation(rcom, com, "");

            using (var excelPackage = new ExcelPackage()) {
                int row_excel = 2;
                ExcelWorksheet excelWorksheet1 = excelPackage.Workbook.Worksheets.Add("ITEMS");
                ExcelRange cells = excelWorksheet1.Cells;

                cells[1, 1].Value = "ItemID"; cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 1].Style.Font.Color.SetColor(Color.Black); cells[1, 1].Style.Font.Size = 16; cells[1, 1].Style.Font.Bold = true;

                cells[1, 2].Value = "LocID"; cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 2].Style.Font.Color.SetColor(Color.Black); cells[1, 2].Style.Font.Size = 16; cells[1, 2].Style.Font.Bold = true;

                cells[1, 3].Value = "LotNo"; cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 3].Style.Font.Color.SetColor(Color.Black); cells[1, 3].Style.Font.Size = 16; cells[1, 3].Style.Font.Bold = true;

                cells[1, 4].Value = "Qty"; cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 4].Style.Font.Color.SetColor(Color.Black); cells[1, 4].Style.Font.Size = 16; cells[1, 4].Style.Font.Bold = true;

                foreach (var e in ItemBal) {
                    cells[row_excel, 1].Value = e.ItemID; cells[row_excel, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 1].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 2].Value = e.LocID; cells[row_excel, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 2].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 3].Value = ""; cells[row_excel, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 3].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 4].Value = Convert.ToDecimal(e.Price).ToString("N0"); cells[row_excel, 4].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 4].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    row_excel++;
                }

                cells[1, 1, row_excel, 11].AutoFitColumns();
                //string fileName = "Items-" + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");

                //Response.Clear();
                //Response.Buffer = true;
                //Response.Charset = "";

                //Response.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                //Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
                excelPackage.SaveAs(memoryStream);
                //memoryStream.WriteTo(Response.OutputStream);
                //Response.Flush();
                //Response.End();
            }
            return memoryStream;
        }

        public static I_BasicResult UploadItem(IFormFile myFile, I_StkTransferSet doc) {
            I_BasicResult read_result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            var NameNoExt = Path.GetFileNameWithoutExtension(myFile.FileName);
            var ExtOnly = Path.GetExtension(myFile.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFile");
            
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, myFile.FileName);

            using (var fileStream = System.IO.File.Create(filePath)) {
                myFile.CopyTo(fileStream);
            }

            using (ExcelPackage excelPackage = new ExcelPackage()) {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    excelPackage.Load(fileStream);
                }

                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[0];

                if (excelWorksheet == null) {
                    read_result.Result = "fail";
                    read_result.Message1 = "Error sheet name";
                    return read_result;
                }

                var rsm = ReadSheetItemStkTransferFile(excelWorksheet, doc);
                if (rsm.Result == "fail") {
                    read_result.Result = "fail";
                    read_result.Message1 = rsm.Message1;
                    return read_result;
                }

                CalDocSet(doc);
                //var rs = SaveBulkV2(doc.Line);
                //if (rs.Result == "fail") {
                //    read_result.Result = rs.Result;
                //    read_result.Message1 = rs.Message1;
                //}
            }
            return read_result;
        }

        public static I_BasicResult ReadSheetItemStkTransferFile(ExcelWorksheet ii, I_StkTransferSet doc) {
            //List<StkTransferLine> ListStkAdjustList = new List<StkTransferLine>();
            var result_msg = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                string error_in_row = "";
                string SheetName = ii.Name;

                int totalRows = ii.Dimension.End.Row;
                int start_row = 2;
                int end_row = totalRows;
                int row_excel = 1;

                var culture = new CultureInfo("en-US");
                List<StkTransferLine> tempList = new List<StkTransferLine>();
                row_excel = 1;//reset row
                for (row_excel = start_row; row_excel <= end_row; row_excel++) {
                    error_in_row = "";
                    var n = StkTransferService.NewLine(doc);
                    try {
                        n.ItemID = ii.Cells[row_excel, 1].Text == null ? "" : ii.Cells[row_excel, 1].Text.Trim();
                        if (n.ItemID == "") {
                            error_in_row = "ไม่พบ ItemID แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->ItemID={ii.Cells[row_excel, 1].Value.ToString()}";
                    }

                    try {
                        n.LocIDFr = ii.Cells[row_excel, 2].Text == null ? "" : ii.Cells[row_excel, 2].Text.Trim();
                        if (n.LocIDFr == "") {
                            error_in_row = "ไม่พบ Loc From แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->LocIDFr={ii.Cells[row_excel, 2].Value}";
                    }

                    try {
                        n.LocIDTo = ii.Cells[row_excel, 3].Text == null ? "" : ii.Cells[row_excel, 3].Text.Trim();
                        if (n.LocIDTo == "") {
                            error_in_row = "ไม่พบ Loc To แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->LocIDTo={ii.Cells[row_excel, 2].Value}";
                    }

                    //try {
                    //    n.CustID = ii.Cells[row_excel, 3].Text == null ? "" : ii.Cells[row_excel, 3].Text.Trim();
                    //} catch {
                    //    error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->CustID={ii.Cells[row_excel, 3].Value}";
                    //}

                    try {
                        n.Qty = ii.Cells[row_excel, 4].Text == "" ? 0 : Convert.ToDecimal(ii.Cells[row_excel, 4].Text.Trim());
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->QTY={ii.Cells[row_excel, 4].Value}";
                    }

                    // ถ้าไม่เจอ error ใน excel ให้ check สต๊อกคงเหลือต่อ
                    var chk_enough_qty_move = InventoryService.GetBalance(n.ItemID, doc.Head.RComID, doc.Head.ComID, doc.Head.LocIDFr);
                    if (chk_enough_qty_move != null) {
                        if (chk_enough_qty_move.BalQty < n.Qty) {
                            error_in_row = error_in_row + $"{n.ItemID} ไม่พอโอน {n.Qty} เนื่องจากเหลือแค่ {chk_enough_qty_move.BalQty.ToString("n2")} ";
                        }
                    } else {
                        error_in_row = error_in_row + $"{n.ItemID} ไม่มีในฐานข้อมูล";
                    }

                    if (error_in_row != "") {
                        result_msg.Result = "fail";
                        result_msg.Message1 = result_msg.Message1 + error_in_row;
                    } else {
                        //tempList.Add(n);
                        doc.Line.Add(n);
                    }
                    error_in_row = "";
                }

                if (string.IsNullOrEmpty(result_msg.Message1)) {
                    //foreach (var n in tempList) {
                    foreach (var n in doc.Line) {
                        var item = ItemService.GetItem(n.ItemID, doc.Head.RComID);
                        if (item != null) {
                            var rs = StkTransferService.GetStkBalByLoc(doc.Head.RComID, doc.Head.ComID, n.ItemID, n.LocIDFr);
                            n.Name = item.Name1;
                            n.Unit = item.UnitID;
                            n.Price = item.Price;
                            n.Qty = n.Qty;
                            //doc.Line.Add(n);
                        }




                        //var un = doc.Line.Where(o => o.ItemID == n.ItemID).FirstOrDefault();
                        //if (un == null) {
                        //    var item = ItemService.GetItem(n.ItemID, doc.Head.RComID);
                        //    if (item != null) {
                        //        var rs = StkTransferService.GetStkBalByLoc(doc.Head.RComID, doc.Head.ComID, n.ItemID, n.LocIDFr);
                        //        n.Name = item.Name1;
                        //        n.Unit = item.UnitID;
                        //        n.Price = item.Price;
                        //        n.Qty = n.Qty;
                        //        doc.Line.Add(n);
                        //    }
                        //} else {
                        //    un.Qty = n.Qty;
                        //}
                    }
                }
                result_msg.Message1 = error_in_row;
            } catch (Exception ex) {
                result_msg.Result = "fail";
                if (ex.InnerException != null) {
                    result_msg.Message1 = result_msg.Message1 + Environment.NewLine + ex.InnerException.ToString();
                } else {
                    result_msg.Message1 = result_msg.Message1 + Environment.NewLine + ex.Message;
                }
            }
            return result_msg;
        }

        public static I_BasicResult SaveBulkV2(List<StkTransferLine> datalist) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    foreach (var l in datalist) {
                        var uh = db.StkTransferLine.Where(o => o.RComID == l.RComID
                                                            && o.ComID == l.ComID
                                                            && o.ItemID == l.ItemID
                                                            && o.TRID == l.TRID
                                                            && o.LocIDFr == l.LocIDFr
                                                            && o.LocIDTo == l.LocIDTo
                                                            && o.IsActive
                                                            ).FirstOrDefault();
                        if (uh == null) {//insert
                            db.StkTransferLine.Add(l);
                            db.SaveChanges();
                        } else {//update
                            uh.Qty = l.Qty;
                            uh.IsActive = l.IsActive;
                            db.SaveChanges();
                        }
                    }

                }
            } catch (Exception e) {
                result.Result = "fail";
                if (e.InnerException != null) {
                    result.Message1 = e.InnerException.ToString();
                } else {
                    result.Message1 = e.Message;
                }
            }
            return result;
        }

    }
}
