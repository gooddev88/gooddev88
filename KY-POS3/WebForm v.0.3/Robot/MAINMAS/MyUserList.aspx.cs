using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using DevExpress.Web;
using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;

namespace Robot.MAINMAS {
    public partial class MyUserList : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["masuserlist_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masuserlist_previouspage"]; } set { HttpContext.Current.Session["masuserlist_previouspage"] = value; } }
        public static List<vw_UserInfo> DocList { get { return (List<vw_UserInfo>)HttpContext.Current.Session["userlist"]; } set { HttpContext.Current.Session["userlist"] = value; } }
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
        }

        private void LoadDefaultFilter() {
            if (UserService.FilterSet == null) {
                UserService.NewFilterSet();
            }
            var f = UserService.FilterSet;
            txtSearch.Text = f.SearchText;
            chkShowDisable.Checked = f.ShowDisAble;
        }
        private void SetActiveControl() {

        }

        private void SetDefaultFilter() {
            var f = UserService.FilterSet;
            f.SearchText = txtSearch.Text;
            f.ShowDisAble = chkShowDisable.Checked;
        }

        private void LoadDropDownList() {

        }

        private void LoadDropDownDevList() {

        }
        private void BindData() {
            lblTopic.Text = PermissionService.GetMenuInfo("9002").Name;
        }


        private void LoadData() {
            SetDefaultFilter();
            DocList= UserService.ListDoc();
            GridBinding();
            BindData();
            SetActiveControl();
        }


        private void GridBinding() {
            grdDetail.DataSource = DocList;
            grdDetail.DataBind();
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            SetDefaultFilter();
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Select") {
                ASPxGridView grid = (ASPxGridView)sender;
                string user = e.KeyValue.ToString();
                UserService.GetDocSetByID(user);

                MyUserDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"../MAINMAS/MyUserDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {

            grdDetail.DataSource = DocList;
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            UserService.NewTransaction();
            MyUserDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;

            string myurl = $"../MAINMAS/MyUserDetail?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void chkShowDisable_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousPage);
        }
    }

}