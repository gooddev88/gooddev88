
using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.Data.BL.I_Result;

namespace Robot.OMASTER {
    public partial class BookBankDetail : MyBasePage {
        public static string PreviousePage { get { return (string)HttpContext.Current.Session["bookbankdetail_previouspage"]; } set { HttpContext.Current.Session["bookbankdetail_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();

            if (BookBankService.IsNewDoc) {
                ShowAlert("บันทึกสำเร็จ", "Success");
            }

            if (!IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter();        
                LoadData();
                ListGrdSel();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];

        }


        private void ShowAlert(string msg, string type) {

            BookBankService.IsNewDoc = false;
            lblMsg.Text = msg;
            msg = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            BookBankService.IsNewDoc = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private void LoadDefaultFilter() {
            if (BookBankService.FilterSet == null) {
                BookBankService.NewFilterSet();
            }

            txtSearch.Text = BookBankService.FilterSet.SearchText;
            chkShowClose.Checked = BookBankService.FilterSet.ShowInActive;
        }
        private void SetDefaultFilter() {
            BookBankService.NewFilterSet();
            BookBankService.FilterSet.SearchText = txtSearch.Text.Trim();
            BookBankService.FilterSet.ShowInActive = chkShowClose.Checked;
        }

        private void LoadData() {
            BindData(); 
            SetActiveControl(); 
        }
        private void BindGrid() {
            grd.DataSource = BookBankService.BooKList. OrderBy(o => o.Sort).ToList();
            grd.DataBind();
        }
        private void BindData() {
            lblinfohead.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            var a = BookBankService.DocSet.Book;
            txtID.Text = a.BookID;
            txtBookDesc.Text = a.BookDesc;
            try { cboBankCode.SelectedValue = a.BankCode; } catch  {  }     
            txtBranchName.Text = a.BranchName;
            txtSort.Text = Convert.ToString(a.Sort);
            txtRemark1.Text = a.Remark1;
            chkIsActive.Checked = a.IsActive;
        }
        private void SetPermission() {

        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousePage);
        }
        private void SetPrimaryData() {
       
            var a = BookBankService.DocSet.Book;

            a.BookID = txtID.Text.Trim().ToUpper();
            a.BookNo = txtID.Text;
            a.BookDesc = txtBookDesc.Text;
            a.BankCode = cboBankCode.SelectedValue;
            a.BankName = cboBankCode.SelectedItem.Text;
            a.BranchName = txtBranchName.Text;
            int sort = 0;
            int.TryParse(txtSort.Text, out sort);
            a.Sort = sort;
            a.Remark1 = txtRemark1.Text;
            a.IsActive = chkIsActive.Checked;
        
        }
        protected void btnOK_Click(object sender, EventArgs e) {
            if (!ValidData()) {
                return;
            }
            SetPrimaryData();
            var h = BookBankService.DocSet.Book;
        
         var r=   BookBankService.Save();
           
            if (r.Result == "ok") {//save successufull
                BookBankService.NewTransaction();
                LoadData();
                ListGrdSel();
            
                ShowAlert("บันทึกสำเร็จ", "Success");

            } else {
                ShowAlert(r.Message1, "Error");
            }
        }

        private bool ValidData() {
            if (txtID.Text.Trim() == "") {
                ShowAlert("ระบุ เลขบัญชี", "Error");
                return false;
            }

            if (txtBookDesc.Text == "") {
                ShowAlert("ระบุ ชื่อบัญชี", "Error");
                return false;
            }

            return true;
        }

      
        private void LoadDropDownDevList() {

        }

        private void LoadDropDownList() {
            cboBankCode.DataSource = BookBankService.ListBank(false);
            cboBankCode.DataBind();
        }

        private void SetActiveControl() {
            var h = BookBankService.DocSet.Book;
            txtID.Enabled = h.BookID == "" ? true : false;         
            SetPermission();
        }

        private string HtmlNewLine(int round) {
            string result = "";
            for (int i = 0; i < round; i++) {
                result = result + "<br />";
            }
            return result;
        }
        private string HtmlSpace(int round) {

            string result = "";
            for (int i = 0; i < round; i++) {
                result = result + "&nbsp";
            }
            return result;
        }
        protected void grd_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {

            if (e.CommandArgs.CommandName == "editrow") {
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString();
                BookBankService.GetDocSetByID(id);
                LoadData();
            }
        }

        protected void grd_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) {

        }
        private void ListGrdSel() {
            SetDefaultFilter();
            BookBankService.ListDoc(false);
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            ListGrdSel();
        }

        protected void grd_DataBinding(object sender, EventArgs e) {
            grd.DataSource = BookBankService.BooKList.OrderBy(o => o.Sort).ToList();
        }


        protected void btnNew_Click(object sender, EventArgs e) {
            BookBankService.NewTransaction();
            Response.Redirect(Request.Url.AbsoluteUri);
        }
        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            ListGrdSel();
        }

    }

}