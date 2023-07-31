
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;
using System.Drawing;
using static Robot.Data.BL.I_Result;
using OfficeOpenXml.Style;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using Dapper;
using System.Data.Entity.Validation;
using Robot.Master.DA;

namespace Robot.Master
{
    public partial class UploadFileItemPrice : MyBasePage {
        public static string PreviousPage { get { return (string)HttpContext.Current.Session["itempriceimport_previouspage"]; } set { HttpContext.Current.Session["itempriceimport_previouspage"] = value; } }
        private List<ItemPriceInfo> ImportExcelList { get { return (List<ItemPriceInfo>)HttpContext.Current.Session["excel_itempriceimport"]; } set { HttpContext.Current.Session["excel_itempriceimport"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void LoadData() {
            BindData();
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            string myurl = "";
            if (PreviousPage != null) {
                myurl = PreviousPage;
            } else {
                myurl = $"/Master/ItemPriceList?menu=9412";
            }
            Response.RedirectPermanent(myurl);
        }

        private void LoadDropDownList() {

        }

        private void BindData() {
           lblHeaderCaption.Text = "Import Excel ItemPrice ";
        }
        private void LoadDropDownDevList() {
        }

        private void ShowAlert(string msg, string type) {
            lblAlertBody.Text = msg;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", $"ShowAlert('{""}','{type}');", true);
        }

        private void CloseAlert() {
            lblAlertBody.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void btnUpload_Click(object sender, EventArgs e) {
            string err = ReadExcel();

            if (err != "") {
                ShowAlert("Error " + err, "Error");
            } else {
                string myurl = $"/Master/ItemPriceList?menu=9412";
                Response.RedirectPermanent(myurl);
            }

        }

        #region Excel Import 
        private string ReadExcel() {
            string error = "";

            try {
                string fileName = fileuploadx.FileName;
                string path = string.Concat(Server.MapPath($"../UploadFile/{fileuploadx.FileName}"));
                fileuploadx.SaveAs(path);
                using (ExcelPackage excelPackage = new ExcelPackage()) {
                    using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                        excelPackage.Load(fileStream);
                    }
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets[1];

                    if (excelWorksheet == null) {
                        ShowAlert("ไม่พบ Sheet TRIPRATE", "Error");
                    }
                    I_BasicResult re = new I_BasicResult();

                    re = ReadSheetTripRateFile(excelWorksheet);

                    if (re.Result == "fail") {
                        error = error + re.Message1;
                        return error;
                    }
                }

                var rs = SaveBulkV2(ImportExcelList);
                if (rs.Result == "fail") {
                    error = error + rs.Message1;
                }

            } catch (Exception ex) {
                if (ex.InnerException != null) {
                    error = error + ex.InnerException.ToString();
                } else {
                    error = error + ex.Message;

                }
            }
            return error;
        }

        private I_BasicResult ReadSheetTripRateFile(ExcelWorksheet ii) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            ImportExcelList = new List<ItemPriceInfo>();
            try {

                string error = "";
                string SheetName = "";
                if (ii == null) {
                    r.Result = "fail";
                    r.Message1 = "ไม่พบ Sheet name Data " + fileuploadx.FileName;
                    return r;
                }
                int totalRows = ii.Dimension.End.Row;
                int start_row = 2;
                int end_row = totalRows;
                int row_excel = 1;
                #region get start_row
                //for (row_excel = 1; row_excel <= totalRows; row_excel++) {
                //    //หากพบ row ที่มีตัวเลขให้ถือว่าเป็น Row ที่หนึ่งที่โปรแกรมต้องอ่าน (Column ลำดับ)
                //    int isfirst_rowno = 0;
                //    var chk = int.TryParse(ii.Cells[row_excel, 1].Text, out isfirst_rowno);
                //    if (chk) {
                //        start_row = row_excel;
                //        //end_row = totalRows - start_row;
                //        if (end_row <= 0) {//end_row ติดลบหรือเป็นศูนย์หมายถึงไม่มีข้อมูล
                //            r.Result = "fail";
                //            r.Message1 = "ไม่พบข้อมูลในไฟล์ " + fileuploadx.FileName;
                //            return r;
                //        } else {//หา start row ได้แล้วให้หยุด Loop
                //            break;
                //        }
                //    }

                //}
                #endregion



                var culture = new CultureInfo("en-US");
                row_excel = 1;//reset row
                for (row_excel = start_row; row_excel <= end_row; row_excel++) {
                    var n = POSItemService.NewPrice();

                    try {
                        n.CompanyID = ii.Cells[row_excel, 1].Text == null ? "" : ii.Cells[row_excel, 1].Text.Trim();
                    } catch {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column CompanyID ห้ามว่างๆ";
                    }

                    try
                    {
                        n.ItemID = ii.Cells[row_excel, 2].Text == null ? "" : ii.Cells[row_excel, 2].Text.Trim();
                    }
                    catch
                    {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column ItemID ห้ามว่างๆ";
                    }

                    try
                    {
                        n.CustID = ii.Cells[row_excel, 3].Text == null ? "" : ii.Cells[row_excel, 3].Text.Trim();
                    }
                    catch
                    {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column CustID ห้ามว่างๆ";
                    }

                    try
                    {
                        n.UseLevel = ii.Cells[row_excel, 4].Text == "" ? 0 : Convert.ToInt32(ii.Cells[row_excel, 4].Text.Trim());
                        if (n.UseLevel < 0 && n.UseLevel > 1)
                        {
                            error = error + "<br>" + $" ? {SheetName}:row {row_excel} column UseLevel ใส่เฉพาะตัวเลข 0 หรือ 1 เท่านั้น";
                        }
                    }
                    catch
                    {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column UseLevel ใส่เฉพาะตัวเลข";
                    }

                    try
                    {
                        n.DateBegin = DateTime.ParseExact(ii.Cells[row_excel, 5].Text, "dd/MM/yyyy", culture);
                    }
                    catch
                    {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column DateBegin -> format วันที่ไม่ถูกต้อง";
                    }

                    try
                    {
                        n.DateEnd = DateTime.ParseExact(ii.Cells[row_excel, 6].Text, "dd/MM/yyyy", culture);
                    }
                    catch
                    {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column DateEnd -> format วันที่ไม่ถูกต้อง";
                    }

                    try
                    {
                        n.Price = ii.Cells[row_excel, 7].Text == "" ? 0 : Convert.ToDecimal(ii.Cells[row_excel, 7].Text.Trim());
                    }
                    catch
                    {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column Price ใส่เฉพาะตัวเลข";
                    }

                    try
                    {
                        n.PriceTaxCondType = ii.Cells[row_excel, 8].Text.ToUpper() == null ? "" : ii.Cells[row_excel, 8].Text.ToUpper();
                        if (n.PriceTaxCondType != "INC VAT")
                        {
                            if (n.PriceTaxCondType != "EXC VAT")
                            {
                                error = error + "<br>" + $" ? {SheetName}:row {row_excel} column PriceTaxCondType ใส่ได้เฉพาะ EXC VAT หรือ INC VAT เท่านั้น";
                            }               
                        }
                        if (n.PriceTaxCondType != "EXC VAT")
                        {
                            if (n.PriceTaxCondType != "INC VAT")
                            {
                                error = error + "<br>" + $" ? {SheetName}:row {row_excel} column PriceTaxCondType ใส่ได้เฉพาะ EXC VAT หรือ INC VAT เท่านั้น";
                            }
                        }
                    }
                    catch
                    {
                        error = error + "<br>" + $" ? {SheetName}:row {row_excel} column PriceTaxCondType ห้ามว่างๆ";
                    }


                    if (error != "") {
                        r.Result = "fail";
                    }

                    ImportExcelList.Add(n);
                }
                r.Message1 = error;

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

        public static I_BasicResult SaveBulkV2(List<ItemPriceInfo> datalist)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    foreach (var l in datalist)
                    {
                        var uh = db.ItemPriceInfo.Where(o => o.RCompanyID == l.RCompanyID 
                                                            && o.CompanyID == l.CompanyID
                                                            && o.ItemID == l.ItemID
                                                            && o.CustID == l.CustID
                                                            ).FirstOrDefault();
                        if (uh == null)
                        {//insert
                            db.ItemPriceInfo.Add(l);
                            db.SaveChanges();
                        }
                        else
                        {//update
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
            }
            catch (DbEntityValidationException e)
            {
                result.Result = "fail";
                string err = "มีบางอย่างผิดลพลาดในไฟล์";
                foreach (var eve in e.EntityValidationErrors)
                {
                    err = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err = err + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }

                result.Message1 = err;
            }

            return result;
        }

        #endregion

    }
}