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

using Robot.Master.DA;
using Robot.POS.DA;
using static Robot.Data.DataAccess.LocationInfoService;
using Robot.POS.Print;
using Robot.Data.ServiceHelper;
using Robot.POSC.DA;
using static Robot.POS.DA.POSStockService;

namespace Robot.POS
{
    public partial class StockBalByDateList : MyBasePage
    {

        public class I_StockDateFiterSet
        {
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
            public String SearchText { get; set; }
            public String SearchItem { get; set; }
            public String Company { get; set; }
            public String Location { get; set; }
            public bool IsShowAviable { get; set; }
        }

        public static string PreviousListPage { get { return HttpContext.Current.Session["StockBalByDateList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["StockBalByDateList_previouspage"]; } set { HttpContext.Current.Session["StockBalByDateList_previouspage"] = value; } }
        public static List<SP_GetStkBalByDate_Result> DocList { get { return (List<SP_GetStkBalByDate_Result>)HttpContext.Current.Session["StockBalByDate_list"]; } set { HttpContext.Current.Session["StockBalByDate_list"] = value; } }
        public static I_StockDateFiterSet Filter { get { return (I_StockDateFiterSet)HttpContext.Current.Session["stockbalbydate_filter"]; } set { HttpContext.Current.Session["stockbalbydate_filter"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousListPage);
        }

        private void LoadDefaultFilter() {

            if (Filter == null)
            {
                Filter = NewFilterSet();
            }


            var f = Filter;
            if (f.Company != "") {
                cboCompany.SelectedValue = f.Company;
            }
            cboLocation.SelectedValue = f.Location==""?"":f.Location;
            dtDate.Value = f.DateFrom;
        }
        private void SetDefaultFilter() {
            NewFilterSet();
            Filter.Company = cboCompany.SelectedValue;
            Filter.Location = cboLocation.SelectedValue == "X"?"" : cboLocation.SelectedValue;
            Filter.DateFrom = dtDate.Date;
        }
        private void BindData() {
            lblinfohead.Text = "สต็อก คงเหลือ แบบเลือกวันที่";
        }
        private void LoadDropDownList() {
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", false);
            cboCompany.DataBind();
            BindLocation();
        }

        private void LoadData() {
            SetDefaultFilter();
            var f = Filter;
            DocList = POSStockService.ReportStockBalByDate(f.Company, f.Location, f.DateFrom);
            BindData();
            BindGrid();
        }

        private void BindGrid() {
            grdDetail.DataSource = DocList.Where(o => o.QtyBal != 0);
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = DocList;
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {

        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }

        protected void cboCompany_SelectedIndexChanged(object sender, EventArgs e) {
            BindLocation();
        }
        private void BindLocation() {
            cboLocation.DataSource = LocationInfoService.ListStockBalLocation("", cboCompany.SelectedValue, true);
            cboLocation.DataBind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            gridExport.FileName = "สต็อก_คงเหลือ_แบบเลือกวันที่_" + DateTime.Now.ToString("dd/MM/yyyy");
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        public static I_StockDateFiterSet NewFilterSet()
        {

            I_StockDateFiterSet n = new I_StockDateFiterSet();
            n.DateFrom = DateTime.Now.Date;
            n.DateTo = DateTime.Now.Date;
            n.Company = "";
            n.Location = "";
            n.SearchText = "";
            n.SearchItem = "";
            n.IsShowAviable = true;
            return n;
        }

        //protected void btnPrintStockBalance_Click(object sender, EventArgs e) {
        //    var stream = new MemoryStream();
        //    var report = new PrintStockBalance();
        //    report.initData();
        //    report.ExportToPdf(stream);

        //    string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
        //    string myfillfilename = @"/TempFile/Print/" + myfilename;
        //    string serverpath = Server.MapPath(myfillfilename);
        //    FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        //    stream.WriteTo(myfile);
        //    myfile.Close();
        //    stream.Close();

        //    #region มีปัญหากับ RawBT

        //    //if (MobileHelper.isMobileBrowser()) {
        //    //    string baseurl = POSHelper.GetBaseUrl();
        //    //    string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
        //    //    yurl = yurl.Replace("http://", "https://");
        //    //    LogJService.SaveLogJ("ismobile:" + yurl);
        //    //    string func = "sendUrlToPrint('" + yurl + "');";
        //    //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
        //    //    return;
        //    //} else {
        //    //    string xurl = String.Format("../TempFile/Print/{0}", myfilename);
        //    //    LogJService.SaveLogJ("iscomputer:" + xurl);
        //    //    Response.RedirectPermanent(xurl);
        //    //    //popPrint.ContentUrl = xurl;
        //    //    //popPrint.ShowOnPageLoad = true;
        //    //}
        //    #endregion


        //    #region ใช้ Code popup ไปก่อน
        //    string xurl = String.Format("../TempFile/Print/{0}", myfilename);
        //    //LogJService.SaveLogJ("iscomputer:" + xurl);
        //    //popPrint.ContentUrl = xurl;
        //    //popPrint.ShowOnPageLoad = true;
        //    Response.Redirect(xurl);
        //    #endregion


        //    //Print.MyPrint.NewReportFItler();
        //    //var f = Print.MyPrint.ReportFilterX;

        //    //Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
        //    //string myurl = $"../POS/Print/MyPrint?report=PrintStockBalance";
        //    //Response.RedirectPermanent(myurl);
        //}
    }

}