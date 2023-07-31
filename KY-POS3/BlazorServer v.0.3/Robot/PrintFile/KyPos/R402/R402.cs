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

namespace Robot.PrintFile.KyPos.R402 {
    public partial class R402 : XtraReport {


        public I_POSSaleSetX x { get; set; }
        public R402(I_POSSaleSetX _x) {
            x = _x;
            InitializeComponent();
            initData();
        }


        public void initData() {
             
            var h = x.Head;

            
            var Line = x.Line.Where(o => o.BillID == h.BillID && o.RComID == h.RComID).OrderBy(o => o.LineNum).ToList();
                Line = Line.Where(o => o.IsLineActive == true && !(o.IsOntopItem && o.Price == 0)).ToList();
                var Payment = x.Payment.Where(o => o.BillID == h.BillID && o.RComID == h.RComID).ToList();
                var payment_cash = Payment.Where(o => o.BillID == h.BillID && o.PaymentType == "CASH").FirstOrDefault();
                var payment_transfer = Payment.Where(o => o.BillID == h.BillID && o.PaymentType == "TRANSFER").FirstOrDefault();
                
              
             
                x.Head = CalHead(x.Head);
                Line = CalLine(x.Line, h.IsVatRegister);



                var sumline = Line.GroupBy(x => new { x.ItemID, x.ItemName, x.PriceIncVat })
                           .Select(g =>
       new POS_SaleLineModel {
           ItemName = g.Key.ItemName,
           ItemID = g.Key.ItemID,
           PriceIncVat = g.Key.PriceIncVat,
           Qty = g.Sum(x => Math.Round(Convert.ToDecimal(x.Qty), 0)),
           TotalAmt = g.Sum(x => Math.Round(Convert.ToDecimal(x.TotalAmt), 2)),
           VatAmt = g.Sum(x => Math.Round(Convert.ToDecimal(x.VatAmt), 2)),
       } ).ToList();

            if (!string.IsNullOrEmpty(x.ComImage64)) {
                logoimg.ImageUrl = x.ComImage64;
            }

            lbltable.Text = x.Head.TableName;
            lblcompanybig.Text = x.ComName;
                lblcompanyBranch.Text = x.ComBrn;
            lblCombigaddr.Text = x.ComAddress;
                lblCombigtax.Text = "เลขผู้เสียภาษี" + " " + x.ComTax + " " + " (" + x.ComTax + ")";
                lbluserlogin.Text = "พนักงานขาย " + x.Username;
                lblMemberCode.Text = "Member code : " + h.CustomerID;

                lbltoday.Text = "วันที่ " + h.BillDate.ToString("dd/MM/yyyy");
                lblinvid.Text = h.INVID;


                decimal netTotalIncVat = Math.Round(h.NetTotalAmtIncVat, 2, MidpointRounding.AwayFromZero);
                decimal round = Math.Round(GetDecimalPart(netTotalIncVat), 2, MidpointRounding.AwayFromZero);
                decimal AfterRound = netTotalIncVat - round;

                lblTotalAmt.Text = Math.Round(h.NetTotalAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalVatAmt.Text = "0.00";
                lblTotalVatAmt.Text = Math.Round(h.NetTotalVatAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
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
                return  new POS_SaleHeadModelX();
            }


            if (x.IsVatRegister == false) {//print แบบไม่มี vat
                                                  //lblVatInfoCaption.Visible = false;
                data.NetTotalAmt = data.NetTotalAmtIncVat;
            }

            return data;
        }

        private List<POS_SaleLineModelX> CalLine(List<POS_SaleLineModelX> data, bool? isvatRegister) {
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
