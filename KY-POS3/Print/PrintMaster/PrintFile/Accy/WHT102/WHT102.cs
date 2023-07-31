using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic; 
using PrintMaster.Helper;
using System.Globalization;
using DevExpress.ClipboardSource.SpreadsheetML;
using static PrintMaster.PrintFile.Accy.WHT102.RunReportWHT102;

namespace PrintMaster.PrintFile.Accy.WHT102 {
    public partial class WHT102 : XtraReport {
        public WHT102(WHT102Set data)
        {
            InitializeComponent();
            LoadData(data);
        }
        private void LoadData(WHT102Set d )
        {
            if (d.Header.FormID=="pnd53") {
                lblTitle.Text = "ใบแนบ ภ.ง.ด. 53";
            }

      
          
            
      
             objectDataSource2.DataSource = d.pnds;
        }
    }
}
