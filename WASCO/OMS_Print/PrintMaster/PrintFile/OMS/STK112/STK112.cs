using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using static PrintMaster.PrintFile.OMS.STK112.RunReport;

namespace PrintMaster.PrintFile.OMS.STK112 {
    public partial class STK112 : XtraReport {

        public STK112(STK112Set data) {
            InitializeComponent();
            LoadData(data);
        }

        private void LoadData(STK112Set d) {
            lblBrand.Text = d.Brand;
            objectDataSource1.DataSource = d.rows.OrderBy(o => o.ItemID).ToList();
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
        }
    }


}


