using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Param {
    public class GenID {
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string DocType { get; set; }
        public DateTime DocDate { get; set; }
        public bool Isrun_next { get; set; }

        public string year_culture { get; set; } 
    }
}
