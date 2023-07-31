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
    public partial class MyUserGroupList : MyBasePage {
        public static string PreviousListPage { get { return HttpContext.Current.Session["masusergrouplist_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masusergrouplist_previouspage"]; } set { HttpContext.Current.Session["masusergrouplist_previouspage"] = value; } }
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
            if (UserGroupInfoV2Service.FilterSet == null) {
                UserGroupInfoV2Service.NewFilterSet();
            }
            var f = UserGroupInfoV2Service.FilterSet;
            txtSearch.Text = f.SearchText;
            chkShowDisable.Checked = f.ShowDisAble;    
        }
        private void SetActiveControl() {
            lblTopic.Text = PermissionService.GetMenuInfo("9003").Name;
        }

        private void SetDefaultFilter() {
            var f = UserGroupInfoV2Service.FilterSet;
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
                UserGroupInfoV2Service.ListDoc(); 
                SetActiveControl(); 
                grdDetail.DataSource = UserGroupInfoV2Service.DocList;
                grdDetail.DataBind();

            } catch (Exception ex) {
            }
        }

        private void GridBinding() {
            grdDetail.DataSource = UserGroupInfoV2Service.DocList;
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
                string id = e.KeyValue.ToString();
                MyUserGroupDetail.XUsergroupID = id;
               MyUserGroupDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"../Master/MyUserGroupDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            UserGroupInfoV2Service.NewTransaction();
            MyUserGroupDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            MyUserGroupDetail.XUsergroupID = "";
            string myurl = $"../Master/MyUserGroupDetail?menu={hddmenu.Value}";
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