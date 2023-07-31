using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data404Set {
        //สถิติวันลาของส่วนงาน

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string leave_type { get; set; }
            public int count_result { get; set; }
        
        }
    }
}
