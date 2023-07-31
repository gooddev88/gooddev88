using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot {
    public partial class SiteMaster : MasterPage {
        protected void Page_Load(object sender, EventArgs e) {

            if (Session["current_company"] != null) {
                hddCompany.Value = Session["current_company"].ToString();
            }
            if (Session["current_backgroup"] != null) {
                hddbackgroupid.Value = Session["current_backgroup"].ToString();
            }
            if (!IsPostBack) {
                ShowHiddenbutton();
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
        public string SetCaption(object menu_id) {

            //string result       = LanguageService.GetValue(menu_id.ToString());
            //return result;
            string result = "";
            string menuid = menu_id.ToString();
            switch (menuid) {
                case "1000":
                    result= "Purchase";
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

        protected void btnhiddenmenu_Click(object sender, EventArgs e) {
            string myurl = "/DashBoard/Menu6666.aspx";
            Response.RedirectPermanent(myurl);
        }
        private void ShowHiddenbutton() {
            if (Session["userlogin"] != null) {
                if (LoginService.LoginInfo.CurrentUser == "tammon.y") {
                    btnhiddenmenu.Visible = true;
                }
            }

        }


    }
}