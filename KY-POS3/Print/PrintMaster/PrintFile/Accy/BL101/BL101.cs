using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.BL101.RunReportBL;
using PrintMaster.Helper;

namespace PrintMaster.PrintFile.Accy.BL101
{
    public partial class BL101 : XtraReport {
        public BL101(BL101Set data, string UseFor, string typeprint, string copyno)
        {
            InitializeComponent();
            LoadData(data, UseFor, typeprint, copyno);
        }
        private void LoadData(BL101Set d, string UseFor, string typeprint, string copyno)
        {

            if (d.ComBrn == "")
            {
                lblBrnCode.Text = "";
            }
            else
            {
                int brnnumber = 0;
                if (int.TryParse(d.ComBrn, out brnnumber))
                {//ระบุเป็นรหัสสาขา
                    lblBrnCode.Text = "สาขา " + d.ComBrn;
                }
                else
                {//ระบุเป็นสำนักงานใหญ่หรืออื่นๆ
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

            lblCaptionInfo1.Text = "ใบแจ้งหนี้ / ใบวางบิล";
                lblCaptionInfo3.Text = "( BILLING NOTE )";

                if (copyno == "1")
                {
                    lblCaptionInfo2.Text = "สำเนา / COPY";
                }
                else
                {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL";
                }

                lblNetTotalAmtIncVat.Text = Convert.ToDecimal(d.head.INVAmt).ToString("N2");
                lblRemark.Text = d.head.Memo;

                lblThaiBath.Text = "(" + TextTHB.Process(Convert.ToDecimal(d.head.INVAmt).ToString("N2")) + ")";

                lblCustomerID.Text = d.head.CustName;
                lblAddr1.Text = d.head.CustAddr1 + Environment.NewLine + d.head.CustAddr2;
                if (d.head.CustAddr1 == "") {
                    lblAddr1.Text = d.CusAddrFull;
                }

            pic_signature1.ImageUrl = d.SignnatureUrl1;
           

            lblDocumentDate.Text = d.head.BillDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
                lblBILL_ID.Text = d.head.BillNo != null ? d.head.BillNo : "";
                lblTaxId.Text = d.head.CustTaxID != null ? d.head.CustTaxID : "";
            objectDataSource1.DataSource = d.line;
        }
    }
}
