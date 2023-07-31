using System;
using System.Linq;
using System.Web;

using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;

namespace Robot.OMASTER
{
    public partial class BookBankList : MyBasePage {
        public static string PreviousPage { get { return (string)HttpContext.Current.Session["bookbanklist_previouspage"]; } set { HttpContext.Current.Session["bookbanklist_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {
                LoadDefaultFilter();                
                LoadData();
            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
            hdddoctype.Value = Request.QueryString["doctype"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }
        private void LoadDefaultFilter() {
            if (BookBankService.FilterSet == null) {
                BookBankService.NewFilterSet();
            }

            txtSearch2.Text = BookBankService.FilterSet.SearchText;
          chkShowClose.Checked = BookBankService.FilterSet.ShowInActive;
        }
        private void SetDefaultFilter() {
            BookBankService.NewFilterSet();
            BookBankService.FilterSet.SearchText = txtSearch2.Text.Trim();
            BookBankService.FilterSet.ShowInActive = chkShowClose.Checked;
        }

        private void LoadDropDownDevList() {

        }

        private void LoadData() {
  
            SetDefaultFilter();
         BookBankService.ListDoc(true);
            GridBinding();
            SetActiveControl();
        }
        private void SetActiveControl()
        {
            hddTopic.Value  = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            lblinfohead.Text = hddTopic.Value;
        }
        private void GridBinding() {
            grdDetail.DataSource = BookBankService.BooKList;
            grdDetail.DataBind();
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "sel") {
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString();
                BookBankDetail.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
                BookBankService.GetDocSetByID(id);
                string url = $"../MAINMAS/BookBankDetail?menu={hddmenu.Value}";
                Response.Redirect(url);                
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = BookBankService.BooKList;
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            BookBankDetail.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            BookBankService.NewTransaction();
            string url = $"../MAINMAS/BookBankDetail?menu={hddmenu.Value}";
            Response.Redirect(url);
        }
        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }

    }

}