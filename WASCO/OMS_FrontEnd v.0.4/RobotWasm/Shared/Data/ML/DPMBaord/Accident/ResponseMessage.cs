using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Accident {
    public class ResponseMessage
    {
        public string StatusCode { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
    }
}
