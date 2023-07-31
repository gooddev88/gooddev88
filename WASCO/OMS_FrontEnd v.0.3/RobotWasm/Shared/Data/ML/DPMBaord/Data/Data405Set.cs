using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data405Set {
        //สถิติการลาแต่ละประเภท (ประเภทพนักงาน)

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string work_year { get; set; }
            public string job_type { get; set; }
            public string leave_type { get; set; }
            public int count_result { get; set; }
 
        }
    }
}
