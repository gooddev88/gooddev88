
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Communication.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.HR;
using Robot.HREmpData.Data.DA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.Communication.DA.MailGroupInfoService;

namespace Robot.Communication.Mail  {
    public partial class MailGroupMini : System.Web.UI.Page {
        public static string PreviousPage { get { return HttpContext.Current.Session["mg_previouspage"] == null ? "" : (string)HttpContext.Current.Session["mg_previouspage"]; } set { HttpContext.Current.Session["mg_previouspage"] = value; } }
        
        public static List<MailGroupReceiver> ListMailGroup { get { return (List<MailGroupReceiver>)HttpContext.Current.Session["mgdoc_list"]; } set { HttpContext.Current.Session["mgdoc_list"] = value; } }
        public static List<MasterTypeLine> ListGroup { get { return (List<MasterTypeLine>)HttpContext.Current.Session["mgroup_list"]; } set { HttpContext.Current.Session["mgroup_list"] = value; } }
        public static IFilterSet Filter { get { return (IFilterSet)HttpContext.Current.Session["mg_filter"]; } set { HttpContext.Current.Session["mg_filter"] = value; } }
        public static string MailGroupID { get { return HttpContext.Current.Session["mg_groupid"] == null ? "" : (string)HttpContext.Current.Session["mg_groupid"]; } set { HttpContext.Current.Session["mg_groupid"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
  
            LoadDropDownDevList();
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
            Response.Redirect(PreviousPage);
        }

        private void LoadDefaultFilter() {
            if (Filter == null) {
                Filter = MailGroupInfoService.NewFilter();
            }
         
        }
        private void SetDefaultFilter() {
            Filter = MailGroupInfoService.NewFilter();
            Filter.GroupID = MailGroupID;
        }

        private void BindData() {


            var mg = MasterTypeInfoV2Service.GetType("MAIL_GROUP", MailGroupID);
            lblTopic.Text = "รายการอีเมลในกลุ่ม";
            lblMailGroupInfo.Text = mg.Description1;

        }

        private void LoadDropDownList() {

        }

        private void LoadDropDownDevList() {
            //ListGroup = MasterTypeInfoV2Service.ListMasterTypeLine("MAIL_GROUP", true);
            //cboMailGroupID.DataSource = ListGroup;
            //cboMailGroupID.DataBind();
           
        }

        //private void ShowPopAlert(bool result) {
        //    popAlert.ShowOnPageLoad = true;
        //}

        private void LoadData() {
    
                SetDefaultFilter();

                ListMailGroup = MailGroupInfoService.ListMailGroupReceiver(Filter);

                BindGrid();
                BindData();
    

        }

        private void BindGrid() {
            grdDetail.DataSource = ListMailGroup;
            grdDetail.DataBind();
        }
        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = ListMailGroup;
        }



        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }

        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
 

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Del") {
                ASPxGridView grid = (ASPxGridView)sender;
                int id = Convert.ToInt32(e.CommandArgs.CommandArgument);
                var r = MailGroupInfoService.DeleteMailInGroup(id);
                LoadData();

            }
        }
 

        protected void btnNew_Click(object sender, EventArgs e) {
            MailGroupMiniAdd.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery; 
            MailGroupMiniAdd.MailGroupID = MailGroupID;
         string     url=   $"/Communication/Mail/MailGroupMiniAdd";
            Response.Redirect(url); 
        }

     
  


        protected void btnClose_Click(object sender, EventArgs e) {
 
            string url = $"../Communication/Mail/MailGroupManage";
            Response.Redirect(url);
        }

   
        protected void btnCloseMailGroup_Click(object sender, EventArgs e) {
            LoadData();

        }
    }
}