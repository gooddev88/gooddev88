
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
    public partial class MailGroupManage : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["mg_previouspage"] == null ? "" : (string)HttpContext.Current.Session["mg_previouspage"]; } set { HttpContext.Current.Session["mg_previouspage"] = value; } }
        public static MailGroupReceiver MailGroupActive { get { return (MailGroupReceiver)HttpContext.Current.Session["mgactive"]; } set { HttpContext.Current.Session["mgactive"] = value; } }
        public static List<MailGroupReceiver> ListMailGroup { get { return (List<MailGroupReceiver>)HttpContext.Current.Session["mgdoc_list"]; } set { HttpContext.Current.Session["mgdoc_list"] = value; } }
        public static List<MasterTypeLine> ListGroup { get { return (List<MasterTypeLine>)HttpContext.Current.Session["mgroup_list"]; } set { HttpContext.Current.Session["mgroup_list"] = value; } }
        public static IFilterSet Filter { get { return (IFilterSet)HttpContext.Current.Session["mg_filter"]; } set { HttpContext.Current.Session["mg_filter"] = value; } }


        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
            CloseAlert();
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
            txtSearch.Text = Filter.SearchText;
            cboMailGroupFilter.Value = Filter.GroupID;
        }
        private void SetDefaultFilter() {
            Filter = MailGroupInfoService.NewFilter();
            Filter.SearchText = txtSearch.Text.Trim();
            if (cboMailGroupFilter.Value != null) {
                Filter.GroupID = cboMailGroupFilter.Value.ToString();
            }
        }

        private void BindData() {


            hddTopic.Value = "9211 Mail Group";


        }

        private void LoadDropDownList() {

        }

        private void LoadDropDownDevList() {
            ListGroup = MasterTypeInfoV2Service.ListMasterTypeLine("MAIL_GROUP", true);
            cboMailGroupID.DataSource = ListGroup;
            cboMailGroupID.DataBind();
            cboMailGroupFilter.DataSource = ListGroup;
            cboMailGroupFilter.DataBind();
        }

        private void ShowPopAlert(bool result) {
            popAlert.ShowOnPageLoad = true;
        }

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

        private void ShowAlert(string msg, string type) {

            lblAlertmsg.Text = msg;
            if (type == "Success") {
                lblAlertmsg.ForeColor = Color.Green;
            }
            if (type == "Error") {
                lblAlertmsg.ForeColor = Color.Red;
            }
            if (type == "Warning") {
                lblAlertmsg.ForeColor = Color.YellowGreen;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Del") {
                ASPxGridView grid = (ASPxGridView)sender;
                int id = Convert.ToInt32(e.CommandArgs.CommandArgument);
                var r = MailGroupInfoService.DeleteMailInGroup(id);
                LoadData();

            }
        }

        private bool ValidateControl() {
            bool result = true;
            if (cboMailGroupID.Value == null) {
                ShowAlert("Select Mail Group", "Error");
                return false;
            }

            if (txtemail.Text == "") {
                ShowAlert("Input Email", "Error");
                return false;
            }

            var chkExits = MailGroupInfoService.GetMailInGroup(txtemail.Text, cboMailGroupID.Value.ToString());
            if (chkExits != null) {
                ShowAlert("Duplicate Email", "Error");

                return false;
            }
            var validMail = MailGroupInfoService.GetMailInUser(txtemail.Text.Trim());
            if (validMail.Result == "fail") {
                ShowAlert(validMail.Message1, "Error");
                return false;
            }
            return result;
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            MailGroupActive = MailGroupInfoService.NewTransaction();
            ShowPopAlert(true);
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            if (!ValidateControl()) {
                return;
            }

            var isnew = PrepairDataSave();


            var r = MailGroupInfoService.Save(MailGroupActive);
            if (r.Result == "fail") {
                ShowAlert(r.Message1, "Error");
            } else {
                ResetInput();
            }

        }

        private void ResetInput() {
            txtemail.Text = "";
            txtRemark.Text = "";
            cboMailGroupID.Value = "";
            //btnClose.Visible = true;
        }
        private bool PrepairDataSave() {
            bool isNew = true;

            MailGroupActive.MailGroupID = "";
            if (cboMailGroupID.Value != null) {
                MailGroupActive.MailGroupID = cboMailGroupID.Value.ToString();
            }
            MailGroupActive.Email = txtemail.Text.Trim();
            MailGroupActive.Remark = txtRemark.Text.Trim();
            return isNew;
        }


        protected void btnClose_Click(object sender, EventArgs e) {
            ClosePopupAlert("OK");
        }

        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }

        private void ClosePopupAlert(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.OnClosePopupAlert('{0}');", command), true);
        }

        protected void btnCloseMailGroup_Click(object sender, EventArgs e) {
            LoadData();

        }
    }
}