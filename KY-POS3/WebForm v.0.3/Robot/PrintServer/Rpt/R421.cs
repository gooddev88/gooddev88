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

namespace Robot.PrintServer.Rpt
{
    public partial class R421 : XtraReport {
        public R421() {
            InitializeComponent();
        }

        public void initData(string billId, string rcom) {

            using (GAEntities db = new GAEntities())
            {
                var Head = db.POS_SaleHead.Where(o => o.BillID == billId && o.RComID == rcom).FirstOrDefault();
                var Line = db.POS_SaleLine.Where(o => o.BillID == billId && o.RComID == rcom).OrderBy(o => o.LineNum).ToList();
                var Payment = db.POS_SalePayment.Where(o => o.BillID == billId && o.RComID == rcom).ToList();
                var payment_cash = Payment.Where(o => o.BillID == billId && o.PaymentType == "CASH").FirstOrDefault();
                var payment_transfer = Payment.Where(o => o.BillID == billId && o.PaymentType == "TRANSFER").FirstOrDefault();
                var com = db.CompanyInfo.Where(o => o.CompanyID == Head.ComID && o.RCompanyID == rcom).FirstOrDefault();

                //var doc = POSSaleService.GetDocSet(billId);
                //var com = CompanyService.GetCompanyInfo(doc.Head.ComID);
                //var payment_cash = doc.Payment.Where(o => o.PaymentType == "CASH").FirstOrDefault();
                //var payment_tend = doc.Payment.Where(o => o.BillID == billId && o.PaymentType == "TRANSFER").FirstOrDefault();

                if (com != null)
                {
                    var files = XFilesService.GetFileFromDocInfo(com.CompanyID, "COMPANY", "COMPANY_PHOTO_PROFILE");
                    if (files != null)
                    {
                        logoimg.Image = XFilesService.ConvertImageByte2Image(files);
                    }
                    else
                    {
                        logoimg.ImageUrl = "~/Image/Logo/kylogo.png";
                        //logoimg.Visible = false;
                    }
                }
               // lbltable.Text = table == null ? Head.TableID : table.TableName;

                lblcompanybig.Text = com.Name1;
                lblcompanyBranch.Text = com.Name2;
                lblCombigaddr.Text = com.BillAddr1 + " " + com.BillAddr2;
                lblCombigtax.Text = "เลขผู้เสียภาษี" + " " + com.TaxID + " " + " (" + com.BrnCode + ")";
                lbluserlogin.Text = LoginService.LoginInfo.CurrentUser;

                lbltoday.Text = Head.BillDate.ToString("dd/MM/yyyy");
                lblisfull.Text = Head.FINVID;
                lblinvid.Text = Head.INVID;

                lblCustomerName.Text = Head.CustomerName;
                lblCustBranchName.Text = Head.CustBranchName;
                //  lblBillAddr.Text = doc.Head.CustAddr1 + " " + doc.Head.CustAddr2;
                lblCustAddr.Text = Head.CustAddr1 + " " + Head.CustAddr2;
                lblCustTaxID.Text = Head.CustTaxID;

                lblTotalAmt.Text = Math.Round(Head.NetTotalAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalVatAmt.Text = "0.00";
                lblTotalVatAmt.Text = Math.Round(Head.NetTotalVatAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalAmtIncVat.Text = Math.Round(Head.NetTotalAmtIncVat, 0, MidpointRounding.AwayFromZero).ToString("N2");

                lblcash.Text = "0.00";
                lbltransfer.Text = "0.00";
                lblChange.Text = "0.00";
                if (payment_cash != null)
                {
                    if (payment_cash.PaymentType == "CASH")
                    {
                        lblcash.Text = Math.Round(payment_cash.PayAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                        lblChange.Text = Math.Round(payment_cash.ChangeAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                    }
                }
                if (payment_transfer != null)
                {
                    if (payment_transfer.PaymentType == "TRANSFER")
                    {
                        lbltransfer.Text = Math.Round(payment_transfer.PayAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");

                    }
                }
                objectDataSource1.DataSource = Line;

            }
        }

 
    }
}
