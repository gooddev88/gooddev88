
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Communication.DA;
using Robot.Communication.Misc;
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
    public partial class LineLogInStep2 : System.Web.UI.Page {
        
        

        public static string UserID { get { return HttpContext.Current.Session["regis_userid"] == null ? "" : (string)HttpContext.Current.Session["regis_userid"]; } set { HttpContext.Current.Session["regis_userid"] = value; } }
        public static string LineID { get { return  HttpContext.Current.Session["regis_lineid"] ==null?"":(string)HttpContext.Current.Session["regis_lineid"]; } set { HttpContext.Current.Session["regis_lineid"] = value; } }
        public static UserInfo User { get { return (UserInfo)HttpContext.Current.Session["line_current_user"]; } set { HttpContext.Current.Session["line_current_user"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            CloseAlert();
            if (!IsPostBack) {
                LoadDropDownList();           
                LoadData();
            }
        }

        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
        }
       
     
       

        private void BindData() {

            lblTopic.Text = "สวัสดีค่ะ";
            lblUser.Text = "คุณ"+User.FullName;
            lblDescription.Text = "ขอต้อนรับสู่บริการของเรานะคะ ^_^";
             
        }

        private void LoadDropDownList() {

        }
 

        private void LoadData() {
       
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
            if (string.IsNullOrEmpty(UserID)) {
                ShowAlert("User ID error please exit this page and try again", "Error");
                return false;
            }
            return result;
        }


        protected void btnOk_Click(object sender, EventArgs e) {
            if (!ValidateControl()) {
                return;
            }
             
            var r = LineRegisterService.CreateUserLogInRequest(UserID, LineID, "KYPOS", txtMomo.Text);
            if (r.Result=="fail") {
                ShowAlert(r.Message1, "Error");
            } else {

                //ShowAlert("ลงทะเบียนสำเร็จ รอผลการอนุมัติ", "Success");
                SuccessPage.msg = "ส่งคำขอสำเร็จค่ะ รอเจ้าหน้าที่พิจารณาอนุมัตินะคะ";
                    string url = $"~/Communication/Misc/SuccessPage";
                Response.Redirect(url);

            }

        }


 
            
    }
}