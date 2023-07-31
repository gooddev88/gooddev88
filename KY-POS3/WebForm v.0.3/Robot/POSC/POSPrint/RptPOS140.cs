using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Robot.Data;
using System.Linq;
using Robot.Data.DataAccess;
using System.Collections.Generic;
using static Robot.Data.DataAccess.AddressInfoService;
using Robot.POSC.DA;

namespace Robot.POSC.POSPrint
{
    public partial class RptPOS140 : XtraReport
    {
        public RptPOS140()
        {
            InitializeComponent();
        }

        public void initData(string comId, DateTime begin, DateTime end)
        {
            using (GAEntities db = new GAEntities())
            {
          
                var line = SaleReportService.ListInvoice(comId, begin, end);
                foreach (var l in line) {
                    if (string.IsNullOrEmpty(l.CustomerName)) {
                        l.CustomerName = "ขายสินค้าหรือบริการ";
                    }
                }
                lblDateFrom.Text = begin.ToString("dd/MM/yyyy");
                lblDateTo.Text = end.ToString("dd/MM/yyyy");

             
                lblAddrCom.Text = "169/71 ถ.รัชดาภิเษก แขวงรัชดาภิเษก เขตดินแดง กรุงเทพมหานคร 10400";
                lblAddrCom1.Text = "เลขผู้เสียภาษี 0105562125888";

                objectDataSource1.DataSource = line;
            }
        }

    }
}
