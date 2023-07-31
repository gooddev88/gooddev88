using System.Collections.Generic;

namespace Robot.Data.ML.DeptFirst {
    public class DPM018DataSet {
        public class Row {
            public string M_TYPE_CODE { get; set; }
            public string M_TYPE_NAME_TH { get; set; }
            public string TOTAL_SUM { get; set; }
            public string GOV01_SUM { get; set; }
            public string GOV02_SUM { get; set; }
            public string GOV03_SUM { get; set; }
            public string GOV04_SUM { get; set; }
            public string GOV05_SUM { get; set; }
            public string GOV06_SUM { get; set; }
            public string GOV07_SUM { get; set; }
            public string GOV08_SUM { get; set; }
            public string GOV09_SUM { get; set; }
            public string GOV10_SUM { get; set; }
            public string GOV11_SUM { get; set; }
            public string GOV12_SUM { get; set; }
            public string GOV13_SUM { get; set; }
            public string GOV14_SUM { get; set; }
            public string GOV15_SUM { get; set; }
            public string GOV16_SUM { get; set; }
            public string GOV17_SUM { get; set; }
            public string GOV18_SUM { get; set; }
        }

        public class Root {
            public int status { get; set; }
            public object message { get; set; }
            public List<Row> rows { get; set; }
        }
    }
}
