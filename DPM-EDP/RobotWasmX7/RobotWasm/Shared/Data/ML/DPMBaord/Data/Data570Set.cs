using RobotWasm.Shared.Data.DimsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data570Set {
        //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<emc_sk1all_cms> rows { get; set; }
        }

    }
}
