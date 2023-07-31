using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data202Set {
        //สถิติการแจ้งเตือนภัยตามจังหวัด

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string province   { get; set; }
            public string alert_type { get; set; }

            public int count_result { get; set; }
        }
    }
}
