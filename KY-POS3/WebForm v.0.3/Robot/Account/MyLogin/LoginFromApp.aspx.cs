using Robot.Account.Authen.DA;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.Data.BL.I_Result;
using static Robot.Data.DataAccess.LogInCrossAppService;

namespace Robot.Account.MyLogin {
    public partial class LoginFromApp : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                OnLogin();
            }
        
          
        }



        void   OnLogin() {
            string reqid = Request.QueryString["reqid"];
            if (!string.IsNullOrEmpty(reqid)) {
                LogInCrossAppReqX reqInfo = LogInCrossAppService.GetReqInfo(reqid);
                var result_req = IsValidReq(reqInfo);
                if (result_req.Result == "fail") {
                    Response.Redirect("MyLogIn");
             
                } else {
                    LoginService.LoginInfo= LogInCenterService.Login(reqInfo.Username, "silent",reqInfo.RComID);
                   // LoginService.Login(reqInfo.Username, "silent");
                    if (LoginService.LoginInfo.LoginResultInfo == "fail") {
                        Response.Redirect("MyLogIn");
                    } else {
                        if (string.IsNullOrEmpty(reqInfo.ForwardToUrl)) {
                            Response.Redirect("~/Default.aspx");
                        } else {
                            Response.Redirect(reqInfo.ForwardToUrl);
                        } 
                    } 
               
                }
            } else {
                Response.Redirect("MyLogIn");
            }

        }

        I_BasicResult IsValidReq(LogInCrossAppReqX reqInfo) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            if (reqInfo == null) {
                r.Result = "fail";
                r.Message1 = "No request record found.";
                return r;
            }
            if (reqInfo.ExpiryTime <= DateTime.Now) {
                r.Result = "fail";
                r.Message1 = "request was expired.";
                return r;
            }
            if (reqInfo.ExpiryTime <= DateTime.Now) {
                r.Result = "fail";
                r.Message1 = "request was expired.";
                return r;
            }
            if (reqInfo.UseTime != null) {
                r.Result = "fail";
                r.Message1 = "request was used.";
                return r;
            }
            return r;
        }
    }
}