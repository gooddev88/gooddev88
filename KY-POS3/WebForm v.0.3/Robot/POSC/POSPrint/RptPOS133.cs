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
using Robot.POSC.DA;
using Robot.Master.DA;

namespace Robot.POSC.Print
{
    public partial class RptPOS133 : XtraReport {
        public RptPOS133() {
            InitializeComponent();
        }

        public void initData(string comId, DateTime begin, DateTime end) {
 
            using (GAEntities db = new GAEntities()) {

                var com = CompanyService.GetCompanyInfo(comId);
                var line = SaleReportService.ReportPOS133(comId, begin, end); 

                lblcompany.Text = com.Name1;
                lblCompany2.Text = com.Name2;
                lbluserlogin.Text = LoginService.LoginInfo.CurrentUser;
                //DateTime today = DateTime.Now;
                lblBilldate.Text = begin.ToString("dd/MM/yyyy") + " " + "ถึง" + " " + end.ToString("dd/MM/yyyy");

                objectDataSource1.DataSource = line;
            }
        }

 
    }
}
