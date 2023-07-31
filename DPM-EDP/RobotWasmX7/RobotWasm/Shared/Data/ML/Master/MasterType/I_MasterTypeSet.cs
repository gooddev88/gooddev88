using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.MasterType {
    public class I_MasterTypeSet {
        public master_type_head head { get; set; }

        public master_type_line lineAtive { get; set; }
        public List<master_type_line> line { get; set; }
    }

}
