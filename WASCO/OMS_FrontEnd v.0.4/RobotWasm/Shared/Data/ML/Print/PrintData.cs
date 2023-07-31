using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.Print {
    public class PrintData {
        public int ID { get; set; }
        public string PrintID { get; set; }
        public string FormPrintID { get; set; }
        public string JsonData { get; set; }
        public DateTime PrintDate { get; set; }
        public string AppID { get; set; }
    }
}
