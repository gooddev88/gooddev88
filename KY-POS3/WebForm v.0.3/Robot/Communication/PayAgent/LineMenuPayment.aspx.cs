
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Communication.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.PayAgent
{
    public partial class LineMenuPayment : System.Web.UI.Page {             
        public static string User { get { return HttpContext.Current.Session["menupayment_user"] == null ? "" : (string)HttpContext.Current.Session["menupayment_user"]; } set { HttpContext.Current.Session["menupayment_user"] = value; } }

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
            lblMenuPayment.Text = "เลือกรายการชำระเงิน";     
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

        protected void btnStepPaymentList_Click(object sender, EventArgs e)
        {
            StepPaymentList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            StepPaymentList.Username = User;
            string url = $"../PayAgent/StepPaymentList";
            Response.Redirect(url);
        }

        protected void btnDriverSendMoney_Click(object sender, EventArgs e)
        {

        }
    }
}