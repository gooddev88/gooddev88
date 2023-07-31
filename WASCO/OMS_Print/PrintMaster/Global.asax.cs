using DevExpress.XtraReports.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace PrintMaster
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
        }
    }
}
