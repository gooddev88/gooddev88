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
using Robot.Data.ServiceHelper;
using Robot.POSC.DA;
using Robot.POS.Print;

namespace Robot.POS {
    public partial class POSPODetail : MyBasePage {
        public static bool ShowHideLine { get { return HttpContext.Current.Session["showhideline"] == null ? true : (bool)HttpContext.Current.Session["showhideline"]; } set { HttpContext.Current.Session["showhideline"] = value; } }
        public static bool ShowHideLineFG { get { return HttpContext.Current.Session["showhidelinefg"] == null ? true : (bool)HttpContext.Current.Session["showhidelinefg"]; } set { HttpContext.Current.Session["showhidelinefg"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {

            SetQueryString();
            RefreshGridPOFGLine();
            LoadDevDropDownList();
            if (POS_POService.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ ", true, "");
            }

            if (!Page.IsPostBack) {
                ShowHideLine = true;
                ShowHideLineFG = true;
                LoadDropDownList();
                LoadData();
                InitDisplayDiv();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
            hddshowhiddenfunction.Value= Request.QueryString["x"];
        }

        private void CheckPermission() {
            var h = POS_POService.DocSet.head;
            if (h.POID != "") {
                if (!PermissionService.CanEdit(hddmenu.Value)) {
                    btnSave.Visible = false;
                }
                if (!PermissionService.CanEdit(hddmenu.Value)) {
                    if (h.Status == "RECEIVED") {
                        btnSave.Visible = false;
                    }
                }
                if (!PermissionService.CanDelete(hddmenu.Value)) {
                    if (h.Status != "RECEIVED") {
                        btnDel.Visible = false;
                    }
                }               
            }

        }

        private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
            }

            if (showbutton == "okcancel") {
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            POS_POService.IsNewDoc = false;
            popAlert.ShowOnPageLoad = true;
        }

        private void ShowPopDeleteAlert(string msg_header, string msg_body, bool result, string showbutton) {
            if (result) {
                lblHeaderMsgDelete.ForeColor = System.Drawing.Color.Green;
            } else {
                lblHeaderMsgDelete.ForeColor = System.Drawing.Color.Red;
            }

            if (showbutton == "okcancel") {
            }
            lblHeaderMsgDelete.Text = msg_header;
            lblBodyMsgDelete.Text = msg_body;
            POS_POService.IsNewDoc = false;
            popDeleteAlert.ShowOnPageLoad = true;
        }

        private void LoadData() {
            BindData();
            SetActiveControl();
            GridBinding();
            GridPOFGLineBinding();
        }

        private void BindData() {
            lblDocinfo.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;

            var h = POS_POService.DocSet.head;
            if (h.POID == "") {
                lblPOID.Text = "++NEW++";
                btnDel.Visible = false;
                btnPrintPO.Visible = false;
            } else {
                lblPOID.Text = h.POID;
                btnPrintPO.Visible = true;
            }
            lblPODate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm");
            //cboCompany.SelectedValue = h.ToLocID;
            var cominfo = CompanyService.GetCompanyInfo(h.ToLocID);

            lblCompany.Text = cominfo.Name1+" " + cominfo.Name2;
            txtRemark1.Text = h.Remark1;
            divLine.Visible = ShowHideLine;
            divFGLine.Visible = ShowHideLineFG;
            if (h.FinishFGDate != null) {
                lblFinishFH.Text = "ผลิตสินค้าเสร็จเมื่อ " + Convert.ToDateTime(h.FinishFGDate).ToString("dd/MM/yyyy HH:mm");
            } else {
                lblFinishFH.Text = "";
            }

        }

        private void SetActiveControl() {
            var h = POS_POService.DocSet.head;

            string strstatus = "<span class=" + "\"" + "badge badge-pill badge-success" + "\"" + ">OPEN</span>";
            if (h.Status == "OPEN") {
                litStatus.Text = Server.HtmlDecode(strstatus);
                btnReceive.Visible = h.POID == "" ? false : true;
                btnSave.Visible = true;
                btnDel.Visible = true;
                btnAddLine.Visible = true;
                btnCalStockFGK.Visible = false;

            }
            if (h.Status == "RECEIVED") {
                if (h.FinishFGDate != null) {
                    btnCalStockFGK.Visible = false;
                }
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-dark" + "\"" + ">RECEIVED</span>";
                btnReceive.Visible = false;
                btnSave.Visible = false;
                btnDel.Visible = false;
                btnAddLine.Visible = false;
            }
            if (h.Status == "CANCEL") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-danger" + "\"" + ">CANCEL</span>";
                btnReceive.Visible = false;
                btnSave.Visible = false;
                btnDel.Visible = false;
                btnAddLine.Visible = false;
                btnCalStockFGK.Visible = false;
            }
            //   btnCalStockFGK.Visible = true;//shit fuck
            if (hddshowhiddenfunction.Value=="x") {
                btnSecretSave.Visible = true;
            } else {
                btnSecretSave.Visible = false;
            }
            litStatus.Text = Server.HtmlDecode(strstatus);
        }
        private void InitDisplayDiv() {
            var h = POS_POService.DocSet.head;
            if (h.Status == "OPEN") {
                ShowHideLine = true;
                divLine.Visible = ShowHideLine;
                ShowHideLineFG = false;
                divFGLine.Visible = ShowHideLineFG;
            } else {
                if (h.FinishFGDate != null) {
                    ShowHideLine = false;
                    divLine.Visible = ShowHideLine;
                    ShowHideLineFG = true;
                    divFGLine.Visible = ShowHideLineFG;
                } else {
                    ShowHideLine = true;
                    divLine.Visible = ShowHideLine;
                    ShowHideLineFG = false;
                    divFGLine.Visible = ShowHideLineFG;
                }
            }
        }

        private void LoadDropDownList() {
            string rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            //cboCompany.DataSource = CompanyService.ListCompanyByWH(rcom);
            //cboCompany.DataBind();
            BindLocation();


        }

        private void LoadDevDropDownList() {
            //cboItem.DataSource = ItemInfoServiceV2.ListViewItemByType("RM");
            //cboItem.DataBind();
        }

        private bool PrepairDataSave() {
            var h = POS_POService.DocSet.head;
            bool isnewrecord = h.POID == "" ? true : false;
            if (isnewrecord) {
                h.POID = IDRuunerService.GetNewID("PO", h.ComID, false, "th", h.PODate)[1];
            }
            SetPrimaryData();
            SetLineData();
            RefreshGridPOFGLine();
            return isnewrecord;
        }

        private void SetPrimaryData() {
            var h = POS_POService.DocSet.head;
            //h.ToLocID = cboCompany.SelectedValue;
            h.Remark1 = txtRemark1.Text;
        }

        private bool ValidData() {

            //if (cboCompany.SelectedValue == "") {
            //    ShowPopAlert("Warning", "ระบุ สาขาที่รับ", false, "");
            //    return false;
            //}
            return true;
        }

        protected void btnDel_Click(object sender, EventArgs e) {
            var h = POS_POService.DocSet.head;
            ShowPopDeleteAlert("ลบเอกสาร", "คุณต้องการ ลบเอกสารนี้หรือไม่ ", false, "");

        }

        private void GoByRedirect() {
            string myurl = $"~/POS/POSPODetail?&menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            var h = POS_POService.DocSet.head;
            if (!ValidData()) {
                return;
            }

            var isnew = PrepairDataSave();
            if (isnew) {
                POS_POService.Save("insert");
            } else {
                POS_POService.Save("update");
            }
            if (POS_POService.DocSet.OutputAction.Result == "ok") {//save successufull
                if (isnew) {
                    POS_POService.IsNewDoc = true;
                    POS_POService.GetDocSetByID(h.POID);
                    GoByRedirect();
                } else {

                    ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                    POS_POService.GetDocSetByID(h.POID);
                    LoadData();
                }
            } else {
                ShowPopAlert("Error", POS_POService.DocSet.OutputAction.Message1, false, "");
            }
        }


        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(POS_POService.DetailPreviousPage);
        }

        protected void btnCancel_Click(object sender, EventArgs e) {

        }

        protected void cboItem_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"select ItemID,Name1,TypeID 
                            from (select ItemID,Name1,TypeID , row_number()over(order by ItemID desc) as [rn]  
                            from ItemInfo  as t 
                            where (( ItemID+Name1+TypeID ) like @filter) 
                                    and RCompanyID=@rcom and TypeID != 'DISCOUNT' 
                                    and (ItemID != 'DEFAULTMENU') 
                                    and TypeID in (  'RMK')
                                    and IsActive=1) as st
                           where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboItem_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            if (e.Value == null) {
                return;
            }
            string value = "0";
            value = e.Value.ToString();
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand = @"select ItemID,Name1,TypeID from ItemInfo where   ItemID = @ID and RCompanyID=@rcom order by ItemID";

            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("ID", TypeCode.String, e.Value.ToString());
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        // fgk
        protected void cboItem_POFGLine_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
            ASPxComboBox comboBox = (ASPxComboBox)source;
            sqlSearch.SelectCommand =
                   @"select ItemID,Name1,TypeID,UnitID 
                            from (select ItemID,Name1,TypeID,UnitID , row_number()over(order by ItemID desc) as [rn]  
                            from ItemInfo  as t 
                            where (( ItemID+Name1+TypeID ) like @filter) 
                                    and RCompanyID=@rcom and TypeID != 'DISCOUNT' 
                                    and (ItemID != 'DEFAULTMENU') 
                                    and TypeID in ( 'FGK')
                                    and IsActive=1) as st
                           where st.[rn] between @startIndex and @endIndex";
            sqlSearch.SelectParameters.Clear();
            sqlSearch.SelectParameters.Add("filter", TypeCode.String, string.Format("%{0}%", e.Filter));
            sqlSearch.SelectParameters.Add("rcom", TypeCode.String, LoginService.LoginInfo.CurrentRootCompany.CompanyID);
            sqlSearch.SelectParameters.Add("startIndex", TypeCode.Int64, (e.BeginIndex + 1).ToString());
            sqlSearch.SelectParameters.Add("endIndex", TypeCode.Int64, (e.EndIndex + 1).ToString());
            comboBox.DataSource = sqlSearch;
            comboBox.DataBind();
        }

        protected void cboItem_POFGLine_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
            if (e.Value == null) {
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
        // fgk

        #region Order Line
        protected void btnAddLine_Click(object sender, EventArgs e) {
            if (!ValidLineData()) {
                return;
            }

            SetLineData();
            vw_POS_POLine pol = POS_POService.NewLine();
            pol.ItemID = cboItem.Value.ToString();
            var item = POSItemService.GetItem(pol.ItemID);
            pol.Unit = cboUnitConvert.SelectedValue.ToString();
            pol.ToLocID = cboLocation.SelectedValue.ToString().Replace("X", "");
            pol.Name = item.Name1;
         //   pol.Unit = item.UnitID;
            pol.Price = Convert.ToDecimal(item.Cost);
            pol.VendorID = item.VendorID;

            decimal qty = 0;
            decimal.TryParse(txtQty.Text, out qty);
            pol.Qty = qty;
            var r = POS_POService.NewLineByItem(pol, pol.ItemID);
            if (r.Result == "fail") {
                ShowPopAlert("Error", r.Message1, false, "");
            } else {

                POS_POService.CalRevertBom(pol.LineNum);
                if (POS_POService.DocSet.OutputAction.Result == "fail") {
                    ShowPopAlert("Error", POS_POService.DocSet.OutputAction.Message1, false, ""); 
                }
                ResetControl();
                GridBinding();
                GridPOFGLineBinding();
            }
        }

        private void ResetControl() {
            txtQty.Text = "0";
            cboItem.Value = null;
        }

        private bool ValidLineData() {

            if (cboItem.Value == null) {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }

            if (cboItem.Value.ToString() == "") {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }
            if (cboLocation.SelectedValue == "X") {
                ShowPopAlert("Warning", "ระบุที่เก็บสินค้า", false, "");
                return false;
            }
 if (string.IsNullOrEmpty(cboUnitConvert.SelectedValue)) {
                ShowPopAlert("Warning", "ระบุหน่วยสินค้า", false, "");
                return false;
            }
            return true;
        }

        private void SetLineData() {
            foreach (ListViewDataItem item in grdline.Items) {
                Label lblLinenum = item.FindControl("lblLineNum") as Label;
                TextBox txtQty = item.FindControl("txtQty") as TextBox;
                TextBox txtPrice = item.FindControl("txtPrice") as TextBox;

                if (txtQty == null) {
                    continue;
                }

                if (txtPrice == null) {
                    continue;
                }

                decimal qty = 0;
                var r = decimal.TryParse(txtQty.Text, out qty);

                decimal price = 0;
                decimal.TryParse(txtPrice.Text, out price);

                int linenum = 0;
                int.TryParse(lblLinenum.Text, out linenum);

                var this_line = POS_POService.DocSet.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                this_line.Qty = qty < 0 ? 0 : qty;
                this_line.Price = price < 0 ? 0 : price;
            }
        }

        private void GridBinding() {
            grdline.DataSource = POS_POService.DocSet.line.OrderBy(o => o.LineNum).ToList();
            grdline.DataBind();
        }

        protected void grdlist_DataBinding(object sender, EventArgs e) {
            grdline.DataSource = POS_POService.DocSet.line.OrderBy(o => o.LineNum).ToList();
        }

        private void BindLocation() {
            var h = POS_POService.DocSet.head;
            

            List<string> exclude_loc = new List<string> { "INTRANSIT", "RETURN" };
            var loc= LocationInfoService.ListStockLocation("", h.ComID, true);
            cboLocation.DataSource = loc.Where(o => !exclude_loc.Contains(o.LocID)).ToList();
            cboLocation.DataBind();

            cboLocation_POFGLine.DataSource = LocationInfoService.ListStockLocation("", h.ComID, true);
            cboLocation_POFGLine.DataBind();
        }
        protected void grdline_ItemCommand(object sender, ListViewCommandEventArgs e) {

            if (e.CommandName == "Del") {
                var h = POS_POService.DocSet.head;
                if (h.Status == "RECEIVED") {
                    ShowPopAlert("Error", "ไม่สามารถลบเอกสาร สถานะ RECEIVED ได้", false, "");
                }
                int id = Convert.ToInt32(e.CommandArgument);
                POS_POService.DeletePOLine(id);
                GridBinding();
                GridPOFGLineBinding();
            }
        }

        protected void grdlist_ItemDataBound(object sender, ListViewItemEventArgs e) {
            var h = POS_POService.DocSet.head;
            LinkButton btnDelete = (e.Item.FindControl("btnDelete") as LinkButton);
            TextBox txtQty = (e.Item.FindControl("txtQty") as TextBox);
            Label lblorder = (e.Item.FindControl("lblorder") as Label);

            var dataItem = e.Item.DataItem;
            var row_data = ((vw_POS_POLine)dataItem);

            if (txtQty.Text != row_data.Qty.ToString("N2")) {
                row_data.Qty = Convert.ToDecimal(txtQty.Text);
            }

            if (h.Status == "RECEIVED") {
                btnDelete.Visible = false;
            }

        }

        protected void grdline_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e) {
            (grdline.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }

        #endregion


        #region POFGLine
        private void GridPOFGLineBinding() {
            grdPOFGLine.DataSource = POS_POService.DocSet.FGLine.OrderByDescending(o => o.VendorID).ThenBy(o => o.FgItemID).ToList();
            grdPOFGLine.DataBind();
        }
        protected void grdPOFGLine_DataBinding(object sender, EventArgs e) {
            grdPOFGLine.DataSource = POS_POService.DocSet.FGLine.OrderByDescending(o => o.VendorID).ThenBy(o => o.FgItemID).ToList();
        }

        protected void btnAddPOFGLine_Click(object sender, EventArgs e) {
            if (!ValidLineData_POFGLine()) {
                return;
            }

            var pol = POS_POService.NewFGLine();
            pol.FgItemID = cboitem_POFGLine.Value.ToString();
            var item = POSItemService.GetItem(pol.FgItemID);
            pol.ToLocID = cboLocation_POFGLine.SelectedValue.ToString().Replace("X", "");
            pol.FgName = item.Name1;
            pol.FgUnit = item.UnitID;
            pol.Price = Convert.ToDecimal(item.Cost);
            pol.VendorID = item.VendorID;

            decimal qty = 0;
            decimal.TryParse(txtqty_POFGLine.Text, out qty);
            pol.FgQty = qty;
            var r = POS_POService.NewPoFGLineByItem(pol, pol.FgItemID);

            if (r.Result == "fail") {
                ShowPopAlert("Error", r.Message1, false, "");
            } else {
                ResetControlPOFGLine();
                GridPOFGLineBinding();
            }
        }

        private void ResetControlPOFGLine() {
            txtqty_POFGLine.Text = "0";
            cboitem_POFGLine.Value = null;
        }

        private bool ValidLineData_POFGLine() {
            if (cboitem_POFGLine.Value == null) {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }

            if (cboitem_POFGLine.Value.ToString() == "") {
                ShowPopAlert("Warning", "ระบุ สินค้า", false, "");
                return false;
            }
            if (cboLocation_POFGLine.SelectedValue == "X") {
                ShowPopAlert("Warning", "ระบุที่เก็บสินค้า", false, "");
                return false;
            }

            return true;
        }

        private void RefreshGridPOFGLine() {
            for (int i = 0; i < grdPOFGLine.VisibleRowCount; i++) {
                ASPxTextBox txtFgQty = ((ASPxTextBox)grdPOFGLine.FindRowCellTemplateControl(i, grdPOFGLine.Columns["FgQty"] as GridViewDataColumn, "txtFgQty"));
                if (txtFgQty == null) {
                    continue;
                }
                int linenum = Convert.ToInt32(grdPOFGLine.GetRowValues(i, grdPOFGLine.KeyFieldName));

                var pofgline = POS_POService.DocSet.FGLine.Where(o => o.LineNum == linenum).FirstOrDefault();
                if (pofgline == null) {
                    continue;
                }
                decimal fgqty = pofgline.FgQty;
                decimal.TryParse(txtFgQty.Text, out fgqty);

                pofgline.FgQty = fgqty;
            }
        }


        protected void grdPOFGLine_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            var h = POS_POService.DocSet.head;
            if (e.CommandArgs.CommandName == "Del") {
                int linenum = Convert.ToInt32(e.KeyValue);
                var rs = POS_POService.DeletePOFGLine(linenum);
                if (rs.Result == "ok") {
                    POS_POService.GetDocSetByID(h.POID);
                    GridPOFGLineBinding();

                } else {
                    ShowPopAlert("Error", rs.Message1, false, "");
                }

            }
        }

        #endregion

        protected void btnNew_Click(object sender, EventArgs e) {
            POS_POService.NewTransaction("");
            string myurl = $"~/POS/POSPODetail?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

        protected void btnReceive_Click(object sender, EventArgs e) {
            var h = POS_POService.DocSet.head;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var r = POS_POService.SetStatusReceive(h.POID, rcom);
            if (r.Result == "ok") {
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                POS_POService.GetDocSetByID(h.POID);
                LoadData();
            } else {
                ShowPopAlert("Error", r.Message1, false, "");
            }
        }
        protected void btnCalStockFGK_Click(object sender, EventArgs e) {
            var h = POS_POService.DocSet.head;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            POS_POService.Save("update");
            if (POS_POService.DocSet.OutputAction.Result == "faiil") {//save successufull
                ShowPopAlert("Error", POS_POService.DocSet.OutputAction.Message1, false, "");
                return;
            }
            var r = POS_POService.SetStatusFinishFG(h.POID, rcom);
            if (r.Result == "ok") {
                ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                POS_POService.GetDocSetByID(h.POID);
                LoadData();
            } else {
                ShowPopAlert("Error", r.Message1, false, "");
            }
        }
        protected void btnConfirmDel_Click(object sender, EventArgs e) {
            var h = POS_POService.DocSet.head;
            POS_POService.DeleteDoc(h.POID);
            if (POS_POService.DocSet.OutputAction.Result == "ok") {
                Response.Redirect($"~/POS/POSPOList?&menu={hddmenu.Value}");
            } else {
                ShowPopAlert("Error", "Delete failed " + Environment.NewLine + POS_POService.DocSet.OutputAction.Message1, false, "");
                return;
            }
        }

        private void ExportPrintPO()
        {
            var h = POS_POService.DocSet.head;
            var stream = new MemoryStream();
            var report = new PrintPO();
            report.initData(h.POID);
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();

            #region ใช้ Code popup ไปก่อน
            string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
            Response.Redirect(xurl);
            #endregion

        }

        protected void btnPrintPO_Click(object sender, EventArgs e)
        {
            var h = POS_POService.DocSet.head;
            if (h.POID == "")
            {
                return;
            }
            ExportPrintPO();
        }

        protected void btnCollapseLine_Click(object sender, EventArgs e) {
            ShowHideLine = !ShowHideLine;
            divLine.Visible = ShowHideLine;
        }

        protected void btnCollapseFGLine_Click(object sender, EventArgs e) {

            ShowHideLineFG = !ShowHideLineFG;
            divFGLine.Visible = ShowHideLineFG;
        }

        protected void grdPOFGLine_DataBound(object sender, EventArgs e) {
            if (POS_POService.DocSet.head==null) {
                return;
            }
            var h = POS_POService.DocSet.head;
            if (h.FinishFGDate != null) {
                grdPOFGLine.Columns["colDel"].Visible = false;
            }

        }

        protected void cboItem_SelectedIndexChanged(object sender, EventArgs e) {
            string itemid = cboItem.Value == null ? "" : cboItem.Value.ToString();
            var data = POSItemService.GetItem(itemid);
            if (data != null) {
                cboUnitConvert.DataSource = POSItemService.ListViewItemUnitConvert(data.ItemID);
                cboUnitConvert.DataBind();
            }
        }
    }
}