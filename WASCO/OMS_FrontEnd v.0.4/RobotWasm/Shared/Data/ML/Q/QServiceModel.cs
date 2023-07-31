using RobotWasm.Shared.Data.GaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Q {
    public class QServiceModel {
        public class QDocSet {
            public List<q_flood> Flood { get; set; }
            public vw_q_answer_group AnsGroup { get; set; }
            public List<q_question> Questions { get; set; }
            public List<q_choice> Choices { get; set; }
            public List<q_answer> Answers { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
    }
}
