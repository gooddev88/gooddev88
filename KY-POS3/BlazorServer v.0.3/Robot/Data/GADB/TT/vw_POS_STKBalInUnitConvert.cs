namespace Robot.Data.GADB.TT {
    public partial class vw_POS_STKBalInUnitConvert {
        public int ID { get; set; }
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string ToUnit { get; set; }
        public decimal QtyBalInToUnit { get; set; }
        public decimal QtyOnOrdInToUnit { get; set; }
        public decimal QtyInstInToUnit { get; set; }
        public string BaseUnit { get; set; }
        public decimal QtyBalInBaseUnit { get; set; }
        public decimal QtyInBaseUnit { get; set; }
        public decimal QtyInThisUnit { get; set; }
        public string LocID { get; set; }
        public string SubLocID { get; set; }
    }
}
