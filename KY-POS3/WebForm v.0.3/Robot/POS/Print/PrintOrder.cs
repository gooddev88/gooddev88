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
using System.Web;
using System.Collections.Generic;
using Robot.POSC.DA;
using Robot.Master.DA;
using Robot.POS.DA;

namespace Robot.POS.Print
{
    public partial class PrintOrder : XtraReport
    {
        public PrintOrder() {
            InitializeComponent();
        }

        public void initData(string OrdId) {

            var head = POSOrderService.GetOrderHeadByOrdID(OrdId);
            var line = POSOrderService.ListViewOrderLineByOrdID(head.OrdID);
            var com = CompanyService.GetCompanyInfo(head.ComID);
            var stkb = POSStockService.ListStkBalByComID(head.ComID);

            var ids = line.Select(o => o.ItemID).ToList();
            var item_stk = stkb.Where(o => !ids.Contains(o.ItemID) && o.BalQty != 0).ToList();
            foreach (var l in line) {
                l.FrLocID = "1.รายการสั่ง";
            }
            foreach (var l in item_stk)
            {
                vw_POS_ORDERLine newline = POSOrderService.NewLine();
                newline.ItemID = l.ItemID;
                newline.Name = l.ItemName;
                newline.BalQtyOrd = l.BalQty;
                newline.Unit = l.UnitID;
                newline.FrLocID = "2.สต๊อกคงเหลือ";
                line.Add(newline);
            }

            lblOrdid.Text = "เลขออเดอร์ : " + head.OrdID;
            lblcompany.Text = com.Name1 + " " + com.Name2;
            lblRemark.Text = head.Remark1;

            lbldatetoday.Text = head.OrdDate.ToString("dd/MM/yyyy");
            objectDataSource1.DataSource = line.OrderByDescending(o => o.VendorID).ThenBy(o => o.ItemID).ToList();

        }

    }
}
