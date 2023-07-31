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

namespace Robot.POS {
    public partial class INVNewDoc : MyBasePage {
        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["oinvnewdoc_previouspage"]; } set { HttpContext.Current.Session["oinvnewdoc_previouspage"] = value; } }
        public static string ParamDocType { get { return (string)HttpContext.Current.Session["oinvnew_doctype"]; } set { HttpContext.Current.Session["oinvnew_doctype"] = value; } }
        public static List<DocTypeInfo> DocTypeList { get { return (List<DocTypeInfo>)HttpContext.Current.Session["doctype_list"]; } set { HttpContext.Current.Session["doctype_list"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDevDropDownList();
            popAlert.ShowOnPageLoad = false;

            if (!Page.IsPostBack) {
                
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString() {

            hddmenu.Value = Request.QueryString["menu"];
            
        }
        private void LoadData() {

            try {

                DocTypeList = DocTypeInfoService.ListDocTypeByParentID("OINV").ToList();
                GridBinding();


            } catch (Exception ex) {

            }
        }
        private void GridBinding()
        {
            grdlist.DataSource = DocTypeList;
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
                string docType = e.CommandArgument.ToString();

                ARInvoiceService.NewTransaction();
                var h = ARInvoiceService.DocSet.Head;
                h.CompanyID = LoginService.LoginInfo.CurrentCompany.CompanyID;
                h.DocTypeID = docType;
                string url = "~/POS/INVDetail";
                Response.Redirect(url);
            }
        }

        private string HtmlSpace(int round)
        {
            string result = "";
            for (int i = 0; i < round; i++)
            {
                result = result + "&nbsp";
            }
            return result;
        }

        protected void grdlist_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            Literal lblstatus = (e.Item.FindControl("lblstatus") as Literal);
            var dataItem = e.Item.DataItem;
            var doctype = ((DocTypeInfo)dataItem).DocTypeID;
            //var icon = ((DocTypeInfo)dataItem).Icon;

            string html_status = "";

            if (doctype != "")
            {
                html_status = html_status + "<span style=\"color: limegreen\">" + " * " + "</span>" + HtmlSpace(2);
            }
            else
            {
                html_status = html_status + "<span class=\"badge badge-pill badge-info\" style=\"font-size: large\">" + "ไม่มีข้อมูล" + "</span>" + HtmlSpace(1);
            }


            lblstatus.Text = html_status;

        }

        private void SetCaptionByMenu() {
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


        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }

        protected void btnCancel_Click(object sender, EventArgs e) {
        }

        protected void btnback_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousPageX);
        }
    }
}