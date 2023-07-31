using System;

namespace Robot.Data.GADB.TT {
    public class ItemSellTime {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string SellTimeID { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public decimal MondayFr { get; set; }
        public decimal MondayTo { get; set; }
        public decimal TuesdayFr { get; set; }
        public decimal TuesdayTo { get; set; }
        public decimal WednesdayFr { get; set; }
        public decimal WednesdayTo { get; set; }
        public decimal ThursdayFr { get; set; }
        public decimal ThursdayTo { get; set; }
        public decimal FridayFr { get; set; }
        public decimal FridayTo { get; set; }
        public decimal SaturdayFr { get; set; }
        public decimal SaturdayTo { get; set; }
        public decimal SundayFr { get; set; }
        public decimal SundayTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }

}
