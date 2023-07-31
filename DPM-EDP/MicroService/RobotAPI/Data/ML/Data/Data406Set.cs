using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data406Set {
        //สถิติการลาแบ่งตามเพศ

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string gender { get; set; }
            public string leave_type { get; set; }
            //public int month { get; set; }
            public int count_result { get; set; }

        }
    }
}
