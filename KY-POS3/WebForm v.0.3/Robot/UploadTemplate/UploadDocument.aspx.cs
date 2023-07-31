using System;
using System.Linq;
using System.Web;
using System.Web.UI;

using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;


using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;
using System.Threading;
using OfficeOpenXml;

namespace Robot.UploadTemplate {
    public partial class UploadDocument : MyBasePage {

        protected void Page_Load(object sender, EventArgs e) {
            hddmenu.Value = Request.QueryString["menu"];
            hdddoctype.Value = Request.QueryString["doctype"];
            LoadDropDownDevList();
            if (!IsPostBack) {
                SetBackLink();
                LoadDefaultFilter();
                LoadData();
                LoadDropDownList();
            }
        }

        private void SetBackLink()
        {
            if (Session["UploadDocument_previouspage"] == null)
            {
                hddPreviouspage.Value = "/Start.aspx";
            }
            else
            {
                hddPreviouspage.Value = Session["UploadDocument_previouspage"].ToString();
            }
        }

        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(hddPreviouspage.Value);
        }
   protected void btnNext_Click(object sender, EventArgs e)
        {
            switch (cboUploadType.SelectedValue) {
                case "ORDER UPLOAD":
                    Session["Upload_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
                    string url = "UploadOrder.aspx?id=";
                    Response.RedirectPermanent(url);
                    break;
                default:
                    break;
            }
          
        }
        private void CheckPermission() {

        }
        private void LoadDefaultFilter() {

            #region default caption

            if (hddmenu.Value == "7601")
            {
                hddTopic.Value = "Upload Document (For Approve) " + "(" + hddmenu.Value + ")";
            }
            #endregion

        }
        private void SetDefaultFilter() {

        }

        
        private void LoadDropDownList()
        {
            cboUploadType.DataSource = MasterTypeService.ListType("UPLOAD DOCTYPE", false);
            cboUploadType.DataBind();
        }

        private void LoadDropDownDevList() {

        }

        private void LoadData() {
            try {           
          
                //var query = QuestionInfoService.ListViewHeadSearch(str_search, date_begin, date_end).ToList();
                //Session[hddmenu.Value + "QuestionList_Head"] = query;

            } catch (Exception ex) {
            }
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            switch (cboUploadType.SelectedValue)
            {
                case "ORDER PROMOTION":
                    UploadFilePromotion();
                    break;
                default: break;
            }
            //UploadFile();
        }

        private void ShowAlert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }

        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private void UploadFilePromotion()
        {

        }

    }

}