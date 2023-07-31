
using DevExpress.Web;
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
    public partial class StepPaymentList : System.Web.UI.Page
    {

        public static string PreviousPage { get { return HttpContext.Current.Session["StepPayment_previouspage"] == null ? "" : (string)HttpContext.Current.Session["StepPayment_previouspage"]; } set { HttpContext.Current.Session["StepPayment_previouspage"] = value; } }
        public static string Username { get { return HttpContext.Current.Session["StepPayment_user"] == null ? "" : (string)HttpContext.Current.Session["StepPayment_user"]; } set { HttpContext.Current.Session["StepPayment_user"] = value; } }
        public static List<vw_CompanyInfo> ListDocCompany { get { return (List<vw_CompanyInfo>)HttpContext.Current.Session["StepPaymentcompany_list"]; } set { HttpContext.Current.Session["StepPaymentcompany_list"] = value; } }
        public static List<BookBankInfo> ListDocBookBank { get { return (List<BookBankInfo>)HttpContext.Current.Session["StepPaymentbookbank_list"]; } set { HttpContext.Current.Session["StepPaymentbookbank_list"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            LoadDropDownDevList();
            CloseAlert();
            SetQueryString();

            if (PayReconcileService.DocSet == null)
            {
                PayReconcileService.DocSet = PayReconcileService.NewTransaction();
            }

            if (!IsPostBack) {
                txtSearch.Text = "";
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }
        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void LoadDefaultFilter()
        {

        }
        private void SetDefaultFilter()
        {

        }

        private void LoadDropDownList() {
            cboBankCode.DataSource = BankInfoService.MiniSelectList(false);
            cboBankCode.DataBind();
        }

        private void LoadDropDownDevList() {

        }

        private void LoadData()
        {
            //SetDefaultFilter();
            BindData();
            
            GridCompanyBinding();
            GridBookBankBinding();
            SetActiveControl();
        }

        private void BindData() {
            //lblheadinfo.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;

            var h = PayReconcileService.DocSet.Head;
            dtReconcileDate.Value = h.ReconcileDate;
        }

        private void SetActiveControl()
        {
            divSelectCompany.Visible = true;
            divSrlectBookBank.Visible = false;
            divtransfer.Visible = false;
            if (ListDocCompany.Count == 1)
            {
                divSelectCompany.Visible = false;
                divSrlectBookBank.Visible = true;
                PayReconcileService.DocSet.Head.CompanyID = ListDocCompany.Select(o => o.CompanyID).FirstOrDefault();
            }
        }


        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousPage);
        }

        private void ShowAlert(string msg, string type)
        {

            lblAlertmsg.Text = msg;
            if (type == "Success")
            {
                lblAlertmsg.ForeColor = Color.Green;
            }
            if (type == "Error")
            {
                lblAlertmsg.ForeColor = Color.Red;
            }
            if (type == "Warning")
            {
                lblAlertmsg.ForeColor = Color.YellowGreen;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('','" + type + "');", true);
        }
        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private void GridCompanyBinding() {
            ListDocCompany = CompanyInfoV2Service.ListViewCompanyBySarch(txtSearch.Text);
            grdCompanyID.DataSource = ListDocCompany;
            grdCompanyID.DataBind();
        }

        private void GridBookBankBinding()
        {
            ListDocBookBank = BookBankInfoV2Service.ListBookBankInfo();
            grdBookBank.DataSource = ListDocBookBank;
            grdBookBank.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void grdCompanyID_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (grdCompanyID.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridCompanyBinding();
        }

        protected void grdCompanyID_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "sel")
            {
                string id = e.CommandArgument.ToString();
                PayReconcileService.DocSet.Head.CompanyID = id;
                divSelectCompany.Visible = false;
                divSrlectBookBank.Visible = true;
            }
        }

        protected void grdCompanyID_DataBinding(object sender, EventArgs e)
        {

        }

        protected void grdBookBank_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (grdBookBank.FindControl("grdlistPager2") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBookBankBinding();
        }

        protected void grdBookBank_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "sel")
            {
                string id = e.CommandArgument.ToString();
                var h = BookBankInfoV2Service.GetBookBankInfo(id);
                PayReconcileService.DocSet.Head.PayToBookBackCode = h.BankCode;
                PayReconcileService.DocSet.Head.PayToBookID = h.BookNo;
                divSrlectBookBank.Visible = false;
                divtransfer.Visible = true;
            }
        }

        protected void grdBookBank_DataBinding(object sender, EventArgs e)
        {

        }

        private bool ValidData()
        {
            if (cboBankCode.SelectedValue == "")
            {
                ShowAlert("ระบุ ธนาคาร", "Warning");
                return false;
            }
            if (dtReconcileDate.Value == null)
            {
                ShowAlert("ระบุ วันที่โอนเงิน", "Warning");
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidData())
            {
                return;
            }
            var h = PayReconcileService.DocSet.Head;
            h.PayID = IDGeneratorServiceV2.GetNewID("PAY", h.CompanyID, false, "th")[1];
            h.PayByBankCode = cboBankCode.SelectedValue;
            h.PayMethod = "TRANSFER";

            decimal payamt = 0;
            decimal.TryParse(txtPayAmt.Text.Trim(), out payamt);
            h.PayAmt = payamt;

            h.ReconcileDate = dtReconcileDate.Date;
            h.Memo = txtMemo.Text;

            var rs = PayReconcileService.Save();
            if (rs.Result == "ok")
            {
                string url = $"../PayAgent/LineMenuPayment";
                Response.Redirect(url);
            }
            else
            {
                ShowAlert("Error " + rs.Message1, "Error");
            }

        }

        protected void btnBackToCompany_Click(object sender, EventArgs e)
        {
            divSrlectBookBank.Visible = false;
            divSelectCompany.Visible = true;
        }

        protected void btnBackToTransfer_Click(object sender, EventArgs e)
        {
            divSrlectBookBank.Visible = true;
            divtransfer.Visible = false;
        }
    }
}