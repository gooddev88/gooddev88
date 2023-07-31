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
using static Robot.POSC.DA.POSSaleService;
using Robot.POSC.DA;
using Robot.Master.DA;

namespace Robot.POSC {
    public partial class POSInvoiceList : MyBasePage {
        public static List<vw_POS_SaleHead> DocList { get { return (List<vw_POS_SaleHead>)HttpContext.Current.Session["possale_doclist"]; } set { HttpContext.Current.Session["possale_doclist"] = value; } }
        public static I_FilterSet Filter { get { return (I_FilterSet)HttpContext.Current.Session["possale_filter"]; } set { HttpContext.Current.Session["possale_filter"] = value; } }
        public static string PreviousPage { get { return HttpContext.Current.Session["posclist_previouspage"] == null ? "" : (string)HttpContext.Current.Session["posclist_previouspage"]; } set { HttpContext.Current.Session["posclist_previouspage"] = value; } }


        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {
                LoadDefaultFilter();
                LoadData();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];          
          
        }


   
        private void LoadDefaultFilter() {
            if (Filter == null) {
                Filter = POSSaleService.NewFilterSet();
            }
            txtSearch.Text = Filter.Search;
            dtBegin.Value = Filter.Begin;
            dtEnd.Value = Filter.End;
            cboCompany.Value = Filter.Company;
            chkShowCancel.Checked = Filter.ShowActive;
        }
        private void SetDefaultFilter() {
            Filter = POSSaleService.NewFilterSet();
            if (dtBegin.Value != null) {
                Filter.Begin = dtBegin.Date;
            }
            if (dtEnd.Value != null) {
                Filter.End = dtEnd.Date;
            }
            if (cboCompany.Value != null) {
                Filter.Company = cboCompany.Value.ToString();
            }
            Filter.ShowActive = chkShowCancel.Checked;
            Filter.Search = txtSearch.Text.Trim();
        }

        private void LoadDropDownDevList() {
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", false).ToList(); 
            cboCompany.DataBind();
        }

        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {

            LoadData();
        }

        private void LoadData() {
            SetDefaultFilter();
            DocList = POSSaleService.ListDoc(Filter).ToList();
           
            BindData();
            GridBinding();
            SetActiveControl();
        }
        private void SetActiveControl() {
            CheckPermission();
        }
        private void CheckPermission() {
            if (!PermissionService.CanDelete("F5991")) {
                //grdDetail.Columns["colDelete"].Visible = false;
            }
        }
        private void BindData() {
            hddTopic.Value = "ประวัติขาย " + "(" + hddmenu.Value + ")";
        }
        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = DocList.Where(o => o.INVID != "").ToList();
        }

        private void GridBinding() {
            grdDetail.DataSource = DocList.Where(o => o.INVID != "").ToList();
            grdDetail.DataBind();
        }


        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });

        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "sel") {
                ASPxGridView grid = (ASPxGridView)sender;
                string billId = e.KeyValue.ToString();
                POSSaleDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                POSSaleService.DocSet = POSSaleService.GetDocSet(billId);
                POSSaleService.Menu = POSItemService.ListMenuItem(POSSaleService.DocSet.Head.ComID, POSSaleService.DocSet.Head.ShipToUsePrice);
          
                string myurl = $"~/POSC/POSSaleDetail?&menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            }
         
        }


        protected void btnNew_Click(object sender, EventArgs e) {
            POSSaleNewDoc.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
        
            string myurl = "~/POSC/POSSaleNewDoc?menu=5002";
            Response.RedirectPermanent(myurl);
        }

        protected void cchkShowCancel_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }


    }

}