 
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.MasterType {
    public class I_MasterTypeSet {
        public MasterTypeHead Head { get; set; }
        public MasterTypeLine LineActive { get; set; }
        public List<MasterTypeLine> Line { get; set; }
    }

}
