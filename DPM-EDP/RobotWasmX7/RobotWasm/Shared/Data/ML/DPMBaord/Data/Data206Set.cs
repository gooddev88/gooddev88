using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data206Set {
        //หา ตำบล อำเภอ จังหวัดจาก gps

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string tcode { get; set; }
            public string tname { get; set; }
            public string tname_e { get; set; }
            public string acode { get; set; }
            public string aname { get; set; }
            public string anme_e { get; set; }
            public string pcode { get; set; }
            public string pname { get; set; }
            public string pname_e { get; set; }
            public double diff { get; set; }
        }
    }
}
