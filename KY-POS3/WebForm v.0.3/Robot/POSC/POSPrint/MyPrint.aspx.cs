
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using Robot.Data;
 
using Robot.POSC.DA;
using Robot.POSC.Print;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.POSC.DA.SaleReportService;

namespace Robot.POSC.POSPrint
{
    public partial class MyPrint : System.Web.UI.Page
    {
       
        public static string PreviousPage { get { return HttpContext.Current.Session["Print_previouspage"] == null ? "" : (string)HttpContext.Current.Session["Print_previouspage"]; } set { HttpContext.Current.Session["Print_previouspage"] = value; } }
        public static I_Filter PrintFilter { get { return  (I_Filter)HttpContext.Current.Session["printfilter"]; } set { HttpContext.Current.Session["printfilter"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            if (!IsPostBack) {
                SetButtonReport();
                LoadData();

            }
        }
        private void SetQueryString() {

            hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
            hddReportName.Value = Request.QueryString["report"];
            hddlinenum.Value = Request.QueryString["linenum"];

        }
      

        private void SetButtonReport() {
            //check show hide button


            //divbtnPrintBillposafull.Visible = false;
            //divbtnPrintBillposa.Visible = false;
            //divTotalBillInCom.Visible = false;
            //divSumTotalBill.Visible = false;
            //divCancelBill.Visible = false;
            //divbtnTotalqtyItem.Visible = false;

            //switch (hddmenu.Value) {
            //    case "BillPOSAFull"://Print from 
            //        divbtnPrintBillposafull.Visible = true;
            //        divbtnPrintBillposa.Visible = false;
            //        divTotalBillInCom.Visible = false;
            //        divSumTotalBill.Visible = false;
            //        divCancelBill.Visible = false;
            //        break;
            //    case "PRINT_BILL_VAT"://Print from 
            //        divbtnPrintBillposa.Visible = true;
            //        divbtnPrintBillposafull.Visible = false;
            //        divTotalBillInCom.Visible = false;
            //        divSumTotalBill.Visible = false;
            //        divCancelBill.Visible = false;
            //        break;
            //    case "SumTotalInCom"://Print from 
            //        divbtnPrintBillposa.Visible = false;
            //        divbtnPrintBillposafull.Visible = false;
            //        divSumTotalBill.Visible = false;
            //        divTotalBillInCom.Visible = true;
            //        divCancelBill.Visible = false;
            //        break;
            //    case "SumTotal"://Print from 
            //        divbtnPrintBillposa.Visible = false;
            //        divbtnPrintBillposafull.Visible = false;
            //        divTotalBillInCom.Visible = true;
            //        divSumTotalBill.Visible = true;
            //        divCancelBill.Visible = false;
            //        break;
            //    case "CancelBill"://Print from 
            //        divbtnPrintBillposa.Visible = false;
            //        divbtnPrintBillposafull.Visible = false;
            //        divTotalBillInCom.Visible = false;
            //        divSumTotalBill.Visible = false;
            //        divCancelBill.Visible = true;
            //        break;
            //    case "TotalqtyItem"://Print from 
            //        divbtnPrintBillposa.Visible = false;
            //        divbtnPrintBillposafull.Visible = false;
            //        divTotalBillInCom.Visible = false;
            //        divSumTotalBill.Visible = false;
            //        divCancelBill.Visible = false;
            //        divbtnTotalqtyItem.Visible = true;
            //        break;

            //}
        }

        private void LoadData() {
            var f = SaleReportService.PrintFilter;
            switch (f.reportname) {
                case "RptPOS134":
                    RptPOS134 pos134 = new RptPOS134();
                    pos134.initData(f.company, f.date_begin, f.date_end);
                    docviewer.OpenReport(pos134);
                    break;
                case "RptPOS133":

                    RptPOS133 pos133 = new RptPOS133();
                    pos133.initData(f.company, f.date_begin, f.date_end);
                    docviewer.OpenReport(pos133);
                    break;
                case "RptPOS138":
                    RptPOS138 pos138 = new RptPOS138();
                    pos138.initData(f.company, f.date_begin, f.date_end);
                    docviewer.OpenReport(pos138);
                    break;

                case "RptPOS140": 
                    RptPOS140 pos140 = new RptPOS140();
                    pos140.initData(f.company, f.date_begin, f.date_end);
                    docviewer.OpenReport(pos140);
                    break;
              
        
                case "BillPOSAFull":
                    R421 r_BILLPOSFull = new R421();
                    r_BILLPOSFull.initData(f.docid);
                    docviewer.OpenReport(r_BILLPOSFull);
                    break;
                case "PRINT_BILL_VAT":
                    R401 r401 = new R401();
                    r401.initData(f.docid);
                    docviewer.OpenReport(r401);
                    break;
                default:
                    break;
            }
        }

        protected void btnPrintBillposaFull_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "MyPrint.aspx?id=" + hddid.Value + "&menu=" + hddmenu.Value + "&report=R_BILLPOSFull";
            Response.RedirectPermanent(myurl);
        }

        protected void btnPrintBillposa_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "MyPrint.aspx?id=" + hddid.Value + "&menu=" + hddmenu.Value + "&report=R_BILLPOS";
            Response.RedirectPermanent(myurl);
        }

        protected void btnPrintTotalBillInCom_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "MyPrint.aspx?id=" + hddid.Value + "&menu=" + hddmenu.Value + "&report=R_TotalBillInCom";
            Response.RedirectPermanent(myurl);
        }

        protected void btnPrintSumTotalBill_Click(object sender, EventArgs e) {

            string myurl = $"MyPrint?id={hddid.Value}&menu={hddmenu.Value}&report=pos133";
            Response.RedirectPermanent(myurl);
        }

        protected void btnPrintbtnCancelBill_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "MyPrint.aspx?id=" + hddid.Value + "&menu=" + hddmenu.Value + "&report=R_CancelBill";
            Response.RedirectPermanent(myurl);
        }

        protected void btnTotalqtyItem_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "MyPrint.aspx?id=" + hddid.Value + "&menu=" + hddmenu.Value + "&report=R_TotalqtyItem";
            Response.RedirectPermanent(myurl);
        }

        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }
        protected void btnShow_Click(object sender, EventArgs e) {
            var xtraReport1 = new R401();
            //Performing pre-rendering operations: binding, passing parameters and etc.  
            xtraReport1.CreateDocument();

            var xtraReport2 = new R401();
            //Performing pre-rendering operations: binding, passing parameters and etc.  
            xtraReport2.CreateDocument();

            xtraReport1.Pages.AddRange(xtraReport2.Pages);
            xtraReport1.PrintingSystem.ContinuousPageNumbering = true;

            using (MemoryStream ms = new MemoryStream()) {
                PdfExportOptions opts = new PdfExportOptions();
                opts.ShowPrintDialogOnOpen = true;
                xtraReport1.ExportToPdf(ms, opts);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] report = ms.ToArray();
                Page.Response.ContentType = "application/pdf";
                Page.Response.Clear();
                Page.Response.OutputStream.Write(report, 0, report.Length);
                Page.Response.End();
            }
        }
    }

   
}