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
    public partial class ItemNewDoc : MyBasePage {
        public static List<MasterTypeLine> DocList { get { return (List<MasterTypeLine>)HttpContext.Current.Session["itemtyeplist"]; } set { HttpContext.Current.Session["itemtyeplist"] = value; } }

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
            DocList = MasterTypeService.ListType("ITEM TYPE",false).Where(o => o.ValueTXT != "DISCOUNT").ToList();
                
            grddoctype.DataSource = DocList;
            BindGrid();
        }
        private void BindGrid (){
            grddoctype.DataSource = DocList;
            grddoctype.DataBind();
        }   

        protected void grddoctype_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Select") {
                string docType = e.KeyValue.ToString(); 
                POSItemService.NewTransaction("");
                POSItemService.DocSet.Info.TypeID = docType; 
                string myurl = $"~/Master/ItemDetail?menu={hddmenu.Value}";
                Response.Redirect(myurl);
            }
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
            Response.RedirectPermanent(POSItemService.NewDocPreviousPage);
        }

        protected void grddoctype_DataBinding(object sender, EventArgs e) {
            grddoctype.DataSource = DocList;
        }
    }
}