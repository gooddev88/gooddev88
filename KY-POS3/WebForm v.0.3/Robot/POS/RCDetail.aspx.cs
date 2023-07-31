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
using Robot.OMASTER;
using Robot.POS.DA;
using Robot.POS.Print;
using static Robot.Data.BL.I_Result;

namespace Robot.POS {
    public partial class RCDetail : MyBasePage {
        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["rcdetail_previouspage"]; } set { HttpContext.Current.Session["rcdetail_previouspage"] = value; } }


        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
            popFile1.ShowOnPageLoad = false;
            if (ORCService.IsNewDoc) {
                ShowAlert("บันทึกสำเร็จ", "Success");
            }
            LoadDevDropDownList();
            SetPrimaryGLData();
            if (!Page.IsPostBack) {
                CloseAlert();
                LoadDropDownList();
                LoadData();
            }

        }

        private void CheckPermission() {

            if (!PermissionService.CanCreate(hddmenu.Value)) {

            }


        }

        private void SetActiveControl() {
            var h = ORCService.DocSet.Head;
            if (h.RCID == "") {
                btnDeleteRV.Visible = false;
                btnPrintORC1.Visible = false;
                btnPrintORC2.Visible = false;
            } else {
                btnPrintORC1.Visible = true;
                btnPrintORC2.Visible = true;
            } 
            var cust = CustomerInfoService.GetDataByID(h.CustomerID);
            string infostr = "";
            infostr = infostr + cust.FullNameTh + " " + cust.CustomerID + " <br>";
            infostr = infostr + h.RCID;
            lblInfo.Text = infostr;
            if (h.PayINVTotalAmt != h.PayTotalAmt) {
                divDiffPay.Visible = true;
            } else {
                divDiffPay.Visible = false;
            }

        }
        private void SetQueryString() {

            hddmenu.Value = Request.QueryString["menu"];

        }


        private void LoadData() {
            var h = ORCService.DocSet.Head;
            BindData();
            BindInvoice();

            BindGrdPayment();
            BindGrdAdjustLine();
            BindGrdTransactionLogBind();
            SetActiveControl();
        }
        private void BindData() {
            var h = ORCService.DocSet.Head;
            cboCurrency.Value = h.Currency;
            txtRateExchange.Text = Convert.ToDecimal(h.RateExchange).ToString("n12");
            dtPayDate.Value = h.PayDate;
            lblShowDiffAmt.Text = "ยอดต่างอินวอยซ์ " + h.PayTotalDiffINVAmt.ToString("n2") + " บาท  ";
            lblSumInvPayAmt.Text = " ยอดตัดอินวอยซ์ " + h.PayINVTotalAmt.ToString("N2") + " บาท  ";

            SetupCurrency();
        }



        private void BindGrdTransactionLogBind() {
            grd_transaction_log.DataSource = ORCService.DocSet.Log;
            grd_transaction_log.DataBind();
        }

        private void BindInvoice() {
            var line = ORCService.DocSet.Line;
            grdInvline.DataSource = line;
            grdInvline.DataBind();
        }

        protected void btnAddFile_Click(object sender, EventArgs e) {
            var h = ORCService.DocSet.Head;
            btnSave_Click(null, null);
            Session["UploadFile_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"../Feature/UploadFile?id={h.RCID}&comid={h.CompanyID}&table=ARReceipt&menu={hddmenu.Value}";
            Response.Redirect(url);

        }






        private void LoadDropDownList() {

        }

        private void LoadDevDropDownList() {
            cboGLCode.DataSource = ORCService.ListGLAccountLineByID("");
            cboGLCode.DataBind();
            cboCurrency.DataSource = CurrencyInfoService.MiniSelectList(false);
            cboCurrency.DataBind();
        }

        private bool ValidData() {
            bool result = true;


            var head = ORCService.DocSet.Head;
            var line = ORCService.DocSet.Line;
            var payline = ORCService.DocSet.PaymentLine;
            //if (head == null || line == null) {
            //    ShowAlert("ไม่พบรายการอินวอยซ์ที่ชำระ", "Error");
            //    return false;
            //}
            if (line.Count() == 0) {
                ShowAlert("ไม่พบรายการอินวอยซ์ที่ชำระ", "Error");
                return false;
            }
          

            if (cboCurrency.Value == null) {
                ShowAlert("ระบุสกุลเงิน", "Error");
                return false; 
            }
            decimal exchange_rate = 0;
            decimal.TryParse(txtRateExchange.Text, out exchange_rate);
            if (exchange_rate == 0) {
                ShowAlert("ระบุอัตราแลกเปลี่ยน", "Error");
            }


            //if (head.PayINVTotalAmt!=head.PayTotalAmt) {
            //    ShowAlert("ยอดชำระไม่เท่ายอดรวมอินวอยซ์", "Error");
            //    return false;
            //}
            //if (payline.Count == 0) {
            //    ShowAlert("ไม่พบวิธีการชำระเงิน", "Error");
            //    return false; 
            //}
            //if (ORCService.DocSet.Head.RCStatus == "") {
            //    ShowAlert("ระบุ Receive status", "Error");
            //    return false;
            //}


            return result;
        }


        private void ShowAlert(string msg, string type) {
            ORCService.IsNewDoc = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);

        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private bool PrepairDataSave() {
            var h = ORCService.DocSet.Head;
            bool isNew = h.RCID == "" ? true : false;
            if (isNew) { // new  
                h.RCID = IDRuunerService.GetNewID("ORC", h.CompanyID, false, "th", h.RCDate)[1];
                if (h.RCID == "") {
                    ShowAlert("Error Generate document", "Error");
                    return true;
                }
            }
            SetPrimaryHeadData();
            return isNew;
        }

        private void SetPrimaryHeadData() {
            var h = ORCService.DocSet.Head;
            if (cboCurrency.Value != null) {
                h.Currency = cboCurrency.Value.ToString();
            }
            if (dtPayDate.Value != null) {
                h.PayDate = dtPayDate.Date;
            }
            h.RCDate = h.PayDate;
            decimal rate = 1;
            decimal.TryParse(txtRateExchange.Text, out rate);
            h.RateExchange = rate;
            SetPrimaryInvoiceData();
            SetPrimaryGLData();

        }

        private void SetPrimaryInvoiceData() {


            foreach (ListViewDataItem item in grdInvline.Items) {

                Label lblLinenum = item.FindControl("lblLineNum") as Label;
                TextBox txtPayNo = item.FindControl("txtPayNo") as TextBox;
                TextBox txtAmount = item.FindControl("txtAmount") as TextBox;
                TextBox txtRemark1 = item.FindControl("txtRemark1") as TextBox;

                int payno = 1;
                int.TryParse(txtPayNo.Text, out payno);
                decimal this_pay_amt = 0;
                decimal.TryParse(txtAmount.Text, out this_pay_amt);

                int linenum = 0;
                int.TryParse(lblLinenum.Text, out linenum);
                var this_line = ORCService.DocSet.Line.Where(o => o.LineNum == linenum).FirstOrDefault();
                this_line.PayTotalAmt = this_pay_amt;
                this_line.PayNo = payno;
                this_line.Remark1 = txtRemark1.Text;
                this_line.Remark3 = "";

            }

        }

        protected void btnSave_Click(object sender, EventArgs e) {

            try {
                if (!ValidData()) {
                    return;
                }
                bool isNew = PrepairDataSave();
                I_BasicResult rr = new I_BasicResult();
                if (isNew) {
                    rr= ORCService.Save("insert");
                } else {
                    rr = ORCService.Save("update");
                }
                if (rr.Result == "fail") {
                    ORCService.DocSet.Head.RCID = "";
                    ShowAlert(rr.Message1, "Error");
                } else {
                    if (isNew) {
                        ORCService.IsNewDoc = true;
                        string url = "../POS/RCDetail";
                        Response.Redirect(url);
                    } else {
                        ShowAlert("Save successfull", "Success");
                        LoadData();
                    }
                }
            } catch {

            }
        }




        protected void btnDeleteRV_Click(object sender, EventArgs e) {
            var h = ORCService.DocSet.Head;
            var r = ORCService.Delete(h.RCID);
            if (r.Result == "fail") {
                ShowAlert(r.Message1, "Error");
            } else {
                Response.RedirectPermanent(PreviousPageX);
            }
        }

        protected void btnaddpayment_Click(object sender, EventArgs e) {
            SetPrimaryHeadData();
            ORCService.AddPaymentLine();
            var h = ORCService.DocSet.Head;
            var payline_act = ORCService.DocSet.PaymentLineActive;
            var payline = ORCService.DocSet.PaymentLine;
            var totalpayAmt = payline.Where(o => o.LineNum != payline_act.LineNum).ToList().Sum(o => o.PayAmt);

            payline_act.PayAmt = h.PayINVTotalAmt - totalpayAmt;
            RCPayment.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"../POS/RCPayment?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnselectinv_Click(object sender, EventArgs e) {
            SetPrimaryHeadData();
            RCSelectInv.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"../POS/RCSelectInv?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }


        protected void btnEditPayment_Click(object sender, EventArgs e) {
            SetPrimaryHeadData();
            RCPayment.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"../POS/RCPayment?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

        protected void grdPaymentMethod_ItemCommand(object sender, ListViewItemEventArgs e) {

        }

        private void BindGrdPayment() {


            grdPaymentMethod.DataSource = ORCService.DocSet.PaymentLine.OrderBy(o => o.LineNum).ToList();
            grdPaymentMethod.DataBind();
        }
        public bool ShowHideGridButton() {
            bool result = true;
            return result;
        }

        protected void grdInvline_ItemCommand(object sender, ListViewCommandEventArgs e) {
            if (e.CommandName == "Del") {// check command is cmd_delete
                var linenum = Convert.ToInt32(e.CommandArgument); // value from CommandArgument 
                ORCService.DocSet.Line.RemoveAll(o => o.LineNum == linenum);
                BindInvoice();
                SetPrimaryHeadData();
            }

        }

        protected void btnAddGLAdjust_Click(object sender, EventArgs e) {
            string glcodeid = cboGLCode.Value != null ? cboGLCode.Value.ToString() : "";
            if (glcodeid == "") {
                return;
            }
            ORCService.NewAdustByGlID(glcodeid,"");
            cboGLCode.Value = "";

        }

        protected void grdAdjust_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "del") {
                int id = Convert.ToInt32(e.KeyValue.ToString());
                ORCService.DeleteAdjust(id);
                grdAdjust.DataBind();
            }

        }
        protected void grdPaymentMethod_ItemCommand(object sender, ListViewCommandEventArgs e) {
            if (e.CommandName == "editrow") {
                SetPrimaryHeadData();
                int linenum = Convert.ToInt32(e.CommandArgument);
                ORCService.DocSet.PaymentLineActive = ORCService.DocSet.PaymentLine.Where(o => o.LineNum == linenum).FirstOrDefault();
                RCPayment.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"../POS/RCPayment?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
            if (e.CommandName == "Del") {
                int linenum = Convert.ToInt32(e.CommandArgument);
                ORCService.DeletePayment(linenum);
                BindGrdPayment();
            }
        }


        private void SetPrimaryGLData() {

            for (int i = 0; i < grdAdjust.VisibleRowCount; i++) {
                ASPxTextBox txtCrAmt = ((ASPxTextBox)grdAdjust.FindRowCellTemplateControl(i, grdAdjust.Columns["CrAmt"] as GridViewDataColumn, "txtCrAmt"));
                ASPxTextBox txtDrAmt = ((ASPxTextBox)grdAdjust.FindRowCellTemplateControl(i, grdAdjust.Columns["DrAmt"] as GridViewDataColumn, "txtDrAmt"));
                if (txtCrAmt == null) {
                    continue;
                }
                int lblID = Convert.ToInt32(grdAdjust.GetRowValues(i, grdAdjust.KeyFieldName));
                var get_line = ORCService.DocSet.AdjustLine.Where(o => o.LineNum == lblID).FirstOrDefault();
                if (get_line == null) {
                    continue;
                }

                decimal qtyCrAmt = get_line.CrAmt;
                decimal.TryParse(txtCrAmt == null ? qtyCrAmt.ToString() : txtCrAmt.Text, out qtyCrAmt);

                decimal qtyDrAmt = get_line.DrAmt;
                decimal.TryParse(txtDrAmt == null ? qtyDrAmt.ToString() : txtDrAmt.Text, out qtyDrAmt);
                get_line.CrAmt = qtyCrAmt;
                get_line.DrAmt = qtyDrAmt;
            }
        }


        private void BindGrdAdjustLine() {
            grdAdjust.DataSource = ORCService.DocSet.AdjustLine;
            grdAdjust.DataBind();
            //grdAdjust.DataSource = ORCService.DocSet.PaymentLine.Where(o => o.PayByType == "ADJUST").ToList();
            //grdAdjust.DataBind();
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPageX);
        }

        private void SetupCurrency() {
            GetCurrency();
            GetCurrencyRate();
        }
        private void GetCurrency() {
            var h = ORCService.DocSet.Head;
            string result = "THB";
            if (cboCurrency.Value != null) {
                result = cboCurrency.Value.ToString();
            }
            h.Currency = result;
            h.RateBy = "SB";
        }

        private void GetCurrencyRate() {
            GetCurrency();
            var h = ORCService.DocSet.Head;
            DateTime rateDate = DateTime.Now.Date;
            try {
                if (h.PayBy == "CHEQUE") {
                    rateDate = dtPayDate.Date;
                } else {
                    rateDate = dtPayDate.Date;
                }
            } catch { }
            decimal rate = CurrencyInfoService.GetExchangeRate(h.CompanyID, h.Currency, h.RateBy, rateDate);
            h.RateDate = rateDate;
            h.RateExchange = rate;
            txtRateExchange.Text = Convert.ToDecimal(h.RateExchange).ToString("n6");
        }


        protected void cboCurrency_SelectedIndexChanged(object sender, EventArgs e) {
            SetupCurrency();
        }

        protected void grdPaymentMethod_ItemDataBound(object sender, ListViewItemEventArgs e) {
            var dataItem = e.Item.DataItem;
            Literal lblRCStatus = (e.Item.FindControl("lblRCStatus") as Literal);

            string html_rcstatus = "";


            if (((ORCPayLine)dataItem).RCStatus == "CONFIRM") {
                //html_rcstatus = html_rcstatus + "<span style=\"color: green\"><i class=\"fas fa-sun fa-2x\"></i> </span>" + HtmlSpace(2);\
                html_rcstatus = html_rcstatus + "<span style=\"color: green;font-size:smaller\">CONFIRM </span>" + HtmlSpace(2);
            } else if (((ORCPayLine)dataItem).RCStatus == "REJECT") {

            } else {
                html_rcstatus = html_rcstatus + "<span style=\"color: red;font-size:smaller\">" + ((ORCPayLine)dataItem).RCStatus + " </span>" + HtmlSpace(2);
            }
            if (((ORCPayLine)dataItem).CompletedDate != null) {
                html_rcstatus = html_rcstatus + "<span style=\"color: gray;font-size:smaller\">" + " วันที่ " + Convert.ToDateTime(((ORCPayLine)dataItem).CompletedDate).ToString("dd/MM/yyyy") + " </span>" + HtmlSpace(2);
            }
            lblRCStatus.Text = html_rcstatus;

        }

        private string HtmlNewLine(int round) {

            string result = "";
            for (int i = 0; i < round; i++) {
                result = result + "<br>";
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

        protected void btnPrintORC1_Click(object sender, EventArgs e) {
            var h = ORCService.DocSet.Head;
            MyPrint.NewReportFItler();
            var f = MyPrint.ReportFilterX;

            f.DocID = h.RCID;
            f.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            f.CompanyID = h.CompanyID;
            f.ReportID = "RCGA01";
            MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;

            string myurl = "../POS/Print/MyPrint?report=RCGA01";

            Response.RedirectPermanent(myurl);
        }

        protected void btnPrintORC2_Click(object sender, EventArgs e) {
            var h = ORCService.DocSet.Head;
            MyPrint.NewReportFItler();
            var f = MyPrint.ReportFilterX;

            f.DocID = h.RCID;
            f.RCompanyID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            f.CompanyID = h.CompanyID;
            f.ReportID = "RCGA02";
            MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;

            string myurl = "../POS/Print/MyPrint?report=RCGA02";
            Response.RedirectPermanent(myurl);
        }

    }
}

