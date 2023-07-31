using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data302Set {
        //5 อันดับข่าว ปภ. Today

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string news_id { get; set; }
            public string  news_title { get; set; }
            public int count_result { get; set; }
        }
    }
}
