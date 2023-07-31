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

namespace Robot.POSC.POSPrint
{
    public partial class R411 : XtraReport
    {
        public R411() {
            InitializeComponent();
        }

        public void initData(string billId) {



            var doc = POSSaleService.GetDocSet(billId);

            var com = CompanyService.GetCompanyInfo(doc.Head.ComID);
            var table = POSTableService.GetTable(doc.Head.TableID);

            //var line = db.POS_SaleLine.Where(o => o.INVUnqHID == doc.Head.BillID).ToList();
            doc.Head = CalHead(doc.Head);
            doc.Line = CalLine(doc.Line);
            lblDocID.Text = "บิล " + doc.Head.BillID;

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
            // lblCombigtax.Text = "เลขผู้เสียภาษี" + " " + com.TaxID + " " + " (" + com.BrnCode + ")";
            lblBarCode.Text = com.QrPaymentData;
            lbluserlogin.Text = LoginService.LoginInfo.CurrentUser;

            lbltoday.Text = doc.Head.BillDate.ToString("dd/MM/yyyy");
            lbltable.Text = table == null ? doc.Head.TableID : table.TableName;

            lblTotalAmt.Text = Math.Round(doc.Head.NetTotalAmt, 0, MidpointRounding.AwayFromZero).ToString("N2");

            lblTotalAmtIncVat.Text = Math.Round(doc.Head.NetTotalAmtIncVat, 0, MidpointRounding.AwayFromZero).ToString("N2");

            lblRound.Text = Math.Round(doc.Head.NetDiff, 2, MidpointRounding.AwayFromZero).ToString("N2");
            lblNetotalAfterRound.Text = Math.Round(doc.Head.NetTotalAfterRound, 2, MidpointRounding.AwayFromZero).ToString("N2");

            objectDataSource1.DataSource = doc.Line;

        }
        private POS_SaleHead CalHead(POS_SaleHead data) {
            if (data == null) {
                return new POS_SaleHead();
            }
            var comid = data.ComID;
            var comInfo = CompanyService.GetCompanyInfo(comid);
            if (comInfo.IsVatRegister == false)    {//print แบบไม่มี vat

                data.NetTotalAmt = data.NetTotalAmtIncVat;
                }
           
            return data;
        }

        private List<POS_SaleLine> CalLine(List<POS_SaleLine> data) {
            if (data.Count() == 0) {
                return new List<POS_SaleLine>();
            }
            var comid = data.FirstOrDefault().ComID;
            var comInfo = CompanyService.GetCompanyInfo(comid);
            if (comInfo.IsVatRegister == false)  {//print แบบไม่มี vat
                foreach (var d in data) {
                        d.TotalAmt = d.TotalAmtIncVat;
                    }
                }
          
            return data;
        }

    }
}
