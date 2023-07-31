using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.User {

    public class XUserInRCom : UserInRCom
    {
        public bool X { get; set; }
        public string RCompanyName { get; set; }
    }

}
