using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.DocHead {
    public class I_DocHeadFiterSet {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Cate { get; set; }
        public int IsPublish { get; set; }
        public string SearchText { get; set; }

    }

}
