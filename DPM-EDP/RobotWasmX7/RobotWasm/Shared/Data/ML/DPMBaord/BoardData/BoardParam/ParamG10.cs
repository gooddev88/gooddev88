using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.BoardData.BoardParam {
    public class ParamG10 {
        //บอร์ดอุบัติเหตุ
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string FilterOption { get; set; }
        public string Provinces { get; set; }
    }
}
