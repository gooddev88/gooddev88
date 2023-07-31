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
using Robot.POS.DA;
using static Robot.Data.DataAccess.LocationInfoService;

namespace Robot.POS
{
    public partial class StockMovementList : MyBasePage
    {

        public class Filter
        {
            public DateTime begin { get; set; }
            public DateTime end { get; set; }
            public string Search { get; set; }
        }

        public static string PreviousFromBalanceListPage { get { return HttpContext.Current.Session["stkmovementfromBalanceList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["stkmovementfromBalanceList_previouspage"]; } set { HttpContext.Current.Session["stkmovementfromBalanceList_previouspage"] = value; } }
        public static string PreviousListPage { get { return HttpContext.Current.Session["stkmovementList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["stkmovementList_previouspage"]; } set { HttpContext.Current.Session["stkmovementList_previouspage"] = value; } }
        public static List<vw_POS_STKMove> DocList { get { return (List<vw_POS_STKMove>)HttpContext.Current.Session["stkmovement_list"]; } set { HttpContext.Current.Session["stkmovement_list"] = value; } }
        public static string ITEMID { get { return HttpContext.Current.Session["stkmovement_itemid"] == null ? "" : (string)HttpContext.Current.Session["stkmovement_itemid"]; } set { HttpContext.Current.Session["stkmovement_itemid"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            if (PreviousListPage == "") {
                Response.RedirectPermanent(PreviousFromBalanceListPage);
            } else {
                Response.RedirectPermanent(PreviousListPage);
            }
        }

        private void LoadDefaultFilter() {
            if (POSStockService.FilterSet == null) {
                POSStockService.NewFilterSet();
            }
            var f = POSStockService.FilterSet;
            dtBegin.Value = f.DateFrom;
            dtEnd.Value = f.DateTo;
            txtSearch.Text = f.SearchText;
            if (f.Company != "") {
                cboCompany.SelectedValue = f.Company;
                BindLocation();
                try {
                    cboLocation.SelectedValue = f.Location;
                } catch {

                }
            }
          //  cboLocation.SelectedValue = f.Location == "" ? "X" : f.Location;
        }
        private void SetDefaultFilter() { 
            POSStockService.FilterSet.SearchText = txtSearch.Text;
            POSStockService.FilterSet.DateFrom = dtBegin.Date;
            POSStockService.FilterSet.DateTo = dtEnd.Date;
            POSStockService.FilterSet.Company = cboCompany.SelectedValue;
            POSStockService.FilterSet.SearchItem = ITEMID;
            POSStockService.FilterSet.Location = cboLocation.SelectedValue;
        }
        private void BindData() {
            lblinfohead.Text = "สต็อก เคลื่อนไหว";

            if (ITEMID != "") {
                divhide.Visible = false;
            }
        }
        private void LoadDropDownList() {
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", false);
            cboCompany.DataBind();
       //     var location = LocationInfoService.ListStockLocation("", cboCompany.SelectedValue, true);        
            BindLocation();
        }

        private void LoadData() {

            SetDefaultFilter();
            DocList = POSStockService.ListViewStockMove();
            BindData();
            BindGrid();

        }

        private void BindGrid() {
            grdDetail.DataSource = DocList;
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = DocList;
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }

        protected void cboCompany_SelectedIndexChanged(object sender, EventArgs e) {
            BindLocation();
        }
        private void BindLocation() {
            cboLocation.DataSource = LocationInfoService.ListStockLocation("", cboCompany.SelectedValue, true);
            cboLocation.DataBind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            gridExport.FileName = "รายการคลื่อนไหว_" + DateTime.Now.ToString("dd/MM/yyyy");
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }

            if (showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            POSBOMService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }
        protected void btnRecalStock_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
        //    var com = LoginService.LoginInfo.CurrentCompany.CompanyID;

         var r=   POSStockService.RecalStock(POSStockService.FilterSet.DateFrom, POSStockService.FilterSet.DateTo, rcom, POSStockService.FilterSet.Company);
            if (r.Result=="fail") {
                ShowPopAlert("Warning", r.Message1, false, "");
            } else {
                ShowPopAlert("Success", "คำนวณเสร็จแล้ว", true, "");
            }   
        }
    }

}