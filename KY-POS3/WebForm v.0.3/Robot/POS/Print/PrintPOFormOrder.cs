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
    public partial class PrintPOFormOrder : XtraReport
    {

        public PrintPOFormOrder()
        {
            InitializeComponent();
        }

        public void initData(DateTime begin,DateTime end,string type,string vendor)
        {
            lblCaption.Text = "สรุปรายการซื้อวัตถุดิบ";

            lbldatebegin.Text = Convert.ToDateTime(begin).ToString("dd/MM/yyyy");
            lbldateend.Text = Convert.ToDateTime(end).ToString("dd/MM/yyyy");

            var line = SumQtyItemInCompany.ListData(begin, end, type).Where
                                        (o => (o.VendorID == vendor || vendor == "") 
                                        && o.ALLCOMPANY_OrdQty > 0).OrderBy(o => o.VendorName).ToList();

            objectDataSource1.DataSource = line;
        }

    }
}
