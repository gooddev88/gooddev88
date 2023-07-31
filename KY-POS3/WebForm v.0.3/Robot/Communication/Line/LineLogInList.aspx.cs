using System;
using System.Linq;
using System.Web;
 
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;
using System.Collections.Generic;
using Robot.Data; 
using Robot.Communication.DA;
using static Robot.Communication.DA.LineRegisterService;

namespace Robot.Communication.Line {
    public partial class LineLogInList : System.Web.UI.Page {

        public static I_FiterSet Filter { get { return  (I_FiterSet)HttpContext.Current.Session["linereq_status"]; } set { HttpContext.Current.Session["linereq_status"] = value; } }
        public static string PreviousPage { get { return HttpContext.Current.Session["linereq_list_previouspage"] == null ? "" : (string)HttpContext.Current.Session["linereq_list_previouspage"]; } set { HttpContext.Current.Session["linereq_list_previouspage"] = value; } }
        public static List<vw_LineLogIn> LineLoginList { get { return (List<vw_LineLogIn>)HttpContext.Current.Session["linelogin_list"]; } set { HttpContext.Current.Session["linelogin_list"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
     
            LoadDropDownDevList();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString() {

        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }

        private void LoadDefaultFilter() {
            if (Filter == null) {
                Filter = LineRegisterService.NewFilterSet();
            }

            txtSearch2.Text = Filter.Search; 
        }
        private void SetActiveControl() {
            
            string pagename = "พนักงานที่ผูกบัญชี Line OA Line Official";
            string look_logo = "<span style=\"color: black\"><i class=\"fas fa-user-check fa-2x\"></i></span>";
            string doctypedesc = $"<span style=\"font-size:large; color: black\">{pagename}</span>";
            lblTopic.Text = Server.HtmlDecode(look_logo + "&nbsp" + doctypedesc);

        }
        private void SetDefaultFilter() {
            LineRegisterService.NewFilterSet();
            Filter.Search =txtSearch2.Text.Trim();
            
        }

        private void LoadDropDownList() {

        }

        private void LoadDropDownDevList() {

        }

        private void LoadData(bool isShowAll=false) {
            SetDefaultFilter(); 
            LineLoginList = LineRegisterService.ListLineLogIn("USER", Filter.Search, isShowAll).ToList();
            GridBinding();
            SetActiveControl();

        }

        private void GridBinding() {
            grdDetail.DataSource = LineLoginList;
            grdDetail.DataBind();
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            LoadData(true);
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "sel") {
                ASPxGridView grid = (ASPxGridView)sender;
                int id = Convert.ToInt32(e.KeyValue);
                LineLogInDetail.LineLoginData= LineRegisterService.GetLogin(id);
                LineLogInDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;            
                string url = "../LineLogInDetail";
                Response.Redirect(url);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = LineLoginList;
        }
 
    }

}