using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.ML {
    public class PORequestSummaryByStoreInDetail
    {
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string ToLocID { get; set; }
        public string CompanyName { get; set; }
        public DateTime PODate { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string ItemID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal Amt { get; set; }
        public string Unit { get; set; }
    }


}
