using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
using Robot.POSC.DA;
using static Robot.POSC.DA.ShipToService;

namespace Robot.POSC {
    public partial class POSShipToPage : System.Web.UI.Page
    {

        public static string PreviousPage { get { return HttpContext.Current.Session["shipto_previouspage"] == null ? "" : (string)HttpContext.Current.Session["shipto_previouspage"]; } set { HttpContext.Current.Session["shipto_previouspage"] = value; } }
        public static List<ShipTo> ShipToList { get { return (List<ShipTo>)HttpContext.Current.Session["shipto"]; } set { HttpContext.Current.Session["shipto"] = value; } }
   
        protected void Page_Load(object sender, EventArgs e) { 
            LoadDevDropDownList();
            if (!Page.IsPostBack) {
                CloseAlert(); 
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString() { 
        }

        private void CheckPermission() {
       

        }

        private void LoadData() {
            ShipToList = ShipToService.ListShipTo();
            GridBinding();

        } 
        private void LoadDropDownList() {
        }

        private void LoadDevDropDownList() {

        }
        private void GridBinding() {
            grdlist.DataSource = ShipToList;
            grdlist.DataBind();
        }
        protected void grdlist_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e) {
            (grdlist.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }
        protected void grdlist_ItemCommand(object sender, ListViewCommandEventArgs e) {
            if (e.CommandName == "sel") {
                var id = e.CommandArgument.ToString(); // value from CommandArgument  
                POSSaleService.NewBillFromPreviousBill();
                var shipto = ShipToList.Where(o => o.ShipToID == id).FirstOrDefault();
                POSSaleService.DocSet.Head.ShipToLocID = shipto.ShipToID;

                POSSaleService.DocSet.Head.ShipToLocName = shipto.ShipToName;
                POSSaleService.DocSet.Head.ShipToUsePrice = shipto.UsePrice;
                POSSaleService.Menu = POSItemService.ListMenuItem(POSSaleService.DocSet.Head.ComID, POSSaleService.DocSet.Head.ShipToUsePrice);
                //OPDDetail.PreviousDetailPage = HttpContext.Current.Request.Url.PathAndQuery;
                //HCareService.GetDocSetByID(id, false);
                //string url = $"../OPD/OPDDetail";
                Response.Redirect(PreviousPage);


            }
        }

        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

   


        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }
          

    }
}