using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Sample {
    public partial class Loading : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void Button1_Click(object sender, EventArgs e) {
            System.Threading.Thread.Sleep(5000);
            lblR.Text = "FUCK YOUUUUUU";

        }
    }
}