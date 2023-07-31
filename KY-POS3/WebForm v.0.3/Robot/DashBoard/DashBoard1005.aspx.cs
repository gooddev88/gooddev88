using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class DashBoard1005 : MyBasePage {
        //board finance
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
                LoadDisplay();
            }
        }
        public string SetMenu(object menu_id)
        {
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null)
            {
                result = data.IsOpen.ToString();
            }
            return result;
        }
        private void LoadDisplay()
        {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) {
                id = "B1005";
            }
            var boardinfo = DashBoradService.GetBoardByID(id);

            lblTopic.Text = boardinfo.Name;
        }

        protected void btnStatatmentImport_Click(object sender, EventArgs e)
        {
            Session["StatementUpload_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../FIN/StatementUpload.aspx?menu=3301";
            Response.RedirectPermanent(myurl);
        }
        protected void btnStmFinMemo_Click(object sender, EventArgs e)
        {
            Session["StatementEdit_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../FIN/StatementEditA.aspx?menu=3302";
            Response.RedirectPermanent(myurl);
        }
   protected void btnStmFinSupMemo_Click(object sender, EventArgs e)
        {
            Session["StatementEdit_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../FIN/StatementEditA.aspx?menu=3303";
            Response.RedirectPermanent(myurl);
        }

    protected void btnStmAccMemo_Click(object sender, EventArgs e)
        {
            Session["StatementEdit_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../FIN/StatementEditA.aspx?menu=3304";
            Response.RedirectPermanent(myurl);
        }
        protected void btnStmAccSupMemo_Click(object sender, EventArgs e)
        {
            Session["StatementEdit_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../FIN/StatementEditA.aspx?menu=3305";
            Response.RedirectPermanent(myurl);
        }
        protected void btnStmAll_Click(object sender, EventArgs e)
        {
            Session["StatementEdit_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../FIN/StatementEditA.aspx?menu=3310";
            Response.RedirectPermanent(myurl);
        }
        protected void btnNextBoard_click(object sender, EventArgs e)
        {
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