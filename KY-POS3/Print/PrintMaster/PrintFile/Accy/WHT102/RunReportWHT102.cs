
using DevExpress.XtraReports;
using Newtonsoft.Json;
using PrintMaster.Data.DA;
using PrintMaster.Data.PrintDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PrintMaster.PrintFile.Accy.WHT102 {
    public class RunReportWHT102
    {

        public static WHT102 OpenReport(PrintData print_row, string typeprint) {
            var doc = JsonConvert.DeserializeObject<WHT102Set>(print_row.JsonData);

            WHT102 report1 = new WHT102(doc);
            report1.DisplayName = "PND53";
            report1.CreateDocument();
          
            return report1;
           
        }

        public class WHT102Set {
            public Header Header { get; set; }
            public List<PND53> pnds { get; set; }
        }

        public class Header {
            public string TaxID { get; set; }
            public string Brn { get; set; }
            public string FormID { get; set; }
        }


        public class PND53 {
            public string TaxID { get; set; }
            public string Name { get; set; }

            public string DocNo { get; set; }
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string Brn { get; set; }
            public DateTime PayDate { get; set; }
            public string PayDesc { get; set; }//ประเภทเงินได้
            public decimal TaxRate { get; set; }
            public decimal TaxBaseAmt { get; set; }
            public decimal WhtAmt { get; set; }
            public string  Condition { get; set; }
        }


    }
}
