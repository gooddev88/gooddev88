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
    public class I_StatusAdjust
    {
        public String Value { get; set; }
        public String Desc { get; set; }
    }
    public partial class POSStkAdjustList : MyBasePage
    {
        public static string PreviousListPage { get { return HttpContext.Current.Session["StkadjustList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["StkadjustList_previouspage"]; } set { HttpContext.Current.Session["StkadjustList_previouspage"] = value; } }
        
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
      
            if (POSStkAdjustService.FilterSet == null) {
                POSStkAdjustService.NewFilterSet();
            }
            var f = POSStkAdjustService.FilterSet;
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
            POSStkAdjustService.NewFilterSet();
            var f = POSStkAdjustService.FilterSet;
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
            cboStatus.DataSource = ListStatus();
            cboStatus.DataBind();
        }

        public static List<I_StatusAdjust> ListStatus()
        {
            List<I_StatusAdjust> result = new List<I_StatusAdjust>();
            result.Add(new I_StatusAdjust { Value = "OPEN", Desc = "OPEN" });
            result.Add(new I_StatusAdjust { Value = "CLOSED", Desc = "CLOSED" });
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
            SetDefaultFilter();
            POSStkAdjustService.ListDoc();
            BindData();
            BindGrid();
        }

        private void BindData() {

        }

        private void BindGrid() {
            grdDetail.DataSource = POSStkAdjustService.DocList;
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = POSStkAdjustService.DocList;
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
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
            if (e.CommandArgs.CommandName == "sel") {

                int id = Convert.ToInt32(e.KeyValue);
                POSStkAdjustService.GetDocSetByID(id);
                POSStkAdjustService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"~/POS/POSStkAdjustDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            POSStkAdjustService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            POSStkAdjustService.NewDocPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/POS/AdjustNewDoc?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

    }

}