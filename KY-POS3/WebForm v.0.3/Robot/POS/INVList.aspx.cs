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
using Robot.POS.DA;

namespace Robot.POS
{
    public partial class INVList : MyBasePage
    {
        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["oinvlist_previouspage"]; } set { HttpContext.Current.Session["oinvlist_previouspage"] = value; } }
        public static string ParamDocType { get { return (string)HttpContext.Current.Session["oinv_doctype"]; } set { HttpContext.Current.Session["oinv_doctype"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {
             
                LoadDefaultFilter();              
                LoadData();
                FiterbyChange();
            }
        }
        private void SetQueryString() { 
            hddmenu.Value = Request.QueryString["menu"]; 
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPageX);
        }
        private void LoadDefaultFilter() {
            if (ARInvoiceService.FilterSet==null) {
                ARInvoiceService.NewFilterSet();
            }


            var f = ARInvoiceService.FilterSet;

            txtSearch.Text = f.SearchText; 
            chkShowClose.Checked = f.ShowClosed;
            dtBegin.Value = f.DateFrom; 
            dtEnd.Value = f.DateTo;
          
        }
        private void SetDefaultFilter() {
            ARInvoiceService.NewFilterSet();
            var f = ARInvoiceService.FilterSet;
            f.SearchText = txtSearch.Text.Trim();
            f.DateFrom = dtBegin.Value == null ? DateTime.Now.Date : dtBegin.Date;
            f.DateTo = dtEnd.Value == null ? DateTime.Now.Date : dtEnd.Date;
            f.SearchBy = cbofilterby.SelectedValue;
        } 

        private void LoadDropDownDevList() {
            //cboCompany.DataSource = CompanyInfoService.MiniSelectList("BRANCH", false).ToList(); ;
            //cboCompany.DataBind();
        }

        private void LoadData() {
            try {
                SetDefaultFilter();
                ARInvoiceService.ListDoc();
                GridBinding();
                SetActiveControl();

            } catch (Exception ex) {
            }
        }


        private void SetActiveControl()
        {
            hddTopic.Value = "O Invoice";
            lblHeaderCaption.Text = "O Invoice"; 
        }
        private void GridBinding() {
            grdDetail.DataSource = ARInvoiceService.DocList;
            grdDetail.DataBind();
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            SetDefaultFilter();
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {

            if (e.CommandArgs.CommandName == "Select") {
                ASPxGridView grid = (ASPxGridView)sender;
             
                int rowId = Convert.ToInt32(e.KeyValue);
                ARInvoiceService.GetDocSetByID(rowId,false);
                INVDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
                string url = $"~/POS/INVDetail?menu={hddmenu.Value}";
                Response.Redirect(url);
              
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = ARInvoiceService.DocList;
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            INVDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            INVNewDoc.PreviousPageX= HttpContext.Current.Request.Url.PathAndQuery;
            INVNewDoc.ParamDocType = "INV";
            string myurl =$"~/POS/INVNewDoc?&menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

        #region Big data source
        protected void cboCustomer_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT [CustomerID],[FullNameTh] FROM  ( SELECT [CustomerID],[FullNameTh], row_number()over(order by [CustomerID] desc) as [rn]  FROM [CustomerInfo]  as t where (([CustomerID]+[FullNameTh]) LIKE @filter) and [IsActive]=1) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboCustomer_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [CustomerID],[FullNameTh] FROM [CustomerInfo] WHERE ([CustomerID] = @ID) ORDER BY [CustomerID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboLocation_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT [LocID],[CompanyID],[Name] FROM  ( SELECT  [LocID],[CompanyID],[Name] , row_number()over(order by [LocID] desc) as [rn]  FROM [LocationInfo]  as t where (([LocID]+[CompanyID]+[Name]    ) LIKE @filter) and [IsActive]=1) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboLocation_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT   [LocID],[CompanyID],[Name]   FROM [LocationInfo]   WHERE ([LocID] = @ID) ORDER BY [LocID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }
        protected void cbofilterby_SelectedIndexChanged(object sender, EventArgs e) {
            FiterbyChange();
        }
        private void FiterbyChange() {
            if (cbofilterby.SelectedValue != "") {
                switch (cbofilterby.SelectedValue) {
                    case "invdate":
                        divdateFilter.Visible = true;
                        break;                  
                }
            }
        }
        protected void chkShowClose_CheckedChanged(object sender, EventArgs e) {
            LoadData();
        }

        #endregion
    }

}