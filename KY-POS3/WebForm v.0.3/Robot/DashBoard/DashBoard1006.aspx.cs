using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class DashBoard1006 : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                LoadDisplay();
            }
        }
        private void LoadDisplay() {
            string id = Request.QueryString["id"];
            var boardinfo = DashBoradService.GetBoardByID(id);

            lblTopic.Text = boardinfo.Name;

            //var d1 = TaskService.GetCount("D1");
            //lblD1.Text = d1.ToString("N0") + " DP/WH";
            //var d2 = TaskService.GetCount("D2");
            //lblD2.Text = d2.ToString("N0") + " รายการ";
            //var d3 = TaskService.GetCount("D3");
            //lblD3.Text = d3.ToString("N0") + " ปลายทาง";
        }

        protected void btnDropPoint_Click(object sender, EventArgs e) {
            Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            //   Task / TaskBList.aspx ? menu = 3112 & id = "";
            string myurl = "/Task/TaskBList.aspx?menu=3112-1";
            Response.RedirectPermanent(myurl);
        }

        protected void btnWarehouse_Click(object sender, EventArgs e) {
            Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Task/TaskBList.aspx?menu=3112-2";
            Response.RedirectPermanent(myurl);
        }
        protected void btnDestination_Click(object sender, EventArgs e) {
            Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Task/TaskBList.aspx?menu=3112-3";
            Response.RedirectPermanent(myurl);
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
    }
}