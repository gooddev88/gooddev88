using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.ML {
    public class PORequestSummaryByItem {
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string ToLocID { get; set; }
        public DateTime PODate { get; set; }
            public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string CompanyName { get; set; }
        public decimal Qty { get; set; }
        public decimal Amt { get; set; }
        public decimal OrdAmtRound { get; set; }
        public decimal total { get; set; }
    }


}
