namespace Robot.Data.GADB.TT {
    public partial class ItemUnitConvert {
        public int ID { get; set; }
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string ItemID { get; set; }
        public string ToUnit { get; set; }
        public decimal QtyInBaseUnit { get; set; }
        public decimal QtyInThisUnit { get; set; }
        public bool IsBaseUnit { get; set; }
    }
}
