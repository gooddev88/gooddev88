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

namespace Robot.POSC.POSPrint
{
    public partial class R421 : XtraReport {
        public R421() {
            InitializeComponent();
        }

        public void initData(string billId) {
 
        
         
                var doc = POSSaleService.GetDocSet(billId);
                var com = CompanyService.GetCompanyInfo(doc.Head.ComID); 
                var payment_cash = doc.Payment.Where(o =>  o.PaymentType == "CASH").FirstOrDefault();
                var payment_tend = doc.Payment.Where(o => o.BillID == billId && o.PaymentType == "TRANSFER").FirstOrDefault();

            if (com != null)
            {
                var files = XFilesService.GetFileFromDocInfo(com.CompanyID, "COMPANY", "COMPANY_PHOTO_PROFILE");
                if (files != null)
                {
                    logoimg.Image = XFilesService.ConvertImageByte2Image(files);
                }
                else
                {
                    logoimg.ImageUrl = "~/POSC/Image/dogx.png";
                    //logoimg.Visible = false;
                }
            }

            lblcompanybig.Text = com.Name1;
                lblcompanyBranch.Text = com.Name2;
                lblCombigaddr.Text = com.BillAddr1 + " " + com.BillAddr2;
                lblCombigtax.Text = "เลขผู้เสียภาษี" + " " + com.TaxID + " " + " (" + com.BrnCode + ")";
                lbluserlogin.Text = LoginService.LoginInfo.CurrentUser;
                
                lbltoday.Text = doc.Head.BillDate.ToString("dd/MM/yyyy");
                lblisfull.Text = doc.Head.FINVID;
                lblinvid.Text = doc.Head.INVID;

                lblCustomerName.Text = doc.Head.CustomerName;
                lblCustBranchName.Text = doc.Head.CustBranchName;
              //  lblBillAddr.Text = doc.Head.CustAddr1 + " " + doc.Head.CustAddr2;
            lblCustAddr.Text = doc.Head.CustAddr1 + " " + doc.Head.CustAddr2;
            lblCustTaxID.Text = doc.Head.CustTaxID;

                lblTotalAmt.Text = Math.Round(doc.Head.NetTotalAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalVatAmt.Text = "0.00";
                lblTotalVatAmt.Text = Math.Round(doc.Head.NetTotalVatAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalAmtIncVat.Text = Math.Round(doc.Head.NetTotalAmtIncVat, 0, MidpointRounding.AwayFromZero).ToString("N2");

                lblcash.Text = "0.00";
                lbltransfer.Text = "0.00";
                lblChange.Text = "0.00";
                if (payment_cash != null) {
                    if (payment_cash.PaymentType == "CASH") {
                        lblcash.Text = Math.Round(payment_cash.PayAmt, 0, MidpointRounding.AwayFromZero).ToString("N2"); 
                        lblChange.Text = Math.Round(payment_cash.ChangeAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                    }
                }
                if (payment_tend != null) {
                    if (payment_tend.PaymentType == "TRANSFER") {
                        lbltransfer.Text = Math.Round(payment_tend.PayAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
 
                    }

      


  
            }
            objectDataSource1.DataSource = doc.Line;
        }

 
    }
}
