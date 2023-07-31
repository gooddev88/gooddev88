using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot {
    public partial class Contact : Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (HttpContext.Current.Session["ga_error"]!=null) {
                lblGaError.Text = HttpContext.Current.Session["ga_error"].ToString();
            } else {
                if (HttpContext.Current.Session["userlogin"]==null) {
                    lblGaError.Text = "Loss user Login";
                } else {
                    lblGaError.Text = LoginService.LoginInfo.CurrentUser;
                }
               
            }
           
        }
    }
}