using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class SiteC : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {


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
        public string ShowCompany() {
            return LoginService.LoginInfo.CurrentCompany.Name1 + " " + LoginService.LoginInfo.CurrentCompany.Name2;

        }
        public string SetCaption(object menu_id) {

           
            string result = "";
            string menuid = menu_id.ToString();
            switch (menuid) {
                case "5000":
                    result = "POS";
                    break;
                case "3000":
                    result = "Finance";
                    break;
                default:
                    result = "xxxx";
                    break;
            }
            return result;

        }

    }
}