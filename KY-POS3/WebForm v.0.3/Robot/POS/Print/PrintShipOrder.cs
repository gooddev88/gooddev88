using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Robot.Data;
using System.Linq;
using Robot.Data.DataAccess;
using System.Collections.Generic;

namespace Robot.POS.Print {
    public partial class PrintShipOrder : XtraReport {
        public PrintShipOrder() {
            InitializeComponent();
        }

        public void initData(string docid) {
            using (GAEntities db = new GAEntities()) {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                var header = db.POS_ORDERHead.Where(o => o.OrdID == docid && o.RComID==rcom).FirstOrDefault();
                var line = db.vw_POS_ORDERLine.Where(o => o.OrdID == docid && o.RComID == rcom).OrderBy(o => o.ItemID).ToList();

                var com = db.CompanyInfo.Where(o => o.CompanyID == "INTER" && o.RCompanyID == rcom).FirstOrDefault();
                var cus = db.CompanyInfo.Where(o => o.CompanyID == header.ComID && o.RCompanyID == rcom).FirstOrDefault();
                //var cust = db.CustomerInfo.Where(o => o.CustomerID == header.CustID).FirstOrDefault();

                lblCustomerName1.Text = cus.Name1;
                lblCustomerName2.Text = cus.Name2;

                if (com != null)
                {
                    lblComanyNameTH.Text = com.Name1;
                    lblComanyAddr.Text = com.BillAddr1 + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + com.TaxID;
                }

                lblDocumentDate.Text = header.OrdDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
                lblORDID.Text = header.OrdID != null ? header.OrdID : "";

                objectDataSource1.DataSource = line.OrderByDescending(o => o.VendorID).ThenBy(o => o.ItemID).ToList();
            }
        }

        int counter = 0;
        private void lblRecordNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e) {
            counter++;
        }
    }
}
