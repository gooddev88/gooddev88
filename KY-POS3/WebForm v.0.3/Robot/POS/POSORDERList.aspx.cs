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
using Robot.Data.ServiceHelper;
using Robot.POSC.DA;
using Robot.POS.Print;

namespace Robot.POS
{
    public partial class POSORDERList : MyBasePage
    {
        public static string PreviousListPage { get { return HttpContext.Current.Session["ordList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["ordList_previouspage"]; } set { HttpContext.Current.Session["ordList_previouspage"] = value; } }
        
        protected void Page_Load(object sender, EventArgs e) {
            popAlert.ShowOnPageLoad = false;
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
      
            if (POSOrderService.FilterSet == null) {
                POSOrderService.NewFilterSet();
                var currentMenu = POSOrderService.ListStatus(hddmenu.Value).Where(o => o.Menu == hddmenu.Value).FirstOrDefault();
            
                if (currentMenu != null) {
                    POSOrderService.FilterSet.Status = currentMenu.Value;
                }
            }
            var f = POSOrderService.FilterSet;
            dtOrdDateBegin.Value = f.DateFrom;
            dtOrdDateEnd.Value = f.DateTo;
            txtSearch.Text = f.SearchText;
            chkShowClose.Checked = !f.ShowActive;
            cboStatus.SelectedValue = f.Status;

        }
        private void SetDefaultFilter() {
            #region default caption
            lblinfohead.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            #endregion 
            POSOrderService.NewFilterSet();
            var f = POSOrderService.FilterSet;
            f.ShowActive = !chkShowClose.Checked;
            f.SearchText = txtSearch.Text;
            f.Status = cboStatus.SelectedValue;
            if (dtOrdDateBegin.Value!=null) {
                f.DateFrom = dtOrdDateBegin.Date;
            }
            if (dtOrdDateEnd.Value != null) {
                f.DateTo = dtOrdDateEnd.Date;
            }
        }

        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        private void LoadDropDownList() {
            cboStatus.DataSource = POSOrderService.ListStatus(hddmenu.Value);
            cboStatus.DataBind();
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

            SetDefaultFilter();
            SetActiveControl();
            POSOrderService.ListDoc();
            BindData();
            BindGrid();

        }

        private void BindData() {

        }

        private void BindGrid() {
            grdDetail.DataSource = POSOrderService.DocList;
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = POSOrderService.DocList;
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        private void SetActiveControl()
        {
            if (hddmenu.Value == "2001")
            {
                btnNew.Visible = true;
            }else if (hddmenu.Value == "2002") {
                //btnPrintFGK.Visible = true;
                //btnPrintRMK.Visible = true;
                btnNew.Visible = false;
            }
            else
            {
                //btnPrintFGK.Visible = false;
                //btnPrintRMK.Visible = false;
                btnNew.Visible = false;
            }
            
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
                POSOrderService.GetDocSetByID(id);

                POSOrderService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"~/POS/POSORDERDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {


            POSOrderService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            POSOrderService.NewDocPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;

            string myurl = $"~/POS/POSORDERNewDoc?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        //protected void btnPrintFGK_Click(object sender, EventArgs e)
        //{
        //    Print.MyPrint.NewReportFItler();
        //    var f = Print.MyPrint.ReportFilterX;
        //    f.Begin = dtOrdDateBegin.Date;
        //    f.End = dtOrdDateEnd.Date;
        //    //f.DocType = "FGK";
        //    f.DocType = "ORDERPRINT";

        //    Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
        //    string myurl = $"../POS/Print/MyPrint?report=OrderPrint";
        //    Response.RedirectPermanent(myurl);
        //}

        protected void btnPrintFGK_Click(object sender, EventArgs e)
        {
            MyPrint.NewReportFItler();
            var f = MyPrint.ReportFilterX;
            f.Begin = dtOrdDateBegin.Date;
            f.End = dtOrdDateEnd.Date;
            f.DocType = "ORDERPRINT";

            var stream = new MemoryStream();
            var report = new PrintSumQtyItemInCompany();
            report.initData(f.Begin, f.End, f.DocType);
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

        //protected void btnPrintRMK_Click(object sender, EventArgs e)
        //{
        //    Print.MyPrint.NewReportFItler();
        //    var f = Print.MyPrint.ReportFilterX;
        //    f.Begin = dtOrdDateBegin.Date;
        //    f.End = dtOrdDateEnd.Date;
        //    //f.DocType = "RMK";
        //    f.DocType = "PURCHASEPRINT";

        //    Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
        //    string myurl = $"../POS/Print/MyPrint?report=OrderPrint";
        //    Response.RedirectPermanent(myurl);
        //}

        protected void btnPrintRMK_Click(object sender, EventArgs e)
        {
            MyPrint.NewReportFItler();
            var f = MyPrint.ReportFilterX;
            f.Begin = dtOrdDateBegin.Date;
            f.End = dtOrdDateEnd.Date;
            //f.DocType = "RMK";
            f.DocType = "PURCHASEPRINT";

            var stream = new MemoryStream();
            var report = new PrintSumQtyItemInCompany();
            report.initData(f.Begin, f.End, f.DocType);
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

    }

}