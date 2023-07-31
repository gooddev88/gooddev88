using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot {
    public partial class POSA1 : System.Web.UI.MasterPage {
        public string ShowCaption { get { return lblCaption.Text; } set { lblCaption.Text = value; } }
        protected void Page_Load(object sender, EventArgs e) {

            //if (Session["current_company"] != null) {
            //    hddCompany.Value = Session["current_company"].ToString();
            //}
            //if (Session["current_backgroup"] != null) {
            //    hddbackgroupid.Value = Session["current_backgroup"].ToString();
            //}
             
        }
        public string ShowCompany() {
            return LoginService.LoginInfo.CurrentCompany.Name1+" "+LoginService.LoginInfo.CurrentCompany.Name2;

        }
        public string ShowUsername() {
            return LoginService.LoginInfo.CurrentUser;
        }
        public string SetMenu(object menu_id) {
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null) {
                result = data.IsOpen.ToString();
            }
            return result;
        }

        public string SetMenuDashBoard(object boardid) {
            string result = "false";
            //var right = LoginService.GetCurrentDashBoardAccess(boardid.ToString());
           
            //    result = right.ToString().ToLower();
           
            return result;
        }
        public string ShowMainMenuDesc(object menuid) {
            return PermissionService.GetMenuInfo(menuid.ToString()).Name;
        }
        public string ShowSubMenuDesc(object menuid) {
            return PermissionService.GetMenuInfo(menuid.ToString()).MenuID;
        }
        public string GetUserLogin() {
            return LoginService.LoginInfo.CurrentUser;
        }

        public string SetCaption(object menu_id) {

       
            string result = "";
            string menuid = menu_id.ToString();
            switch (menuid) {
                case "5000":
                    result = "POS";
                    break;
               
            }
            return result;

        }

          
    }
}