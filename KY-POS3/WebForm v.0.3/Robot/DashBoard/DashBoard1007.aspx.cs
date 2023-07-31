using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class DashBoard1007 : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                LoadDisplay();
            }
        }
        private void LoadDisplay() {
            string id = Request.QueryString["id"];
            var boardinfo = DashBoradService.GetBoardByID(id);

            lblTopic.Text = boardinfo.Name;

      //      var count_ord_today = DashBoradService.Get_CountTodayOrder();
      //      btnListOrder.Text = count_ord_today.ToString("N0") + " ออเดอร์วันนี้";
      //var count_packing_pending = DashBoradService.Get_CountOrderPedningPackingInShop();
      
      //      btnListPacking.Text = count_packing_pending.ToString("N0") + " ออเดอร์รอส่ง";
        }

        protected void btnNewOrder_Click(object sender, EventArgs e) {

            Session["ORDER_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            //   Task / TaskBList.aspx ? menu = 3112 & id = "";
            string myurl = "/Order/Menu2001.aspx";
            Response.RedirectPermanent(myurl);
        }


        protected void btnNewPacking_Click(object sender, EventArgs e) {
            Session["PACKING_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Order/PackingDetail?id=&menu=3021&doctype=PK1";
            Response.RedirectPermanent(myurl);
        }
        protected void btnListOrder_Click(object sender, EventArgs e) {
         //   Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Order/MyOrderList?id=&menu=2001";
            Response.RedirectPermanent(myurl);
        }

        protected void btnListPacking_Click(object sender, EventArgs e) {
            //Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Order/PackingList?menu=3021&id=";
            Response.RedirectPermanent(myurl);
        }
   protected void btnShowComm_Click(object sender, EventArgs e) {
            //Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Order/Report/Rcommission001.aspx?menu=8102&id=";
            Response.RedirectPermanent(myurl);
        }
   protected void btnTracking_Click(object sender, EventArgs e) {
            //Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Tracking/CustTrackingV2.aspx?menu=v&id=";
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