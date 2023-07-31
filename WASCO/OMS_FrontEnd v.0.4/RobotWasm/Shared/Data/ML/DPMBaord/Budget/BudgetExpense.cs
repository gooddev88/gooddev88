namespace RobotWasm.Shared.Data.ML.DPMBaord.Budget {
    public class BudgetExpense {

        public class DataRow {
            public int year { get; set; }
            public string department { get; set; }
            public string budgetType { get; set; }
            public string budgetType2 { get; set; }
            public int budgetAmount { get; set; }
            public double expenseAmount { get; set; }
        }

    }
}
