using Robot.Account.Authen.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Helper;
using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Account.MyLogin {
    public partial class MyLogIn : System.Web.UI.Page, IPostBackEventHandler {
        protected void Page_Load(object sender, EventArgs e) {

 
            if (!IsPostBack) {
                //  Session["userlogin"] = null;
                Session.Clear();
                lnkLoginInfo.Text = "";
               
                hddmode.Value = Request.QueryString["mode"];
                hdduser.Value = Request.QueryString["id"];
                LoadData();
                if (hddmode.Value == "hhhhhHhhhh") {
                    LogIn(hdduser.Value, hddmode.Value);
                }
                if (hddmode.Value == "change_password") {
                    lnkLoginInfo.Text = "Change password successfull please login again";
                    lnkLoginInfo.ForeColor = System.Drawing.Color.Blue;
                }

            }
        }
        private void LoadData() {
            //LoginService.ComRoot = CompanyService.GetRootCom();
            BindData();
        }
        private void BindData() {
            //if (LoginService.LoginInfo == null)
            //{              
            //     imgLogo.ImageUrl = "../../Image/Logo/kylogo.png";
            //}
            //else
            //{
            //    imgLogo.ImageUrl = LoginService.LoginInfo.AppLogoImage;
            //}
            var logo=  CompanyService.GetRComLogoThumb("APPLOGO");
            imgLogo.ImageUrl = logo;
            //lblRCom.Text = "GooDapp";
            //lblDesc.Text = "Accy";
        }
        public void RaisePostBackEvent(string Arg) {
            if (Arg.Contains("btnlogin")) {
                LogIn(txtusername.Value.Trim(), txtpassword.Value.Trim());
            }
        }
        private void GotoPage() { 
            if (LoginService.LoginInfo.UserInRCompany.Count()>1) {
                Response.Redirect("MyLogInRCompany.aspx");
            } else {
                Response.RedirectPermanent("~/Default.aspx");
            }

        }

        protected void LogIn(string username, string password) {


       
            LoginService.LoginInfo = LogInCenterService.Login(username, password);

            if (LoginService.LoginInfo.LoginResult == "ok") {
                //**  suppose you  are someone
                if (!string.IsNullOrEmpty(hdduser.Value)) {
                    LoginService.LoginInfo = LoginService.LoginInfo = LogInCenterService.Login(hdduser.Value, "silent");
                    GotoPage();
                    return;
                }
            }  
                if (LoginService.LoginInfo.LoginResult == "new") {
                    Response.RedirectPermanent("~/Account/MyLogin/MyChangePassword?username=" + username);
                } else {
                    GotoPage();
                }
                if (LoginService.LoginInfo.LoginResult == "fail") {
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = txtusername.Value, TableID = "USERINFO", ActionType = "LOGIN", ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", Action = "LOGIN FAIL" });
                    lnkLoginInfo.Text = LoginService.LoginInfo.LoginResultInfo;
                    lnkLoginInfo.ForeColor = System.Drawing.Color.Red;
                }
            
         

        }
        protected void btnChangePassword_Click(object sender, EventArgs e) {
            string url = "~/Account/MyLogin/MyChangePassword.aspx?username=" + txtusername.Value;
            Response.RedirectPermanent(url);
        }


        protected void btnDownLoad_Click(object sender, EventArgs e) {
     var url  =     LinkService.GetLinkByLinkName("download_app");
      //   string url = @"http://dl.gooddev.net/ky/pos.apk";
       
            Response.Redirect(url);
        }
    }
}