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
    public partial class INVGA01 : DevExpress.XtraReports.UI.XtraReport {
        public INVGA01() {
            InitializeComponent();

        }
        public void initData(string id, string caption1, string caption2) {
            string myID = id;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                var header = db.OINVHead.Where(o => o.SOINVID == myID && o.RCompanyID==rcom).FirstOrDefault();
                var line = db.OINVLine.Where(o => o.SOINVID == myID && o.RCompanyID == rcom).ToList();

                var com = db.CompanyInfo.Where(o => o.CompanyID == header.CompanyID && o.RCompanyID == rcom).FirstOrDefault();
                var cust = db.CustomerInfo.Where(o => o.CustomerID == header.CustomerID && o.RCompanyID == rcom).FirstOrDefault();

                foreach (var l in line)
                {
                    l.ItemName = l.ItemName + Environment.NewLine + l.Remark1;
                }
                
                objectDataSource2.DataSource = line;
                //if (user != null) {
                //    lblCreatedBy.Text = user.FullName;
                //}

                if (com != null)
                {
                    var files = XFilesService.GetFileFromDocInfo(com.CompanyID, "COMPANY", "COMPANY_PHOTO_PROFILE");
                    if (files != null)
                    {
                        logoimg.Image = XFilesService.ConvertImageByte2Image(files);
                    }
                    

                    lblComanyNameTH.Text = com.Name1;
                    lblComanyAddr.Text = com.BillAddr1 + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + com.TaxID;
                }

                lblCaptionInfo1.Text = "ใบแจ้งหนี้ / ใบวางบิล";
                lblCaptionInfo3.Text = "(INVOICE)";

                //if (caption2 == "INVGA01")
                //{
                //    lblCaptionInfo1.Text = "ใบกำกับภาษี / ใบส่งของ / ใบแจ้งหนี้";
                //    lblCaptionInfo3.Text = "(TAX INVOICE / DELIVERY ORDER / DEBIT NOTE)";                    
                //}
                //else
                //{
                //    lblCaptionInfo1.Text = "ใบแจ้งหนี้ / ใบวางบิล";
                //    lblCaptionInfo3.Text = "(INVOICE)";
                //}

                if (caption1 == "copy")
                {
                    lblCaptionInfo2.Text = "สำเนา / COPY";
                }
                else
                {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL";
                }

                lblBaseTotalAmtHead.Text = header.BaseNetTotalAmt.ToString("N2");
                lblVatAmount.Text = header.NetTotalVatAmt.ToString("N2");
                lblNetTotalAmtIncVat.Text = header.NetTotalAmtIncVat.ToString("N2");

                //lblOntopDiscountCaption.Text = "ส่วนลด " + " (" + header.OntopDiscPer.ToString("N2") + "%) ";

                lblRemark.Text = header.Remark2;
                //lblPaymentMemo.Text = header.PaymentMemo;

                lblThaiBath.Text = header.NetTotalAmtIncVat == 0 ? "ศูนย์บาท" : "(" + TextTHB.Process(header.NetTotalAmtIncVat.ToString("N2")) + ")";

                lblCustomerID.Text = header.CustomerName;

                lblAddr1.Text = header.BillAddr1 + Environment.NewLine + header.BillAddr2;
                if (header.BillAddr1 == "") {
                    lblAddr1.Text = cust.AddrFull;
                    //lblAddr2.Text = header.BillAddr2;
                } 

                lblDocumentDate.Text = header.SODate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
                lblINV_ID.Text = header.SOINVID != null ? header.SOINVID : "";                               
                lblTaxId.Text = header.CustTaxID != null ? header.CustTaxID : "";
            }
        }
    }
}
