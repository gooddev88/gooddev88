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

using DevExpress.Web;
using Robot.POS.DA;
using Robot.Master.DA;

namespace Robot.POS {
    public partial class INVLineActive : MyBasePage
    {

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {
              
                LoadDropDownList();
                LoadData();

            }
        }
        private void SetQueryString() {

        }

        private void LoadDropDownList() {
            ARInvoiceService.ListDocLine();
            cboDocLine.DataSource = ARInvoiceService.DocSet.LineIndex;
            cboDocLine.DataBind();

        }
        private void LoadDropDownDevList() {
            cboVatType.DataSource = TaxInfoService.MiniSelectListV2("SALE", GetSelectCurrency(),false);
            cboVatType.DataBind();
        }
        private string GetSelectCurrency() {
            string result = "THB";
            result = ARInvoiceService.DocSet.Head.Currency;
            return result;
        }
        private void LoadData() {
            BindData();
        }
        private void ResetControl() {
            cboItem.Value = null;
            txtItemName.Text = ""; 
            txtPrice.Text = "0";
            txtQtyInv.Text = "0";
            txtBaseTotalAmt.Text = "0";
            txtTotalAmt.Text = "0";
            txtVatAmt.Text = "0";
            txtTotalAmtIncVat.Text = "0";
            ARInvoiceService.DocSet.LineActive.VatTypeID = ARInvoiceService.DocSet.Head.VatTypeID;
            txtRemark1.Text = "";
        }
        private void BindData() {
            if (ARInvoiceService.DocSet.LineActive.Status == "new") {
                lbldocid.Text = ARInvoiceService.DocSet.Head.SOINVID + " (++NEW++)";

                if (cboVatType.Value == null)
                {
                    cboVatType.Value = ARInvoiceService.DocSet.Head.VatTypeID;
                    ResetControl();
                }                          
            } else {
                lbldocid.Text = ARInvoiceService.DocSet.Head.SOINVID + "(++EDIT++)";
            }
            cboItem.Value = ARInvoiceService.DocSet.LineActive.ItemID;
            txtItemName.Text = ARInvoiceService.DocSet.LineActive.ItemName; 
            txtPrice.Text = ARInvoiceService.DocSet.LineActive.Price.ToString("N2");
            txtQtyInv.Text = ARInvoiceService.DocSet.LineActive.QtyInvoice.ToString("N2");
            txtBaseTotalAmt.Text = ARInvoiceService.DocSet.LineActive.BaseTotalAmt.ToString("N2");
            txtTotalAmt.Text = ARInvoiceService.DocSet.LineActive.TotalAmt.ToString("N2");
            txtVatAmt.Text = ARInvoiceService.DocSet.LineActive.VatAmt.ToString("N2");
            txtTotalAmtIncVat.Text = ARInvoiceService.DocSet.LineActive.TotalAmtIncVat.ToString("N2");
            cboVatType.Value = ARInvoiceService.DocSet.LineActive.VatTypeID;
            txtRemark1.Text = ARInvoiceService.DocSet.LineActive.Remark1;
            txtRemark2.Text = ARInvoiceService.DocSet.LineActive.Remark2; 
            cboDocLine.SelectedValue = ARInvoiceService.DocSet.LineActive.LineNum.ToString();
        }

        private void SetPrimaryData() {
          
            if (!ValidData()) {
                return;
            } 
            ARInvoiceService.DocSet.LineActive.ItemID = cboItem.Value.ToString();
            ARInvoiceService.DocSet.LineActive.ItemName = txtItemName.Text;
           
            ARInvoiceService.DocSet.LineActive.VatTypeID = cboVatType.Value != null ? cboVatType.Value.ToString() : "";
            ARInvoiceService.DocSet.LineActive.VatRate = TaxInfoService.GetRateByTaxID(ARInvoiceService.DocSet.LineActive.VatTypeID);

            ARInvoiceService.DocSet.LineActive.PointID = "";
            ARInvoiceService.DocSet.LineActive.PointName = ""; 

            decimal price = 0;
            decimal.TryParse(txtPrice.Text.Trim(), out price);
            ARInvoiceService.DocSet.LineActive.Price = price;

            decimal qty = 0;
            decimal.TryParse(txtQtyInv.Text.Trim(), out qty);
            ARInvoiceService.DocSet.LineActive.QtyInvoice = qty;

       
            decimal vatamt = 0;
            decimal.TryParse(txtVatAmt.Text.Trim(), out vatamt);
            ARInvoiceService.DocSet.LineActive.VatAmt = vatamt;

            decimal totalamtIncVat = 0;
            decimal.TryParse(txtTotalAmtIncVat.Text.Trim(), out totalamtIncVat);
            ARInvoiceService.DocSet.LineActive.TotalAmtIncVat = totalamtIncVat;

            ARInvoiceService.DocSet.LineActive.Remark1 = txtRemark1.Text;
            ARInvoiceService.DocSet.LineActive.Remark2 = txtRemark2.Text; 
    
            ARInvoiceService.CalTax();

            LoadData();
            LoadDropDownList();
            
         
        }

        #region Search area

       

        protected void cboItem_SelectedIndexChanged(object sender, EventArgs e) {
            string itemid = cboItem.Value == null ? "" : cboItem.Value.ToString();
            int linelinenum = ARInvoiceService.DocSet.LineActive.LineNum;
            var data = POSItemService.GetItem(itemid);
            if (data != null) {

                ARInvoiceService.DocSet.LineActive.ItemID = itemid;
                ARInvoiceService.DocSet.LineActive.ItemName = data.Name1;
                ARInvoiceService.DocSet.LineActive.Unit = data.UnitID;
                txtItemName.Text = data.Name1;
 

            }
        }
        protected void cboItem_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"select ItemID,Name1,TypeID,UnitID
                            from (select ItemID,Name1,TypeID,UnitID
                                    , row_number()over(order by ItemID desc) as [rn]  
                                                                    from ItemInfo  as t 
                                                                        where (( ItemID+Name1+TypeID ) like @filter) 
                                                                                and RCompanyID=@rcom 
                                                                                and TypeID != 'DISCOUNT' 
                                                                                and ItemID != 'DEFAULTMENU' 
                                                                                and TypeID= 'FGK'
                                                                                and IsActive=1
                                  ) as st
                           where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboItem_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"select ItemID,Name1,TypeID,UnitID from ItemInfo where ItemID = @ID and RCompanyID=@rcom order by ItemID";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        #endregion
        #region PopUp management 
        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }
        #endregion

        protected void btnSave_Click(object sender, EventArgs e) {
            SetPrimaryData();
            ARInvoiceService.DocSet.LineActive.Status = "OPEN";
            ShowAlert("เพิ่มรายการสำเร็จ", "Success");
        }
      
        protected void btnClose_Click(object sender, EventArgs e) {
            ClosePopup("OK-Line");
        }
        protected void btnCancel_Click(object sender, EventArgs e) {
            ClosePopup("Cancel-Line");
        }


        #region Function move line helper
        private void GotoNextLine() {

            if (ARInvoiceService.DocSet.Line.Count == 1) {
                if (ARInvoiceService.DocSet.Line.FirstOrDefault().LineNum != ARInvoiceService.DocSet.LineActive.LineNum) {
                    ARInvoiceService.DocSet.LineActive = ARInvoiceService.DocSet.Line.FirstOrDefault();
                }
                LoadData();
                LoadDropDownList();
                return;
            }
            //if has next row go to next row
            var nxt_line = ARInvoiceService.DocSet.Line.Where(o => o.LineNum > ARInvoiceService.DocSet.LineActive.LineNum).OrderBy(o => o.LineNum).FirstOrDefault();
            if (nxt_line == null) {//if it is last row go to first row
                nxt_line = ARInvoiceService.DocSet.Line.OrderBy(o => o.LineNum).FirstOrDefault();
            }
            ARInvoiceService.DocSet.LineActive = nxt_line;
            LoadData();
            LoadDropDownList();
        }

        private void GotoPreviousLine() {

            if (ARInvoiceService.DocSet.Line.Count() == 1) {
                ARInvoiceService.DocSet.LineActive = ARInvoiceService.DocSet.Line.FirstOrDefault();
                LoadData();
                LoadDropDownList();
                return;
            }
            //if has next row go to next row
            var previous_line = ARInvoiceService.DocSet.Line.Where(o => o.LineNum < ARInvoiceService.DocSet.LineActive.LineNum).OrderBy(o => o.LineNum).FirstOrDefault();
            if (previous_line == null) {//if it is first row go to last row
                previous_line = ARInvoiceService.DocSet.Line.OrderByDescending(o => o.LineNum).FirstOrDefault();
            }
            ARInvoiceService.DocSet.LineActive = previous_line;
            LoadData();
            LoadDropDownList();
        }
        private void GotoSpecificLine(int linenum) {
            ARInvoiceService.GetLineActive(linenum);
            LoadData();
            LoadDropDownList();
        }
    

        #endregion
        #region Control move line helper 
        protected void cboDocLine_SelectedIndexChanged(object sender, EventArgs e) {
            int linenum = 0;
            int.TryParse(cboDocLine.SelectedValue, out linenum);
            if (linenum == 0) {
                return;
            }
            GotoSpecificLine(linenum);
        }

        protected void btnNewLine_Click(object sender, EventArgs e) {
            ARInvoiceService.AddLine();
            LoadDropDownList();
            LoadData();
        }

        protected void btnBackward_Click(object sender, EventArgs e) {
            GotoPreviousLine();
        }

        protected void btnforward_Click(object sender, EventArgs e) {
            GotoNextLine();
        }

        protected void btnDel_Click(object sender, EventArgs e) {
            ARInvoiceService.DeleteLine(ARInvoiceService.DocSet.LineActive.LineNum); 
            ShowAlert("ลบรายการสำเร็จ", "Success");
            if (ARInvoiceService.DocSet.Line.Count() > 0) {
                GotoNextLine();
            } else {
                ARInvoiceService.AddLine(); 
                LoadDropDownList();
                LoadData();
            }
        }
        #endregion

        #region Valid data
        
        private void ShowAlert(string msg, string type)
        {
          
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private bool ValidData() {
          
            bool result = true;
            if (cboItem.Value == null) {
           
                ShowAlert("Input Item", "Error");
                result = false;
            }
            return result;
        }

        #endregion

        protected void txtBaseTotalAmt_TextChanged(object sender, EventArgs e) {
            decimal qty = 0;
            decimal price = 0;
            decimal base_amt = 0;
            try { qty = Convert.ToDecimal(txtQtyInv.Text); } catch { }
            try { base_amt = Convert.ToDecimal(txtBaseTotalAmt.Text); } catch { }
            if (qty != 0) {
                price = Math.Round(base_amt / qty, 6);
                txtPrice.Text = price.ToString("n6");
            }
            SetPrimaryData();
            BindData();
        }

        protected void txtPrice_TextChanged(object sender, EventArgs e) {
            decimal qty = 0;
            decimal price = 0;
            decimal base_amt = 0;
            try { qty = Convert.ToDecimal(txtQtyInv.Text); } catch { }
            try { price = Convert.ToDecimal(txtPrice.Text); } catch { }
            if (qty != 0) {
                base_amt = Math.Round(qty * price, 6);
                txtBaseTotalAmt.Text = base_amt.ToString("n6");
            }
            SetPrimaryData();
            BindData();
        }

        protected void txtQtyInv_TextChanged(object sender, EventArgs e) {
            decimal qty = 0;
            decimal price = 0;
            decimal base_amt = 0;
            try { qty = Convert.ToDecimal(txtQtyInv.Text); } catch { }
            try { price = Convert.ToDecimal(txtPrice.Text); } catch { }
            if (qty != 0) {
                base_amt = Math.Round(qty * price, 6);
                txtBaseTotalAmt.Text = base_amt.ToString("n6");
            }
            SetPrimaryData();
            BindData();
        }

        protected void cboVatType_SelectedIndexChanged(object sender, EventArgs e) {
            SetPrimaryData();
            BindData();
        }
    }

}