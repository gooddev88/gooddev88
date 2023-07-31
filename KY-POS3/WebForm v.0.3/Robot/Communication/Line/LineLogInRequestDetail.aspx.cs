
using Robot.Communication.API.Line;
using Robot.Communication.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Drawing;
using System.Web;
using System.Web.UI;

namespace Robot.Communication.Line {
    public partial class LineLogInRequestDetail : System.Web.UI.Page {

        public static string PreviousPage { get { return HttpContext.Current.Session["app_req_detail_previouspage"] == null ? "" : (string)HttpContext.Current.Session["app_req_detail_previouspage"]; } set { HttpContext.Current.Session["app_req_detail_previouspage"] = value; } }
        public static LineLogInRequest ReqData { get { return (LineLogInRequest)HttpContext.Current.Session["app_req_info"]; } set { HttpContext.Current.Session["app_req_info"] = value; } }


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
        private void SetActiveControl() {
            if (ReqData.ActionStatus != "PENDING") {
                btnApprove.Visible = false;
                btnReject.Visible = false;
            }
           
        }


        private void BindData() {
            var u = UserService.GetVievUserInfo(ReqData.UserID);
            lblTopic.Text = "คำขอใช้งานผ่าน LINE จาก";
            lblDescription.Text = u.FullName; //+ " จากหน่วยงาน " + u.CompanyName;
            lblDescriptio2.Text = "เมื่อวันที่ " + ReqData.RequestDate.ToString("dd/MM/yyyy");
            lblRequestMeomo.Text = ReqData.ReqMemo;
        }

        private void LoadDropDownList() {

        }


        private void LoadData() {

            BindData();
            SetActiveControl();
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



        private bool ValidateControl(string action) {
            bool result = true;
            if (action.ToLower() == "rejected") {
                if (txtMomo.Text.Trim() == "") {
                    ShowAlert("บอกเหตุผลการปฏิเสธ เค้าหน่อยซิ", "Error");
                    return false;
                }
            }
            return result;
        }

        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }

        protected void btnApprove_Click(object sender, EventArgs e) {
             

            var r = LineRegisterService.ActionRequest("ACCEPTED", ReqData.ReqID, txtMomo.Text);
            if (r.Result == "fail") {
                ShowAlert(r.Message1, "Error");
            } else {

                var myMsg = LineMsgAPI.MessageHelper("ท่านได้รับการอนุมัติให้ใช้บริการ KY POS แล้วค่ะ " + "\\n" + txtMomo.Text);
                LineMsgAPI.SendLineMsgAPI(myMsg, ReqData.LineID, "KYPOS");
                Response.Redirect(PreviousPage);
              
            }
        }


        protected void btnReject_Click(object sender, EventArgs e) {
            if (!ValidateControl("reject")) {
                return;
            }
            var r = LineRegisterService.ActionRequest("REJECTED", ReqData.ReqID, txtMomo.Text);
            if (r.Result == "fail") {
                ShowAlert(r.Message1, "Error");
            } else {
                var myMsg = LineMsgAPI.MessageHelper("เสียใจด้วยค่ะ ขอปฏิเสธคำขอใช้บริการ KYPOS เนื่องจาก" + "\\n" + txtMomo.Text);
                LineMsgAPI.SendLineMsgAPI(myMsg, ReqData.LineID, "KYPOS");
                Response.Redirect(PreviousPage);

            }
        }

    }
}