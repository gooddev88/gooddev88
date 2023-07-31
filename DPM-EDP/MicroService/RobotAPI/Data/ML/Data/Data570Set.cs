using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;
using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data570Set {
        //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<emc_sk1all_cms> rows { get; set; }
        }
      
    }
}
