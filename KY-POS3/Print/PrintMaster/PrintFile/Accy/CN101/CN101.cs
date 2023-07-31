using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.CN101.RunReportCN;
using PrintMaster.Helper;

namespace PrintMaster.PrintFile.Accy.CN101
{
    public partial class CN101 : XtraReport {
        public CN101(CN101Set data, string UseFor, string typeprint, string copyno)
        {
            InitializeComponent();
            LoadData(data, UseFor, typeprint, copyno);
        }
        private void LoadData(CN101Set d, string UseFor, string typeprint, string copyno)
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

            lblCaptionInfo1.Text = "ใบลดหนี้ / ใบกำกับภาษี";
                lblCaptionInfo3.Text = "( CREDIT NOTE / TAX INVOICE )";

                if (copyno == "1")
                {
                    lblCaptionInfo2.Text = "สำเนา / COPY / " + UseFor;
                }
                else
                {
                    lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL / " + UseFor;
                }

                lblBaseTotalAmtHead.Text = d.head.BaseNetTotalAmt.ToString("N2");

                lblInvIdOld.Text = "";
                lblBaseTotalAmtHeadOld.Text = "0";
                lbldiffAmt.Text = "0";
                if (d.RefDocInv != null)
                {
                    lblInvIdOld.Text = "อ้างอิงจากเลข : " + d.RefDocInv.INVID;
                    lblBaseTotalAmtHeadOld.Text = d.RefDocInv.BaseNetTotalAmt.ToString("N2");
                    decimal diffamt = d.RefDocInv.BaseNetTotalAmt - d.head.BaseNetTotalAmt;
                    lbldiffAmt.Text = diffamt.ToString("N2");
                }

            pic_signature1.ImageUrl = d.SignnatureUrl1;
            pic_signature2.ImageUrl = d.SignnatureUrl1;

            lblVatAmount.Text = d.head.NetTotalVatAmt.ToString("N2");
                lblNetTotalAmtIncVat.Text = d.head.NetTotalAmtIncVat.ToString("N2");
                lblRemark.Text = d.head.Remark2;
                lblRemarkRC.Text = d.head.RemarkRC;               
                lblThaiBath.Text = "(" + TextTHB.Process(d.head.NetTotalAmtIncVat.ToString("N2")) + ")";
                lblCustomerID.Text = d.head.CustName;
                lblAddr1.Text = d.head.BillAddr1 + Environment.NewLine + d.head.BillAddr2;
                if (d.head.BillAddr1 == "") {
                    lblAddr1.Text = d.head.CustAddr1 + " " + d.head.CustAddr2;
            }

                if (d.head.POID!="") {
                    lblPO.Text = "เลขใบสั่งซื้อ " + d.head.POID;
                }
             
                lblDocumentDate.Text = d.head.INVDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
                lblINV_ID.Text = d.head.INVID != null ? d.head.INVID : "";
                lblTaxId.Text = d.head.CustTaxID != null ? d.head.CustTaxID : "";

                objectDataSource2.DataSource = d.line;

        }
    }
}
