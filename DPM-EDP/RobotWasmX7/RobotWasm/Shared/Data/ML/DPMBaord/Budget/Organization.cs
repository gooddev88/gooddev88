namespace RobotWasm.Shared.Data.ML.DPMBaord.Budget {
    public class Organization {

        public class DataRow {
            public int year { get; set; }
            public long totalAmount { get; set; }
            public double targetPercentQuater1 { get; set; }
            public double targetPercentQuater2 { get; set; }
            public double targetPercentQuater3 { get; set; }
            public double targetPercentQuater4 { get; set; }
            public int targetAmountQuater1 { get; set; }
            public int targetAmountQuater2 { get; set; }
            public int targetAmountQuater3 { get; set; }
            public int targetAmountQuater4 { get; set; }
        }

    }
}
