using System;

namespace Robot.Data.GADB.TT {
    public class ItemInSellTime {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string ItemID { get; set; }
        public string SellTimeID { get; set; }
        public DateTime ActiveDateFr { get; set; }
        public DateTime ActiveDateTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }

}
