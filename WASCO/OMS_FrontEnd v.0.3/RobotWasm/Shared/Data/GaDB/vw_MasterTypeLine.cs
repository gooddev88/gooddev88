using System;

namespace RobotWasm.Shared.Data.GaDB {
    public class vw_MasterTypeLine {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string MasterTypeID { get; set; }
        public string ValueTXT { get; set; }
        public decimal ValueNUM { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }
        public int Sort { get; set; }
        public string ParentID { get; set; }
        public string ParentValue { get; set; }
        public string? RefID { get; set; }
        public string? RefIDL2 { get; set; }
        public string? RefIDL3 { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsSysData { get; set; }
        public bool IsActive { get; set; }
    }

}
