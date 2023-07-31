using RobotAPI.Data.CimsDB.TT;
using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data561Set {
        //อาสามัคร

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dpm_volunteer> rows { get; set; }
        }
      
    }
}
