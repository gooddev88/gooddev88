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

namespace Robot.Master
{
    public partial class CompanyNewDoc : MyBasePage {
        public static List<vw_CompanyInfo> DocList { get { return (List<vw_CompanyInfo>)HttpContext.Current.Session["companytype_list"]; } set { HttpContext.Current.Session["companytype_list"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDevDropDownList();
            if (!Page.IsPostBack) {       
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"]; 
        }
        private void LoadData() {        
            DocList = CompanyService.ListCompanyMain();

            GridBinding();
        }

        private void GridBinding()
        {
            grdlist.DataSource = DocList;
            grdlist.DataBind();
        }

        protected void grdlist_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (grdlist.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }

        protected void grdlist_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "selrow")
            {
                string id = e.CommandArgument.ToString(); // value from CommandArgument   
                CompanyService.NewTransaction();
                CompanyService.DocSet.Info.ParentID = id;
                string myurl = $"`/Master/CompanyDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
        }

        private void LoadDropDownList() {

        }

        private void LoadDevDropDownList() {

        }

        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void btnback_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(CompanyService.NewDocPreviousPage);
        }

    }
}