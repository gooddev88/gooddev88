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
 
using Robot.Data.BL;
using Robot.POS.DA;
using Robot.POS.Print;
using Robot.Master.DA;

namespace Robot.POS
{
    public partial class INVDetail : MyBasePage
    {
        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["oinvdetail_previouspage"]; } set { HttpContext.Current.Session["oinvdetail_previouspage"] = value; } }
        public static string ParamDocType { get { return (string)HttpContext.Current.Session["oinv_doctype"]; } set { HttpContext.Current.Session["oinv_doctype"] = value; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            SetQueryString();
            popFile.ShowOnPageLoad = false;
            popFile2.ShowOnPageLoad = false;   
            popAlert.ShowOnPageLoad = false;

            if(ARInvoiceService.IsNewDoc) {
                ShowPopAlert("Success", "Save successfull "  , true, "");
            }
            if (!string.IsNullOrEmpty(hddInvId.Value))
            {
                ARInvoiceService.GetDocSetByID(Convert.ToInt32(hddInvId.Value), false);
            }
            ARInvoiceService.checkDocSetNull();
            LoadDropDownDevList();
            if(!IsPostBack) {
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString()
        {
            hddmenu.Value = Request.QueryString["menu"];        
            hddTopic.Value = Request.QueryString["Topic"];
            hddInvId.Value = Request.QueryString["id"]; 
        }

        private void RefreshGridLine()
        {

        }
        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton)
        {
            if(result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }
            if(showbutton == "") {
                btnCancel.Visible = false;
            }
            if(showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            ARInvoiceService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        private void LoadDropDownList()
        {
         
        }
        private void LoadDropDownDevList()
        {           
            //var shipvendor = VendorInfoService.MiniSelectList("", "SERVICE PROVIDERS", false).ToList();           
            cboCurrency.DataSource = CurrencyInfoService.MiniSelectList(false);
            cboCurrency.DataBind();
            cboVatType.DataSource = TaxInfoService.MiniSelectListV2("SALE", "THB", false);
            cboVatType.DataBind();
            cboCreditTerm.DataSource = CreditTermInfoService.MiniSelectList(false, "AR");
            cboCreditTerm.DataBind();

            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", true);
            cboCompany.DataBind();
        }

        private void ShowStatusInfo()
        {
            OINVHead h = ARInvoiceService.DocSet.Head;
 
            if(h.SOINVID == "") {
                lblStatusInfo.Text = "++NEW++" + "<br />";
                return;
            } else {
                lblStatusInfo.Text = h.Status + "<br />";
                lblStatusInfo.Text = lblStatusInfo.Text + "Inovice No.: " + h.SOINVID;
                if(h.SODate != null) {
                    lblStatusInfo.Text = lblStatusInfo.Text + " @ : " + Convert.ToDateTime(h.SOINVDate).ToString("dd/MM/yyyy") + "<br />";
                }

                //if(h.IsLink) {
                //    lblStatusInfo.Text = lblStatusInfo.Text + "Link ERP  : " + Convert.ToDateTime(h.LinkDate).ToString("dd/MM/yyyy") + "<br />";
                //} else {
                //    lblStatusInfo.Text = lblStatusInfo.Text + "<strong><span style='font-size:large;color:red'>" + "No Link" + "</span></strong>" + "<br />";
                //}
                if(h.SOID != "") {
                    lblStatusInfo.Text = lblStatusInfo.Text + "Order No.: " + h.SOID;
                }
                if(h.SODate != null) {
                    lblStatusInfo.Text = lblStatusInfo.Text + " @ : " + Convert.ToDateTime(h.SODate).ToString("dd/MM/yyyy") + "<br />";
                }

                //if(h.ShipID != "") {
                //    lblStatusInfo.Text = lblStatusInfo.Text + "Ship No.: " + h.ShipID;
                //}
                //if(h.ShipDate != null) {
                //    lblStatusInfo.Text = lblStatusInfo.Text + " @ : " + Convert.ToDateTime(h.ShipDate).ToString("dd/MM/yyyy") + "<br />";
                //}

                lblStatusInfo.Text = lblStatusInfo.Text + "<strong><span>" + "ยอดอินวอยซ์ : " + Convert.ToDecimal(h.NetTotalAmtIncVat).ToString("N2") + "</span></strong>" + "<br />";
                
                lblStatusInfo.Text = lblStatusInfo.Text + "<strong><span>" + "ยอดชำระ : " + Convert.ToDecimal(h.RCAmt).ToString("N2") + "</span></strong>" + "<br />";
                lblStatusInfo.Text = lblStatusInfo.Text + "<strong><span>" + "ยอดค้างชำระ : " + Convert.ToDecimal(h.INVPendingAmt).ToString("N2") + "</span></strong>" + "<br />";
            }
            if(!h.IsActive) {
                lblStatusInfo.Text = h.Status + "<br />";
            }

            #region Caption Stauts
          
            string strstatus = "<span class=" + "\"" + "badge badge-pill badge-info" + "\"" + ">++NEW++</span>";
            if(h == null) {
                litStatus.Text = Server.HtmlDecode(strstatus);
                return;
            }
            if(!h.IsActive) {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-danger" + "\"" + ">DELETED</span>";
            }
            if(h.Status == "CLOSED") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-dark" + "\"" + ">CLOSE</span>";
            }
            if(h.Status == "PENDING") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-warning" + "\"" + ">PENDING</span>";
            }
            if(h.Status == "OPEN") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-success" + "\"" + ">OPEN</span>";
            }
            if(h.Status == "CANCEL") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-danger" + "\"" + ">CANCEL</span>";
            }
            litStatus.Text = Server.HtmlDecode(strstatus);
            #endregion
        }
       
        private void LoadData(){  
            BindData();       
            SetActiveControl();
        }      

        private void BindData()   {
            var h = ARInvoiceService.DocSet.Head;
            txtID.Text = h.SOINVID;
            if (txtID.Text != "")
            {
                cboCompany.Enabled = false;
            }
            dtDocumentDate.Value = h.SOINVDate;
            cboDocTo.Value = h.CustomerID;
            cboCompany.Value = h.CompanyID;
            txtCustomerName.Text = h.CustomerName;
            txtPONo.Text = h.POID;
            //cboGLGroup.Value = h.AccGroupID;
            txtBillAddr1.Text = h.BillAddr1;
            txtBillAddr2.Text = h.BillAddr2;
            txtCustTaxID.Text = h.CustTaxID;
            cboSaleman.Value = h.SalesID1;

            cboCurrency.Value = h.Currency;
            txtRateExchange.Text = h.RateExchange.ToString("N2");
            dtDateRate.Value = h.RateDate;
            cboCreditTerm.Value = h.TermID;

            dtPaymentDate.Value = h.PayDueDate;
            cboVatType.Value = h.VatTypeID;
            cboDiscBy.SelectedValue = h.DiscCalBy == "" ? "A" : h.DiscCalBy;
            txtOntopDisAmt.Text = h.OntopDiscAmt.ToString("N2");
            txtOntopDisPer.Text = h.OntopDiscPer.ToString("N2");
            ChangeDiscount();
          
            txtRemark2.Text = h.Remark2;
            //txtRemark1.Text = h.Remark1;
            //txtRemark3.Text = h.Remark3;
            txtPaymentMemo.Text = h.PaymentMemo;

            lblBaseTotalAmt.Text = h.BaseNetTotalAmt.ToString("N2");
            lblTotalAmt.Text = h.NetTotalAmt.ToString("N2");
            lblVatAmt.Text = h.NetTotalVatAmt.ToString("N2");
            lblTotalAmtIncVat.Text = h.NetTotalAmtIncVat.ToString("N2");

            ShowStatusInfo();
            grdlinex.DataBind();      
            GridTransactionLogBind(); 
            SetupCurrency();
            SetupGLGroupBinding();
            SetupVatBinding();    
        }
        private void SetActiveControl()
        {
            var h = ARInvoiceService.DocSet.Head;
            if(h.SOINVID == "") {
                btnCopy.Visible = false;
                btnDel.Visible = false;
                btnNewDoc.Visible = false;

            } else {
                tab_moreinfoX.Visible = true;
                txtID.ReadOnly = true;
                btnCopy.Visible = true;
            }
            //btnPrintOINV1.Visible = false;
            txtID.ReadOnly = true;
         
            if(h.IsLink == true) {
                btnSave.Visible = false;
                btnDel.Visible = false;
            }
            if(h.IsActive == false) {
                btnSave.Visible = false;
                btnDel.Visible = false;
            }
          
            if (!string.IsNullOrEmpty( h.RCID)) {
                btnDel.Visible = false;
                btnSave.Visible = false;             
            }

            if (h.SOINVID == "")
            {
                btnPrintOINV1.Visible = false;
            }

        }

        private void GridTransactionLogBind()
        {
            grd_transaction_log.DataSource = ARInvoiceService.DocSet.Log;
            grd_transaction_log.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(PreviousPageX);
        }

        protected void cboDiscBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDiscount();
        }
        private void ChangeDiscount()
        {
            if(cboDiscBy.SelectedValue == "P") {
                txtOntopDisPer.ReadOnly = false;
                txtOntopDisAmt.ReadOnly = true;
            }
            if(cboDiscBy.SelectedValue == "A") {
                txtOntopDisPer.ReadOnly = true;
                txtOntopDisAmt.ReadOnly = false;
            }
        }

        #region componenet

        private bool SetPrimaryHeadData()
        {
            var h = ARInvoiceService.DocSet.Head;

            if(cboDocTo.Value == null) {
                ShowPopAlert("Warning", "Select customer", false, "");
                return false;
            }
            if(cboDocTo.Value.ToString() == "") {
                ShowPopAlert("Warning", "Select customer", false, "");
                return false;
            }

            if(dtDocumentDate.Value == null) {
                ShowPopAlert("Warning", "Select Document Date", false, "");
                return false;
            }            

            h.CustomerID = cboDocTo.Value.ToString();
            h.CustomerName = txtCustomerName.Text;
            h.POID = txtPONo.Text;
            h.SOINVDate = dtDocumentDate.Date;
            //h.AccGroupID = cboGLGroup.Value != null ? cboGLGroup.Value.ToString() : "";
            h.CompanyID = cboCompany.Value.ToString();
            h.ShipFrLocID = "";
            h.CustTaxID = txtCustTaxID.Text;

            h.BillAddr1 = txtBillAddr1.Text;
            h.BillAddr2 = txtBillAddr2.Text;
            h.DiscCalBy = cboDiscBy.SelectedValue;

            try { h.OntopDiscAmt = Convert.ToDecimal(txtOntopDisAmt.Text); } catch { h.OntopDiscAmt = 0; }
            try { h.OntopDiscPer = Convert.ToDecimal(txtOntopDisPer.Text); } catch { h.OntopDiscPer = 0; }
            decimal rate = 1; 
            decimal.TryParse(txtRateExchange.Text, out rate);
            h.RateExchange = rate;
         
            h.TermID = cboCreditTerm.Value==null?"": cboCreditTerm.Value.ToString();

            h.PayDueDate = null;
            h.PayDueDate = CreditTermInfoService.GetPaymentDueDateFromTerm(h.TermID, h.SOINVDate, "AR");
 
            h.SalesID1 = cboSaleman.Value != null ? cboSaleman.Value.ToString() : "";

            h.VatTypeID = cboVatType.Value == null ? "" : cboVatType.Value.ToString();
            h.VatRate = h.VatTypeID == "" ? 0 : TaxInfoService.GetRateByTaxID(h.VatTypeID);

            h.Remark2 = txtRemark2.Text; 
            h.PaymentMemo = txtPaymentMemo.Text;
            return true;
        }

        protected void grdlinex_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            int linenum = 0;
            if(e.CommandArgs.CommandName == "show") {
                ARInvoiceService.GetLineActive(Convert.ToInt32(e.KeyValue));
                popFile.ContentUrl = "~/POS/INVLineActive";
                popFile.ShowOnPageLoad = true;
            }

            if(e.CommandArgs.CommandName == "del") {
                ARInvoiceService.DeleteLine(Convert.ToInt32(e.KeyValue));
                grdlinex.DataBind();

            }
            if(e.CommandArgs.CommandName == "so") {
                linenum = Convert.ToInt32(e.KeyValue);

            }
            if(e.CommandArgs.CommandName == "ship") {
                linenum = Convert.ToInt32(e.KeyValue);

            }
        }

        public List<OINVLine> DSLineX()
        {
            return ARInvoiceService.DocSet.Line;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void cboDocTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cusid = "";
            if(cboDocTo.Value != null) {
                cusid = cboDocTo.Value.ToString();
                SetDocToInfo(cusid);
            }
            cboDocTo.Items.Clear();
        }

        private void SetDocToInfo(string cusid)
        {
            var h = ARInvoiceService.DocSet.Head;
            var data = CustomerInfoService.GetDataByID(cusid);

            if(data != null) {
                txtCustomerName.Text = data.FullNameTh;
                txtBillAddr1.Text = data.BillAddr1;
                txtBillAddr2.Text = data.BillAddr2;

                cboCreditTerm.Value = data.PaymentTermID;
                txtCustTaxID.Text = data.TaxID;
                h.TermID = data.PaymentTermID;          
                h.PayDueDate = CreditTermInfoService.GetPaymentDueDateFromTerm(h.TermID, h.SOINVDate, "AR");
                              
                cboCurrency.Value = "THB";
                if(!string.IsNullOrEmpty(data.Currency)) {
                    cboCurrency.Value = data.Currency;
                }
                try { cboVatType.Value = data.VatTypeID; } catch { }

                SetupCurrency();
                SetupGLGroupBinding();
                SetupVatBinding();

            }
        }



        private void SetupCurrency()
        {
            GetCurrency();
            GetCurrencyRate();
        }
        private void GetCurrency()
        {
            var h = ARInvoiceService.DocSet.Head;
            string result = "THB";
            if(cboCurrency.Value != null) {
                result = cboCurrency.Value.ToString();
            }
            h.Currency = result;
            h.RateBy = "PB";
        }

        private void GetCurrencyRate()
        {
            GetCurrency();
            var h = ARInvoiceService.DocSet.Head;
            DateTime rateDate = DateTime.Now.Date;
            if(dtDateRate.Value != null) {
                rateDate = dtDateRate.Date;
            }
            decimal rate = CurrencyInfoService.GetExchangeRate(h.CompanyID, h.Currency, h.RateBy, rateDate);
            h.RateDate = rateDate;
            h.RateExchange = rate;

            txtRateExchange.Text = h.RateExchange.ToString("n6");

        }
        private void SetupVatBinding()
        {
            SetupCurrency();
            var h = ARInvoiceService.DocSet.Head;
            cboVatType.DataSource = TaxInfoService.MiniSelectListV2("SALE", h.Currency, false);
            cboVatType.DataBind();
        }
        private void SetupGLGroupBinding()
        {
            SetupCurrency();
            var h = ARInvoiceService.DocSet.Head;
            //var glgrp = GLGroupService.MiniSelectList("AP").Where(o => o.CompanyID == h.CompanyID && o.Currency == h.Currency).ToList();
            //cboGLGroup.DataSource = glgrp;
            //cboGLGroup.DataBind();
        }
        protected void cboCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupCurrency();
            SetupGLGroupBinding();
            SetupVatBinding();

        }
        protected void dtDateRate_DateChanged(object sender, EventArgs e)
        {
            SetupCurrency();
        }



        #region ddl Big datasource
        protected void cboDocTo_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            //string company = cboCompany.Value == null ? "" : cboCompany.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT [CustomerID],[NameDisplay]  FROM (
                                                                SELECT [CustomerID]
                                                                        ,[NameTh1]+' '+[NameTh2] as [NameDisplay] 
                                                                        , row_number()over(order by [CustomerID] desc) as [rn]  
                                                                           FROM [CustomerInfo] as t 
                                                                where (([CustomerID]+[NameTh1]+[NameTh2]) LIKE @filter) 
                                                                        and RCompanyID=@rcom 
                                                                        and [IsActive]=1) as st 
                                                    where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, string.Format("{0}", rcom));
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboDocTo_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [CustomerID],[NameTh1]+' '+[NameTh2] as [NameDisplay] 
                                        FROM [CustomerInfo] 
                                        WHERE ([CustomerID] = @ID) 
                                                and RCompanyID=@rcom 
                                                ORDER BY [CustomerID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, string.Format("{0}", rcom));
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }


        #endregion

        protected void btnAddline_Click(object sender, EventArgs e)
        {
            if(!SetPrimaryHeadData()) {
                return;
            }
            ARInvoiceService.AddLine();
            //string url = "/AR/INVLineActive";
            //Response.Redirect(url);
            popFile.ContentUrl = "~/POS/INVLineActive";
            popFile.ShowOnPageLoad = true;
        }


        protected void btnAddSaleOrder_Click(object sender, EventArgs e)
        {
            if(!SetPrimaryHeadData()) {
                return;
            }
            popFile.ContentUrl = "~/POS/INVSOSelect"; 
            popFile.ShowOnPageLoad = true;
        }

     
        protected void btnLoadLine_Click(object sender, EventArgs e)
        {
            ARInvoiceService.ClearPendingLine();

            BindData();
            grdlinex.DataBind();     
        }
        #endregion

        #region fucntion move doc helper     

        private void GotoNextDoc()
        {
            var h = ARInvoiceService.DocSet.Head;

            ARInvoiceService.GoNextDoc(h.ID);
            string url = $"~/POS/INVDetail?menu={hddmenu.Value}";
            Response.Redirect(url);
        }
        private void GotoPreviousDoc()
        {
            var h = ARInvoiceService.DocSet.Head;

            ARInvoiceService.GoPreviousDoc(h.ID);
            string url = $"~/POS/INVDetail?menu={hddmenu.Value}";
            Response.Redirect(url);
        }

        #endregion

        #region Control On toolbar
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidData()) {
                return;
            }
            var isnew = PrepairDataSave();
            if(isnew) {
                ARInvoiceService.Save("insert");
            } else {
                ARInvoiceService.Save("update");
            }
            if(ARInvoiceService.DocSet.OutputAction.Result == "ok") {//save successufull
                if(isnew) {
                    ARInvoiceService.IsNewDoc = true;
                    string url = $"~/POS/INVDetail?menu={hddmenu.Value}";
                    Response.Redirect(url);
                } else {
                    ShowPopAlert("Result", "Save document successfull", true, "");
                    LoadData();
                }
            } else {
                if (isnew) {
                    ARInvoiceService.DocSet.Head.SOINVID = "";
                }
                ShowPopAlert("Error", ARInvoiceService.DocSet.OutputAction.Message1, false, "");
            }
        }
        protected void btnRefreshDoc_Click(object sender, EventArgs e)
        {
            string invid = ARInvoiceService.DocSet.Head.SOINVID;
            string comid = ARInvoiceService.DocSet.Head.CompanyID;
            ARInvoiceService.RefreshSet(invid);
            string url = $"~/POS/INVDetail?menu={hddmenu.Value}";
            Response.Redirect(url);
        }


        protected void btnDel_Click(object sender, EventArgs e)
        {
            var h = ARInvoiceService.DocSet.Head;
            var r = ARInvoiceService.DeleteDoc(h.ID);
            if(r.Result == "ok") {
                Response.Redirect(PreviousPageX);
            } else {
                ShowPopAlert("Error", r.Message1, false, "");
            }

        }

        protected void btnNewDoc_Click(object sender, EventArgs e)
        {

            INVNewDoc.PreviousPageX = HttpContext.Current.Request.Url.PathAndQuery;
            INVNewDoc.ParamDocType = "INV";
            string myurl = $"~/POS/INVNewDoc?&menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);


        }
        protected void btnCopy_Click(object sender, EventArgs e)
        {

            var h = ARInvoiceService.DocSet.Head;
            ARInvoiceService.GetDocSetByID(h.ID, true);
            string url = $"~/POS/INVDetail?menu={hddmenu.Value}";
            Response.RedirectPermanent(url);
        }
        protected void btnBackward_Click(object sender, EventArgs e)
        {
            Response.Redirect(PreviousPageX);
        }

        protected void btnforward_Click(object sender, EventArgs e)
        {
            GotoNextDoc();
        }
        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(ARInvoiceService.PreviousPage);
        }
        protected void cboDocLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strid = cboDocLine.Value == null ? "0" : cboDocLine.Value.ToString();

            if(strid == "0") {
                return;
            }
            var id = Convert.ToInt32(strid);
            ARInvoiceService.GoPreviousDoc(id);
            string url = $"~/POS/INVDetail?menu={hddmenu.Value}";
            Response.Redirect(url);
        }
        protected void cboDocLine_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT  [ID],[SOINVID],[CustomerID],[CustomerName] FROM ( SELECT  [ID], [SOINVID],[CustomerID],[CustomerName] , row_number()over(order by [ID] desc) as [rn]  FROM [OINVHead]  as t where (( [SOINVID]+[CustomerID]+[CustomerName]) LIKE @filter) and [IsActive]=1) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboDocLine_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if(e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT [ID],[SOINVID],[CustomerID],[CustomerName] FROM [OINVHead] WHERE ([ID] = @ID) ORDER BY [ID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.Int32, e.Value.ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }
        protected void cboShipToLoc_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT  [LocID],[Name]   FROM  ( SELECT [LocID],[Name] , row_number()over(order by [LocID] desc) as [rn] FROM [LocationInfo]  as t where ((  [LocID]+[Name]     ) LIKE @filter) and [IsActive]=1) as st where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboShipToLoc_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if(e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT   [LocID],[Name]   FROM [LocationInfo]   WHERE ([LocID] = @ID) ORDER BY [LocID]";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboSaleman_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"SELECT Username,FullName,DepartmentID FROM ( SELECT Username,FullName,DepartmentID , row_number()over(order by t.Username) as rn  
            FROM vw_UserInfo  as t where ((Username + ' ' + FullName +' '+DepartmentID  ) LIKE @filter) and [DepartmentID] IN ('Amity','REG','SALESZIC') and [IsActive]=1 )  as st where st.rn between @startIndex and @endIndex";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            cboSaleman.DataSource = sqlSearch;
            cboSaleman.DataBind();
        }

        protected void cboSaleman_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if(e.Value == null) {
                return;
            }
            string value = "";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"SELECT Username,FullName,DepartmentID FROM vw_UserInfo WHERE (Username = @ID) ORDER BY Username";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            cboSaleman.DataSource = sqlSearch;
            cboSaleman.DataBind();
        }

        #endregion

        #region Valid date

        private bool ValidData()
        {


            //if(cboDocTo.Value == null) {
            //    ShowPopAlert("Warning", "Select vendor", false, "");
            //    return false;
            //}
            //if(cboDocTo.Value.ToString() == "") {
            //    ShowPopAlert("Warning", "Select vendor", false, "");
            //    return false;
            //}


            string RateExchange = txtRateExchange.Text;
            if(Convert.ToDecimal(RateExchange) <= 0) {
                ShowPopAlert("Warning", "Rate ต้องไม่เท่ากับ 0", false, "");
                return false;
            }

            if(dtDocumentDate.Value == null) {
                ShowPopAlert("Warning", "Select PO date", false, "");
                return false;
            }
            if(dtDateRate.Value == null) {
                ShowPopAlert("Warning", "Select rate date", false, "");
                return false;
            }
            if(cboVatType.Value == null) {
                ShowPopAlert("Warning", "Select VAT type", false, "");
                return false;
            }
            if(cboVatType.Value.ToString() == "") {
                ShowPopAlert("Warning", "Select VAT type", false, "");
                return false;
            }
            if(cboCurrency.Value == null) {
                ShowPopAlert("Warning", "Select currency", false, "");
                return false;
            }
            if(cboCreditTerm.Value == null) {
                ShowPopAlert("Warning", "Select Term", false, "");
                return false;
            }
            if(cboCreditTerm.Value.ToString() == "") {
                ShowPopAlert("Warning", "Select Term", false, "");
                return false;
            }
            if(dtDateRate.Value == null) {
                ShowPopAlert("Warning", "Select Rate date", false, "");
                return false;
            }
            return true;
        }


        private bool PrepairDataSave()
        {
            SetPrimaryHeadData();
            var h = ARInvoiceService.DocSet.Head;
            bool isnewrecord = h.SOINVID == "" ? true : false;
            ARInvoiceService.ClearPendingLine();
            if(h.SOINVID == "") {

                if(txtID.Text.Trim() != "") {//input doc by user
                    h.SOINVID = txtID.Text.Trim().ToUpper();
                } else {//gen doc id by program
                    var id = IDRuunerService.GetNewID("OINV", h.CompanyID, false, "th", h.SOINVDate)[1];
                    if(id == "") {
                        ShowPopAlert("Erro", "No ducument type found.", false, "");
                        return true;
                    }
                    h.SOINVID = id;
                }
            }
             
            return isnewrecord;
        }
        #endregion

        protected void btnPrintOINV1_Click(object sender, EventArgs e)
        {
            var h = ARInvoiceService.DocSet.Head;
            MyPrint.NewReportFItler();
            var f = MyPrint.ReportFilterX;

            f.DocID = h.SOINVID;
            f.DocType = h.DocTypeID;
            //f.ReportID = "INVGA01";
            MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;

            string myurl = "~/POS/Print/MyPrint?report=INVGA01";
            Response.RedirectPermanent(myurl);
        }

    }

}