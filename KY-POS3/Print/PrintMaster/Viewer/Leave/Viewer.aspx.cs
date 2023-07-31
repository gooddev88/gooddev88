using DevExpress.XtraPrinting;
using Newtonsoft.Json;
using PrintMaster.Data.PrintDB;
using PrintMaster.PrintFile.Accy;
using PrintMaster.PrintFile.Leave;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrintMaster.Viewer.Leave
{
    public partial class Viewer : System.Web.UI.Page {
        //for Leave
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
                case "LEAVE"://leave    
                    var Leave101 = PrintFile.Leave.RunReportLeave.OpenReport(print_record, "LEAVE");
                    //Leave101.DisplayName= report_name;
                    docviewer.OpenReport(Leave101);
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