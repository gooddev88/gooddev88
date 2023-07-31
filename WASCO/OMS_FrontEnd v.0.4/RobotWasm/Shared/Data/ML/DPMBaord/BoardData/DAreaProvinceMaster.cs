using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.BoardData {
    public partial class DAreaProvinceMaster {
        public int ID { get; set; }

        public string Province { get; set; }
        public string? ProvinceEng { get; set; }
        public string MapHKey { get; set; }
        public string MapColor { get; set; }
        public string DepID { get; set; }
        public bool IsActive { get; set; }
    }
}
