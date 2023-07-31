using PrintMaster.Data.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrintMaster.PrintFile.OMS.STK113
{
    public partial class ViewerSK113 : System.Web.UI.Page
    {
        //for oms
        protected void Page_Load(object sender, EventArgs e)
        {
            OpenReport(Page.Request.QueryString["id"], Request.QueryString["export"]);

        }
        private void OpenReport(string printid, string isexport)
        {
            if (string.IsNullOrEmpty(isexport))
            {
                isexport = "0";
            }
            if (string.IsNullOrEmpty(printid))
            {
                return;
            }
            var print_record = PrintService.GetPrintRecord(printid);
            string report_name = "";
            switch (print_record.FormPrintID)
            {
              
                case "STK113"://sale stock 
                    var stk112 = PrintFile.OMS.STK113.RunReport.OpenReport(print_record, isexport);
                    docviewer.OpenReport(stk112);
                    break;
            }
        }


    }
}