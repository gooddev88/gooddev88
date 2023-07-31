using Robot.Communication.Line;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard
{
    public partial class MenuLineData : MyBasePage
    {
        public static string PreviousPage { get { return HttpContext.Current.Session["MenuLineBSData_prevoiuspage"] == null ? "" : (string)HttpContext.Current.Session["MenuLineBSData_prevoiuspage"]; } set { HttpContext.Current.Session["MenuLineBSData_prevoiuspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        public string SetMenu(object menu_id) {
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null) {
                result = data.IsOpen.ToString();
            }
            return result;
        }

        public string ShowMainMenuDesc(object menuid)
        {
            return PermissionService.GetMenuInfo(menuid.ToString()).Name;
        }
        public string ShowSubMenuDesc(object menuid)
        {
            return PermissionService.GetMenuInfo(menuid.ToString()).MenuID;
        }

        private void LoadData()
        {
            BindData();
        }
        private void BindData() {
            lblTopic.Text = "ข้อมูล เมนู LINE";
        }
    

        protected void btnLineLoginReq_Click(object sender, EventArgs e) {
            LineLogInRequestList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "../Communication/Line/LineLogInRequestList";
            Response.RedirectPermanent(myurl);
        }

        protected void btnLineLogin_Click(object sender, EventArgs e) {
            LineLogInList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "../Communication/Line/LineLogInList";
            Response.RedirectPermanent(myurl);
        }


        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }

    }
}