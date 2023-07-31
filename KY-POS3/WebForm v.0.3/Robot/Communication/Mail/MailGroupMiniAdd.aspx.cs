
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

namespace Robot.Communication.Mail  {
    public partial class MailGroupMiniAdd : System.Web.UI.Page {
        public static string PreviousPage { get { return HttpContext.Current.Session["mgadd_previouspage"] == null ? "" : (string)HttpContext.Current.Session["mgadd_previouspage"]; } set { HttpContext.Current.Session["mgadd_previouspage"] = value; } }
        public static MailGroupReceiver MailGroupActive { get { return (MailGroupReceiver)HttpContext.Current.Session["mgactive"]; } set { HttpContext.Current.Session["mgactive"] = value; } }
        
         
        public static string MailGroupID { get { return HttpContext.Current.Session["mg_groupid"] == null ? "" : (string)HttpContext.Current.Session["mg_groupid"]; } set { HttpContext.Current.Session["mg_groupid"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
            CloseAlert();
          
            if (!IsPostBack) {

                LoadDropDownList();
           
                LoadData();

            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }
       
     
       

        private void BindData() {
            var mg =  MasterTypeInfoV2Service.GetType("MAIL_GROUP", MailGroupID);
            lblTopic.Text = "เพิ่มอีเมลในกลุ่ม";
            lblMailGroupInfo.Text = mg.Description1;
             
        }

        private void LoadDropDownList() {

        }
 

        private void LoadData() {
            if (MailGroupActive==null) {
                MailGroupActive = MailGroupInfoService.NewTransaction();
                MailGroupActive.MailGroupID = MailGroupID;
            }
                BindData(); 
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
           
            if (string.IsNullOrEmpty(MailGroupID)) {
                ShowAlert("Invalid mail group", "Error");
                return false;
            }
            if (txtemail.Text == "") {
                ShowAlert("Input Email", "Error");
                return false;
            }
   if (!MailGroupInfoService.IsValidEmail(txtemail.Text)) {
                ShowAlert("Email format error", "Error");
                return false;
            }
            var chkExits = MailGroupInfoService.GetMailInGroup(txtemail.Text, MailGroupID);
            if (chkExits != null) {
                ShowAlert("Duplicate Email", "Error");

                return false;
            }
            var user = UserInfoService.GetUserByEmail(txtemail.Text.Trim());
            if (user == null) {
                ShowAlert("ไม่พบอีเมล "+txtemail.Text+" ในฐานข้อมูลบุคคล", "Error");

                return false;
            }
            return result;
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
                ShowAlert("เพิ่มอีเมล "+txtemail.Text+" สำเร็จ", "Success");
            }

        }

        private void ResetInput() {
            txtemail.Text = "";
            txtRemark.Text = ""; 
            MailGroupActive = MailGroupInfoService.NewTransaction();
        }
        private bool PrepairDataSave() {
            bool isNew = true;

            MailGroupActive.MailGroupID = MailGroupID;
          

            MailGroupActive.Email = txtemail.Text.Trim();
            MailGroupActive.Remark = txtRemark.Text.Trim();
            return isNew;
        }


        protected void btnClose_Click(object sender, EventArgs e) {
            //string url = $"../Communication/Mail/MailGroupMini";
            //Response.Redirect(url);
            Response.Redirect(PreviousPage);
        }
         
       
    }
}