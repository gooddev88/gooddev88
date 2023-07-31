using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;
using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data571Set {
        //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามศูนย์

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<emc_sk1all_cms_c> rows { get; set; }
        }
        public class emc_sk1all_cms_c: emc_sk1all_cms {
            public string section_name { get; set; }
            public string section_code { get; set; }
           
           
        }
    }
}
