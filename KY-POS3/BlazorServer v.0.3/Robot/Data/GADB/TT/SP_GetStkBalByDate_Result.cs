using System;

namespace Robot.Data.GADB.TT {
    public class SP_GetStkBalByDate_Result {
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string ItemID { get; set; }
        public string StockAsDate { get; set; }
        public string ItemName { get; set; }
        public string LocID { get; set; }
        public string LotNo { get; set; }
        public Nullable<decimal> QtyBal { get; set; }
    }
}
