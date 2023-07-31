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

namespace Robot.Master
{
    public partial class ItemList : MyBasePage {
        public static string PreviousListPage { get { return HttpContext.Current.Session["itemList_previouspage"] == null ? "" : (string)HttpContext.Current.Session["itemList_previouspage"]; } set { HttpContext.Current.Session["itemList_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popAlert.ShowOnPageLoad = false;
            SetQueryString2HiddenFiled();
            if (!IsPostBack) {
                if (POSItemService.FilterSet == null) {
                    POSItemService.NewFilterSet();
                }
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString2HiddenFiled() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousListPage);
        }

        private void LoadDefaultFilter() {
            txtSearch.Text = POSItemService.FilterSet.SearchText;
            cboType.SelectedValue = POSItemService.FilterSet.DocType;
            chkShowClose.Checked = !POSItemService.FilterSet.ShowActive;
        }
        private void SetDefaultFilter() {
            #region default caption
            hddTopic.Value = "สินค้าและบริการ " + "(" + hddmenu.Value + ")";
            #endregion

            POSItemService.NewFilterSet();
            POSItemService.FilterSet.ShowActive = !chkShowClose.Checked;
            POSItemService.FilterSet.DocType = cboType.SelectedValue;
            POSItemService.FilterSet.SearchText = txtSearch.Text;
        }

        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }
        private void LoadDropDownList() {

            List<string> filter_Itemtype = new List<string>() { "DISCOUNT" };
            var Itemtype = MasterTypeService.ListType("ITEM TYPE", true);
            Itemtype = Itemtype.Where(o => !filter_Itemtype.Contains(o.ValueTXT)).ToList();
            cboType.DataSource = Itemtype;
            cboType.DataBind();
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
                POSItemService.ListDoc();
                BindData();
                BindGrid();
            } catch (Exception ex) {
            }
        }
        private void BindGrid() {
            grdDetail.DataSource = POSItemService.DocList.Where(o => o.TypeID != "DISCOUNT").ToList();
            grdDetail.DataBind();
        }
        private void BindData() {
            lblinfohead.Text = "สินค้าและบริการ";
        }
        private void GridBinding() {
            grdDetail.DataSource = POSItemService.DocList.Where(o => o.TypeID != "DISCOUNT").ToList(); 
        }
        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {

        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Select") {
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString();

                POSItemService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                POSItemService.GetDocSetByID(id);
                string myurl = $"~/Master/ItemDetail?menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        protected void btnNew_Click(object sender, EventArgs e) {

            POSItemService.NewDocPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/Master/ItemNewDoc?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnSavePhoto_Click(object sender, EventArgs e) {
            XFilesService.ConvertByte2File("", "ITEMS_PHOTO_PROFILE");

        }

        protected void btnCopyPrice_Click(object sender, EventArgs e)
        {
            ItemPriceCopy.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/Master/ItemPriceCopy?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

    }

}