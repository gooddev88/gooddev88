using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.PublishDoc {
    public class ListPublishDocSet {
        public List<xpublish_doc_head> ListHead { get; set; }
        public List<vw_publish_doc_line> ListLine { get; set; }
    }

}
