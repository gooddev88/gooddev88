
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Feature {
    public partial class ShowMessage : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {
            LoadData();

        }

        private void LoadData() {
            if (Session["show_msg"] == null) {
                lblMessage.Text = "";
            }
            lblMessage.Text = Session["show_msg"].ToString();
            string msg_logo = "ERROR";
            if (Session["show_msg_logo"]!=null) {
                msg_logo = Session["show_msg_logo"].ToString();
            }
            switch (msg_logo) {
                case "ERROR":
                    imgErrorLogo.Visible = true;
                    imgSuccesLogo.Visible = false;
                    break;
                case "SUCCESS":
                    imgErrorLogo.Visible = false;
                    imgSuccesLogo.Visible = true;
                    break;
                default:
                    imgErrorLogo.Visible = true;
                    imgSuccesLogo.Visible = false;
                    break;
            }
         
        }
        protected void btnCancel_Click(object sender, EventArgs e) {
            ClosePopup("Cancel");
        }
        protected void btnOk_Click(object sender, EventArgs e) {
            ClosePopup("OK");
        }
        protected void btnClose_Click(object sender, EventArgs e) {
            ClosePopup("Cancel");
        }
        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), command, string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }
    }
}