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

namespace Robot.MAINMAS
{
    public partial class MyUserGroupList : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["masusergrouplist_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masusergrouplist_previouspage"]; } set { HttpContext.Current.Session["masusergrouplist_previouspage"] = value; } }
        public static List<vw_UserGroupInfo> DocList { get { return (List<vw_UserGroupInfo>)HttpContext.Current.Session["usergrouplist"]; } set { HttpContext.Current.Session["usergrouplist"] = value; } }
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
            if (UserGroupService.FilterSet == null) {
                UserGroupService.NewFilterSet();
            }
            var f = UserGroupService.FilterSet;
            txtSearch.Text = f.SearchText;
            chkShowDisable.Checked = f.ShowDisAble;    
        }
        private void SetActiveControl() {
            lblTopic.Text = PermissionService.GetMenuInfo("9003").Name;
        }

        private void SetDefaultFilter() {
            var f = UserGroupService.FilterSet;
            f.SearchText = txtSearch.Text; 
            f.ShowDisAble = chkShowDisable.Checked; 
        }

        private void LoadDropDownList() {
             
        }

        private void LoadDropDownDevList() {

        }

        private void LoadData() {           
                SetDefaultFilter();
               DocList= UserGroupService.ListDoc();
            GridBinding();
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
                string id = e.KeyValue.ToString();
                UserGroupService.GetDocSetByID(id);
                MyUserGroupDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"../MAINMAS/MyUserGroupDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = DocList;
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            UserGroupService.NewTransaction();
            MyUserGroupDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;            
            string myurl = $"../MAINMAS/MyUserGroupDetail?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void chkShowDisable_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.Redirect(PreviousPage);
        }
    }

}