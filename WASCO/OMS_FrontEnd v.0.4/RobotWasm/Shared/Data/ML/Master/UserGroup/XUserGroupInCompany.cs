using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.UserGroup {

    public class XUserGroupInCompany : UserGroupInCompany
    {
        public bool X { get; set; }
        public string RComID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string CompanyTypeName { get; set; }
    }

}
