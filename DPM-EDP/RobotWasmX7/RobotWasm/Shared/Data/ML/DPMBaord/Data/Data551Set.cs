using RobotWasm.Shared.Data.DimsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data551Set {
        //จำนวนสิ่งของสำรองจ่าย แบ่งตามศูนย์

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<est_stock_cms_c> rows { get; set; }
        }
        public class est_stock_cms_c : est_stock_cms {
            public string section_name { get; set; }
            public string section_code { get; set; }


        }
    }
}
