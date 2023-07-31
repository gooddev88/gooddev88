using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Robot.Data.DataAccess;
using DevExpress.Web;
using Robot.Master.DA;

namespace Robot.Master
{
    public partial class MyUserDetail : MyBasePage
    {
        public static string XUserID { get { return HttpContext.Current.Session["xuserID"] == null ? "" : (string)HttpContext.Current.Session["xuserID"]; } set { HttpContext.Current.Session["xuserID"] = value; } }
        public static string PreviousPageX { get { return HttpContext.Current.Session["masuser_previouspage"] == null ? "" : (string)HttpContext.Current.Session["masuser_previouspage"]; } set { HttpContext.Current.Session["masuser_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            popAlert.ShowOnPageLoad = false;
            SetQueryString2HiddenFiled();            

            if(MyUserInfoService.IsNewDoc) {
                ShowPopAlert("Result", "Save successfull ", true, "");
            }
            LoadDevDropDownList();
            
            if(!IsPostBack) {
                //MyUserInfoService.NewTransaction();
                LoadDropdownList();
                CloseAlert();
                LoadData();
            }
        }
        private void SetQueryString2HiddenFiled()
        {

            hddmenu.Value = Request.QueryString["menu"];
            //hddid.Value = Request.QueryString["id"];
        }

        private void SetActiveControl()
        {
            var h = MyUserInfoService.DocSet.Info;
            lblTopic.Text = PermissionService.GetMenuInfo("9002").Name;
            if (XUserID == "") {
                txtUsername.ReadOnly = false;
                divPermission.Visible = false;
                chkIsActive.Checked = true;
                chkIsProgramUser.Checked = true;
                divlogin.Visible = false;
                //txtSort.Text = h.Sort.ToString();
            } else {
                divPermission.Visible = true;
            }
        }

        private void LoadDevDropDownList()
        {
            cboAddrCountry.DataSource = AddressInfoService.ListCountry();
            cboAddrCountry.DataBind(); 
            //cboCompany.DataSource = CompanyInfoV2Service.ListCompanyInfoUIC("BRANCH", false).ToList(); ;
            //cboCompany.DataBind();

        }

        private void LoadDropdownList()
        {
            cboMaritalStatus.DataSource = MasterTypeInfoService.MiniSelectListV2("MARITAL STATUS", false);
            cboMaritalStatus.DataBind();
            cboGender.DataSource = MasterTypeInfoService.MiniSelectListV2("GENDER", false);
            cboGender.DataBind();
            cboDepartment.DataSource = MasterTypeInfoService.MiniSelectListV2("DEPARTMENT", false);
            cboDepartment.DataBind();
            cboPosition.DataSource = MasterTypeInfoService.MiniSelectListV2("JOB POSITION", false);
            cboPosition.DataBind();

        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton)
        {
            if(result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }

            if(showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            MyUserInfoService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        private void BindDropDownDocStep()
        {
            var stepId = MyUserInfoService.DocSet.ShowInDocStep.Select(o => (string)o.StepID).Distinct().ToList();
            var query = DocStepService.MiniSelectList(false).Where(o => !stepId.Contains(o.StepID)).ToList();
            cboDocStep.DataSource = query;
            cboDocStep.DataBind();
        }

        private void LoadData()
        {
            if(XUserID != "") {
                MyUserInfoService.GetDocSetByID(XUserID); 
            }
            BindData();

            SetActiveControl();
            BindGrdPermisson();
            BindGrdInDocStep();
            BindGrdInBoard();
            GridUserInGroup();
            BindGrdInCompany();
            BindDropDownDocStep();

        }
        private void BindData()
        {


            var u = MyUserInfoService.DocSet.Info;
            txtUsername.Text = XUserID.ToString();
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
            //cboCompany.Value = u.DefaultCompany;

            txtTel1.Text = u.Tel;
            txtMobile.Text = u.Mobile;
            txtEmail.Text = u.Email;
            txtCitizenId.Text = u.CitizenId;
            txtBookbankNumber.Text = u.BookBankNumber;
            if(u.MaritalStatus != null) {
                cboMaritalStatus.SelectedValue = u.MaritalStatus;
            }
            //dtBirthDate.Value = u.Birthdate;
            dtJobStartDate.Value = u.JobStartDate;
            dtResignDate.Value = u.ResignDate;
            chkIsProgramUser.Checked = u.IsProgramUser;
            chkIsUseTimeStampt.Checked = Convert.ToBoolean(u.UseTimeStamp);

            if(u.IsActive) {
                lnkResetPassword.Visible = true;
                //lblDocStatus.Text = "ACTIVE";

            } else {
                lnkResetPassword.Visible = false;
                //lblDocStatus.Text = "INACTIVE";
            }

            txtUsername.ReadOnly = true;

            chkIsActive.Checked = u.IsActive;
        }
        private void BindGrdInDocStep()
        {
            grdGroupInDoc.DataSource = MyUserInfoService.DocSet.ShowInDocStep;
            grdGroupInDoc.DataBind();
        }

        private void BindGrdPermisson()
        {
            grdPermission.DataSource = MyUserInfoService.DocSet.ShowPermission;
            grdPermission.DataBind();
        }

        private void GridUserInGroup()
        {
            grdUserInGroup.DataSource = MyUserInfoService.DocSet.ShowIngroup;
            grdUserInGroup.DataBind();
        }

        private void BindGrdInBoard()
        {
            grdDashBoard.DataSource = MyUserInfoService.DocSet.ShowInBoard;
            grdDashBoard.DataBind();
        }

        private void BindGrdInCompany()
        {
            grdUserInCompany.DataSource = MyUserInfoService.DocSet.ShowInCompany;
            grdUserInCompany.DataBind();
        }

        private void SetInMenu()
        {
            MyUserInfoService.DocSet.SavePermission = new List<UserPermission>();

            foreach(GridViewRow row in grdPermission.Rows) {
                UserPermission per = new UserPermission();
                per.Username = (row.FindControl("lblUserId") as Label).Text;
                per.MenuID = (row.FindControl("lblMenuId") as Label).Text;
                per.IsOpen = (row.FindControl("chkIsOpen") as CheckBox).Checked;
                per.IsCreate = (row.FindControl("chkIsCreate") as CheckBox).Checked;
                per.IsEdit = (row.FindControl("chkIsEdit") as CheckBox).Checked;
                per.IsDelete = (row.FindControl("chkIsDelete") as CheckBox).Checked;
                per.IsPrint = (row.FindControl("chkIsPrint") as CheckBox).Checked;
                MyUserInfoService.DocSet.SavePermission.Add(per);
            }
        }

        private void SetInInGroup()
        {
            MyUserInfoService.DocSet.SaveIngroup = new List<UserInGroup>();
            foreach(GridViewRow row in grdUserInGroup.Rows) {
                if((row.FindControl("chkGroupSelect") as CheckBox).Checked) {
                    UserInGroup com = new UserInGroup();
                    com.UserName = XUserID;
                    com.UserGroupID = (row.FindControl("lblUserGroupID") as Label).Text;
                    com.IsActive = true;
                    MyUserInfoService.DocSet.SaveIngroup.Add(com);
                }
            }
        }

        private void SetInCompany()
        {
            MyUserInfoService.DocSet.SaveInCompany = new List<UserInCompany>();
            foreach(GridViewRow row in grdUserInCompany.Rows) {
                if((row.FindControl("chkIsInCompany") as CheckBox).Checked) {
                    UserInCompany com = new UserInCompany();
                    com.UserName = XUserID;
                    com.CompanyID = (row.FindControl("lblCompanyID") as Label).Text;
                    com.IsActive = true;
                    MyUserInfoService.DocSet.SaveInCompany.Add(com);
                }
            }
        }
        private void SetInBoard()
        {
            MyUserInfoService.DocSet.SaveInBoard = new List<UserInBoard>();

            foreach(GridViewRow row in grdDashBoard.Rows) {
                if((row.FindControl("chkIsInBoard") as CheckBox).Checked) {
                    UserInBoard b = new UserInBoard();
                    b.Username = XUserID;
                    b.DashBoardID = (row.FindControl("lblDashBoardID") as Label).Text;
                    MyUserInfoService.DocSet.SaveInBoard.Add(b);
                }
            }
        }


        private void ShowAlert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private bool PrepairDataSave()
        {
            bool isNew = XUserID == "" ? true : false;
            var h = MyUserInfoService.DocSet.Info;

            h.Username = txtUsername.Text.Trim();
            h.EmpCode = txtEmployeeCode.Text.Trim();
            h.Password = EncryptService.hashPassword("MD5", UserInfoService.GetDefualtPassword());
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
            if(cboAddrCountry.Value != null) {
                h.AddrCountry = cboAddrCountry.Value.ToString();
            }
            h.DefaultCompany = "";
            //if(cboCompany.Value != null) {
            //    h.DefaultCompany = cboCompany.Value.ToString();
            //}
            h.AddrNo = txtAddrNo.Text;
            h.AddrMoo = txtAddrMoo.Text;
            h.AddrTumbon = txtAddrTumbon.Text;
            h.AddrAmphoe = txtAddrAmphoe.Text;
            h.AddrPostCode = txtAddrPostCode.Text;
            h.AddrProvince = txtAddrProvince.Text;
            if(cboAddrCountry.Value != null) {
                h.AddrCountry = cboAddrCountry.Value.ToString();
            }

            h.Tel = txtTel1.Text;
            h.Mobile = txtMobile.Text;
            h.Email = txtEmail.Text;
            h.CitizenId = txtCitizenId.Text;
            h.BookBankNumber = txtBookbankNumber.Text;

            h.MaritalStatus = cboMaritalStatus.SelectedValue.ToString();

            if(dtJobStartDate.Value != null) {
                h.JobStartDate = DateTime.Parse(dtJobStartDate.Value.ToString());
            }
            if(dtResignDate.Value != null) {
                h.ResignDate = DateTime.Parse(dtResignDate.Value.ToString());
            }
            h.ApproveBy = "";
            h.IsProgramUser = chkIsProgramUser.Checked;
            h.UseTimeStamp = chkIsUseTimeStampt.Checked;


            h.IsActive = chkIsActive.Checked;
            SetInBoard();
            SetInCompany();
            SetInMenu();
            SetInInGroup();
            return isNew;
        }

        private bool ValidData()
        {
            bool result = true;

            if(txtUsername.Text.Trim() == "") {
                ShowPopAlert("Error", "Input Username !! ", false, "");
                return false;
            }
            if(txtFirstName.Text == "") {
                ShowPopAlert("Error", "กรุณาระบุ ชื่อ!! ", false, "");
                return false;
            }
            if(txtLastName.Text == "") {
                ShowPopAlert("Error", "กรุณาระบุ นามสกุล!! ", false, "");
                return false;
            }
            //if (cboCompany.Value==null) {
            //    ShowPopAlert("Error", "ระบุ สาขา (Default) ", false, "");
            //    return false;
            //}



            if(XUserID == "") {
                var chkDup = UserInfoService.GetDataByUserID(txtUsername.Text.Trim());
                if(chkDup != null) {
                    ShowPopAlert("Error", "มีชื่อผู้ใช้ " + txtUsername.Text.Trim() + "ในระบบแล้ว!! ", false, "");
                    return false;
                }
            }

            return result;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try {
                if(!ValidData()) {
                    return;
                }
                bool isNew = PrepairDataSave();



                if(isNew) {
                    MyUserInfoService.Save("insert");
                } else {
                    MyUserInfoService.Save("update");

                }
                var h = MyUserInfoService.DocSet.Info;
                if(MyUserInfoService.DocSet.OutputAction.Result == "fail") {
                    ShowPopAlert("Error", MyUserInfoService.DocSet.OutputAction.Message1, false, "");
                } else {//save success  
                    if(isNew) {
                        XUserID = h.Username;
                        MyUserInfoService.IsNewDoc = true;

                        string myurl = $"../Master/MyUserDetail?menu={hddmenu.Value}";
                        Response.RedirectPermanent(myurl);
                    } else {
                        ShowPopAlert("Success", "Save Succesfull", true, "");
                        LoadData();
                    }
                }
            } catch(Exception ex) {
                ShowPopAlert("Error", ex.Message, false, "");
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            XUserID = "";
            string myurl = $"../Master/MyUserDetail?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void grdPermission_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow) {
                Label lblIsOpen = e.Row.FindControl("lblIsOpen") as Label;
                Label lblNeedOpen = e.Row.FindControl("lblNeedOpen") as Label;
                CheckBox chkIsOpen = e.Row.FindControl("chkIsOpen") as CheckBox;
                if(lblNeedOpen.Text == "False") {
                    chkIsOpen.Enabled = false;
                }
                if(lblIsOpen.Text == "1") {
                    chkIsOpen.Checked = true;
                }

                Label lblIsCreate = e.Row.FindControl("lblIsCreate") as Label;
                Label lblNeedCreate = e.Row.FindControl("lblNeedCreate") as Label;
                CheckBox chkIsCreate = e.Row.FindControl("chkIsCreate") as CheckBox;
                if(lblNeedCreate.Text == "False") {
                    chkIsCreate.Enabled = false;
                }
                if(lblIsCreate.Text == "1") {
                    chkIsCreate.Checked = true;
                }

                Label lblIsEdit = e.Row.FindControl("lblIsEdit") as Label;
                Label lblNeedEdit = e.Row.FindControl("lblNeedEdit") as Label;
                CheckBox chkIsEdit = e.Row.FindControl("chkIsEdit") as CheckBox;
                if(lblNeedEdit.Text == "False") {
                    chkIsEdit.Enabled = false;
                }
                if(lblIsEdit.Text == "1") {
                    chkIsEdit.Checked = true;
                }

                Label lblIsDelete = e.Row.FindControl("lblIsDelete") as Label;
                Label lblNeedDelete = e.Row.FindControl("lblNeedDelete") as Label;
                CheckBox chkIsDelete = e.Row.FindControl("chkIsDelete") as CheckBox;
                if(lblNeedDelete.Text == "False") {
                    chkIsDelete.Enabled = false;
                }
                if(lblIsDelete.Text == "1") {
                    chkIsDelete.Checked = true;
                }

                Label lblIsPrint = e.Row.FindControl("lblIsPrint") as Label;
                Label lblNeedPrint = e.Row.FindControl("lblNeedPrint") as Label;
                CheckBox chkIsPrint = e.Row.FindControl("chkIsPrint") as CheckBox;
                if(lblNeedPrint.Text == "False") {
                    chkIsPrint.Enabled = false;
                }
                if(lblIsPrint.Text == "1") {
                    chkIsPrint.Checked = true;
                }
            }
        }

        protected void grdUserInCompany_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grdDashBoard_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grdUserInGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow) {
                bool IsInGroup = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsInGroup"));
                CheckBox chkGroupSelect = e.Row.FindControl("chkGroupSelect") as CheckBox;
                if(IsInGroup) {
                    chkGroupSelect.Checked = true;
                } else {
                    chkGroupSelect.Checked = false;
                }
            }
        }


        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(PreviousPageX);
        }

        protected void chkselectCompany_CheckedChanged(object sender, EventArgs e)
        {
            MyUserInfoService.BulkCheckCompany(chkselectCompany.Checked);
            BindGrdInCompany();
        }

        protected void btnCopyRole_Click(object sender, EventArgs e)
        {
            CopyPermission.FromRoleID = XUserID;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "user";
            CopyPermission.CopyType = "menu";
            string myurl = $"../Master/CopyPermission";
            Response.Redirect(myurl);
        }
        protected void btnCopyCom_Click(object sender, EventArgs e)
        {
            CopyPermission.FromRoleID = XUserID;
            CopyPermission.PreviousePage = HttpContext.Current.Request.Url.PathAndQuery;
            CopyPermission.RoleType = "user";
            CopyPermission.CopyType = "company";
            string myurl = $"../Master/CopyPermission";
            Response.Redirect(myurl);
        }
        protected void btnAddDocStep_Click(object sender, EventArgs e)
        {
            var chk_User_InDoc = MyUserInfoService.CheckUserInDoc(cboDocStep.SelectedValue, XUserID);
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
            var h = MyUserInfoService.DocSet.Info;
            var n = MyUserInfoService.NewDocStep();
            n.Username = h.Username;
            n.StepID = cboDocStep.SelectedValue;
            n.StepName = stepInfo.StepName;

            var r = MyUserInfoService.SaveDocStep(n);
            if (r.Result == "ok")
            {
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                MyUserInfoService.ListDocStep(h.Username);
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
            if(e.CommandName == "Del") {
                var h = MyUserInfoService.DocSet.Info;
                int id = Convert.ToInt32(e.CommandArgument);
                var r = MyUserInfoService.DeleteDocStep(id);

                if (r.Result == "ok")
                {
                    ShowPopAlert("Success", "Delete successfull", false, "");

                    MyUserInfoService.ListDocStep(h.Username);
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


        protected void lnkResetPassword_Click(object sender, EventArgs e)
        {
            lblShowNewPasswordAfterReset.Visible = true;
            var r = MyUserInfoService.ResetPassword(XUserID); 
            if(r[0] == "R1") { 
                ShowPopAlert("Success", r[1], true, "");
            }
            if(r[0] == "R0") { 
                ShowPopAlert("Error", r[1], false, "");
            }
        }

        protected void cboCusAddr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboCusAddr.Value == null) {
                return;
            }
            int addrId = Convert.ToInt32(cboCusAddr.Value);
            var getAddr = AddressService.GetAddressInfoById(addrId);
            SetAddr(getAddr);
        }

        private void SetAddr(vw_ThaiPostAddress addr)
        {
            txtAddrPostCode.Text = addr.DISTRICT_POSTAL_CODE;
            txtAddrTumbon.Text = addr.DISTRICT_NAME;
            txtAddrAmphoe.Text = addr.BORDER_NAME;
            txtAddrProvince.Text = addr.PROVINCE_NAME;
        }

        protected void cboCusAddr_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
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

        protected void cboCusAddr_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if(e.Value == null) {
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

        protected void chkIsNewUser_CheckedChanged(object sender, EventArgs e)
        {
            var r = MyUserInfoService.SetIsNewUser(XUserID, chkIsNewUser.Checked);
        }
        protected void chkIsProgramUser_CheckedChanged(object sender, EventArgs e)
        {
            var r = MyUserInfoService.SetIsProgramUser(XUserID, chkIsProgramUser.Checked);
        }

        protected void chkIsUseTimeStampt_CheckedChanged(object sender, EventArgs e)
        {
            var r = MyUserInfoService.SetUseTimeStamp(XUserID, chkIsUseTimeStampt.Checked);
        }

    }

}