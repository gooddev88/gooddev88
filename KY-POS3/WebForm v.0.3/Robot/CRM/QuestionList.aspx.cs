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

namespace Robot.CRM {
    public partial class QuestionList : MyBasePage {

        protected void Page_Load(object sender, EventArgs e) {
            hddmenu.Value = Request.QueryString["menu"];
            hdddoctype.Value = Request.QueryString["doctype"];
            hddcompany.Value = Request.QueryString["com"];
            LoadDropDownDevList();
            if (!IsPostBack) {
                SetBackLink();
                LoadDefaultFilter();
                CheckPermission();
                LoadData();
            
            }
        }

        private void SetBackLink()
        {
            if (Session["QuestionnairList_previouspage"] == null)
            {
                hddPreviouspage.Value = "/Start.aspx";
            }
            else
            {
                hddPreviouspage.Value = Session["QuestionnairList_previouspage"].ToString();
            }
        }

        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(hddPreviouspage.Value);
        }

        private void CheckPermission() {
            if (!PermissionService.CanDelete("F5991")) {
                //grdDetail.Columns["colDelete"].Visible = false;
            }
        }
        private void LoadDefaultFilter() {

            #region default caption
            hddTopic.Value = "แบบสอบถาม " + "(" + hddmenu.Value + ")";
            #endregion

            if (Session[hddmenu.Value + "_search"] != null) {
                txtSearch.Text = Session[hddmenu.Value + "_search"].ToString();
            }

            DateTime date_begin = DateTime.Now.Date.AddDays(-15);
            DateTime date_end = DateTime.Now.Date;

            if (Session[hddmenu.Value + "_datebegin"] != null) {
                date_begin = (DateTime)Session[hddmenu.Value + "_datebegin"];
            }
            if (Session[hddmenu.Value + "_dateend"] != null) {
                date_end = (DateTime)Session[hddmenu.Value + "_dateend"];
            }

            dtBegin.Value = date_begin;
            dtEnd.Value = date_end;

        }
        private void SetDefaultFilter() {
            Session[hddmenu.Value + "_search"] = txtSearch.Text;

            DateTime date_begin = DateTime.Now.Date.AddDays(-15);
            DateTime date_end = DateTime.Now.Date;
            try { date_begin = DateTime.Parse(dtBegin.Value.ToString()); } catch { }
            try { date_end = DateTime.Parse(dtEnd.Value.ToString()); } catch { }
            Session[hddmenu.Value + "_datebegin"] = date_begin;
            Session[hddmenu.Value + "_dateend"] = date_end;
        }

        private void LoadDropDownDevList() {

        }


        private void LoadData() {
            try {
           
                string str_search = txtSearch.Text;
                DateTime date_begin = DateTime.Now.Date;
                DateTime date_end = DateTime.Now.Date;

                if (dtBegin.Value != null) {
                    date_begin = dtBegin.Date;
                }
                if (dtEnd.Value != null) {
                    date_end = dtEnd.Date;
                }
          
                var query = QuestionInfoService.ListViewHeadSearch(str_search, date_begin, date_end).ToList();

                Session[hddmenu.Value + "QuestionList_Head"] = query;
                grdDetail.DataSource = query;
                grdDetail.DataBind();

            } catch (Exception ex) {
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            GridBinding();
        }

        private void GridBinding() {
            var data = (List<QuestionHead>)Session[hddmenu.Value + "QuestionList_Head"];
            grdDetail.DataSource = data.Where(o => o.IsActive == true).OrderByDescending(o => o.DateBegin);
        }
        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();            
            LoadData();
        }
    
        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Select") {
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString();
                Session["Questionnair_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = "";
                myurl = "/CRM/QuestionDetail.aspx?id=" + id + "&menu=" + hddmenu.Value;
                Response.RedirectPermanent(myurl);
            }
        }


        protected void btnNew_Click(object sender, EventArgs e) {
            Session["Questionnair_previouspage"] = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = "/CRM/QuestionDetail.aspx?id=" + "" + "&menu=" + hddmenu.Value;

            Response.RedirectPermanent(myurl);
        }

    }

}