using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Param {
    public class GenPOSSaleIDParam {
        public string DocType { get; set; }
        public string Rcom { get; set; }
        public string StoreId { get; set; }
        public string Macno { get; set; }
        public string ShiptoId { get; set; }
        public bool Isrun_next { get; set; }
        public DateTime TransDate { get; set; }
    }
}
