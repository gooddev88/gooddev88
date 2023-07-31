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

namespace Robot.POS
{
    public class I_TypeBOM
    {
        public String Value { get; set; }
        public String Desc { get; set; }
    }
    public partial class POSBOMList : MyBasePage {
        public static string PreviousListPage { get { return HttpContext.Current.Session["bomList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["bomList_previouspage"]; } set { HttpContext.Current.Session["bomList_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popAlert.ShowOnPageLoad = false;
            SetQueryString();
            if (!IsPostBack) {
                LoadDropDownList();
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
            txtSearch.Text = POSBOMService.FilterSet.SearchText;
            cboType.SelectedValue = POSBOMService.FilterSet.DocType;
            chkShowClose.Checked = !POSBOMService.FilterSet.ShowActive;
        }
        private void SetDefaultFilter() {
            #region default caption
            lblinfohead.Text = "สูตรการผลิต (Bom) ";
            #endregion

            POSBOMService.NewFilterSet();
            POSBOMService.FilterSet.ShowActive = !chkShowClose.Checked;
            POSBOMService.FilterSet.SearchText = txtSearch.Text;
            POSBOMService.FilterSet.DocType = cboType.SelectedValue;
        }

        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        private void LoadDropDownList() {

            cboType.DataSource = ListType();
            cboType.DataBind();
        }

        public static List<I_TypeBOM> ListType()
        {
            List<I_TypeBOM> result = new List<I_TypeBOM>();
            result.Add(new I_TypeBOM { Value = "", Desc = "ทั้งหมด" });
            result.Add(new I_TypeBOM { Value = "SALE", Desc = "SALE" });
            result.Add(new I_TypeBOM { Value = "KITCHEN", Desc = "KITCHEN" });
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
                POSBOMService.ListDoc();
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
            grdDetail.DataSource = POSBOMService.DocList;
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = POSBOMService.DocList;
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
              var rcom=  LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                string id = e.KeyValue.ToString();
                POSBOMService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                POSBOMService.GetDocSetByID(id, rcom);
                string myurl = $"~/POS/POSBOMDetail?menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {

           POSBOMService.NewTransaction("");
           POSBOMService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            POSBOMService.NewTransaction("");
            string myurl = $"~/POS/POSBOMDetail?menu={hddmenu.Value}";
           Response.RedirectPermanent(myurl);
        }

    }

}