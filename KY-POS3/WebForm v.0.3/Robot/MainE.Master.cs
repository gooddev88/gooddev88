using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot {
    public partial class MainE : MasterPage {
        protected void Page_Load(object sender, EventArgs e) {

            if (Session["current_company"] != null) {
                hddCompany.Value = Session["current_company"].ToString();
            }
            if (Session["current_backgroup"] != null) {
                hddbackgroupid.Value = Session["current_backgroup"].ToString();
            } 
        } 
    }
}