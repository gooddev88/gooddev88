
using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.POS.DA;
using Robot.POS.Print;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Robot.POS
{
    public partial class POSORDERMobileList : System.Web.UI.Page
    {

        public static string PreviousPage { get { return HttpContext.Current.Session["ordermobile_previouspage"] == null ? "" : (string)HttpContext.Current.Session["ordermobile_previouspage"]; } set { HttpContext.Current.Session["ordermobile_previouspage"] = value; } }
        public static List<POS_ORDERHead> ListDoc { get { return (List<POS_ORDERHead>)HttpContext.Current.Session["ordermobile_list"]; } set { HttpContext.Current.Session["ordermobile_list"] = value; } }
        public static DateTime ShowDate { get { return HttpContext.Current.Session["ChangeDate_previouspage"] == null ? DateTime.Now.Date : (DateTime)HttpContext.Current.Session["ChangeDate_previouspage"]; } set { HttpContext.Current.Session["ChangeDate_previouspage"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            LoadDropDownDevList();
            CloseAlert();
            SetQueryString();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }
        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void LoadDefaultFilter()
        {

            if (POSOrderService.FilterSet == null)
            {
                POSOrderService.NewFilterSet();
                var currentMenu = POSOrderService.ListStatus(hddmenu.Value).Where(o => o.Menu == hddmenu.Value).FirstOrDefault();

                if (currentMenu != null)
                {
                    POSOrderService.FilterSet.Status = currentMenu.Value;
                }
            }
            var f = POSOrderService.FilterSet;
            dtOrdDateBegin.Value = f.DateFrom;
            dtOrdDateEnd.Value = f.DateTo;
            txtSearch.Text = f.SearchText;
            cboStatus.SelectedValue = f.Status;

        }
        private void SetDefaultFilter()
        {
            POSOrderService.NewFilterSet();
            var f = POSOrderService.FilterSet;
            f.SearchText = txtSearch.Text;
            f.Status = cboStatus.SelectedValue;
            if (dtOrdDateBegin.Value != null)
            {
                f.DateFrom = dtOrdDateBegin.Date;
            }
            if (dtOrdDateEnd.Value != null)
            {
                f.DateTo = dtOrdDateEnd.Date;
            }
        }

        private void LoadDropDownList() {
            cboStatus.DataSource = POSOrderService.ListStatus(hddmenu.Value);
            cboStatus.DataBind();
        }

        private void LoadDropDownDevList() {

        }

        private void LoadData()
        {
            SetDefaultFilter();
            SetActiveControl();
            ListDoc = POSOrderService.ListDocOrderMobile();
            BindData();
            GridBinding();
        }

        private void BindData() {
            lblheadinfo.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            lblshowdate.Text = Convert.ToDateTime(ShowDate).ToString("dd/MM/yyyy");
        }

        private void SetActiveControl()
        {
            if (hddmenu.Value == "2001")
            {
                btnNew.Visible = true;
            }
            else if (hddmenu.Value == "2002")
            {
                btnPrintFGK.Visible = true;
                btnPrintRMK.Visible = true;
                btnNew.Visible = false;
            }
            else
            {
                btnPrintFGK.Visible = false;
                btnPrintRMK.Visible = false;
                btnNew.Visible = false;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SetDefaultFilter();
            LoadData();
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousPage);
        }

        private void ShowAlert(string msg, string type)
        {

            lblAlertmsg.Text = msg;
            if (type == "Success")
            {
                lblAlertmsg.ForeColor = Color.Green;
            }
            if (type == "Error")
            {
                lblAlertmsg.ForeColor = Color.Red;
            }
            if (type == "Warning")
            {
                lblAlertmsg.ForeColor = Color.YellowGreen;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('','" + type + "');", true);
        }
        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private void GridBinding() {
            grdline.DataSource = ListDoc;
            grdline.DataBind();
        }

        protected void grdline_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (grdline.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }

        protected void grdLine_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "show")
            {
                string id = e.CommandArgument.ToString();
                POSOrderService.GetDocSetByID(id);
                POSOrderService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"~/POS/POSORDERDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void grdLine_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            

        }

        protected void grdline_DataBinding(object sender, EventArgs e)
        {

        }

        protected void btnChangeDate_Click(object sender, EventArgs e)
        {
            ChangeDate.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/POS/ChangeDate?menu={hddmenu.Value}";
            Response.Redirect(url);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            POSOrderService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            POSOrderService.NewDocPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/POS/POSORDERNewDoc?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

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