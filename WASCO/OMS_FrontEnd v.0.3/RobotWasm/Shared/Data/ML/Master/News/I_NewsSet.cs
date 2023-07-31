using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.News {
    public class I_NewsSet {
        public news_info head { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }

}
