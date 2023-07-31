using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.DPMBaord.ExclusiveBoard {
    public class I_Board_MasterSet {
        public board_master head { get; set; }
        public vw_board_master vhead { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }

}
