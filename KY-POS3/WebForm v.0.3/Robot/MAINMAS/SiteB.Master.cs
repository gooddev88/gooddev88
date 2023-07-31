using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.MAINMAS
{
    public partial class SiteB : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        public string SetCaption(object menu_id)
        {


            string result = "";
            string menuid = menu_id.ToString();
            switch (menuid)
            {
                case "5000":
                    result = "POS";
                    break;
                case "3000":
                    result = "Finance";
                    break;
                default:
                    result = "xxxx";
                    break;
            }
            return result;

        }
        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            if (LoginService.LoginInfo != null)
            {
                Session.Abandon();
            }
            string url = LinkService.GetLinkByLinkName("loginpage");
            Response.Redirect(url);
        }
    }
}