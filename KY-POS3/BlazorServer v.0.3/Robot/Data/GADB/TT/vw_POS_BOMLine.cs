using System;

namespace Robot.Data.GADB.TT {
    public class vw_POS_BOMLine {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string BomID { get; set; }
        public int Priority { get; set; }
        public int LineNum { get; set; }
        public string UserForModule { get; set; }
        public string ItemIDFG { get; set; }
        public string ItemIDFGName { get; set; }
        public string ItemIDRM { get; set; }
        public string ItemIDRMName { get; set; }
        public string RMDescription { get; set; }
        public decimal FgQty { get; set; }
        public decimal RmQty { get; set; }
        public string FgUnit { get; set; }
        public string RmUnit { get; set; }
        public bool IsDefault { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }

}
