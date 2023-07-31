using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Robot.Data;
using System.Linq;
 
using Robot.Data.DataAccess;
 
using System.Web;
using Robot.POSC.DA;
using Robot.Master.DA;

namespace Robot.POSC.POSPrint
{
    public partial class RptPOS134 : XtraReport {
        public RptPOS134() {
            InitializeComponent();
        }

        public void initData(string comId, DateTime begin, DateTime end) {
 
            using (GAEntities db = new GAEntities()) {

                var com = CompanyService.GetCompanyInfo(comId);
                var line = SaleReportService.ReportPOS134(comId, begin, end);

                lblcompany.Text = com.Name1 +" "+com.Name2;
                lbluserlogin.Text = LoginService.LoginInfo.CurrentUser;                
                lbltoday.Text = begin.ToString("dd/MM/yyyy") + " " + "ถึง :" + " " + end.ToString("dd/MM/yyyy");
                objectDataSource1.DataSource = line.OrderBy(o=>o.ItemName).ToList();
            }
        } 
    }
}
