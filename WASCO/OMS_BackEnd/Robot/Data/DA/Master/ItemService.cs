using Robot.Data.ML;
using Robot.Data.GADB.TT;
using Robot.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using static Robot.Data.DA.Stock.StkTransferService;
using Robot.Data.DA.Stock;
using System.Globalization;

namespace Robot.Data.DA.Master {
    public class ItemService {
        public static string sessionActiveId = "activeitemid";
        public class I_ItemSet {
            public ItemInfo Info { get; set; }
            public List<ItemInfo> LineItem { get; set; }
            public List<ItemBarcode> ItemBarcode { get; set; }
            public List<TransactionLog> Log { get; set; }
            public List<XFilesRef> Files { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }


        public string PreviousPageUrl = "";
        public I_ItemSet DocSet { get; set; }

        public ItemService() {
            //DocSet = NewTransaction("", "","");
        }

        public class I_ShipTo {
            public string ShipToID { get; set; }
            public string ShipToName { get; set; }
        }

        #region Get&ListData

        public I_ItemSet GetDocSet(string itemId, string rcom, string com) {
            I_ItemSet n = NewTransaction(rcom, com);
            using (GAEntities db = new GAEntities()) {
                n.Info = db.ItemInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                n.ItemBarcode = db.ItemBarcode.Where(o => o.ItemID == itemId && o.RCompanyID == rcom && o.IsActive).ToList();
                n.Log = db.TransactionLog.Where(o => o.TransactionID == itemId && o.TableID == "ITEM").OrderBy(o => o.CreatedDate).ToList();
            }
            return n;
        }

        public static ItemInfo GetItemInfo(string rcom, string itemid) {
            ItemInfo result = new ItemInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.ItemInfo.Where(o => o.RCompanyID == rcom && o.ItemID == itemid).FirstOrDefault();
            }
            return result;
        }

        public static ItemInfo GetItemInfo(string rcom, string com, string itemid) {
            ItemInfo result = new ItemInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.ItemInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == com && o.ItemID == itemid).FirstOrDefault();
            }
            return result;
        }

        public static List<ItemInfo> ListItemInfo(string rcom, string com, string brandId) {
            brandId = brandId == null ? "" : brandId;
            List<ItemInfo> result = new List<ItemInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.ItemInfo.Where(o => o.RCompanyID == rcom
                                            && o.CompanyID == com
                                            && (o.BrandID == brandId || brandId == "")
                                            && o.IsActive == true
                                            ).ToList();
            }
            return result;
        }

        public static List<SelectOption> ListItemForSelect(string rcom, List<string> types) {
            List<SelectOption> result = new List<SelectOption>();
            using (GAEntities db = new GAEntities()) {
                var items = db.ItemInfo.Where(o => o.RCompanyID == rcom
                                                            && types.Contains(o.TypeID)
                                                            && o.IsActive == true
                                                            ).OrderBy(o => o.TypeID).ThenBy(o => o.Name1).ToList();
                int i = 0;
                foreach (var v in items) {
                    i++;
                    SelectOption n = new SelectOption();
                    n.IsSelect = true;
                    n.Value = v.ItemID;
                    n.Description = v.Name1;
                    n.Sort = i;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<vw_ItemInfo> ListDoc(string Search, string rcom, string com, string brand_id, bool isNotActive) {
            List<vw_ItemInfo> result = new List<vw_ItemInfo>();
            using (GAEntities db = new GAEntities()) {
                if (string.IsNullOrEmpty(Search)) {
                    result = db.vw_ItemInfo.Where(o =>
                                                           o.RCompanyID == rcom
                                                        && o.CompanyID == com
                                                        && o.IsActive == !isNotActive
                                                        && (o.BrandID == brand_id || brand_id == "")
                                                ).OrderBy(o => o.TypeID).ThenBy(o => o.ItemID).ToList();
                } else {
                    result = db.vw_ItemInfo.Where(o =>
                                                        (o.ItemID.Contains(Search)
                                                                || o.RefID.Contains(Search)
                                                                || o.Name1.Contains(Search)
                                                                || o.CateID.Contains(Search)
                                                                || o.TypeID.Contains(Search)
                                                                || o.TaxTypeID.Contains(Search)
                                                                || o.AccID_Purchase.Contains(Search)
                                                                || o.Group1ID.Contains(Search)
                                                                || o.Group1Name.Contains(Search)
                                                                || o.TypeName.Contains(Search)
                                                                || Search == "") 
                                                        && o.RCompanyID == rcom
                                                        && o.CompanyID == com
                                                        && o.IsActive == !isNotActive
                                                        && (o.BrandID == brand_id || brand_id == "")
                                                ).OrderBy(o => o.TypeID).ThenBy(o => o.ItemID).ToList();
                }
        

            }
            return result;
        }

        public static ItemInfo GetItem(string itemId, string rcom) {
            ItemInfo result = new ItemInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.ItemInfo.Where(o => o.ItemID == itemId && o.RCompanyID == rcom).FirstOrDefault();
            }
            return result;
        }

        public static List<vw_ItemInfo> ListViewItemByType(string typeID, string rcom) {
            List<vw_ItemInfo> result = new List<vw_ItemInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_ItemInfo.Where(o => (o.TypeID == typeID || typeID == "")
                                                    && o.RCompanyID == rcom && o.IsActive).ToList();
            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(ItemInfo doc, string createby) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var i = db.ItemInfo.Where(o => o.ItemID == doc.ItemID && o.RCompanyID == doc.RCompanyID && o.CompanyID == doc.CompanyID).FirstOrDefault();
                    if (i == null) {
                        db.ItemInfo.Add(doc);
                        db.SaveChanges();
                        SaveLog(new TransactionLog { TransactionID = doc.ItemID, TableID = "ITEM", ParentID = doc.CreatedBy, TransactionDate = DateTime.Now, CompanyID = "", Action = "INSERT NEW ITEM" }, doc.RCompanyID, doc.CreatedBy);
                    } else {
                        i.ItemCode = doc.ItemCode;
                        i.RefID = doc.RefID;
                        i.BrandID = doc.BrandID;
                        i.Model = doc.Model;
                        i.Size = doc.Size;
                        i.Color = doc.Color;
                        i.Name1 = doc.Name1;
                        i.Name2 = doc.Name2;
                        i.TypeID = doc.TypeID;
                        i.CateID = doc.CateID;
                        i.Group1ID = doc.Group1ID;
                        i.Group2ID = doc.Group2ID;
                        i.Group3ID = doc.Group3ID;
                        i.VendorID = doc.VendorID;
                        i.Cost = doc.Cost;
                        i.Price = doc.Price;
                        i.PriceIncVat = doc.PriceIncVat;
                        i.TaxGroupID = doc.TaxGroupID;
                        i.TaxTypeID = doc.TaxTypeID;
                        i.UnitID = doc.UnitID;
                        i.StkUnitID = doc.StkUnitID;
                        i.Dimension = doc.Dimension;
                        i.IsKeepStock = doc.IsKeepStock;
                        i.Remark1 = doc.Remark1;
                        i.Remark2 = doc.Remark2;
                        i.AccID_Sale = doc.AccID_Sale;
                        i.AccID_Purchase = doc.AccID_Purchase;
                        i.Acc_Side = doc.Acc_Side;
                        i.PhotoID = doc.PhotoID;
                        i.ModifiedDate = DateTime.Now;
                        i.ModifiedBy = doc.CreatedBy;
                        i.IsHold = doc.IsHold;
                        i.IsActive = doc.IsActive;
                        db.SaveChanges();
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

        public static I_BasicResult checkDupID(ItemInfo doc) {
            I_BasicResult r = new I_BasicResult { Result = "fail", Message1 = "Duplicate iteminfo code", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var get_id = db.ItemInfo.Where(o => o.ItemID == doc.ItemID && o.RCompanyID == doc.RCompanyID && o.CompanyID == doc.CompanyID).FirstOrDefault();

                    if (get_id == null) {
                        r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
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

        public static void SaveLog(TransactionLog data, string rcom, string username) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                data.RCompanyID = rcom;
                data.TableID = data.TableID == null ? "" : data.TableID;
                data.TransactionID = data.TransactionID == null ? "" : data.TransactionID;
                data.TransactionDate = data.TransactionDate == null ? data.CreatedDate : data.TransactionDate;
                data.CreatedBy = username;
                data.CreatedDate = DateTime.Now;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.CompanyID = data.CompanyID == null ? "" : data.CompanyID;
                data.ParentID = data.ParentID == null ? "" : data.ParentID;
                data.Action = data.ActionType == null ? "" : data.ActionType;
                data.Action = data.Action == null ? "" : data.Action;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.IsActive = true;
                using (GAEntities db = new GAEntities()) {
                    db.TransactionLog.Add(data);
                    var r = db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
        }
        public static void SavePhotoID(string rcom, string com, string itemid, string photoid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var query = db.ItemInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == com && o.ItemID == itemid).FirstOrDefault();
                    query.PhotoID = photoid;
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
        }
        #endregion


        #region New transaction

        public static I_ItemSet NewTransaction(string rcom, string com) {
            I_ItemSet n = new I_ItemSet();

            n.Info = NewItem(rcom, com);
            n.ItemBarcode = new List<ItemBarcode>();
            n.Files = new List<XFilesRef>();
            n.Log = new List<TransactionLog>();

            return n;
        }
        public static ItemInfo NewItem(string rcom, string comid) {
            ItemInfo n = new ItemInfo();
            n.ItemID = "";
            n.CompanyID = comid;
            n.RCompanyID = rcom;
            n.ItemCode = "";
            n.Model = "";
            n.Color = "";
            n.Size = "";
            n.RefID = "";
            n.Name1 = "";
            n.Name2 = "";
            n.TypeID = "";
            n.CateID = "";
            n.Group1ID = "";
            n.Group2ID = "";
            n.Group3ID = "";
            n.BrandID = "";
            n.Cost = 0;
            n.Price = 0;
            n.PriceIncVat = 0;
            n.UnitID = "";
            n.StkUnitID = "";
            n.Weight = 0;
            n.Dimension = 0;
            n.VendorID = "";
            n.IsKeepStock = false;
            n.Remark1 = "";
            n.Remark2 = "";
            n.TaxGroupID = "";
            n.TaxTypeID = "";
            n.AccID_Sale = "";
            n.AccID_Purchase = "";
            n.Acc_Side = "";
            n.PhotoID = "";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.Status = "";
            n.IsHold = false;
            n.IsActive = true;
            return n;
        }

        //public static ItemPriceInfo NewPrice(string username, string rcom)
        //{
        //    ItemPriceInfo newdata = new ItemPriceInfo();
        //    newdata.ItemID = "";
        //    newdata.CompanyID = "";
        //    newdata.RCompanyID = rcom;
        //    newdata.Price = 0;
        //    newdata.UseLevel = 0;
        //    newdata.RefID = "";
        //    newdata.DataSource = "";
        //    newdata.CompanyID = "";
        //    newdata.Remark = "";
        //    newdata.RefID = "";
        //    newdata.DateBegin = DateTime.Now.Date;
        //    newdata.DateEnd = DateTime.Now.Date;
        //    newdata.IsActive = true;
        //    return newdata;
        //}

        public static List<SelectOption> ListUseLevel() {
            return new List<SelectOption>() {
                new SelectOption(){ IsSelect = true ,Value= "0", Description="0" ,Sort = 1},
                new SelectOption(){ IsSelect = true ,Value = "1", Description="1", Sort = 2}
            };
        }
        #endregion

        public static List<ItemInfo> ListItem(string rcom) {
            List<ItemInfo> result = new List<ItemInfo>();
            using (GAEntities db = new GAEntities()) {

                result = db.ItemInfo.Where(o => o.RCompanyID == rcom
                                                                
                                                                && o.IsActive == true
                                                                ).ToList();
            }
            return result;
        }

        public static MemoryStream DownloadItemExcel(string rcom) {
            var memoryStream = new MemoryStream();
            var ItemInfo = ListItem(rcom);

            using (var excelPackage = new ExcelPackage()) {
                int row_excel = 2;
                ExcelWorksheet excelWorksheet1 = excelPackage.Workbook.Worksheets.Add("ITEMS");
                ExcelRange cells = excelWorksheet1.Cells;

                cells[1, 1].Value = "ITEM ID"; cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 1].Style.Font.Color.SetColor(Color.Black); cells[1, 1].Style.Font.Size = 16; cells[1, 1].Style.Font.Bold = true;

                cells[1, 2].Value = "DESCRIPTION"; cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 2].Style.Font.Color.SetColor(Color.Black); cells[1, 2].Style.Font.Size = 16; cells[1, 2].Style.Font.Bold = true;

                cells[1, 3].Value = "CATEGORY"; cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 3].Style.Font.Color.SetColor(Color.Black); cells[1, 3].Style.Font.Size = 16; cells[1, 3].Style.Font.Bold = true;

                cells[1, 4].Value = "BRAND ID"; cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 4].Style.Font.Color.SetColor(Color.Black); cells[1, 4].Style.Font.Size = 16; cells[1, 4].Style.Font.Bold = true;

                cells[1, 5].Value = "PRICE (INC.VAT)"; cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 5].Style.Font.Color.SetColor(Color.Black); cells[1, 5].Style.Font.Size = 16; cells[1, 5].Style.Font.Bold = true;

                cells[1, 6].Value = "PRICE PROMOTION (INC.VAT)"; cells[1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 6].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 6].Style.Font.Color.SetColor(Color.Black); cells[1, 66].Style.Font.Size = 16; cells[1, 6].Style.Font.Bold = true;

                cells[1, 7].Value = "COST"; cells[1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 7].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 7].Style.Font.Color.SetColor(Color.Black); cells[1, 7].Style.Font.Size = 16; cells[1, 7].Style.Font.Bold = true;

                cells[1, 8].Value = "UNIT"; cells[1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 8].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 8].Style.Font.Color.SetColor(Color.Black); cells[1, 8].Style.Font.Size = 16; cells[1, 8].Style.Font.Bold = true;

                cells[1, 9].Value = "REMARK"; cells[1, 9].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 9].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 9].Style.Font.Color.SetColor(Color.Black); cells[1, 9].Style.Font.Size = 16; cells[1, 9].Style.Font.Bold = true;
                foreach (var e in ItemInfo) {
                    cells[row_excel, 1].Value = e.ItemID; cells[row_excel, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 1].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 2].Value = e.Name1; cells[row_excel, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 2].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 3].Value = e.CateID; cells[row_excel, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 3].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 4].Value = e.BrandID; cells[row_excel, 4].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 4].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 5].Value = Convert.ToDecimal(e.PriceIncVat).ToString("N0"); cells[row_excel, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 5].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 6].Value = Convert.ToDecimal(e.PriceProIncVat).ToString("N0"); cells[row_excel, 6].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 6].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 7].Value = e.Cost; cells[row_excel, 7].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 7].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 8].Value = e.UnitID; cells[row_excel, 8].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 8].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 9].Value = e.Remark1; cells[row_excel, 9].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 9].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    row_excel++;
                }

                cells[1, 1, row_excel, 9].AutoFitColumns();
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

        public static I_BasicResult UploadItem(IFormFile myFile, I_ItemSet doc,string rcom,string com) {
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

                read_result = ReadSheetItemInfoFile(excelWorksheet, doc, rcom,com);
                if (read_result.Result == "fail") { 
                    return read_result;
                }

                //CalDocSet(doc);
                //var rs = SaveBulkV2(doc.Line);
                //if (rs.Result == "fail") {
                //    read_result.Result = rs.Result;
                //    read_result.Message1 = rs.Message1;
                //}
            }
            return read_result;
        }

        public static I_BasicResult ReadSheetItemInfoFile(ExcelWorksheet ii, I_ItemSet doc, string rcom,string comid) {
            //List<StkTransferLine> ListStkAdjustList = new List<StkTransferLine>();
        var    result_msg = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<ItemInfo> itemsList = new List<ItemInfo>();
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
                    var n = ItemService.NewItem(rcom,comid);
                    try {
                        n.ItemID = ii.Cells[row_excel, 1].Text == null ? "" : ii.Cells[row_excel, 1].Text.Trim();
                        if (n.ItemID == "") {
                            error_in_row = "ไม่พบ ITEM ID แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->ItemID={ii.Cells[row_excel, 1].Value.ToString()}";
                    }

                    try {
                        n.Name1 = ii.Cells[row_excel, 2].Text == null ? "" : ii.Cells[row_excel, 2].Text.Trim();
                        if (n.Name1 == "") {
                            error_in_row = "ไม่พบ DESCRIPTION แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->Name1={ii.Cells[row_excel, 2].Value.ToString()}";
                    }

                    try {
                        n.CateID = ii.Cells[row_excel, 3].Text == null ? "" : ii.Cells[row_excel, 3].Text.Trim();
                        n.CateID = n.CateID == null ?"": n.CateID;
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->CateID={ii.Cells[row_excel, 3].Value.ToString()}";
                    }

                    try {
                        n.BrandID = ii.Cells[row_excel, 4].Text == null ? "" : ii.Cells[row_excel, 4].Text.Trim();
                        if (string.IsNullOrEmpty( n.BrandID ) ) {
                            error_in_row = "ไม่พบ BRAND ID แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->BrandID={ii.Cells[row_excel, 4].Value.ToString()}";
                    }


                    try {
                        n.PriceIncVat = string.IsNullOrEmpty(ii.Cells[row_excel, 5].Text)   ? 0 : Convert.ToDecimal(ii.Cells[row_excel, 5].Text.Trim());
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->PRICE (INC.VAT)={ii.Cells[row_excel, 5].Value}";
                    }
                    try {
                        n.PriceProIncVat = string.IsNullOrEmpty(ii.Cells[row_excel, 6].Text) ? 0 : Convert.ToDecimal(ii.Cells[row_excel, 6].Text.Trim());
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->PRICE PROMOTION (INC.VAT)={ii.Cells[row_excel, 6].Value}";
                    }

                    try {
                        n.Cost = string.IsNullOrEmpty(ii.Cells[row_excel, 7].Text ) ? 0 : Convert.ToDecimal(ii.Cells[row_excel, 7].Text.Trim());
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->COST={ii.Cells[row_excel, 7].Value}";
                    }


                    try {
                        n.UnitID = string.IsNullOrEmpty(ii.Cells[row_excel, 8].Text)   ? "" : ii.Cells[row_excel, 8].Text.Trim();
                        if (string.IsNullOrEmpty(n.UnitID)) {
                            error_in_row = "ไม่พบ UNIT แถวที่ " + row_excel;
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->UnitID={ii.Cells[row_excel, 8].Value}";
                    }

                    try {
                        n.Remark1 = string.IsNullOrEmpty(ii.Cells[row_excel, 9].Text) ? "" : ii.Cells[row_excel, 9].Text.Trim();
                      
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->Remark1={ii.Cells[row_excel, 9].Value.ToString()}";
                    }

                  

                    //try {
                    //    n.Qty = ii.Cells[row_excel, 4].Text == "" ? 0 : Convert.ToDecimal(ii.Cells[row_excel, 4].Text.Trim());
                    //} catch {
                    //    error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->QTY={ii.Cells[row_excel, 4].Value}";
                    //}

                    if (error_in_row != "") {
                        result_msg.Result = "fail";
                        result_msg.Message1 = result_msg.Message1 + error_in_row;
                    } 
                    error_in_row = ""; 
                    itemsList.Add(n);
                }
               

                if (string.IsNullOrEmpty(result_msg.Message1)) {
                    using (GAEntities db=new GAEntities()) {
                        foreach (var ix in itemsList) {
                            var query_item = db.ItemInfo.Where(o => o.RCompanyID == rcom && o.CompanyID==comid && o.ItemID == ix.ItemID).FirstOrDefault();
                            if (query_item==null) {
                                var new_item = NewItem(rcom,comid);
                                new_item.ItemID = ix.ItemID;
                                new_item.Name1 = ix.Name1;
                                new_item.CateID = ix.CateID;
                                new_item.BrandID = ix.BrandID;
                                new_item.PriceIncVat = ix.PriceIncVat;
                                new_item.PriceProIncVat = ix.PriceProIncVat;
                                new_item.Cost = ix.Cost;
                                new_item.UnitID = ix.UnitID;
                                new_item.Remark1 = ix.Remark1;
                                new_item.IsActive = true;
                                db.ItemInfo.Add(new_item);
                                db.SaveChanges();
                            } else {
                                query_item.ItemID = ix.ItemID;
                                query_item.Name1 = ix.Name1;
                                query_item.CateID = ix.CateID;
                                query_item.BrandID = ix.BrandID;
                                query_item.PriceIncVat = ix.PriceIncVat;
                                query_item.PriceProIncVat = ix.PriceProIncVat;
                                query_item.Cost = ix.Cost;
                                query_item.UnitID = ix.UnitID;
                                query_item.Remark1 = ix.Remark1;
                                query_item.IsActive = true;
                                db.SaveChanges();
                            }
                        }
                    }
                } else {
                    return result_msg;
                }

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
