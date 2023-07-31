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
 

namespace Robot.OMASTER {
    public partial class CompanyList : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["comlist_previouspage"] == null ? "" : (string)HttpContext.Current.Session["comlist_previouspage"]; } set { HttpContext.Current.Session["comlist_previouspage"] = value; } }
        public static List<vw_CompanyInfo> DocList { get { return (List<vw_CompanyInfo>)HttpContext.Current.Session["comdoc_list"]; } set { HttpContext.Current.Session["comdoc_list"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popAlert.ShowOnPageLoad = false;
            SetQueryString2HiddenFiled();
            if (!IsPostBack) {
                if (CompanyService.FilterSet == null)
                {
                    CompanyService.NewFilterSet();
                }
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString2HiddenFiled()
        {
            hddmenu.Value = Request.QueryString["menu"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }

        private void LoadDefaultFilter() {
            txtSearch.Text = CompanyService.FilterSet.SearchText;
            chkShowClose.Checked = !CompanyService.FilterSet.ShowActive;
        }
        private void SetDefaultFilter() {
            #region default caption
            lblinfohead.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            #endregion

            CompanyService.NewFilterSet();
            CompanyService.FilterSet.ShowActive = !chkShowClose.Checked;
            CompanyService.FilterSet.SearchText = txtSearch.Text;
        }

        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        private void LoadDropDownList() {

            var itemtype = MasterTypeService.ListType("ITEM TYPE", true).ToList();
         
            cboType.DataSource = itemtype;
            cboType.DataBind();
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
         
                SetDefaultFilter();
            DocList=    CompanyService.ListDoc();
                BindData();
                BindGrid();
            SetActiveControl();
        }

        private void SetActiveControl()
        {
            CheckPermission();
        }
        private void CheckPermission()
        {

            if (!PermissionService.CanOpen(hddmenu.Value))
            {
                Response.Redirect("~/Default");
            }

        }
        private void BindGrid()
        {
            grdDetail.DataSource = DocList;
            grdDetail.DataBind();
        }
        private void BindData()
        {

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

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "sel") {
                ASPxGridView grid = (ASPxGridView)sender;             
                string id = e.KeyValue.ToString();

                CompanyDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                CompanyService.GetDocSetByID(id);
                string myurl = $"../MAINMAS/CompanyDetail?menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        protected void btnNew_Click(object sender, EventArgs e) {

            CompanyService.NewTransaction("");
            CompanyService.DocSet.ComInfo.TypeID = "BRANCH";
            CompanyDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"../MAINMAS/CompanyDetail?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

    }

}