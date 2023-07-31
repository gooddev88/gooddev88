using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
using Robot.Master.Upload;
using Robot.Upload.UploadPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.Data.BL.I_Result;
using static Robot.Data.DataAccess.AddressInfoService;

namespace Robot.Master {
    public partial class CompanyDetail : MyBasePage {
        public static string PreviousDetailPage { get { return HttpContext.Current.Session["companydetail_previouspage"] == null ? "" : (string)HttpContext.Current.Session["companydetail_previouspage"]; } set { HttpContext.Current.Session["companydetail_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
            LoadDropDownDevList();

            popprofile.ShowOnPageLoad = false;
            popFile.ShowOnPageLoad = false;
            popAlert.ShowOnPageLoad = false;

            if (CompanyService.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
            }

            if (!IsPostBack) {
                CloseAlert();
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
            hddid.Value = Request.QueryString["id"];
            hddtaxid.Value = Request.QueryString["taxid"];
        }
        public string SetMenu(object menu_id) {
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null) {
                result = data.IsOpen.ToString();
            }
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
            CompanyService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        protected void btnOK_Click(object sender, EventArgs e) {
        }

        protected void btnCancel_Click(object sender, EventArgs e) {
        }

        private void LoadDropDownList() {
            //cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", false).ToList();
            //cboCompany.DataBind();
            cboBankCode.DataSource = BookBankService.ListBank(true);
            cboBankCode.DataBind();

            cboPromtpayType.DataSource = BookBankService.ListPromptPayType();
            cboPromtpayType.DataBind();

            cboPriceTaxcon.DataSource = MasterTypeService.ListType("SALE RPICE TAX CON", false);
            cboPriceTaxcon.DataBind();
        }

        private void LoadDropDownDevList() {

        }


        private void LoadData() {
            if (hddid.Value != "") {
                CompanyService.GetDocSetByID(hddid.Value,false);
                LoadProfile();
            }

            BindData();
            GridBindLocation();
            GridBindTable();
            BindGridTransactionLog();
            SetActiveControl();
            CheckPermission();

        }

        private void BindData() {

            var h = CompanyService.DocSet.Info;

            txtCompanyID.Text = h.CompanyID;
          //  try { cboCompany.SelectedValue = h.RCompanyID; } catch { cboCompany.SelectedValue = ""; }
            txtName1.Text = h.Name1;
            txtName2.Text = h.Name2;
            txtShortCode.Text = h.ShortCode;
            txtMobile.Text = h.Mobile;
            txtBrnCode.Text = h.BrnCode;
            txtEmail.Text = h.Email;
            txtAddrNo.Text = h.AddrNo;
            txtAddrMoo.Text = h.AddrTanon;
            txtAddrTumbon.Text = h.AddrTumbon;
            txtAddrAmphoe.Text = h.AddrAmphoe;
            txtAddrProvince.Text = h.AddrProvince;
            txtAddrPostCode.Text = h.AddrPostCode;
            txtTaxID.Text = h.TaxID;
            cboBankCode.SelectedValue = h.BankCode;
            txtBookBankID.Text = h.BookBankNo;
            txtBookName.Text = h.BookBankName;
            txtPromptpay.Text = h.PromptPay;
            cboPromtpayType.SelectedValue = h.PromptPayAccType;
            txtQrPaymentData.Text = h.QrPaymentData;
            txtBillAddr1.Text = h.BillAddr1;
            txtBillAddr2.Text = h.BillAddr2;
            ckIsWH.Checked = Convert.ToBoolean(h.IsWH);

            cboPriceTaxcon.SelectedValue = h.PriceTaxCondType;
            ckIsVatRegister.Checked = Convert.ToBoolean(h.IsVatRegister);
            chkActive.Checked = h.IsActive;
            LoadProfile();
        }

        private void SetActiveControl() {
            if (hddid.Value == "") {
                btnNew.Visible = false;
                btnUploadProfile.Visible = false;
                btnDel.Visible = false;
                btnRemoveProfile.Visible = false;
            } else {
                txtCompanyID.Enabled = false;
            }
        }

        private void GridBindLocation()
        {
            grdLocation.DataSource = CompanyService.DocSet.Location;
            grdLocation.DataBind();
        }

        private void GridBindTable()
        {
            grdPOSTable.DataSource = CompanyService.DocSet.Table;
            grdPOSTable.DataBind();
        }

        private void BindGridTransactionLog() {
            grd_transaction_log.DataSource = CompanyService.DocSet.Log;
            grd_transaction_log.DataBind();
        }

        protected void cboComAddr_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboComAddr.Value == null) {
                return;
            }
            int addrId = Convert.ToInt32(cboComAddr.Value);
            var getAddr = AddressService.GetAddressInfoById(addrId);
            SetAddr(getAddr);
        }

        private void SetAddr(vw_ThaiPostAddress addr) {
            txtAddrPostCode.Text = addr.DISTRICT_POSTAL_CODE;
            txtAddrTumbon.Text = addr.DISTRICT_NAME;
            txtAddrAmphoe.Text = addr.BORDER_NAME;
            txtAddrProvince.Text = addr.PROVINCE_NAME;
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent($"~/Master/CompanyList?&menu={hddmenu.Value}");
        }
        private void CheckPermission() {
            var h = CompanyService.DocSet.Info;
            if (h.CompanyID == "") {
                chkActive.Checked = true;
            }
        }

        #region Control move line helper

        protected void btnSave_Click(object sender, EventArgs e) {
            var r = Save();
            if (r.Result == "fail") {
                ShowPopAlert("Error", r.Message1, false, "");
            }
        }
        private I_BasicResult Save() {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var err = ValidData();
                if (err != "") {
                    result.Result = "fail";
                    result.Message1 = err;

                    return result;
                }
                var isnew = PrepairDataSave();

                result = CompanyService.Save(CompanyService.DocSet.Info, "", CompanyService.NeedRunNext);

                if (result.Result == "fail") {//save successufull

                    ShowPopAlert("Error", result.Message1, false, "");
                    return result;
                } else {
                    var customer = CustomerInfoService.ConvertCompany2Customer(CompanyService.DocSet.Info);
                    var rs = CompanyService.SaveCustomer(customer);
                    if (rs.Result == "fail") {
                        ShowPopAlert("Error", result.Message1, false, "");
                        return result;
                    }

                    if (isnew) {//save success and is new doc
                        CompanyService.IsNewDoc = true;
                        string url = "~/Master/CompanyDetail?id=" + CompanyService.DocSet.Info.CompanyID + "&menu=" + hddmenu.Value;
                        Response.Redirect(url);
                    } else {
                        LoadData();
                        ShowPopAlert("Successful", "Save successfull", true, "");
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                result.Message1 = ex.Message;
            }

            return result;
        }

        protected void btnDel_Click(object sender, EventArgs e) {

            CompanyService.Delete();
            if (CompanyService.DocSet.OutputAction.Result == "ok") {
                string myurl = $"~/Master/CompanyList.aspx?id=&menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            } else {
                ShowPopAlert("Error", CompanyService.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }

        private void ShowAlert(string msg, string type) {
            CompanyService.IsNewDoc = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }

        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void cboComAddr_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT [ID],[FULLADDR] FROM ( SELECT [ID],[FULLADDR] , row_number()over(order by [FULLADDR] desc) as [rn]  FROM [vw_ThaiPostAddress] as t where (([FULLADDR]) LIKE @filter)   ) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboComAddr_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [ID],[FULLADDR] FROM [vw_ThaiPostAddress] WHERE ([ID] = @ID)  ";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        #endregion Control move line helper

        private string ValidData() {
            string result = "";
            string mobile = txtMobile.Text.Trim().Replace(".", "").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
            txtMobile.Text = mobile;
            if (mobile == "") {
                result = "ระบุเบอร์";
                ShowPopAlert("Error", result, false, "");
                return result;

            }
            if (txtCompanyID.Text.Trim() == "") {
                result = "ระบุรหัสสาขา";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            if (txtName1.Text.Trim() == "") {
                result = "ระบุชื่อร้าน";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            if (txtName2.Text.Trim() == "")
            {
                result = "ระบุชื่อสาขา";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            if (txtName2.Text.Trim() == "")
            {
                result = "ระบุเลขสาขา";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            //if (txtTaxID.Text.Trim() == "") {
            //    result = "หมายเลขบัตรประชาชน";
            //    ShowPopAlert("Error", result, false, "");
            //    return result;
            //}
            if (!(mobile.Count() >= 9 && mobile.Count() <= 10)) {
                result = "Input wrong format need number in 10 digit ";
                ShowPopAlert("Error", result, false, "");
                return result;
            }

            //if (cboComAddr.Value == null)
            //{
            //    result = "ระบุ ที่อยู่";
            //    ShowPopAlert("Error", result, false, "");
            //    return result;
            //}

            //if (cboComAddr.Value.ToString() == "")
            //{
            //    result = "ระบุ ที่อยู่";
            //    ShowPopAlert("Error", result, false, "");
            //    return result;
            //}
            if (txtShortCode.Text.Trim() == "") {
                result = "ระบุชื่อย่อ";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            var isdup = CompanyService.checkIsDupShortID(txtCompanyID.Text.Trim().ToUpper(), txtShortCode.Text.Trim().ToUpper());
            if (isdup) {
                result = "ชื่อย่อซ้ำในระบบ";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            return result;
        }
        private bool PrepairDataSave() {
            var head = CompanyService.DocSet.Info;
            bool isNew = head.CompanyID==""?true:false;
         
            if (isNew) {              

                head.CompanyID = txtCompanyID.Text.Trim().ToUpper();
             
            }
            head.Name1 = txtName1.Text.Trim();
            head.Name2 = txtName2.Text.Trim();
            head.ShortCode = txtShortCode.Text.Trim().ToUpper();
            head.TaxID = txtTaxID.Text.Trim();
            head.BrnCode = txtBrnCode.Text.Trim();
            //  head.RCompanyID = cboCompany.SelectedValue;
            head.Mobile = txtMobile.Text.Trim();
            head.Email = txtEmail.Text.Trim();
            head.AddrNo = txtAddrNo.Text.Trim();
            head.AddrTanon = txtAddrMoo.Text.Trim();
            head.AddrTumbon = txtAddrTumbon.Text.Trim();
            head.AddrAmphoe = txtAddrAmphoe.Text.Trim();
            head.AddrProvince = txtAddrProvince.Text.Trim();
            head.AddrPostCode = txtAddrPostCode.Text.Trim();
            head.BillAddr1 = txtBillAddr1.Text;
            head.BillAddr2 = txtBillAddr2.Text;
            head.IsWH = ckIsWH.Checked;
            head.BankCode = cboBankCode.SelectedValue;
            head.BookBankNo = txtBookBankID.Text;
            head.BookBankName = txtBookName.Text;
            head.PromptPay = txtPromptpay.Text;
            head.PromptPayAccType = cboPromtpayType.SelectedValue;
            head.QrPaymentData = txtQrPaymentData.Text;
            head.IsVatRegister = ckIsVatRegister.Checked;
            head.PriceTaxCondType = cboPriceTaxcon.SelectedValue;
     
            var addrCompo = new AddressComponent { AddrNo = head.AddrNo, AddrMoo = head.AddrTanon, SubDistrict = head.AddrTumbon, District = head.AddrAmphoe, Province = head.AddrProvince, Country = head.AddrCountry, Zipcode = head.AddrPostCode };
            string comaddr = AddressInfoService.Convert_FullAddr(addrCompo);

            head.AddrFull = comaddr;
            head.IsActive = chkActive.Checked;
            return isNew;
        }

        protected void chkActive_CheckedChanged(object sender, EventArgs e) {
            var active = chkActive.Checked;
            CompanyService.SetActive(active);
            if (CompanyService.DocSet.OutputAction.Result == "fail") {
                ShowPopAlert("Error", CompanyService.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }

        protected void btnUploadProfile_Click(object sender, EventArgs e) {
            var h = CompanyService.DocSet.Info;
            if (h.CompanyID == "") {
                return;
            }

            XFilesService.UploadInfo = XFilesService.NewTemplateUploadCompanyProfile(h.CompanyID, h.Name1, LoginService.LoginInfo.CurrentUser);

            UploadFile.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"Upload/UploadFile";
            Response.Redirect(url);

            //string url = $"~/Upload/UploadPage/UploadImage";
            //popprofile.ContentUrl = url;
            //popprofile.ShowOnPageLoad = true;
        }

        private void LoadProfile() {
            // Get your image from database, I hope it is stored in binary format, so it would return a byte array     
            string img_url = "~/Image/Little/girlprofile.gif";
            var c = CompanyService.DocSet.Info;
            if (c.CompanyID == "") {
                imgProfile.ImageUrl = img_url;
                return;
            }
            var img = XFilesService.GetFileRefByDocAndTableSource2B64("",c.CompanyID,   "COMPANY", true);
            if (!string.IsNullOrEmpty(img)) {
                img_url = img;
            }

            imgProfile.ImageUrl = img_url;
        }
        protected void btnPostProfile_Click(object sender, EventArgs e) {
            LoadProfile();
        }

        protected void btnRemoveProfile_Click(object sender, EventArgs e) {
            var c = CompanyService.DocSet.Info;
            if (c.CompanyID == "") {
                return;
            }
            var r1 = XFilesService.DeleteFileByDocInfo(c.CompanyID, "COMPANY", "COMPANY_PHOTO_PROFILE");
            LoadProfile();
        }

        protected void grd_transaction_log_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            BindGridTransactionLog();
            grd_transaction_log.PageIndex = e.NewPageIndex;
            grd_transaction_log.DataBind();
        }

        protected void btnSave2_Click(object sender, EventArgs e) {
            Save();
        }

        #region Table
        protected void btnAddTable_Click(object sender, EventArgs e)
        {
            if (!ValidTableData())
            {
                return;
            }
            var h = CompanyService.DocSet.Info;
            POS_Table table = CompanyService.NewTable();

            table.ComID = h.CompanyID;

            int sort = 0;
            int.TryParse(txtSort.Text, out sort);
            table.Sort = sort;


            table.TableID = txtTableID.Text.Trim();
            table.TableName = txtTableName.Text.Trim();

            var r = CompanyService.AddTable(table);

            if (r.Result == "fail")
            {
                ShowPopAlert("Error", r.Message1, false, "");
            }
            else
            {
                CompanyService.GetDocSetByID(h.CompanyID,false);
                ResetControl();
                GridBindTable();
            //    ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
            }
        }

        private void ResetControl()
        {
            txtTableID.Text = "";
            txtTableName.Text = "";
            txtSort.Text = "0";
        }

        private bool ValidTableData()
        {
            if (txtTableID.Text == "")
            {
                ShowPopAlert("Error", "ระบุ หมายเลขโต๊ะ", false, "");
                return false;
            }
            if (txtTableName.Text == "")
            {
                ShowPopAlert("Error", "ระบุ ชื่อโต๊ะ", false, "");
                return false;
            }
            if (txtSort.Text == "")
            {
                ShowPopAlert("Error", "ระบุ จัดเรียง", false, "");
                return false;
            }

            return true;
        }

        protected void grdPOSTable_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            var h = CompanyService.DocSet.Info;
            if (e.CommandArgs.CommandName == "Del")
            {
                string docid = e.KeyValue.ToString();
                CompanyService.DeleteTable(docid, h.CompanyID);
                if (CompanyService.DocSet.OutputAction.Result == "ok")
                {
                    CompanyService.GetDocSetByID(h.CompanyID,false);
                    GridBindTable();
                }
                else
                {
                    ShowPopAlert("Error", CompanyService.DocSet.OutputAction.Message1, false, "");
                }

            }
        }

        protected void grdPOSTable_DataBinding(object sender, EventArgs e)
        {
            grdPOSTable.DataSource = CompanyService.DocSet.Table;
        }
        #endregion


        protected void btnNew_Click(object sender, EventArgs e) { 
            CompanyDetail.PreviousDetailPage = HttpContext.Current.Request.Url.PathAndQuery;
            CompanyService.NewTransaction(); 
            Response.Redirect(Request.RawUrl);
        }

    }
}