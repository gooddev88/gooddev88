using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data403Set {
        //สถิติวันลาของส่วนงาน

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
           
            public string department_name { get; set; }
            public string sub_department_name { get; set; }
      //      public int month { get; set; }
            public int count_result { get; set; }
        
        }
    }
}
