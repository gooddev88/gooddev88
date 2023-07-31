
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Communication.DA;
using Robot.Communication.Report;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Report
{
    public partial class LineMenuReport : System.Web.UI.Page {             
        public static string User { get { return HttpContext.Current.Session["menureport_user"] == null ? "" : (string)HttpContext.Current.Session["menureport_user"]; } set { HttpContext.Current.Session["menureport_user"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            CloseAlert();
            if (!IsPostBack) { 
                LoadDropDownList();           
                LoadData();
            }
        }

        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
        }
        
        private void BindData() {
            lblMenuPayment.Text = "เลือกรายงานที่ต้องการ";     
        }

        private void LoadDropDownList() {

        }

        private void LoadData() {
           // User = UserInfoService.GetUserInfo(LogInFromOuterService.LogOuterInfo.InnerUserID);
                BindData(); 
        } 
 
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }


        protected void btnReportSummary_Click(object sender, EventArgs e)
        {
            ReportSalesSummary.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"ReportSalesSummary";
            Response.Redirect(url);
        }

        protected void btnReportSumPo_Click(object sender, EventArgs e)
        {
            var link = LinkService.GetLinkByLinkInfo("apibase_url");
            string url = $"{link.AppLink}/POSV3/ReportSumPoByStore?openExternalBrowser=1";
            Response.Redirect(url);
        }

        

    }
}