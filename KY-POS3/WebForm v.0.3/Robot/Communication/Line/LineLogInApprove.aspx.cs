
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
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
    public partial class LineLogInApprove : System.Web.UI.Page {
        
        

        public static string UserID { get { return HttpContext.Current.Session["regis_userid"] == null ? "" : (string)HttpContext.Current.Session["regis_userid"]; } set { HttpContext.Current.Session["regis_userid"] = value; } }
        public static string LineID { get { return  HttpContext.Current.Session["regis_lineid"] ==null?"":(string)HttpContext.Current.Session["regis_lineid"]; } set { HttpContext.Current.Session["regis_lineid"] = value; } }


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
            
            lblTopic.Text = "ลงชื่อขอใช้ POSSY ผ่าน Line";
            lblMailGroupInfo.Text = "ล๊อกอินด้วยรหัสพนักและรหัสผ่านเดียวกับ POSSY";
             
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
           
            if (string.IsNullOrEmpty(hddid.Value)) {
                ShowAlert("Line ID error please exit this page and try again", "Error");
                return false;
            } 
          
            return result;
        }

 

        protected void btnApprove_Click(object sender, EventArgs e) {
            if (!ValidateControl()) {
                return;
            }

            var r = LineRegisterService.ActionRequest("APPROVED","", txtMomo.Text);
            if (r.Result == "fail") {
                ShowAlert(r.Message1, "Error");
            } else {
                ShowAlert("ลงทะเบียนสำเร็จ รอผลการอนุมัติ", "Success");
            }
        }

        protected void btnReject_Click(object sender, EventArgs e) {

        }

        protected void btnClosed_Click(object sender, EventArgs e) {

        }
    }
}