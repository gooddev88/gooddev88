using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Master.UserGroup;

namespace RobotWasm.Shared.Data.ML.Master.UserGroup {
    public class I_userGroupSet {
        public UserGroupInfo Group { get; set; }
        //public List<vw_user_in_group> User { get; set; }
        public List<XMenu> XMenu { get; set; }
        public List<XUserGroupInCompany> XCompany { get; set; }
        public List<XUserGroupInBoard> XBoard { get; set; }
    }

}
