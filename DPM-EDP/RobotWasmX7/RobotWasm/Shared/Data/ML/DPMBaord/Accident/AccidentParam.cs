using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Accident {
    public class AccidentParam {
        public class AccidentCountSetParam {
            public DateTime DateBegin { get; set; }
            public DateTime DateEnd { get; set; }
            public bool isGetAllProvince { get; set; }
        
            public List<string> Province { get; set; }
        }
    }
}
