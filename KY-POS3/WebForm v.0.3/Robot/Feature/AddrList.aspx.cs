
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot.Feature {
    public partial class AddrList : System.Web.UI.Page {

        protected void Page_Load(object sender, EventArgs e) {


            if (!IsPostBack) {
                Session["p_sel_addr"] = null;
                LoadData();
            }
        }

        private void LoadData() {
            string search = txtSearch.Text.Trim();
            var data = AddressInfoService.ListAddrBySearch(txtSearch.Text.Trim()).ToList();
            Session["p_addr_list"] = data.OrderBy(o => o.ProvinceID).ThenBy(o => o.AmphoeID).ThenBy(o => o.DistrictID).ToList(); ;
            GridBind();
        }

        private void GridBind() {
            var data = (List<Addr_District>)Session["p_addr_list"];
            grd.DataSource = data;
            grd.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            LoadData();
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            grd.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "Select") {
                Button btn = (Button)e.CommandSource;
                GridViewRow grdrow = ((GridViewRow)btn.NamingContainer);
                Label lblID = (Label)grdrow.FindControl("lblID");
                int lineNum = Convert.ToInt32(lblID.Text);
                var data_list = (List<Addr_District>)Session["p_addr_list"];
                var sel_data = data_list.Where(o => o.ID == lineNum).FirstOrDefault();
                Session["p_sel_addr"] = sel_data;
                ClosePopup("OK");
            }
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e) {

        }
        protected void btnCancel_Click(object sender, EventArgs e) {
            ClosePopup("Cancel");
        }
        protected void btnOk_Click(object sender, EventArgs e) {
            ClosePopup("OK");
        }
        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }
    }
}