using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.ML.DPMBaord.Data {
    public class Data601Set {
        //ข้อมูลภาพรวมการใช้งบประมาณ

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
                public double ExpenseAmount { get; set; }
                public double PlanQuater1 { get; set; }
                public double PlanQuater2 { get; set; }
                public double PlanQuater3 { get; set; }
                public double PlanQuater4 { get; set; }
                public double ExpenseQuater1 { get; set; }
                public double ExpenseQuater2 { get; set; }
                public double ExpenseQuater3 { get; set; }
                public double ExpenseQuater4 { get; set; }

             

        }
    }
}
