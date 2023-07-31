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

namespace Robot.POS
{
    public partial class StockBalanceList : MyBasePage
    {
        public static string PreviousListPage { get { return HttpContext.Current.Session["stkbalanceList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["stkbalanceList_previouspage"]; } set { HttpContext.Current.Session["stkbalanceList_previouspage"] = value; } }
        public static List<vw_POS_STKBal> DocList { get { return (List<vw_POS_STKBal>)HttpContext.Current.Session["stkbalance_list"]; } set { HttpContext.Current.Session["stkbalance_list"] = value; } }
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
            if (POSStockService.FilterSet == null) {
                POSStockService.NewFilterSet();
            }
            var f = POSStockService.FilterSet;
            txtSearch.Text = f.SearchText;
            chkShowAvl.Checked = f.IsShowAviable;
            if (POSStockService.FilterSet.Company != "") {
                cboCompany.SelectedValue = f.Company;
                BindLocation();
                try {
                    cboLocation.SelectedValue = f.Location;
                } catch {
                    
                }
            }
            cboItemType.ItemType = f.ItemType; 
           // cboLocation.SelectedValue = f.Location==""?"X":f.Location;
        }
        private void SetDefaultFilter() {
            POSStockService.NewFilterSet();
            POSStockService.FilterSet.SearchText = txtSearch.Text;
            POSStockService.FilterSet.IsShowAviable = chkShowAvl.Checked;
            POSStockService.FilterSet.Company = cboCompany.SelectedValue;
            POSStockService.FilterSet.Location = cboLocation.SelectedValue;
            POSStockService.FilterSet.ItemType = cboItemType.SelectedValue;
        }
        private void BindData() {
            lblinfohead.Text = "สต็อก คงเหลือ";
        }
        private void LoadDropDownList() {
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", false);
            cboCompany.DataBind();
            cboItemType.DataSource = MasterTypeService.ListType("ITEM TYPE", true);
            cboItemType.DataBind();
    //        var location = LocationInfoService.ListStockLocation("", cboCompany.SelectedValue, true);
            BindLocation();
        }

        private void LoadData() {
            SetDefaultFilter();
            DocList = POSStockService.ListViewStockBalance();
            BindData();
            BindGrid();
        }

        private void BindGrid() {
            grdDetail.DataSource = DocList;
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = DocList;
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "itemid") {
                string id = e.KeyValue.ToString(); 
                StockMovementList.PreviousFromBalanceListPage = HttpContext.Current.Request.Url.PathAndQuery;
                StockMovementList.ITEMID = id;
                string myurl = $"~/POS/StockMovementList?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
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
            var location = LocationInfoService.ListStockBalLocation("", cboCompany.SelectedValue, true);
            cboLocation.DataSource = location;
            cboLocation.DataBind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            gridExport.FileName = "รายงานสต็อก_คงเหลือ_" + DateTime.Now.ToString("dd/MM/yyyy");
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void btnPrintStockBalance_Click(object sender, EventArgs e) {
            var stream = new MemoryStream();
            var report = new PrintStockBalance();
            report.initData();
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();

            #region มีปัญหากับ RawBT

            //if (MobileHelper.isMobileBrowser()) {
            //    string baseurl = POSHelper.GetBaseUrl();
            //    string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
            //    yurl = yurl.Replace("http://", "https://");
            //    LogJService.SaveLogJ("ismobile:" + yurl);
            //    string func = "sendUrlToPrint('" + yurl + "');";
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
            //    return;
            //} else {
            //    string xurl = String.Format("../TempFile/Print/{0}", myfilename);
            //    LogJService.SaveLogJ("iscomputer:" + xurl);
            //    Response.RedirectPermanent(xurl);
            //    //popPrint.ContentUrl = xurl;
            //    //popPrint.ShowOnPageLoad = true;
            //}
            #endregion


            #region ใช้ Code popup ไปก่อน
            string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
            //LogJService.SaveLogJ("iscomputer:" + xurl);
            //popPrint.ContentUrl = xurl;
            //popPrint.ShowOnPageLoad = true;
            Response.Redirect(xurl);
            #endregion


            //Print.MyPrint.NewReportFItler();
            //var f = Print.MyPrint.ReportFilterX;

            //Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string myurl = $"../POS/Print/MyPrint?report=PrintStockBalance";
            //Response.RedirectPermanent(myurl);
        }

        protected void btnStockBalByDate_Click(object sender, EventArgs e)
        {
            StockBalByDateList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/POS/StockBalByDateList?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }
    }

}