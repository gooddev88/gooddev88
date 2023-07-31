﻿using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Account
{
    public partial class SiteA : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                imgLogo.ImageUrl = LoginService.LoginInfo.AppLogoImage;
            }

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
            //        return LoginService.LoginInfo.CurrentCompanyName;
            return "";

        }
        public string SetCaption(object menu_id) {

           
            string result = "";
            return result;

        }

    }
}