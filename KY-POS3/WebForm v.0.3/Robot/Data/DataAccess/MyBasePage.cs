using System;
using System.Web.Security;

namespace Robot.Data.DataAccess {
    public class MyBasePage : System.Web.UI.Page {
        public MyBasePage() {
            //this.PreRender += new EventHandler(Page_Prerender);

        }

        protected override void OnLoad(EventArgs e) {
       

            if (LoginService.LoginInfo == null)
            {
                Response.RedirectPermanent("~/Account/MyLogin/MyLogIn.aspx");
            }
            if (LoginService.LoginInfo.CurrentUser == "")
            {
                Response.RedirectPermanent("~/Account/MyLogin/MyLogIn.aspx");
            }
            base.OnLoad(e);
        }

        //protected void Page_Error(object sender, EventArgs e) {
        //    Exception ex = Server.GetLastError();

        //    // Do something with the exception e.g. log it
        //    //...
        //    Session["pageerror_msg"] = ex.Message;
        //    Response.RedirectPermanent("~/MyServicePage/ErrorPage.aspx");

        //}

    }
}