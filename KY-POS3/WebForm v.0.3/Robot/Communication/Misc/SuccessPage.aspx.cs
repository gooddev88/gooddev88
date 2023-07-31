using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Communication.Misc {
    public partial class SuccessPage : System.Web.UI.Page {
        public static string msg { get { return HttpContext.Current.Session["successpage_msg"] == null ? "" : (string)HttpContext.Current.Session["successpage_msg"]; } set { HttpContext.Current.Session["successpage_msg"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "ส่งคำขอสำเร็จค่ะ รอเจ้าหน้าที่พิจารณาอนุมัตินะคะ";
                }
            }
            lblMsg.Text = msg;
        }
    }
}