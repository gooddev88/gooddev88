using System;
using DevExpress.XtraReports.UI;
using System.Linq;
using PrintMaster.Helper;
using static PrintMaster.PrintFile.OMS.SO102.RunReport;

namespace PrintMaster.PrintFile.OMS.SO102 {
    public partial class SO102 : XtraReport {
     
        public SO102(SO102Set data, string copyno) { 
            InitializeComponent();
            LoadData(data,copyno);
        }

        private void LoadData(SO102Set d, string copyno) {
            //logoimg.ImageUrl = "Image\\wasco_logo.png";
            //if (d.ComBrn == "") {
            //    lblBrnCode.Text = "";
            //} else {
            //    int brnnumber = 0;
            //    if (int.TryParse(d.ComBrn, out brnnumber)) {//ระบุเป็นรหัสสาขา
            //        lblBrnCode.Text = "สาขา " + d.Head.CustBrnID;
            //    } else {//ระบุเป็นสำนักงานใหญ่หรืออื่นๆ
            //        lblBrnCode.Text = d.Head.CustBrnID;
            //    }
            //}
            int row = 1;
            foreach (var l in d.Line) {
                l.LineNum = row;
                if (copyno=="0") {
                    l.ItemName = l.ItemName + Environment.NewLine + l.Remark1;
                }
       
                row++;
            }

            objectDataSource2.DataSource = d.Line.Where(o=>o.Status!= "REJECT").ToList();

            //if (!string.IsNullOrEmpty(d.ComImage64)) {
            //    //logoimg.Image = ImageHelper.Base64ToImage(d.ComImage64);
            //    logoimg.ImageUrl = d.ComImage64;
            //}
            lblItemTotal.Text = d.Head.Qty.ToString("N0");
            lblRemark.Text = d.Head.Remark1;
            //lblComanyNameTH.Text = d.ComName;
            //lblComanyAddr.Text = d.ComAddress + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + d.ComTax + " " + d.ComBrn;
            //pic_signature1.ImageUrl = d.SignnatureUrl1;


            //lblCaptionInfo1.Text = "ใบสั่งขาย";
            //lblCaptionInfo3.Text = "( SALE ORDER )";
            ////if (copyno == "0") {
            //lblCaptionInfo2.Text = "ใบสั่งขาย / SALE ORDER";
            //} else {
            //    lblCaptionInfo2.Text = "สำเนา / COPY";
            //}
            //lblItemDiscount.Text = d.Head.ItemDiscAmtIncVat.ToString("n2");

            //lblAfterDiscountIncVat.Text = d.Head.NetTotalAmtIncVat.ToString("n2");
            //lblNetTotalAmt.Text = d.Head.NetTotalAmt.ToString("n2");
            //lblItemTotal.Text = d.Head.Qty.ToString("N2");
            //lblVatAmount.Text = d.Head.NetTotalVatAmt.ToString("N2");
            //lblNetTotalAmtIncVat.Text = d.Head.NetTotalAmtIncVat.ToString("N2");

            //lblRemark.Text = d.Head.Remark1;
            //lblapprovername.Text = d.Head.ApproverName;
            //lblQoDate.Text = d.Head.OrdDate.ToString("dd/MM/yyyy");
            //lblTel.Text = "โทร : " + user.Mobile;
            //lblemail.Text = "อีเมล : " + user.Email;

            //lblThaiBath.Text = "(" + TextTHB.Process(d.Head.NetTotalAmtIncVat.ToString("N2")) + ")";

            lblCustomerID.Text = d.Head.CustName + " (" + d.Head.CustID + ")";

            lblAddr1.Text = d.CustAddress;
            //if (d.Head.CustAddr1 == "") {
            //    lblAddr1.Text = d.Head.CustAddr1+" "+d.Head.CustAddr2;
            //    //lblAddr2.Text = header.BillAddr2;
            //}


            //lblDocumentDate.Text = d.Head.OrdDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            //lblSO_ID.Text = d.Head.OrdID != null ? d.Head.OrdID : "";
            //lblTaxId.Text = d.CustTax != null ? d.CustTax : "";

            int i = 0;
            foreach (var dd in d.Line.OrderBy(o => o.LineNum)) {
                if (dd.ItemTypeID != "MISC") {
                    i++;
                    dd.RN = i;
                } else {
                    dd.RN = -99;
                }
            }
            objectDataSource2.DataSource = d.Line.OrderBy(o => o.LineNum).ToList();
            
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


