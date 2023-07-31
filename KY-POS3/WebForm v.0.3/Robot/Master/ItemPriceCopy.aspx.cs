using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Master {
    public partial class ItemPriceCopy : System.Web.UI.Page {
        public static string PreviousPage { get { return HttpContext.Current.Session["itempricecopy_previouspage"] == null ? "" : (string)HttpContext.Current.Session["itempricecopy_previouspage"]; } set { HttpContext.Current.Session["itempricecopy_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
       
            SetQueryString();
            if (!Page.IsPostBack) {
                CloseAlert();
                LoadDropDownList();
                //LoadData();
            }
        }
        private void SetQueryString() {
            //hddid.Value = Request.QueryString["id"];
           hddmenu.Value = Request.QueryString["menu"];
        }

        private void LoadDropDownList() {
            cboCompanyFr.DataSource = CompanyService.ListBranch(  false);
            cboCompanyFr.DataBind();
            cboCompanyTo.DataSource = CompanyService.ListBranch(  false);
            cboCompanyTo.DataBind();
        }




        private void ShowAlert(string msg, string type) {
            lblAlertBody.Text = msg;
            msg = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            lblAlertBody.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private bool ValidData() {
            bool isValid = true;
            if (string.IsNullOrEmpty(cboCompanyFr.SelectedValue)) {
                isValid = false;
                ShowAlert("ระบุ ก็อปปี้จากสาขา", "Error");
                return isValid;
            }
            if (string.IsNullOrEmpty(cboCompanyTo.SelectedValue)) {
                isValid = false;
                ShowAlert("ระบุ ก็อปปี้ไปที่สาขา", "Error");
                return isValid;
            }
            if (cboCompanyFr.SelectedValue== cboCompanyTo.SelectedValue)
            {
                isValid = false;
                ShowAlert("ระบุ ก็อปปี้ข้อมูลไปสาขาเดียวกันไม่ได้", "Error");
                return isValid;
            }
            return isValid;
        }

        protected void btnOK_Click(object sender, EventArgs e) {
            if (!ValidData()) {
                return;
            }
         var rr=   POSItemService.CopyPrice(cboCompanyFr.SelectedValue, cboCompanyTo.SelectedValue);
            if (rr.Result=="ok") {
                Response.Redirect(PreviousPage);
            } else {
                ShowAlert(rr.Message1, "Error");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }
    }
}