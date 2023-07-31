using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Account.MyLogin {
    public partial class MyChangePassword : System.Web.UI.Page, IPostBackEventHandler {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                lnkLoginInfo.Text = "";
                txtusername.Value = Request.QueryString["username"];
            } 
        }
        public void RaisePostBackEvent(string Arg) {
            if (Arg.Contains("btnlogin")) {
                ChangePasswordPress();
            }          
        }

        private void ChangePasswordPress() {
            ChangePassword();
        }

        protected void ChangePassword() {
            if (txtpassword2.Value!=txtpassword3.Value) {
                lnkLoginInfo.Text = "Mismatch password";
                return;
            }
            string userName = txtusername.Value ;
            string password    = EncryptService.hashPassword("MD5", txtpassword1.Value);
            string newPassword = EncryptService.hashPassword("MD5", txtpassword2.Value);
            try {
                using (GAEntities entities = new GAEntities()) {
                    var u = entities.UserInfo.Where(o => o.Username == userName).FirstOrDefault();
                    if (u==null) {
                        lnkLoginInfo.Text = "";
                        return;
                    }
                    if (u.Password!=password) {
                        lnkLoginInfo.Text = "The username or password is incorrect";
                        return;
                    }
                    u.Password = newPassword;
                    u.IsNewUser = false;
                    entities.SaveChanges();           
                    //Response.RedirectPermanent("~/Account/MyLogin/Mylogin.aspx?menu=1");
                    Response.Redirect("~/Account/MyLogin/Mylogin.aspx?menu=1", false);
                }
            } catch (Exception ex) {
                lnkLoginInfo.Text = "The username or password is incorrect";
                lnkLoginInfo.ForeColor = System.Drawing.Color.Red;
            }
        }
        

        protected void btnBackLoginPage_Click(object sender, EventArgs e) {
            Response.RedirectPermanent("~/Account/MyLogin/Mylogin.aspx");
        }
    }
}