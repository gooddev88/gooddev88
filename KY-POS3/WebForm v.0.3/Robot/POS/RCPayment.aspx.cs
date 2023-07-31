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
using Robot.POS.DA;
using static Robot.POS.DA.ORCService;

namespace Robot.POS {
    public partial class RCPayment : MyBasePage {
        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["rcselectpay_previouspage"]; } set { HttpContext.Current.Session["rcselectpay_previouspage"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            LoadDevDropDownList();
            if (!Page.IsPostBack) {
                CloseAlert();
                LoadDropDownList();
                LoadData();
            }
        }

        private void CheckPermission() {

        }

        private void LoadData() {
            BindData();
            SetActiveControl();
        }



        private void SetPrimaryPaymentData() {    
            var payactive = ORCService.DocSet.PaymentLineActive;
            payactive.PayBy = cboPayBy.SelectedValue;
            var opay = OPaymentService.GetPayType(payactive.PayBy, payactive.CompanyID);
            payactive.PayByType = opay.Type;
            payactive.PayByDesc = opay.Name;
            payactive.RCStatus = cboStatementStatus.SelectedValue;
            payactive.ChqReturnReason = cboChqReturnReson.SelectedValue;
            payactive.PayMemo = txtMemo.Text;

            payactive.CompletedDate = null;
            if (dtCompletedDate.Value != null) {
                payactive.CompletedDate = dtCompletedDate.Date;
            }

            decimal payAmt = 0;
            if (payactive.PayBy == "CHEQUE") {
                decimal.TryParse(txtPayAmt_CHQ.Text, out payAmt);
                payactive.PayToBookID = cboPayToBook_CHQ.Value == null ? "" : cboPayToBook_CHQ.Value.ToString();
                payactive.PayAmt = payAmt;
                payactive.PayDate = DateTime.Now.Date;
                if (dtPayDate_CHQ.Value != null) {
                    payactive.PayDate = DateTime.Parse(dtPayDate_CHQ.Value.ToString());
                }

                payactive.ChqDate = null;
                if (dtChqDate_CHQ.Value != null) {
                    payactive.ChqDate = DateTime.Parse(dtChqDate_CHQ.Value.ToString());
                }
                payactive.StatementDate = null;
                if (dtStatementDate_CHQ.Value != null) {
                    payactive.StatementDate = DateTime.Parse(dtStatementDate_CHQ.Value.ToString());
                }
                payactive.ClearingDate = null;
                if (dtClearingDate_CHQ.Value != null) {
                    payactive.ClearingDate = DateTime.Parse(dtClearingDate_CHQ.Value.ToString());
                }
                payactive.ChqDepositDate = null;
                if (dtChqDepositDate_CHQ.Value != null) {
                    payactive.ChqDepositDate = DateTime.Parse(dtChqDepositDate_CHQ.Value.ToString());
                }
                payactive.ChqExpired = null;
                if (dtChqExpired_CHQ.Value != null) {
                    payactive.ChqExpired = DateTime.Parse(dtChqExpired_CHQ.Value.ToString());
                }

                payactive.ChqReturnDate = null;
                if (dtChqReturnDate.Value != null) {
                    payactive.ChqReturnDate = DateTime.Parse(dtChqReturnDate.Value.ToString());
                }

                payactive.CustBankCode = cboCustomerBank_CHQ.Value == null ? "" : cboCustomerBank_CHQ.Value.ToString();
                var data_Bank = BookBankService.GetBankInfo(payactive.CustBankCode);
                payactive.CustBankName = data_Bank == null ? "" : data_Bank.Name_TH;

                payactive.CustBankBranch = txtCustomerBankBranch_CHQ.Text;

                payactive.PaymentRefNo = txtRefNo_CHQ.Text;
            } else {
                decimal.TryParse(txtPayAmt_CASH.Text, out payAmt);
                payactive.PayToBookID = cboPayToBook_TR.Value == null ? "" : cboPayToBook_TR.Value.ToString();
                var bookinfom = BookBankService.GetBookBankInfo(payactive.PayToBookID);
                payactive.PayToBookName = bookinfom == null ? "" : bookinfom.BookDesc;
                payactive.PayToBookID = bookinfom == null ? "" : bookinfom.BookNo;
                payactive.PayAmt = payAmt;
                payactive.PayDate = DateTime.Now.Date;

                payactive.ChqDate = null;
                payactive.StatementDate = null;
                payactive.ClearingDate = null;
                if (dtStatementDate_CASH.Value != null) {
                    payactive.StatementDate = DateTime.Parse(dtStatementDate_CASH.Value.ToString());
                    payactive.ClearingDate = DateTime.Parse(dtStatementDate_CASH.Value.ToString());
                } else {
                    if (payactive.PayBy == "CASH" || payactive.PayBy == "TRANSFER") {
                        payactive.StatementDate = payactive.PayDate;
                    }
                }
                payactive.ChqDepositDate = null;
                payactive.CustBankCode = "";
                if (cboCustomerBank_TR.Value != null) {
                    payactive.CustBankCode = cboCustomerBank_TR.Value.ToString();
                    var data = BookBankService.GetBankInfo(payactive.CustBankCode);
                    payactive.CustBankName = data == null ? "" : data.Name_TH;
                }
                payactive.CustBankBranch = txtCustomerBankBranch_TR.Text;
                payactive.PaymentRefNo = txtRefNo_TR.Text;
            }
            ORCService.DocSet = CalDocSet(ORCService.DocSet);
        }
        private bool ValidData() {
            var payactive = ORCService.DocSet.PaymentLineActive;

            if (payactive.RCStatus == "CONFIRM") {
                if (dtCompletedDate.Value == null) {
                    ShowAlert("Input Receive completed date", "Error");
                    return false;
                }
            }
            if (payactive.RCStatus == "") {
                ShowAlert("Input Receive status", "Error");
                return false;
            }

            return true;
        }

        private void BindData() {
            var h = ORCService.DocSet.Head;
            var line = ORCService.DocSet.Line;
            var payactive = ORCService.DocSet.PaymentLineActive;

            txtRCID.Text = payactive.RCID;
            txtCompanyID.Text = payactive.CompanyID;
            txtMemo.Text = payactive.PayMemo;
            cboStatementStatus.SelectedValue = payactive.RCStatus;
            dtCompletedDate.Value = payactive.CompletedDate;
            dtChqReturnDate.Value = payactive.ChqReturnDate;

            cboPayBy.SelectedValue = payactive.PayBy;
            lblShowTotalinvoice_m.Text = "รวมตัดยอดอินวอยซ์ " + Convert.ToDecimal(h.PayINVTotalAmt).ToString("N2");
            lblShowTotalinvoice_chq.Text = "รวมตัดยอดอินวอยซ์ " + Convert.ToDecimal(h.PayINVTotalAmt).ToString("N2");
            txtPayAmt_CASH.Text = payactive.PayAmt.ToString("N2");
            txtPayAmt_CHQ.Text = payactive.PayAmt.ToString("N2");
            cboChqReturnReson.SelectedValue = payactive.ChqReturnReason;
            switch (cboPayBy.SelectedValue) {
                case "CHEQUE": //เช็ค 

                    dtPayDate_CHQ.Value = payactive.PayDate;
                    dtStatementDate_CHQ.Value = payactive.StatementDate;
                    cboPayToBook_CHQ.Value = payactive.PayToBookID;
                    txtRefNo_CHQ.Text = payactive.PaymentRefNo;
                    cboCustomerBank_CHQ.Value = payactive.CustBankCode;
                    txtCustomerBankBranch_CHQ.Text = payactive.CustBankBranch;
                    dtChqDate_CHQ.Value = payactive.ChqDate;
                    dtChqDepositDate_CHQ.Value = payactive.ChqDepositDate;
                    dtClearingDate_CHQ.Value = payactive.ClearingDate;
                    dtChqExpired_CHQ.Value = payactive.ChqExpired;
                    break;
                default:// ไม่ไช่เช็ค 

                    dtStatementDate_CASH.Value = payactive.StatementDate;
                    cboPayToBook_TR.Value = payactive.PayToBookID;
                    txtRefNo_TR.Text = payactive.PaymentRefNo;
                    cboCustomerBank_TR.Value = payactive.CustBankCode;
                    txtCustomerBankBranch_TR.Text = payactive.CustBankBranch;
                    break;
            }
        }


        protected void btnBack_Click(object sender, EventArgs e) {
            ORCService.ClearPaymentLinePending();
            Response.Redirect(PreviousPageX);
        }

        private void LoadDropDownList() {
            cboChqReturnReson.DataSource = MasterTypeService.ListType("CHQ_REJECT_REASON", false);
            cboChqReturnReson.DataBind();
            cboPayBy.DataSource = OPaymentService.ListPayType("", ORCService.DocSet.Head.CompanyID);
            cboPayBy.DataBind();
        }

        private void LoadDevDropDownList() {
            string comid = DocSet.Head.CompanyID;
            cboPayToBook_CHQ.DataSource = BookBankService.ListBook(comid).ToList();
            cboPayToBook_CHQ.DataBind();

            cboPayToBook_TR.DataSource = BookBankService.ListBook(comid).ToList();
            cboPayToBook_TR.DataBind();

            cboCustomerBank_CHQ.DataSource = BookBankService.ListBank(false).OrderBy(o => o.Sort).ToList();
            cboCustomerBank_CHQ.DataBind();
            cboCustomerBank_TR.DataSource = BookBankService.ListBank(false).OrderBy(o => o.Sort).ToList();
            cboCustomerBank_TR.DataBind();
        }


        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            SaveActive();
        }

        private void SaveActive() {
            SetPrimaryPaymentData();
            if (!ValidData()) {
                return;
            }

            var payactive = ORCService.DocSet.PaymentLineActive;
            payactive.Status = "OPEN";
            Response.Redirect(PreviousPageX);
        }

        private void SetActiveControl() {

            var h = ORCService.DocSet.Head;
            var a = ORCService.DocSet.PaymentLineActive;
            ListStatementStatus();

            switch (a.PayBy) {
                case "CHEQUE":
                    divpaychq.Visible = true;
                    divpaycash.Visible = false;
                    divTrasnfer.Visible = false;
                    divchq_expire.Visible = true;
                    divChqReturnDate.Visible = false;
                    break;
                case "TRANSFER":
                    divpaychq.Visible = false;
                    divpaycash.Visible = true;
                    divTrasnfer.Visible = true;
                    divRefNo_m.Visible = false;
                    divchq_expire.Visible = false;
                    divChqReturnDate.Visible = false;
                    if (dtCompletedDate.Value == null) {
                        dtCompletedDate.Value = h.PayDate;
                        cboStatementStatus.SelectedValue = "CONFIRM";

                    }
                    if (dtStatementDate_CASH.Value == null) {
                        dtStatementDate_CASH.Value = h.PayDate;
                    }
                    break;
                default:
                    divpaychq.Visible = false;
                    divpaycash.Visible = true;
                    divTrasnfer.Visible = false;
                    divRefNo_m.Visible = false;
                    divchq_expire.Visible = false;
                    divChqReturnDate.Visible = false;
                    if (dtCompletedDate.Value == null) {
                        dtCompletedDate.Value = h.PayDate;
                        cboStatementStatus.SelectedValue = "CONFIRM";
                    }
                    if (dtStatementDate_CASH.Value == null) {
                        dtStatementDate_CASH.Value = h.PayDate;
                    }
                    if (a.PayByType== "ADJUST") {
                        cboStatementStatus.Enabled = false;
                    }
                    break;
            }
            if (a.RCStatus == "REJECT") {
                Returndate_th.Visible = true;
                Returndate_en.Visible = false;

                Statusdate_en.Visible = false;
                divchqreturn.Visible = true;
                divChqReturnDate.Visible = true;
            } else {
                Returndate_th.Visible = false;
                Returndate_en.Visible = true;

                Statusdate_en.Visible = true;
                divchqreturn.Visible = false;
                divChqReturnDate.Visible = false;
            }
            //cboStatementStatus.Enabled = false;
            //dtCompletedDate.Enabled = false;


        }


        protected void cboStatementStatus_SelectedIndexChanged(object sender, EventArgs e) {
            SetActiveControl();
        }
        public string SetCaption(string captionID) {
            string result = "";
            if (captionID == "PAYMENT") {
                switch (cboPayBy.SelectedValue) {
                    case "CHEQUE":
                        result = "จำนวนเงินหน้าเช็ค";
                        break;
                    case "TRANSFER":
                        result = "ยอดเงินโอน";
                        break;
                    default:
                        result = "ยอดชำระ";
                        break;
                }
            }
            return result;
        }
        private List<I_StatementStatus> ListStatementStatus() {
            List<I_StatementStatus> result = new List<I_StatementStatus>();
            if (cboPayBy.SelectedValue == "CHEQUE") {
                result.Add(new I_StatementStatus { Code = "OPEN", Desc = "OPEN" });
                result.Add(new I_StatementStatus { Code = "ON-HAND", Desc = "ON-HAND" });
                result.Add(new I_StatementStatus { Code = "DEPOSIT", Desc = "DEPOSIT" });
                result.Add(new I_StatementStatus { Code = "CONFIRM", Desc = "CONFIRM" });
                result.Add(new I_StatementStatus { Code = "REJECT", Desc = "REJECT" });

            } else {
                result.Add(new I_StatementStatus { Code = "OPEN", Desc = "OPEN" });
                result.Add(new I_StatementStatus { Code = "CONFIRM", Desc = "CONFIRM" });
                result.Add(new I_StatementStatus { Code = "REJECT", Desc = "REJECT" });
            }
            cboStatementStatus.DataSource = result;
            cboStatementStatus.DataBind();

            return result;
        }

        protected void cboPayBy_SelectedIndexChanged(object sender, EventArgs e) {
            SetDefaultStatus();
            SetPrimaryPaymentData();
            SetActiveControl();
        }

        private void SetDefaultStatus() {
            switch (cboPayBy.SelectedValue) {
                case "CHEQUE":
                    cboStatementStatus.SelectedValue = "OPEN";
                    break;
                case "TRANSFER":
                    cboStatementStatus.SelectedValue = "CONFIRM";
                    break;
                default:
                    cboStatementStatus.SelectedValue = "CONFIRM";
                    break;
            }

        }
    }
}