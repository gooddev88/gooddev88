using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.DocHead {
    public class I_DocHeadSet {
        public doc_head head { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }

    public class xdoc_head : doc_head
    {
        public bool IsVisible { get; set; }
    }

}
