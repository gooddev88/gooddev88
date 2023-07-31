using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.POS
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
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void BindData() {

        }
 

        private void LoadData() {
           BindData();
        }

        private void ShowAlert(string msg, string type)
        {
            //CustomerInfoService.IsNewDoc = false;
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
            if (dtProductNeedDate.Value == null)
            {
                ShowAlert("ระบุ วันที่", "Warning");
                return;
            }

            POSORDERMobileList.ShowDate = dtProductNeedDate.SelectedDate.Date;
            string url = $"~/POS/POSORDERMobileList?menu={hddmenu.Value}";
            Response.Redirect(url);

        }

    }
}