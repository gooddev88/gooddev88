using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.ML {
    public class PORequestSummaryByStore
    {
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string ToLocID { get; set; }
        public DateTime PODate { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string CompanyName { get; set; }
        public decimal Qty { get; set; }
        public decimal Amt { get; set; }
        public decimal OrdAmtRound { get; set; }
        public decimal total { get; set; }
    }
    public class POSummaryByStoreWithVendor {
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string StoreID { get; set; }
        public string StoreName { get; set; }
        public DateTime PODate { get; set; }
        public string Vendor1Name { get; set; }
        public decimal? TotalInVendor1 { get; set; }
        public string Vendor2Name { get; set; }
        public decimal? TotalInVendor2 { get; set; }
        public string Vendor3Name { get; set; }
        public decimal? TotalInVendor3 { get; set; }
        public decimal? Total { get; set; }
      
    }

}
