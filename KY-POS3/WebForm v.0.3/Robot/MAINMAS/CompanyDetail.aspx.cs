using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.Data.BL.I_Result;
using static Robot.Data.DataAccess.AddressInfoService;

namespace Robot.OMASTER {
    public partial class CompanyDetail : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["comdetail_previouspage"] == null ? "" : (string)HttpContext.Current.Session["comdetail_previouspage"]; } set { HttpContext.Current.Session["comdetail_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popprofile.ShowOnPageLoad = false;
            popFile.ShowOnPageLoad = false;
            popAlert.ShowOnPageLoad = false;
            if (CompanyService.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
            }
            SetQueryString();
            LoadDropDownDevList();

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
            cboCompany.DataSource = CompanyService.ListCompany();
            cboCompany.DataBind();
            cboBankCode.DataSource = BankMasterService.MiniSelectList(false);
            cboBankCode.DataBind();
        }

        private void LoadDropDownDevList() {

        }

        private void LoadData() {


            BindData();
            BindGridTransactionLog();
            SetActiveControl();
            CheckPermission();
            LoadProfile();

        }

        private void BindData() {

            var h = CompanyService.DocSet.ComInfo;

            txtCompanyID.Text = h.CompanyID;
            txtNameTh1.Text = h.Name1;
            txtNameTh2.Text = h.Name2;
            if (h.CompanyID == "") {
                if (hddtaxid.Value != "") {
                    txtTaxID.Text = hddtaxid.Value.ToUpper();
                }
            } else {
                txtTaxID.Text = h.TaxID;
                txtCompanyID.Enabled = false;
            }

            try { cboCompany.SelectedValue = h.RCompanyID; } catch { cboCompany.SelectedValue = ""; }
            txtMobile.Text = h.Mobile;
            txtEmail.Text = h.Email;
            txtAddrNo.Text = h.AddrNo;
            txtAddrMoo.Text = h.AddrTanon;
            txtAddrTumbon.Text = h.AddrTumbon;
            txtAddrAmphoe.Text = h.AddrAmphoe;
            txtAddrProvince.Text = h.AddrProvince;
            txtAddrPostCode.Text = h.AddrPostCode;
            chkActive.Checked = h.IsActive;
            txtComCode.Text = h.ComCode;
            txtBillAddr1.Text = h.BillAddr1;
            txtBillAddr2.Text = h.BillAddr2;
            LoadProfile();
        }

        private void SetActiveControl() {
            if (CompanyService.DocSet.ComInfo.CompanyID == "") {
                btnNew.Visible = false;
                btnUploadProfile.Visible = false;
                btnDel.Visible = false;
                btnRemoveProfile.Visible = false;
            } else {
                btnUploadProfile.Visible = true;
                txtCompanyID.Enabled = false;
                btnRemoveProfile.Visible = true;
            }
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

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }
        private void CheckPermission() {
            if (!PermissionService.CanOpen(hddmenu.Value)) {
                Response.Redirect("~/Default");
            }
            var h = CompanyService.DocSet.ComInfo;
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
                if (isnew) {
                    result = CompanyService.Save(CompanyService.DocSet, "insert");
                } else {
                    result = CompanyService.Save(CompanyService.DocSet, "");
                }

                if (result.Result == "fail") {//save successufull     
                    ShowPopAlert("Error", result.Message1, false, "");
                    return result;
                } else {
                    if (isnew) {//save success and is new doc
                        CompanyService.IsNewDoc = true;
                        CompanyService.GetDocSetByID(CompanyService.DocSet.ComInfo.CompanyID);
                        Response.Redirect(Request.RawUrl);
                    } else {
                        LoadData();
                        ShowPopAlert("Successful", "บันทึกสำเร็จ", true, "");
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
                Response.Redirect(PreviousPage);
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
            if (txtComCode.Text.Trim() == "") {
                result = "ระบุรหัสย่อ";
                ShowPopAlert("Error", result, false, "");
                return result;
            }
            if (txtNameTh1.Text.Trim() == "") {
                result = "ระบุชื่อ";
                ShowPopAlert("Error", result, false, "");
                return result;
            }

            if (txtTaxID.Text.Trim() == "") {
                result = "ระบุ เลขผู้เสียภาษี";
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
            var head = CompanyService.DocSet.ComInfo;
            if (head.CompanyID == "") {
                isNew = true;
            }
            CompanyService.NeedRunNext = false;

            if (isNew) { // new customer 
                if (txtCompanyID.Text.Trim() != "") {//user input manual
                    head.CompanyID = txtCompanyID.Text.Trim();
                } else {//auto gen
                    head.CompanyID = IDRuunerService.GetNewID("OCOMPANY", head.CompanyID, false, "th", DateTime.Now.Date)[1];
                    CompanyService.NeedRunNext = true;
                    if (head.CompanyID == "") {
                        ShowPopAlert("Error", "No ducument type found", false, "");
                        return true;
                    }
                }
            }

            head.RCompanyID = cboCompany.SelectedValue;
            head.Name1 = txtNameTh1.Text.Trim();
            head.Name2 = txtNameTh2.Text.Trim();
            head.TaxID = txtTaxID.Text.Trim();
            head.Mobile = txtMobile.Text.Trim();
            head.Email = txtEmail.Text.Trim();
            head.ComCode = txtComCode.Text.ToUpper().Trim();
            head.AddrNo = txtAddrNo.Text.Trim();
            head.AddrTanon = txtAddrMoo.Text.Trim();
            head.AddrTumbon = txtAddrTumbon.Text.Trim();
            head.AddrAmphoe = txtAddrAmphoe.Text.Trim();
            head.AddrProvince = txtAddrProvince.Text.Trim();
            head.AddrPostCode = txtAddrPostCode.Text.Trim();
            head.BillAddr1 = txtBillAddr1.Text;
            head.BillAddr2 = txtBillAddr2.Text;

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
            var h = CompanyService.DocSet.ComInfo;
            if (h.CompanyID == "") {
                return;
            }

            XFilesService.UploadInfo = XFilesService.NewTemplateUploadCompanyProfile(h.CompanyID, h.Name1 + " " + h.Name2, LoginService.LoginInfo.CurrentUser);

            string url = $"~/Upload/UploadPage/UploadImage";
            popprofile.ContentUrl = url;
            popprofile.ShowOnPageLoad = true;
        }

        private void LoadProfile() {
            // Get your image from database, I hope it is stored in binary format, so it would return a byte array     
            string img_url = "~/Image/Little/girlprofile.gif";
            var c = CompanyService.DocSet.ComInfo;
            if (c.CompanyID == "") {
                imgProfile.ImageUrl = img_url;
                return;
            }
            var img = XFilesService.GetFileRefByDocAndTableSource2B64(c.CompanyID, "COMPANY", "COMPANY_PHOTO_PROFILE", true);
            if (!string.IsNullOrEmpty(img)) {
                img_url = img;
            }

            imgProfile.ImageUrl = img_url;
        }
        protected void btnPostProfile_Click(object sender, EventArgs e) {
            LoadProfile();
        }

        protected void btnRemoveProfile_Click(object sender, EventArgs e) {
            var c = CompanyService.DocSet.ComInfo;
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

        protected void btnNew_Click(object sender, EventArgs e) {
            CompanyService.NewTransaction("");
            string url = $"../MAINMAS/CompanyDetail?menu={hddmenu.Value}";
            Response.Redirect(url);
        }

        protected void btnClosePopUpCompany_Click(object sender, EventArgs e) {
            LoadData();
        }
    }
}