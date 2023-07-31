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
using Robot.POS.DA;


namespace Robot.POS {
    public partial class POSPONewDoc : MyBasePage {
        public static string PrevoiusNewDoc { get { return (string)HttpContext.Current.Session["ponewdoc_previouspage"]; } set { HttpContext.Current.Session["ponewdoc_previouspage"] = value; } }
        public static string DocType { get { return HttpContext.Current.Session["podoctype"] ==null?"":(string)HttpContext.Current.Session["podoctype"]; } set { HttpContext.Current.Session["podoctype"] = value; } }
        public static string DocID { get { return HttpContext.Current.Session["podocid"] == null ? "" : (string)HttpContext.Current.Session["podocid"]; } set { HttpContext.Current.Session["podocid"] = value; } }
        
        public static I_POFilter Filter { get { return  (I_POFilter)HttpContext.Current.Session["po_filter"]; } set { HttpContext.Current.Session["po_filter"] = value; } }
        public static List<CompanyInfo> ComListInUserLogIn { get { return (List<CompanyInfo>)HttpContext.Current.Session["so_custlist_inuser"]; } set { HttpContext.Current.Session["so_custlist_inuser"] = value; } }

        #region Class Filter Set
        public partial class I_POFilter { 
            public string CustID { get; set; }
            public string RComID { get; set; }
            public string ComID { get; set; }
            public string SearchText { get; set; }
            public string FilterText { get; set; } 
        }
     
        #endregion
        protected void Page_Load(object sender, EventArgs e) { 
            LoadDevDropDownList();
            popAlert.ShowOnPageLoad = false;
            SetQueryString();
            if (!Page.IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter(); 
                LoadData();
            }
        }

        public static void NewFilter() {
            Filter =new I_POFilter();
            Filter.RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            Filter.CustID = "";
            Filter.ComID = "";
            Filter.SearchText = "";
    
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void LoadDefaultFilter() {
            if (Filter == null) {
                NewFilter();
            } 
            txtSearch.Text = Filter.SearchText;        
           

        }
        private void SetDefaultFilter() {
            Filter.SearchText = txtSearch.Text.Trim();
        }


        private void LoadData() { 
                SetDefaultFilter();
            //ComListInUserLogIn = CompanyService.ListCompanyByWH(Filter.RComID);
            ComListInUserLogIn = CompanyService.ListBranch(false);
            GridBinding(); 
        }

        private void GridBinding() {
            grdlist.DataSource = ComListInUserLogIn;
            grdlist.DataBind();
        }

        private void LoadDropDownList() { 

        }

        private void LoadDevDropDownList() {

        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }
            if (showbutton == "") {
                btnCancel.Visible = false;
            }
            if (showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            popAlert.ShowOnPageLoad = true;
        }

        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }


        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }

        protected void btnCancel_Click(object sender, EventArgs e) {
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(POS_POService.NewDocPreviousPage);
        }

        protected void grdlist_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e) {
            (grdlist.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }
        protected void grdlist_ItemCommand(object sender, ListViewCommandEventArgs e) {

            SetDefaultFilter();
            if (e.CommandName == "ok") {
                var id = e.CommandArgument; // value from CommandArgument    
                Label lblComID = (Label)e.Item.FindControl("lblComID");

                POS_POService.NewTransaction("PO");
                var h = POS_POService.DocSet.head;
                h.ComID = lblComID.Text;
                h.ToLocID = lblComID.Text;
                string myurl = $"~/POS/POSPODetail?menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);             
            }

        }

    }
}