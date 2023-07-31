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

namespace Robot.Master
{
    public partial class MyUserList : MyBasePage {
        public static string PreviousListPage { get { return HttpContext.Current.Session["masuserlist_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masuserlist_previouspage"]; } set { HttpContext.Current.Session["masuserlist_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString2HiddenFiled();
            LoadDropDownDevList();
            if (!IsPostBack) {

                LoadDefaultFilter();
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString2HiddenFiled() {
            hddmenu.Value = Request.QueryString["menu"];         
        }
        private void LoadDefaultFilter() {
            if (MyUserInfoService.FilterSet == null) {
                MyUserInfoService.NewFilterSet();
            }
            var f = MyUserInfoService.FilterSet;
            txtSearch.Text = f.SearchText;
            chkShowDisable.Checked = f.ShowDisAble;        
        }
        private void SetActiveControl() {
            lblTopic.Text = PermissionService.GetMenuInfo("9002").Name;
        }

        private void SetDefaultFilter() {
            var f = MyUserInfoService.FilterSet;
            f.SearchText = txtSearch.Text; 
            f.ShowDisAble = chkShowDisable.Checked; 
        }

        private void LoadDropDownList() {
             
        }

        private void LoadDropDownDevList() {

        }


        private void LoadData() {
            try {
                SetDefaultFilter();
                MyUserInfoService.ListDoc(); 
                SetActiveControl(); 
                grdDetail.DataSource = MyUserInfoService.DocList;
                grdDetail.DataBind();

            } catch (Exception ex) {
            }
        }


        private void GridBinding() {
            grdDetail.DataSource = MyUserInfoService.DocList;
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
              
                MyUserDetail.XUserID = e.KeyValue.ToString(); ;
                MyUserDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"../Master/MyUserDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            MyUserInfoService.NewTransaction();
            MyUserDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            MyUserDetail.XUserID = "";
            string myurl = $"../Master/MyUserDetail?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void chkShowDisable_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(PreviousListPage);
        }
    }

}