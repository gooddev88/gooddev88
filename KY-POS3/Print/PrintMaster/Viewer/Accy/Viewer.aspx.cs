using DevExpress.XtraPrinting;
using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using PrintMaster.PrintFile.Accy;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrintMaster.Viewer.Accy {
    public partial class Viewer : System.Web.UI.Page {
        //for accy
        protected void Page_Load(object sender, EventArgs e) {
            Report1 report1 = new Report1();  
            OpenReport( Page.Request.QueryString["id"]); 
        }
        private void OpenReport(string printid) {
            if (string.IsNullOrEmpty(printid)) {
                return;
            }
            var print_record=  GetPrintRecord(printid);
            string report_name = "";
            switch (print_record.FormPrintID) {
                case "QO101"://qoutation 
                    
                    var qo101 = PrintFile.Accy.QO101.RunReport.OpenReport(print_record, out report_name);
                  //  qo101.DisplayName= report_name;
                
                    //PrintingSystemBase printingSystem1 = qo101.PrintingSystem;

                    //// Obtain the current export options.
                    //ExportOptions options = printingSystem1.ExportOptions;

                    //// Set Print Preview options.
                    //options.PrintPreview.ActionAfterExport = ActionAfterExport.AskUser;
                    ////options.PrintPreview.DefaultDirectory = "C:\\Temp";
                    //options.PrintPreview.DefaultFileName = "Report";
                    //options.PrintPreview.SaveMode = SaveMode.UsingDefaultPath;
                    //options.PrintPreview.ShowOptionsBeforeExport = false;
                    
                    docviewer.OpenReport(qo101);
                    break;
                case "INV101"://พิมพ์ใบกำกับภาษี 
                case "INV101_1"://พิมพ์ใบกำกับภาษี แบบขายสด
                    docviewer.OpenReport(PrintFile.Accy.INV101.RunReportINV.OpenReport(print_record, print_record.FormPrintID));
                    break;
                case "INV102"://พิมพ์อินวอยซ์/บริการ 
                    docviewer.OpenReport(PrintFile.Accy.INV101.RunReportINV.OpenReport(print_record,"INV102"));
                    break;
                case "RC101_1"://พิมพ์ใบเสร็จรับเงินขายสินค้าจากหน้าอินวอยซ์
                    docviewer.OpenReport(PrintFile.Accy.INV102.RunReportINV102.OpenReport(print_record, "RC101_1"));
                    break;
                case "CN101":// CN
                    docviewer.OpenReport(PrintFile.Accy.CN101.RunReportCN.OpenReport(print_record, "CN101"));
                    break;
                case "RC101":// พิมพ์ใบเสร็จรับเงินขายสินค้า
                    docviewer.OpenReport(PrintFile.Accy.RC101.RunReportRC.OpenReport(print_record, "RC101"));
                    break;
                case "RC102":// พิมพ์ใบเสร็จรับเงินงานบริการ
                    docviewer.OpenReport(PrintFile.Accy.RC101.RunReportRC.OpenReport(print_record, "RC102"));
                    break;
                case "TAX101":// พิมพ์รายงานภาษีขาย
                    docviewer.OpenReport(PrintFile.Accy.TAX101.RunReportTAX.OpenReport(print_record, "TAX101"));
                    break;
                case "BL101":// พิมพ์ใบวางบิล
                    docviewer.OpenReport(PrintFile.Accy.BL101.RunReportBL.OpenReport(print_record, "BL101"));
                    break;
                case "WHT101":// พิมพ์ใบหัก ณ ที่จ่าย
                    docviewer.OpenReport(PrintFile.Accy.WHT101.RunReportWHT.OpenReport(print_record, "WHT101"));
                    break;
                case "WHT102":// pnd53
                    docviewer.OpenReport(PrintFile.Accy.WHT102.RunReportWHT102.OpenReport(print_record, "WHT102"));
                    break;

                case "PVWHT101"://รายงานหัก ณ ที่จ่ายรายเดือน
                    docviewer.OpenReport(PrintFile.Accy.PVWHT101.RunReport.OpenReport(print_record, "PVWHT101"));
                    break;
                case "Envelop":// พิมพ์หน้าซองจกหมาย
                    docviewer.OpenReport(PrintFile.Accy.FrontEnvelop.RunReportFrontEnvelop.OpenReport(print_record, "Envelop"));
                    break;
                case "EnvelopINV":// พิมพ์หน้าซองจกหมายที่หน้าINV
                    docviewer.OpenReport(PrintFile.Accy.FrontEnvelop.RunReportFrontEnvelopINV.OpenReport(print_record, "EnvelopINV"));
                    break;
                default:
                    break;
            }
        }


        private PrintData GetPrintRecord(string printid) {

            using (PrintEntities db = new PrintEntities()) {
                var print_row = db.PrintDatas.Where(o => o.PrintID == printid).FirstOrDefault();
                return print_row; 
            }
        }
    }
}