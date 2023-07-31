using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard
{
    public partial class Menu1001 : MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {

        }

        protected void btnTruck_postcode_Click(object sender, EventArgs e)
        {
            string myurl = "";
            myurl = "../Order/Report/truckinpostcode.aspx?menu=";
            Response.RedirectPermanent(myurl);
        }

        protected void btn_UserIn_OrderPerDay(object sender, EventArgs e)
        {
            string myurl = "";
            myurl = "../Order/Report/UserInOrderPerDay.aspx?menu=";
            Response.RedirectPermanent(myurl);
        }

        protected void btn_CountOrder_InBrn(object sender, EventArgs e)
        {
            string myurl = "";
            myurl = "../Order/Report/CountOrderInBrn.aspx?menu=";
            Response.RedirectPermanent(myurl);
        }
        


    }
}