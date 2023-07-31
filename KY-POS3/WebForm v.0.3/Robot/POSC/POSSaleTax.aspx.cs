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
using Robot.Helper;
using Robot.POSC.DA;
using Robot.POSC.POSPrint;

namespace Robot.POSC {
    public partial class POSSaleTax : MyBasePage {

        public static string PreviousPage { get { return HttpContext.Current.Session["posc_tax_previouspage"] == null ? "" : (string)HttpContext.Current.Session["posc_tax_previouspage"]; } set { HttpContext.Current.Session["posc_tax_previouspage"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {

            LoadDevDropDownList();
            if (!Page.IsPostBack) {
                CloseAlert();
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString() {

            hddmenu.Value = Request.QueryString["menu"];

        }

        private void CheckPermission() {


        }

        private void LoadData() {
            var h = POSSaleService.DocSet.Head;
            //var comInfo = CompanyInfoService.GetDataByComID(h.ComID);
            txtCustomerName.Text = h.CustomerName;
            //txtCustBranchName.Text = "สำนักงานใหญ่";
            txtCustBranchName.Text = h.CustBranchName;
            if (!string.IsNullOrEmpty(h.CustBranchName)) {
                txtCustBranchName.Text = h.CustBranchName;
            }
            txtBillAddr1.Text = h.CustAddr1;
            txtBillAddr2.Text = h.CustAddr2;
            txtCustTaxID.Text = h.CustTaxID;
        }
        private void LoadDropDownList() {
        }

        private void LoadDevDropDownList() {

        }
        private string ValidateControl() {

            if (txtCustomerName.Text == "") {
                return "กรอกชื่อลูกค้า..";
            }
            if (txtCustBranchName.Text == "") {
                return "กรอกสาขา..";
            }
            if (txtBillAddr1.Text == "") {
                return "กรอกที่อยู่..";
            }
            if (txtCustTaxID.Text == "") {
                return "กรอกเลขผู้เสียภาษี..";
            }
            return "";
        }


        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private void PrepairDataSave() {
            var h = POSSaleService.DocSet.Head;
            h.CustomerName = txtCustomerName.Text;
            h.CustBranchName = txtCustBranchName.Text;
            h.CustAddr1 = txtBillAddr1.Text;
            h.CustAddr2 = txtBillAddr2.Text;
            h.CustTaxID = txtCustTaxID.Text;

        }

        //private void ExportPrintTaxBill() {
        //    var h = POSSaleService.DocSet.Head;
        //    var report = new R421();
        //    var stream = new MemoryStream();
        //    report.initData(h.BillID);
        //    report.ExportToPdf(stream);

        //    string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
        //    string myfillfilename = @"/TempFile/Print/" + myfilename;
        //    string serverpath = Server.MapPath(myfillfilename);
        //    FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        //    stream.WriteTo(myfile);
        //    myfile.Close();
        //    stream.Close();

        //    ////string xurl = String.Format("printPDF('../TempFile/Print/{0}');", myfilename);
        //    //string xurl = String.Format("../TempFile/Print/{0}", myfilename);
        //    //LogJService.SaveLogJ("iscomputer:" + xurl);
        //    //Response.RedirectPermanent(xurl);




        //    //if (MobileHelper.isMobileBrowser()) {
        //    //    string baseurl = POSHelper.GetBaseUrl();
        //    //    string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
        //    //    yurl = yurl.Replace("http://", "https://");
        //    //    LogJService.SaveLogJ("ismobile:" + yurl);

        //    //    string func = "sendUrlToPrint('" + yurl + "');";
        //    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
        //    //} else {
        //    //    string xurl = String.Format("../TempFile/Print/{0}", myfilename);
        //    //    LogJService.SaveLogJ("iscomputer:" + xurl);
        //    //    popPrint.ContentUrl = xurl;
        //    //    popPrint.ShowOnPageLoad = true;
        //    //    //Response.RedirectPermanent(xurl);
        //    //}


        //    if (MobileHelper.isMobileBrowser()) {
        //        string baseurl = POSHelper.GetBaseUrl();
        //        string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
        //        yurl = yurl.Replace("http://", "https://");
        //        LogJService.SaveLogJ("ismobile:" + yurl);
        //        string func = "sendUrlToPrint('" + yurl + "');";
        //        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
        //    } else {
        //        string xurl = String.Format("../TempFile/Print/{0}", myfilename);
        //        LogJService.SaveLogJ("iscomputer:" + xurl);
        //        popPrint.ContentUrl = xurl;
        //        popPrint.ShowOnPageLoad = true;
        //        //Response.RedirectPermanent(xurl);
        //    }

        //}
        private void ExportPrintFullTax() {
            var h = POSSaleService.DocSet.Head;
            var report = new R421();
            var stream = new MemoryStream();
            report.initData(h.BillID);
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();
        

            if (MobileHelper.isMobileBrowser()) {
                string baseurl = URLHelper.GetBaseUrl();
                string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
                yurl = yurl.Replace("http://", "https://");
                string func = "sendUrlToPrint('" + yurl + "');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
                return;
            } else {
                string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
                popPrint.ContentUrl = xurl;
                popPrint.ShowOnPageLoad = true;
                //Response.RedirectPermanent(xurl);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e) {
            var h = POSSaleService.DocSet.Head;
            try {

                string resultValid = ValidateControl();
                if (resultValid != "") {
                    ShowAlert(resultValid, "Error");
                    return;
                }

                PrepairDataSave();

                var rr = POSSaleService.SaveTaxSlip(POSSaleService.DocSet);
                if (rr.Result == "fail") {
                    ShowAlert(rr.Message1, "Error");
                } else {
                    ExportPrintFullTax();
                }

            } catch (Exception ex) {
                string resultValid = "";
                if (ex.InnerException != null) {
                    resultValid = ex.InnerException.ToString();
                } else {
                    resultValid = ex.Message.ToString();
                }
                ShowAlert(resultValid, "Error");
            }
        }


        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }


    }
}