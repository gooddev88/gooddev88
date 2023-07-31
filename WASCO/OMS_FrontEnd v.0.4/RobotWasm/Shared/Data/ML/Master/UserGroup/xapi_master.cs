using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.UserGroup {

    public class xapi_master : api_master {
        public bool X { get; set; }
        public string GroupID { get; set; }
    }

}
