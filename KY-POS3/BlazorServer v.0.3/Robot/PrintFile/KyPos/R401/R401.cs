using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Robot.Data;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Robot.Data.DA;
using static Robot.Data.DA.POSSY.POSService;
using Robot.Data.GADB.TT;
using static Robot.Data.DA.POSSY.POSSaleConverterService;
using Robot.Data.FILEDB.TT;
using static Robot.PrintOut.CreatePrintData.SalePrintConverter;

namespace Robot.PrintFile.KyPos.R401 {
    public partial class R401 : DevExpress.XtraReports.UI.XtraReport {
        
        public I_POSSaleSetX x { get; set; }
        public R401(I_POSSaleSetX _x) {
            x = _x;
            InitializeComponent();
            initData();
        }

        public void initData() {


            var h = x.Head;
            var payment_cash = x.Payment.Where(o => o.BillID == h.BillID && o.PaymentType == "CASH").FirstOrDefault();
            var payment_transfer = x.Payment.Where(o => o.BillID == h.BillID && o.PaymentType == "TRANSFER").FirstOrDefault();


            h = CalHead(x.Head);
            x.Line = x.Line.Where(o => o.IsLineActive == true && !(o.IsOntopItem && o.Price == 0)).ToList();
            x.Line = CalLine(x.Line);

            var sumline = x.Line.GroupBy(x => new { x.ItemID, x.ItemName, x.PriceIncVat })
        .Select(g =>
new POS_SaleLineModel {
    ItemName = g.Key.ItemName,
    ItemID = g.Key.ItemID,
    PriceIncVat = g.Key.PriceIncVat,
    Qty = g.Sum(x => Math.Round(Convert.ToDecimal(x.Qty), 0)),
    TotalAmt = g.Sum(x => Math.Round(Convert.ToDecimal(x.TotalAmt), 2)),
    VatAmt = g.Sum(x => Math.Round(Convert.ToDecimal(x.VatAmt), 2)),
}).ToList();


            if (!string.IsNullOrEmpty(x.ComImage64)) {
                logoimg.ImageUrl = x.ComImage64;
            }

            lbltable.Text = x.Head.TableName;
            lblcompanybig.Text = x.ComName;
            lblcompanyBranch.Text = x.ComBranch;
            lblCombigaddr.Text = x.ComAddress;

            lbluserlogin.Text = "พนักงานขาย " + x.Username;
            lblMemberCode.Text = "Member code : " + h.CustomerID;

            lbltoday.Text = "วันที่ " + h.BillDate.ToString("dd/MM/yyyy");
            lblinvid.Text = x.Head.INVID;

            decimal netTotalIncVat = Math.Round(x.Head.NetTotalAmtIncVat, 2, MidpointRounding.AwayFromZero);
            decimal round = Math.Round(GetDecimalPart(netTotalIncVat), 2, MidpointRounding.AwayFromZero);
            decimal AfterRound = netTotalIncVat - round;


            lblTotalAmt.Text = Math.Round(x.Head.NetTotalAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
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
            objectDataSource1.DataSource = sumline;

        }


        public static decimal GetDecimalPart(decimal number) {
            decimal result = (number - Math.Truncate(number));
            return result;
        }

        private POS_SaleHeadModelX CalHead(POS_SaleHeadModelX data) {
            if (data == null) {
                return new POS_SaleHeadModelX();
            }


            if (x.IsVatRegister == false) {//print แบบไม่มี vat
                                           //lblVatInfoCaption.Visible = false;
                data.NetTotalAmt = data.NetTotalAmtIncVat;
            }

            return data;
        }

        private List<POS_SaleLineModelX> CalLine(List<POS_SaleLineModelX> data) {
            if (data.Count() == 0) {
                return new List<POS_SaleLineModelX>();
            }
            var comid = data.FirstOrDefault().ComID;

            if (x.IsVatRegister == false) {//print แบบไม่มี vat
                foreach (var d in data) {
                    d.TotalAmt = d.TotalAmtIncVat;
                }
            }

            return data;
        }

    }
}
