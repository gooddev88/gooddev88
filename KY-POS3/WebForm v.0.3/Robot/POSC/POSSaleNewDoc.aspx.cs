using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.ServiceHelper;
using Robot.Master.DA;
using Robot.POSC.DA;

namespace Robot.POSC {
    public partial class POSSaleNewDoc : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["posc_new_previouspage"] == null ? "" : (string)HttpContext.Current.Session["posc_new_previouspage"]; } set { HttpContext.Current.Session["posc_new_previouspage"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            LoadDevDropDownList();
            SetQueryString();
            if (!Page.IsPostBack) {
                CloseAlert();
                LoadDropDownList();
                LoadData(); 
            }
        }
        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
        }
        private void SetActiveControl() {

        }
        private void CheckPermission() {

        }

        private void LoadData() {
            BindData();
            SetActiveControl(); 
        }
        private void BindData() {
            dtInvDate.Value = DateTime.Now;
        }

        private void LoadDropDownList() {
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH",false);
            cboCompany.DataBind(); 
        }

        private void LoadDevDropDownList() { 
        }

        private string ValidateControl() { 
            return "";
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


        protected void btnList_Click(object sender, EventArgs e) {
            string myurl = $"~/POSC/POSInvoiceList?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

        protected void btnsaveto_Click(object sender, EventArgs e) {
            if (dtInvDate.Value == null) {
                ShowAlert("กรุณาเลือกวันที่ขาย", "Error");
                return;
            }
            if (string.IsNullOrEmpty(cboCompany.SelectedValue)) {
                ShowAlert("กรุณาเลือกสาขา", "Error");
                return;
            }
            //var comInfo = LoginService.LoginInfo.CurrentCompany;
            var comInfo = CompanyService.GetCompanyInfo(cboCompany.SelectedValue);
            LoginService.LoginInfo.CurrentCompany = comInfo;
            if (string.IsNullOrEmpty(comInfo.ShortCode))
            {
                ShowAlert("ตั้งชื่อย่อ สาขาก่อนเปิดบิลขาย", "Error");
                return;
            }
            LoginService.LoginInfo.CurrentTransactionDate = dtInvDate.Date;   
            POSSaleService.DocSet = POSSaleService.NewTransaction(); 
            POSSaleService.DocSet.Head.ComID = comInfo.CompanyID;
            POSSaleService.DocSet.Head.IsVatRegister = comInfo.IsVatRegister;
            //POSSaleService.DocSet.Head.IsVatInPrice = "";
            POSSaleService.DocSet.Head.ShipToLocID = "";
            POSSaleService.DocSet.Head.BillDate = Convert.ToDateTime( dtInvDate.Value).Date;
            POSSaleService.DocSet.Head.MacNo = LoginService.LoginInfo.CurrentMacNo;
            POSSaleService.Menu = POSItemService.ListMenuItem(POSSaleService.DocSet.Head.ComID, POSSaleService.DocSet.Head.ShipToUsePrice);
            if (comInfo.IsVatRegister==true) {
                POSSaleService.DocSet.Head.VatRate = LoginService.LoginInfo.CurrentVatRate;
            }
            POSSaleDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
      
         
            string myurl = $"~/POSC/POSSaleDetail?menu={hddmenu.Value }";
            Response.Redirect(myurl);
        }


    }
}