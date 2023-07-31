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
using static Robot.Data.BL.I_Result;

namespace Robot.Account.MyLogin
{
    public partial class ChangeRCompany : MyBasePage
    {
        public static string PreviousPage { get { return HttpContext.Current.Session["changecom_previouspage"] == null ? "" : (string)HttpContext.Current.Session["changecom_previouspage"]; } set { HttpContext.Current.Session["changecom_previouspage"] = value; } }
        public static List<vw_UserInRCom> ComList { get { return HttpContext.Current.Session["changecom_list"] == null ? new List<vw_UserInRCom>() : (List<vw_UserInRCom>)HttpContext.Current.Session["changecom_list"]; } set { HttpContext.Current.Session["changecom_list"] = value; } }

        protected void Page_Load(object sender, EventArgs e)  {
            SetQueryString();
            LoadDevDropDownList();       
            if(!Page.IsPostBack) {
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString()
        {
            hddmenu.Value = Request.QueryString["menu"];
        }
        private void LoadData()
        {
            GridBinding();
        }

        private void ShowAlert(string msg, string type) {
            lblAlertMsg.Text = msg;
            msg = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }


        private void GridBinding() {
         
            if (hddmenu.Value=="rcom") {
                var user = LoginService.LoginInfo.CurrentUser;
                ComList = CompanyService.ListRComByUser(user);
            }
         
            grdlist.DataSource = ComList;
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
                string comId = e.CommandArgument.ToString(); // value from CommandArgument 
                //I_BasicResult rr = new I_BasicResult();
                if (hddmenu.Value == "rcom") {
                    LoginService.LoginInfo = LogInCenterService.Login(LoginService.LoginInfo.CurrentUser, "silent", comId);
                    if (LoginService.LoginInfo.LoginResult == "fail") {
                        ShowAlert(LoginService.LoginInfo.LoginResultInfo, "Error");
                        return;
                    }
                  
                }
                if (hddmenu.Value == "com") {
                var    rr = LoginService.ChangeCurrentCompany(comId);
                    if (rr.Result == "fail") {
                        ShowAlert(rr.Message1, "Error");
                        return;
                    }  
                }

                if (!string.IsNullOrEmpty(PreviousPage)) {
                    Response.Redirect(PreviousPage);
                } else {
                    var b = LoginService.LoginInfo.CurrentBoard;
                    if (b != null) {
                        string url = $"../../DashBoard/{b.BoardPage}";
                        Response.Redirect(url);
                    }
                }


            }
        }

        private void SetCaptionByMenu()
        {
        }

        private void LoadDropDownList()
        {

        }

        private void LoadDevDropDownList()
        {

        }

     
        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private void ClosePopup(string command)
        {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PreviousPage))
            {
                Response.Redirect(PreviousPage);
            }
            else
            {
                var b = LoginService.LoginInfo.CurrentBoard;
                if (b != null)
                {
                    string url = $"../../DashBoard/{b.BoardPage}";
                    Response.Redirect(url);
                }
            }
        }
    }
}