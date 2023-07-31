using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Master.UserGroup;

namespace RobotWasm.Shared.Data.ML.Master.UserGroup {
    public class I_userGroupSet {
        public usergroup_info Group { get; set; }
        public List<vw_user_in_group> UserInGroup { get; set; }
        public List<xusergroup_in_menu> XMenu { get; set; }
        public List<xusergroup_in_board> XBoard { get; set; }
        public List<xapi_master> XApi { get; set; }
        //public I_BasicResult OutputAction { get; set; }
    }

}
