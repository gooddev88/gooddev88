using OfficeOpenXml;
using OfficeOpenXml.Style;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class ItemPriceService {
        public class I_FilterSet {
            public String Search { get; set; }
            public String ItemID { get; set; }
            public String PriceTaxcon { get; set; }
            public String RCompany { get; set; }
            public String Company { get; set; }
            public String ShipTo { get; set; }
            public String UseLevel { get; set; }
        }

        //public class I_FilterSet {
        //    public DateTime Begin { get; set; }
        //    public DateTime End { get; set; }
        //    public string Company { get; set; }
        //    public string Table { get; set; }
        //    public string ShipTo { get; set; } 
        //    public string Search { get; set; }
        //    public bool ShowActive { get; set; }
        //}

        public static List<vw_ItemPriceInfo> ListItemPrice(I_FilterSet filter) {
            filter.Search = filter.Search.Trim().ToLower();
            int UseLevel = 0;
            if (filter.UseLevel != "") {
                UseLevel = Convert.ToInt32(filter.UseLevel);
            }


            List<vw_ItemPriceInfo> result = new List<vw_ItemPriceInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_ItemPriceInfo.Where(o =>
                                        (
                                            o.ItemID.ToLower().Contains(filter.Search)
                                            || o.CompanyID.ToLower().Contains(filter.Search)
                                            || o.ItemName.ToLower().Contains(filter.Search)
                                            || o.CompanyName.ToLower().Contains(filter.Search)
                                            || o.CustID.ToLower().Contains(filter.Search)
                                            || filter.Search == ""
                                        )
                                        && (o.ItemID == filter.ItemID || filter.ItemID == "")
                                        && (o.CompanyID == filter.Company || filter.Company == "")
                                        && (o.PriceTaxCondType == filter.PriceTaxcon || filter.PriceTaxcon == "")
                                        && (o.UseLevel == UseLevel || filter.UseLevel == "")
                                        && (o.CustID == filter.ShipTo || filter.ShipTo == "")
                                        && o.RCompanyID == filter.RCompany
                                        && o.IsActive == true
                                        ).OrderBy(o => o.ItemID).ThenByDescending(o => o.CompanyID).ToList();
            }
            return result;

        }

        public static I_FilterSet NewFilter() {
            I_FilterSet n = new I_FilterSet();
            n.Search = "";
            n.ItemID = "";
            n.PriceTaxcon = "";
            n.Company = "";
            n.RCompany = "";
            n.ShipTo = "";
            n.UseLevel = "0";
            return n;
        }
        public static ItemPriceInfo NewPrice(string rcom, string com) {
            ItemPriceInfo newdata = new ItemPriceInfo();
            newdata.ItemID = "";
            newdata.CompanyID = com;
            newdata.RCompanyID = rcom;
            newdata.Price = 0;
            newdata.UseLevel = 0;
            newdata.RefID = "";
            newdata.DataSource = "";
            newdata.CompanyID = "";
            newdata.Remark = "";
            newdata.RefID = "";
            newdata.DateBegin = DateTime.Now.Date;
            newdata.DateEnd = DateTime.Now.Date;
            newdata.IsActive = true;
            return newdata;
        }




        #region Excel export
        public static MemoryStream DownItemPriceToExel(string rcom) {
            var memoryStream = new MemoryStream();
            I_FilterSet f = NewFilter();
            f.RCompany = rcom;
            var price = ListItemPrice(f);

            using (var excelPackage = new ExcelPackage()) {
                int row_excel = 2;
                ExcelWorksheet excelWorksheet1 = excelPackage.Workbook.Worksheets.Add("PRICE");
                ExcelRange cells = excelWorksheet1.Cells;

                cells[1, 1].Value = "CompanyID"; cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 1].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 1].Style.Font.Color.SetColor(Color.Black); cells[1, 1].Style.Font.Size = 16; cells[1, 1].Style.Font.Bold = true;

                cells[1, 2].Value = "ItemID"; cells[1, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 2].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 2].Style.Font.Color.SetColor(Color.Black); cells[1, 2].Style.Font.Size = 16; cells[1, 2].Style.Font.Bold = true;

                cells[1, 3].Value = "CustID"; cells[1, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 3].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 3].Style.Font.Color.SetColor(Color.Black); cells[1, 3].Style.Font.Size = 16; cells[1, 3].Style.Font.Bold = true;

                cells[1, 4].Value = "UseLevel"; cells[1, 4].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 4].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 4].Style.Font.Color.SetColor(Color.Black); cells[1, 4].Style.Font.Size = 16; cells[1, 4].Style.Font.Bold = true;

                cells[1, 5].Value = "DateBegin"; cells[1, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 5].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 5].Style.Font.Color.SetColor(Color.Black); cells[1, 5].Style.Font.Size = 16; cells[1, 5].Style.Font.Bold = true;

                cells[1, 6].Value = "DateEnd"; cells[1, 6].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 6].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 6].Style.Font.Color.SetColor(Color.Black); cells[1, 6].Style.Font.Size = 16; cells[1, 6].Style.Font.Bold = true;

                cells[1, 7].Value = "Price"; cells[1, 7].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 7].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 7].Style.Font.Color.SetColor(Color.Black); cells[1, 7].Style.Font.Size = 16; cells[1, 7].Style.Font.Bold = true;

                cells[1, 8].Value = "PriceTaxCondType"; cells[1, 8].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[1, 8].Style.Fill.BackgroundColor.SetColor(Color.CadetBlue);
                cells[1, 8].Style.Font.Color.SetColor(Color.Black); cells[1, 8].Style.Font.Size = 16; cells[1, 8].Style.Font.Bold = true;

                foreach (var e in price) {
                    cells[row_excel, 1].Value = e.CompanyID; cells[row_excel, 1].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 1].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 2].Value = e.ItemID; cells[row_excel, 2].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 2].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 3].Value = e.CustID; cells[row_excel, 3].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 3].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 4].Value = Convert.ToInt32(e.UseLevel).ToString("N0"); cells[row_excel, 4].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 4].Style.Fill.BackgroundColor.SetColor(Color.Wheat);

                    string day = e.DateBegin.Day.ToString("00");
                    string month = e.DateBegin.Month.ToString("00");
                    string year = e.DateBegin.Year.ToString("0000");
                    string DateBegin = day + "/" + month + "/" + year;
                    cells[row_excel, 5].Value = DateBegin; cells[row_excel, 5].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 5].Style.Fill.BackgroundColor.SetColor(Color.Wheat);

                    string dayend = e.DateEnd.Day.ToString("00");
                    string monthend = e.DateEnd.Month.ToString("00");
                    string yearend = e.DateEnd.Year.ToString("0000");
                    string DateEnd = dayend + "/" + monthend + "/" + yearend;
                    cells[row_excel, 6].Value = DateBegin; cells[row_excel, 6].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 6].Style.Fill.BackgroundColor.SetColor(Color.Wheat);

                    cells[row_excel, 7].Value = Convert.ToDecimal(e.Price).ToString("N2"); cells[row_excel, 7].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 7].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    cells[row_excel, 8].Value = e.PriceTaxCondType; cells[row_excel, 8].Style.Fill.PatternType = ExcelFillStyle.Solid; cells[row_excel, 8].Style.Fill.BackgroundColor.SetColor(Color.Wheat);
                    row_excel++;
                }

                cells[1, 1, row_excel, 11].AutoFitColumns();
                string fileName = "ItemPrice-" + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");

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
        #endregion

        public static List<ItemPriceInfo> ReadSheetItemPriceFile(ExcelWorksheet ii,string rcom, out I_BasicResult result_msg) {
            List<ItemPriceInfo> ListItemPrice = new List<ItemPriceInfo>();
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
                    var n = NewPrice(rcom, "");
                    try {
                        n.CompanyID = ii.Cells[row_excel, 1].Text == null ? "" : ii.Cells[row_excel, 1].Text.Trim();
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->CompanyID={ ii.Cells[row_excel, 1].Value.ToString()}";
                    }

                    try {
                        n.ItemID = ii.Cells[row_excel, 2].Text == null ? "" : ii.Cells[row_excel, 2].Text.Trim();
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->ItemID={ ii.Cells[row_excel, 2].Value}";
                    }

                    try {
                        n.CustID = ii.Cells[row_excel, 3].Text == null ? "" : ii.Cells[row_excel, 3].Text.Trim();
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->CustID={ ii.Cells[row_excel, 3].Value}";
                    }

                    try {
                        n.UseLevel = ii.Cells[row_excel, 4].Text == "" ? 0 : Convert.ToInt32(ii.Cells[row_excel, 4].Text.Trim());
                        if (n.UseLevel < 0 && n.UseLevel > 1) {
                            error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->UseLevel={ ii.Cells[row_excel, 2].Value}";
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->UseLevel={ii.Cells[row_excel, 4].Value}";
                    }

                    try {
                        n.DateBegin = DateTime.ParseExact(ii.Cells[row_excel, 5].Text, "dd/MM/yyyy", culture);
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->ateBegin->{ii.Cells[row_excel, 5].Value}";
                    }

                    try {
                        n.DateEnd = DateTime.ParseExact(ii.Cells[row_excel, 6].Text, "dd/MM/yyyy", culture);
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->DateEnd={ii.Cells[row_excel, 6].Value}";
                    }

                    try {
                        n.Price = ii.Cells[row_excel, 7].Text == "" ? 0 : Convert.ToDecimal(ii.Cells[row_excel, 7].Text.Trim());
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->Price={ii.Cells[row_excel, 7].Value}";
                    }

                    try {
                        n.PriceTaxCondType = ii.Cells[row_excel, 8].Text.ToUpper() == null ? "" : ii.Cells[row_excel, 8].Text.ToUpper();
                        if (n.PriceTaxCondType != "INC VAT") {
                            if (n.PriceTaxCondType != "EXC VAT") {
                                error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->PriceTaxCondType={ii.Cells[row_excel, 8].Value}";
                            }
                        }
                        if (n.PriceTaxCondType != "EXC VAT") {
                            if (n.PriceTaxCondType != "INC VAT") {
                                error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->PriceTaxCondTyp={ii.Cells[row_excel, 8].Value}";
                            }
                        }
                    } catch {
                        error_in_row = error_in_row + Environment.NewLine + $"error: {SheetName}->row={row_excel}->PriceTaxCondType={ii.Cells[row_excel, 8].Value}";
                    }


                    if (error_in_row != "") {
                        result_msg.Result = "fail";
                        result_msg.Message1 = result_msg.Message1 +error_in_row;
                    }
                    error_in_row = "";
                    ListItemPrice.Add(n);
                }
                //result_msg.Message1 = error_in_row;

            } catch (Exception ex) {
                result_msg.Result = "fail";
                if (ex.InnerException != null) {
                    result_msg.Message1 = result_msg.Message1+Environment.NewLine+ ex.InnerException.ToString();
                } else {
                    result_msg.Message1 = result_msg.Message1 + Environment.NewLine + ex.Message;
                }
            }

            return ListItemPrice;
        }

        #region Excel Import 
        

        public static I_BasicResult SaveBulkV2(List<ItemPriceInfo> datalist) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    foreach (var l in datalist) {
                        var uh = db.ItemPriceInfo.Where(o => o.RCompanyID == l.RCompanyID
                                                            && o.CompanyID == l.CompanyID
                                                            && o.ItemID == l.ItemID
                                                            && o.CustID == l.CustID
                                                            ).FirstOrDefault();
                        if (uh == null) {//insert
                            db.ItemPriceInfo.Add(l);
                            db.SaveChanges();
                        } else {//update
                            uh.Price = l.Price;
                            uh.UseLevel = l.UseLevel;
                            uh.PriceTaxCondType = l.PriceTaxCondType;
                            uh.RefID = l.RefID;
                            uh.DateBegin = l.DateBegin;
                            uh.DateEnd = l.DateEnd;
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
                //string err = "มีบางอย่างผิดลพลาดในไฟล์";
                //foreach (var eve in e.EntityValidationErrors) {
                //    err = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors) {
                //        err = err + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                //    }
                //}

                //result.Message1 = err;
            }
            //catch (DbEntityValidationException e) {
            //    result.Result = "fail";
            //    string err = "มีบางอย่างผิดลพลาดในไฟล์";
            //    foreach (var eve in e.EntityValidationErrors) {
            //        err = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors) {
            //            err = err + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }

            //    result.Message1 = err;
            //}

            return result;
        }

        #endregion
    }
}
