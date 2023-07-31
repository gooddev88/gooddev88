using System;
using System.Linq;
using System.Web;
using System.Web.UI;

using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;


using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;
using Robot.POSC.POSPrint;
using static Robot.POSC.DA.SaleReportService;
using Robot.POSC.DA;
using Robot.Master.DA;
using Robot.POSC.Print;

namespace Robot.POSC {
    public partial class POSReports : MyBasePage {

        public static string PreviousPage { get { return HttpContext.Current.Session["Print_previouspage"] == null ? "" : (string)HttpContext.Current.Session["Print_previouspage"]; } set { HttpContext.Current.Session["Print_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            popAlert.ShowOnPageLoad = false;
            LoadDropDownDevList();
            if (!IsPostBack) {
                CheckPermission();
                grdDetail.ExpandAll();
                LoadDefaultFilter();
                LoadData();
            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
            hddParentPage.Value = Request.QueryString["p_page"];
        }
        private void LoadDefaultFilter() {
            if (SaleReportService.PrintFilter == null) {
                SaleReportService.NewFilter();
            }
            var f = SaleReportService.PrintFilter;
            txtSearch.Text = f.searchText;
            dtBegin.Value = f.date_begin;
            dtEnd.Value = f.date_end;
            cboCompany.Value = f.company;
        }
        private void SetDefaultFilter() {
            SaleReportService.NewFilter();
            var f = SaleReportService.PrintFilter;
            f.searchText = txtSearch.Text;
            if (dtBegin.Value != null) {
                f.date_begin = dtBegin.Date;
            }
            if (dtEnd.Value != null) {
                f.date_end = dtEnd.Date;
            }
            f.company = "";
            if (cboCompany.Value != null) {
                f.company = cboCompany.Value.ToString();
            }
        }

        private void CheckPermission() {
            var f = SaleReportService.PrintFilter;
            if (!PermissionService.CanCreate("5902")) { //5902 รายงานขาย
                                                        //if (!PermissionService.CanEdit("F5994"))
                                                        //can edit ถ้าไม่มีสิทธฺ์จะดูย้อนหลังได้แค่ 1 วัน
                DateTime minDate = DateTime.Now.Date.AddDays(-3);
                dtBegin.MinDate = minDate;
                //if (dtEnd.Date > DateTime.Now.Date.AddDays(+1)) {
                //    f.date_end = DateTime.Now.Date.AddDays(+1);
                //    dtEnd.Value = f.date_end;
                //}

                //if (dtBegin.Date < DateTime.Now.Date.AddDays(-1)) {
                //    f.date_begin = DateTime.Now.Date.AddDays(-1);
                //    dtBegin.Value = f.date_begin; 
                //}
            }
        }

        private void BindData() {
            lblTopic.Text = "รายงานยอดขาย";
        }
        private void LoadDropDownDevList() {
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", true);
            cboCompany.DataBind();
        }


        private void LoadData() {

            SetDefaultFilter();
            CheckPermission();
            SaleReportService.ListDailySale();
            BindData();
            GridBinding();

        }
        private void GridBinding() {

            grdDetail.DataSource = SaleReportService.SaleList;
            grdDetail.DataBind();

        }
        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = SaleReportService.SaleList;
        }

        protected void RptPOS140_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            var f = SaleReportService.PrintFilter;

            var stream = new MemoryStream();
            var report = new RptPOS140();
            report.initData(f.company, f.date_begin, f.date_end);
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();


            #region ใช้ Code popup ไปก่อน
            string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
            Response.Redirect(xurl);
            #endregion
        }

        //protected void RptPOS140_Click(object sender, EventArgs e) {
        //    SetDefaultFilter();
        //    SaleReportService.PrintFilter.reportname = "RptPOS140";
        //    MyPrint.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
        //    string myurl = "~/POSC/POSPrint/MyPrint";
        //    Response.RedirectPermanent(myurl);

        //}

        protected void RptPOS133_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            var f = SaleReportService.PrintFilter;
            if (f.company == "") {
                return;
            }
            var stream = new MemoryStream();
            var report = new RptPOS133();
            report.initData(f.company, f.date_begin, f.date_end);
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();


            #region ใช้ Code popup ไปก่อน
            string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
            Response.Redirect(xurl);
            #endregion
        }

        protected void RptPOS134_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            var f = SaleReportService.PrintFilter;
            if (f.company == "") {
                return;
            }
            var stream = new MemoryStream();
            var report = new RptPOS134();
            report.initData(f.company, f.date_begin, f.date_end);
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();


            #region ใช้ Code popup ไปก่อน
            string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
            Response.Redirect(xurl);
            #endregion
        }

        protected void RptPOS138_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            var f = SaleReportService.PrintFilter;

            var stream = new MemoryStream();
            var report = new RptPOS138();
            report.initData(f.company, f.date_begin, f.date_end);
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();


            #region ใช้ Code popup ไปก่อน
            string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
            Response.Redirect(xurl);
            #endregion
        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {

            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
        private void ShowPopAlert1(string msg_header, string msg_body, bool result) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;

            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;

            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            popAlert.ShowOnPageLoad = true;
        }
        protected void btnClear_Click(object sender, EventArgs e) {
            ClearBillFolder();

        }

        private void ClearBillFolder() {
            string mypath = Server.MapPath("~/TempFile/Print/");
            DirectoryInfo di = new DirectoryInfo(mypath);
            foreach (FileInfo file in di.GetFiles()) {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories()) {
                dir.Delete(true);
            }
        }

        protected void btnRefreshDataDiff_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            var f = SaleReportService.PrintFilter;
            var rr = POSSaleService.RefreshBill(f.date_begin, f.date_end);
            ShowPopAlert1("ผลดำเนินการ", rr.Message2, true);
        }
    }

}