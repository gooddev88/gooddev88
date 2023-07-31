namespace RobotWasm.Shared.Data.ML.DPMBaord.Budget {
    public class OverallBudget {

        public class DataRow {
            public int year { get; set; }
            public string department { get; set; }
            public string budgetType { get; set; }
            public string budgetType2 { get; set; }
            public int budgetAmount { get; set; }
            public double expenseAmount { get; set; }
            public int planQuater1 { get; set; }
            public int planQuater2 { get; set; }
            public int planQuater3 { get; set; }
            public int planQuater4 { get; set; }
            public double expenseQuater1 { get; set; }
            public double expenseQuater2 { get; set; }
            public double expenseQuater3 { get; set; }
            public double expenseQuater4 { get; set; }
        }

    }
}
