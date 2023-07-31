using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data604Set {
        //ข้อมูลเป้าหมายตาม ครม.

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public int Year { get; set; }
            public int TotalAmount { get; set; }
            public int TargetPercentQuater1 { get; set; }
            public int TargetPercentQuater2 { get; set; }
            public int TargetPercentQuater3 { get; set; }
            public int TargetPercentQuater4 { get; set; }
            public int TargetAmountQuater1 { get; set; }
            public int TargetAmountQuater2 { get; set; }
            public int TargetAmountQuater3 { get; set; }
            public int TargetAmountQuater4 { get; set; }

        }
    }
}
