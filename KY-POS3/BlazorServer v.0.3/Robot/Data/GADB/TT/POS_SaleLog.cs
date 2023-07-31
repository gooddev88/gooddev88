using System;

namespace Robot.Data.GADB.TT {
    public class POS_SaleLog {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string MacNo { get; set; }
        public int SaveNo { get; set; }
        public int LineNum { get; set; }
        public string LineUnq { get; set; }
        public string BillID { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmt { get; set; }
        public string CreatedByApp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UploadedDate { get; set; }
        public bool IsActive { get; set; }
    }

}
