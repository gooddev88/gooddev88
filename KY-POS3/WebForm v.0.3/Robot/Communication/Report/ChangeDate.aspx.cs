using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Communication.Report
{
    public partial class ChangeDate : Page {
        public static string PreviousPage { get { return (string)HttpContext.Current.Session["changedate_previouspage"]; } set { HttpContext.Current.Session["changedate_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            if(!IsPostBack) {
            
                LoadData();
            }
        }

        private void SetQueryString() {

        }

        private void BindData() {

        }
 

        private void LoadData() {
           BindData();
        }

        private void ShowAlert(string msg, string type)
        {
            lblMsgBody.Text = msg;
            string javaBody = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + javaBody + "','" + type + "');", true);
        }

        private void CloseAlert()
        {
            lblMsgBody.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (dtChangeDate.Value == null)
            {
                ShowAlert("ระบุ วันที่", "Warning");
                return;
            }

            ReportSalesSummary.ShowDate = dtChangeDate.SelectedDate.Date;
            string url = $"ReportSalesSummary";
            Response.Redirect(url);

        }

    }
}