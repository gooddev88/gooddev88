﻿ 
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.POS {

    public class I_BillFilterSet {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public string ComID { get; set; }
        public string RComID { get; set; }
        public string Table { get; set; }
        public string ShipTo { get; set; }
        public string MacNo { get; set; }
        public string Search { get; set; }
        public bool ShowActive { get; set; }
        public List<string> comIDs { get; set; }
    }

}