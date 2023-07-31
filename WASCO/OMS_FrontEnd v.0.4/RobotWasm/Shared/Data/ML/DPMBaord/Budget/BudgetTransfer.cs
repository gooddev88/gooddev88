namespace RobotWasm.Shared.Data.ML.DPMBaord.Budget {
    public class BudgetTransfer {

        public class DataRow {
            public int year { get; set; }
            public int totalAmount { get; set; }
            public int targetPercentQuater1 { get; set; }
            public int targetPercentQuater2 { get; set; }
            public int targetPercentQuater3 { get; set; }
            public int targetPercentQuater4 { get; set; }
            public int targetAmountQuater1 { get; set; }
            public int targetAmountQuater2 { get; set; }
            public int targetAmountQuater3 { get; set; }
            public int targetAmountQuater4 { get; set; }
        }

    }
}
