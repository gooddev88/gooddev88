using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data301Set {
        //อันดับหมวดหมู่ข่าวเตือนภัย ปภ. Today

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string news_cate { get; set; }
            public int count_result { get; set; }
        }
    }
}
