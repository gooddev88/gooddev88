using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data602Set {
        //ข้อมูลสัดส่วนงบประมาณหลังโอนเปลี่ยนแปลง

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
        public class DataRow {
            public int Year { get; set; }
            public string Department { get; set; }
            public string BudgetType { get; set; }
            public string BudgetType2 { get; set; }
            public double BudgetAmount { get; set; }
            public double TransferBudget { get; set; }

        }
    }
}
