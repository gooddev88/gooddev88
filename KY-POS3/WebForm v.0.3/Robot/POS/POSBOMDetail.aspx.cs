using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
using Robot.POS.DA;

namespace Robot.POS
{
    public partial class POSBOMDetail : MyBasePage
    {
        public static string ItemTypeFGSelect { get { return HttpContext.Current.Session["itemtypefg_select"] == null ? "" : (string)HttpContext.Current.Session["itemtypefg_select"]; } set { HttpContext.Current.Session["itemtypefg_select"] = value; } }
        public static string ItemTypeRMSelect { get { return HttpContext.Current.Session["itemtyperm_select"] == null ? "" : (string)HttpContext.Current.Session["itemtyperm_select"]; } set { HttpContext.Current.Session["itemtyperm_select"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
            LoadDevDropDownList();
            if (POSBOMService.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ ", true, "");
            }

            if (!Page.IsPostBack) { 
                LoadDefaultFilter();
                LoadDropDownList();
                LoadData();
                RefreshGridSelect();
            }
        }



        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void CheckPermission() {
            var h = POSBOMService.DocSet.Info;
            if (h.BomID == "") {
                if (!PermissionService.CanCreate(hddmenu.Value)) {
                    btnSave.Visible = false;
                }
            }
        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }

            if (showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            POSBOMService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        private void LoadDefaultFilter() {
            if (POSBOMService.FilterSet == null) {
                POSBOMService.NewFilterSet();
            }
            txtSearch.Text = POSBOMService.FilterSet.SearchText;
        }
        private void SetDefaultFilter() {
            POSBOMService.FilterSet.SearchText = txtSearch.Text;
        }
        private void FilterItemSelect() {
            var bomType = cboTypeBom.SelectedValue;
            if (bomType=="SALE") {
                ItemTypeFGSelect = "FG";
                ItemTypeRMSelect = "FGK";
            }
            if (bomType== "KITCHEN") {
                ItemTypeFGSelect = "FGK";
                ItemTypeRMSelect = "RMK";
            }
        }  
        private void LoadData() {

            SetDefaultFilter();
            BindData();
            SetActiveControl();
       
            BindGrdBomLine();
        }

        private void RefreshGridSelect() {
            SetDefaultFilter();
            POSBOMService.ListDoc();
            BindGrid();

        }
        private void BindGrid() {
            grd.DataSource = POSBOMService.DocList;
            grd.DataBind();
        }
        private void BindData() {

            var h = POSBOMService.DocSet.Info;
            txtBomID.Enabled = h.BomID == "" ? true : false;
            txtBomID.Text = h.BomID;
            cboItemIDFG.Value = h.ItemIDFG;
            cboTypeBom.SelectedValue = h.UserForModule;
            txtDescription.Text = h.Description;

            chkIsDefault.Checked = h.IsDefault;
            ckIsActive.Checked = h.IsActive;
            txtRemark1.Text = h.Remark1;
            FilterItemSelect();
            BindCboUnit();
            GridTransactionLogBind();
        }

        private void SetActiveControl() {

        }

        private void GridTransactionLogBind() {
            if (POSBOMService.DocSet.Log == null) {
                grd_transaction_log.DataSource = new List<TransactionLog>();
                grd_transaction_log.DataBind();
            } else {
                var data = POSBOMService.DocSet.Log;
                grd_transaction_log.DataSource = data.OrderByDescending(o => o.TransactionDate).ToList();
                grd_transaction_log.DataBind();
            }
        }

        protected void grd_transaction_log_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            GridTransactionLogBind();
            grd_transaction_log.PageIndex = e.NewPageIndex;
            grd_transaction_log.DataBind();
        }

        private void LoadDropDownList() {
            cboTypeBom.DataSource = MasterTypeService.ListType("BOM TYPE", false);
            cboTypeBom.DataBind();
            FilterItemSelect();
        }

        private void BindCboUnit() {
            var h = POSBOMService.DocSet.Info;
            string fg_unit = cboItemIDFG.Value == null ? "" : cboItemIDFG.Value.ToString();
            string rm_unit = cboItemIDRM.Value == null ? "" : cboItemIDRM.Value.ToString();
            
            cboUnitFG.DataSource = POSItemService.ListViewItemUnitConvert(fg_unit); 
            cboUnitFG.DataBind();
            cboUnitRm.DataSource = POSItemService.ListViewItemUnitConvert(rm_unit);
            cboUnitRm.DataBind();
        }

        private void LoadDevDropDownList() {
            //cboItemIDFG.DataSource = ItemInfoServiceV2.ListViewItemByType("").Where(o => o.TypeID != "DISCOUNT").ToList();
            //cboItemIDFG.DataBind();

            //cboItemIDRM.DataSource = ItemInfoServiceV2.ListViewItemByType("").Where(o => o.TypeID != "DISCOUNT").ToList();
            //cboItemIDRM.DataBind();

            //cboRmUnit.DataSource = MasterTypeInfoService.MiniSelectListV2("UNIT", false);
            //cboRmUnit.DataBind();
        }

        private bool PrepairDataSave() {
            var h = POSBOMService.DocSet.Info;
            bool isnewrecord = false;//true =new trans / false = edit trans   
            if (h.BomID == "") {
                isnewrecord = true;
                if (txtBomID.Text.Trim() != "") {//input doc by user
                    h.BomID = txtBomID.Text.Trim();
                } else {//gen doc id by program 
                    POSBOMService.DocSet.NeedRunNextID = true;
                    h.BomID = IDRuunerService.GetNewID("BOM", h.ComID, false, "th", h.CreatedDate)[1];
                }
            }
            SetPrimaryData();

            return isnewrecord;
        }

        private void SetPrimaryData() {
            var h = POSBOMService.DocSet.Info;
            h.ItemIDFG = cboItemIDFG.Value.ToString();
            h.Description = txtDescription.Text.Trim();
            h.UserForModule = cboTypeBom.SelectedValue;
            h.Remark1 = txtRemark1.Text;
            h.IsDefault = chkIsDefault.Checked;
            h.IsActive = ckIsActive.Checked;
        }

        private bool ValidData() {

            if (cboItemIDFG.Value == null) {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }

            if (cboItemIDFG.Value.ToString() == "") {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }
            if (string.IsNullOrEmpty(cboUnitFG.SelectedValue)) {
                ShowPopAlert("Warning", "หน่วยสินค้า", false, "");
                return false;
            }
            if (string.IsNullOrEmpty(cboUnitRm.SelectedValue)) {
                ShowPopAlert("Warning", "หน่วยวัตถุดิบ", false, "");
                return false;
            }
            return true;
        }

        protected void btnDel_Click(object sender, EventArgs e) {
            var h = POSBOMService.DocSet.Info;
            POSBOMService.DeleteDoc(h.BomID,h.RComID);
            if (POSBOMService.DocSet.OutputAction.Result == "ok") {
                Response.Redirect($"~/POS/POSBOMList?&menu={hddmenu.Value}");
            } else {
                ShowPopAlert("Error", "Delete failed " + Environment.NewLine + POSBOMService.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }

        private void GoByRedirect() {
            string myurl = $"~/POS/POSBOMDetail?&menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            var h = POSBOMService.DocSet.Info;
            if (!ValidData()) {
                return;
            }

            var isnew = PrepairDataSave();
            POSBOMService.Save();
            if (POSBOMService.DocSet.OutputAction.Result == "ok") {//save successufull
                if (isnew) {
                    POSBOMService.IsNewDoc = true;
                    POSBOMService.GetDocSetByID(h.BomID,h.RComID);
                    GoByRedirect();
                } else {
                    ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                    LoadData();
                    RefreshGridSelect();
                }

            } else {
                ShowPopAlert("Error", POSBOMService.DocSet.OutputAction.Message1, false, "");
            }
        }

        #region ItemPrice
        protected void btnAddBomLine_Click(object sender, EventArgs e) {
            if (!ValidLineData()) {
                return;
            }

            POS_BOMLine my_line = POSBOMService.NewLine(); 
            my_line.ItemIDFG = cboItemIDFG.Value.ToString();
            my_line.ItemIDRM = cboItemIDRM.Value.ToString(); 
            var itemfg = POSItemService.GetItem(my_line.ItemIDFG);
            var itemrm = POSItemService.GetItem(my_line.ItemIDRM); 
            my_line.RMDescription = itemrm.Name1;
            my_line.FgUnit = itemfg.UnitID;
            my_line.RmUnit = itemrm.UnitID; 
            decimal rmqty = 0;
            decimal.TryParse(txtRmQty.Text, out rmqty);
            my_line.RmQty = rmqty; 
            decimal fgqty = 0;
            decimal.TryParse(txtFgQty.Text, out fgqty);
            my_line.FgQty = fgqty; 
            var r = POSBOMService.NewLineByBomLine(my_line, my_line.ItemIDRM); 
            if (r.Result == "fail") {
                ShowPopAlert("Error", r.Message1, false, "");
            } else {
                ResetControl();
                BindGrdBomLine();
            }
        }

        private void ResetControl() {
            txtRmQty.Text = "0";
            cboItemIDRM.Value = null;
            txtFgQty.Text = "1";
        }

        private bool ValidLineData() {

            if (cboItemIDRM.Value == null) {
                ShowPopAlert("Warning", "ระบุ วัตถุดิบ", false, "");
                return false;
            }

            if (cboItemIDRM.Value.ToString() == "") {
                ShowPopAlert("Warning", "ระบุ วัตถุดิบ", false, "");
                return false;
            }

            return true;
        }
        #endregion


        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(POSBOMService.DetailPreviousPage);
        }

        protected void btnCancel_Click(object sender, EventArgs e) {

        }

        protected void grd_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            
            if (e.CommandArgs.CommandName == "Select") {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString();
                POSBOMService.GetDocSetByID(id,rcom);
                LoadData();
            }
        }

        protected void grd_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) {

        }

        private void BindGrdBomLine() {
            grdBomLine.DataSource = POSBOMService.DocSet.line;
            grdBomLine.DataBind();
        }

        protected void grdBomLine_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Del") {
                int linenum = Convert.ToInt32(e.KeyValue);
                POSBOMService.DeleteBOMLine(linenum);
                BindGrdBomLine();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            RefreshGridSelect();
        }

        protected void grd_DataBinding(object sender, EventArgs e) {
            grd.DataSource = POSBOMService.DocList;
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            POSBOMService.NewTransaction("");
            string myurl = $"~/POS/POSBOMDetail?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

        protected void cboItemIDFG_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboItemIDFG.Value != null) {
                var item = POSItemService.GetItem(cboItemIDFG.Value.ToString());
                txtDescription.Text = item.Name1;
                BindCboUnit();
            }

        }
        protected void cboItemIDRM_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboItemIDRM.Value != null) { 
                BindCboUnit();
            }
        }
        protected void cboItemIDFG_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            try {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                sqlSearch.SelectCommand =
                       @"select ItemID,Name1,TypeID 
                            from (select ItemID,Name1,TypeID , row_number()over(order by ItemID desc) as [rn]  
                            from ItemInfo  as t 
                            where (( ItemID+Name1+TypeID ) like @filter) 
                                    and RCompanyID=@rcom and TypeID != 'DISCOUNT' 
                                    and (ItemID != 'DEFAULTMENU') 
                                    and TypeID=@type
                                    and IsActive=1) as st
                           where st.[rn] between @startIndex and @endIndex";
                sqlSearch.SelectParameters.Clear();
                sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
                sqlSearch.SelectParameters.Add("type", TypeCode.String, ItemTypeFGSelect);
                sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                comboBox.DataSource = sqlSearch;
                comboBox.DataBind();
            } catch (Exception) { 
            }
           
        }

        protected void cboItemIDFG_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"select ItemID,Name1,TypeID from ItemInfo where   ItemID = @ID and RCompanyID=@rcom order by ItemID";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }
      

        protected void cboItemIDRM_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            try {
                ASPxComboBox comboBox = (ASPxComboBox)source;
                sqlSearch.SelectCommand =
                       @"select ItemID,Name1,TypeID 
                                            from (select ItemID,Name1,TypeID , row_number()over(order by ItemID desc) as [rn]  
                                            from ItemInfo  as t 
                                            where (( ItemID+Name1+TypeID ) like @filter) 
                                                and RCompanyID=@rcom and TypeID != 'DISCOUNT' 
                                                and (ItemID != 'DEFAULTMENU') 
                                                and TypeID=@type
                                                and IsActive=1) as st 
                                                where st.[rn] between @startIndex and @endIndex";
                sqlSearch.SelectParameters.Clear();
                sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
                sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
                sqlSearch.SelectParameters.Add("type", TypeCode.String, ItemTypeRMSelect);
                sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
                sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
                comboBox.DataSource = sqlSearch;
                comboBox.DataBind();
            } catch (Exception) { 
            }
           
        }

        protected void cboItemIDRM_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"select ItemID,Name1,TypeID from ItemInfo where   ItemID = @ID and RCompanyID=@rcom order by ItemID";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboTypeBom_SelectedIndexChanged(object sender, EventArgs e) {
            FilterItemSelect();
        }

   
    }
}