using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data { 
    public class Data561Set {
        //อาสามัคร

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<dpm_volunteer> rows { get; set; }
        }

   

    } 
}
