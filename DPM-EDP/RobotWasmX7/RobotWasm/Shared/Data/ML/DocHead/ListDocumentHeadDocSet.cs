using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.DocHead
{
    public class ListDocumentHeadDocSet
    {
        public List<xdoc_head> ListHead { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }
}
