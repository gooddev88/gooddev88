using PrintMaster.Data.PrintDB;
using PrintMaster.PrintFile.Accy;
using System;
using System.Linq;
using System.Web.UI;

namespace PrintMaster.Viewer.KyPos {
    public partial class Viewer : Page {
        //for pos
     
        protected void Page_Load(object sender, EventArgs e) {
            Report1 report1 = new Report1();  
            OpenReport(Request.QueryString["id"], Request.QueryString["export"]); 
 
        }
        private void OpenReport(string printid ,string isexport) {
            if (string.IsNullOrEmpty(printid)) {
                return;
            }
          var print_record=  GetPrintRecord(printid);
           
            switch (print_record.FormPrintID) {
                case "R401":
                    docviewer.OpenReport(PrintFile.KyPos.R40X.RunReport.OpenReportR401(print_record, isexport));
                    break;
                case "R402"://R402  ใบเสร็จรับเงินไม่มี Vat
                    docviewer.OpenReport(PrintFile.KyPos.R40X.RunReport.OpenReportR402(print_record, isexport));
                    break;
                case "R411":
                    docviewer.OpenReport(PrintFile.KyPos.R40X.RunReport.OpenReportR411(print_record, isexport));
                    break;
                case "R412":
                    docviewer.OpenReport(PrintFile.KyPos.R40X.RunReport.OpenReportR412(print_record, isexport));
                    break;
                case "R421":
                    docviewer.OpenReport(PrintFile.KyPos.R40X.RunReport.OpenReportR421(print_record, isexport));
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