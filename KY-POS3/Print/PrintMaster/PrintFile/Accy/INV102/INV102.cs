using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.INV102.RunReportINV102;
using PrintMaster.Helper;
using System;
using DevExpress.ClipboardSource.SpreadsheetML;
using System.Runtime.InteropServices;
using DevExpress.CodeParser;

namespace PrintMaster.PrintFile.Accy.INV102 {
    public partial class INV102 : XtraReport {

        public INV102(INV102Set data, string UseFor, string typeprint, string copyno) {
            InitializeComponent();
            LoadData(data, UseFor, typeprint, copyno);
        }
        private void LoadData(INV102Set d, string UseFor, string typeprint, string copyno) {
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
                foreach (var l in d.payline) {
                    if (pi == 1) {
                        if (l.PayBy == "CASH") {
                            ckPayby1.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby1.Checked = true;
                            
                            ckPayby1.Visible = true;
                           


                        }
                        if (l.PayBy == "TRANSFER") {
                           
                            ckPayby1.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby1.Checked = true;
                            ckPayby1.Visible = true;
                           
                        }
                        if (l.PayBy == "CHEQUE") {
                            
                            ckPayby1.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby1.Checked = true;
                            ckPayby1.Visible = true;
                            
                        }
                        if (l.PayBy == "OTHER") {
                           
                            ckPayby1.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby1.Checked = true;
                            ckPayby1.Visible = true;
                           
                        }
                        if (l.PayBy == "WHT") {
                           
                            ckPayby1.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";
                            ckPayby1.Checked = true;
                            ckPayby1.Visible = true;
                           
                        }
                        pi++;
                        continue;
                    }

                    if (pi == 2) {
                        if (l.PayBy == "CASH") {
                            ckPayby2.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby2.Checked = true;
                            
                            ckPayby2.Visible = true;
                           



                        }
                        if (l.PayBy == "TRANSFER") {
                           
                            ckPayby2.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby2.Checked = true;
                            ckPayby2.Visible = true;
                            
                        }
                        if (l.PayBy == "CHEQUE") {
                           
                            ckPayby2.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby2.Checked = true;
                            ckPayby2.Visible = true;
                           
                        }
                        if (l.PayBy == "OTHER") {
                           
                            ckPayby2.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby2.Checked = true;
                            ckPayby2.Visible = true;
                           

                        }
                        if (l.PayBy == "WHT") {
                           
                            ckPayby2.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
                            ckPayby2.Checked = true;
                            ckPayby2.Visible = true;
                            
                        }
                        pi++;
                        continue;
                    }

                    if (pi == 3) {
                        if (l.PayBy == "CASH") {
                            ckPayby3.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby3.Checked = true;
                           
                            ckPayby3.Visible = true;
                            

                        }
                        if (l.PayBy == "TRANSFER") {
                           
                            ckPayby3.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby3.Checked = true;
                            ckPayby3.Visible = true;
                           
                        }
                        if (l.PayBy == "CHEQUE") {
                           
                            ckPayby3.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby3.Checked = true;
                            ckPayby3.Visible = true;
                            
                        }
                        if (l.PayBy == "OTHER") {
                           
                            ckPayby3.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby3.Checked = true;
                            ckPayby3.Visible = true;
                           
                        }
                        if (l.PayBy == "WHT") {
                           
                            ckPayby3.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
                            ckPayby3.Checked = true;
                            ckPayby3.Visible = true;
                            
                        }
                        pi++;
                        continue;

                    }

                    if (pi == 4) {
                        if (l.PayBy == "CASH") {
                            ckPayby4.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby4.Checked = true;
                           
                            ckPayby4.Visible = true;
                            


                        }
                        if (l.PayBy == "TRANSFER") {
                           
                            ckPayby4.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby4.Checked = true;
                            ckPayby4.Visible = true;
                            
                        }
                        if (l.PayBy == "CHEQUE") {
                            
                            ckPayby4.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby4.Checked = true;
                            ckPayby4.Visible = true;
                            
                        }
                        if (l.PayBy == "OTHER") {
                            
                            ckPayby4.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby4.Checked = true;
                            ckPayby4.Visible = true;
                            
                        }
                        if (l.PayBy == "WHT") {
                            
                            ckPayby4.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
                            ckPayby4.Checked = true;
                            ckPayby4.Visible = true;
                            
                        }
                        pi++;
                        continue;

                    }

                    if (pi == 5) {
                        if (l.PayBy == "CASH") {
                            ckPayby5.Text = "เงินสด  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby5.Checked = true;
                            
                            ckPayby5.Visible = true;
                            


                        }
                        if (l.PayBy == "TRANSFER") {
                            
                            ckPayby5.Text += "เงินโอน  " + l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby5.Checked = true;
                            ckPayby5.Visible = true;
                            
                        }
                        if (l.PayBy == "CHEQUE") {
                            
                            ckPayby5.Text += "เช็ค  " + l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckPayby5.Checked = true;
                            ckPayby5.Visible = true;
                            
                        }
                        if (l.PayBy == "OTHER") {
                           
                            ckPayby5.Text += "อื่นๆ  " + l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckPayby5.Checked = true;
                            ckPayby5.Visible = true;
                           
                        }
                        if (l.PayBy == "WHT") {
                            
                            ckPayby5.Text += "ภาษีหัก ณ ที่จ่าย  " +  l.PayAmt.ToString("N2") + " บาท ";;
                            ckPayby5.Checked = true;
                            ckPayby5.Visible = true;
                           
                        }

                        continue;
                    }

                }
            }

            //if (typeprint == "RC101") {//ใบเสร็จรับเงินขายสินค้า
                lblCaptionInfo1.Text = "ใบเสร็จรับเงิน";
                lblCaptionInfo3.Text = "Receipt";
                GF_GROUP1.Visible = false;

              //  if (copyno == "1") {
                //    lblCaptionInfo2.Text = "สำเนา / COPY";
               // } else {
               //     lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL";
               // }
           // } else {
                //ใบเสร็จรับเงินงานบริการ
             //   lblCaptionInfo1.Text = "ใบเสร็จรับเงิน / ใบกำกับภาษี";
            //    lblCaptionInfo3.Text = "(Receipt / Tax invoice)";
            //    GF_GROUP1.Visible = true;

               // if (copyno == "1") {
                    //lblCaptionInfo2.Text = "สำเนา / COPY / " + UseFor;
               // } //else {
                  //  lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL / " + UseFor;
               // }
            //}

            lblBeforeVat.Text = d.head.NetTotalAmt.ToString("n2");
            lblVatAmt.Text = d.head.NetTotalVatAmt.ToString("n2");

            var fl = d.line.FirstOrDefault();
            lblVatRate.Text = "VAT " + "0" + "%";
            if (fl != null) {
                lblVatRate.Text = "VAT " + fl.VatRate.ToString("n2") + "%";
            }


            lblNetTotalAmtIncVat.Text = d.head.NetTotalAmtIncVat.ToString("N2");
            lblRemark.Text = d.head.Remark2;
            lblBaseTotalAmtHead.Text = d.head.BaseNetTotalAmt.ToString("N2");
            lblOntopDiscount.Text = d.head.OntopDiscAmt.ToString("n2");


            //lblThaiBath.Text = "(" + TextTHB.Process(d.head.NetTotalAmtIncVat.ToString("N2")) + ")";
            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            string strNumber = d.head.NetTotalAmtIncVat.ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            string BahtText = "";

            if (strInteger == "0") {
                BahtText = "ศูนย์บาท";
            } else {
                int strLength = strInteger.Length;
                for (int i = 0; i < strInteger.Length; i++) {
                    string number = strInteger.Substring(i, 1);
                    if (number != "0") {
                        if (i == strLength - 1 && number == "1" && strLength != 1) {
                            BahtText += "เอ็ด";
                        } else if (i == strLength - 2 && number == "2" && strLength != 1) {
                            BahtText += "ยี่";
                        } else if (i != strLength - 2 || number != "1") {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "บาท";
            }

            if (strSatang == "00") {
                BahtText += "ถ้วน";
            } else {
                int strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++) {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0") {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0") {
                            BahtText += "เอ็ด";
                        } else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0") {
                            BahtText += "ยี่";
                        } else if (i != strLength - 2 || number != "1") {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            lblThaiBath.Text = BahtText;

            lblCustomerID.Text = d.head.CustName;
            //lblAddr1.Text = "";
            //if (!string.IsNullOrEmpty(d.BillAddr1)) {
            //    lblAddr1.Text = d.BillAddr1 + Environment.NewLine + d.BillAddr2;
            //}
            lblAddr1.Text = d.head.BillAddr1 /*+ Environment.NewLine + d.head.BillAddr2*/;
            if (string.IsNullOrEmpty(d.head.BillAddr1)) {
                lblAddr1.Text = d.head.CustAddr1 /*+ " " + d.head.CustAddr2*/;
            }
            lblMobile.Text = d.head.Mobile;
            lblEmail.Text = d.head.Email;
            lblDocumentDate.Text = d.head.INVDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            
            lblRC_ID.Text = d.head.INVID != null ? d.head.INVID : "";
            lblTaxId.Text = d.head.CustTaxID != null ? d.head.CustTaxID : "";
            foreach (var x in d.line) {
                x.DocTypeID = d.head.DocTypeID;
                if (x.DocTypeID == "OINV3") {//ขายสินค้า
                    x.TotalAmt = x.TotalAmtIncVat;
                }

                //if (x.RCType == "ORC2") {//ขายบริการ
                //    x.TotalAmt = x.TotalAmt;
                //} 

            }
            objectDataSource2.DataSource = d.line;

        }
    }
}
