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
    public partial class PrintStockBalance : XtraReport
    {
        public static List<vw_POS_STKBal> DocList { get { return (List<vw_POS_STKBal>)HttpContext.Current.Session["stkbalance_list"]; } set { HttpContext.Current.Session["stkbalance_list"] = value; } }
        public PrintStockBalance() {
            InitializeComponent();
        }

        public void initData() {

            var line = DocList;
            var com = CompanyService.GetCompanyInfo(POSStockService.FilterSet.Company);

            if (POSStockService.FilterSet.Location == "")
            {
                lblLocID.Text = "ที่เก็บ : ไม่ระบุที่เก็บ";
            }
            else
            {
                lblLocID.Text = "ที่เก็บ : " + POSStockService.FilterSet.Location;
            }

            var myline = from obj in line
                              group obj by new { obj.ItemName, obj.TypeID } into g
                              select new vw_POS_STKBal {
                                  ItemName = g.Key.ItemName,
                                  TypeID = g.Key.TypeID,
                                  OrdQty = g.Sum(x => x.OrdQty),
                                  InstQty = g.Sum(x => x.InstQty),
                                  BalQty = g.Sum(x => x.BalQty),
                              };
            
            lblcompany.Text = com.Name1 + " " + com.Name2;
            lbldatetoday.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            objectDataSource1.DataSource = myline.OrderBy(x => x.ItemID);

        }

    }
}
