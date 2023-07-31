using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;


using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;

using Robot.Master.DA;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Robot.Master.Upload;

namespace Robot.Master
{
    public partial class ItemPriceList : MyBasePage {

        public class I_ItemPriceSet
        {
            public String Search { get; set; }
            public String ItemID { get; set; }
            public String PriceTaxcon { get; set; }
            public String Company { get; set; }
            public String ShipTo { get; set; }
            public String UseLevel { get; set; }
        }

        public static I_ItemPriceSet FilterSet { get { return (I_ItemPriceSet)HttpContext.Current.Session["filterset_itemprice"]; } set { HttpContext.Current.Session["filterset_itemprice"] = value; } }
        public static List<vw_ItemPriceInfo> DocList { get { return (List<vw_ItemPriceInfo>)HttpContext.Current.Session["vw_itemprice_list"]; } set { HttpContext.Current.Session["vw_itemprice_list"] = value; } }
        public static string PreviousListPage { get { return HttpContext.Current.Session["itempriceList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["itempriceList_previouspage"]; } set { HttpContext.Current.Session["itempriceList_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popAlert.ShowOnPageLoad = false;
            SetQueryString2HiddenFiled();
            if (!IsPostBack) {               
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString2HiddenFiled() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void NewFilterSet()
        {
            FilterSet = new I_ItemPriceSet
            {
                PriceTaxcon = "",
                Search = "",
                ItemID = "",
                ShipTo = "",
                UseLevel = "",
                Company = ""

            };
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousListPage);
        }

        private void LoadDefaultFilter() {

            NewFilterSet();

            txtSearch.Text = FilterSet.Search;
            cboItem.Value = FilterSet.ItemID;
            cboCompanyID.SelectedValue = FilterSet.Company;
            cboPriceTaxcon.SelectedValue = FilterSet.PriceTaxcon;
            cboShipTo.SelectedValue = FilterSet.ShipTo;
            cboUseLevel.SelectedValue = FilterSet.UseLevel;
        }
        private void SetDefaultFilter() {

            FilterSet.Search = txtSearch.Text.ToUpper();
            FilterSet.Company = cboCompanyID.SelectedValue;
            FilterSet.PriceTaxcon = cboPriceTaxcon.SelectedValue;
            FilterSet.UseLevel = cboUseLevel.SelectedValue;
            FilterSet.ShipTo = cboShipTo.SelectedValue;
            FilterSet.Search = txtSearch.Text.ToUpper();
            if (cboItem.Value == null)
            {
                FilterSet.ItemID = "";
            }
            else
            {
                FilterSet.ItemID = cboItem.Value.ToString();
            }
        }

        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        private void LoadDropDownList() {
            cboPriceTaxcon.DataSource = MasterTypeService.ListType("SALE RPICE TAX CON", true);
            cboPriceTaxcon.DataBind();
            cboCompanyID.DataSource = CompanyService.ListBranch(true);
            cboCompanyID.DataBind();

            cboShipTo.DataSource = POSItemService.ListShipTo();
            cboShipTo.DataBind();
        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }
            if (showbutton == "") {
                btnCancel.Visible = false;
            }
            if (showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            popAlert.ShowOnPageLoad = true;
        }

        private void LoadData() {
            try {
                SetDefaultFilter();
                DocList = POSItemService.ListItemPrice(FilterSet);
                BindData();
                BindGrid();
            } catch (Exception ex) {
            }
        }
        private void BindGrid() {
            grdDetail.DataSource = DocList;
            grdDetail.DataBind();
        }
        private void BindData() {
            lblinfohead.Text = "ราคาสินค้า";
        }
        private void GridBinding() {
            grdDetail.DataSource = DocList; 
        }
        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {

        }

        protected void cboItem_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT [ItemID],[Name1],[TypeID] FROM  ( SELECT [ItemID],[Name1],[TypeID] , row_number()over(order by [ItemID] desc) as [rn]  FROM [ItemInfo]  as t where (( [ItemID]+[Name1]+[TypeID] ) LIKE @filter) and [IsActive]=1) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboItem_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [ItemID],[Name1],[TypeID] FROM [ItemInfo] WHERE ([ItemID] = @ID) ORDER BY [ItemID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        #region excel

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DownloadTripRate();
        }
        private void DownloadTripRate()
        {
            var ListItemPrice = POSItemService.ListViewItemPriceInfoALL();

            using (var excelPackage = new ExcelPackage())
            {
                int row_excel = 2;
                ExcelWorksheet excelWorksheet1 = excelPackage.Workbook.Worksheets.Add("DATA");
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

                foreach (var e in ListItemPrice)
                {
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
                using (var memoryStream = new MemoryStream())
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";

                    Response.AddHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileName + ".xlsx");
                    excelPackage.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        #endregion

        protected void btnImport_Click(object sender, EventArgs e)
        {
            UploadFileItemPrice.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"../Master/UploadFileItemPrice";
            Response.RedirectPermanent(myurl);
        }

    }

}