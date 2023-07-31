using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.User {

    public class xuser_in_group : user_in_group {
        public bool X { get; set; }
        public string Name { get; set; }
    }

}
