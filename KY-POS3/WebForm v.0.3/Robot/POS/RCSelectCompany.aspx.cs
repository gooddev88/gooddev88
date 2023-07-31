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
using Robot.POS.DA;
using Robot.OMASTER;
using Robot.Master.DA;

namespace Robot.POS
{
    public partial class RCSelectCompany : MyBasePage
    {

        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["selectcom_rc_previouspage"]; } set { HttpContext.Current.Session["selectcom_rc_previouspage"] = value; } }
        public static I_Filter FilterX { get { return (I_Filter)HttpContext.Current.Session["filtercom_rc_previouspage"]; } set { HttpContext.Current.Session["filtercom_rc_previouspage"] = value; } }
        public static List<CompanyInfo> ComList { get { return (List<CompanyInfo>)HttpContext.Current.Session["comlist"]; } set { HttpContext.Current.Session["comlist"] = value; } }
        public class I_Filter
        {
            public String SearchText { get; set; }
            public String Company { get; set; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            SetQueryStirng();

            popAlert.ShowOnPageLoad = false;
            LoadDevDropDownList();
            if (!Page.IsPostBack)
            {

                LoadDropDownList();
                LoadDefaultFilter();

                LoadData();
            }
        }
        private void SetQueryStirng()
        {
            hddmenu.Value = Request.QueryString["menu"];
        }
        private void NewFilter()
        {
            FilterX = new I_Filter();
            FilterX.Company = "";
            FilterX.SearchText = "";
        }

        private void LoadDefaultFilter()
        {
            if (FilterX == null)
            {
                NewFilter();
            }


            txtSearch.Text = FilterX.SearchText;
        }
        private void SetDefaultFilter()
        {


            FilterX.SearchText = txtSearch.Text.Trim();
        }


        private void LoadData()
        {          
            SetDefaultFilter();
            ComList = CompanyService.ListCompany(FilterX.SearchText,true);
            GridBinding();
        }

        private void GridBinding()
        {
            grdlist.DataSource = ComList;
            grdlist.DataBind();
        }

        private void LoadDropDownList()
        {
           
        }

        private void LoadDevDropDownList()
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SetDefaultFilter();
            LoadData();
        }

        private void ShowAlert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }


        private void ClosePopup(string command)
        {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
        }

        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.Redirect(PreviousPageX);
        }

        protected void grdlist_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (grdlist.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }
        protected void grdlist_ItemCommand(object sender, ListViewCommandEventArgs e)
        {         
            SetDefaultFilter();
            if (e.CommandName == "Confirm")  {
                var id = e.CommandArgument; // value from CommandArgument    
                Label lblComID = (Label)e.Item.FindControl("lblComID");
                SetData(lblComID.Text);
                string url = $"../POS/RCSelectCustomer?menu={hddmenu.Value}";
                Response.Redirect(url);

            } 
        }
        private void SetData(string comId)
        {
            // string comid = cboCompany.SelectedValue;
            ORCService.NewTransaction();
            var h = ORCService.DocSet.Head;
            var com = CompanyService.GetCompanyInfo(comId);
            h.CompanyID = com.CompanyID;
        }

        protected void cboCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }


    }
}