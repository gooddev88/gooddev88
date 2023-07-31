using Robot.Data.ServiceHelper;
using Robot.Helper;
using Robot.POSC.DA;
using Robot.POSC.POSPrint;
using System;
using System.IO;
using System.Web.UI;

namespace Robot.PrintServer {
    public partial class PrintBillV1 : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                Export();
            }
        }


        private void Export() {
            var billId = Request.QueryString["billid"];
            var rcom = Request.QueryString["rcom"];
            var h = POSSaleService.GetBill(billId, rcom);
  
            var stream = new MemoryStream();

            #region save bill local path before print
            if (h.IsVatRegister == true) {
                var report = new  Rpt.R402();
                report.initData(h.BillID,h.RComID);
                report.ExportToPdf(stream);
            } else {
                var report = new Rpt.R401();
                report.initData(h.BillID, h.RComID);
                report.ExportToPdf(stream);
            }

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();
            #endregion


            if (MobileHelper.isMobileBrowser()) {
                string baseurl = URLHelper.GetBaseUrl();
                string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
                yurl = yurl.Replace("http://", "https://");


                string func = "sendUrlToPrint('" + yurl + "');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
            } else {
                string xurl = String.Format("~/TempFile/Print/{0}", myfilename);

                //popPrint.ContentUrl = xurl;
                //popPrint.ShowOnPageLoad = true;
                //Response.RedirectPermanent(xurl);
            }

        
    }
    }
}