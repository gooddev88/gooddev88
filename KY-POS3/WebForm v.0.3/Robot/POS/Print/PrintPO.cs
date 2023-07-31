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
    public partial class PrintPO : XtraReport
    {

        public PrintPO()
        {
            InitializeComponent();
        }

        public void initData(string docid)
        {

            using (GAEntities db = new GAEntities())
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                DateTime date = DateTime.Now.Date;
                var line = db.vw_POS_POLine.Where(o => o.POID == docid && o.RComID == rcom).OrderBy(o => o.ItemID).ToList();
                var order_from = line.Where(o => o.ToLocID != "").FirstOrDefault();
                var head = db.POS_POHead.Where(o => o.POID == docid && o.RComID == rcom).FirstOrDefault();
                var com = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == head.ComID).FirstOrDefault();
                if (order_from!=null) {
                    if (!string.IsNullOrEmpty(order_from.ToLocID)) {
                          var get_actual_com = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.CompanyID == order_from.ToLocID).FirstOrDefault();
                        if (get_actual_com!=null) {
                            com = get_actual_com;
                        }
                    }
                }
                
                
                lblCompany.Text = com.Name1+" "+com.Name2;
                lblpoid.Text = head.POID;
                lblpodate.Text = head.PODate.ToString("dd/MM/yyyy");
                lblremark.Text = head.Remark1;

                objectDataSource1.DataSource = line;
            }
        }

    }
}
