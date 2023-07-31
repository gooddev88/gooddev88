
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Communication.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Report;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Communication.Line  {


    public partial class LineMenuAgent : System.Web.UI.Page {
        
       //public class UserX : UserInfo
       // {
       //     public string ChlidenName { get; set; }
       // }

        public static string InnerUserID { get { return HttpContext.Current.Session["inner_userid"] == null ? "" : (string)HttpContext.Current.Session["inner_userid"]; } set { HttpContext.Current.Session["inner_userid"] = value; } }
        public static string OuterUserID { get { return  HttpContext.Current.Session["regis_lineid"] ==null?"":(string)HttpContext.Current.Session["outer_lineid"]; } set { HttpContext.Current.Session["outer_lineid"] = value; } }
        public static UserInfo User { get { return  (UserInfo)HttpContext.Current.Session["line_current_user"]; } set { HttpContext.Current.Session["line_current_user"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
           
            SetQueryString();
            CloseAlert();
            if (!IsPostBack) {
                if (!string.IsNullOrEmpty(Request.QueryString["id"])) {//ถ้ามี line มากับ query string ให้ทำการ log
                    Login(Request.QueryString["id"]);
                } else {
                    if (LogInFromOuterService.LogOuterInfo == null) {//ถ้าไม่มีข้อมูลการ login ให้ขึ้น error page
                        //go to error page
                        string url = $"../Misc/ErrorPage";
                        Response.Redirect(url);
                    }
                } 
                LoadDropDownList();           
                LoadData();
            }
        }

        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
           
        }
        
        private void BindData() {
            //lblTopic.Text = "สวัสดีจ้าาา";
            lblUser.Text="คุณ" + User.FullName; 
            lblLineSay.Text = "เลือกบริการได้เลยค่ะ";
        
        }

        private void LoadDropDownList() {

        }
 private void Login (string outUserId){
        var r=    LogInFromOuterService.Login(outUserId,"USER", "LINE");
            LoginService.Login(r.InnerUserID, "silent");
            if (r.Output.Result=="ok") {
                Response.Redirect("LineMenuAgent");
            } else {
                LineLogInStep1.LineID = outUserId;
                string url = $"LineLogInStep1";
                Response.Redirect(url);
            }
        }

        private void LoadData() {
            User = UserService.GetUserInfo(LogInFromOuterService.LogOuterInfo.InnerUserID);
                BindData(); 
        } 
 
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        } 

        protected void btnReport_Click(object sender, EventArgs e) {
            LineMenuReport.User = User.Username;
            string url = $"../Report/LineMenuReport";
            Response.Redirect(url);
        }

        //protected void btnDownLoadApp_Click(object sender, EventArgs e) {
        //    string url = @"https://dl.gooddev.net/ky/pos.apk?openExternalBrowser=1";
        //    Response.Redirect(url);
        //}

        //protected void btnOpenWeb_Click(object sender, EventArgs e) {

        //}
    }
}