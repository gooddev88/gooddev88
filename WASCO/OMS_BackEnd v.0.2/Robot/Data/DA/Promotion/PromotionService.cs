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
using static Robot.Data.DA.Order.SO.SOServiceBackup;
using FisSst.BlazorMaps;
using Blazorise;
using static Robot.Data.DA.HR.InventoryService;
using Color = System.Drawing.Color;
using static Robot.Data.DA.Stock.StkTransferService;
using Path = System.IO.Path;

namespace Robot.Data.DA.Stock {

    public class PromotionService {

        public static string sessionActiveId = "activeadjid";
        public static string sessionSubBackUrl = "supbackurl";
        public class I_PromotionSet {
            public Promotions Head { get; set; }
          
            public List<vw_PromotionItem> Line { get; set; }
            public List<TransactionLog> Log { get; set; }
            public bool NeedRunNextID { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        public class I_PromotionFiterSet {
            public String Rcom { get; set; }
            public String Com { get; set; }
            public String SearchText { get; set; }
        }

        public string PreviousPageUrl = "";
        public string PreviousSubPageUrl = "";

        public I_PromotionSet DocSet { get; set; }
        ISessionStorageService sessionStorage;

        public PromotionService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;
        }

        #region Query Transaction

        public I_PromotionSet GetDocSet(string docid, string rcom, string com) {
            I_PromotionSet n = NewTransaction(rcom, com);
            try {
                using (GAEntities db = new GAEntities()) {
                    n.Head = db.Promotions.Where(o => o.ProID == docid && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                    n.Line = db.vw_PromotionItem.Where(o => o.ProID == docid && o.RCompanyID == rcom && o.CompanyID == com).ToList();
                    n.Log = TransactionService.ListLogByDocID(docid, rcom, com);
                }
            } catch (Exception ex) {
                n.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return n;
        }

        public static List<Promotions> ListDoc(I_PromotionFiterSet f) {
            List<Promotions> result = new List<Promotions>();
            try {
                using (GAEntities db = new GAEntities()) {
                    if (string.IsNullOrEmpty(f.SearchText)) {
                        result = db.Promotions.Where(o =>  o.CompanyID == f.Com
                                                      && o.RCompanyID == f.Rcom
                                                      && o.IsActive == true
                                      ).OrderByDescending(o => o.CreatedDate).ToList();
                    } else {
                        result = db.Promotions.Where(o => (
                                                        o.CompanyID.Contains(f.SearchText)
                                                        || o.ProID.Contains(f.SearchText)
                                                        || o.RCompanyID.Contains(f.SearchText)
                                                        || o.CompanyID.Contains(f.SearchText)
                                                        || f.SearchText == ""
                                                      )             
                                                      && o.CompanyID == f.Com
                                                      && o.RCompanyID == f.Rcom
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


        //public class LISTSELECT_ItemInfo : ItemInfo {
        //    public bool IsSelect { get; set; }
        //}
        public static List<ItemInfo> ListItemForPromotion(string rcom, string com,string brand, I_PromotionSet input) {
            List<ItemInfo> result = new List<ItemInfo>();
            List<string> item = new List<string>();
            var itemIds = input.Line.Select(o => (string)o.ItemID).ToList();


            using (GAEntities db = new GAEntities()) {
            
                result = db.ItemInfo.Where(o =>
                                                   o.CompanyID == com
                                                && o.RCompanyID == rcom
                                                //&& o.CustID == input.Head.CustID
                                                //&& !invIds.Contains(o.INVID)
                                                && o.BrandID== brand
                                                && !itemIds.Contains(o.ItemID)
                                                && o.IsActive == true
                                        ).OrderByDescending(o => o.CreatedDate).ToList();


                //result = Convert2PromotionSelectList(query);
            }

            return result;
        }

        #endregion

        #region stock bal
        //public static vw_STKBal GetStkBalByLoc(string rcom, string comId, string itemId, string locId) {
        //    vw_STKBal r = new vw_STKBal();
        //    using (GAEntities db = new GAEntities()) {
        //        r = db.vw_STKBal.Where(o => 
        //        o.RCompanyID == rcom 
        //        && o.ItemID == itemId 
        //        && o.CompanyID == comId 
        //        && (o.LocID == locId || locId == "")
        //        ).FirstOrDefault();
        //        return r;
        //    }
        //}



        #endregion

        #region Save

        public static I_BasicResult Save(I_PromotionSet input, string rcom, string com, string action) {
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

                        db.Promotions.Add(input.Head);
                        var l_item = Convert2PromotionItemp(input.Line);
                        db.PromotionItem.AddRange(l_item);
                        db.SaveChanges();
                     
                        if (input.NeedRunNextID) {
                            IDRuunerService.GetNewIDV2("PROMOTION", rcom, input.Head.CompanyID, input.Head.DateBegin, true, "th");
                            input.NeedRunNextID = false;
                        }
                        TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.ProID, TableID = "Promotions", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.CompanyID, Action = "INSERT NEW  PROMOTION" }, rcom, input.Head.CompanyID, input.Head.CreatedBy);
                    } else {
                        var uh = db.Promotions.Where(o => o.ProID == input.Head.ProID && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                        uh.PatternID = head.PatternID;
                        uh.ProDesc = head.ProDesc;
                        uh.XValue = head.XValue;
                        uh.YValue = head.YValue;
                        uh.DateBegin = head.DateBegin;
                        uh.DateEnd = head.DateEnd;
                        uh.CreatedBy = head.CreatedBy;
                        uh.CreatedDate = DateTime.Now;
                        uh.ModifiedBy = head.ModifiedBy;
                        uh.ModifiedDate = DateTime.Now;
                         
                        db.PromotionItem.RemoveRange(db.PromotionItem.Where(o => o.ProID == uh.ProID && o.RCompanyID == uh.RCompanyID && o.CompanyID == uh.CompanyID));
                        var l_item = Convert2PromotionItemp(input.Line);
                        db.PromotionItem.AddRange(l_item);

                        db.SaveChanges();
                       
                        TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.ProID, TableID = "Promotions", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.CompanyID, Action = "UPDATE  PROMOTION" }, rcom, input.Head.CompanyID, input.Head.ModifiedBy);
                    }
                }
             
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

        public static I_PromotionSet CalDocSet(I_PromotionSet input) {
            var h = input.Head;
            var line = input.Line;
           

            foreach (var l in line) {
                l.ProID = h.ProID;
                l.CompanyID = h.CompanyID;
                l.RCompanyID = h.RCompanyID;
                l.PatternID = h.PatternID;
            }




            return input;
        }

        public static I_PromotionSet checkDupID(I_PromotionSet input, string rcom, string com) {
            try {
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                var h = input.Head;
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.Promotions.Where(o => o.RCompanyID == h.RCompanyID && o.CompanyID == h.CompanyID && o.ProID == h.ProID).FirstOrDefault();
                    int i = 0;
                    while (get_id != null) {
                        if (i > 100) {
                            input.OutputAction = new I_BasicResult { Result = "fail", Message1 = "Cannot create document no", Message2 = "" };
                            break;
                        }
                        i++;
                        IDRuunerService.GetNewIDV2("PROMOTION", rcom, input.Head.CompanyID, input.Head.DateBegin, true, "th");
                        h.ProID = IDRuunerService.GetNewIDV2("PROMOTION", rcom, input.Head.CompanyID, input.Head.DateBegin, false, "th")[1];
                        get_id = db.Promotions.Where(o => o.RCompanyID == h.RCompanyID && o.CompanyID == h.CompanyID && o.ProID == h.ProID).FirstOrDefault();
                    }
                }
                CalDocSet(input);
            } catch (Exception ex) {
            }
            return input;
        }


        #endregion

        #region Delete

        public static I_BasicResult DeleteDoc(I_PromotionSet input, string updateby) {
            input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var head = db.Promotions.Where(o => o.ProID == input.Head.ProID && o.RCompanyID == input.Head.RCompanyID).FirstOrDefault();
                    var line = db.PromotionItem.Where(o => o.ProID == input.Head.ProID && o.RCompanyID == input.Head.RCompanyID).ToList();
                    head.ModifiedBy = updateby;
                    head.ModifiedDate = DateTime.Now;
                    head.IsActive = false;

                    foreach (var l in line) {

                        l.IsActive = false;
                    }

                    db.SaveChanges();
                    //CalStock(input);
                    TransactionService.SaveLog(new TransactionLog { TransactionID = input.Head.ProID, TableID = "Promotions", ParentID = "", TransactionDate = DateTime.Now, CompanyID = input.Head.CompanyID, Action = "DELETE  PROMOTION" }, input.Head.RCompanyID, input.Head.CompanyID, updateby);
                }
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input.OutputAction;
        }

        public static I_PromotionSet DeleteLine(string itemid, I_PromotionSet input) {
            try {
                input.Line.RemoveAll(o => o.ItemID == itemid);
                input.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                input = CalDocSet(input);
            } catch (Exception ex) {
                input.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }
            return input;
        }

        #endregion

        #region New transaction

        async public void SetSessionFiterSet(I_PromotionFiterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("promotion_fiter", json);
        }
        async public Task<I_PromotionFiterSet> GetSessionFiterSet() {
            I_PromotionFiterSet result = NewFilterSet();
            var strdoc = await sessionStorage.GetItemAsync<string>("promotion_fiter");
            if (strdoc != null) {
                return JsonConvert.DeserializeObject<I_PromotionFiterSet>(strdoc);
            } else {
                return null;
            }
        }

        public static I_PromotionFiterSet NewFilterSet() {
            I_PromotionFiterSet n = new I_PromotionFiterSet();
            n.Rcom = "";
            n.Com = "";
            n.SearchText = "";
            return n;
        }

        public static I_PromotionSet NewTransaction(string rcom, string com) {
            I_PromotionSet n = new I_PromotionSet();
            n.Head = NewHead(rcom, com);
            n.Line = new List<vw_PromotionItem>();
            n.NeedRunNextID = false;
            n.Log = new List<TransactionLog>();
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return n;
        }

        public static Promotions NewHead(string rcom, string com) {
            Promotions n = new Promotions();

            n.ProID = "";
            n.PatternID = "P100";
            n.DateBegin = DateTime.Now.Date;
            n.RCompanyID = rcom;
            n.CompanyID = com;
            n.ProDesc = "";
            n.XValue = 0;
            n.YValue = 0;
            n.DateEnd = DateTime.Now.Date;
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static vw_PromotionItem NewLine(I_PromotionSet input) {
            vw_PromotionItem n = new vw_PromotionItem();
            n.CompanyID = input.Head.CompanyID;
            n.RCompanyID = input.Head.RCompanyID;
            n.ProID = "";
            n.ItemID = "";
            n.PatternID = "P001";
            n.IsActive = true;
            return n;
        }


        //public static List<LISTSELECT_ItemInfo> Convert2PromotionSelectList(List<ItemInfo> input) {
        //    List<LISTSELECT_ItemInfo> nn = new List<LISTSELECT_ItemInfo>();
            
        //    foreach (var i in input) {
        //        LISTSELECT_ItemInfo n = new LISTSELECT_ItemInfo();
        //        //int multiply = 1;
        //        //if (cn.Contains(i.DocTypeID)) {
        //        //    multiply = -1;
        //        //}
        //        n.IsSelect = false;
        //        n.ID = i.ID;
        //        n.RCompanyID = i.RCompanyID;
        //        n.CompanyID = i.CompanyID;
        //        n.ItemID = i.ItemID;
        //        n.Name1 = i.Name1;
        //        n.Cost = i.Cost;
        //        n.Price = i.Price;
        //        n.PriceIncVat = i.PriceIncVat;
        //        n.UnitID = i.UnitID;

        //        n.IsActive = i.IsActive;
        //        nn.Add(n);
        //    }

        //    return nn;
            
        //}
        public static I_PromotionSet ConvertItem2PromotinItem(string rcom, string com, List<string> ItemIds, I_PromotionSet doc) {
            try {
                using (GAEntities db = new GAEntities()) {
                    var exist_itemid = doc.Line.Select(o => o.ItemID).ToList();
                    var item_list = db.ItemInfo.Where(o => ItemIds.Contains(o.ItemID) && o.RCompanyID == rcom && o.CompanyID == com).OrderBy(o => o.ItemID).ToList();
                    item_list = item_list.Where(o => !exist_itemid.Contains(o.ItemID)).ToList();
                    var rcs = db.PromotionItem.Where(o => ItemIds.Contains(o.ItemID) && o.IsActive).ToList();
                    foreach (var i in item_list) {

                        vw_PromotionItem n = NewLine(doc);
            
                        n.ItemID = i.ItemID;
                        n.CompanyID = i.CompanyID;
                        n.RCompanyID = i.RCompanyID;
                        
                        doc.Line.Add(n);
                    }
                    doc = CalDocSet(doc);
                }
            } catch (Exception ex) {
                throw ex;
            }

            return doc;
        }


        public static List<PromotionItem> Convert2PromotionItemp(List<vw_PromotionItem> input) {
            List<PromotionItem> output = new List<PromotionItem>();
            foreach (var i in input) {
                PromotionItem n = new PromotionItem();
                n.RCompanyID = i.RCompanyID;
                n.CompanyID = i.CompanyID;
                n.ProID = i.ProID;
                n.PatternID = i.PatternID;
                n.ItemID = i.ItemID;
                
                n.IsActive = i.IsActive;
                output.Add(n);
            }

            return output;
        }


        public static I_PromotionSet DeleteItem(string rcom, string com, List<string> ItemIds, I_PromotionSet doc) {
            try {
       
                doc.Line.RemoveAll(o => ItemIds.Contains(o.ItemID));
                doc.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                doc = CalDocSet(doc);
            } catch (Exception ex) {
                doc.OutputAction = new I_BasicResult { Result = "fail", Message1 = ex.Message, Message2 = "" };
            }

            return doc;
        }
        #endregion


        public static List<ItemInfo> ListItemPromotion(string rcom, string com) {
            List<ItemInfo> result = new List<ItemInfo>();
            using (GAEntities db = new GAEntities()) { 
                result = db.ItemInfo.Where(o => o.RCompanyID == rcom
                                 && o.CompanyID == com 
                                 && o.IsActive == true
                                 ).ToList();
            }
            return result;
        }
        public static MemoryStream DownItemToExcel(string rcom, string com) {
            var memoryStream = new MemoryStream();
         var ItemPro = ListItemPromotion(rcom, com);

            using (var excelPackage = new ExcelPackage()) {
                int row_excel = 2;
                ExcelWorksheet excelWorksheet1 = excelPackage.Workbook.Worksheets.Add("ITEMS");
                ExcelRange cells = excelWorksheet1.Cells;

                cells[1, 1].Value = "ItemID"; cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 1].Style.Font.Color.SetColor(Color.Black); cells[1, 1].Style.Font.Size = 16; cells[1, 1].Style.Font.Bold = true;

                //cells[1, 2].Value = "Brand"; cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                //cells[1, 2].Style.Font.Color.SetColor(Color.Black); cells[1, 2].Style.Font.Size = 16; cells[1, 2].Style.Font.Bold = true;

                //cells[1, 3].Value = "Price (Inc.Vat)"; cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                //cells[1, 3].Style.Font.Color.SetColor(Color.Black); cells[1, 3].Style.Font.Size = 16; cells[1, 2].Style.Font.Bold = true;
                foreach (var e in ItemPro) {
                    cells[row_excel, 1].Value = e.ItemID; cells[row_excel, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 1].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    //cells[row_excel, 2].Value = e.BrandID; cells[row_excel, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 2].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    //cells[row_excel, 3].Value = e.PriceIncVat.ToString("N2"); cells[row_excel, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 3].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    row_excel++;
                }

                cells[1, 1, row_excel, 3].AutoFitColumns();
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

        //public static List<StkTransferLine> STKBalConvert2AdjustLine(List<vw_STKBal> STKBal, I_StkTransferSet input) {
        //    var line = input.Line;
        //    foreach (var i in STKBal) {
        //        var chkExist = line.Where(o => o.ItemID == i.ItemID).FirstOrDefault();
        //        if (chkExist != null) {
        //            continue;
        //        }
        //        StkTransferLine n = NewLine(input);
        //        n.ItemID = i.ItemID;
        //        n.Name = i.ItemName;
        //        n.Qty = i.BalQty;
        //        n.Price = i.Price;
        //        n.LocIDFr = i.LocID;
        //        n.LocIDTo = i.LocID;

        //        n.Unit = i.UnitID;
        //        input.Line.Add(n);
        //    }
        //    CalDocSet(input);

        //    return input.Line;
        //}

        public static I_BasicResult UploadItem(IFormFile myFile, I_PromotionSet doc) {
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

                var rsm = ReadSheetItemStkTransferFile(excelWorksheet, doc, out read_result);
                if (read_result.Result == "fail") {
                    read_result.Message1 = "Error sheet name";
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


        public static I_BasicResult ReadSheetItemStkTransferFile(ExcelWorksheet ii, I_PromotionSet doc, out I_BasicResult result_msg) {
            //List<StkTransferLine> ListStkAdjustList = new List<StkTransferLine>();
            result_msg = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                string error_in_row = "";
                string SheetName = ii.Name;

                int totalRows = ii.Dimension.End.Row;
                int start_row = 2;
                int end_row = totalRows;
                int row_excel = 1;

                var culture = new CultureInfo("en-US");
                row_excel = 1;//reset row
                for (row_excel = start_row; row_excel <= end_row; row_excel++) {
                    error_in_row = "";
                    var n = PromotionService.NewLine(doc);
                    try {
                        n.ItemID = ii.Cells[row_excel, 1].Text == null ? "" : ii.Cells[row_excel, 1].Text.Trim();
                        if (n.ItemID == "") {
                            error_in_row = "ไม่พบ ItemID แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->ItemID={ii.Cells[row_excel, 1].Value.ToString()}";
                    }



                    if (error_in_row != "") {
                        result_msg.Result = "fail";
                        result_msg.Message1 = result_msg.Message1 + error_in_row;
                    }

                    error_in_row = "";

                    var un = doc.Line.Where(o => o.ItemID == n.ItemID && o.ProID == n.ProID).FirstOrDefault();
                    if (un == null) {
                        var item = ItemService.GetItem(n.ItemID, doc.Head.RCompanyID);
                        if (item != null) {
                            //var rs = StkTransferService.GetStkBalByLoc(doc.Head.RCompanyID, doc.Head.CompanyID, n.ItemID, n.LocIDFr);
                            n.ItemID = item.ItemID;

                            doc.Line.Add(n);
                        }
                    } else {
                        un.ItemID = n.ItemID;
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
    }
}
