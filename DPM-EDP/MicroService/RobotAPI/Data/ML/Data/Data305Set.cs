using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data305Set {
        //5 อันดับบราวเซอร์ที่ใช้งาน

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string browser { get; set; }
           
            public int count_result { get; set; }
        }
    }
}
