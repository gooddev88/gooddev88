using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.ApiCate {
    public class I_ApiCateSet {
        public vw_api_cate head { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }

}
