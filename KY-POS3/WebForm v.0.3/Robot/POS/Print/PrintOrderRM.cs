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

namespace Robot.POS.Print
{
    public partial class PrintOrderRM : XtraReport
    {

        public PrintOrderRM()
        {
            InitializeComponent();
        }

        public void initData(DateTime begin,DateTime end)
        {

            using (GAEntities db = new GAEntities())
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                DateTime date = DateTime.Now.Date;

                var line = db.vw_POS_ORDER_RM.Where(o => o.OrdDate >= begin && o.OrdDate <= end && o.RComID==rcom).OrderBy(o => o.RmItemID).ToList();

                objectDataSource1.DataSource = line;
            }
        }

    }
}
