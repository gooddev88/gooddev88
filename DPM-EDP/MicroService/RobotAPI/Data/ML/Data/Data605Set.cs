using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data605Set {
        //ข้อมูลเป้าหมายตาม ปภ. 
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public int Year { get; set; }
            public double TotalAmount { get; set; }
            public double TargetPercentQuater1 { get; set; }
            public double TargetPercentQuater2 { get; set; }
            public double TargetPercentQuater3 { get; set; }
            public double TargetPercentQuater4 { get; set; }
            public double TargetAmountQuater1 { get; set; }
            public double TargetAmountQuater2 { get; set; }
            public double TargetAmountQuater3 { get; set; }
            public double TargetAmountQuater4 { get; set; }

        }
    }
}
