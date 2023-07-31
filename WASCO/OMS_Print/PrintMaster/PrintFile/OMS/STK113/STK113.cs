using System;
using System.Linq;
using DevExpress.XtraReports.UI;
using static PrintMaster.PrintFile.OMS.STK113.RunReport;

namespace PrintMaster.PrintFile.OMS.STK113
{
    public partial class STK113 : XtraReport {

        public STK113(STK113Set data) {
            InitializeComponent();
            LoadData(data);
        }

        private void LoadData(STK113Set d) {
            lblProDesc.Text = d.ProDesc;
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


