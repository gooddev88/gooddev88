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
using Robot.POS.Print;
using Robot.Communication.API.Line;
using Robot.Communication.DA;
using Robot.Helper;

namespace Robot.POS
{
    public class I_StatusBinding
    {
        public String Value { get; set; }
        public String Desc { get; set; }
    }
    public partial class POSPOList : MyBasePage {
        public static string PreviousListPage { get { return HttpContext.Current.Session["poList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["poList_previouspage"]; } set { HttpContext.Current.Session["poList_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popAlert.ShowOnPageLoad = false;
            SetQueryString();
            LoadDevDropDownList();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString()
        {
            hddmenu.Value = Request.QueryString["menu"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousListPage);
        }

        private void LoadDefaultFilter() {
            if (POS_POService.FilterSet == null)
            {
                POS_POService.NewFilterSet();
            }
            txtSearch.Text = POS_POService.FilterSet.SearchText;
            dtPODateBegin.Value = POS_POService.FilterSet.DateFrom;
            dtPODateEnd.Value = POS_POService.FilterSet.DateTo;
            chkShowClose.Checked = !POS_POService.FilterSet.ShowActive;
            cboStatus.SelectedValue = POS_POService.FilterSet.Status;
            cboVendor.Value = POS_POService.FilterSet.Vendor;
        }
        private void SetDefaultFilter() {
            #region default caption
            lblinfohead.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            #endregion

            POS_POService.NewFilterSet();
            POS_POService.FilterSet.ShowActive = !chkShowClose.Checked;
            POS_POService.FilterSet.SearchText = txtSearch.Text;
            POS_POService.FilterSet.Status = cboStatus.SelectedValue;
            if (dtPODateBegin.Value != null)
            {
                POS_POService.FilterSet.DateFrom = dtPODateBegin.Date;
            }
            if (dtPODateEnd.Value != null)
            {
                POS_POService.FilterSet.DateTo = dtPODateEnd.Date;
            }

            POS_POService.FilterSet.Vendor = "";
            if (cboVendor.Value != null)
            {
                POS_POService.FilterSet.Vendor = cboVendor.Value.ToString();
            }
        }

        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        private void LoadDropDownList() {
            cboStatus.DataSource = ListStatus();
            cboStatus.DataBind();
        }

        private void LoadDevDropDownList()
        {
            cboVendor.DataSource = VendorInfoV2Service.ListViewVendorByID("");
            cboVendor.DataBind();
        }

        public static List<I_StatusBinding> ListStatus()
        {
            List<I_StatusBinding> result = new List<I_StatusBinding>();
            result.Add(new I_StatusBinding { Value = "OPEN", Desc = "OPEN" });
            result.Add(new I_StatusBinding { Value = "RECEIVED", Desc = "RECEIVED" });
            return result;
        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }
            if (showbutton == "") {
                btnCancel.Visible = false;
            }
            if (showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            popAlert.ShowOnPageLoad = true;
        }

        private void LoadData() {
            try {
                SetDefaultFilter();
                POS_POService.ListDoc();

                BindData();
                BindGrid();
            } catch (Exception ex) {
            }
        }

        private void BindData()
        {

        }

        private void BindGrid()
        {
            grdDetail.DataSource = POS_POService.DocList;
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = POS_POService.DocList;
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e)
        {
            GridBinding();
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            //gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Select") {   
                
                string id = e.KeyValue.ToString();             
                POS_POService.GetDocSetByID(id);

                POS_POService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"~/POS/POSPODetail?menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {

            POS_POService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            POS_POService.NewDocPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            POS_POService.NewTransaction("");
            string myurl = $"~/POS/POSPONewDoc?menu={hddmenu.Value}";
           Response.RedirectPermanent(myurl);
        }

        //protected void btnPrintPO_Click(object sender, EventArgs e)
        //{
        //    Print.MyPrint.NewReportFItler();
        //    var f = Print.MyPrint.ReportFilterX;
        //    f.Begin = dtPODateBegin.Date;
        //    f.End = dtPODateEnd.Date;
        //    f.DocType = "POPRINT";

        //    Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
        //    string myurl = $"../POS/Print/MyPrint?report=POPrint1";
        //    Response.RedirectPermanent(myurl);
        //}

        protected void btnPrintPO_Click(object sender, EventArgs e)
        {
            MyPrint.NewReportFItler();
            var f = MyPrint.ReportFilterX;
            f.Begin = dtPODateBegin.Date;
            f.End = dtPODateEnd.Date;
            f.DocType = "POPRINT";

            string vendor = "";
            if (cboVendor.Value != null)
            {
                vendor = cboVendor.Value.ToString();
            }

            var stream = new MemoryStream();
            var report = new PrintPOFormOrder();
            report.initData(f.Begin, f.End, f.DocType, vendor);
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

        protected void btnnotifyPo_Click(object sender, EventArgs e)
        {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var link = LinkService.GetLinkByLinkInfo("apibase_url");
            var myMsg = LineMsgAPI.MessageHelper("ขอเงินซื้อของหน่อย ");
            myMsg = myMsg  + link.AppLink + "/POSREPORTS/PO/ReportPOSumByStore/"+ rcom +"/1?openExternalBrowser=1";

            var linelogin = LineRegisterService.ListLineLogIn();

            foreach (var l in linelogin)
            {
                 var rs = LineMsgAPI.SendLineMsgAPIByUser(myMsg, l.UserID, "KYPOS");
                if (rs.Result == "fail")
                {
                    ShowPopAlert("Error", rs.Message1, false, "");
                }
            }

            ShowPopAlert("Success", "ส่งสำเร็จ", true, "");

        }

        protected void btnPOReport_Click(object sender, EventArgs e) {
            string rooturl = URLHelper.GetBaseUrlNoAppPath();
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("POSREPORTS/PO/ReportPOSumByStore/1/web1");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);
           
        }

       
    }

}