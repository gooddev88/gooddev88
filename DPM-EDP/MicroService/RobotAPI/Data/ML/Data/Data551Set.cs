using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;
using System;
using System.Collections.Generic;

namespace RobotAPI.Data.ML.Data {
    public class Data551Set {
        //จำนวนสิ่งของสำรองจ่าย แบ่งตามศูนย์

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<est_stock_cms_c> rows { get; set; }
        }
        public class est_stock_cms_c: est_stock_cms {
            public string section_name { get; set; }
            public string section_code { get; set; }
           
           
        }
    }
}
