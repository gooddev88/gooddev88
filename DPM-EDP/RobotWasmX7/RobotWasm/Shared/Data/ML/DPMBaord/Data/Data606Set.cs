using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data606Set {
        //แสดงแผนก

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public string DepartmentGroup { get; set; }
            public string Department { get; set; }
          

        }
    }
}
