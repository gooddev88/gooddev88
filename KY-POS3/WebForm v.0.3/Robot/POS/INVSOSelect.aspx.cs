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
using Robot.POS.DA;

namespace Robot.POS {
    public partial class INVSOSelect : MyBasePage
    {
        GridViewCommandColumn ComandColumn { get { return (GridViewCommandColumn)grid.Columns[0]; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString2HiddenFiled();
            if (!IsPostBack) { 
                LoadData();
            }
        }
        private void SetQueryString2HiddenFiled() { 
            hddmenu.Value = Request.QueryString["menu"];
            hddid.Value = Request.QueryString["id"]; 
        }
        private void LoadDropDownList() {

        }

        private void LoadDefaultFilter() {

        }

        private void LoadData() { 
            try {
                ShowStatusInfo();
            } catch (Exception ex) { 
                
            }
        }

        public List<POS_ORDERHead> DSLineX() { 
            return ARInvoiceService.ListSaleOrderHead();  
        }
        private void ShowStatusInfo() {
            OINVHead data = ARInvoiceService.DocSet.Head;
          
        
            var h = ARInvoiceService.DocSet.Head;
            //string strstatus = "<span class=" + "\"" + "badge badge-pill badge-info" + "\"" + ">Select SaleOrder</span>";
            string strstatus = "เลือกใบเบิก";
            if (h == null) {
                litStatus.Text = Server.HtmlDecode(strstatus);
                return;
            }
          
            litStatus.Text = Server.HtmlDecode(strstatus);
  
        }

        private void SetDefualtNewRecord() {
           
        }

        #region grid select template
        protected void GridView_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e) {
            e.Properties["cpVisibleRowCount"] = grid.VisibleRowCount;
            e.Properties["cpFilteredRowCountWithoutPage"] = GetFilteredRowCountWithoutPage();
        }
        protected void SelectAllMode_SelectedIndexChanged(object sender, EventArgs e) {
            grid.Selection.UnselectAll();
        }
        protected void lnkSelectAllRows_Load(object sender, EventArgs e) {
            ((ASPxHyperLink)sender).Visible = ComandColumn.SelectAllCheckboxMode != GridViewSelectAllCheckBoxMode.AllPages;
        }
        protected void lnkClearSelection_Load(object sender, EventArgs e) {
            ((ASPxHyperLink)sender).Visible = ComandColumn.SelectAllCheckboxMode != GridViewSelectAllCheckBoxMode.AllPages;
        }

        protected int GetFilteredRowCountWithoutPage() {
            int selectedRowsOnPage = 0;
            foreach (var key in grid.GetCurrentPageRowValues("ID")) {
                if (grid.Selection.IsRowSelectedByKey(key))
                    selectedRowsOnPage++;
            }
            return grid.Selection.FilteredCount - selectedRowsOnPage;
        }
        #endregion

        #region PopUp management

        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }
        #endregion
 

        protected void btnOk_Click(object sender, EventArgs e) {
            string[] fieldNames = new string[] { "OrdID" };
            List<object> columnValues = grid.GetSelectedFieldValues(fieldNames);

            if (columnValues == null) {
                return;
            }
            List<string> ids = new List<string>();
            foreach (var id in columnValues) {
                ids.Add((string)id);
            }       

            ARInvoiceService.ConvertOSaleOrderHead2Line(ids); 
            ClosePopup("OK-Line");
        }
        protected void btnClose_Click(object sender, EventArgs e) {
            ClosePopup("OK-Line");
        }
        protected void btnCancel_Click(object sender, EventArgs e) {
            ClosePopup("Cancel-Line");
        }
    }

}