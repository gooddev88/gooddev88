using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
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

namespace Robot.Master
{
    public partial class VendorDetail : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
            LoadDropDownDevList();

            popprofile.ShowOnPageLoad = false;
            popFile.ShowOnPageLoad = false;
            popAlert.ShowOnPageLoad = false;

            if (VendorInfoV2Service.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
            }

            if (!IsPostBack) {
                CloseAlert();
                if (VendorInfoV2Service.DocSet == null)
                {
                    VendorInfoV2Service.NewTransaction();
                }                
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
            VendorInfoV2Service.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        protected void btnOK_Click(object sender, EventArgs e) {
        }

        protected void btnCancel_Click(object sender, EventArgs e) {
        }

        private void LoadDropDownList() {
            cboTitleTh.DataSource = MasterTypeService.ListType("TITLE_TH", false);
            cboTitleTh.DataBind();

            cboBankCode.DataSource = BookBankService.ListBank(false);
            cboBankCode.DataBind();
        }

        private void LoadDropDownDevList() {

            cboCreditTerm.DataSource = CreditTermInfoService.MiniSelectList(false, "AR");
            cboCreditTerm.DataBind();

            cboCurrency.DataSource = CurrencyInfoService.MiniSelectList(false);
            cboCurrency.DataBind();
            cboVatType.DataSource = TaxInfoService.MiniSelectListV2("SALE", GetSelectCurrency(), false);
            cboVatType.DataBind();

        }
        private string GetSelectCurrency() {
            string result = "THB";
            if (cboCurrency.Value != null) {
                result = cboCurrency.Value.ToString();
            }
            return result;
        }
        private void ClearSession() {

        }


        private void LoadData() {
            if (hddid.Value != "") {
                VendorInfoV2Service.GetDocSetByID(hddid.Value, false);
                LoadProfile();
            }  

            BindData();
            BindGridTransactionLog();
            SetActiveControl();
            CheckPermission();

        }

        private void BindData() {
        
            var h = VendorInfoV2Service.DocSet.Info;
            
            txtVenderID.Text= h.VendorID;

            try { cboTitleTh.SelectedValue = h.TitleTh; } catch { }

            txtNameTh1.Text = h.NameTh1;
            txtNameTh2.Text = h.NameTh2;
            if (h.VendorID == "") {
                if (hddtaxid.Value != "") {
                    txtTaxID.Text = hddtaxid.Value.ToUpper();
                }
            } else {
                txtTaxID.Text = h.TaxID;
            }

            cboBankCode.SelectedValue = h.BankCode;
            txtBookBankID.Text = h.BankID;

            txtMobile.Text = h.Mobile;
            txtEmail.Text = h.Email;
            txtAddrNo.Text = h.AddrNo;
            txtAddrMoo.Text = h.AddrMoo;
            txtAddrTumbon.Text = h.AddrTumbon;
            txtAddrAmphoe.Text = h.AddrAmphoe;
            txtAddrProvince.Text = h.AddrProvince;
            txtAddrPostCode.Text = h.AddrPostCode;
            chkActive.Checked = h.IsActive;
            chkIsParner.Checked = Convert.ToBoolean( h.IsPartner);
            cboCreditTerm.Value = h.PaymentTermID;

            //cboCurrency.Value = h.Currency;
            cboVatType.Value = h.VatTypeID;
            txtCreditLimit.Text = h.CreditLimit.ToString("n2");
            txtBillAddr1.Text = h.BillAddr1;
            txtBillAddr2.Text = h.BillAddr2;
            LoadProfile();
        }

        private void SetActiveControl() {
            if (hddid.Value == "") {
                btnNew.Visible = false;
                btnUploadProfile.Visible = false;
                btnDel.Visible = false;
                btnRemoveProfile.Visible = false;
            } else {
                txtVenderID.Enabled = false;
            }
        }

        private void BindGridTransactionLog()
        {
            grd_transaction_log.DataSource = VendorInfoV2Service.DocSet.Log;
            grd_transaction_log.DataBind();
        }

        protected void cboVenAddr_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboVenAddr.Value == null) {
                return;
            }
            int addrId = Convert.ToInt32(cboVenAddr.Value);
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
            Response.RedirectPermanent(VendorInfoV2Service.PreviouseDetailPage);
        }
        private void CheckPermission() {
            var h = VendorInfoV2Service.DocSet.Info;
            if (h.VendorID == "") {
                chkActive.Checked = true;
            }
        }

        #region Control move line helper

        protected void btnSave_Click(object sender, EventArgs e) {
         var r=   Save();
            if (r.Result=="fail") {
                ShowPopAlert("Error",r. Message1, false, "");
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
                result =    VendorInfoV2Service.Save(VendorInfoV2Service.DocSet.Info, LoginService.LoginInfo.CurrentCompany.CompanyID, VendorInfoV2Service.NeedRunNext);
               
                if (result.Result == "fail") {//save successufull
            
                    ShowPopAlert("Error", result.Message1, false, "");
                    return result;
                } else {
                    if (isnew) {//save success and is new doc
                        VendorInfoV2Service.IsNewDoc = true;
                        string url = "~/Master/VendorDetail?id=" + VendorInfoV2Service.DocSet.Info.VendorID + "&menu=" + hddmenu.Value;
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

        protected void cboCurrency_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboCurrency.Value == null) {
                return;
            }
            if (cboCurrency.Value.ToString() == "") {
                return;
            }

            cboVatType.DataSource = TaxInfoService.MiniSelectListV2("SALE", GetSelectCurrency(), false);
            cboVatType.DataBind();

        }
        protected void btnDel_Click(object sender, EventArgs e) {

            VendorInfoV2Service.Delete();
            if (VendorInfoV2Service.DocSet.OutputAction.Result == "ok") {
                string myurl = $"~/Master/VendorList.aspx?id=&menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            } else {
                ShowPopAlert("Error", VendorInfoV2Service.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }

        private void ShowAlert(string msg, string type) {
            VendorInfoV2Service.IsNewDoc = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }

        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void cboVenAddr_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
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

        protected void cboVenAddr_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
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
            if (txtNameTh1.Text.Trim() == "") {
                result = "ระบุชื่อ";
                ShowPopAlert("Error", "ระบุชื่อ", false, "");
                return result;
            }
            if (txtNameTh2.Text.Trim() == "") {
                result = "ระบุนามสกุล";
                ShowPopAlert("Error", "ระบุนามสกุล", false, "");
                return result;
            }
            if (txtTaxID.Text.Trim() == "") {
                result = "หมายเลขบัตรประชาชน";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            if (!(mobile.Count() >= 9 && mobile.Count() <= 10)) {
                result = "Input wrong format need number in 10 digit ";
                ShowPopAlert("Error", result, false, "");
                return result;

            }

            return result;
        }
        private bool PrepairDataSave() {
            bool isNew = false;
            var head = VendorInfoV2Service.DocSet.Info;
            if (hddid.Value == "") {
                isNew = true;
            }
            VendorInfoV2Service.NeedRunNext = false;

            if (isNew) { // new customer 
                if (txtVenderID.Text.Trim() != "") {//user input manual
                    head.VendorID = txtVenderID.Text.Trim();
                } else {//auto gen
                    if (txtTaxID.Text.Trim() != "") {//สำหรับ hospy
                        head.VendorID = txtTaxID.Text.Trim().ToUpper();
                    } else {
                        head.VendorID = IDRuunerService.GetNewID("VENDOR_INFO", head.CompanyID, false, "th", Convert.ToDateTime(head.CreatedDate))[1];

                        VendorInfoV2Service.NeedRunNext = true;
                        if (head.VendorID == "") {
                            ShowPopAlert("Error", "No ducument type found", false, "");
                            return true;
                        }
                    }

                }
                head.CreatedBy = LoginService.LoginInfo.CurrentUser;
                head.CreatedDate = DateTime.Now;
            } else {
                head.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                head.ModifiedDate = DateTime.Now;

            }
            
            head.NameTh1 = txtNameTh1.Text.Trim();
            head.NameTh2 = txtNameTh2.Text.Trim();
            head.TaxID = txtTaxID.Text.Trim();
            head.Mobile = txtMobile.Text.Trim();
            head.Email = txtEmail.Text.Trim();

            head.TitleTh = cboTitleTh.SelectedValue;

            head.AddrNo = txtAddrNo.Text.Trim();
            head.AddrMoo = txtAddrMoo.Text.Trim();
            head.AddrTumbon = txtAddrTumbon.Text.Trim();
            head.AddrAmphoe = txtAddrAmphoe.Text.Trim();
            head.AddrProvince = txtAddrProvince.Text.Trim();
            head.AddrPostCode = txtAddrPostCode.Text.Trim();

            head.BillAddr1 = txtBillAddr1.Text;
            head.BillAddr2 = txtBillAddr2.Text;
            head.VatTypeID = cboVatType.Value != null ? cboVatType.Value.ToString() : head.VatTypeID = "";
            head.PaymentTermID = cboCreditTerm.Value != null ? cboCreditTerm.Value.ToString() : head.PaymentTermID = "";

            head.BankCode = cboBankCode.SelectedValue;
            head.BankID = txtBookBankID.Text;

            decimal creditlimit = 0;
            decimal.TryParse(txtCreditLimit.Text, out creditlimit);
            head.CreditLimit = creditlimit;

            try { head.CreditLimit = Convert.ToDecimal(txtCreditLimit.Text); } catch { head.CreditLimit = 0; }

            var addrCompo = new AddressComponent { AddrNo = head.AddrNo, AddrMoo = head.AddrMoo, SubDistrict = head.AddrTumbon, District = head.AddrAmphoe, Province = head.AddrProvince, Country = head.AddrCountry, Zipcode = head.AddrPostCode };
            string comaddr = AddressInfoService.Convert_FullAddr(addrCompo);

            head.AddrFull = comaddr;
            head.IsActive = chkActive.Checked;
            head.IsPartner = chkIsParner.Checked;
            return isNew;
        }

        protected void chkActive_CheckedChanged(object sender, EventArgs e) {
            var active = chkActive.Checked;
            VendorInfoV2Service.SetActive(active);
            if (VendorInfoV2Service.DocSet.OutputAction.Result == "fail") {
                ShowPopAlert("Error", VendorInfoV2Service.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }
    
        protected void btnUploadProfile_Click(object sender, EventArgs e) {
            var h = VendorInfoV2Service.DocSet.Info;
            if (h.VendorID == "") {
                return;
            }

            XFilesService.UploadInfo = XFilesService.NewTemplateUploadVendorProfile(h.VendorID, h.FullNameTh, LoginService.LoginInfo.CurrentUser);
         

            string url = $"../Upload/UploadPage/UploadImage";
            popprofile.ContentUrl = url;
            popprofile.ShowOnPageLoad = true;
        }

        private void LoadProfile() {
            // Get your image from database, I hope it is stored in binary format, so it would return a byte array     
            string img_url = "~/Image/Little/girlprofile.gif";
            var c = VendorInfoV2Service.DocSet.Info;
            if (c.VendorID == "") {
                imgProfile.ImageUrl = img_url;
                return;
            }
            var img = XFilesService.GetFileRefByDocAndTableSource2B64("",c.VendorID,   "VENDORS_PHOTO_PROFILE");
            if (!string.IsNullOrEmpty(img)) {
                img_url = img;
            }

            imgProfile.ImageUrl = img_url;
        }
        protected void btnPostProfile_Click(object sender, EventArgs e) {
            LoadProfile();
        }

        protected void btnRemoveProfile_Click(object sender, EventArgs e) {
            var c = VendorInfoV2Service.DocSet.Info;
            if (c.VendorID=="") {
                return;
            }
            var r1 = XFilesService.DeleteFileByDocInfo(c.VendorID, "VENDORS", "VENDORS_PHOTO_PROFILE");
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

        protected void btnNew_Click(object sender, EventArgs e) {
            string url = $"~/Master/VendorDetail?id=&menu={hddmenu.Value}";
            Response.Redirect(url);
        }
    
    }
}