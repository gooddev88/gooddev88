using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Login {
    public class ResetPasswordRequest {
        public string User { get; set; }
        public string password { get; set; }
        public string NewPassword { get; set; }
    }
}
