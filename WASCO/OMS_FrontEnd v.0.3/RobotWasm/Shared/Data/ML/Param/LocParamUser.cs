using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Param {
    public class LocParamUser {
        public string RCom { get; set; }
        public string Com { get; set; }
        public string User { get; set; }
        public List<string> LocIds { get; set; }
    }
}
