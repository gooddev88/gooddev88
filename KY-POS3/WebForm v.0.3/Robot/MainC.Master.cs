using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot {
    public partial class MainC : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
        
            hddCompany.Value = Session["current_company"].ToString();
            hddbackgroupid.Value = Session["current_backgroup"].ToString();
            if (!IsPostBack) {
      
            }
        }
      
        public string SetMenu(object menu_id) {
            menu_id = "MENU" + menu_id;
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null) {
                result = data.IsOpen.ToString();
            }

            return result;
        }
    }
}