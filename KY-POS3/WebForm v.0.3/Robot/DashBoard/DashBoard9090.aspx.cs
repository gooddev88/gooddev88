using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class DashBoard9090 : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                LoadDisplay();
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
        private void LoadDisplay() {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) {
                id = "B5011";
            }
            var boardinfo = DashBoradService.GetBoardByID(id);

            lblTopic.Text = boardinfo.Name;
        }


        protected void btnNextBoard_click(object sender, EventArgs e) {
            DashBoradService.GetNextBoard();
            var b = LoginService.LoginInfo.CurrentBoard;
            if (b != null)
            {
                string url = $"../DashBoard/{b.BoardPage}";
                Response.RedirectPermanent(url);
            }
        }

      

        protected void btnMailGroup_Click(object sender, EventArgs e) {
            Session["MailGroupDetail_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/DocStep/MailGroupDetail.aspx?id=" + "" + "&menu=9211";
            Response.RedirectPermanent(myurl);
        }

        protected void btnSetMailGroupInDocstep_Click(object sender, EventArgs e) {
            Session["SetMailGroupInDocstep_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/DocStep/SetMailGroupInDocStep.aspx?id=" + "" + "&menu=9202";
            Response.RedirectPermanent(myurl);
        }
    }
}