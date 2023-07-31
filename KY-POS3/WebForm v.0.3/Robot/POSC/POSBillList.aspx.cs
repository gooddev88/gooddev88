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
using System.Threading;
using static Robot.POSC.DA.POSSaleService;
using Robot.POSC.DA;
using Robot.Master.DA;

namespace Robot.POSC {
    public partial class POSBillList : MyBasePage {
        public static List<vw_POS_SaleHead> DocList { get { return (List<vw_POS_SaleHead>)HttpContext.Current.Session["posordsale_doclist"]; } set { HttpContext.Current.Session["posordsale_doclist"] = value; } }
        public static I_FilterSet Filter { get { return (I_FilterSet)HttpContext.Current.Session["posordsale_filter"]; } set { HttpContext.Current.Session["posordsale_filter"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {

                LoadDefaultFilter();
                LoadDropDownList();
                LoadData();

            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
            hddcompany.Value = Request.QueryString["com"];
            hddParentPage.Value = Request.QueryString["p_page"];
        }

        private void CheckPermission() {
            if (!PermissionService.CanDelete("F5991")) {

            }
        }

        private void BindData() {
            hddTopic.Value = "เช็คบิล " + "(" + hddmenu.Value + ")";
        }
        private void LoadDefaultFilter() {

            if (Filter == null) {
                Filter = POSSaleService.NewFilterSet();
            }

            cboTable.SelectedValue = Filter.Company;
         
            cboCompany.SelectedValue = Filter.Company;

        }
        private void SetDefaultFilter() {

            Filter = POSSaleService.NewFilterSet();
            Filter.Table = cboTable.SelectedValue;
        
            Filter.Company = cboCompany.SelectedValue;

        }

        private void LoadDropDownDevList() {
        }
        private void LoadDropDownList() {
            var datacom = CompanyService.ListCompanyInfoUIC("BRANCH", false).ToList();
            cboCompany.DataSource = datacom;
            try {
                if (cboCompany.DataSource != null) {
                    if (datacom.Count() != 0) {
                        cboCompany.SelectedIndex = 0;
                    }
                }
                cboCompany.DataBind();
            } catch {  }
            try {
                cboTable.DataSource = POSTableService.ListTable();
            cboTable.DataBind();
            } catch { }
        }


        private void LoadData() {
            SetDefaultFilter();
            BindData();
            DocList = POSSaleService.ListPendinbBill(Filter);
            GridBinding();
            SetActiveControl();
        }
        private void SetActiveControl() {
            CheckPermission();
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = DocList.OrderByDescending(o => o.INVID);
        }

        private void GridBinding() {
            grdDetail.DataSource = DocList.OrderByDescending(o => o.INVID);
            grdDetail.DataBind();

        }
        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }

        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
            var k = 1;
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "sel") {
                ASPxGridView grid = (ASPxGridView)sender;                
                string billId = e.KeyValue.ToString();
                POSSaleDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                POSSaleService.DocSet = POSSaleService.GetDocSet(billId);
                string myurl = $"~/POSC/POSSaleDetail?&menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void btnShowCancelOrder_Click(object sender, EventArgs e) {

            POSCancelBillList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
           
            string myurl = $"~/POSC/POSCancelBillList?&menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }
    }

}