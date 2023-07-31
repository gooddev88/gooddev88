using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using static PrintMaster.PrintFile.Accy.PVWHT101.RunReport; 

namespace PrintMaster.PrintFile.Accy.PVWHT101 {
    public partial class PVWHT101 : DevExpress.XtraReports.UI.XtraReport {
        public PVWHT101(PVWHT101Set data) {
            InitializeComponent();
            LoadData(data);
        }

        private void LoadData(PVWHT101Set data) {
            vw_PVWHT info = data.PVWHTs.FirstOrDefault();
            if (info == null) {
                return;
            }
            lblWHTypeTopic.Text = info.WHTTypeName;
            lblComTaxID.Text = info.WHTTypeName;
            lblComCode.Text = info.ComCode;
            foreach (var d in data.PVWHTs) {
                d.VendName = $"<size=14> {d.VendName} </size><br>" +
        "<size=12>" +
        $"<b>ชื่อ</b>  {d.VendName} <br>" +
        $"<b>ที่อยู่</b> {d.VendName}  {d.VendName} <br>" +
        $"</size>";
            }

            objectDataSource1.DataSource = data.PVWHTs.OrderBy(o=>o.VendName).ToList();
        }
    }
}
