using RobotWasm.Shared.Data.DimsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.ApiMaster {
    public class I_ApiMasterSet {
        public api_master Head { get; set; }
        public List<api_param_res> paramLine { get; set; }
        public List<api_tag> TagLine { get; set; }
        public I_BasicResult OutputAction { get; set; }
    }
}
