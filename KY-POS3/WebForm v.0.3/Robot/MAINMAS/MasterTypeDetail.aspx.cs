
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

namespace Robot.MAINMAS {
    public partial class MasterTypeDetail : MyBasePage {
        public static string PreviousPage { get { return (string)HttpContext.Current.Session["mastertypedetail_previouspage"]; } set { HttpContext.Current.Session["mastertypedetail_previouspage"] = value; } }
        public MasterTypeHead Head { get; set; }
        public static MasterTypeHead MasterHead { get { return (MasterTypeHead)HttpContext.Current.Session["masterhead"]; } set { HttpContext.Current.Session["masterhead"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popprofile.ShowOnPageLoad = false;
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {
                LoadDefaultFilter();
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void ShowAlert(string msg, string type) {
            MasterTypeService.IsNewDoc = false;
            lblMsg.Text = msg;
            msg = "";
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
      
            SetPermission();
        }

        private void GridBind() {
            grd.DataSource = MasterTypeService.DocSet.Line.ToList();
            grd.DataBind();
        }


        private void BindData() {
            var a = MasterTypeService.DocSet.LineActive;
            lblinfohead.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            if (a == null) {
                divProfile.Visible = false;
                return;
            }
            divProfile.Visible = true;
            lblMasterID.Text = MasterHead.MasterTypeID;
            txtID.Text = a.ValueTXT;
            txtID.ReadOnly = a.ValueTXT == "" ? false : true;
            txtdesc1.Text = a.Description1;
            txtdesc2.Text = a.Description2;
            txtSort.Text = Convert.ToInt32(a.Sort).ToString("N0");
            chkIsActive.Checked = a.IsActive;

            LoadProfile();
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }
        private void SetPrimaryData() {
            var a = MasterTypeService.DocSet.LineActive;
            a.ValueTXT = txtID.Text.Trim().ToUpper();
            a.Description1 = txtdesc1.Text;
            a.Description2 = txtdesc2.Text;
            a.MasterTypeID = MasterHead.MasterTypeID;
            int sort = 0;
            int.TryParse(txtSort.Text.Trim(), out sort);
            a.Sort = sort;
            a.IsActive = chkIsActive.Checked;

        }

        protected void btnOK_Click(object sender, EventArgs e) {
            SetPrimaryData();
            if (!ValidData()) {
                return;
            }
            var a = MasterTypeService.DocSet.LineActive;
            a.Description4 = "";
            var r = MasterTypeService.Save(MasterHead);
            if (r.Result == "ok") {
         
                RefreshGrid();
                //MasterTypeInfoV2Service.GetDocSetByID(a.MasterTypeID);            
                LoadData();
             
                MasterTypeService.AddLineItem("",MasterHead.UseFor);
                ShowAlert("บันทึกสำเร็จ", "Success");
            } else {
                ShowAlert(r.Message1, "Error");
            }
        }

        private void RefreshGrid() {

            MasterTypeService.GetDocSetByID(MasterHead.MasterTypeID);
            GridBind();
        }

        private void SetPermission() {

        }

        private bool ValidData() {
            if (txtID.Text.Trim() == "") {
                ShowAlert("ระบุ รหัสตัวเลือก", "Error");
                return false;
            }
            return true;
        }


        private void LoadDropDownDevList() {

        }

        private void LoadDropDownList() {

        }


        protected void grd_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "editrow") {
                ASPxGridView grid = (ASPxGridView)sender;
                string valuetxt = e.KeyValue.ToString();
                MasterTypeService.DocSet.LineActive = MasterTypeService.DocSet.Line.Where(o => o.ValueTXT == valuetxt).FirstOrDefault();
                BindData();
            }
        }

        protected void grd_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) {

        }

        protected void grd_DataBinding(object sender, EventArgs e) {
            grd.DataSource = MasterTypeService.DocSet.Line. ToList(); ;
        }


        protected void btnNew_Click(object sender, EventArgs e) {
            MasterTypeService.AddLineItem("", MasterHead.UseFor);
            BindData();
        }

        protected void btnUploadProfile_Click(object sender, EventArgs e) {
            var h = MasterTypeService.DocSet.LineActive;
            if (h.ValueTXT == "") {
                return;
            }
            XFilesService.UploadInfo = XFilesService.NewTemplateUploadMasterTypeProfile(h.MasterTypeID + h.ValueTXT, h.Description1, LoginService.LoginInfo.CurrentUser);

            string url = $"~/Upload/UploadPage/UploadImage";
            popprofile.ContentUrl = url;
            popprofile.ShowOnPageLoad = true;
        }

        private void LoadProfile() {
            // Get your image from database, I hope it is stored in binary format, so it would return a byte array     
            string img_url = "~/Image/Little/girlprofile.gif";
            var active = MasterTypeService.DocSet.LineActive;
            if (active.ValueTXT == "") {
                imgProfile.ImageUrl = img_url;
                return;
            }
            var img = XFilesService.GetFileRefByDocAndTableSource2B64("",active.MasterTypeID + active.ValueTXT, "MASTERTYPE_PHOTO_PROFILE", true);
            if (!string.IsNullOrEmpty(img)) {
                img_url = img;
            }

            imgProfile.ImageUrl = img_url;
        }
        protected void btnPostProfile_Click(object sender, EventArgs e) {
            LoadProfile();
        }

        protected void btnRemoveProfile_Click(object sender, EventArgs e) {
            var active = MasterTypeService.DocSet.LineActive;
            if (active.ValueTXT == "") {
                return;
            }
            var r1 = XFilesService.DeleteFileByDocInfo(active.MasterTypeID + active.ValueTXT, "MASTERTYPE", "MASTERTYPE_PHOTO_PROFILE");
            LoadProfile();
        }

    }

}