using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data303Set {
        //จำนวนผู้เข้าชมตามช่วงเวลา

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public int in_hour { get; set; }
            public string  in_hour_rang{ get; set; }
            public int sort { get; set; }
            public int count_result { get; set; }
        }
    }
}
