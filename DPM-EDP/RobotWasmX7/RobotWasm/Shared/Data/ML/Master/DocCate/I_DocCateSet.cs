using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.DocCate
{
    public class I_DocCateSet
    {
        public vw_publishdoc_cate head { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }

}
