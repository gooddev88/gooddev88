using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Robot.Data;
using System.Linq;
using static Robot.Data.DataAccess.AddressInfoService;
using Robot.Data.DataAccess;
using static Robot.Data.DataAccess.CurrencyInfoService;
using Robot.POS.BL;

namespace Robot.POS.Print
{
    public partial class PrintSumQtyItemInCompany : XtraReport
    {

        public PrintSumQtyItemInCompany()
        {
            InitializeComponent();
        }

        public void initData(DateTime begin,DateTime end,string type)
        {
            lblCaption.Text = "รายการเบิกสินค้า";
            if (type == "PURCHASEPRINT")
            {
                lblCaption.Text = "รายการสั่งสินค้า";
            }

            lbldatebegin.Text = Convert.ToDateTime(begin).ToString("dd/MM/yyyy");
            lbldateend.Text = Convert.ToDateTime(end).ToString("dd/MM/yyyy");
            var line = SumQtyItemInCompany.ListData(begin, end, type);

            objectDataSource1.DataSource = line.OrderBy(o => o.ItemID);
        }

    }
}
