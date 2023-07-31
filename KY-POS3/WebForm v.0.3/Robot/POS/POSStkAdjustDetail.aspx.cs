using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
using Robot.POS.DA;

namespace Robot.POS
{
    public partial class POSStkAdjustDetail : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
          
            SetQueryString();
            LoadDevDropDownList();
            if(POSStkAdjustService.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ ", true, "");
            }

            if(!Page.IsPostBack) {
           
                
                LoadDropDownList(); 
                LoadData();
            }
        }

        private void SetQueryString() {        
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void CheckPermission() {
            var h = POSStkAdjustService.DocSet.head;
            if(h.AdjID == "") {
                if(!PermissionService.CanCreate(hddmenu.Value)) {
                    btnSave.Visible = false;
                }
            }
        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if(result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }
           
            if(showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            POSStkAdjustService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        private void ShowPopDeleteAlert(string msg_header, string msg_body, bool result, string showbutton)
        {
            if (result)
            {
                lblHeaderMsgDelete.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblHeaderMsgDelete.ForeColor = System.Drawing.Color.Red;
            }

            if (showbutton == "okcancel")
            {
            }
            lblHeaderMsgDelete.Text = msg_header;
            lblBodyMsgDelete.Text = msg_body;
            POSStkAdjustService.IsNewDoc = false;
            popDeleteAlert.ShowOnPageLoad = true;
        }

        private void LoadData() {       
            BindData();  
            SetActiveControl();
            GridBinding();
        }

        private void BindData() {

            lblDocinfo.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;

            var h = POSStkAdjustService.DocSet.head;
            lblAdjID.Text = h.AdjID;
            lblStockDate.Text = Convert.ToDateTime(h.CreatedDate).ToString("dd/MM/yyyy HH:mm");
            var com = CompanyService.GetCompanyInfo(h.ComID);
            lblCompany.Text = "สาขา " + com.Name1 + " " + com.Name2 + " (" + com.CompanyID + ")";
            txtRemark1.Text = h.Remark;
            dtAdjDate.Value = h.AdjDate;
        }

        private void SetActiveControl() {
            var h = POSStkAdjustService.DocSet.head;

            string strstatus = "<span class=" + "\"" + "badge badge-pill badge-success" + "\"" + ">OPEN</span>";
            if (h == null)
            {
                litStatus.Text = Server.HtmlDecode(strstatus);
                return;
            }
            if (h.Status == "CLOSED")
            {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-danger" + "\"" + ">CLOSED</span>";
                btnSave.Visible = false;
                btnDel.Visible = false;
                btnCloseDoc.Visible = false;
                btnLoadStockBalFGK.Visible = false;
                btnLoadStockBalRMK.Visible = false;
            }
            litStatus.Text = Server.HtmlDecode(strstatus);
        }

        private void LoadDropDownList() {
            BindLocation();
        }

        private void LoadDevDropDownList() {
            cboItem.DataSource = POSItemService.ListViewItemByType("").Where(o => o.TypeID != "DISCOUNT").ToList();
            cboItem.DataBind();
        }

        private bool PrepairDataSave()
        {
            var h = POSStkAdjustService.DocSet.head;
            bool isnewrecord = h.AdjID == "" ? true : false;
            if (isnewrecord)
            {
                h.AdjID = IDRuunerService.GetNewID("ADJUST", h.ComID, false, "th", h.AdjDate)[1];
            }
            h.AdjDate = dtAdjDate.Date;
            h.Remark = txtRemark1.Text;
            SetLineData();
            return isnewrecord;
        }

        private void SetPrimaryData() {
            var h = POSStkAdjustService.DocSet.head;
            h.AdjDate = dtAdjDate.Date;
            h.Remark = txtRemark1.Text;
        }


        private bool ValidData() {

            if (dtAdjDate.Value == null)
            {
                ShowPopAlert("Warning", "ระบุ วันที่ปรับสต็อก", false, "");
                return false;
            }

            return true;
        }
 
        protected void btnDel_Click(object sender, EventArgs e) {
            var h = POSStkAdjustService.DocSet.head;
            //POSStkAdjustService.DeleteDoc(h.AdjID);
            //if(POSStkAdjustService.DocSet.OutputAction.Result == "ok") {             
            //    Response.Redirect($"/POS/POSStkAdjustList?&menu={hddmenu.Value}");
            //} else {
            //    ShowPopAlert("Error", "Delete failed " + Environment.NewLine + POSStkAdjustService.DocSet.OutputAction.Message1, false, "");
            //    return;
            //}
            ShowPopDeleteAlert("ลบเอกสาร", "คุณต้องการ ลบเอกสารนี้หรือไม่ ", false, "");
        }   

        private void GoByRedirect() {
            string myurl = $"~/POS/POSStkAdjustDetail?&menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            var h = POSStkAdjustService.DocSet.head;
            if(!ValidData()) {
                return;
            }
        
            var isnew = PrepairDataSave();
            if (isnew)
            {
                POSStkAdjustService.Save("insert");
            }
            else
            {
                POSStkAdjustService.Save("update");
            }
            if(POSStkAdjustService.DocSet.OutputAction.Result == "ok") {//save successufull
                if(isnew) {
                    POSStkAdjustService.IsNewDoc = true;
                    POSStkAdjustService.GetDocSetByID(h.ID);
                    Response.Redirect(Request.RawUrl);
                } else {
                    ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                    LoadData();
                }
            } else {
                ShowPopAlert("Error", POSStkAdjustService.DocSet.OutputAction.Message1, false, "");
            }
        }


        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(POSStkAdjustService.DetailPreviousPage);
        }      

        protected void btnCancel_Click(object sender, EventArgs e) {

        }

        #region Order Line
        protected void btnAddLine_Click(object sender, EventArgs e)
        {
            if (!ValidLineData())
            {
                return;
            }

            POS_StkAdjustLine newLine = POSStkAdjustService.NewLine();
            var loc = cboLocation.SelectedValue.ToString();
            var item = POSItemService.GetItem(cboItem.Value.ToString());
            var rs = POSStockService.GetStkBalByLoc(cboItem.Value.ToString(), POSStkAdjustService.DocSet.head.ComID,loc);
            newLine.ItemID = item.ItemID;
            newLine.LocID = cboLocation.SelectedValue.ToString().Replace("X", "");
            newLine.Name = item.Name1;
            newLine.Unit = item.UnitID;
            newLine.BeginQty = rs == null ? 0 : rs.BalQty;
            newLine.Price = item.Price;
            decimal qty = 0;
            decimal.TryParse(txtQty.Text, out qty);
            newLine.ActualQty = qty;
            newLine.AdjQty= Convert.ToDecimal( newLine.BeginQty) - Convert.ToDecimal(newLine.ActualQty);
            var r = POSStkAdjustService.NewLineByItem(newLine, newLine.ItemID);
            if (r.Result == "fail") {
                ShowPopAlert("Error", r.Message1, false, "");
            } else {
                ResetControl();
                GridBinding();
            } 

        }

        private void ResetControl()
        {
            txtQty.Text = "0";
            cboItem.Value = null;
        }

        private bool ValidLineData()
        {

            if (cboItem.Value == null)
            {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }

            if (cboItem.Value.ToString() == "")
            {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }

            return true;
        }

        private void SetLineData()
        {
            foreach (ListViewDataItem item in grdline.Items)
            {
                Label lblLinenum = item.FindControl("lblLineNum") as Label;
                TextBox txtQty = item.FindControl("txtQty") as TextBox;

                if (txtQty == null)
                {
                    continue;
                }

                decimal qty = 0;
                var r = decimal.TryParse(txtQty.Text, out qty);

                int linenum = 0;
                int.TryParse(lblLinenum.Text, out linenum);

                var this_line = POSStkAdjustService.DocSet.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                this_line.ActualQty = qty < 0 ? 0 : qty;
            }
        }
        private void BindLocation() {
            var h = POSStkAdjustService.DocSet.head;
            cboLocation.DataSource = LocationInfoService.ListStockLocation("", h.ComID, false);
            cboLocation.DataBind();
        }
        private void GridBinding()
        {
            grdline.DataSource = POSStkAdjustService.DocSet.line.OrderBy(o => o.LineNum).ToList();
            grdline.DataBind();
        }

        protected void grdline_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                POSStkAdjustService.DeleteLine(id);
                GridBinding();
            }
        }

        protected void grdlist_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            LinkButton btnDelete = (e.Item.FindControl("btnDelete") as LinkButton);
        }

        protected void grdline_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            (grdline.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }

        #endregion

        protected void btnNew_Click(object sender, EventArgs e) {
            POSStkAdjustService.NewTransaction();
            string myurl = $"~/POS/POSStkAdjustDetail?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

        protected void btnLoadStockBalFGK_Click(object sender, EventArgs e)
        {
            var h = POSStkAdjustService.DocSet.head;
            var data = POSStockService.ListPOS_STKBal( h.ComID,"FGK");

            POSStkAdjustService.STKBalConvert2AdjustLine(data);
            GridBinding();            
        }

        protected void btnLoadStockBalRMK_Click(object sender, EventArgs e)
        {
            var h = POSStkAdjustService.DocSet.head;
            var data = POSStockService.ListPOS_STKBal( h.ComID,"RMK");

            POSStkAdjustService.STKBalConvert2AdjustLine(data);
            GridBinding();            
        }

        protected void btnCloseDoc_Click(object sender, EventArgs e)
        {
            var h = POSStkAdjustService.DocSet.head;
            POSStkAdjustService.SetStatusCLOSE(h.AdjID);
            LoadData();
        }

        protected void btnConfirmDel_Click(object sender, EventArgs e)
        {
            var h = POSStkAdjustService.DocSet.head;
            POSStkAdjustService.DeleteDoc(h.AdjID);
            if (POSStkAdjustService.DocSet.OutputAction.Result == "ok")
            {
                Response.Redirect($"~/POS/POSStkAdjustList?&menu={hddmenu.Value}");
            }
            else
            {
                ShowPopAlert("Error", "Delete failed " + Environment.NewLine + POSStkAdjustService.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }
    }
}