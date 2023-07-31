using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data306Set {
        //ข่าวล่าสุด 100 ลำดับแรก

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string id { get; set; }
            public string news_id { get; set; }
            public DateTime? news_date { get; set; }
            public string title { get; set; }
            public string detail { get; set; }
            public string pic_link { get; set; }
            public string news_link { get; set; }
            public string news_cate { get; set; }
            public string type { get; set; }
            public string source { get; set; }
            public string tag { get; set; }
            public int like_count { get; set; }
            public DateTime? n_timestamp { get; set; }
            //extend property
            public int Number { get; set; }
            public string news_cate_logo { get; set; }
            public string news_order_logo { get; set; }
        }
    }
}
