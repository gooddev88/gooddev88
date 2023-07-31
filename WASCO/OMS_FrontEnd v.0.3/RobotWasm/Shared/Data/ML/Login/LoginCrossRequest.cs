using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Login {
    public class LoginCrossRequest {
        public string Username { get; set; }
        public string RComID { get; set; }
        public string AppID { get; set; }
        public string ToPage { get; set; }
        public string BackPage { get; set; }
    }
}
