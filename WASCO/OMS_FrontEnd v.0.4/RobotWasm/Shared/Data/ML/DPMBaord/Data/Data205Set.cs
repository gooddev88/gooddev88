using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data205Set {
        //สถิติเพศของผู้ใช้งาน

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string province { get; set; } 
            public int count_result { get; set; }
        }
    }
}
