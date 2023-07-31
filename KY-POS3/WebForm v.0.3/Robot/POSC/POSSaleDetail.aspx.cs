using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.ServiceHelper;
using Robot.Helper;
using Robot.Master.DA;
using Robot.POSC.DA;
using Robot.POSC.POSPrint;


namespace Robot.POSC {
    public partial class POSSaleDetail : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["posc_previouspage"] == null ? "" : (string)HttpContext.Current.Session["posc_previouspage"]; } set { HttpContext.Current.Session["posc_previouspage"] = value; } }
        //public static CompanyInfo ComInfo { get { return (CompanyInfo)HttpContext.Current.Session["posc_cominfo"]; } set { HttpContext.Current.Session["posc_cominfo"] = value; } }
        public static bool IsSaveSuccess { get { return HttpContext.Current.Session["posc_issavesuccess"] == null ? false : (bool)HttpContext.Current.Session["posc_issavesuccess"]; } set { HttpContext.Current.Session["posc_issavesuccess"] = value; } }

        protected void Page_Load(object sender, EventArgs e) { 
            SetQueryString();
            LoadDevDropDownList();
            if (IsSaveSuccess) {
                ShowPopAlert1("Success", "บันทึกสำเร็จ", true);
            }
            popPrint.ShowOnPageLoad = false;
            popAlert.ShowOnPageLoad = false;
            if (!Page.IsPostBack) {
                CloseAlert();
                LoadDropDownList();
                LoadData();
            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"]; 
        }
        private void CheckPermission() {
            var h = POSSaleService.DocSet.Head;  
            if (PermissionService.CanEdit("F5991")) {//edit bill
                                                     //if (h.BillID != "") { 
                                                     //        //if (h.INVID != "") {
                                                     //        //    btnsave.Visible = false;
                                                     //        //    dtInvDate.Enabled = false;
                                                     //        //} 
                                                     //}
                if (!h.IsActive) {
                    btnsave.Visible = true;
                }
               
                dtInvDate.Enabled = true; 
            }
            else
            {
              
            }
            if (!PermissionService.CanOpen("F5992")) {//cancel bill
                btnDelete.Visible = false;
            }
            //if (!PermissionService.CanEdit("F5993")) {//delete bill
            //    btnDeletelPermanent.Visible = false;
            //}
        }


        //private void ShowPopAlert(string msg_header, string msg_body, bool result, string showbutton) {
        //    if (result) {
        //        lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
        //    } else {
        //        lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
        //    }
        //    if (showbutton == "") {
        //        btnCancel.Visible = false;
        //    }
        //    if (showbutton == "okcancel") {
        //    }
        //    lblHeaderMsg.Text = msg_header;
        //    lblBodyMsg.Text = msg_body;
        //    IsSaveSuccess = false;
        //    popAlert.ShowOnPageLoad = true;
        //}

        private void ShowPopAlert1(string msg_header, string msg_body, bool result) {
            if (result) {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Green;
                btnCancel.Visible = false;
            } else {
                lblHeaderMsg.ForeColor = System.Drawing.Color.Red;
                btnCancel.Visible = false;
            }
            lblHeaderMsg.Text = msg_header;
            lblBodyMsg.Text = msg_body;
            popAlert.ShowOnPageLoad = true;
        }

        private void SetDivVisible(string type) {

            switch (type) {
                case "main":
                    txtPrice.Text = "";
                    txtqty.Text = "";
                    divmain.Visible = true;
                    divpay.Visible = false;
                    divitem_select.Visible = true;
                    divitem_price.Visible = false;
                    break;
                case "pay":
                    divmain.Visible = false;
                    divpay.Visible = true;
                    divitem_select.Visible = false;

                    divitem_price.Visible = false;
                    txtCustPayAmt.Text = "";
                    txtsum_price.Disabled = true;
                    txtCustPayAmt.Focus();
                    break;
                case "select":
                    divmain.Visible = false;
                    divpay.Visible = false;
                    divitem_select.Visible = true;
                    divitem_price.Visible = false;
                    break;
                case "AddItem":
                    divmain.Visible = false;
                    divpay.Visible = false;
                    divitem_select.Visible = true;
                    divitem_price.Visible = true;
                    break;
            }
            BindData();
        }

        private void LoadData() {
          
            BindData();
            SetDivVisible("main");
            BindGrdItemLine();
            BindGrdPayment();
            BindGrdItemLine();
            BindGrdItemSelect(); 
            SetActiveControl();


        }
        private void BindData() {
            var h = POSSaleService.DocSet.Head;
            var comInfo = LoginService.LoginInfo.CurrentCompany;
       
            cboCustomerID.Value = h.CustomerID;
            dtInvDate.Value = h.BillDate;
            if (h.BillID == "") {
                lblID.Text = "++NEW++";
            } else {
                if (h.INVID != "") {
                    lblID.Text = h.INVID;
                } else {
                    lblID.Text = h.BillID;
                }

                if (!string.IsNullOrEmpty(h.FINVID)) {
                    lblID.Text = h.INVID + " (" + h.FINVID + ")";
                }
            }

            btnShipTo.Text = string.IsNullOrEmpty(h.ShipToLocID) ? "หน้าร้าน" : h.ShipToLocID;
            try { cboTable.SelectedValue = h.TableID; } catch {  }
           
            lblcompany.Text = comInfo.Name1+" "+ comInfo.Name2 + " - " + h.ComID  ;
            dtInvDate.Value = h.BillDate;
            lblSumTotalAmtIncVat.Text = h.NetTotalAmtIncVat.ToString("n2");
            lblSumVatAmt.Text = h.NetTotalVatAmt.ToString("n2");
            lblSumNetTotalAfterRound.Text = h.NetTotalAfterRound.ToString("n2");
            txtsum_price.Value = h.NetTotalAfterRound.ToString("n2");

        }

        private void SetActiveControl() {

                var h = POSSaleService.DocSet.Head;
            if (h.BillID == "") { 
            
                divprint.Visible = false;
                btnsave.Visible = true;
                btnSaveInvoice.Visible = true;
                btnDelete.Visible = false;
              
            } else {
                divprint.Visible = true;
                btnDelete.Visible = true; 
                if (h.INVID != "") {//ยังไม่ เก็บเงิน
                    btnSaveInvoice.Visible = false;
                    btnsave.Visible = false;
                }
            }
            if (new List<string> { "GRAB", "PANDA", "LINEMAN", "GOJEK" }.Contains(h.ShipToLocID)) {//ขายให้ grap และอื่นๆให้ Add Payment เลย
                btnpay.Visible = false;
                btnsave.Visible = false;
            }
            if (!h.IsActive) {
                btnsave.Visible = false;
                btnSaveInvoice.Visible=false;
                btnCancel.Visible = false;
            }
            dtInvDate.Enabled = false;
            CheckPermission();
        }
         
        private void LoadDropDownList() {
            cboTable.DataSource = POSTableService.ListTable();
            cboTable.DataBind();
        }

        private void LoadDevDropDownList() {

        }

        private bool ValidData() {
            bool r = true;
            var line = POSSaleService.DocSet.Line;
            if (line.Count == 0) {
                r = false;
                ShowAlert("ไม่พบรายการสินค้า", "Error");
                ShowPopAlert1("Error!", "ไม่พบรายการสินค้า", false);
            }
            return r;
        }

        private void BindGrdItemSelect() {
            grdlist.DataSource = POSSaleService.Menu.Where(o => o.TypeID != "DISCOUNT" && o.IsActive == true).OrderBy(o => o.ItemID);
            grdlist.DataBind();
        }

        private void BindGrdItemLine() {
            grditemSelect.DataSource = POSSaleService.DocSet.Line;
            grditemSelect.DataBind();
        }

        private void BindGrdPayment() {
            grdpay.DataSource = POSSaleService.DocSet.Payment;
            grdpay.DataBind();
        }

        private void ShowAlert(string msg, string type) {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        private bool PrepairDataSave() {
            var h = POSSaleService.DocSet.Head;
            h.IsLink = true;
            h.LinkBy = "OnWeb";
            h.LinkDate = DateTime.Now;
            bool isNew = h.BillID == "" ? true : false;
            if (isNew) { // new 
                h.BillID = IDRuunerService.GenPOSSaleID( "ORDER",h.ComID, h.MacNo,h.ShipToLocID, false, h.BillDate)[1];
            } 
            h.TableID = cboTable.SelectedValue; 
            h.TableName = cboTable.SelectedItem.Text;
            return isNew;
        }

        protected void btnSave_Click(object sender, EventArgs e) {
         
            if (!ValidData()) {
                return;
            }
            SaveBill(true);
        }
        private void SaveBill(bool isRefreshForNewDoc) {
            var h = POSSaleService.DocSet.Head;
            bool isNew = PrepairDataSave();
            try {
                var r = POSSaleService.Save(POSSaleService.DocSet, isNew);
                if (r.Result == "fail") {
                    //ShowAlert(r.Message1, "Error");
                    ShowPopAlert1("Error!", r.Message1, false);
                    return;
                } else {
                    if (isNew) {
                        //NewDoc();
                        POSSaleService.DocSet = POSSaleService.GetDocSet(h.BillID);
                        if (isRefreshForNewDoc) {
                            IsSaveSuccess = true;
                            Response.Redirect(Request.RawUrl);
                        }                     
                    } else {
                        LoadData();
                        ShowPopAlert1("Success", "บันทึกสำเร็จ", true);
                    }
                }

            } catch (Exception ex) {
                var err = "Save Error becuase : " + ex.Message.ToString();
                //ShowAlert(err, "Error");
                ShowPopAlert1("Error!", err, false);
            }
        }
      private void SaveInvoice() {
            var h = POSSaleService.DocSet.Head;
            if (new List<string> { "GRAB", "PANDA", "LINEMAN", "GOJEK" }.Contains(h.ShipToLocID)) {//ขายให้ grap และอื่นๆให้ Add Payment เลย
                AddOnlinePayment();
            }
            if (h.BillID == "") {//ถ้ายังไม่ได้บันทึก Bill ให้บันทึกบิลก่อน
                SaveBill(false);
            }
            POSSaleService.DocSet = POSSaleService.CalDocSet(POSSaleService.DocSet);
            var diff = h.NetTotalAfterRound - h.PayTotalAmt;

            if (diff > 0) {
               // ShowAlert("ชำระราคาสินค้าไม่ครบ", "Error");
                ShowPopAlert1("Error!", "ชำระราคาสินค้าไม่ครบ", false);
                return;
            }
            if (diff < 0) {
           //     ShowAlert("ชำระราคาสินค้าเกินจำนวน", "Error");
                ShowPopAlert1("Error!", "ชำระราคาสินค้าเกินจำนวน", false);
                return;
            }
            if (POSSaleService.DocSet.Payment.Count == 0) {
                //ShowAlert("ยังไม่ชำระเงิน", "Error");
                ShowPopAlert1("Error!", "ยังไม่ชำระเงิน", false);
                return;
            }
            var rr = POSSaleService.SaveInvoice(POSSaleService.DocSet);
            if (rr.Result == "fail") {
                //ShowAlert(rr.Message1, "Error");
                ShowPopAlert1("Error!", rr.Message1, false);
            } else {
                IsSaveSuccess = true;
                LoadData();
            }
        }

        protected void btnSaveInvoice_Click(object sender, EventArgs e) {
            SaveInvoice();
        }


        protected void grdlist_ItemCommand(object source, DataListCommandEventArgs e) {
            if (e.CommandName == "sel") {
                var itemId = e.CommandArgument.ToString(); 
                POSSaleService .DocSet.SelectItem= POSSaleService.Menu.Where(o => o.ItemID == itemId).FirstOrDefault();
                POSSaleService.AddItem(POSSaleService.DocSet, -1);
                BindGrdItemLine();
                BindData();

            }
        }
        protected void btnDirectInputQty_Click(object sender, EventArgs e) {
            decimal inputQty = 0;
        decimal.TryParse (txtInputQty.Text, out inputQty);
            POSSaleService.AddItem(POSSaleService.DocSet, inputQty);
            popInputQty.ShowOnPageLoad = false;
            BindGrdItemLine();

        }
        protected void grditemSelect_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "Del") {
                if (e.CommandArgument == null) {
                    return;
                }
                int linenum = Convert.ToInt32(e.CommandArgument);
                var r = POSSaleService.DeleteItem(POSSaleService.DocSet, linenum); 
                BindGrdItemLine();
                BindData();

            }
            if (e.CommandName == "editqty") {
                if (e.CommandArgument == null) {
                    return;
                }
                int linenum = Convert.ToInt32(e.CommandArgument); 
                var itemId = POSSaleService.DocSet.Line.Where(o => o.LineNum == linenum).FirstOrDefault();
                POSSaleService.DocSet.SelectItem = POSSaleService.Menu.Where(o => o.ItemID == itemId.ItemID).FirstOrDefault();                 
                txtInputQty.Value = itemId.Qty;
                popInputQty.ShowOnPageLoad = true;
            }
        }
        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
        }


        protected void cboCustomerID_OnItemsRequestedByFilterCondition_SQL(object source, ListEditItemsRequestedByFilterConditionEventArgs e) {

        }

        protected void cboCustomerID_OnItemRequestedByValue_SQL(object source, ListEditItemRequestedByValueEventArgs e) {

        }

         

      
        protected void grditemSelect_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.Footer) {
           
            
                e.Row.Cells[0].Text = "รวม";
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Text = POSSaleService.DocSet.Line.Sum(o => o.Qty).ToString("N2");
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = POSSaleService.DocSet.Line.Sum(o=>o.TotalAmt).ToString("N2");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
          
            }
        }

        protected void grdpay_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "Del") {
                if (e.CommandArgument == null) {
                    return;
                }
                string paymentType = e.CommandArgument.ToString();
                POSSaleService.DocSet.Payment.RemoveAll(o => o.PaymentType == paymentType);
                POSSaleService.DocSet = POSSaleService.CalDocSet(POSSaleService.DocSet);
                BindGrdPayment();
            }
        }

    
        protected void grdpay_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.Footer) { 
                e.Row.Cells[1].Text = "รวม";
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[2].Text = POSSaleService.DocSet.Payment.Sum(o=>o.PayAmt).ToString("N2");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
             
            }
        }

  

        protected void btnpay_Click(object sender, EventArgs e) {
            txtCustPayAmt.Text = "0";
            txtCustPayAmt.Text = "0";
            POSSaleService.DocSet = POSSaleService.CalDocSet(POSSaleService.DocSet);
            SetDivVisible("pay");
        }



        protected void btncloseAddItem_Click(object sender, EventArgs e) {
            SetDivVisible("select");
        }

        protected void btnAddItem_Click(object sender, EventArgs e) {
       
            if (txtPrice.Text == "") {
                //ShowAlert("ระบุราคา", "Error");
                ShowPopAlert1("Error!", "ระบุราคา", false);
                return;
            }
            if (txtqty.Text == "") {
                //ShowAlert("ระบุจำนวน", "Error");
                ShowPopAlert1("Error!", "ระบุจำนวน", false);
                return;
            }
            decimal disc_amt = 0;
            decimal.TryParse(txtDisAmtPer.Text, out disc_amt);
            POSSaleService.AddItem(POSSaleService.DocSet, -1, disc_amt);
            BindGrdItemLine();

            SetDivVisible("main");
          
        }
      
        protected void btnSubmitPayment_Click(object sender, EventArgs e) {
            AddPayment();
            SetDivVisible("main");
        }
        private void AddPayment() {
            decimal CustPayAmt = 0;
            decimal.TryParse(txtCustPayAmt.Text, out CustPayAmt);
            string paymentType = cboTenderType.SelectedValue;
            POSSaleService.AddPayment(POSSaleService.DocSet, paymentType, CustPayAmt);
            cboTenderType.SelectedValue = "CASH";
            BindGrdPayment();
        }
        private void AddOnlinePayment() {
            POSSaleService.DocSet = POSSaleService.CalDocSet(POSSaleService.DocSet);
         var h=  POSSaleService.DocSet.Head; 
            POSSaleService.AddPayment(POSSaleService.DocSet, "ONLINE", Convert.ToDecimal( h.NetTotalAfterRound));
            cboTenderType.SelectedValue = "CASH";
            BindGrdPayment();
        }
        protected void btnCancel_Click(object sender, EventArgs e) {

        }

        protected void btnnew_Click(object sender, EventArgs e) {
            NewDoc();
        }

        private void NewDoc() {
           // string comId = POSSaleService.DocSet.Head.ComID;
           // string shiptoLoc = POSSaleService.DocSet.Head.ShipToLocID;
           // string macno = POSSaleService.DocSet.Head.MacNo;
           // bool isVatRegister =Convert.ToBoolean( POSSaleService.DocSet.Head.IsVatRegister);
           // DateTime tranDate = POSSaleService.DocSet.Head.BillDate;

           // POSSaleService.DocSet = POSSaleService.NewTransaction();
           // POSSaleService.DocSet.Head.ComID = comId;
           // POSSaleService.DocSet.Head.IsVatRegister = isVatRegister;
           //POSSaleService.DocSet.Head.ShipToLocID = "";
           // POSSaleService.DocSet.Head.BillDate = tranDate;
           // POSSaleService.DocSet.Head.MacNo = macno;
           // if (POSSaleService.DocSet.Head.IsVatRegister == true) {
           //     POSSaleService.DocSet.Head.VatRate = LoginService.LoginInfo.CurrentVatRate;
           // }

POSSaleService.NewBillFromPreviousBill();
            string myurl = $"~/POSC/POSSaleDetail?menu={hddmenu.Value }";
            Response.Redirect(myurl);
             
              
        }

        private void ExportPrintBill() {
            var h = POSSaleService.DocSet.Head;
            var stream = new MemoryStream();
            #region save bill local path before print
            if (h.IsVatRegister == true) {
                var report = new R412();
                report.initData(h.BillID);
                report.ExportToPdf(stream);
            } else {
                var report = new R411();
                report.initData(h.BillID);
                report.ExportToPdf(stream);
            }

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();
            #endregion


            if (MobileHelper.isMobileBrowser()) {
                string baseurl = URLHelper.GetBaseUrl();
                string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
                yurl = yurl.Replace("http://", "https://");
                
                string func = "sendUrlToPrint('" + yurl + "');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
                return;
            } else {
                string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
                
                popPrint.ContentUrl = xurl;
                popPrint.ShowOnPageLoad = true;
                //Response.RedirectPermanent(xurl);
            }
        }
        private void ExportPrintInvoice() {
            var h = POSSaleService.DocSet.Head;
            var stream = new MemoryStream();

            //var img_format = new ImageExportOptions() {
            //    ExportMode = ImageExportMode.DifferentFiles,
            //    Format = System.Drawing.Imaging.ImageFormat.Jpeg,
            //    PageBorderWidth = 0,
            //    Resolution = 192
            //};

            #region save bill local path before print
            if (h.IsVatRegister == true) {
                var report = new R402();
                report.initData(h.BillID);
                report.ExportToPdf(stream);
               // report.ExportToImage(stream, img_format);
                 
            } else {
                var report = new R401();
                report.initData(h.BillID);
                report.ExportToPdf(stream);
            }

            string myfilename = Guid.NewGuid().ToString().Substring(0, 20) + ".pdf";
            string myfillfilename = @"~/TempFile/Print/" + myfilename;
            string serverpath = Server.MapPath(myfillfilename);
            FileStream myfile = new FileStream(serverpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.WriteTo(myfile);
            myfile.Close();
            stream.Close();
            #endregion


            if (MobileHelper.isMobileBrowser()) {
                string baseurl = URLHelper. GetBaseUrl();
                string yurl = String.Format("{0}/TempFile/Print/{1}", baseurl, myfilename);
                yurl = yurl.Replace("http://", "https://");
                

                string func = "sendUrlToPrint('" + yurl + "');";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
            } else {
                string xurl = String.Format("~/TempFile/Print/{0}", myfilename);
                
                popPrint.ContentUrl = xurl;
                popPrint.ShowOnPageLoad = true;
                //Response.RedirectPermanent(xurl);
            }

        }


     

        private void SaveAlert(string inPutTopic, string inPutMsg, string inPutType) {
            string callFuction = string.Format("ServerAlert('{0}','{1}','{2}');", inPutTopic, inPutMsg, inPutType);
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", callFuction, true);
        }



        protected void btnDiscPer_Click(object sender, EventArgs e) {

            decimal disc = 0;
            decimal.TryParse(txtDisAmtPer.Text, out disc);
            POSSaleService .DocSet.SelectItem = POSSaleService.Menu.Where(o => o.ItemID == "DISCPER01").FirstOrDefault();
            POSSaleService.AddItem(POSSaleService.DocSet,1, disc);
            BindGrdItemLine();
            BindData();
            txtDisAmtPer.Text = "0";

             
        }
        protected void btnDiscAmt_Click(object sender, EventArgs e) {
            decimal disc = 0;
            decimal.TryParse(txtDisAmtPer.Text, out disc);
            POSSaleService.DocSet.SelectItem = POSSaleService.Menu.Where(o => o.ItemID == "DISCAMT01").FirstOrDefault();
            POSSaleService.AddItem(POSSaleService.DocSet,1, disc);
            BindGrdItemLine();
            BindData();
            txtDisAmtPer.Text = "0";
        }
        protected void btnprintBill_Click(object sender, EventArgs e) {
          
            if (POSSaleService.DocSet.Line.Count == 0) {
                ShowPopAlert1("Error!", "ต้องมีรายการอาหารก่อน", true);
                return;
            }
            ExportPrintBill();
        }

  

        protected void btnprintInv_Click(object sender, EventArgs e) {

            var head = POSSaleService.DocSet.Head;
            if (head.INVID == "") {
                ShowPopAlert1("Error!", "ต้องเปิดบิลก่อนพิมพ์ใบเสร็จรับเงิน", true);
                return;
            }
            ExportPrintInvoice();
        }
  

        protected void btnprintFInv_Click(object sender, EventArgs e) {

            var head = POSSaleService.DocSet.Head; 
            if ( POSSaleService.DocSet.Payment.Count == 0 || head.INVID == "") {
                ShowPopAlert1("Error!", "ต้องจ่ายเงินก่อนถึงจะพิมพ์ได้", true);
                return;
            }
            POSSaleTax.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"~/POSC/POSSaleTax?menu={hddmenu.Value }";
            Response.Redirect(myurl);
        }

        protected void btnviewlist_Click(object sender, EventArgs e) {
            string myurl = $"~/POSC/POSInvoiceList?menu={ hddmenu.Value }";
            Response.Redirect(myurl);
        }
        protected void btnCheckBill_Click(object sender, EventArgs e) {
            string myurl = $"~/POSC/POSBillList?menu={hddmenu.Value}";
            Response.Redirect(myurl);
        }

        protected void btnDelete_Click(object sender, EventArgs e) {
          var h=  POSSaleService.DocSet.Head; 
             var r = POSSaleService.DeleteDoc( h.BillID);
            if (r.Result=="fail") {
                //ShowAlert(r.Message1, "Error");
                ShowPopAlert1("Error!", r.Message1, false);
            } else {
                string myurl = $"~/POSC/POSInvoiceList?menu={hddmenu.Value}"; 
                Response.Redirect(myurl);
            }
        }

        //protected void btnDeletelPermanent_Click(object sender, EventArgs e) {
        //    var h = POSSaleService.DocSet.Head;
        //    var r = POSSaleService.DeletePermanent(h.BillID);
        //    if (r.Result == "fail") {
        //        //ShowAlert(r.Message1, "Error");
        //        ShowPopAlert1("Error!", r.Message1, false);
        //    } else {
        //        string myurl = $"~/POSC/POSInvoiceList?menu={hddmenu.Value}";
        //        Response.Redirect(myurl);
        //    }
        //}

        protected void cboTenderType_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboTenderType.SelectedValue == "") {
                return;
            }
            var h = POSSaleService.DocSet.Head;
          
            if (cboTenderType.SelectedValue == "CASH") {
                txtCustPayAmt.Text = "0";
            } else {
                txtCustPayAmt.Text = (Convert.ToDecimal( h.NetTotalAfterRound)- Convert.ToDecimal(h.PayTotalAmt)) .ToString("n2");
            }
        }

        protected void cboShipTo_SelectedIndexChanged(object sender, EventArgs e) {
         
        }

        protected void btnShipTo_Click(object sender, EventArgs e) {
            POSShipToPage.PreviousPage= HttpContext.Current.Request.Url.PathAndQuery;
            string myurl = $"../POSC/POSShipToPage?menu={hddmenu.Value}";
            Response.Redirect(myurl);


        }

        protected void dtInvDate_DateChanged(object sender, EventArgs e) {
            var h = POSSaleService.DocSet.Head;
            if (dtInvDate.Value != null)
            {
                h.BillDate = Convert.ToDateTime(dtInvDate.Value).Date;
            }
        }

        protected void cboTable_SelectedIndexChanged(object sender, EventArgs e) {
            var h = POSSaleService.DocSet.Head;          
            h.TableID = cboTable.SelectedValue;
            h.TableName = cboTable.SelectedItem.Text;
        }
    }
}