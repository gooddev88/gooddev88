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

using Robot.Master.DA;
using Robot.POS.DA;

namespace Robot.POS
{
    public partial class RPT_POSORDER_RMList : MyBasePage {
        public static string PreviousListPage { get { return HttpContext.Current.Session["rptposorderrm_previouspage"] == null ? "" : (string)HttpContext.Current.Session["rptposorderrm_previouspage"]; } set { HttpContext.Current.Session["rptposorderrm_previouspage"] = value; } }
        public static List<vw_POS_ORDER_RM> DocList { get { return (List<vw_POS_ORDER_RM>)HttpContext.Current.Session["rptposorderrm_list"]; } set { HttpContext.Current.Session["rptposorderrm_list"] = value; } }
        public static FilterSet MyFilter { get { return (FilterSet)HttpContext.Current.Session["rptposorderrm_filter"]; } set { HttpContext.Current.Session["rptposorderrm_filter"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString()
        {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void NewFilterSet()
        {
            MyFilter = new FilterSet();
            MyFilter.Begin = DateTime.Now.Date.AddDays(-7);
            MyFilter.End = DateTime.Now.Date;
            MyFilter.Search = "";
        }

        public class FilterSet
        {
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
            public string Search { get; set; }
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousListPage);
        }

        private void LoadDefaultFilter() {
            if (MyFilter == null)
            {
                NewFilterSet();
            }
            var f = MyFilter;

            txtSearch.Text = f.Search;
            dtBegin.Value = f.Begin;
            dtEnd.Value = f.End;
        }
        private void SetDefaultFilter() {
            var f = MyFilter;

            f.Search = txtSearch.Text;
            DateTime date_begin = DateTime.Now.Date;
            try { date_begin = DateTime.Parse(dtBegin.Value.ToString()); } catch { }
            f.Begin = date_begin;

            DateTime date_end = DateTime.Now.Date;
            try { date_end = DateTime.Parse(dtEnd.Value.ToString()); } catch { }
            f.End = date_end;
        }
        private void BindData() {
            lblinfohead.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
        }
        private void LoadDropDownList() {

        }

        private void LoadData() {       
            SetDefaultFilter();
            BindData();
            DocList = POSOrderService.ListDoc_POSORDER_RM(MyFilter);
            BindGrid();      
        }

        private void BindGrid()
        {
            grdDetail.DataSource = DocList;
            grdDetail.DataBind();
        }

        private void GridBinding() {
            grdDetail.DataSource = DocList;
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e)
        {
            GridBinding();
        }

        protected void btnSearch_Click(object sender, EventArgs e) {
            SetDefaultFilter();
            LoadData();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Print.MyPrint.NewReportFItler();
            var f = Print.MyPrint.ReportFilterX;
            f.Begin = dtBegin.Date;
            f.End = dtEnd.Date;

            Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/POS/Print/MyPrint?report=PrintOrderRM";
            Response.RedirectPermanent(myurl);
        }

    }

}