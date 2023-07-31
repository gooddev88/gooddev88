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
    public partial class POSORDERDetail : MyBasePage {
        protected void Page_Load(object sender, EventArgs e) {
            popPrint.ShowOnPageLoad = false;
            SetQueryString();
            RefreshGridLine();
            LoadDevDropDownList();
            if (POSOrderService.IsNewDoc) {
                ShowPopAlert("Success", "บันทึกสำเร็จ ", true, "");
            }

            if (!Page.IsPostBack) {
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
        }

        private void CheckPermission() {
            var h = POSOrderService.DocSet.head;


            //switch (hddmenu.Value)
            //{
            //    case "2001"://เบิกสินค้า
            //        btnSave.Text = "บันทึกออเดอร์";
            //        divRM.Visible = false;
            //        btnPrintOrder.Visible = true;
            //        break;
            //    case "2002"://จัดเตรียมสินค้า
            //        //btnSave.Text = "จัดเตรียมสินค้า";
            //        btnSave.Text = "บันทึกจัดเตรียม";
            //        divAddItem.Visible = false;
            //        btnDel.Visible = false;
            //        break;
            //    case "2003"://ส่งสินค้า
            //        btnSave.Text = "บันทึกจัดส่ง";
            //        btnPrintShipOrder.Visible = true;
            //        break;
            //    case "2004"://รับสินค้า
            //        divAddItem.Visible = false;
            //        btnSave.Text = "รับสินค้า";
            //        break;
            //}
            if (h.Status == "RECEIVED") {
                btnSave.Visible = false;
                if (PermissionService.CanEdit("2004")) {//can edit
                    btnDel.Visible = true;
                }
            }

            if (!PermissionService.CanOpen("9901")) {//delete ord
                btnDel.Visible = false;
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
            POSOrderService.IsNewDoc = false;
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
            POSStkAdjustService.IsNewDoc = false;
            popDeleteAlert.ShowOnPageLoad = true;
        }

        private void LoadData() {
            CheckPermission();
            BindData();
            SetActiveControl();
            GridBinding();
            GridBomLineBinding();
        }

        private void BindData() {

            lblDocinfo.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;

            var h = POSOrderService.DocSet.head;
            if (h.OrdID == "") {
                lblOrderID.Text = "++NEW++";
                btnDel.Visible = false;
                btnPrintOrder.Visible = false;
            } else {
                lblOrderID.Text = h.OrdID;
            }

            lblOrderDate.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm");
            var com = CompanyService.GetCompanyInfo(h.ComID);
            lblCompany.Text = "สาขา " + com.Name1 + " " + com.Name2 + " (" + com.CompanyID + ")";
            cboCompany.SelectedValue = h.FrLocID;
            txtRemark1.Text = h.Remark1;

        }


        private void SetActiveControl() {
            var h = POSOrderService.DocSet.head;

            switch (hddmenu.Value) {
                case "2001"://เบิกสินค้า
                    btnSave.Text = "บันทึกออเดอร์";
                    divRM.Visible = false;
                    btnPrintOrder.Visible = true;
                    break;
                case "2002"://จัดเตรียมสินค้า
                    //btnSave.Text = "จัดเตรียมสินค้า";
                    btnSave.Text = "บันทึกจัดเตรียม";
                    divAddItem.Visible = false;
                    btnDel.Visible = false;
                    break;
                case "2003"://ส่งสินค้า
                    btnSave.Text = "บันทึกจัดส่ง";
                    btnPrintShipOrder.Visible = true;
                    break;
                case "2004"://รับสินค้า
                    divAddItem.Visible = false;
                    btnSave.Text = "รับสินค้า";
                    break;
            }



            string strstatus = "<span class=" + "\"" + "badge badge-pill badge-success" + "\"" + ">OPEN</span>";

            if (h.Status == "ACCEPTED") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-info" + "\"" + ">ACCEPTED</span>";
            }
            if (h.Status == "SHIPPING") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-warning" + "\"" + ">SHIPPING</span>";
            }
            if (h.Status == "RECEIVED") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-dark" + "\"" + ">RECEIVED</span>";
                btnSave.Visible = false;
            }
            if (h.Status == "CANCEL") {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-danger" + "\"" + ">CANCEL</span>";
            }
            litStatus.Text = Server.HtmlDecode(strstatus);
            List<string> statusOpenEditBom = new List<string> { "ACCEPTED", "SHIPPING" };
            if (statusOpenEditBom.Contains(h.Status) && hddmenu.Value == "2003") {
                grdBomLine.Columns["RmQty"].Visible = true;
                grdBomLine.Columns["OrdID"].Visible = false;

            } else {
                grdBomLine.Columns["RmQty"].Visible = false;
                grdBomLine.Columns["OrdID"].Visible = true;
            }
        }

        private void LoadDropDownList() {
            string rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            cboCompany.DataSource = CompanyService.ListCompanyByWH(rcom);
            cboCompany.DataBind();

        }

        private void LoadDevDropDownList() {
            //cboItem.DataSource = ItemInfoServiceV2.ListViewItemByType("FGK").Where(o => o.TypeID != "DISCOUNT").ToList();
            //cboItem.DataBind();
        }

        private bool PrepairDataSave() {
            var h = POSOrderService.DocSet.head;
            bool isnewrecord = h.OrdID == "" ? true : false;
            if (isnewrecord) {
                h.OrdID = IDRuunerService.GetNewID("ORD", h.ComID, false, "th", h.OrdDate)[1];
            }
            h.FrLocID = cboCompany.SelectedValue;
            h.Remark1 = txtRemark1.Text;
            SetLineData();
            RefreshGridLine();
            return isnewrecord;
        }



        private bool ValidData() {
            if (cboCompany.SelectedValue == "") {
                ShowPopAlert("Warning", "ระบุ สาขาที่ส่ง", false, "");
                return false;
            }
            return true;
        }

        protected void btnDel_Click(object sender, EventArgs e) {
            var h = POSOrderService.DocSet.head;
            ShowPopDeleteAlert("ลบเอกสาร", "คุณต้องการ ลบเอกสารนี้หรือไม่ ", false, "");
        }

        private void GoByRedirect() {
            string myurl = $"~/POS/POSORDERDetail?&menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnSave_Click(object sender, EventArgs e) {

            if (!ValidData()) {
                return;
            }
            Save();

        }
        private void Save() {

            var isnew = PrepairDataSave();
            var h = POSOrderService.DocSet.head;
            if (isnew) {
                POSOrderService.Save("insert");
            } else {
                POSOrderService.Save("update");
            }

            if (POSOrderService.DocSet.OutputAction.Result == "ok") {//save successufull
                if (isnew) {
                    POSOrderService.IsNewDoc = true;
                    POSOrderService.GetDocSetByID(h.OrdID);
                    GoByRedirect();
                } else {
                    switch (hddmenu.Value) {
                        case "2001"://เบิกสินค้า
                            btnSave.Text = "บันทึกออเดอร์";
                            break;
                        case "2002"://จัดเตรียมสินค้า 
                            POSOrderService.SetStatusAccepted(h.OrdID);
                            break;
                        case "2003"://ส่งสินค้า
                            POSOrderService.SetStatusShipping(h.OrdID);
                            break;
                        case "2004"://รับสินค้า
                            POSOrderService.SetStatusReceive(h.OrdID);
                            break;
                    }
                    ShowPopAlert("Success", "บันทึกสำเร็จ", true, "");
                    POSOrderService.GetDocSetByID(h.OrdID);
                    LoadData();
                }
            } else {
                ShowPopAlert("Error", POSOrderService.DocSet.OutputAction.Message1, false, "");
            }
        }

        private void RefreshGridLine() {
            for (int i = 0; i < grdBomLine.VisibleRowCount; i++) {
                ASPxTextBox txtrmqty = ((ASPxTextBox)grdBomLine.FindRowCellTemplateControl(i, grdBomLine.Columns["RmQty"] as GridViewDataColumn, "txtRmQty"));
                if (txtrmqty == null) {
                    continue;
                }
                int linenum = Convert.ToInt32(grdBomLine.GetRowValues(i, grdBomLine.KeyFieldName));

                var ordbomline = POSOrderService.DocSet.lineBom.Where(o => o.LineNum == linenum).FirstOrDefault();
                if (ordbomline == null) {
                    continue;
                }
                decimal rmqty = ordbomline.RmQty;
                decimal.TryParse(txtrmqty.Text, out rmqty);

                ordbomline.RmQty = rmqty;
            }
        }


        protected void cboItem_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {
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

        protected void cboItem_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {
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



        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(POSOrderService.DetailPreviousPage);
        }

        protected void btnCancel_Click(object sender, EventArgs e) {

        }

        #region Order Line
        protected void btnAddLine_Click(object sender, EventArgs e) {
            if (!ValidLineData()) {
                return;
            }
            var h = POSOrderService.DocSet.head;
            vw_POS_ORDERLine my_line = POSOrderService.NewLine();

            my_line.ItemID = cboItem.Value.ToString();
            my_line.Unit = cboUnitConvert.SelectedValue.ToString();

            var item = POSItemService.GetItem(my_line.ItemID);
            var stk = POSStockService.GetBalanceByUnit(my_line.ItemID, h.ComID ,my_line.Unit);

            my_line.Name = item.Name1;

            my_line.Unit = item.UnitID;
            my_line.GrUnit = item.UnitID;

            if (cboUnitConvert.SelectedValue != "") {
                my_line.Unit = cboUnitConvert.SelectedValue;
                my_line.GrUnit = cboUnitConvert.SelectedValue;
            }

            my_line.Price = item.Price;
            my_line.BalQty = stk.QtyBalInToUnit;
            my_line.BalQtyOrd = stk.QtyBalInToUnit;
            my_line.OnOrdQty = Convert.ToDecimal(stk.QtyOnOrdInToUnit);
            decimal qty = 0;
            decimal.TryParse(txtQty.Text, out qty);

            switch (hddmenu.Value) {
                case "2003"://ส่งสินค้า
                    my_line.OrdQty = 0;
                    my_line.ShipQty = qty;
                    break;

                default:
                    my_line.OrdQty = qty;
                    break;
            }




            var r = POSOrderService.NewLineByOrdLine(my_line, my_line.ItemID);

            if (r.Result == "fail") {
                ShowPopAlert("Error", r.Message1, false, "");
            } else {
                ResetControl();
                GridBinding();
                GridBomLineBinding();
            }
        }

        private void ResetControl() {
            txtQty.Text = "";
            cboItem.Value = "";
            cboItem.Items.Clear();
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

            return true;
        }

        private void SetLineData() {
            var h = POSOrderService.DocSet.head;
            foreach (ListViewDataItem item in grdline.Items) {
                Label lblLinenum = item.FindControl("lblLineNum") as Label;
                TextBox txtQty = item.FindControl("txtQty") as TextBox;
                TextBox txtShipQty = item.FindControl("txtShipQty") as TextBox;
                TextBox txtGrQty = item.FindControl("txtGrQty") as TextBox;
                TextBox txtBalQty = item.FindControl("txtBalQty") as TextBox;
                DropDownList ddlUnitID = item.FindControl("ddlUnitID") as DropDownList;

                if (ddlUnitID.SelectedValue == null) {
                    ddlUnitID.SelectedValue = "";
                }

                if (txtQty == null) {
                    continue;
                }

                if (txtBalQty == null) {
                    continue;
                }

                int linenum = 0;
                int.TryParse(lblLinenum.Text, out linenum);
                decimal balqty = 0;
                decimal qty = 0;



                vw_POS_ORDERLine this_line = new vw_POS_ORDERLine();
                switch (hddmenu.Value) {
                    case "2001"://เบิกสินค้า
                        decimal.TryParse(txtQty.Text, out qty);
                        decimal.TryParse(txtBalQty.Text, out balqty);
                        this_line = POSOrderService.DocSet.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                        this_line.OrdQty = qty < 0 ? 0 : qty;
                        this_line.BalQtyOrd = balqty == 0 ? 0 : balqty;
                        this_line.GrUnit = ddlUnitID.SelectedValue;
                        break;
                    case "2002"://จัดเตรียมสินค้า 
                        decimal.TryParse(txtShipQty.Text, out qty);
                        this_line = POSOrderService.DocSet.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                        this_line.ShipQty = qty < 0 ? 0 : qty;
                        this_line.GrUnit = ddlUnitID.SelectedValue;
                        break;
                    case "2003"://ส่งสินค้า
                        decimal.TryParse(txtShipQty.Text, out qty);
                        this_line = POSOrderService.DocSet.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                        this_line.ShipQty = qty < 0 ? 0 : qty;
                        this_line.GrUnit = ddlUnitID.SelectedValue;
                        break;
                    case "2004"://รับสินค้า
                        decimal.TryParse(txtGrQty.Text, out qty);
                        this_line = POSOrderService.DocSet.line.Where(o => o.LineNum == linenum).FirstOrDefault();
                        this_line.GrQty = qty < 0 ? 0 : qty;
                        break;
                }
            }
        }

        private void GridBinding() {
            var h = POSOrderService.DocSet.head;

            if (new List<string> { "OPEN", "ACCEPTED" }.Contains(h.Status) && hddmenu.Value == "2003") {
                //กำจะบันทึกจัดส่งให้ Default Qty ให้ก่อน
                foreach (var l in POSOrderService.DocSet.line) {
                    l.ShipQty = l.OrdQty;
                    l.ShipdAmt = l.OrdAmt;
                }
            }

            if (new List<string> { "SHIPPING" }.Contains(h.Status) && hddmenu.Value == "2004") {
                //กำจะบันทึกรับให้ Default QtyShip จะ GRQty ให้ก่อน
                foreach (var l in POSOrderService.DocSet.line) {
                    l.GrQty = l.ShipQty;
                    l.GrAmt = l.ShipdAmt;
                }
            }
            grdline.DataSource = POSOrderService.DocSet.line;
            grdline.DataBind();
        }
        private void GridBomLineBinding() {
            grdBomLine.DataSource = POSOrderService.DocSet.lineBom.OrderByDescending(o => o.VendorID).ThenBy(o => o.FgItemID).ToList();
            grdBomLine.DataBind();
        }
        protected void grdBomLine_DataBinding(object sender, EventArgs e) {
            grdBomLine.DataSource = POSOrderService.DocSet.lineBom.OrderByDescending(o => o.VendorID).ThenBy(o => o.FgItemID).ToList();
        }
        protected void grdline_ItemCommand(object sender, ListViewCommandEventArgs e) {
            if (e.CommandName == "Del") {
                int id = Convert.ToInt32(e.CommandArgument);
                POSOrderService.DeleteBOMLine(id);
                //SORequestService.DeleteLine(id);
                //SORequestService.ClearPendingLine("");
                GridBinding();
            }
        }

        protected void grdlist_ItemDataBound(object sender, ListViewItemEventArgs e) {
            var h = POSOrderService.DocSet.head;
            LinkButton btnDelete = (e.Item.FindControl("btnDelete") as LinkButton);
            TextBox txtQty = (e.Item.FindControl("txtQty") as TextBox);
            TextBox txtShipQty = (e.Item.FindControl("txtShipQty") as TextBox);
            TextBox txtGrQty = (e.Item.FindControl("txtGrQty") as TextBox);
            Label lblorder = (e.Item.FindControl("lblorder") as Label);
            Label lblShip = (e.Item.FindControl("lblShip") as Label);
            Label lblGrQty = (e.Item.FindControl("lblGrQty") as Label);
            TextBox txtBalQty = (e.Item.FindControl("txtBalQty") as TextBox);
            var dataItem = e.Item.DataItem;

            if (e.Item.ItemType == ListViewItemType.DataItem) {

                var unitlist = MasterTypeService.ListType("UNIT", false);
                DropDownList ddlUnitID = (DropDownList)e.Item.FindControl("ddlUnitID");
                ddlUnitID.DataSource = unitlist;
                ddlUnitID.DataBind();

                ListViewDataItem item = (ListViewDataItem)e.Item;
                string GrUnit = (string)DataBinder.Eval(item.DataItem, "GrUnit");
                string Unit = (string)DataBinder.Eval(item.DataItem, "Unit");

                if (string.IsNullOrEmpty(GrUnit)) {
                    ddlUnitID.SelectedValue = Unit;
                } else {
                    ddlUnitID.SelectedValue = GrUnit == null ? "" : GrUnit;
                }
            }

            var row_data = ((vw_POS_ORDERLine)dataItem);

            switch (hddmenu.Value) {
                case "2001"://เบิกสินค้า
                    btnDelete.Visible = true;
                    txtQty.Visible = true;
                    txtBalQty.Enabled = true;
                    txtShipQty.Visible = false;
                    txtGrQty.Visible = false;
                    lblorder.Visible = true;
                    lblShip.Visible = false;
                    lblGrQty.Visible = false;
                    break;
                case "2002"://จัดเตรียมสินค้า
                    btnDelete.Visible = false;
                    txtQty.Visible = false;
                    txtBalQty.Enabled = false;
                    txtShipQty.Visible = false;
                    txtGrQty.Visible = false;
                    lblorder.Visible = false;
                    lblShip.Visible = false;
                    lblGrQty.Visible = false;
                    break;
                case "2003"://ส่งสินค้า
                    btnDelete.Visible = true;
                    txtQty.Visible = false;
                    txtBalQty.Enabled = false;
                    txtShipQty.Visible = true;
                    txtGrQty.Visible = false;
                    lblorder.Visible = false;
                    lblShip.Visible = true;
                    lblGrQty.Visible = false;
                    break;
                case "2004"://รับสินค้า
                    btnDelete.Visible = true;
                    txtQty.Visible = false;
                    txtBalQty.Enabled = false;
                    txtShipQty.Visible = false;
                    txtGrQty.Visible = true;
                    txtGrQty.Enabled = false;
                    lblorder.Visible = false;
                    lblShip.Visible = false;
                    lblGrQty.Visible = true;
                    break;

            }
            if (!h.IsActive) {
                btnDelete.Visible = false;
                txtQty.Visible = false;
                txtShipQty.Visible = false;
                txtGrQty.Visible = false;
                lblorder.Visible = false;
                lblShip.Visible = false;
                lblGrQty.Visible = false;
            }

        }

        protected void grdline_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e) {
            (grdline.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }

        #endregion

        protected void btnNew_Click(object sender, EventArgs e) {
            POSOrderService.NewTransaction("");
            string myurl = $"~/POS/POSORDERDetail?menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }

        protected void PrintShipOrder_Click(object sender, EventArgs e) {

            var stream = new MemoryStream();
            var report = new PrintShipOrder();
            //report.DisplayName = POSOrderService.DocSet.head.OrdID;
            report.initData(POSOrderService.DocSet.head.OrdID);
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


            //Print.MyPrint.NewReportFItler();
            //var f = Print.MyPrint.ReportFilterX;
            //f.DocID = POSOrderService.DocSet.head.OrdID;

            //Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string myurl = $"../POS/Print/MyPrint?report=PrintShipOrder";
            //Response.RedirectPermanent(myurl);
        }



        //protected void btnAccepted_Click(object sender, EventArgs e) {
        //    var h = POSOrderService.DocSet.head;
        //    Save();
        //    POSOrderService.SetStatusAccepted(h.OrdID);
        //    string myurl = $"/POS/POSORDERList?menu=2001";
        //    Response.RedirectPermanent(myurl);
        //}

        private void ExportPrintOrder() {
            var h = POSOrderService.DocSet.head;
            var stream = new MemoryStream();
            var report = new PrintOrder();
            //report.DisplayName = h.OrdID;
            report.initData(h.OrdID);
            report.ExportToPdf(stream);

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();


            #region  โค้ดเช็ค Mobile มีปัญหา
            //if (MobileHelper.isMobileBrowser())
            //         {
            //             string baseurl = POSHelper.GetBaseUrl();
            //             string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
            //             yurl = yurl.Replace("http://", "https://");
            //             LogJService.SaveLogJ("ismobile:" + yurl);
            //             string func = "sendUrlToPrint('" + yurl + "');";
            //             Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
            //             return;
            //         }
            //         else
            //         {
            //             string xurl = String.Format("../TempFile/Print/{0}", myfilename);
            //             LogJService.SaveLogJ("iscomputer:" + xurl);
            //             popPrint.ContentUrl = xurl;
            //             popPrint.ShowOnPageLoad = true;
            //         }
            #endregion
            #region ใช้ Code popup ไปก่อน
            string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
            //LogJService.SaveLogJ("iscomputer:" + xurl);
            //popPrint.ContentUrl = xurl;
            //popPrint.ShowOnPageLoad = true;
            Response.Redirect(xurl);
            #endregion

        }

        protected void btnPrintOrder_Click(object sender, EventArgs e) {
            var h = POSOrderService.DocSet.head;
            if (h.OrdID == "") {
                return;
            }
            ExportPrintOrder();
            //Print.MyPrint.PreviousPrintPage = HttpContext.Current.Request.Url.PathAndQuery;
            //string myurl = $"../POS/Print/MyPrint?report=PrintStoreOrder";
            //Response.RedirectPermanent(myurl);
        }

        protected void btnConfirmDel_Click(object sender, EventArgs e) {
            var h = POSOrderService.DocSet.head;
            POSOrderService.DeleteDoc(h.OrdID);
            if (POSOrderService.DocSet.OutputAction.Result == "ok") {
                Response.Redirect($"~/POS/POSORDERList?&menu={hddmenu.Value}");
            } else {
                ShowPopAlert("Error", "Delete failed " + Environment.NewLine + POSOrderService.DocSet.OutputAction.Message1, false, "");
                return;
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