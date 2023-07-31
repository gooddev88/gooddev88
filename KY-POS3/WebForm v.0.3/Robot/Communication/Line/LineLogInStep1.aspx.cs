
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Communication.API.Line;
using Robot.Communication.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Communication.Line  {
    public partial class LineLogInStep1 : System.Web.UI.Page {
        
        

        public static string LineID { get { return  HttpContext.Current.Session["regis_lineid"] ==null?"":(string)HttpContext.Current.Session["regis_lineid"]; } set { HttpContext.Current.Session["regis_lineid"] = value; } }
        public static List<LineLogInRequest> ReqPending { get { return   (List<LineLogInRequest>)HttpContext.Current.Session["req_pending_list"]; } set { HttpContext.Current.Session["req_pending_list"] = value; } }
        
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            CloseAlert();
            if (!IsPostBack) {
                LoadDropDownList();           
                LoadData();
            }
        }

        private void SetQueryString() {
            //hddid.Value = Request.QueryString["id"];
        }
       
     
       

        private void BindData() {
            lblTopic.Text = "ยืนยันข้อมูลก่อนผูกสัมพันธ์";
            lblDescription.Text = "ล๊อกอินด้วย รหัสพนักงานและรหัสผ่านเดียวกับ KYPOS นะจ้าาา";

            if (ReqPending .Count>0 ) {
                divPending.Visible = true;
                divLogin.Visible = false;

            } else {
                divPending.Visible = false;
                divLogin.Visible = true;
            } 
        }

        private void LoadDropDownList() {

        }
 

        private void LoadData() {
            ReqPending = LineRegisterService.ListPendingReqWithLineID(LineID, "USER");
     
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

       

        private bool ValidateControl() {
            bool result = true;
           
            if (string.IsNullOrEmpty(LineID)) {
                ShowAlert("Line ID error please exit this page and try again", "Error");
                return false;
            } 
          
            return result;
        }


        protected void btnOk_Click(object sender, EventArgs e) {
     
            if (!ValidateControl()) {
                return;
            } 
            var u = LineRegisterService.GetUserInfoWithAuthen(txtusername.Text.Trim(),txtPassword.Text);
            if (u==null) {
                ShowAlert("รหัสพนักงานหรือรหัสผ่านไม่ถูกต้องจ้าา", "Error");
            } else {
                LineLogInStep2.UserID = u.Username;
                LineLogInStep2.LineID = LineID;
                LineLogInStep2.User = u;
                string url = $"~/Communication/Line/LineLogInStep2";
                Response.Redirect(url);
            }
        }   
    }
}