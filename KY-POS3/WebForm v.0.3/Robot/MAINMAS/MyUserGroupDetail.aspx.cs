using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Robot.Data.DataAccess;
using static Robot.Data.BL.I_Result;
using DevExpress.Web;

namespace Robot.MAINMAS {
    public partial class MyUserGroupDetail : MyBasePage {

        //public static string XUsergroupID { get { return HttpContext.Current.Session["xusergroupID"] == null ? "" : (string)HttpContext.Current.Session["xusergroupID"]; } set { HttpContext.Current.Session["xusergroupID"] = value; } }
        public static string PreviousPageX { get { return HttpContext.Current.Session["masusergroup_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masusergroup_previouspage"]; } set { HttpContext.Current.Session["masusergroup_previouspage"] = value; } }


        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();

            LoadDevDropDownList();
            popAlert.ShowOnPageLoad = false;
            if (UserGroupService.IsNewDoc) {
                ShowPopAlert("Result", "Save successfull ", true, "");
            }
            if (!IsPostBack) {

                CloseAlert();

                LoadData();
            }
        }
        private void SetQueryString() {
            //hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void SetActiveControl() {
            var h = UserGroupService.DocSet.Group;
            lblTopic.Text = PermissionService.GetMenuInfo("9003").Name;
            if (h.UserGroupID == "") {
                txtUserGroupId.Enabled = true;
                divPermission.Visible = false;
                chkActive.Checked = true;
                txtSort.Text = h.Sort.ToString();
            } else {
                txtUserGroupId.Enabled = false;
                divPermission.Visible = true;
            }
        }

        private void LoadDevDropDownList() {

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
            UserGroupService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }


        private void LoadData() {
            var h = UserGroupService.DocSet.Group;
            BindData();
            BindGrdPermisson();
            BindGrdInBoard();
            BindGrdInCompany();
            BindGrdUser();
            SetActiveControl();
        }
        private void BindData() {
            var h = UserGroupService.DocSet.Group;
            txtUserGroupId.Text = h.UserGroupID;
            txtGroupName.Text = h.GroupName;
            txtSort.Text = h.Sort.ToString();
            chkActive.Checked = h.IsActive;
        }
        private void BindGrdPermisson() {
            grdPermission.DataSource = UserGroupService.DocSet.XMenu;
            grdPermission.DataBind();
        }
        private void BindGrdInBoard() {
            grdDashBoard.DataSource = UserGroupService.DocSet.XBoard;
            grdDashBoard.DataBind();
        }

        private void BindGrdInCompany() {
            grdUserInCompany.DataSource = UserGroupService.DocSet.XCompany;
            grdUserInCompany.DataBind();
        }


        private void SetSelectMenu() {
            var ox = UserGroupService.DocSet.XMenu;
            foreach (GridViewRow row in grdPermission.Rows) {
                var q = (row.FindControl("lblMenuId") as Label).Text;
                var oo = ox.Where(o => o.MenuID == q).FirstOrDefault();
                if (oo != null) {
                    oo.IsOpen = (row.FindControl("chkIsOpen") as CheckBox).Checked;
                    oo.IsCreate = (row.FindControl("chkIsCreate") as CheckBox).Checked;
                    oo.IsEdit = (row.FindControl("chkIsEdit") as CheckBox).Checked;
                    oo.IsDelete = (row.FindControl("chkIsDelete") as CheckBox).Checked;
                    oo.IsPrint = (row.FindControl("chkIsPrint") as CheckBox).Checked;
                }
            }
        }

        private void SetSelectCompany() {
            var ox = UserGroupService.DocSet.XCompany;
            foreach (GridViewRow row in grdUserInCompany.Rows) {
                var q = (row.FindControl("lblCompanyID") as Label).Text;
                var oo = ox.Where(o => o.CompanyID == q).FirstOrDefault();
                oo.X = (row.FindControl("chkIsInCompany") as CheckBox).Checked;
            }

        }
        private void SetSelectBoard() {
            var ox = UserGroupService.DocSet.XBoard;
            foreach (GridViewRow row in grdDashBoard.Rows) {
                var q = (row.FindControl("lblDashBoardID") as Label).Text;
                var oo = ox.Where(o => o.DashBoardID == q).FirstOrDefault();
                oo.X = (row.FindControl("chkIsInBoard") as CheckBox).Checked;

            }
        }


        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private bool PrepairDataSave() {
            var h = UserGroupService.DocSet.Group;
            bool isNew = h.UserGroupID == "" ? true : false;
            if (isNew) {
                h.UserGroupID = txtUserGroupId.Text.Trim().ToUpper();
                h.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            }

            h.GroupName = txtGroupName.Text;
            int sort = 0;
            int.TryParse(txtSort.Text, out sort);
            h.Sort = sort;
            h.IsActive = chkActive.Checked;
            SetSelectBoard();
            SetSelectCompany();
            SetSelectMenu();
            return isNew;
        }

        private bool ValidData() {
            var h = UserGroupService.DocSet.Group;
            bool result = true;

            if (txtUserGroupId.Text.Trim() == "") {
                ShowPopAlert("Error", "ระบุรหัสกลุ่ม!!", false, "");
                return false;

            }
            if (txtGroupName.Text == "") {
                ShowPopAlert("Error", "ระบุชื่อกลุ่ม!!", false, "");
                return false;
            }
            if (h.UserGroupID == "") {
                UserGroupService.DocSet.Group.Sort = Convert.ToInt32(txtSort.Text);
                if (UserGroupService.CheckDupSort().Result == "fail") {
                    ShowPopAlert("Error", UserGroupService.CheckDupSort().Message1, false, "");
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
                var r = new I_BasicResult();
                if (isNew) {
                    r = UserGroupService.Save("insert");
                } else {
                    r = UserGroupService.Save("update");
                }
                var h = UserGroupService.DocSet.Group;
                if (r.Result == "fail") {
                    if (isNew) {
                        UserGroupService.DocSet.Group.UserGroupID = "";
                    }
             
                    ShowPopAlert("Error", r.Message1, false, "");
                } else {//save success  

                    if (isNew) {
                        UserGroupService.IsNewDoc = true;
                        UserGroupService.GetDocSetByID(h.UserGroupID);
                        string myurl = $"../MAINMAS/MyUserGroupDetail?menu={hddmenu.Value}";
                        Response.Redirect(Request.RawUrl);

                    } else {
                        ShowPopAlert("Success", "Save Succesfull", true, "");
                        UserGroupService.GetDocSetByID(h.UserGroupID);
                        LoadData();
                    }
                }
            } catch (Exception ex) {

                ShowPopAlert("Error", ex.Message, false, "");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            UserGroupService.NewTransaction();
            Response.Redirect(Request.RawUrl);

        }

        protected void grdPermission_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {

                Label lblIsOpen = e.Row.FindControl("lblIsOpen") as Label;
                Label lblNeedOpen = e.Row.FindControl("lblNeedOpen") as Label;
                CheckBox chkIsOpen = e.Row.FindControl("chkIsOpen") as CheckBox;
                chkIsOpen.Enabled = Convert.ToBoolean(lblNeedOpen.Text);
                chkIsOpen.Checked = Convert.ToBoolean(lblIsOpen.Text);

                Label lblIsCreate = e.Row.FindControl("lblIsCreate") as Label;
                Label lblNeedCreate = e.Row.FindControl("lblNeedCreate") as Label;
                CheckBox chkIsCreate = e.Row.FindControl("chkIsCreate") as CheckBox;
                chkIsCreate.Enabled = Convert.ToBoolean(lblNeedCreate.Text);
                chkIsCreate.Checked = Convert.ToBoolean(lblIsCreate.Text);


                Label lblIsEdit = e.Row.FindControl("lblIsEdit") as Label;
                Label lblNeedEdit = e.Row.FindControl("lblNeedEdit") as Label;
                CheckBox chkIsEdit = e.Row.FindControl("chkIsEdit") as CheckBox;
                chkIsEdit.Enabled = Convert.ToBoolean(lblNeedEdit.Text);
                chkIsEdit.Checked = Convert.ToBoolean(lblIsEdit.Text);


                Label lblIsDelete = e.Row.FindControl("lblIsDelete") as Label;
                Label lblNeedDelete = e.Row.FindControl("lblNeedDelete") as Label;
                CheckBox chkIsDelete = e.Row.FindControl("chkIsDelete") as CheckBox;
                chkIsDelete.Enabled = Convert.ToBoolean(lblNeedDelete.Text);
                chkIsDelete.Checked = Convert.ToBoolean(lblIsDelete.Text);

                Label lblIsPrint = e.Row.FindControl("lblIsPrint") as Label;
                Label lblNeedPrint = e.Row.FindControl("lblNeedPrint") as Label;
                CheckBox chkIsPrint = e.Row.FindControl("chkIsPrint") as CheckBox;
                chkIsPrint.Enabled = Convert.ToBoolean(lblNeedPrint.Text);
                chkIsPrint.Checked = Convert.ToBoolean(lblIsPrint.Text);

            }
        }

        protected void grdUserInCompany_RowDataBound(object sender, GridViewRowEventArgs e) {

        }



        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousPageX);
        }

        protected void chkselectCompany_CheckedChanged(object sender, EventArgs e) {
            UserGroupService.CheckUnCheckCompany(chkselectCompany.Checked);
            BindGrdInCompany();
        }

        protected void btnCopyRole_Click(object sender, EventArgs e) {
            CopyPermission.FromRoleID = UserGroupService.DocSet.Group.UserGroupID;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "group";
            CopyPermission.CopyType = "menu";
            string myurl = $"../MAINMAS/CopyPermission";
            Response.Redirect(myurl);
        }
        protected void btnCopyCom_Click(object sender, EventArgs e) {
            CopyPermission.FromRoleID = UserGroupService.DocSet.Group.UserGroupID;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "group";
            CopyPermission.CopyType = "company";
            string myurl = $"../MAINMAS/CopyPermission";
            Response.Redirect(myurl);
        }

        private void BindGrdUser() {
            grdUser.DataSource = UserGroupService.DocSet.User;
            grdUser.DataBind();
        }

        protected void grdUser_DataBinding(object sender, EventArgs e) {
            grdUser.DataSource = UserGroupService.DocSet.User;
        }
        protected void grdUser_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e) {

        }

        protected void grdUser_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e) {
            var h = UserGroupService.DocSet.Group;


        }
    }

}