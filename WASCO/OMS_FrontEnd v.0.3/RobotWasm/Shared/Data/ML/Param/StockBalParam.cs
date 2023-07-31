using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Param {
  
        public class StockBalParam {

            public String RCom { get; set; }
            public String Com { get; set; }
            public String Type { get; set; }
            public String Brand { get; set; }
            public String Month { get; set; }
            public String LocID { get; set; }
            public String Year { get; set; }
            public String CostCenter { get; set; }
            public String SearchText { get; set; }
        public String PromotionID { get; set; } 
        public List<string> UIC { get; set; }
            public List<string> UILoc { get; set; }
            public bool ShowNotZero { get; set; }

        }


}
