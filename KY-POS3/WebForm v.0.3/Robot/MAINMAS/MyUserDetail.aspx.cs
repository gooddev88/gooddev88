using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Robot.Data.DataAccess;
using DevExpress.Web;
using static Robot.Data.BL.I_Result;
using Robot.Master.DA;

namespace Robot.MAINMAS {
    public partial class MyUserDetail : MyBasePage {
                public static string PreviousPageX { get { return HttpContext.Current.Session["masuser_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masuser_previouspage"]; } set { HttpContext.Current.Session["masuser_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            popAlert.ShowOnPageLoad = false;
            SetQueryString();
            if (UserService.IsNewDoc) {
                ShowPopAlert("Result", "Save successfull ", true, "");
            }
            LoadDevDropDownList();
            if (!IsPostBack) { 
                LoadDropdownList();
                CloseAlert();
                LoadData();
            }
        }
        private void SetQueryString() { 
            hddmenu.Value = Request.QueryString["menu"]; 
        }

        private void SetActiveControl() {
            var h = UserService.DocSet.User;      
            if (h.Username == "") { 
                divPermission.Visible = false;
                chkIsActive.Checked = true;
                chkIsProgramUser.Checked = true;
                divlogin.Visible = false;              
            } else {
                divPermission.Visible = true;
                txtUsername.Enabled = false;
            }
         var isSupperman=   LoginService.IsSupperman(LoginService.LoginInfo.CurrentRootCompany.CompanyID, LoginService.LoginInfo.CurrentUser);
            if (!isSupperman) {
                divMenuPermission.Visible = false;
            }
          
        }

        private void LoadDevDropDownList() {
            cboAddrCountry.DataSource = AddressInfoService.ListCountry();
            cboAddrCountry.DataBind();
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", true);
            cboCompany.DataBind();
        }

        private void LoadDropdownList() {
            cboMaritalStatus.DataSource = MasterTypeService.ListType("MARITAL STATUS", false);
            cboMaritalStatus.DataBind();
            cboGender.DataSource = MasterTypeService.ListType("GENDER", false);
            cboGender.DataBind();
            cboDepartment.DataSource = MasterTypeService.ListType("DEPARTMENT", false);
            cboDepartment.DataBind();
            cboPosition.DataSource = MasterTypeService.ListType("JOB POSITION", false);
            cboPosition.DataBind();
            cboUserType.DataSource = MasterTypeService.ListType("USER TYPE", true);
            cboUserType.DataBind();
            cboUserType.SelectedIndex = 0;
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
            UserService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

         
        private void LoadData() {
            var h = UserService.DocSet.User;
            BindData();
            BindGrdPermisson();
            BindGrdInBoard();
            BindGrdInRCompany();
            BindGrdInCompany();
            BindGrdInGroup();
            SetActiveControl();
        }

        private void BindData() { 
            var u = UserService.DocSet.User;
            lblTopic.Text = PermissionService.GetMenuInfo("9002").Name;
            txtUsername.Text = u.Username;
            txtEmployeeCode.Text = u.EmpCode;
            txtFirstName.Text = u.FirstName;
            txtLastName.Text = u.LastName;
            chkIsNewUser.Checked = u.IsNewUser;
            txtFirstName_En.Text = u.FirstName_En;
            txtLastName_En.Text = u.LastName_En;
            txtNickName.Text = u.NickName;
            cboGender.SelectedValue = u.Gender;
            cboPosition.SelectedValue = u.PositionID;
            cboDepartment.SelectedValue = u.DepartmentID;
            txtAddrNo.Text = u.AddrNo;
            txtAddrMoo.Text = u.AddrMoo;
            txtAddrTumbon.Text = u.AddrTumbon;
            txtAddrAmphoe.Text = u.AddrAmphoe;
            txtAddrPostCode.Text = u.AddrPostCode;
            txtAddrProvince.Text = u.AddrProvince;
            cboAddrCountry.Value = u.AddrCountry;
            cboCompany.Value = u.DefaultCompany;           
   
            txtTel1.Text = u.Tel;
            txtMobile.Text = u.Mobile;
            txtEmail.Text = u.Email;
            txtCitizenId.Text = u.CitizenId;
            txtBookbankNumber.Text = u.BookBankNumber;
            if (u.MaritalStatus != null) {
                cboMaritalStatus.SelectedValue = u.MaritalStatus;
            }
            dtJobStartDate.Value = u.JobStartDate;
            dtResignDate.Value = u.ResignDate;
            chkIsProgramUser.Checked = u.IsProgramUser;
            chkIsUseTimeStampt.Checked = Convert.ToBoolean(u.UseTimeStamp);

            if (u.IsActive) {
                lnkResetPassword.Visible = true;
            } else {
                lnkResetPassword.Visible = false;
            } 
            chkIsActive.Checked = u.IsActive;
        }


        private void BindGrdPermisson() {
            grdPermission.DataSource = UserService.DocSet.XMenu;
            grdPermission.DataBind();
        }
        private void BindGrdInBoard() {
            grdDashBoard.DataSource = UserService.DocSet.XBoard;
            grdDashBoard.DataBind();
        }
        private void BindGrdInRCompany() {
            grdUserInRCompany.DataSource = UserService.DocSet.XRCompany;
            grdUserInRCompany.DataBind();
        }
        private void BindGrdInCompany() {
            grdUserInCompany.DataSource = UserService.DocSet.XCompany;
            grdUserInCompany.DataBind();
        }
        private void BindGrdInGroup() {
            grdUserInGroup.DataSource = UserService.DocSet.XGroup;
            grdUserInGroup.DataBind();
        }

        private void SetSelectMenu() {
            var ox = UserService.DocSet.XMenu;
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

        private void SetSelectRCompany() {
            var ox = UserService.DocSet.XRCompany;
            foreach (GridViewRow row in grdUserInRCompany.Rows) {
                var q = (row.FindControl("lblRCompanyID") as Label).Text;
                var oo = ox.Where(o => o.RComID == q).FirstOrDefault();
                oo.X = (row.FindControl("chkIsInRCompany") as CheckBox).Checked;
            } 
        }

        private void SetSelectCompany() {
            var ox = UserService.DocSet.XCompany;
            foreach (GridViewRow row in grdUserInCompany.Rows) {
                var q = (row.FindControl("lblCompanyID") as Label).Text;
                var oo = ox.Where(o => o.CompanyID == q).FirstOrDefault();
                oo.X = (row.FindControl("chkIsInCompany") as CheckBox).Checked;
            }
        }

        private void SetSelectBoard() {
            var ox = UserService.DocSet.XBoard;
            foreach (GridViewRow row in grdDashBoard.Rows) {
                var q = (row.FindControl("lblDashBoardID") as Label).Text;
                var oo = ox.Where(o => o.DashBoardID == q).FirstOrDefault();
                oo.X = (row.FindControl("chkIsInBoard") as CheckBox).Checked;
            }
        }

        private void SetSelectGroup() {
            var ox = UserService.DocSet.XGroup;
            foreach (GridViewRow row in grdUserInGroup.Rows) {
                var q = (row.FindControl("lblUserGroupID") as Label).Text;
                var oo = ox.Where(o => o.UserGroupID == q).FirstOrDefault();
                oo.X = (row.FindControl("chkGroupSelect") as CheckBox).Checked;
            }
        }

        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }

        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private bool PrepairDataSave() {
            var h = UserService.DocSet.User;
            bool isNew = h.Username == "" ? true : false;
            h.Username = txtUsername.Text.Trim();
            h.EmpCode = txtEmployeeCode.Text.Trim();
            h.Password = EncryptService.hashPassword("MD5", UserService.GetDefualtPassword());
            h.FirstName = txtFirstName.Text.Trim();
            h.LastName = txtLastName.Text.Trim();
            h.IsNewUser = chkIsNewUser.Checked;
            h.FirstName_En = txtFirstName_En.Text.Trim();
            h.LastName_En = txtLastName_En.Text.Trim();
            h.NickName = txtNickName.Text.Trim();
            h.Gender = cboGender.SelectedValue.ToString();
            h.DepartmentID = cboDepartment.SelectedValue.ToString();
            h.PositionID = cboPosition.SelectedValue.ToString();
            h.AddrFull = "";
            h.LineToken = txtTokenLine.Text;
            h.AddrCountry = "";
            if (cboAddrCountry.Value != null) {
                h.AddrCountry = cboAddrCountry.Value.ToString();
            }
            h.DefaultCompany = "";
            if (cboCompany.Value != null) {
                h.DefaultCompany = cboCompany.Value.ToString();
            }
            h.AddrNo = txtAddrNo.Text;
            h.AddrMoo = txtAddrMoo.Text;
            h.AddrTumbon = txtAddrTumbon.Text;
            h.AddrAmphoe = txtAddrAmphoe.Text;
            h.AddrPostCode = txtAddrPostCode.Text;
            h.AddrProvince = txtAddrProvince.Text;
            if (cboAddrCountry.Value != null) {
                h.AddrCountry = cboAddrCountry.Value.ToString();
            } 
            h.Tel = txtTel1.Text;
            h.Mobile = txtMobile.Text;
            h.Email = txtEmail.Text;
            h.CitizenId = txtCitizenId.Text;
            h.BookBankNumber = txtBookbankNumber.Text;
            h.MaritalStatus = cboMaritalStatus.SelectedValue.ToString();

            if (dtJobStartDate.Value != null) {
                h.JobStartDate = DateTime.Parse(dtJobStartDate.Value.ToString());
            }
            if (dtResignDate.Value != null) {
                h.ResignDate = DateTime.Parse(dtResignDate.Value.ToString());
            }
            h.ApproveBy = "";
            h.IsProgramUser = chkIsProgramUser.Checked;
            h.UseTimeStamp = chkIsUseTimeStampt.Checked;
            h.IsActive = chkIsActive.Checked;
            SetSelectBoard();
            SetSelectGroup();
            SetSelectRCompany();
            SetSelectCompany();
            SetSelectMenu();
            return isNew;
        }

        private bool ValidData() {
            var h = UserService.DocSet.User;
            bool result = true;
            if (txtUsername.Text.Trim() == "") {
                ShowPopAlert("Error", "Input Username !! ", false, "");
                return false;
            }
            if (txtFirstName.Text == "") {
                ShowPopAlert("Error", "กรุณาระบุ ชื่อ!! ", false, "");
                return false;
            }
            if (txtLastName.Text == "") {
                ShowPopAlert("Error", "กรุณาระบุ นามสกุล!! ", false, "");
                return false;
            }
            //if (cboCompany.Value == null) {
            //    ShowPopAlert("Error", "ระบุ สาขา (Default) ", false, "");
            //    return false;
            //}
            if (h.Username == "") {
                var chkDup = UserService.GetUserInfo(txtUsername.Text.Trim());
                if (chkDup != null) {
                    ShowPopAlert("Error", "มีชื่อผู้ใช้ " + txtUsername.Text.Trim() + "ในระบบแล้ว!! ", false, "");
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
                    r = UserService.Save("insert");
                } else {
                    r = UserService.Save("update");
                }
                var h = UserService.DocSet.User;
                if (r.Result == "fail") {
                    if (isNew) {
                        UserService.DocSet.User.Username = "";
                    }
                    ShowPopAlert("Error", r.Message1, false, "");
                } else {//save success  
                    if (isNew) {
                        UserService.IsNewDoc = true;
                        UserService.GetDocSetByID(h.Username);
                        string myurl = $"../MAINMAS/MyUserDetail?menu={hddmenu.Value}";
                        Response.Redirect(Request.RawUrl);
                    } else {
                        ShowPopAlert("Success", "Save Succesfull", true, "");
                        UserService.GetDocSetByID(h.Username);
                        LoadData();
                    }
                }
            } catch (Exception ex) {
                ShowPopAlert("Error", ex.Message, false, "");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            UserService.NewTransaction();
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
        protected void grdUserInRCompany_RowDataBound(object sender, GridViewRowEventArgs e) {

        }
        protected void grdDashBoard_RowDataBound(object sender, GridViewRowEventArgs e) {

        }
        protected void grdUserInGroup_RowDataBound(object sender, GridViewRowEventArgs e) {
            //if (e.Row.RowType == DataControlRowType.DataRow) {
            //    bool IsInGroup = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "X"));
            //    CheckBox chkGroupSelect = e.Row.FindControl("chkGroupSelect") as CheckBox;
            //    if (IsInGroup) {
            //        chkGroupSelect.Checked = true;
            //    } else {
            //        chkGroupSelect.Checked = false;
            //    }
            //}
        }


        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPageX);
        }
       protected void chkselectRCompany_CheckedChanged(object sender, EventArgs e) {
            UserService.CheckUnCheckRCompany(chkselectRCompany.Checked);
            BindGrdInRCompany();
        }
        protected void chkselectCompany_CheckedChanged(object sender, EventArgs e) {
            UserService.CheckUnCheckCompany(chkselectCompany.Checked);
            BindGrdInCompany();
        }

        protected void btnCopyRole_Click(object sender, EventArgs e) {
            var h = UserService.DocSet.User;
            CopyPermission.FromRoleID = h.Username;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "user";
            CopyPermission.CopyType = "menu";
            string myurl = $"../MAINMAS/CopyPermission";
            Response.Redirect(myurl);
        }
        protected void btnCopyCom_Click(object sender, EventArgs e) {
            var h = UserService.DocSet.User;
            CopyPermission.FromRoleID = h.Username;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "user";
            CopyPermission.CopyType = "company";
            string myurl = $"../MAINMAS/CopyPermission";
            Response.Redirect(myurl);
        }

        protected void lnkResetPassword_Click(object sender, EventArgs e) {
            var h = UserService.DocSet.User;
            lblShowNewPasswordAfterReset.Visible = true;
            var r = UserService.ResetPassword(h.Username);
            if (r.Result=="ok") {
                ShowPopAlert("Success","รหัส่ผ่านถูกรีเซ็ตเป็น : " +r.Message2, true, "");
            } else {
                ShowPopAlert("Error",  r.Message1, false, "");
            }
        }

        protected void cboCusAddr_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboCusAddr.Value == null) {
                return;
            }
            int addrId = Convert.ToInt32(cboCusAddr.Value);
            var getAddr = AddressInfoService.GetAddressInfoById(addrId);
            SetAddr(getAddr);
        }

        private void SetAddr(vw_ThaiPostAddress addr) {
            txtAddrPostCode.Text = addr.DISTRICT_POSTAL_CODE;
            txtAddrTumbon.Text = addr.DISTRICT_NAME;
            txtAddrAmphoe.Text = addr.BORDER_NAME;
            txtAddrProvince.Text = addr.PROVINCE_NAME;
        }

        protected void cboCusAddr_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
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

        protected void cboCusAddr_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
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

      

    }

}