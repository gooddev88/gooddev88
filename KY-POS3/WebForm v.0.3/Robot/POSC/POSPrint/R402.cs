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

namespace Robot.POSC.POSPrint {
    public partial class R402 : XtraReport {
        public R402() {
            InitializeComponent();
        }

        public void initData(string billId) {


            var doc = POSSaleService.GetDocSet(billId);
            var payment_cash = doc.Payment.Where(o => o.BillID == billId && o.PaymentType == "CASH").FirstOrDefault();
            var payment_transfer = doc.Payment.Where(o => o.BillID == billId && o.PaymentType == "TRANSFER").FirstOrDefault();

            var table = POSTableService.GetTable(doc.Head.TableID);
            var com = CompanyService.GetCompanyInfo(doc.Head.ComID);

            lbltable.Text = table == null ? doc.Head.TableID : table.TableName;
            doc.Head = CalHead(doc.Head);
            doc.Line = CalLine(doc.Line, com);
            //var addrCompo = new AddressComponent { AddrNo = com.AddrNo, SubDistrict = com.AddrTumbon, District = com.AddrAmphoe, Province = com.AddrProvince, Country = com.AddrCountry, Zipcode = com.AddrPostCode };
            //string comaddr = AddressInfoService.Convert_FullAddr(addrCompo);

            if (com != null) {
                var files = XFilesService.GetFileFromDocInfo(com.CompanyID, "COMPANY", "COMPANY_PHOTO_PROFILE");
                if (files != null) {
                    //logoimg.Image = XFilesService.ConvertImageByte2Image(files);
                    logoimg.ImageUrl = "~/Image/Logo/kylogo.png";
                } else {
                    logoimg.ImageUrl = "~/Image/Logo/kylogo.png";
                    //logoimg.Visible = false;
                }
            }

            lblcompanybig.Text = com.Name1;
            lblcompanyBranch.Text = com.Name2;
            lblCombigaddr.Text = com.BillAddr1 + " " + com.BillAddr2;
            lblCombigtax.Text = "เลขผู้เสียภาษี" + " " + com.TaxID + " " + " (" + com.BrnCode + ")";
            lbluserlogin.Text = LoginService.LoginInfo.CurrentUser;
            //lblTable.Text = "";
            // if (table!=null) {
            //     lblTable.Text = table.TableName;
            // }


            lbltoday.Text = doc.Head.BillDate.ToString("dd/MM/yyyy");
            lblinvid.Text = doc.Head.INVID;


            decimal netTotalIncVat = Math.Round(doc.Head.NetTotalAmtIncVat, 2, MidpointRounding.AwayFromZero);
            decimal round = Math.Round(GetDecimalPart(netTotalIncVat), 2, MidpointRounding.AwayFromZero);
            decimal AfterRound = netTotalIncVat - round;

            lblTotalAmt.Text = Math.Round(doc.Head.NetTotalAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
            lblTotalVatAmt.Text = "0.00";
            lblTotalVatAmt.Text = Math.Round(doc.Head.NetTotalVatAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
            lblTotalAmtIncVat.Text = netTotalIncVat.ToString("N2");

            lblRound.Text = round.ToString("n2");
            lblAfterRound.Text = AfterRound.ToString("n2");

            lblcash.Text = "0.00";
            lbltransfer.Text = "0.00";
            lblChange.Text = "0.00";
            if (payment_cash != null) {
                if (payment_cash.PaymentType == "CASH") {
                    lblcash.Text = Math.Round(payment_cash.PayAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");

                    lblChange.Text = Math.Round(payment_cash.ChangeAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
                }
            }
            if (payment_transfer != null) {
                if (payment_transfer.PaymentType == "TRANSFER") {
                    lbltransfer.Text = payment_transfer.PayAmt.ToString("N2");
                }
            }
            if (payment_cash != null && payment_transfer != null) {
                lblChange.Text = Math.Round(payment_cash.ChangeAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");
            }
            objectDataSource1.DataSource = doc.Line;

        }

        public static decimal GetDecimalPart(decimal number) {
            decimal result = (number - Math.Truncate(number));
            return result;
        }

        private POS_SaleHead CalHead(POS_SaleHead data) {
            if (data == null) {
                return new POS_SaleHead();
            }
            var comid = data.ComID;
            var comInfo = CompanyService.GetCompanyInfo(comid);
            if (comInfo.IsVatRegister == false) {//print แบบไม่มี vat
                lblTotalVatAmt.Visible = false;
                lblVatCaption.Visible = false;
                //lblVatInfoCaption.Visible = false;
                data.NetTotalAmt = data.NetTotalAmtIncVat;
            }

            return data;
        }

        private List<POS_SaleLine> CalLine(List<POS_SaleLine> data, CompanyInfo com) {
            if (data.Count() == 0) {
                return new List<POS_SaleLine>();
            }
           
                foreach (var d in data) {
                    if (com.PriceTaxCondType == "INC VAT") {
                        d.TotalAmt = d.TotalAmtIncVat;
                    } else {
                        d.TotalAmt = d.TotalAmt;
                    } 
                }
          
            return data;
        }

    }
}
