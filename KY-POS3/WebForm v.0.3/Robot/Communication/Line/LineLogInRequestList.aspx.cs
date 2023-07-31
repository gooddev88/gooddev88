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

namespace Robot.Communication.Line
{
    public partial class LineLogInRequestList : System.Web.UI.Page
    {

        public static I_FiterSet Filter { get { return (I_FiterSet)HttpContext.Current.Session["linereq_status"]; } set { HttpContext.Current.Session["linereq_status"] = value; } }
        public static string PreviousPage { get { return HttpContext.Current.Session["linereq_list_previouspage"] == null ? "" : (string)HttpContext.Current.Session["linereq_list_previouspage"]; } set { HttpContext.Current.Session["linereq_list_previouspage"] = value; } }
        public static List<vw_LineLogInRequest> ReqList { get { return (List<vw_LineLogInRequest>)HttpContext.Current.Session["linereq_list"]; } set { HttpContext.Current.Session["linereq_list"] = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetQueryString();

            LoadDropDownDevList();
            if (!IsPostBack)
            {
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString()
        {

        }

        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.Redirect(PreviousPage);
        }

        private void LoadDefaultFilter()
        {
            //if (LicenseService.FilterSet == null) {
            //    LicenseService.NewFilterSet();
            //}

            //txtSearch2.Text = LicenseService.FilterSet.SearchText;
            //cboStatus.SelectedValue = LicenseService.FilterSet.Status;
            //cboemp.Value = LicenseService.FilterSet.Username;
        }
        private void SetActiveControl()
        {

            string pagename = "อนุมัติคำขอผูกบัญชี Line Official";
            string look_logo = "<span style=\"color: black\"><i class=\"fas fa-user-check fa-2x\"></i></span>";
            string doctypedesc = $"<span style=\"font-size:large; color: black\">{pagename}</span>";
            lblTopic.Text = Server.HtmlDecode(look_logo + "&nbsp" + doctypedesc);

        }
        private void SetDefaultFilter()
        {
            //LicenseService.NewFilterSet();
            //LicenseService.FilterSet.SearchText = txtSearch2.Text.Trim();
            //LicenseService.FilterSet.Status = cboStatus.SelectedValue;
            //if (cboemp.Value != null) {
            //    LicenseService.FilterSet.Username = cboemp.Value.ToString();
            //} else {
            //    LicenseService.FilterSet.Username = "";
            //}
        }

        private void LoadDropDownList()
        {

        }

        private void LoadDropDownDevList()
        {

        }

        private void LoadData()
        {
            SetDefaultFilter();
            ReqList = LineRegisterService.ListViewLineLogInRequest("USER", "PENDING").ToList();
            GridBinding();
            SetActiveControl();

        }

        private void GridBinding()
        {
            grdDetail.DataSource = ReqList;
            grdDetail.DataBind();
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            if (e.CommandArgs.CommandName == "sel")
            {
                ASPxGridView grid = (ASPxGridView)sender;
                string reqid = e.KeyValue.ToString();
                LineLogInRequestDetail.ReqData = LineRegisterService.GetReq(reqid);
                LineLogInRequestDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                string url = "../LineLogInRequestDetail";
                Response.Redirect(url);
            }
            if (e.CommandArgs.CommandName == "app")
            {
                ASPxGridView grid = (ASPxGridView)sender;
                string reqid = e.KeyValue.ToString();

                var r = LineRegisterService.ActionRequest("ACCEPTED", reqid, "");

                if (r.Result == "fail")
                {

                }
                else
                {

                    LoadData();

                }

           
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e)
        {
            grdDetail.DataSource = ReqList;
        }

    }

}