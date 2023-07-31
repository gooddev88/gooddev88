using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;
using static Robot.Data.BL.I_Result;

namespace Robot.Master
{
    public partial class CopyPermission : MyBasePage {
        #region Global var

        
        public static string FromRoleID { get { return HttpContext.Current.Session["fromroleid"]==null?"":(string)HttpContext.Current.Session["fromroleid"]; } set { HttpContext.Current.Session["fromroleid"] = value; } }
        public static string PreviousePage { get { return HttpContext.Current.Session["copyper_previousage"]==null?"":(string)HttpContext.Current.Session["copyper_previousage"]; } set { HttpContext.Current.Session["copyper_previousage"] = value; } }
        public static string RoleType { get { return HttpContext.Current.Session["copyroletype"]==null?"":(string)HttpContext.Current.Session["copyroletype"]; } set { HttpContext.Current.Session["copyroletype"] = value; } }
        public static string CopyType { get { return HttpContext.Current.Session["copytype"] == null ? "" : (string)HttpContext.Current.Session["copytype"]; } set { HttpContext.Current.Session["copytype"] = value; } }
        public static I_BasicResult OutputAction { get { return HttpContext.Current.Session["copyroleoutputaction"] == null ?  new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" } : (I_BasicResult)HttpContext.Current.Session["copyroleoutputaction"]; } set { HttpContext.Current.Session["copyroleoutputaction"] = value; } }

        #endregion

        protected void Page_Load(object sender, EventArgs e) {
  
           
            if (!Page.IsPostBack) {
                CloseAlert(); 
          
                LoadData();
            }
        }
       

        private void LoadData() {
            SetActiveControl();


        }
 private void SetActiveControl() {
            lblInfo.Text = $"{FromRoleID} - {CopyType}  ";
            if (RoleType == "user") {
             
                divRoleGroup.Visible = false;
                divroleUser.Visible = true; 
            }
            if (RoleType == "group") {
            ;
                divRoleGroup.Visible = true;
                divroleUser.Visible = false; 
            }
           
        }

      


        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", $"ShowAlert('{msg}','{type}');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

       
        protected void btnList_Click(object sender, EventArgs e) {

        }
        public List<UserInfo> LoadRoleUser(string fromUser) {
            List<UserInfo> rusult = new List<UserInfo>();
            using (GAEntities db =new GAEntities()) {
                rusult = db.UserInfo.Where(o => o.IsActive && o.IsProgramUser && o.Username!=fromUser).OrderBy(o=>o.Username) .ToList();
            }
            return rusult;
        }
        public List<UserGroupInfo> LoadRoleGroup(string fromGroup) {
            List<UserGroupInfo> rusult = new List<UserGroupInfo>();
            using (GAEntities db = new GAEntities()) {
                rusult = db.UserGroupInfo.Where(o => o.IsActive && o.UserGroupID != fromGroup).OrderBy(o => o.Sort).ToList();
            }
            return rusult;
        }


        private bool ValidData() { 
            bool result = true;
            if (RoleType=="user") {
                if (cboRoleUserSource.Value==null) {
                    ShowAlert("เลือกผู้ใช้งาน ที่ต้องการคัดลอกสิทธิ์ ", "Error");
                    return false;
                }
            }
            if (RoleType == "group") {
                if (cboRoleGroupSource.Value == null) {
                    ShowAlert("เลือกกลุ่มผู้ใช้งาน ที่ต้องการคัดลอกสิทธิ์ ", "Error");
                    return false;
                }
            }
         

            return result;
        }

        protected void btnOk_Click(object sender, EventArgs e) {
          
            if (!ValidData()) {
                return;
            }
            try {
                OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
                using (GAEntities db =new GAEntities()) {
                    if (CopyType == "company")
                    {//copy company
                        string source = "";
                        if (RoleType == "user")
                        {
                            source = cboRoleUserSource.Value.ToString();
                            //db.SP_CopyCompany(source, FromRoleID, "user");
                        }
                        if (RoleType == "group")
                        {
                            source = cboRoleGroupSource.Value.ToString();
                            //db.SP_CopyCompany(source, FromRoleID, "group");
                        }
                    }
                    if (CopyType == "menu")
                    {//copy menu
                        string source = "";
                        if (RoleType == "user")
                        {
                            source = cboRoleUserSource.Value.ToString();
                            //db.SP_CopyPermission(source, FromRoleID, "user");
                        }
                        if (RoleType == "group")
                        {
                            source = cboRoleGroupSource.Value.ToString();
                            //db.SP_CopyPermission(source, FromRoleID, "group");
                        }
                    }

                } 
                Response.RedirectPermanent(PreviousePage);
            } catch (Exception ex) {
               
                string err = ex.Message;
                if (ex.InnerException!=null) {
                    err = err + " " + ex.InnerException;
                }
                OutputAction.Result = "fail";
                OutputAction.Message1 = err;
                ShowAlert(err, "Error");
            }
        
        }

      



        protected void btnBack_Click(object sender, EventArgs e) {

            Response.Redirect(PreviousePage);
        }
      

    }
}