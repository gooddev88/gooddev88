using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Robot {
    public class Global : HttpApplication {
        protected void Application_Error(object sender, EventArgs e) {
            Exception exception = Server.GetLastError();
            if (exception != null) {
      //          HttpContext.Current.Session["ga_error"] = exception.Message;
              
             //   Response.Redirect("~/Default.aspx");

                //log the errorD:\SourceCode\BS\GAQuickWin\Robot\Contact.aspx
            }
        }

        void Application_Start(object sender, EventArgs e) {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
        }
    }
}