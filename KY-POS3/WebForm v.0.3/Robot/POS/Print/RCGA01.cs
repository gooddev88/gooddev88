using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Robot.Data;
using System.Linq;
using Robot.Data.DataAccess;
using System.Collections.Generic;
using Robot.Helper;

namespace Robot.POS.Print {
    public partial class RCGA01 : DevExpress.XtraReports.UI.XtraReport {

       private class RCPrintLine {
            public string ItemID { get; set; }
            public string ItemName { get; set; }
            public string Doctype { get; set; }
            public decimal Qty { get; set; }
            public decimal TotalAmt { get; set; }
            public decimal TotalAmtIntVat { get; set; }
        }

        public RCGA01() {
            InitializeComponent();

        }
        public void initData(string docid, string com, string rcom,string UseFor, string caption1, string caption2, string rcid) {


            using (GAEntities db = new GAEntities()) {
                var header = db.ORCHead.Where(o => o.RCID == docid && o.CompanyID == com && o.RCompanyID == rcom).FirstOrDefault();
                var line = db.ORCLine.Where(o => o.RCID == docid && o.CompanyID == com && o.RCompanyID == rcom && o.IsActive).ToList();
                var payline = db.ORCPayLine.Where(o => o.RCID == docid && o.CompanyID == com && o.RCompanyID == rcom && o.IsActive).ToList();

                var comInfo = db.CompanyInfo.Where(o => o.CompanyID == header.CompanyID && o.RCompanyID == rcom && o.CompanyID == com).FirstOrDefault();
                var cust = db.CustomerInfo.Where(o => o.CustomerID == header.CustomerID && o.RCompanyID == rcom).FirstOrDefault();
                if (cust.BrnDesc == "") {
                    lblBrnCode.Text = "สำนักงานใหญ่";
                } else {
                    int brnnumber = 0;
                    if (int.TryParse(cust.BrnDesc, out brnnumber)) {//ระบุเป็นรหัสสาขา
                        lblBrnCode.Text = "สาขา " + cust.BrnDesc;
                    } else {//ระบุเป็นสำนักงานใหญ่หรืออื่นๆ
                        lblBrnCode.Text = cust.BrnDesc;
                    }
                }

                List<RCPrintLine> rcline = new List<RCPrintLine>();

                //foreach (var l in line) {
                //    RCPrintLine n = new RCPrintLine();
                //    n.ItemID = l.INVID;
                //    n.ItemName = l.INVID + Environment.NewLine + l.Remark1;
                //    n.Qty = 1;
                //    n.TotalAmt = l.InvTotalAmt;
                //    rcline.Add(n);
                //}

                if (line.Count() == 1) {//มี invoice 1 ใบใน rc นี้
                    var inv = line.Select(o => o.INVID).FirstOrDefault();
                    var invline = db.OINVLine.Where(o => o.SOINVID == inv && o.CompanyID == com && o.RCompanyID == rcom && o.IsActive).ToList();

                    foreach (var l in invline) {
                        RCPrintLine n = new RCPrintLine();
                        n.ItemID = l.ItemID;
                        n.ItemName = l.ItemName + " ( Inv no : " + l.SOINVID + " )" + Environment.NewLine + l.Remark1;
                        n.Doctype = l.DocTypeID;
                        n.Qty = l.QtyInvoice;
                        n.TotalAmt = l.TotalAmt;
                        n.TotalAmtIntVat = l.TotalAmtIncVat;
                        rcline.Add(n);
                    }

                } else {
                    foreach (var l in line) {
                        RCPrintLine n = new RCPrintLine();
                        n.ItemID = l.INVID;
                        n.ItemName = l.INVID + Environment.NewLine + l.Remark1;
                        n.Doctype = l.DocType;
                        n.Qty = 1;
                        n.TotalAmt = l.PayTotalAmt;
                        n.TotalAmtIntVat = l.PayTotalAmt;
                        rcline.Add(n);
                    }
                }


                objectDataSource2.DataSource = rcline;

                if (com != null) {
                    var files = XFilesService.GetFileFromDocInfo(com, "COMPANY_PHOTO_PROFILE" );
                    if (files != null) {
                        logoimg.Image = XFilesService.ConvertImageByte2Image(files);
                    }


                    lblComanyNameTH.Text = comInfo.Name1;
                    lblComanyAddr.Text = comInfo.BillAddr1 + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + comInfo.TaxID + " " + comInfo.BrnCode; 
                }


              
                if (payline.Count>0) {
                    foreach (var l in payline) {
                      //  lblWaitPayCaption.Visible = false;

                        if (l.PayBy == "CASH") {
                            lblMemoCash.Text = l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckCASH.Checked = true;
                        }
                        if (l.PayBy == "TRANSFER") {
                            lblMemoTRANSFER.Text = l.CustBankName + " " + "วันที่ " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckTRANSFER.Checked = true;
                        }
                        if (l.PayBy == "CHEQUE") {
                            lblMemoCHEQUE.Text = l.CustBankName + "  เลขที่  " + l.PaymentRefNo + "   ลงวันที่  " + Convert.ToDateTime(l.CompletedDate).ToString("dd/MM/yyyy");
                            ckCHEQUE.Checked = true;
                        }
                        if (l.PayBy == "OTHER") {
                            lblMemoOTHER.Text = l.PayAmt.ToString("N2") + " บาท " + l.PayMemo;
                            ckOTHER.Checked = true;
                        }
                    }
                } else {
                    lblMemoCash.Text = "........................................................................................................";
                    lblMemoTRANSFER.Text = "........................................................................................................";
                    lblMemoCHEQUE.Text = ".......................................................................................................";
                    lblMemoOTHER.Text = "......................................................................................................."; 
                }
                    
             
                 
                if (caption2 == "RCGA01") {//ใบเสร็จรับเงินขายสินค้า
                    lblCaptionInfo1.Text = "ใบเสร็จรับเงิน";
                    lblCaptionInfo3.Text = "Receipt";
                    GF_GROUP1.Visible = false;

                    if (caption1 == "copy")
                    {
                        lblCaptionInfo2.Text = "สำเนา / COPY";
                    }
                    else
                    {
                        lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL";
                    }
                }
                if(caption2 == "RCGA02"){//ใบเสร็จรับเงินงานบริการ
                    lblCaptionInfo1.Text = "ใบเสร็จรับเงิน / ใบกำกับภาษี";
                    lblCaptionInfo3.Text = "(Receipt / Tax invoice)";
                    GF_GROUP1.Visible = true;

                    if (caption1 == "copy")
                    {
                        lblCaptionInfo2.Text = "สำเนา / COPY / " + UseFor;
                    }
                    else
                    {
                        lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL / " + UseFor;
                    }
                }

                lblBeforeVat.Text = header.PayINVAmt.ToString("n2");
                lblVatAmt.Text = header.PayINVVatAmt.ToString("n2");

                var fl = line.FirstOrDefault();
                lblVatRate.Text = "VAT " + "0" + "%";
                if (fl != null) {
                    lblVatRate.Text ="VAT "+ fl.VatRate.ToString("n2")+"%";
                }
               

                //lblBaseTotalAmtHead.Text = header.TotalInvAmt.ToString("N2");
                //lblVatAmount.Text = header.TotalPayInvAmt.ToString("N2");
                lblNetTotalAmtIncVat.Text = header.PayINVTotalAmt.ToString("N2");

                lblRemark.Text = header.Remark2;
                //lblPaymentMemo.Text = header.PaymentMemo;

                lblThaiBath.Text = "(" + TextTHB.Process(header.PayINVTotalAmt.ToString("N2")) + ")";

                lblCustomerID.Text = cust.NameTh1;

                lblAddr1.Text = cust.BillAddr1 + Environment.NewLine + cust.BillAddr2;
                if (cust.BillAddr1 == "") {
                    lblAddr1.Text = cust.AddrFull;
                    //lblAddr2.Text = header.BillAddr2;
                }

                lblDocumentDate.Text = header.RCDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
                lblRC_ID.Text = header.RCID != null ? header.RCID : "";
                lblTaxId.Text = cust.TaxID != null ? cust.TaxID : "";
            }
        }
    }
}
