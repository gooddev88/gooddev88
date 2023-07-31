using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Communication.Misc {
    public partial class ErrorPage : System.Web.UI.Page {


        public static string msg { get { return HttpContext.Current.Session["successpage_msg"] == null ? "" : (string)HttpContext.Current.Session["successpage_msg"]; } set { HttpContext.Current.Session["successpage_msg"] = value; } }
         


        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "เกิดเหตุไม่ปกติ กรุณาแจ้งโทรแจ้งเจ้าหน้าที่ค่ะ";
                }
            }
            lblMsg.Text = msg;

        }
    }
}