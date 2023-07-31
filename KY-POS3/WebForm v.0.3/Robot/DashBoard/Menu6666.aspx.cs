using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.DashBoard {
    public partial class Menu6666 : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                LoadData();
            }
        }

        private void LoadData() {

        }

        protected void btnPOList_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "../PO/POList.aspx?menu=1103";
            Response.RedirectPermanent(myurl);
        }
        protected void btnPODeltail_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "../PO/PODetail.aspx?menu=1103";
            Response.RedirectPermanent(myurl);
        }
        protected void btnDlvHead_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "../PO/DlvHeadList.aspx?menu=1201";
            Response.RedirectPermanent(myurl);
        }

        protected void btnDlvLine_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "../PO/DlvLineList.aspx?menu=";
            Response.RedirectPermanent(myurl);
        }


        protected void btnDlvDetail_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "../PO/DlvDetail.aspx?menu=1202";
            Response.RedirectPermanent(myurl);
        }

        protected void btnDlvLicense_Click(object sender, EventArgs e) {
            string myurl = "";
            myurl = "../PO/DlvLicenselist.aspx?menu=1251";
            Response.RedirectPermanent(myurl);
        }

    }
}