using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Feature {
    public partial class MyQrScan : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                if (Session["QRSCAN_previouspage"] != null) {
                    hddPreviouspage.Value = Session["QRSCAN_previouspage"].ToString();                 
                }
                hddid.Value = Request.QueryString["id"];
                hddmenu.Value = Request.QueryString["menu"];
                hddFileType.Value = Request.QueryString["filetype"];
              
            }
        }
        protected void btnBack_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(hddPreviouspage.Value);
        }

    }
}