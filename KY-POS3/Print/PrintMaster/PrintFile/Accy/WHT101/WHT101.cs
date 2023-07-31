using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Collections.Generic;
using static PrintMaster.PrintFile.Accy.WHT101.RunReportWHT;
using PrintMaster.Helper;
using System.Globalization;

namespace PrintMaster.PrintFile.Accy.WHT101 {
    public partial class WHT101 : XtraReport {
        public WHT101(WHT101Set data,string docno)
        {
            InitializeComponent();
            LoadData(data, docno);
        }
        private void LoadData(WHT101Set d,string docno)
        {
             

              switch (docno) {
                case "1":
                    lblCopyNo.Text = "ฉบับที่ 1(สำหรับผู้ถูกหักภาษีหัก ณ ที่จ่ายใช้แนบพร้อมกับแบบแสดงรายการภาษี)";
                    break;
                case "2":
                    lblCopyNo.Text = "ฉบับที่ 2(สำหรับผู้ถูกหักภาษีหัก ณ ที่จ่ายเก็บไว้เป็นหลักฐาน)";
                    break;
                case "3":
                    lblCopyNo.Text = "ฉบับที่ 3(สำหรับผู้หักภาษี ณ ที่จ่าย ใช้แนบพร้อมกับแบบแสดงรายการ)";
                    break;
                case "4":
                    lblCopyNo.Text = "ฉบับที่ 4(สำหรับผุ้หักภาษี ณ ที่จ่าย เก็บไว้เป็นหลักฐาน)";
                    break;
                default:
                    break;
            }
          
            lblWHT_ID.Text = "เลขที่ " + d.head.WHTNo;
            lblCompanyFullName.Text = d.ComName;
            lblCompanyTaxID.Text = d.ComTax;
            lblComAddr.Text = d.ComAddress;
            lblWhtDate.Text = Convert.ToDateTime( d.head.WHTDate).ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
             
       
            lblCustTaxID.Text = d.head.VendTaxID;
            List<string> prefix = new List<string> { "นาย", "นาง", "น.ส." };
            if (prefix.Contains(d.head.VendTitle)) {
                d.head.VendName = d.head.VendTitle + " " + d.head.VendName;
            }
            lblCustFullName.Text = d.head.VendName;
            pic_signature1.ImageUrl = d.SignnatureUrl1;
            string addr_full = "";
            if (!string.IsNullOrEmpty(d.head.HouseNo)) {
                if (d.head.HouseNo.Replace(" ", "")!="") {
                    addr_full = addr_full + " เลขที่ " + d.head.HouseNo;
                } 
            }
          
            if (!string.IsNullOrEmpty(d.head.Village)) {
                if (d.head.Village.Replace(" ", "")!="") {
                    addr_full = addr_full + " หมู่บ้าน " + d.head.Village;
                }
              
            }
             if (!string.IsNullOrEmpty(d.head.Building)) {
                if (d.head.Building.Replace(" ", "") != "") {
                    addr_full = addr_full + " อาคาร " + d.head.Building;
                }
               
            }
            if (!string.IsNullOrEmpty(d.head.RoomNo)) {
                if (d.head.RoomNo.Replace(" ", "") != "") {
                    addr_full = addr_full + " ห้อง " + d.head.RoomNo;
                }
               
            }
            if (!string.IsNullOrEmpty(d.head.FloorNo)) {
                if (d.head.FloorNo.Replace(" ", "") != "") {
                    addr_full = addr_full + " ชั้น " + d.head.FloorNo;
                }
                
            }
            if (!string.IsNullOrEmpty(d.head.Soi)) {
                if (d.head.Soi.Replace(" ", "") != "") {
                    addr_full = addr_full + " ซอย " + d.head.Soi;
                }
                
            }
            if (!string.IsNullOrEmpty(d.head.Yaek)) {
                if (d.head.Yaek.Replace(" ", "") != "") {
                    addr_full = addr_full + " แยก " + d.head.Yaek;
                }
                
            }
        
            if (!string.IsNullOrEmpty(d.head.Road)) {
                if (d.head.Road.Replace(" ", "") != "") {
                    addr_full = addr_full + " ถนน " + d.head.Road;
                }
                
            }
           
             if (!string.IsNullOrEmpty(d.head.VendAddrMoo)) {
                if (d.head.VendAddrMoo.Replace(" ", "") != "") {
                    addr_full = addr_full + " หมู่ " + d.head.VendAddrMoo;
                }
                
            }
            if (d.head.VendAddrProvince== "กรุงเทพมหานคร") {
                if (d.head.VendAddrProvince.Replace(" ", "") != "") {
                    addr_full = addr_full + " แขวง" + d.head.VendAddrSubDistrict;
                    addr_full = addr_full + " เขต" + d.head.VendAddrDistrict;
                }
                
            } else {
        addr_full = addr_full + " ต." + d.head.VendAddrSubDistrict;
                addr_full = addr_full + " อ." + d.head.VendAddrDistrict;
            }
  addr_full = addr_full + " จ." + d.head.VendAddrProvince;
            if (!string.IsNullOrEmpty(d.head.Postcode)) {
                if (d.head.Postcode.Replace(" ", "") != "") {
                    addr_full = addr_full + " " + d.head.Postcode;
                }
               
            }
            lblCustAddr.Text = addr_full;






            switch (d.head.WHTTypeID) {
                case "1":
                    ck1.Checked = true;
                    break;
                case "2":
                    ck2.Checked = true;
                    break;
                case "3":
                    ck3.Checked = true;
                    break;
                case "4":
                    ck4.Checked = true;
                    break;
                case "5":
                    ck5.Checked = true;
                    break;
                case "6":
                    ck6.Checked = true;
                    break;
                case "7":
                    ck7.Checked = true;
                    break;
                default:
                    break;
            }

            switch (d.head.PayCondID) {
                case "1":
                    ckpay1.Checked = true;
                    break;
                case "2":
                    ckpay2.Checked = true;
                    break;
                case "3":
                    ckpay3.Checked = true;
                    break;
                case "4":
                    ckpay4.Checked = true;
                    break;
                default:
                    break;
            }

            //lblComanyNameTH.Text = d.ComName;
            //lblComanyAddr.Text = d.ComAddress + Environment.NewLine + "เลขประจำตัวผู้เสียภาษี " + d.ComTax + " " + d.ComBrn;

            //    lblNetTotalAmtIncVat.Text = Convert.ToDecimal(d.head.INVAmt).ToString("N2");
            //    lblRemark.Text = d.head.Memo;

            //lblThaiBath.Text = "(" + TextTHB.Process(Convert.ToDecimal(d.lineDisplay.Sum(o => o.PayAmt)).ToString("N2")) + ")";

            string[] strThaiNumber = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] strThaiPos = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            string strNumber = d.lineDisplay.Sum(o => o.PayAmt).ToString("0.00");
            string strInteger = strNumber.Split('.')[0];
            string strSatang = strNumber.Split('.')[1];

            string BahtText = "";

            if (strInteger == "0") {
                BahtText = "ศูนย์บาท";
            } else {
                int strLength = strInteger.Length;
                for (int i = 0; i < strInteger.Length; i++) {
                    string number = strInteger.Substring(i, 1);
                    if (number != "0") {
                        if (i == strLength - 1 && number == "1" && strLength != 1) {
                            BahtText += "เอ็ด";
                        } else if (i == strLength - 2 && number == "2" && strLength != 1) {
                            BahtText += "ยี่";
                        } else if (i != strLength - 2 || number != "1") {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "บาท";
            }

            if (strSatang == "00") {
                BahtText += "ถ้วน";
            } else {
                int strLength = strSatang.Length;
                for (int i = 0; i < strSatang.Length; i++) {
                    string number = strSatang.Substring(i, 1);
                    if (number != "0") {
                        if (i == strLength - 1 && number == "1" && strSatang[0].ToString() != "0") {
                            BahtText += "เอ็ด";
                        } else if (i == strLength - 2 && number == "2" && strSatang[0].ToString() != "0") {
                            BahtText += "ยี่";
                        } else if (i != strLength - 2 || number != "1") {
                            BahtText += strThaiNumber[int.Parse(number)];
                        }

                        BahtText += strThaiPos[(strLength - i) - 1];
                    }
                }

                BahtText += "สตางค์";
            }

            lblThaiBath.Text = BahtText;



            foreach (var l in d.lineDisplay) {
                var a = d.line.Where(o => o.WHTLineTypeID == l.ValueTXT).FirstOrDefault();
                if (a != null) {
                    //if (a.WHTLineTypeID=="6") {
                    //    l.Description1 = l.Description1 + " " + a.WHTRemark;
                    //}

                    if (a.WHTLineTypeID == "5" || a.WHTLineTypeID == "6") {
                        string Description1 = a.WHTDesc;
                        string modifiedDescription1 = Description1.Replace(".......................................", "");
                        Console.WriteLine(modifiedDescription1);
                        if (a.WHTRemark == "" || a.WHTRemark == null) {

                            l.Description1 = modifiedDescription1 + "  " + ".......................................";
                        } else {
                            l.Description1 = modifiedDescription1 + "  " + a.WHTRemark;
                        }
                    }
                    //if (a.WHTLineTypeID == "5") {
                        
                    //    if (a.WHTRemark == "") {
                         
                    //        l.Description1 = l.Description1 + "  " + ".......................................";
                    //    } else {
                    //        l.Description1 = l.Description1 + "  " + a.WHTRemark;
                    //    }
                    //}
                    l.WHTNo = a.WHTNo;
                    l.RComID = a.RCompanyID;
                    l.CustID = a.VendID;
                    l.CustName = a.VendName;
                    l.DocDate = a.PayDate.ToString("dd/MM/yyyy");
                    l.DocID = a.DocID;
                    l.TaxRate = a.TaxRate;
                    l.TaxBaseAmt = a.TaxBaseAmt;
                    l.PayAmt = a.PayAmt;
                }
            }



            //    lblCustomerID.Text = d.head.CustName;
            //    lblAddr1.Text = d.head.CustAddr1 + Environment.NewLine + d.head.CustAddr2;
            //    if (d.head.CustAddr1 == "") {
            //        lblAddr1.Text = d.CusAddrFull;
            //    }

            //    lblDocumentDate.Text = d.head.BillDate.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH"));
            //    lblBILL_ID.Text = d.head.BillNo != null ? d.head.BillNo : "";
            //    lblTaxId.Text = d.head.CustTaxID != null ? d.head.CustTaxID : "";
            foreach (var dd in d.lineDisplay) {
            
                if (string.IsNullOrEmpty(dd.DocDate)) {
                    dd.DocDate = "";
                } else {
                    var xdata = DateTime.ParseExact(dd.DocDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dd.DocDate = xdata.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
                }
            
            }
            objectDataSource1.DataSource = d.lineDisplay;
        }
    }
}
