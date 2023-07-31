using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.INV101.RunReportINV;
using PrintMaster.Helper;
using DevExpress.ClipboardSource.SpreadsheetML;

namespace PrintMaster.PrintFile.Accy.INV101 {
    public partial class INV101 : XtraReport {

        public INV101(INV101Set data, string UseFor, string typeprint, string copyno, string payment_memo) {
            InitializeComponent();
            LoadData(data, UseFor, typeprint, copyno, payment_memo);
        }


        private void LoadData(INV101Set d, string UseFor, string typeprint, string copyno, string payment_memo) {
            string brn = "";
            if (d.ComBrn == "") {
                brn  = "";
            } else {
                int brnnumber = 0;
                if (int.TryParse(d.ComBrn, out brnnumber)) {//ระบุเป็นรหัสสาขา
                    brn = "สาขา " + d.ComBrn;
                } else {//ระบุเป็นสำนักงานใหญ่หรืออื่นๆ
                    brn  = d.ComBrn;
                }
            }
            //int row = 1;
            //foreach (var l in d.line) {
            //    l.LineNum = row;
            //    if (l.Remark1!="") {
            //        l.ItemName = l.ItemName + Environment.NewLine + l.Remark1;
            //    }
            //    //var master = db.MasterTypeLine.Where(o => o.ValueTXT == l.Unit).FirstOrDefault();
            //    //if (master != null) {
            //    //    l.Unit = master.Description1;
            //    //}
            //    row++;
            //}

            //if (!string.IsNullOrEmpty(d.ComImage64))
            //{
            //    logoimg.Image = ImageHelper.Base64ToImage(d.ComImage64);
            //}


            if (!string.IsNullOrEmpty(d.ComImage64)) {
                //logoimg.Image = ImageHelper.Base64ToImage(d.ComImage64);
                logoimg.ImageUrl = d.ComImage64;
            }
            lblComanyNameTH.Text = d.ComName;
            lblComanyAddr.Text = d.ComAddress + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + d.ComTax + " " + d.ComBrn;
            pic_signature1.ImageUrl = d.SignnatureUrl1;
            pic_signature2.ImageUrl = d.SignnatureUrl1;
            if (typeprint == "INV101") {
                lblCaptionInfo1.Text = "ใบกำกับภาษี / ใบส่งของ / ใบแจ้งหนี้";
                lblCaptionInfo3.Text = "(TAX INVOICE/DELIVERY ORDER/DEBIT NOTE)";
                lblfooter2.Text = "ผู้วางบิล / ผู้ส่งสินค้า";
                lblfooter3.Text = "ผู้รับแจ้งหนี้/ผู้รับสินค้า";
                lblPaymentTopic.Text = "เงื่อนไขการชำระ : ";
                if (copyno == "1") {
                    lblCaptionInfo2.Text = "สำเนา / COPY / " + UseFor;
                } else {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL / " + UseFor;
                }
                lblPaymentMemo.Text = d.head.PaymentMemo;
            } else if (typeprint == "INV101_1") {//print ขายสด
                lblCaptionInfo1.Text = "ใบเสร็จรับเงิน /ใบกำกับภาษี";
                lblCaptionInfo3.Text = "(RECEIPT / TAX INVOICE)";
                lblfooter3.Text = "ผู้รับสินค้า / บริการ";
                lblfooter2.Text = "ผู้รับเงิน";
                lblPaymentTopic.Text = "การชำระเงิน : ";
                xrLabel20.Visible = true;
                if (copyno == "1") {
                    lblCaptionInfo2.Text = "สำเนา / COPY / " + UseFor;
                } else {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL / " + UseFor;
                }
               
                int pi = 1;
                if (d.PaymentLine.Count > 0) {
                    for (int v = 0; v < Math.Min(d.PaymentLine.Count, 5); v++) {
                        var l = d.PaymentLine[v];
                        XRCheckBox checkBox = null;
                        switch (pi) {
                            case 1:
                                checkBox = ckPayby1;
                                break;
                            case 2:
                                checkBox = ckPayby2;
                                break;
                            case 3:
                                checkBox = ckPayby3;
                                break;
                            case 4:
                                checkBox = ckPayby4;
                                break;
                            case 5:
                                checkBox = ckPayby5;
                                break;
                        }

                        if (l.PayBy == "CASH") {
                            checkBox.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                        } else if (l.PayBy == "TRANSFER") {
                            checkBox.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                        } else if (l.PayBy == "CHEQUE") {
                            checkBox.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                        } else if (l.PayBy == "OTHER") {
                            checkBox.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                        } else if (l.PayBy == "WHT") {
                            checkBox.Text += "ภาษีหัก ณ ที่จ่าย  " + l.PayAmt.ToString("N2") + " บาท ";
                        }

                        checkBox.Checked = true;
                        checkBox.Visible = true;
                        pi++;
                    }
                }


            } else {
                lblCaptionInfo1.Text = "ใบแจ้งหนี้ / ใบวางบิล";
                lblCaptionInfo3.Text = "(INVOICE)";
                lblfooter2.Text = "ผู้วางบิล / ผู้ส่งสินค้า";
                lblfooter3.Text = "ผู้รับวางบิล/ใบแจ้งหนี้/ผู้รับสินค้า";
                lblPaymentTopic.Text = "เงื่อนไขการชำระ : ";
                if (copyno == "1") {
                    lblCaptionInfo2.Text = "สำเนา / COPY";
                } else {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL";
                }
                lblPaymentMemo.Text = d.head.PaymentMemo;
            }

            lblBaseTotalAmtHead.Text = d.head.BaseNetTotalAmt.ToString("N2");
            lblVatAmount.Text = d.head.NetTotalVatAmt.ToString("N2");
            lblNetTotalAmtIncVat.Text = d.head.NetTotalAmtIncVat.ToString("N2");
            lblOntopDiscount.Text = d.head.OntopDiscAmt.ToString("n2");
            lblNetTotalAmt.Text = d.head.NetTotalAmt.ToString("n2");

            lblRemark.Text = d.head.Remark1;

            //string remark = d.head.Remark1;
            //lblRemark.Text = remark;
            //if (remark.Trim()=="") {
            //    lblCaptionRemark.Text = "";
            //}



            lblThaiBath.Text = "(" + TextTHB.Process(d.head.NetTotalAmtIncVat.ToString("N2")) + ")";
            lblCustomerID.Text = d.head.CustName;
            lblAddr1.Text = d.head.BillAddr1 /*+ Environment.NewLine + d.head.BillAddr2*/;
            if (string.IsNullOrEmpty(d.head.BillAddr1)) {
                lblAddr1.Text = d.head.CustAddr1 + " " + d.head.CustAddr2;
            }

            lblMobile.Text = d.head.Mobile;
            lblEmail.Text = d.head.Email;

          
            lblDocumentDate.Text = d.head.INVDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            lblINV_ID.Text = d.head.INVID != null ? d.head.INVID : "";
            lblPOId.Text = d.head.POID != null ? d.head.POID : "";
            lblTaxId.Text = d.head.CustTaxID != null ? d.head.CustTaxID : "";
            lblTaxId.Text = lblTaxId.Text + "   " + brn;
            int i = 0;
            foreach (var dd in d.line.OrderBy(o => o.Sort)) {
                if (dd.ItemTypeID != "MISC") {
                    i++;
                    dd.RN = i;
                } else {
                    dd.RN = -99;
                } 
            }
            objectDataSource2.DataSource = d.line.OrderBy(o=>o.Sort).ToList();
        }

        int RecordCount = 0;
        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            var isPageBreak = Convert.ToBoolean(GetCurrentColumnValue("PageBreak"));
            RecordCount++;
            if (isPageBreak ) {
                Detail.PageBreak = PageBreak.AfterBand;
                RecordCount = 0;
            } else {
                Detail.PageBreak = PageBreak.None;
            }
            
            //int value = CurrentRowIndex + 1;
            //if (value % 3 == 0) {
            //    (sender as DetailBand).PageBreak = PageBreak.BeforeBand;
            //}
        }

        private void xr_endline_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            //var isPageBreak = Convert.ToBoolean(GetCurrentColumnValue("PageBreak"));
            //XRLine line = sender as XRLine;
            //if (isPageBreak) {
            //    line.Visible = true;
            //}
        }
        //private void xrPageBreak1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        //    XRPageBreak control = sender as XRPageBreak;
        //    int value = CurrentRowIndex + 1;
        //    if (value % 2 == 0) { 
        //        control.Visible = true;
        //    xline_end.Visible = true;
        //    } else { 
        //        control.Visible = false;
        //    xline_end.Visible = false;
        //    }
        //}
    }
}
