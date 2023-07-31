using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Robot.Data.DataAccess;

namespace Robot.Master
{
    public partial class MyUserGroupDetail : MyBasePage {
        #region Global var
        public static string XUsergroupID { get { return HttpContext.Current.Session["xusergroupID"] == null ? "" : (string)HttpContext.Current.Session["xusergroupID"]; } set { HttpContext.Current.Session["xusergroupID"] = value; } }
        public static string PreviousPageX { get { return HttpContext.Current.Session["masusergroup_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masusergroup_previouspage"]; } set { HttpContext.Current.Session["masusergroup_previouspage"] = value; } }

        #endregion
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString2HiddenFiled();

            LoadDevDropDownList();
            popAlert.ShowOnPageLoad = false;
            if (UserGroupInfoV2Service.IsNewDoc) {
                ShowPopAlert("Result", "Save successfull " + XUsergroupID, true, "");
            }
            if (!IsPostBack) {
                //UserGroupInfoV2Service.NewTransaction();
                CloseAlert();

                LoadData();
            }
        }
        private void SetQueryString2HiddenFiled() {
            //hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void SetActiveControl() {
            var h = UserGroupInfoV2Service.DocSet.Info;
            lblTopic.Text = PermissionService.GetMenuInfo("9003").Name;
            if (XUsergroupID == "") {
                txtUserGroupId.ReadOnly = false;
                divPermission.Visible = false; 
                chkIsActive.Checked = true;
                txtSort.Text = h.Sort.ToString();
            } else {
                divPermission.Visible = true;
            }  
        }

        private void LoadDevDropDownList() {

        }
        private void BindDropDownDocStep()
        {
            var stepId = UserGroupInfoV2Service.DocSet.ShowInDocStep.Select(o => (string)o.StepID).Distinct().ToList();
            var query = DocStepService.MiniSelectList(false).Where(o => !stepId.Contains(o.StepID)).ToList();
            cboDocStep.DataSource = query;
            cboDocStep.DataBind();
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
            UserGroupInfoV2Service.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }


        private void LoadData() {

            if (XUsergroupID != "") {
                UserGroupInfoV2Service.GetDocSetByID(XUsergroupID);
            }  
              

                var h = UserGroupInfoV2Service.DocSet.Info;
                txtUserGroupId.Text = XUsergroupID;
                txtGroupName.Text = h.GroupName;
                txtSort.Text = h.Sort.ToString();
                chkIsActive.Checked = h.IsActive;
                SetActiveControl();
                BindGrdPermisson();
                BindGrdInBoard();
                BindGrdInCompany();
                BindDropDownDocStep();
        
        }

        private void BindGrdPermisson() {
            grdPermission.DataSource = UserGroupInfoV2Service.DocSet.ShowPermission;
            grdPermission.DataBind();
        }
        private void BindGrdInBoard() {
            grdDashBoard.DataSource = UserGroupInfoV2Service.DocSet.ShowInBoard;
            grdDashBoard.DataBind();
        }

        private void BindGrdInCompany() {
            grdUserInCompany.DataSource = UserGroupInfoV2Service.DocSet.ShowInCompany;
            grdUserInCompany.DataBind();
        }

        private void BindGrdInDocStep()
        {
            grdGroupInDoc.DataSource = UserGroupInfoV2Service.DocSet.ShowInDocStep;
            grdGroupInDoc.DataBind();
        }
        private void SetInMenu() {

            UserGroupInfoV2Service.DocSet.SavePermission = new List<UserGroupPermission>();
            foreach (GridViewRow row in grdPermission.Rows) {
                UserGroupPermission per = new UserGroupPermission();
                per.UserGroupID = (row.FindControl("lblUserGroupId") as Label).Text;
                per.MenuID = (row.FindControl("lblMenuId") as Label).Text;
                per.IsOpen = (row.FindControl("chkIsOpen") as CheckBox).Checked;
                per.IsCreate = (row.FindControl("chkIsCreate") as CheckBox).Checked;
                per.IsEdit = (row.FindControl("chkIsEdit") as CheckBox).Checked;
                per.IsDelete = (row.FindControl("chkIsDelete") as CheckBox).Checked;
                per.IsPrint = (row.FindControl("chkIsPrint") as CheckBox).Checked;
                UserGroupInfoV2Service.DocSet.SavePermission.Add(per);
            }
        }

        private void SetInCompany() {
            UserGroupInfoV2Service.DocSet.SaveInCompany = new List<UserGroupInCompany>();
            foreach (GridViewRow row in grdUserInCompany.Rows) {
                if ((row.FindControl("chkIsInCompany") as CheckBox).Checked) {
                    UserGroupInCompany com = new UserGroupInCompany();
                    com.UserGroupID = XUsergroupID;
                    com.CompanyID = (row.FindControl("lblCompanyID") as Label).Text;
                    com.IsActive = true;
                    UserGroupInfoV2Service.DocSet.SaveInCompany.Add(com);
                }
            }

        }
        private void SetInBoard() {
            UserGroupInfoV2Service.DocSet.SaveInBoard = new List<UserGroupInBoard>();

            foreach (GridViewRow row in grdDashBoard.Rows) {
                if ((row.FindControl("chkIsInBoard") as CheckBox).Checked) {
                    UserGroupInBoard b = new UserGroupInBoard();
                    b.UserGroupID = XUsergroupID;
                    b.DashBoardID = (row.FindControl("lblDashBoardID") as Label).Text;
                    UserGroupInfoV2Service.DocSet.SaveInBoard.Add(b);

                }
            }
        }


        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private bool PrepairDataSave() {
            bool isNew = XUsergroupID == "" ? true : false;
            var h = UserGroupInfoV2Service.DocSet.Info;
            h.UserGroupID = txtUserGroupId.Text.Trim().ToUpper();
            h.GroupName = txtGroupName.Text;
            int sort = 0;
            int.TryParse(txtSort.Text, out sort);
            h.Sort = sort;
            h.IsActive = chkIsActive.Checked;
            SetInBoard();
            SetInCompany();
            SetInMenu();
            return isNew;
        }

        private bool ValidData() {
            bool result = true;

            if (txtUserGroupId.Text.Trim() == "") {
                ShowPopAlert("Error", "Input user group ID!!", false, "");
                return false;

            }
            if (txtGroupName.Text == "") {
                ShowPopAlert("Error", "Input User group name!!", false, "");
                return false;
            }
            if (XUsergroupID == "") {
                UserGroupInfoV2Service.DocSet.Info.Sort = Convert.ToInt32(txtSort.Text);
                if (UserGroupInfoV2Service.CheckDupSort().Result == "fail") {
                    ShowPopAlert("Error", UserGroupInfoV2Service.CheckDupSort().Message1, false, "");
                    return false;
                }
            }          

            return result;
        }
        protected void btnSave_Click(object sender, EventArgs e) {
            try {
                if (!ValidData()) {
                    return;
                }
                bool isNew = PrepairDataSave();

              

                if (isNew) {
                    UserGroupInfoV2Service.Save("insert");
                } else {
                    UserGroupInfoV2Service.Save("update");
                }
                var h = UserGroupInfoV2Service.DocSet.Info;
                if (UserGroupInfoV2Service.DocSet.OutputAction.Result == "fail") {
                    ShowPopAlert("Error", UserGroupInfoV2Service.DocSet.OutputAction.Message1, false, "");
                } else {//save success  
                    if (isNew) {
                        UserGroupInfoV2Service.IsNewDoc = true;
                        XUsergroupID = h.UserGroupID;
                        string myurl = $"../Master/MyUserGroupDetail?menu={hddmenu.Value}";
                        Response.RedirectPermanent(myurl);
                    } else {
                        ShowPopAlert("Success", "Save Succesfull", true, "");
                        LoadData();
                    }
                }
            } catch (Exception ex) {
                ShowPopAlert("Error", ex.Message, false, "");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            XUsergroupID = "";
          
            string myurl = $"../Master/MyUserGroupDetail?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void grdPermission_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {

                Label lblIsOpen = e.Row.FindControl("lblIsOpen") as Label;
                Label lblNeedOpen = e.Row.FindControl("lblNeedOpen") as Label;
                CheckBox chkIsOpen = e.Row.FindControl("chkIsOpen") as CheckBox;
                if (lblNeedOpen.Text == "False") {
                    chkIsOpen.Enabled = false;
                }
                if (lblIsOpen.Text == "1") {
                    chkIsOpen.Checked = true;
                }

                Label lblIsCreate = e.Row.FindControl("lblIsCreate") as Label;
                Label lblNeedCreate = e.Row.FindControl("lblNeedCreate") as Label;
                CheckBox chkIsCreate = e.Row.FindControl("chkIsCreate") as CheckBox;
                if (lblNeedCreate.Text == "False") {
                    chkIsCreate.Enabled = false;
                }
                if (lblIsCreate.Text == "1") {
                    chkIsCreate.Checked = true;
                }

                Label lblIsEdit = e.Row.FindControl("lblIsEdit") as Label;
                Label lblNeedEdit = e.Row.FindControl("lblNeedEdit") as Label;
                CheckBox chkIsEdit = e.Row.FindControl("chkIsEdit") as CheckBox;
                if (lblNeedEdit.Text == "False") {
                    chkIsEdit.Enabled = false;
                }
                if (lblIsEdit.Text == "1") {
                    chkIsEdit.Checked = true;
                }

                Label lblIsDelete = e.Row.FindControl("lblIsDelete") as Label;
                Label lblNeedDelete = e.Row.FindControl("lblNeedDelete") as Label;
                CheckBox chkIsDelete = e.Row.FindControl("chkIsDelete") as CheckBox;
                if (lblNeedDelete.Text == "False") {
                    chkIsDelete.Enabled = false;
                }
                if (lblIsDelete.Text == "1") {
                    chkIsDelete.Checked = true;
                }

                Label lblIsPrint = e.Row.FindControl("lblIsPrint") as Label;
                Label lblNeedPrint = e.Row.FindControl("lblNeedPrint") as Label;
                CheckBox chkIsPrint = e.Row.FindControl("chkIsPrint") as CheckBox;
                if (lblNeedPrint.Text == "False") {
                    chkIsPrint.Enabled = false;
                }
                if (lblIsPrint.Text == "1") {
                    chkIsPrint.Checked = true;
                }
            }
        }

        protected void grdUserInCompany_RowDataBound(object sender, GridViewRowEventArgs e) {

        }
        protected void grdDashBoard_RowDataBound(object sender, GridViewRowEventArgs e) {

        }
        protected void grdUserInGroup_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                bool IsInGroup = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsInGroup"));
                CheckBox chkGroupSelect = e.Row.FindControl("chkGroupSelect") as CheckBox;
                if (IsInGroup) {
                    chkGroupSelect.Checked = true;
                } else {
                    chkGroupSelect.Checked = false;
                }
            }
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousPageX);
        }

        protected void chkselectCompany_CheckedChanged(object sender, EventArgs e) {
            UserGroupInfoV2Service.BulkCheckCompany(chkselectCompany.Checked);
            BindGrdInCompany();
        }

        protected void btnCopyRole_Click(object sender, EventArgs e) {
            CopyPermission.FromRoleID = XUsergroupID;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "group";
            CopyPermission.CopyType = "menu";
            string myurl = $"../Master/CopyPermission";
            Response.Redirect(myurl);
        }
    protected void btnCopyCom_Click(object sender, EventArgs e) {
            CopyPermission.FromRoleID = XUsergroupID;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "group";
            CopyPermission.CopyType = "company";
            string myurl = $"../Master/CopyPermission";
            Response.Redirect(myurl);
        }


        protected void btnAddDocStep_Click(object sender, EventArgs e)
        {
            var chk_User_InDoc = UserGroupInfoV2Service.CheckUserInDoc(cboDocStep.SelectedValue, XUsergroupID);
            if (chk_User_InDoc != null)
            {
                ShowPopAlert("Error", "Duplicate Doc step", false, "");
                return;
            }
            var stepInfo = DocStepService.GetDocStepInfo(cboDocStep.SelectedValue);
            if (stepInfo == null)
            {
                ShowPopAlert("Error", "Missing Doc step", false, "");
                return;
            }
            var h = UserGroupInfoV2Service.DocSet.Info;
            var n = UserGroupInfoV2Service.NewDocStep();
            n.UserGroupID = h.UserGroupID;
            n.StepID = cboDocStep.SelectedValue;
            n.StepName = stepInfo.StepName;

            var r = UserGroupInfoV2Service.SaveDocStep(n);
            if (r.Result == "ok")
            {
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                UserGroupInfoV2Service.ListDocStep(h.UserGroupID);
                ResetDocStepControl();
                BindGrdInDocStep();
            }
            else
            {
                ShowPopAlert("Error", r.Message1, false, "");
            }
        }
        protected void grdGroupInDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                var h = UserGroupInfoV2Service.DocSet.Info;
                int id = Convert.ToInt32(e.CommandArgument);
                var r = UserGroupInfoV2Service.DeleteDocStep(id);

                if (r.Result == "ok")
                {
                    ShowPopAlert("Success", "Delete successfull", false, "");

                    UserGroupInfoV2Service.ListDocStep(h.UserGroupID);
                    ResetDocStepControl();
                    BindGrdInDocStep();
                }
                else
                {
                    ShowPopAlert("Error", r.Message1, false, "");
                }
            }
        }
        private void ResetDocStepControl()
        {
            BindDropDownDocStep();
        }
    }

}