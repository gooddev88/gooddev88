using System;

namespace RobotWasm.Shared.Data.GaDB {
    public class OSOLot {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string OrdID { get; set; }
        public int LineLineNum { get; set; }
        public int LineNum { get; set; }
        public string DocTypeID { get; set; }
        public DateTime OrdDate { get; set; }
        public string CustID { get; set; }
        public string ItemID { get; set; }
        public bool IsStockItem { get; set; }
        public decimal Qty { get; set; }
        public string Unit { get; set; }
        public string LocID { get; set; }
        public string LotNo { get; set; }
        public string SerialNo { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}
