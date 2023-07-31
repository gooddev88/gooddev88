using System;
using System.Web;


using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;
using Robot.POS.DA;
using Robot.Data;
using System.Collections.Generic;

namespace Robot.POS {
    public partial class RCList : MyBasePage {
        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["rc_previouslistpage"]; } set { HttpContext.Current.Session["rc_previouslistpage"] = value; } }
        public static List<vw_ORCHead> DocList { get { return (List<vw_ORCHead>)HttpContext.Current.Session["vwrcdoc_list"]; } set { HttpContext.Current.Session["vwrcdoc_list"] = value; } }
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
            if (ORCService.FilterSet == null) {
                ORCService.NewFilterSet();
            }

            chkShowClose.Checked = ORCService.FilterSet.ShowClosed;

            dtBegin.Value = ORCService.FilterSet.DateFrom;
            dtEnd.Value = ORCService.FilterSet.DateTo;
            txtSearch.Text = ORCService.FilterSet.SearchText;
            cbofilterby.SelectedValue = ORCService.FilterSet.SearchBy;
            FiterbyChange();


        }
        private void SetDefaultFilter() {
            ORCService.NewFilterSet();

            ORCService.FilterSet.ShowClosed = chkShowClose.Checked;
            ORCService.FilterSet.DateFrom = dtBegin.Date;
            ORCService.FilterSet.DateTo = dtEnd.Date;
            ORCService.FilterSet.SearchText = txtSearch.Text.Trim();
            ORCService.FilterSet.SearchBy = cbofilterby.SelectedValue;
        }


        private void LoadDropDownDevList() {

        }

        private void LoadData() {
           
                SetDefaultFilter();
                DocList = ORCService.ListDoc();
                GridBinding();
                SetActiveControl();
          
        }

        private void SetActiveControl() {
            hddTopic.Value = "O Receipt ";
        }
        private void GridBinding() {
        
            grdDetail.DataBind();
            grdDetail.DataSource = DocList;
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            SetDefaultFilter();
            LoadData();
        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "sel_row") {
                ASPxGridView grid = (ASPxGridView)sender;
                int id = Convert.ToInt32(e.KeyValue);
                ORCService.GetDocSetByID(id);
                RCDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
                string url = $"../POS/RCDetail?menu={hddmenu.Value}";
                Response.Redirect(url);
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = DocList;
        }

        protected void btnNew_Click(object sender, EventArgs e) {
            RCSelectCompany.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            RCDetail.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"../POS/RCSelectCompany?menu={hddmenu.Value}";
            Response.Redirect(url);
        }

        #region Big data source
        protected void cboCustomer_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            string rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT 
                                 [ID],[CustomerID],[FullNameTh]  
                    FROM ( 
                                    SELECT 
                                            [ID],[CustomerID],[FullNameTh] , row_number()over(order by [CustomerID] desc) as [rn]  
                                    FROM [CustomerInfo] as t 
                                    where (
                                            ([CustomerID]+[FullNameTh]) LIKE @filter) 
                                                    and RCompanyID=@rcom
                                                    and [IsActive]=1
                                            ) as st 
                where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, string.Format("{0}", rcom));
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboCustomer_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            string rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [ID],[CustomerID],[FullNameTh] FROM [CustomerInfo] WHERE ([CustomerID] = @ID) and RCompanyID=@rcom ORDER BY [CustomerID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, string.Format("{0}", rcom));
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
                    case "DOCDATE":
                        divdateFilter.Visible = true;
                        divsearch.Visible = true;
                        break;

                    case "TEXT":
                        divdateFilter.Visible = false;
                        divsearch.Visible = true;
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