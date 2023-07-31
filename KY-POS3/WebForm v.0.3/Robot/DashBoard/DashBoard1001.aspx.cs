using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class DashBoard1001 : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
   //  var r=       EmailServer.SendMail("Notify", "taramon.y@jtgoodapp.com", "Love Love", "Test mail นะคะ", true);

        }
        protected void btnShow_Click(object sender, EventArgs e) {

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "ShowPopupItem();", true);
        }

        protected void btnAlert_Click(object sender, EventArgs e) {
            SaveAlert("ตรวจสอบข้อมูล!!", "SHIT", "info");
        }
        private void SaveAlert(string inPutTopic, string inPutMsg, string inPutType) {
            string callFuction = string.Format("ServerAlert('{0}','{1}','{2}');", inPutTopic, inPutMsg, inPutType);
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", callFuction, true);
        }
    }
}