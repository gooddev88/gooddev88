using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.User {
    public class I_UserSet {
        public user_info User { get; set; }
        public List<xuser_in_group> XGroup { get; set; }
    }

}
