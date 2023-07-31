using RobotWasm.Shared.Data.GaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Q {
    public class QFloodServiceModel {
        public class QFloodDocSet {
            public q_flood Flood { get; set; }
        }
    }
}
