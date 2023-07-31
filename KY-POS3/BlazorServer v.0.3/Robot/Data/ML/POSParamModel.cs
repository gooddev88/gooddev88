using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.ML {
    public class POSParamModel {
        public string RComID { get; set; }
        public string ComID { get; set; }

        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public int forceRepeat { get; set; }
    }
}
