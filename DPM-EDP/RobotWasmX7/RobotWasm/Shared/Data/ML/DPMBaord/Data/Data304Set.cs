using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data304Set {
        //จำนวนผู้เข้าชมประจำวัน

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public DateTime dates_veiw { get; set; }
           
            public int count_result { get; set; }
        }
    }
}
