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
using static Robot.Master.DA.CompanyService;

namespace Robot.POS {
    public partial class AdjustNewDoc : MyBasePage {
        public static string PrevoiusNewDoc { get { return (string)HttpContext.Current.Session["Adjustnewdoc_previouspage"]; } set { HttpContext.Current.Session["Adjustnewdoc_previouspage"] = value; } }
        public static string DocType { get { return HttpContext.Current.Session["Adjustnewdoc_doctype"] ==null?"":(string)HttpContext.Current.Session["Adjustnewdoc_doctype"]; } set { HttpContext.Current.Session["Adjustnewdoc_doctype"] = value; } }
        public static string DocID { get { return HttpContext.Current.Session["Adjust_docid"] == null ? "" : (string)HttpContext.Current.Session["Adjust_docid"]; } set { HttpContext.Current.Session["Adjust_docid"] = value; } }
        
        public static I_StkAjdFilter Filter { get { return  (I_StkAjdFilter)HttpContext.Current.Session["Adjust_filter"]; } set { HttpContext.Current.Session["Adjust_filter"] = value; } }
        public static List<CompanyInfoList> ComListInUserLogIn { get { return (List<CompanyInfoList>)HttpContext.Current.Session["Adjust_custlist_inuser"]; } set { HttpContext.Current.Session["Adjust_custlist_inuser"] = value; } }

        #region Class Filter Set
        public partial class I_StkAjdFilter
        { 
            public string CustID { get; set; }
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
            Filter =new I_StkAjdFilter();
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
            try {            
                SetDefaultFilter();
                ComListInUserLogIn = CompanyService.ListCompanyInfoUIC("BRANCH",true);
                GridBinding();

            } catch (Exception ex) {
            }

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
            Response.Redirect(POSOrderService.NewDocPreviousPage);
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

                POSStkAdjustService.NewTransaction();
                var h = POSStkAdjustService.DocSet.head;
                h.ComID = lblComID.Text;
                string myurl = $"~/POS/POSStkAdjustDetail?menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);          
            }

        }

    }
}