using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;
using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data550Set {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด 

            public class DocSet {
                public int status { get; set; }
                public object message { get; set; }
                public List<est_stock_cms    > rows { get; set; }
            }
      
    }
}
