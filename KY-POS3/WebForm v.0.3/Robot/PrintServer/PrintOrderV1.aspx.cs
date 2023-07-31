using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.ServiceHelper;
using Robot.Helper;
using Robot.POSC.DA;
using Robot.POSC.POSPrint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.PrintServer {
    public partial class PrintOrderV1 : System.Web.UI.Page {
        public static POS_SaleHead Doc { get { return (POS_SaleHead)HttpContext.Current.Session["print_doc"]; } set { HttpContext.Current.Session["print_doc"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                var refid = Request.QueryString["refid"];
                var billId = Request.QueryString["billid"];
                var rcom = Request.QueryString["rcom"];
                hdddevice.Value = Request.QueryString["device"];
                hdddevice.Value = hdddevice.Value.ToLower();
              Doc = POSSaleService.GetBill(billId, rcom);
                if (Doc.INVID=="") {
                    btnPrintBill.Visible = false;   
                }
                docviewer.Visible = true;
                lblUrl.Text = HttpContext.Current.Request.Url.AbsoluteUri;
                if (refid == "tax")
                {
                    if (hdddevice.Value == "d")
                    {
                        ExportPrintFullTax(hdddevice.Value);
                    }
                    else
                    {
                        docviewer.Visible = false;
                        btnPrintInv.Visible = true;           
                    }    
                }

                if (refid == "order")
                {
                    if (hdddevice.Value == "d")
                    {
                        ExportOrder(hdddevice.Value);
                    }
                    else
                    {
                        docviewer.Visible = false;
                        btnPrintBill.Visible = false;
                        btnPrintOrder.Visible = true;                     
                    }
                }

                if (refid == "bill")
                {
                    if (hdddevice.Value == "d")
                    {
                        ExportBill(hdddevice.Value);
                    }
                    else
                    {
                        docviewer.Visible = false;
                        btnPrintBill.Visible = true;
                        btnPrintOrder.Visible = false;
                    }
                }

            }
           
        }

        private void ExportOrder(string device) {

            var h = Doc;

            //RCGA01 firstrc2_report = new RCGA01();

            //docviewer.initData(ff.DocID, ff.CompanyID, ff.RCompanyID, "ลูกค้า", "original", "RCGA02", ff.RefID);
            //docviewer.DisplayName = ff.DocID;
            //docviewer.CreateDocument();
            //docviewer.OpenReport(printshiporder);



            if (device == "m")
            {
                //var img_format = new ImageExportOptions() {
                //    ExportMode = ImageExportMode.DifferentFiles,
                //    Format = System.Drawing.Imaging.ImageFormat.Jpeg,
                //    PageBorderWidth = 0,
                //    Resolution = 192
                //};
                #region save bill local path before print
                var stream = new MemoryStream();
              
                if (h.IsVatRegister == true) {
                    var report = new Rpt.R412();
                    report.initData(h.BillID, h.RComID);
                    report.ExportToPdf(stream);
                    //report.ExportToImage(stream, img_format);
                } else {
                    var report = new Rpt.R411();
                    report.initData(h.BillID, h.RComID);
                    report.ExportToPdf(stream);
                    //report.ExportToImage(stream, img_format);
                }

                string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
                string myfillfilename = @"~/TempFile/Print/" + myfilename;
                string serverpath = Server.MapPath(myfillfilename);
                FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                stream.WriteTo(myfile);
                myfile.Close();
                stream.Close();
                #endregion

               // Thread.Sleep(5000);
                #region print to rawbt
                string baseurl = URLHelper.GetBaseUrl();
                string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
                yurl = yurl.Replace("http://", "https://");
                string func = "sendUrlToPrint('" + yurl + "');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
                #endregion

                return;
            } else {
                if (h.IsVatRegister == true) {
                    var report = new Rpt.R412();
                    report.initData(h.BillID, h.RComID); 
                    docviewer.OpenReport(report);

                } else {
                    var report = new Rpt.R411();
                    report.initData(h.BillID, h.RComID);
                    docviewer.OpenReport(report);
                }

                //popPrint.ContentUrl = xurl;
                //popPrint.ShowOnPageLoad = true;

            }

        }
        private void ExportBill(string device) {
            var h = Doc;

            //var img_format = new ImageExportOptions() {
            //    ExportMode = ImageExportMode.DifferentFiles,
            //    Format = System.Drawing.Imaging.ImageFormat.Jpeg,
            //    PageBorderWidth = 0,
            //    Resolution = 192
            //};
            if (device == "m")
            {
                #region save bill local path before print
                var stream = new MemoryStream();

                if (h.IsVatRegister == true) {
                    var report = new Rpt.R402();
                    report.initData(h.BillID, h.RComID);
                    report.ExportToPdf(stream);
                   // report.ExportToImage(stream, img_format);
                } else {
                    var report = new Rpt.R401();
                    report.initData(h.BillID, h.RComID);
                    report.ExportToPdf(stream);
                  //  report.ExportToImage(stream, img_format);
                }

                string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
                string myfillfilename = @"~/TempFile/Print/" + myfilename;
                string serverpath = Server.MapPath(myfillfilename);
                FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                stream.WriteTo(myfile);
                myfile.Close();
                stream.Close();
                #endregion

                // Thread.Sleep(5000);
                #region print to rawbt
                string baseurl = URLHelper.GetBaseUrl();
                string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
                yurl = yurl.Replace("http://", "https://");
                string func = "sendUrlToPrint('" + yurl + "');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
                #endregion

                return;
            } else {
                if (h.IsVatRegister == true) {
                    var report = new Rpt.R412();
                    report.initData(h.BillID, h.RComID);
                    docviewer.OpenReport(report);
                } else {
                    var report = new Rpt.R411();
                    report.initData(h.BillID, h.RComID);
                    docviewer.OpenReport(report);
                }

                //popPrint.ContentUrl = xurl;
                //popPrint.ShowOnPageLoad = true;

            }

        }

        private void ExportPrintFullTax(string device)
        {
            var h = Doc;

            if (device=="m")
            {

                //var img_format = new ImageExportOptions() {
                //    ExportMode = ImageExportMode.DifferentFiles,
                //    Format = System.Drawing.Imaging.ImageFormat.Jpeg,
                //    PageBorderWidth = 0,
                //    Resolution = 192
                //};
                #region save bill local path before print
                var report = new Rpt.R421();
                var stream = new MemoryStream();
                report.initData(h.BillID, h.RComID);
                report.ExportToPdf(stream);
               // report.ExportToImage(stream, img_format);

                string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
                string myfillfilename = @"~/TempFile/Print/" + myfilename;
                string serverpath = Server.MapPath(myfillfilename);
                FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                stream.WriteTo(myfile);
                myfile.Close();
                stream.Close();
            #endregion

                // Thread.Sleep(5000);
                #region print to rawbt
                string baseurl = URLHelper.GetBaseUrl();
                string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
                yurl = yurl.Replace("http://", "https://");
                string func = "sendUrlToPrint('" + yurl + "');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
                #endregion

                return;
            }
            else
            {
                var reportr421 = new Rpt.R421();
                reportr421.initData(h.BillID, h.RComID);
                docviewer.OpenReport(reportr421);
                //string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
                //popPrint.ContentUrl = xurl;
                //popPrint.ShowOnPageLoad = true;
                //Response.RedirectPermanent(xurl);
            }
        }

        protected void btnPrintOrder_Click(object sender, EventArgs e) {
            ExportOrder(hdddevice.Value);
        }

        protected void btnPrintBill_Click(object sender, EventArgs e) {
            ExportBill(hdddevice.Value);
        }

        protected void btnPrintInv_Click(object sender, EventArgs e) {
            ExportPrintFullTax(hdddevice.Value);
        }

         
    }
}