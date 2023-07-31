namespace RobotAPI.Data.ML.Budget {
    public class OverallBudget {
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
