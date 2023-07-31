using DevExpress.XtraPrinting;
using Newtonsoft.Json;
using PrintMaster.Data.DA;
using PrintMaster.Data.PrintDB;
using PrintMaster.PrintFile.OMS;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrintMaster.Viewer.OMS {
    public partial class Viewer : System.Web.UI.Page {
        //for oms
        protected void Page_Load(object sender, EventArgs e) {
            OpenReport( Page.Request.QueryString["id"], Request.QueryString["export"]); 

        }
        private void OpenReport(string printid, string isexport) {
            if (string.IsNullOrEmpty(isexport)) {
                isexport = "0";
            }
            if (string.IsNullOrEmpty(printid)) {
                return;
            }
            var print_record= PrintService.GetPrintRecord(printid);
            string report_name = "";
            switch (print_record.FormPrintID) {
                case "SO101"://sale order 
                    var so101 = PrintFile.OMS.SO101.RunReport.OpenReport(print_record, isexport);
                    docviewer.OpenReport(so101);
                    break;
                case "STK112"://sale stock 
                    var stk112 = PrintFile.OMS.STK112.RunReport.OpenReport(print_record, isexport);
                    docviewer.OpenReport(stk112);
                    
                    break;
            }
        }


       
    }
}