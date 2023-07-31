using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data201Set {
        //สถิติการแจ้งเตือนตามประเภทภัยพิบัติ

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string alert_type { get; set; }
            public int   count_result { get; set; }
        }
    }
}
