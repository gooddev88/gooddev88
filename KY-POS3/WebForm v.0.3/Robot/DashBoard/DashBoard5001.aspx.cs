using Robot.Data.DataAccess;
using Robot.Data.ServiceHelper;
using Robot.Helper;
using Robot.MAINMAS;
using Robot.Master;
using Robot.POS;
using Robot.POSC;
using Robot.POSC.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class DashBoard5001 : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                LoadDisplay();
            }
        }
        public string SetMenu(object menu_id) {
            string result = "0";
            //if (menu_id.ToString() == "4111") {
            //    var xxx = "";
            //    }
            //var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            //if (data != null) {
            //    result = data.IsOpen.ToString();

            //}
            //return result;
            
            return Convert.ToInt32(PermissionService.CanOpen(menu_id.ToString())).ToString();
        }
        private void LoadDisplay() {
            string id = Request.QueryString["id"];
            if (string.IsNullOrEmpty(id)) {
                id = "B5001";
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

        public string ShowMainMenuDesc(object menuid)
        {
            return PermissionService.GetMenuInfo(menuid.ToString()).Name;
        }
        public string ShowMainMenuDesc2(object menuid) {
            return PermissionService.GetMenuInfo(menuid.ToString()).Desc2;
        }
        public string ShowSubMenuDesc(object menuid)
        {
            return PermissionService.GetMenuInfo(menuid.ToString()).MenuID;
        }


        protected void btnPOSV1_Click(object sender, EventArgs e)
        {
            POSSaleDetail. PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/POSC/POSSaleNewDoc?menu=5002";
            Response.RedirectPermanent(myurl);
        }

        protected void btnPOSV2_Click(object sender, EventArgs e)
        {
            string rooturl = URLHelper.GetBaseUrlNoAppPath();
           // string subapp = "/POSSYSALE/POS/POSSaleNewDoc/";
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("POS/POSSaleNewDoc/x999/web1");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);


           
        }

        protected void btnCheckBillList_Click(object sender, EventArgs e)
        {
            POSSaleDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/POSC/POSBillList?menu=5100"; ;
            Response.RedirectPermanent(myurl);
        }


        protected void btnItemV2_Click(object sender, EventArgs e)
        {
            //ItemList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string url = $"~/Master/ItemList?menu=5901";
            //Response.Redirect(url); 

            string rooturl = URLHelper.GetBaseUrlNoAppPath(); 
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("Master/ItemDetail/web1");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);
        }

        protected void btnItemPrice_Click(object sender, EventArgs e)
        {
            //ItemPriceList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string url = $"~/Master/ItemPriceList?menu=9412";
            //Response.Redirect(url);
            string rooturl = URLHelper.GetBaseUrlNoAppPath();
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("Master/ItemPriceList/web1");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);
        }

        protected void btnCompany_Click(object sender, EventArgs e)
        {
            //CompanyList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string url = $"~/Master/CompanyList?menu=9201";
            //Response.Redirect(url);


            string rooturl = URLHelper.GetBaseUrlNoAppPath();
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("Master/CompanyList/web1");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);
        }

        protected void btnPOSBOM_Click(object sender, EventArgs e)
        {
            POSBOMList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/POS/POSBOMList?menu=3001";
            Response.Redirect(url);
        }

        protected void btnPOSORD_Click(object sender, EventArgs e)
        {
            string url = "";
            POSORDERList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            //POSSaleService.IsRunOnMobile = MobileHelper.isMobileBrowser();
            if (MobileHelper.isMobileBrowser()) {
                url = $"~/POS/POSORDERMobileList?menu=2001";
            }
            else
            {
                //url = $"../POS/POSORDERMobileList?menu=2001";
                url = $"~/POS/POSORDERList?menu=2001";
            }
            Response.Redirect(url);
        }

        protected void btnPOSORD_ACCEP_Click(object sender, EventArgs e)
        {
            string url = "";
            POSORDERList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            //POSSaleService.IsRunOnMobile = MobileHelper.isMobileBrowser();
            if (MobileHelper.isMobileBrowser()) {
                url = $"~/POS/POSORDERMobileList?menu=2002";
            }
            else
            {
                url = $"~/POS/POSORDERList?menu=2002";
            }
            Response.Redirect(url);
        }

        protected void btnPOSORDShip_Click(object sender, EventArgs e)
        {
            string url = "";
            POSORDERList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            if (MobileHelper.isMobileBrowser()) {
                url = $"~/POS/POSORDERMobileList?menu=2003";
            }
            else
            {
                url = $"~/POS/POSORDERList?menu=2003";
            }
            Response.Redirect(url);
        }

        protected void btnPOSORDGR_Click(object sender, EventArgs e)
        {
            string url = "";
            POSORDERList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            if (MobileHelper.isMobileBrowser()) {
                url = $"~/POS/POSORDERMobileList?menu=2004";
            }
            else
            {
                url = $"~/POS/POSORDERList?menu=2004";
            }
            Response.Redirect(url);
        }

        protected void btnPOSPO_Click(object sender, EventArgs e)
        {
            POSPOList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/POS/POSPOList?menu=2101";
            Response.Redirect(url);
        }

        protected void btnOInvoice_Click(object sender, EventArgs e)
        {
            INVList.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/POS/INVList?menu=4111";
            Response.RedirectPermanent(myurl);
        }

        protected void btnOReceipt_Click(object sender, EventArgs e)
        {
            RCList.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/POS/RCList?menu=4112";
            Response.RedirectPermanent(myurl);
        }

        protected void btnPOSStkAdjust_Click(object sender, EventArgs e)
        {
            POSStkAdjustList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/POS/POSStkAdjustList?menu=6011";
            Response.Redirect(url);
        }        

        protected void btnStockBalance_Click(object sender, EventArgs e)
        {
            StockBalanceList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/POS/StockBalanceList?menu=8001";
            Response.Redirect(url);
        }

        protected void btnStockMovement_Click(object sender, EventArgs e)
        {
            StockMovementList.ITEMID = "";
            StockMovementList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/POS/StockMovementList?menu=8002";
            Response.Redirect(url);
        }

        protected void btnPOSORDERRM_Click(object sender, EventArgs e)
        {
            RPT_POSORDER_RMList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/POS/RPT_POSORDER_RMList?menu=5903";
            Response.Redirect(url);
        }

        protected void btnAPPV2_Click(object sender, EventArgs e)
        {
            string rooturl = URLHelper.GetBaseUrlNoAppPath();
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("Menu/MainMenu");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);
        }

        protected void btnMyUserInfo_Click(object sender, EventArgs e)
        {
            //MyUserList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string myurl = "~/MAINMAS/MyUserList?menu=9002";
            //Response.RedirectPermanent(myurl);

            string rooturl = URLHelper.GetBaseUrlNoAppPath();
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("User/UserList/web1");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);
        }

        protected void btnMyUserGroupInfo_Click(object sender, EventArgs e)
        {
            //MyUserGroupList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string myurl = "~/MAINMAS/MyUserGroupList?menu=9003";
            //Response.RedirectPermanent(myurl);
            string rooturl = URLHelper.GetBaseUrlNoAppPath();
            string subapp = "/SALE/LoginFromApp/";
            var req = LoginService.CreateUserCrossAppReq("User/UserGroupList/web1");
            string url = rooturl + subapp + req.ReqID;
            Response.Redirect(url);
        }

        protected void btnVendor_Click(object sender, EventArgs e)
        {
            VendorList.PreviousListPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/Master/VendorList?menu=9017";
            Response.Redirect(url);
        }

        protected void btnMasterTypeInfo_Click(object sender, EventArgs e)
        {
            MasterTypeList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/MAINMAS/MasterTypeList?menu=9911";
            Response.RedirectPermanent(myurl);
        }

        protected void btnRawBT_Click(object sender, EventArgs e) {
        
            string myurl = @"https://dl.jtgoodapp.com/codeyum/rawbt3_9.rar";

            Response.RedirectPermanent(myurl);
        }
        protected void btnAppDownLoad_Click(object sender, EventArgs e) {

            var url = LinkService.GetLinkByLinkName("download_app"); 
            Response.Redirect(url);
        }
        protected void btnBilHistory_Click(object sender, EventArgs e) {
            POSInvoiceList.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/POSC/POSInvoiceList?menu={"5004"}";
            Response.Redirect(myurl);
        }

        protected void btnPOSReport_Click(object sender, EventArgs e) {
            POSReports.PreviousPage= HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/POSC/POSReports?menu=5902";
            Response.Redirect(myurl);
        }

        protected void btnDeletePermanace_Click(object sender, EventArgs e) {
            POSSaleDeletePermance.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "~/POSC/POSSaleDeletePermance?menu=5028";
            Response.Redirect(myurl);
        }

        protected void btnMenuLine_Click(object sender, EventArgs e)
        {
            MenuLineData.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            Response.RedirectPermanent("MenuLineData");
        }

    }
}