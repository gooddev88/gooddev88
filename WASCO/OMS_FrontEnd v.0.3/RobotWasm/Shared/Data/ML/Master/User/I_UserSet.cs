using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.User {
    public class I_UserSet {
        public UserInfo User { get; set; }
        public List<XUserInGroup> XGroup { get; set; }
        public List<XUserInRCom> XRcom { get; set; }
    }

}
