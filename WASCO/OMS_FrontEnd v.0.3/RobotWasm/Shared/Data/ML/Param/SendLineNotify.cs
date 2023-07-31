using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Param {
    public class SendLineNotify {
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string DocID { get; set; }
        public string Msg { get; set; }
        public string Msg_Type { get; set; }
    }
}
