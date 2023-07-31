using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using System.Drawing;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;

namespace Robot.CRM {
    public partial class QuestionDetail : MyBasePage {

        protected void Page_Load(object sender, EventArgs e)
        {
            hddmenu.Value = Request.QueryString["menu"];
            hdddoctype.Value = Request.QueryString["doctype"];
            hddid.Value = Request.QueryString["id"];
            hddcopy.Value = Request.QueryString["copy"];

            popAlert.ShowOnPageLoad = false;
            if (Convert.ToBoolean(Session["save_new_success"]))
            {
                ShowPopAlert("ผลการทำงาน", "สร้างเอกสารใหม่สำเร็จ " + hddid.Value, true, "");
            }
            LoadDropDownDevList();


            if (!IsPostBack)
            {
                CloseAlert();
                LoadDropDownList();
                hddPreviouspage.Value = Session["Questionnair_previouspage"].ToString();
                LoadData();
            }

            InitControlByDocStatus();
        }

        private void InitControlByDocStatus()
        {

        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton)
        {
            if (result)
            {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }
            if (showbutton == "")
            {
                btnCancel.Visible = false;
            }
            if (showbutton == "okcancel")
            {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            Session["save_new_success"] = false;
            popAlert.ShowOnPageLoad = true;
        }

        public string SetMenu(object menu_id)
        {
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null)
            {
                result = data.IsOpen.ToString();
            }
            return result;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        private void LoadDropDownList()
        {
            cboQType.DataSource = MasterTypeInfoService.MiniSelectList("QUESTION TYPE", false, false);
            cboQType.DataBind();
            cboQGroup.DataSource = MasterTypeInfoService.MiniSelectList("QUESTION GROUP", false, false);
            cboQGroup.DataBind();
        }
        private void LoadDropDownDevList()
        {

        }
        private void ClearSession()
        {
            Session[hddmenu.Value + "Ques_head"] = null;
            Session[hddmenu.Value + "Ques_lines"] = null;
        }


        private void LoadData()
        {
            string docid = hddid.Value;
            if (hddid.Value == "")
            {
                ClearSession();
                SetupControl("NEW");
                NewHead();
                BindGrdLine();
                CheckPermission();
                if (hddcopy.Value == "")
                {
                    return;
                }
            }

            QuestionHead head = new QuestionHead();
            List<QuestionLine> line = new List<QuestionLine>();

            if (hddcopy.Value != "")
            {
                docid = hddcopy.Value;
            }

            head = QuestionInfoService.GetDataByID(docid);
            line = QuestionInfoService.GetDataListByID(docid);


            SetupControl("EDIT");
            Session[hddmenu.Value + "Ques_head"] = head;
            Session[hddmenu.Value + "Ques_lines"] = line;

            BindGrdLine();
            BindData();
            CheckPermission();
        }

        private void BindData()
        {
            var head = (QuestionHead)Session[hddmenu.Value + "Ques_head"];

            if (hddcopy.Value == "")
            {
                txtQID.Text = head.QID;
            }
            else
            {
                txtQID.Text = "";
                txtQID.ReadOnly = false;
            }
            cboQType.SelectedValue = head.QType;
            cboQGroup.SelectedValue = head.QGroup;
            cboChoiceType.SelectedValue = head.ChoiceType;
            txtQDescription.Text = head.QDescription;
            dtDateBegin.Value = head.DateBegin;
            dtDateEnd.Value = head.DateEnd;

        }
        private void SetupControl(string statusdoc)
        {
            if (statusdoc == "NEW")
            {
                btnCopy.Visible = false;
                btnDel.Visible = false;
                btnNewLine.Visible = false;
                btnCopy.Visible = false;
            }
            if (statusdoc == "EDIT")
            {
                btnCopy.Visible = true;
                btnDel.Visible = true;
            }
        }

        private void CheckPermission()
        {

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(hddPreviouspage.Value);
        }


        #region componenet

        private bool ValidHeader()
        {
            if (cboChoiceType.SelectedValue == "")
            {
                ShowAlertLine("Input ชนิดตัวเลือกแบบสอบถาม", false);
                return false;
            }

            return true;
        }
        private bool ValidLine()
        {
            if (cboChoiceID.SelectedValue == "")
            {
                ShowPopAlert("Warning", "Select Choice ID", false, "");
                return false;
            }
            
            return true;
        }


        protected void grdLine_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Show")
            {
                
            }
        }

        private void BindGrdLine()
        {
            var data = (List<QuestionLine>)Session[hddmenu.Value + "Ques_lines"];

            if (data == null)
            {
               data = new List<QuestionLine>();
            }

            grdLine.DataSource = data;
            grdLine.DataBind();
        }

        #endregion

        #region Search area
  

        #endregion

        #region fucntion move doc helper

        private void NewHead()
        {
            QuestionHead newdata = QuestionInfoService.NewQuesHead();

            Session[hddmenu.Value + "Ques_head"] = newdata;
            BindData();
        }

        private void GotoNextLine()
        {
            QuestionHead data = new QuestionHead();
            if (hddid.Value == "")
            {
                data = QuestionInfoService.GetLastHeadData();
                if (data == null)
                {
                    return;
                }
            }
            else
            {
                data = QuestionInfoService.GetNextHeadData(hddid.Value);
            }

            if (data == null)
            {
                return;
            }
            switch (hddmenu.Value)
            {
                case "4021":
                    GoByRedirect(data.QID);
                    break;
                default:
                    break;
            }
        }
        private void GotoPreviousLine()
        {
            QuestionHead data = new QuestionHead();
            if (hddid.Value == "")
            {
                data = QuestionInfoService.GetLastHeadData();
                if (data == null)
                {
                    return;
                }
            }
            else
            {
                data = QuestionInfoService.GetPreviousHeadData(hddid.Value);
            }

            if (data == null)
            {
                return;
            }
            switch (hddmenu.Value)
            {
                case "4021":
                    GoByRedirect(data.QID);
                    break;
                default:
                    break;
            }
        }

        private void GoByRedirect(string ordid)
        {
            string myurl = "";
            myurl = "/CRM/QuestionDetail.aspx?id=" + ordid + "&menu=" + hddmenu.Value;
            Response.Redirect(myurl);
        }



        #endregion

        #region Control move line helper 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            ShowAlertMain("", true);
            var r0 = ValidData();
            if (!r0)
            {
                return;
            }
            var isnew = PrepairDataSave();

            var head = (QuestionHead)Session[hddmenu.Value + "Ques_head"];
            var line = (List<QuestionLine>)Session[hddmenu.Value + "Ques_lines"];
            if (head.QID == "")
            {
                return;
            }
            List<string> r1 = new List<string>();
            if (isnew)
            {
                r1 = QuestionInfoService.Save(head, line, "insert");
            }
            else
            {
                r1 = QuestionInfoService.Save(head, line, "update");
            }
            if (r1[0] == "R1")
            {//save successufull
                if (isnew)
                {
                    Session["save_new_success"] = true;
                    string url = "/CRM/QuestionDetail.aspx?id=" + head.QID + "&menu=" + hddmenu.Value;
                    Response.Redirect(url);
                }
                else
                {
                    LoadData();
                    ShowPopAlert("สำเร็จ", "บันทึกข้อมูลสำเร็จ", true, "");
                }

            }
            else
            {
                ShowPopAlert("Error", r1[1], false, "");
            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            var r = QuestionInfoService.Delete(hddid.Value);
            if (r[0] == "R1")
            {
                var dataNext = QuestionInfoService.GetNextHeadData(hddid.Value);
                if (dataNext != null)
                {
                    GotoNextLine();
                    return;
                }
                var dataPrev = QuestionInfoService.GetPreviousHeadData(hddid.Value);
                if (dataPrev != null)
                {
                    GotoPreviousLine();
                    return;
                }
                hddPreviouspage.Value = Session["QuestionnairList_previouspage"].ToString();
                string myurl = "/CRM/QuestionList.aspx?id=" + "" + "&menu=" + hddmenu.Value;
                Response.RedirectPermanent(myurl);
            }
            else
            {
                ShowAlertMain("Delete fail : " + r[1], false);
                return;
            }
        }

        protected void btnNewLine_Click(object sender, EventArgs e)
        {
            string myurl = "/CRM/QuestionDetail.aspx?id=" + "" + "&menu=" + hddmenu.Value;
            Response.RedirectPermanent(myurl);
        }
        protected void btnCopy_Click(object sender, EventArgs e)
        {
            string myurl = "/CRM/QuestionDetail.aspx?id=" + "" + "&menu=" + hddmenu.Value + "&copy=" + hddid.Value;
            Response.RedirectPermanent(myurl);
        }

        protected void btnBackward_Click(object sender, EventArgs e)
        {
            GotoPreviousLine();
        }

        protected void btnforward_Click(object sender, EventArgs e)
        {
            GotoNextLine();
        }
        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(hddPreviouspage.Value);
        }
        protected void cboDocLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qid = "";
            if (cboDocLine.Value != null)
            {
                qid = cboDocLine.Value.ToString();
            }
            if (qid == "")
            {
                return;
            }
            var data = QuestionInfoService.GetDataByID(qid);
            if (data == null)
            {
                return;
            }
            string myurl = "";
            switch (hddmenu.Value)
            {               
                case "4021":                    
                    myurl = "/CRM/QuestionDetail.aspx?id=" + data.QID + "&menu=" + hddmenu.Value;
                    Response.Redirect(myurl);
                    break;
                default:
                    break;
            }
        }
        protected void cboDocLine_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT [QID] FROM ( SELECT [QID], row_number()over(order by [QID] desc) as [rn]  FROM [QuestionHead]  as t where (( [QID]) LIKE @filter) and [IsActive]=1) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboDocLine_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [QID] FROM [QuestionHead] WHERE ([QID] = @ID) ORDER BY [QID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        private void ShowAlert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }

        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        #endregion

        #region Valid date
        private void ShowAlertMain(string msg, bool result)
        {
            lblInfoSave.Text = msg;
            if (result)
            {
                lblInfoSave.ForeColor = Color.Green;
            }
            else
            {
                lblInfoSave.ForeColor = Color.Red;
            }
        }
        private void ShowAlertLine(string msg, bool result)
        {
            lblInfoSave2.Text = msg;
            if (result)
            {
                lblInfoSave2.ForeColor = Color.Green;
            }
            else
            {
                lblInfoSave2.ForeColor = Color.Red;
            }
        }
        private bool ValidData()
        {
            ShowAlertMain("", true);

            if (cboChoiceType.SelectedValue == "")
            {
                ShowPopAlert("Warning", "Select ChoiceType", false, "");
                return false;
            }
            //if (string.IsNullOrEmpty(cboDocType.SelectedValue))
            //{
            //    //ShowAlertMain("Input company.", false);
            //    ShowPopAlert("Warning", "Select company", false, "");
            //    return false;
            //}


            return true;
        }


        private bool PrepairDataSave()
        {
            string user = LoginService.GetCurrentloginUser();
            bool isnewrec = false;//true =new trans / false = edit trans 
            var head = (QuestionHead)Session[hddmenu.Value + "Ques_head"];
            if (head == null)
            {
                head = QuestionInfoService.NewQuesHead();
            }
            var line = (List<QuestionLine>)Session[hddmenu.Value + "Ques_lines"];
            if (hddid.Value == "")
            {
                isnewrec = true;
                if (txtQID.Text.Trim() != "" && txtQID.Text.Trim() != "++NEW++")
                {//input qid by user
                    head.QID = txtQID.Text.Trim();
                }
                else
                {//gen q id by program
                    var comdata = CompanyInfoService.GetDataByComID("S00000");
                    head.QID = IDGeneratorServiceV2.GenNewID("QA","", true, DateTime.Now.Date, "th")[1];
                    if (head.QID == "")
                    {
                        ShowAlertMain("No ducument type found.", false);
                        return true;
                    }
                }

            }

            #region head data

            head.QType = cboQType.SelectedValue;
            head.QGroup = cboQGroup.SelectedValue;
            head.ChoiceType = cboChoiceType.SelectedValue;
            head.QDescription = txtQDescription.Text.Trim();
            head.DateBegin = dtDateBegin.Date;
            head.DateEnd = dtDateEnd.Date;            

            #endregion

            #region line data
            if (line == null)
            {
                line = new List<QuestionLine>();
            }

            #endregion

            BindData();
            Session[hddmenu.Value + "Ques_head"] = head;
            Session[hddmenu.Value + "Ques_lines"] = line;
            return isnewrec;
        }



        #endregion


        protected void btnSaveChoice_Click(object sender, EventArgs e)
        {
            #region Validate data
            var valid_result = ValidLine();
            if (!valid_result)
            {
                return;
            }
            #endregion

            var chk_ChoiceID = QuestionInfoService.GetQIDBychoiceID(hddid.Value,cboChoiceID.SelectedValue);
            if (chk_ChoiceID != null)
            {
                ShowAlert("กรุณาเลือก Choice ข้อมูลที่ไม่ซํ้ากัน", "Error");
                return;
            }

            var lines = (List<QuestionLine>)Session[hddmenu.Value + "Ques_lines"];
            if (lines == null)
            {
                lines = new List<QuestionLine>();
            }

            if (cboChoiceID.SelectedValue != "")
            {
                foreach (var liness in lines)
                {
                    if (liness.ChoiceID == cboChoiceID.SelectedValue)
                    {
                        ShowAlert("กรุณาเลือก Choice ข้อมูลที่ไม่ซํ้ากัน", "Error");
                        return;
                    }
                    
                }

                
            }            
           

            QuestionLine Ques_line = new QuestionLine();

            Ques_line.ChoiceID = cboChoiceID.SelectedValue;
            Ques_line.ChoiceDescription = txtChoiceDescription.Text.Trim();

            lines.Add(Ques_line);
            Session[hddmenu.Value + "Ques_lines"] = lines;
            cboChoiceID.SelectedIndex = 0;
            txtChoiceDescription.Text = "";
            BindGrdLine();
        }
    }

}