using DevExpress.XtraReports.UI;
using PrintMaster.Helper;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using static PrintMaster.PrintFile.Accy.BL101.RunReportBL;
using static PrintMaster.PrintFile.Accy.FrontEnvelop.RunReportFrontEnvelopINV;

namespace PrintMaster.PrintFile.Accy.FrontEnvelop {
    public partial class EnvelopINV : DevExpress.XtraReports.UI.XtraReport {
        public EnvelopINV(FrontEnvelopINVSet data, string UseFor, string typeprint, string copyno) {
            InitializeComponent();
            LoadData(data, UseFor, typeprint, copyno);
        }

        private void LoadData(FrontEnvelopINVSet d, string UseFor, string typeprint, string copyno) {

            lblCustVenName.Text = d.head.CustName;
            if (d.head.BillAddr2 != "") { 
                lblAddr1.Text = d.head.BillAddr2 ; 
            } else {
                lblAddr1.Text = d.head.BillAddr1;
            }
            
            lblTel.Text = d.head.Mobile;
            //if (!string.IsNullOrEmpty(d.ComImage64)) {
            //    logoimgbox.ImageUrl = d.ComImage64;

            //}
            lblComanyNameTH.Text = d.ComName;

            lblComanyAddr.Text = d.ComAddress;
            lblComanyMobile.Text = d.ComTel;


        }
    }
}
