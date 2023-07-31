using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.QO101.RunReport;
using PrintMaster.Helper;
using DevExpress.DataAccess.Sql;

namespace PrintMaster.PrintFile.Accy.QO101 {
    public partial class QO101 : XtraReport {
     
        public QO101(QO101Set data, string copyno) { 
            InitializeComponent();
            LoadData(data,copyno);
        }

        private void LoadData(QO101Set d, string copyno) { 
            if (d.ComBrn == "") {
                lblBrnCode.Text = "";
            } else {
                int brnnumber = 0;
                if (int.TryParse(d.ComBrn, out brnnumber)) {//ระบุเป็นรหัสสาขา
                    lblBrnCode.Text = "สาขา " + d.head.CustBrnID;
                } else {//ระบุเป็นสำนักงานใหญ่หรืออื่นๆ
                    lblBrnCode.Text = d.head.CustBrnID;
                }
            }
            int row = 1;
            foreach (var l in d.line) {
                l.LineNum = row;
                if (copyno=="0") {
                    l.ItemName = l.ItemName + Environment.NewLine + l.Remark1;
                }
       
                row++;
            }

            objectDataSource2.DataSource = d.line;

            if (!string.IsNullOrEmpty(d.ComImage64)) {
                //logoimg.Image = ImageHelper.Base64ToImage(d.ComImage64);
                logoimg.ImageUrl = d.ComImage64;
            }


            lblComanyNameTH.Text = d.ComName;
            lblComanyAddr.Text = d.ComAddress + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + d.ComTax + " " + d.ComBrn;
            pic_signature1.ImageUrl = d.SignnatureUrl1;


            lblCaptionInfo1.Text = "ใบเสนอราคา";
            lblCaptionInfo3.Text = "( Quotation )";
            if (copyno == "0") {
                lblCaptionInfo2.Text = "ต้นฉบับ / ORIGINAL";
            } else {
                lblCaptionInfo2.Text = "สำเนา / COPY";
            }
            lblOnTopDIscount.Text = d.head.OntopDiscAmt.ToString("n2");
            lblNetTotalAmt.Text = d.head.NetTotalAmt.ToString("n2");
            lblBaseTotalAmtHead.Text = d.head.BaseNetTotalAmt.ToString("N2");
            lblVatAmount.Text = d.head.NetTotalVatAmt.ToString("N2");
            lblNetTotalAmtIncVat.Text = d.head.NetTotalAmtIncVat.ToString("N2");

            lblRemark.Text = d.head.Remark1;
            lblapprovername.Text = d.head.ApproverName;
            lblQoDate.Text = d.head.QODate.ToString("dd/MM/yyyy");
            //lblTel.Text = "โทร : " + user.Mobile;
            //lblemail.Text = "อีเมล : " + user.Email;

            lblThaiBath.Text = "(" + TextTHB.Process(d.head.NetTotalAmtIncVat.ToString("N2")) + ")";

            lblCustomerID.Text = d.head.CustomerName;

            lblAddr1.Text = d.head.CustomerAddr1;
            if (string.IsNullOrEmpty(d.head.CustomerAddr1)) {
                lblAddr1.Text = d.head.CustomerAddr2;
                //lblAddr2.Text = header.BillAddr2;
            }


            lblDocumentDate.Text = d.head.QODate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            lblQUO_ID.Text = d.head.QOID != null ? d.head.QOID : "";
            lblTaxId.Text = d.head.CustTaxID != null ? d.head.CustTaxID : "";

            int i = 0;
            foreach (var dd in d.line.OrderBy(o => o.Sort)) {
                if (dd.ItemTypeID != "MISC") {
                    i++;
                    dd.RN = i;
                } else {
                    dd.RN = -99;
                }
            }
            objectDataSource2.DataSource = d.line.OrderBy(o => o.Sort).ToList();
            
        }
      
        int RecordCount = 0;
        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
        var isPageBreak = Convert.ToBoolean(GetCurrentColumnValue("PageBreak"));
        RecordCount++;
        if (isPageBreak) {
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
    }


}


