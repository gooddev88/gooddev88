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
using static PrintMaster.PrintFile.OMS.SO101.RunReport;

namespace PrintMaster.PrintFile.OMS.SO101 {
    public partial class SO101 : XtraReport {
     
        public SO101(SO101Set data, string copyno) { 
            InitializeComponent();
            LoadData(data,copyno);
        }

        private void LoadData(SO101Set d, string copyno) { 
            if (d.ComBrn == "") {
                lblBrnCode.Text = "";
            } else {
                int brnnumber = 0;
                if (int.TryParse(d.ComBrn, out brnnumber)) {//ระบุเป็นรหัสสาขา
                    lblBrnCode.Text = "สาขา " + d.Head.CustBrnID;
                } else {//ระบุเป็นสำนักงานใหญ่หรืออื่นๆ
                    lblBrnCode.Text = d.Head.CustBrnID;
                }
            }
            int row = 1;
            foreach (var l in d.Line) {
                l.LineNum = row;
                if (copyno=="0") {
                    l.ItemName = l.ItemName + Environment.NewLine + l.Remark1;
                }
       
                row++;
            }

            objectDataSource2.DataSource = d.Line;

            if (!string.IsNullOrEmpty(d.ComImage64)) {
                //logoimg.Image = ImageHelper.Base64ToImage(d.ComImage64);
                logoimg.ImageUrl = d.ComImage64;
            }


            lblComanyNameTH.Text = d.ComName;
            lblComanyAddr.Text = d.ComAddress + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + d.ComTax + " " + d.ComBrn;
            //pic_signature1.ImageUrl = d.SignnatureUrl1;


            lblCaptionInfo1.Text = "ใบสั่งขาย";
            lblCaptionInfo3.Text = "( SALE ORDER )";
            //if (copyno == "0") {
            lblCaptionInfo2.Text = "ใบสั่งขาย / SALE ORDER";
            //} else {
            //    lblCaptionInfo2.Text = "สำเนา / COPY";
            //}
            lblOnTopDIscount.Text = d.Head.OntopDiscAmt.ToString("n2");
            lblNetTotalAmt.Text = d.Head.NetTotalAmt.ToString("n2");
            lblBaseTotalAmtHead.Text = d.Head.BaseNetTotalAmt.ToString("N2");
            lblVatAmount.Text = d.Head.NetTotalVatAmt.ToString("N2");
            lblNetTotalAmtIncVat.Text = d.Head.NetTotalAmtIncVat.ToString("N2");

            lblRemark.Text = d.Head.Remark1;
            //lblapprovername.Text = d.Head.ApproverName;
            //lblQoDate.Text = d.Head.OrdDate.ToString("dd/MM/yyyy");
            //lblTel.Text = "โทร : " + user.Mobile;
            //lblemail.Text = "อีเมล : " + user.Email;

            lblThaiBath.Text = "(" + TextTHB.Process(d.Head.NetTotalAmtIncVat.ToString("N2")) + ")";

            lblCustomerID.Text = d.Head.CustName;

            lblAddr1.Text = d.Head.CustAddr1 /*+ Environment.NewLine + d.Head.CustAddr2*/;
            if (string.IsNullOrEmpty(d.Head.CustAddr1)) {
                lblAddr1.Text = /*d.Head.CustAddr1+" "+*/d.Head.CustAddr2;
                //lblAddr2.Text = header.BillAddr2;
            }


            lblDocumentDate.Text = d.Head.OrdDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            lblSO_ID.Text = d.Head.OrdID != null ? d.Head.OrdID : "";
            lblTaxId.Text = d.Head.CustTaxID != null ? d.Head.CustTaxID : "";

            int i = 0;
            foreach (var dd in d.Line.OrderBy(o => o.Sort)) {
                if (dd.ItemTypeID != "MISC") {
                    i++;
                    dd.RN = i;
                } else {
                    dd.RN = -99;
                }
            }
            objectDataSource2.DataSource = d.Line.OrderBy(o => o.Sort).ToList();
            
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


