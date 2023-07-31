using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.RC101.RunReportRC;
using PrintMaster.Helper;
using System;
using DevExpress.ClipboardSource.SpreadsheetML;
using System.Runtime.InteropServices;
using DevExpress.CodeParser;
using System.Web.UI.WebControls;

namespace PrintMaster.PrintFile.Accy.RC101 {
    public partial class RC101 : XtraReport {

        public RC101(RC101Set data, string UseFor, string typeprint, string copyno) {
            InitializeComponent();
            LoadData(data, UseFor, typeprint, copyno);
        }
        private void LoadData(RC101Set d, string UseFor, string typeprint, string copyno) {
            if (d.ComBrn == "") {
                lblBrnCode.Text = "";
            } else {
                int brnnumber = 0;
                if (int.TryParse(d.ComBrn, out brnnumber)) {//ระบุเป็นรหัสสาขา
                    lblBrnCode.Text = "สาขา " + d.ComBrn;
                } else {//ระบุเป็นสำนักงานใหญ่หรืออื่นๆ
                    lblBrnCode.Text = d.ComBrn;
                }
            }

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

            int pi = 1;
            if (d.payline.Count > 0) {
                for (int i = 0; i < Math.Min(d.payline.Count, 5); i++) {
                    var l = d.payline[i];
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

            //int pi = 1;
            //if (d.payline.Count > 0) {
            //    foreach (var l in d.payline) {
            //        if (pi == 1) {
            //            if (l.PayBy == "CASH") {
            //                ckPayby1.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby1.Checked = true;

            //                ckPayby1.Visible = true;



            //            }
            //            if (l.PayBy == "TRANSFER") {

            //                ckPayby1.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby1.Checked = true;
            //                ckPayby1.Visible = true;

            //            }
            //            if (l.PayBy == "CHEQUE") {

            //                ckPayby1.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby1.Checked = true;
            //                ckPayby1.Visible = true;

            //            }
            //            if (l.PayBy == "OTHER") {

            //                ckPayby1.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby1.Checked = true;
            //                ckPayby1.Visible = true;

            //            }
            //            if (l.PayBy == "WHT") {

            //                ckPayby1.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";
            //                ckPayby1.Checked = true;
            //                ckPayby1.Visible = true;

            //            }
            //            pi++;
            //            continue;
            //        }
            //        if (pi == 2) {
            //            if (l.PayBy == "CASH") {
            //                ckPayby2.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby2.Checked = true;

            //                ckPayby2.Visible = true;




            //            }
            //            if (l.PayBy == "TRANSFER") {

            //                ckPayby2.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby2.Checked = true;
            //                ckPayby2.Visible = true;

            //            }
            //            if (l.PayBy == "CHEQUE") {

            //                ckPayby2.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby2.Checked = true;
            //                ckPayby2.Visible = true;

            //            }
            //            if (l.PayBy == "OTHER") {

            //                ckPayby2.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby2.Checked = true;
            //                ckPayby2.Visible = true;


            //            }
            //            if (l.PayBy == "WHT") {

            //                ckPayby2.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
            //                ckPayby2.Checked = true;
            //                ckPayby2.Visible = true;

            //            }
            //            pi++;
            //            continue;
            //        }
            //        if (pi == 3) {
            //            if (l.PayBy == "CASH") {
            //                ckPayby3.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby3.Checked = true;

            //                ckPayby3.Visible = true;


            //            }
            //            if (l.PayBy == "TRANSFER") {

            //                ckPayby3.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby3.Checked = true;
            //                ckPayby3.Visible = true;

            //            }
            //            if (l.PayBy == "CHEQUE") {

            //                ckPayby3.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby3.Checked = true;
            //                ckPayby3.Visible = true;

            //            }
            //            if (l.PayBy == "OTHER") {

            //                ckPayby3.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby3.Checked = true;
            //                ckPayby3.Visible = true;

            //            }
            //            if (l.PayBy == "WHT") {

            //                ckPayby3.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
            //                ckPayby3.Checked = true;
            //                ckPayby3.Visible = true;

            //            }
            //            pi++;
            //            continue;

            //        }
            //        if (pi == 4) {
            //            if (l.PayBy == "CASH") {
            //                ckPayby4.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby4.Checked = true;

            //                ckPayby4.Visible = true;



            //            }
            //            if (l.PayBy == "TRANSFER") {

            //                ckPayby4.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby4.Checked = true;
            //                ckPayby4.Visible = true;

            //            }
            //            if (l.PayBy == "CHEQUE") {

            //                ckPayby4.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby4.Checked = true;
            //                ckPayby4.Visible = true;

            //            }
            //            if (l.PayBy == "OTHER") {

            //                ckPayby4.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby4.Checked = true;
            //                ckPayby4.Visible = true;

            //            }
            //            if (l.PayBy == "WHT") {

            //                ckPayby4.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
            //                ckPayby4.Checked = true;
            //                ckPayby4.Visible = true;

            //            }
            //            pi++;
            //            continue;

            //        }
            //        if (pi == 5) {
            //            if (l.PayBy == "CASH") {
            //                ckPayby5.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby5.Checked = true;

            //                ckPayby5.Visible = true;



            //            }
            //            if (l.PayBy == "TRANSFER") {

            //                ckPayby5.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby5.Checked = true;
            //                ckPayby5.Visible = true;

            //            }
            //            if (l.PayBy == "CHEQUE") {

            //                ckPayby5.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
            //                ckPayby5.Checked = true;
            //                ckPayby5.Visible = true;

            //            }
            //            if (l.PayBy == "OTHER") {

            //                ckPayby5.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
            //                ckPayby5.Checked = true;
            //                ckPayby5.Visible = true;

            //            }
            //            if (l.PayBy == "WHT") {

            //                ckPayby5.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
            //                ckPayby5.Checked = true;
            //                ckPayby5.Visible = true;

            //            }

            //            continue;
            //        }
            //    }
            //}

            if (typeprint == "RC101") {//ใบเสร็จรับเงินขายสินค้า
                lblCaptionInfo1.Text = "ใบเสร็จรับเงิน";
                lblCaptionInfo3.Text = "Receipt";
                GF_GROUP1.Visible = false;

                if (copyno == "1") {
                    lblCaptionInfo2.Text = "สำเนา / COPY";
                } else {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL";
                }
            } else {
                //ใบเสร็จรับเงินงานบริการ
                lblCaptionInfo1.Text = "ใบเสร็จรับเงิน / ใบกำกับภาษี";
                lblCaptionInfo3.Text = "(Receipt / Tax invoice)";
                GF_GROUP1.Visible = true;

                if (copyno == "1") {
                    lblCaptionInfo2.Text = "สำเนา / COPY / " + UseFor;
                } else {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL / " + UseFor;
                }
            }

            lblBeforeVat.Text = d.head.PayINVAmt.ToString("n2");
            lblVatAmt.Text = d.head.PayINVVatAmt.ToString("n2");

            var fl = d.line.FirstOrDefault();
            lblVatRate.Text = "VAT " + "0" + "%";
            if (fl != null) {
                lblVatRate.Text = "VAT " + fl.VatRate.ToString("n2") + "%";
            }

            lblNetTotalAmtIncVat.Text = d.head.PayINVTotalAmt.ToString("N2");

            lblRemark.Text = d.head.Remark2;

            lblThaiBath.Text = "(" + TextTHB.Process(d.head.PayINVTotalAmt.ToString("N2")) + ")";

            lblCustomerID.Text = d.head.CustName;

            lblAddr1.Text = d.BillAddr1 ;
            if (string.IsNullOrEmpty(d.BillAddr1)) {
                lblAddr1.Text = d.BillAddr2;
            }
            lblMobile.Text = d.head.Mobile;
            lblEmail.Text = d.head.Email;
            lblDocumentDate.Text = d.head.RCDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            lblRC_ID.Text = d.head.RCID != null ? d.head.RCID : "";
            lblTaxId.Text = d.head.CustTaxID != null ? d.head.CustTaxID : "";
            foreach (var x in d.linePrint) {
                x.RCType = d.head.DocType;
                if (x.RCType== "ORC1") {//ขายสินค้า
                    x.TotalAmt = x.TotalAmtIntVat;
                }

                //if (x.RCType == "ORC2") {//ขายบริการ
                //    x.TotalAmt = x.TotalAmt;
                //} 

            }
            objectDataSource2.DataSource = d.linePrint;

        }
    }
}
