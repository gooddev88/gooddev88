﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

using System.Linq;
using System.Web;
using System.Collections.Generic;
using PrintMaster.Helper;
using static PrintMaster.PrintFile.KyPos.R401X.Model;

namespace Robot.PrintFile.KyPos.R40X {
    public partial class R401 : DevExpress.XtraReports.UI.XtraReport {

        public I_POSSaleSetX x { get; set; }
        public R401(I_POSSaleSetX _x) {
            x = _x;
            InitializeComponent();
            initData();
        }

        public void initData() {
            decimal disc = 0; 
            var h = x.Head;
            x.Payment = x.Payment.Where(o => o.IsLineActive == true && o.IsActive == true).ToList();
            List<string> payTransfer = new List<string> { "ONLINE", "TRANSFER" };
            var payment_voucher = x.Payment.Where(o => o.BillID == h.BillID && o.PaymentType == "VOUCHER" && o.IsActive == true).Sum(o => o.PayAmt);
            var payment_credit = x.Payment.Where(o => o.BillID == h.BillID && o.PaymentType == "CREDIT" && o.IsActive == true).Sum(o => o.PayAmt);
            var payment_cash = x.Payment.Where(o => o.BillID == h.BillID && o.PaymentType == "CASH" && o.IsActive == true).FirstOrDefault();
            var payment_transfer = x.Payment.Where(o => o.BillID == h.BillID && payTransfer.Contains( o.PaymentType )  && o.IsActive == true).FirstOrDefault();
            
            h = CalHead(x.Head);
            x.Line = x.Line.Where(o => o.IsActive == true && o.IsLineActive == true && o.Status != "K-REJECT").ToList();
            x.Line = x.Line.Where(o => o.IsLineActive == true && !(o.IsOntopItem && o.Price == 0) && o.IsActive == true).ToList();
        
            x.Line = CalLine(x.Line);
            var discountLine = x.Line.Where(o => o.ItemTypeID == "DISCOUNT" && o.IsActive == true).FirstOrDefault();
            if (discountLine != null) {
                disc = (Math.Round(discountLine.TotalAmtIncVat, 2, MidpointRounding.AwayFromZero)*-1);
                if (discountLine.DiscCalBy=="P") {
                    lblDiscountBy.Text = $"ส่วนลด {discountLine.DiscPer.ToString("n0")} %";
                } else {
                    lblDiscountBy.Text = $"ส่วนลด";
                }
                lblDiscount.Text = disc.ToString("n2");
            } else {
                lblDiscountBy.Text = "ส่วนลด";
                lblDiscount.Text = "0.00";
            }
            var sumline = x.Line.Where(o=> o.ItemTypeID != "DISCOUNT" && o.IsActive==true).GroupBy(x => new { x.ItemID, x.ItemName, x.PriceIncVat })
        .Select(g =>
            new POS_SaleLineModelX {
                ItemName = g.Key.ItemName,
                ItemID = g.Key.ItemID,
                PriceIncVat = g.Key.PriceIncVat,
                Qty = g.Sum(x => Math.Round(Convert.ToDecimal(x.Qty), 0)),
                TotalAmt = g.Sum(x => Math.Round(Convert.ToDecimal(x.TotalAmt), 2)),
                VatAmt = g.Sum(x => Math.Round(Convert.ToDecimal(x.VatAmt), 2)),
            }).ToList();
            string rooturl = URLHelper.GetBaseUrlNoAppPath();

            if (!string.IsNullOrEmpty(x.ComImage64)) {
                logoimg.ImageUrl = x.ComImage64;
            } else {
                logoimg.ImageUrl = rooturl + "/Image/kylogo.png";
            }

            lbltable.Text = x.Head.TableName;
            lblcompanybig.Text = x.ComName;
            lblcompanyBranch.Text = "สาขา " + x.ComBrn;
            lblCombigaddr.Text = x.ComAddress;

            lbluserlogin.Text = x.Head.CreatedBy;
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
            lblPayCredit.Text = payment_credit.ToString("n2");
            lblPayVoucher.Text = payment_voucher.ToString("n2");
            if (payment_cash != null) {
                if (payment_cash.PaymentType == "CASH") {
                    lblcash.Text = Math.Round(payment_cash.GetAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");

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