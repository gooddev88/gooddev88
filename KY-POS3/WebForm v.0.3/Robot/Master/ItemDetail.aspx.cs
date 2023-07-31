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
using Robot.Upload.UploadPage;
using Robot.Master.Upload;

namespace Robot.Master
{
    public partial class ItemDetail : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
          
            SetQueryString();
            LoadDevDropDownList();
            if(POSItemService.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ ", true, "");
            }
            if(!Page.IsPostBack) { 
           
                LoadDefaultFilter(); 
                LoadDropDownList(); 
                LoadData();
                RefreshGridSelect();
            }
        }

        public string ShowItemType() {
            var i = POSItemService.DocSet.Info;
            return i.TypeID;
        }

        private void SetQueryString() {
        
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void CheckPermission() {
            var h = POSItemService.DocSet.Info;
            if(h.ItemID == "") {
                if(!PermissionService.CanCreate(hddmenu.Value)) {
                    btnSave.Visible = false;
                }
            }
        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if(result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }
           
            if(showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            POSItemService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        private void LoadDefaultFilter() {
            if(POSItemService.FilterSet == null) {
                POSItemService.NewFilterSet();
            }
            txtSearch.Text = POSItemService.FilterSet.SearchText;
            cboTypeFilter.SelectedValue = POSItemService.FilterSet.DocType;
        }
        private void SetDefaultFilter() {
            POSItemService.FilterSet.SearchText = txtSearch.Text;
            POSItemService.FilterSet.DocType = cboTypeFilter.SelectedValue;
        }

        private void LoadData() {
        
            SetDefaultFilter(); 
            BindData();  
            SetActiveControl();
            BindGrdItemPrice();
            BindGrdItemInPointRate();
            LoadProfile();
        }

        private void RefreshGridSelect() {
            SetDefaultFilter();
            POSItemService.ListDoc();
            BindGrid();
            BindGrdItemPrice();
            BindGrdItemInPointRate();
            LoadProfile();
        }
        private void BindGrid() {
            grd.DataSource = POSItemService.DocList.Where(o=>o.TypeID!= "DISCOUNT").ToList();
            grd.DataBind();
        }
        private void BindData() {
            var Item = POSItemService.DocSet.Info;
      
            var h = POSItemService.DocSet.Info;
            txtItemID.Enabled = h.ItemID==""?true:false;
            txtItemID.Text = Item.ItemID;
            txtRefID.Text = Item.RefID;
            txtName1.Text = Item.Name1;
            txtName2.Text = Item.Name2;
            
            try{ cboType.SelectedValue = Item.TypeID; } catch{ }
            cboVendor.Value = Item.VendorID;
            txtCost.Text = Item.Cost ==null?"0":Convert.ToDecimal( Item.Cost).ToString("n2");
            try  {  cboCate.SelectedValue = Item.CateID;  } catch      {  }
            try { cboUnitID.SelectedValue = Item.UnitID; } catch { }
            try { cboGroup1.SelectedValue = Item.Group1ID; } catch { }
            try { cboGroup2.SelectedValue = Item.Group2ID; } catch { }
            try { cboGroup3.SelectedValue = Item.Group3ID; } catch { }

            txtSort.Text = Convert.ToInt32(Item.Sort).ToString("N0");
            txtPrice.Text = Item.Price.ToString("N2");
            //cboVatType.SelectedValue = Item.VatTypeID;
            ckIsHold.Checked = Item.IsHold;
            ckIsActive.Checked = Item.IsActive;
            txtRemark1.Text = Item.Remark1;
            txtRemark2.Text = Item.Remark2;
 
            GridTransactionLogBind();
        }

        private void SetActiveControl() {
            if (POSItemService.DocSet.Info.ItemID == "")
            {
                btnUploadProfile.Visible = false;
                btnRemoveProfile.Visible = false;
                divitemInPointRate.Visible = false;
            }
            else
            {
                btnUploadProfile.Visible = true;
                btnRemoveProfile.Visible = true;
                divitemInPointRate.Visible = true;
            }
            divOther.Visible = false;
        }

        private void GridTransactionLogBind() {
            if(POSItemService.DocSet.Log == null) {
                grd_transaction_log.DataSource = new List<TransactionLog>();
                grd_transaction_log.DataBind();
            } else {
                var data = POSItemService.DocSet.Log;
                grd_transaction_log.DataSource = data.OrderByDescending(o => o.TransactionDate).ToList();
                grd_transaction_log.DataBind();
            }
        }

        private void LoadDropDownList() {
            cboCompany.DataSource = CompanyService.ListBranch(false);
            cboCompany.DataBind();
         
            cboUnitID.DataSource = MasterTypeService.ListType("UNIT", false);
            cboUnitID.DataBind();

            var Packing = MasterTypeService.ListType("ITEM PACK", true);
            cboPacking.DataSource = Packing;
            cboPacking.DataBind();

            List<string> filter_Itemtype = new List<string>() { "DISCOUNT" };
            var Itemtype = MasterTypeService.ListType("ITEM TYPE", true);
            Itemtype = Itemtype.Where(o => !filter_Itemtype.Contains(o.ValueTXT)).ToList();
            cboType.DataSource = Itemtype;
            cboType.DataBind();

            cboTypeFilter.DataSource = Itemtype;
            cboTypeFilter.DataBind();

            List<string> filter_Itemcate = new List<string>() { "DISC PER", "DISC AMT" };
            var data = MasterTypeService.ListType("ITEM CATE", true);
            data = data.Where(o => !filter_Itemcate.Contains(o.ValueTXT)).ToList();

            cboCate.DataSource = data;
            cboCate.DataBind();

            cboGroup1.DataSource = MasterTypeService.ListType("ITEM GROUP1", true);
            cboGroup1.DataBind();
            cboGroup2.DataSource = MasterTypeService.ListType("ITEM GROUP2", false);
            cboGroup2.DataBind();
            cboGroup3.DataSource = MasterTypeService.ListType("ITEM GROUP3", false);
            cboGroup3.DataBind();
            cboGroup3.DataSource = MasterTypeService.ListType("ITEM GROUP3", false);
            cboGroup3.DataBind();
     
            cboPriceTaxcon.DataSource = MasterTypeService.ListType("SALE RPICE TAX CON", false);
            cboPriceTaxcon.DataBind();
            cboCompanyID2.DataSource = CompanyService.ListBranch(true);
            cboCompanyID2.DataBind();

            cboShipTo.DataSource = POSItemService.ListShipTo();
            cboShipTo.DataBind();
        }

        private void LoadDevDropDownList() {

        }

        private bool PrepairDataSave() { 
            var h = POSItemService.DocSet.Info;  
            bool isnewrecord = false;//true =new trans / false = edit trans   
            if(h .ItemID== "") {
                isnewrecord = true;
                if(txtItemID.Text.Trim() != "") {//input doc by user
                    h.ItemID = txtItemID.Text.Trim(); 
                } else {//gen doc id by program 
                    h.ItemID = IDRuunerService.GetNewID("ITEM", "", false, "th", h.CreatedDate)[1]; 
                }
            } 
            SetPrimaryData();
             
            return isnewrecord;
        }

        private void SetPrimaryData() {
            var h = POSItemService.DocSet.Info;
           
            h.RefID = txtRefID.Text.Trim(); 
            h.Name1 = txtName1.Text.Trim();
            h.Name2 = txtName2.Text.Trim();
            h.TypeID = cboType.SelectedValue;
            h.CateID = cboCate.SelectedValue;
            h.UnitID = cboUnitID.SelectedValue;
            h.Group1ID = cboGroup1.SelectedValue;
            h.Group2ID = cboGroup2.SelectedValue;
            h.Group3ID = cboGroup3.SelectedValue;
            h.PackingID = cboPacking.SelectedValue;

            h.UnitID = cboUnitID.SelectedValue;
      
            try { h.Price = Convert.ToDecimal(txtPrice.Text); } catch { h.Price = 0; }
            try { h.Cost = Convert.ToDecimal(txtCost.Text); } catch { h.Cost = 0; }
            try { h.Sort = Convert.ToInt32(txtSort.Text); } catch { h.Sort = 0; }
            h.VendorID = "";
            if (cboVendor.Value!=null) {
                h.VendorID = cboVendor.Value.ToString();
            }

            //h.VatTypeID = cboVatType.SelectedValue;
            h.Remark1 = txtRemark1.Text;
            h.Remark2 = txtRemark2.Text;

            h.IsHold = ckIsHold.Checked;
            h.IsActive = ckIsActive.Checked;
            h.IsKeepStock = true;
            h.IsSysData = false;
            h.Status = "ACTIVE";
        }

        private bool ValidData() { 

            if (cboCate.SelectedValue == "")
            {
                ShowPopAlert("Error", "ไม่ได้ระบุ หมวดสินค้า", false, "");
                return false;
            }

            if (cboType.SelectedValue == "")
            {
                ShowPopAlert("Error", "ไม่ได้ระบุ ประเภทสินค้า", false, "");
                return false;
            }

            return true;
        }
 
        protected void btnDel_Click(object sender, EventArgs e) {
            var h = POSItemService.DocSet.Info;
            POSItemService.DeleteDoc(h.ItemID);
            if(POSItemService.DocSet.OutputAction.Result == "ok") {             
                Response.Redirect($"~/Master/ItemList?&menu={hddmenu.Value}");
            } else {
                ShowPopAlert("Error", "Delete failed " + Environment.NewLine + POSItemService.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }   

        private void GoByRedirect() {
            string myurl = $"~/Master/ItemDetail?&menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            var h = POSItemService.DocSet.Info;
            if(!ValidData()) {
                return;
            }
        
            var isnew = PrepairDataSave();
            POSItemService.SaveV2();
            if(POSItemService.DocSet.OutputAction.Result == "ok") {//save successufull
                if(isnew) {
                    POSItemService.IsNewDoc = true;
                    POSItemService.GetDocSetByID(h.ItemID);
                    GoByRedirect();
                } else {
                    ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                    LoadData();
                }

            } else {
                ShowPopAlert("Error", POSItemService.DocSet.OutputAction.Message1, false, "");
            }
        }

        #region ItemPrice
        protected void btnSaveItemPrice_Click(object sender, EventArgs e)
        {
            if (!ValidLineData())
            {
                return;
            }
            var h = POSItemService.DocSet.Info;
            ItemPriceInfo price = POSItemService.NewPrice();

            price.ItemID = h.ItemID;
            price.CompanyID = cboCompanyID2.SelectedValue;
            price.CustID = cboShipTo.SelectedValue;
            price.UseLevel = Convert.ToInt32(cboUseLevel.SelectedValue);

            decimal Price = 0;
            decimal.TryParse(txtItemPrice.Text, out Price);
            price.Price = Price; 
            price.PriceTaxCondType = cboPriceTaxcon.SelectedValue;
            price.DateBegin = dtDateBegin.Date;
            price.DateEnd = dtDateEnd.Date;

            var r = POSItemService.AddPrice(price);

            if (r.Result == "fail")
            {
                ShowPopAlert("Error", r.Message1, false, "");
            }
            else
            {

                POSItemService.GetDocSetByID(h.ItemID);

                ResetControl();
                BindGrdItemPrice();
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
            }
        }

        private void ResetControl()
        {
            txtItemPrice.Text = "0";
            dtDateBegin.Value = null;
            dtDateEnd.Value = null;
        }

        private bool ValidLineData()
        {

            if (dtDateBegin.Value == null)
            {
                ShowPopAlert("Error", "ระบุ วันที่เริ่มขาย", false, "");
                return false;
            }

            if (dtDateEnd.Value == null)
            {
                ShowPopAlert("Error", "ระบุ ขายถึงวันที่", false, "");
                return false;
            }

            return true;
        }
        #endregion

        #region ItemInPointRate
        protected void btnItemInPointRate_Click(object sender, EventArgs e)
        {
            if (!ValidItemPointData())
            {
                return;
            }
            var h = POSItemService.DocSet.Info;
            ItemInPointRate pointrate = POSItemService.NewItemPoint();

            pointrate.ItemID = h.ItemID;

            decimal AmtPerPointRate = 0;
            decimal.TryParse(txtAmtPerPointRate.Text, out AmtPerPointRate);
            pointrate.AmtPerPointRate = AmtPerPointRate;

            int ExpireInMont = 0;
            int.TryParse(txtExpireInMont.Text, out ExpireInMont);
            pointrate.ExpireInMont = ExpireInMont;

            pointrate.DateBegin = dtPointDateBegin.Date;
            pointrate.DateEnd = dtPointDateEnd.Date;

            var r = POSItemService.AddItemInPointRate(pointrate);

            if (r.Result == "fail")
            {
                ShowPopAlert("Error", r.Message1, false, "");
            }
            else
            {
                POSItemService.GetDocSetByID(h.ItemID);
                ResetItemPointControl();
                BindGrdItemInPointRate();
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
            }
        }

        private void ResetItemPointControl()
        {
            txtAmtPerPointRate.Text = "0";
            dtPointDateBegin.Value = null;
            dtPointDateEnd.Value = null;
            txtExpireInMont.Text = "0";
        }

        private bool ValidItemPointData()
        {
            if (dtPointDateBegin.Value == null)
            {
                ShowPopAlert("Error", "ระบุ วันที่เริ่มขาย", false, "");
                return false;
            }

            if (dtPointDateEnd.Value == null)
            {
                ShowPopAlert("Error", "ระบุ ขายถึงวันที่", false, "");
                return false;
            }

            return true;
        }
        #endregion


        protected void cboVendor_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT [VendorID],[FullNameTh] FROM ( SELECT [VendorID],[FullNameTh], row_number()over(order by [VendorID] desc) as [rn] FROM [VendorInfo] as t where (([VendorID]+[FullNameTh]) LIKE @filter) and [IsActive]=1) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboVendor_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [VendorID],[FullNameTh] FROM [VendorInfo] WHERE ([VendorID] = @ID) ORDER BY [VendorID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        #region Barcode
     
    
        #endregion

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect($"~/Master/ItemList?&menu={hddmenu.Value}");
        }      

        protected void btnCancel_Click(object sender, EventArgs e) {

        }

        protected void grd_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if(e.CommandArgs.CommandName == "Select") {
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString(); 
                POSItemService.GetDocSetByID(id);
                LoadData();
            }
        }

        protected void grd_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) {

        }

        private void BindGrdItemPrice()
        {
            grdItemPrice.DataSource = POSItemService.DocSet.VItemPrice;
            grdItemPrice.DataBind();
        }

        protected void grdItemPrice_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            var h = POSItemService.DocSet.Info;
            if (e.CommandArgs.CommandName == "Del")
            {
                int id = Convert.ToInt32( e.KeyValue);
               POSItemService.DeleteItemPrice(id);
                if (POSItemService.DocSet.OutputAction.Result=="ok") {
                    POSItemService.GetDocSetByID(h.ItemID);
                    BindGrdItemPrice();

                } else {
                    ShowPopAlert("Error", POSItemService.DocSet.OutputAction.Message1, false, "");
                }
         
            }
        }

        protected void grdItemPrice_DataBinding(object sender, EventArgs e)
        {
            grdItemPrice.DataSource = POSItemService.DocSet.VItemPrice;
        }


        private void BindGrdItemInPointRate()
        {
            grditemInPointRate.DataSource = POSItemService.DocSet.ItemPointRate;
            grditemInPointRate.DataBind();
        }

        protected void grditemInPointRate_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            var h = POSItemService.DocSet.Info;
            if (e.CommandArgs.CommandName == "Del")
            {
                string docid = e.KeyValue.ToString();
                POSItemService.DeleteItemInPointRate(docid);
                if (POSItemService.DocSet.OutputAction.Result == "ok")
                {
                    POSItemService.GetDocSetByID(h.ItemID);
                    BindGrdItemInPointRate();
                }
                else
                {
                    ShowPopAlert("Error", POSItemService.DocSet.OutputAction.Message1, false, "");
                }

            }
        }

        protected void grditemInPointRate_DataBinding(object sender, EventArgs e)
        {
            grditemInPointRate.DataSource = POSItemService.DocSet.ItemPointRate;
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            RefreshGridSelect();
        }

        protected void grd_DataBinding(object sender, EventArgs e) {
            grd.DataSource = POSItemService.DocList.Where(o => o.TypeID != "DISCOUNT").ToList(); 
        }

        protected void btnUploadProfile_Click(object sender, EventArgs e)
        {
            var h = POSItemService.DocSet.Info;
            if (h.ItemID == "")
            {
                return;
            }

            XFilesService.UploadInfo = XFilesService.NewTemplateUploadItemsProfile(h.ItemID, h.Name1, LoginService.LoginInfo.CurrentUser);

            UploadFile.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"Upload/UploadFile";
            Response.Redirect(url);
 
        }

        private void LoadProfile()
        {
            // Get your image from database, I hope it is stored in binary format, so it would return a byte array     
            string img_url = "~/Image/Little/girlprofile.gif";
            var c = POSItemService.DocSet.Info;
            if (c.ItemID == "")
            {
                imgProfile.ImageUrl = img_url;
                return;
            }
            var img = XFilesService.GetFileRefByDocAndTableSource2B64("",c.ItemID,  "ITEMS_PHOTO_PROFILE", true);
            if (!string.IsNullOrEmpty(img))
            {
                img_url = img;
            }

            imgProfile.ImageUrl = img_url;
        }
        protected void btnPostProfile_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }

        protected void btnRemoveProfile_Click(object sender, EventArgs e)
        {
            var c = POSItemService.DocSet.Info;
            if (c.ItemID == "")
            {
                return;
            }
            var r1 = XFilesService.DeleteFileByDocInfo(c.CompanyID, "ITEMS", "ITEMS_PHOTO_PROFILE");
            LoadProfile();
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            POSItemService.DetailPreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/Master/ItemNewDoc?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

     
    }
}