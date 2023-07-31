
using Robot.Communication.API.Line;
using Robot.Communication.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Drawing;
using System.Web;
using System.Web.UI;

namespace Robot.Communication.Line {
    public partial class LineLogInDetail : System.Web.UI.Page {

        public static string PreviousPage { get { return HttpContext.Current.Session["app_linelogin_detail_previouspage"] == null ? "" : (string)HttpContext.Current.Session["app_linelogin_detail_previouspage"]; } set { HttpContext.Current.Session["app_linelogin_detail_previouspage"] = value; } }
        public static vw_LineLogIn LineLoginData { get { return (vw_LineLogIn)HttpContext.Current.Session["vw_linelogin_info"]; } set { HttpContext.Current.Session["vw_linelogin_info"] = value; } }


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
           
            lblTopic.Text = "ข้อมูลการผูกบัญชี LINE OA จาก";
            lblDescription.Text = LineLoginData.UserFullName;/* " จากหน่วยงาน " + LineLoginData.UserCompanyName;*/
            lblDescriptio2.Text = "ขอเมื่อวันที่ " + LineLoginData.RequestDate.ToString("dd/MM/yyyy H:mm");

            if (LineLoginData.ApprovedDate != null)
            {
                lblDescriptio2.Text = lblDescriptio2.Text + "อนุมัติเมื่อวันที่ " + Convert.ToDateTime(LineLoginData.ApprovedDate).ToString("dd/MM/yyyy H:mm");
            }
            lblRequestMeomo.Text = LineLoginData.ApprovedMemo;
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


 

        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }

      

        protected void btnDelete_Click(object sender, EventArgs e) {
            var r = LineRegisterService.DeleteLineLogIn(LineLoginData.ID, "KYPOS");
            if (r.Result=="fail") {
                ShowAlert("Error " + r.Message1,"Error");
            } else {
                var myMsg = LineMsgAPI.MessageHelper("คุณถูกยกเลิกบริการ Line OA แล้ว" + "\\n" + "T_T");
                LineMsgAPI.SendLineMsgAPI(myMsg, LineLoginData.LineID, "BS");
                Response.Redirect(PreviousPage);
            }
           
           

        }

    }
}