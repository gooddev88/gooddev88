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

namespace Robot.PrintServer.Rpt {
    public partial class R401 : XtraReport {
        public R401() {
            InitializeComponent();
        }

        public void initData(string billId, string rcom) {
            using (GAEntities db=new GAEntities()) {

            
            var Head = db.POS_SaleHead.Where(o => o.BillID == billId && o.RComID == rcom).FirstOrDefault();
            var Line = db.POS_SaleLine.Where(o => o.BillID == billId && o.RComID == rcom).OrderBy(o => o.LineNum).ToList();
            var Payment = db.POS_SalePayment.Where(o => o.BillID == billId && o.RComID == rcom).ToList();
            var payment_cash = Payment.Where(o => o.BillID == billId && o.PaymentType == "CASH").FirstOrDefault();
            var payment_transfer = Payment.Where(o => o.BillID == billId && o.PaymentType == "TRANSFER").FirstOrDefault();
            var table = db.POS_Table.Where(o => o.RComID == rcom && o.TableID == Head.TableID).FirstOrDefault();
            var com = db.CompanyInfo.Where(o => o.CompanyID == Head.ComID && o.RCompanyID == rcom).FirstOrDefault();
                var userinfo = db.UserInfo.Where(o => o.Username == Head.CreatedBy).FirstOrDefault();

                Head =CalHead(Head);
                Line= CalLine(Line,Head.IsVatRegister);
            

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

            lbltable.Text = table == null ? Head.TableID : table.TableName;
            lblcompanybig.Text = com.Name1;
                lblcompanyBranch.Text = com.Name2;
                lblCombigaddr.Text = com.BillAddr1 + " " + com.BillAddr2;
                lbluserlogin.Text = userinfo == null ? "ไม่ทราบ" : userinfo.FullName;

                lbltoday.Text = Head.BillDate.ToString("dd/MM/yyyy");
                lblinvid.Text = Head.INVID;

                decimal netTotalIncVat = Math.Round(Head.NetTotalAmtIncVat, 2, MidpointRounding.AwayFromZero);
                decimal round = Math.Round(GetDecimalPart(netTotalIncVat), 2, MidpointRounding.AwayFromZero);
                decimal AfterRound = netTotalIncVat - round;


                lblTotalAmt.Text = Math.Round(Head.NetTotalAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalAmtIncVat.Text = netTotalIncVat.ToString("N2");

                lblRound.Text = round.ToString("n2");
                lblAfterRound.Text = AfterRound.ToString("n2");
                 

                lblcash.Text = "0.00";
                lbltransfer.Text = "0.00";
                lblChange.Text = "0.00";
                if (payment_cash!=null) {
                    if (payment_cash.PaymentType == "CASH") {
                        // lblcash.Text = Math.Round(payment_cash.InvAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                        lblcash.Text = Math.Round(payment_cash.PayAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                        //payment_cash.PayAmt
                        //decimal balance = payment_cash.InvAmt - head.NetTotalAmtIncVat;
                        lblChange.Text = Math.Round(payment_cash.ChangeAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                    }
                }
                if (payment_transfer != null) {
                    if (payment_transfer.PaymentType == "TRANSFER") {
                        lbltransfer.Text = payment_transfer.PayAmt.ToString("N2");

                        //decimal balance = payment_transfer.InvAmt - head.NetTotalAmtIncVat;
                        //lblChange.Text = Math.Round(balance, 0, MidpointRounding.AwayFromZero).ToString("N2");
                    }                  
                }
                if (payment_cash !=null && payment_transfer !=null) { 
                    lblChange.Text = Math.Round(payment_cash.ChangeAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                } 
                objectDataSource1.DataSource = Line;
            }
        }


        public static decimal GetDecimalPart(decimal number) {
            decimal result = (number - Math.Truncate(number));
            return result;
        }

        private POS_SaleHead CalHead(POS_SaleHead data) {
            if (data == null) {
                return new POS_SaleHead();
            }
            if (data.IsVatRegister == false)
            {//print แบบไม่มี vat
             //lblVatInfoCaption.Visible = false;
                data.NetTotalAmt = data.NetTotalAmtIncVat;
                }
          
            return data;
        }

        private List<POS_SaleLine> CalLine(List<POS_SaleLine> data, bool? isvatRegister) {
            if (data.Count() == 0) {
                return new List<POS_SaleLine>();
            }
            if (isvatRegister == false) {//print แบบไม่มี vat
                foreach (var d in data) {
                        d.TotalAmt = d.TotalAmtIncVat;
                    }
                }
          
            return data;
        }

    }
}
