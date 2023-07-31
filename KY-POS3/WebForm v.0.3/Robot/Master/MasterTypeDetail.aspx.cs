
using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.Data.BL.I_Result;

namespace Robot.Master
{
    public partial class MasterTypeDetail : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            popprofile.ShowOnPageLoad = false;
            SetQueryString();
            LoadDropDownDevList();
            if(!IsPostBack) {
                LoadDefaultFilter();
                LoadDropDownList();
                LoadData();
                MasterTypeService.AddLineItem("");
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void ShowAlert(string msg, string type) {
            MasterTypeService.IsNewDoc = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            MasterTypeService.IsNewDoc = false;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private void LoadDefaultFilter() {

        }

        private void SetDefaultFilter() {

        }

        private void LoadData() {
            BindData();
            GridBind();            
            SetActiveControl();
        }


        private void ResetControl() {
            var a = MasterTypeService.DocSet.LineActive;
            txtID.Text = ""; 
            txtdesc1.Text = "";
            txtdesc2.Text = "";
            txtSort.Text = a.Sort.ToString();
            chkIsActive.Checked = true;
        }

        private void SetActiveControl() {
            var a = MasterTypeService.DocSet.LineActive;
            lblinfohead.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            //txtID.ReadOnly = a.Description4 != "NEW" ? false : true;
            SetPermission();
        }

        private void GridBind() {
            grd.DataSource = MasterTypeService.DocSet.Line.Where(o => o.Description4 != "NEW").ToList();
            grd.DataBind();
        }


        private void BindData() {
            var a = MasterTypeService.DocSet.LineActive;
            var h = MasterTypeService.DocSet.Head;
            if(a == null) {
                divProfile.Visible = false;
                return;
            }
            divProfile.Visible = true;
            lblMasterID.Text = h.Name;
            txtID.Text = a.ValueTXT;
            txtID.ReadOnly = a.ValueTXT == "" ? false : true;
            txtdesc1.Text = a.Description1;
            txtdesc2.Text = a.Description2;
            txtSort.Text = Convert.ToInt32(a.Sort).ToString("N0");
            chkIsActive.Checked = a.IsActive;

            LoadProfile();
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(MasterTypeService.PreviousDetailPage);
        }
        private void SetPrimaryData() {
            var a = MasterTypeService.DocSet.LineActive;
            a.ValueTXT = txtID.Text.Trim().ToUpper();
            a.Description1 = txtdesc1.Text;
            a.Description2 = txtdesc2.Text;

            int sort = 0;
            int.TryParse(txtSort.Text.Trim(), out sort);
            a.Sort = sort;
            a.IsActive = chkIsActive.Checked;
            MasterTypeService.CalTax();
        }

        protected void btnOK_Click(object sender, EventArgs e) {
            SetPrimaryData();
            if(!ValidData()) {
                return;
            }
            var a = MasterTypeService.DocSet.LineActive;
            a.Description4 = "";

            MasterTypeService.Save("update");
            if(MasterTypeService.DocSet.OutputAction.Result == "ok") {

                LoadData();
                RefreshGrid();
                MasterTypeService.AddLineItem("");
                ShowAlert("บันทึกสำเร็จ", "Success");
            } else {
                ShowAlert(MasterTypeService.DocSet.OutputAction.Message1, "Error");
            }
        }

        private void RefreshGrid() {
            var h = MasterTypeService.DocSet.Head;
            MasterTypeService.ListLine2DocSetLine();
            GridBind();
        }

        private void SetPermission() {

        }

        private bool ValidData() {
            if(txtID.Text.Trim() == "") {
                ShowAlert("ระบุ รหัสตัวเลือก", "Error");
                return false;
            }
            return true;
        }

        private void GoByRedirect(string docid) {
            string url = $"../Master/MasterTypeDetail?id={docid}&menu={hddmenu.Value}";
            Response.Redirect(url);
        }
        private void LoadDropDownDevList() {

        }

        private void LoadDropDownList() {

        }


        protected void grd_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if(e.CommandArgs.CommandName == "editrow") {
                ASPxGridView grid = (ASPxGridView)sender;
                string valuetxt = e.KeyValue.ToString();
                MasterTypeService.DocSet.LineActive = MasterTypeService.DocSet.Line.Where(o => o.ValueTXT == valuetxt).FirstOrDefault();
                BindData();
            }
        }

        protected void grd_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) {

        }

        protected void grd_DataBinding(object sender, EventArgs e) {
            grd.DataSource = MasterTypeService.DocSet.Line.Where(o => o.Description4 != "NEW").ToList(); ;
        }


        protected void btnNew_Click(object sender, EventArgs e) {
            MasterTypeService.AddLineItem("");
            BindData();
        }

        protected void btnUploadProfile_Click(object sender, EventArgs e)
        {
            var h = MasterTypeService.DocSet.LineActive;
            if (h.ValueTXT == "")
            {
                return;
            }
            XFilesService.UploadInfo = XFilesService.NewTemplateUploadMasterTypeProfile(h.MasterTypeID+h.ValueTXT, h.Description1, LoginService.LoginInfo.CurrentUser);

            string url = $"~/Upload/UploadPage/UploadImage";
            popprofile.ContentUrl = url;
            popprofile.ShowOnPageLoad = true;
        }

        private void LoadProfile()
        {
            // Get your image from database, I hope it is stored in binary format, so it would return a byte array     
            string img_url = "~/Image/Little/girlprofile.gif";
            var active = MasterTypeService.DocSet.LineActive;
            if (active.ValueTXT == "")
            {
                imgProfile.ImageUrl = img_url;
                return;
            }
            var img = XFilesService.GetFileRefByDocAndTableSource2B64(active.MasterTypeID+active.ValueTXT, "MASTERTYPE", "MASTERTYPE_PHOTO_PROFILE", true);
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
            var active = MasterTypeService.DocSet.LineActive;
            if (active.ValueTXT == "")
            {
                return;
            }
            var r1 = XFilesService.DeleteFileByDocInfo(active.MasterTypeID+active.ValueTXT, "MASTERTYPE", "MASTERTYPE_PHOTO_PROFILE");
            LoadProfile();
        }

        protected void btnClosePopUpMasterType_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }

}