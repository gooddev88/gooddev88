using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Account.Authen.DA;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;

namespace Robot.Account.MyLogin
{
    public partial class MyLogInRCompany : MyBasePage {
        public static List<vw_UserInRCom> DocList { get { return (List<vw_UserInRCom>)HttpContext.Current.Session["login_rcom"]; } set { HttpContext.Current.Session["login_rcom"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDevDropDownList();
            if (!Page.IsPostBack) {       
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"]; 
        }
        private void LoadData() {        
            
            DocList = CompanyService.ListRComByUser(LoginService.LoginInfo.CurrentUser);
            if (DocList.Count == 1)
            {
                Response.Redirect("~/Default.aspx");
            }
            GridBinding();
        }

        private void GridBinding()
        {
            grdlist.DataSource = DocList;
            grdlist.DataBind();
        }

        protected void grdlist_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (grdlist.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }

        protected void grdlist_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "selrow")
            {
                string rcom = e.CommandArgument.ToString();
                GoToDashBoard(rcom);
            }
        }
        private void GoToDashBoard(string rcom)
        {
            var rcomInfo = CompanyService.GetRCompanyInfo(rcom);
            //LoginService.LoginInfo.CurrentRootCompany = comInfo;
            //LoginService. LoginInfo.CurrentCompany = CompanyService.GetCompanyInfo(LoginService.LoginInfo.UserInCompany.FirstOrDefault());
            var user = LoginService.LoginInfo.CurrentUser;
            LoginService.LoginInfo = LoginService.LoginInfo = LogInCenterService.Login(user, "silent", rcomInfo.CompanyID);
            // LoginService.LoginInfo.CurrentRootCompanyName = comInfo.RComName; 
            Response.Redirect("~/Default.aspx");
        }

        private void LoadDropDownList() {

        }

        private void LoadDevDropDownList() {

        }

        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        

    }
}