using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.TAX101.RunReportTAX;
using PrintMaster.Helper;
using System;

namespace PrintMaster.PrintFile.Accy.TAX101
{
    public partial class TAX101 : XtraReport {
        public TAX101(TAX101Set data)
        {
            InitializeComponent();
            LoadData(data);
        }
        private void LoadData(TAX101Set d)
        {

               
            lblDescInfo.Text = d.lblDescInfo;
              
            lblComanyNameTH.Text = d.ComName;
            lblComanyAddr.Text = d.ComAddress + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + d.ComTax + " " + d.ComBrn;

            lblDocumentDate.Text = d.head.CreatedDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
                lblTaxSubmitID.Text = d.head.TaxSubmitID != null ?d.head.TaxSubmitID : "";
                lblTaxSeries.Text = d.head.TaxSeries.ToString("N0");

            objectDataSource2.DataSource = d.line.OrderBy(o => o.TaxDate).ToList();
        }
    }
}
