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
    public partial class R412 : XtraReport {
        public R412() {
            InitializeComponent();
        }

        public void initData(string billId, string rcom) {

            using (GAEntities db = new GAEntities()) {
                var Head = db.POS_SaleHead.Where(o => o.BillID == billId && o.RComID == rcom).FirstOrDefault();
                var Line = db.POS_SaleLine.Where(o => o.BillID == billId && o.RComID == rcom).OrderBy(o => o.LineNum).ToList();
                var Payment = db.POS_SalePayment.Where(o => o.BillID == billId && o.RComID == rcom).ToList();
                var payment_cash = Payment.Where(o => o.BillID == billId && o.PaymentType == "CASH").FirstOrDefault();
                var payment_transfer = Payment.Where(o => o.BillID == billId && o.PaymentType == "TRANSFER").FirstOrDefault();
                var table = db.POS_Table.Where(o => o.RComID == rcom && o.TableID == Head.TableID).FirstOrDefault();
                var com = db.CompanyInfo.Where(o => o.CompanyID == Head.ComID && o.RCompanyID == rcom).FirstOrDefault();
                var userinfo = db.UserInfo.Where(o => o.Username == Head.CreatedBy).FirstOrDefault();

                Head = CalHead(Head);
                Line = CalLine(Line, Head.IsVatRegister);

                if (com != null) {
                    var files = XFilesService.GetFileFromDocInfo(com.CompanyID, "COMPANY", "COMPANY_PHOTO_PROFILE");
                    if (files != null) {
                        logoimg.Image = XFilesService.ConvertImageByte2Image(files);
                    } else {
                        logoimg.ImageUrl = "~/POSC/Image/dogx.png";
                        //logoimg.Visible = false;
                    }
                }
                lbltable.Text = table == null ? Head.TableID : table.TableName;
                lblDocID.Text = "บิล " + Head.BillID;
                lblcompanybig.Text = com.Name1;
                lblcompanyBranch.Text = com.Name2;
                lblCombigaddr.Text = com.BillAddr1 + " " + com.BillAddr2;
                lblCombigtax.Text = "เลขผู้เสียภาษี" + " " + com.TaxID + " " + " (" + com.BrnCode + ")";
                lbluserlogin.Text = userinfo == null ? "ไม่ทราบ" : userinfo.FullName;
                lbltoday.Text = Head.BillDate.ToString("dd/MM/yyyy");
                lblTotalAmt.Text = Math.Round(Head.NetTotalAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalVatAmt.Text = "0.00";
                lblTotalVatAmt.Text = Math.Round(Head.NetTotalVatAmt, 2, MidpointRounding.AwayFromZero).ToString("N2");
                lblTotalAmtIncVat.Text = Math.Round(Head.NetTotalAmtIncVat, 2, MidpointRounding.AwayFromZero).ToString("N2");
                lblRound.Text = Math.Round(Head.NetDiff, 2, MidpointRounding.AwayFromZero).ToString("N2");
                lblNetotalAfterRound.Text = Math.Round(Head.NetTotalAfterRound, 2, MidpointRounding.AwayFromZero).ToString("N2");
                objectDataSource1.DataSource = Line;
            }
        }
        private POS_SaleHead CalHead(POS_SaleHead data) {
            if (data == null) {
                return new POS_SaleHead();
            }
            if (data.IsVatRegister == false) {
                lblTotalVatAmt.Visible = false;
                lblVatCaption.Visible = false;
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
