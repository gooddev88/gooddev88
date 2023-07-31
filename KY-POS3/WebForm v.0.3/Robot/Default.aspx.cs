using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot {
    public partial class _Default : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {

                var b = LoginService.LoginInfo.CurrentBoard;
                if (b != null)
                {
                    string url = $"~/DashBoard/{b.BoardPage}";
                    Response.RedirectPermanent(url);
                }

            }
        }
    

 
    }
}