using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.PublishDoc {
    public class I_PublishDoc_DocSet {
        public publish_doc_head head { get; set; }
        public List<vw_publish_doc_line> line { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }


    public class xpublish_doc_head : publish_doc_head {
        public bool IsVisible { get; set; }
    }

}
