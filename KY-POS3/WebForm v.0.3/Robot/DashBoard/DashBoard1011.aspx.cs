using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class DashBoard1011 : MyBasePage {
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
                id = "B1011";
            }
            var boardinfo = DashBoradService.GetBoardByID(id);

            lblTopic.Text = boardinfo.Name;
        }
        protected void btnCreateOrder_Click(object sender, EventArgs e) {
            Session["ORDER_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "../Order/Menu2000?id=&menu=2001";
            Response.RedirectPermanent(myurl);
        }
        protected void btnOrderStatus_Click(object sender, EventArgs e) {
            //Session["ORDER_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "../Order/MyOrderStatusListB.aspx?id=&menu=2141";
            Response.RedirectPermanent(myurl);
        }
        protected void btnPacking1Ganreator_Click(object sender, EventArgs e) {
            Session["Packing1Ganreator_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/AdminJob/PK1Generator.aspx?menu=5111&id=";
            Response.RedirectPermanent(myurl);
        }


        protected void btnTKI211InPending_Click(object sender, EventArgs e) {
            Session["TKI211InPending_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/AdminJob/TKI211InPendingList.aspx?menu=5122&id=";
            Response.RedirectPermanent(myurl);
        }
        protected void TKI211InPK3_Click(object sender, EventArgs e) {
            Session["TKI211InPK3_Click_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/AdminJob/TKI211InPK3.aspx";
            Response.RedirectPermanent(myurl);
        }
        protected void btnTKI221InPending_Click(object sender, EventArgs e) {
            Session["TKI221InPending_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/AdminJob/TKI221InPendingList.aspx?menu=5123&id=";
            Response.RedirectPermanent(myurl);
        }

        protected void PK3Ganreator_Click(object sender, EventArgs e) {
            Session["PK3Ganreator_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/AdminJob/PK3Generator.aspx";
            Response.RedirectPermanent(myurl);
        }



        protected void PK3EditGenerator_Click(object sender, EventArgs e) {
            Session["PK3EditGenerator_Click_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/AdminJob/PK3EditGenerator.aspx";
            Response.RedirectPermanent(myurl);
        }

        protected void btnListPackingLM_Click(object sender, EventArgs e) {
            //Session["TASK_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/Order/PackingList?menu=3041&id=";
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
        //รับพัสดุ
        protected void TKI901InPendingList_click(object sender, EventArgs e) {
            Session["TKI901InPendingList_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/AdminJob/TKI901InPendingList.aspx";

            Response.RedirectPermanent(myurl);
        }

   protected void btnDeliveryHead_Click(object sender, EventArgs e) {
            Session["POHeadList_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../PO/DlvHeadList.aspx?menu=1201";
            Response.RedirectPermanent(myurl);
        }
        protected void btnDeliveryTracking_Click(object sender, EventArgs e) {
       Session["DlvPackagingList_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../PO/DlvPackagingList.aspx?menu=1211";
            Response.RedirectPermanent(myurl);
        }

    protected void btnPOTracking_Click(object sender, EventArgs e) {
            Session["DlvPOLineGRList_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../PO/DlvPOLineGRList.aspx?menu=1111";
            Response.RedirectPermanent(myurl);
        }

        protected void btnLicense_Click(object sender, EventArgs e) {
   Session["LicneseList_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../PO/DlvLicenselist.aspx?menu=1251";
            Response.RedirectPermanent(myurl);
        }

        protected void btnPRList_Click(object sender, EventArgs e) {
            Session["PR_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../PO/PRHeadList.aspx?menu=1101";
            Response.RedirectPermanent(myurl);
        }
        protected void btnPOList_Click(object sender, EventArgs e) {
            Session["POHeadList_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "";
            myurl = "../PO/POHeadList.aspx?menu=1103";
            Response.RedirectPermanent(myurl);
        }


    }
}