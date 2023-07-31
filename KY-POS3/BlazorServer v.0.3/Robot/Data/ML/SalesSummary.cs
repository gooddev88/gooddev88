using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.ML {
    public class SalesSummary
    {
        public Nullable<System.DateTime> SaleDateFr { get; set; }
        public Nullable<System.DateTime> SaleDateTo { get; set; }
        public string BranchID { get; set; }
        public string BranchName { get; set; }
        public decimal NettotalAmt { get; set; }
        public decimal NettotalAmtIncVat { get; set; }
        public decimal NettotalVatAmt { get; set; }
        public decimal RoundDown { get; set; }
        public decimal NetTotalAfterRound { get; set; }
        public int TotalBill { get; set; }
        public int TotalCancelBill { get; set; }
        public decimal CashPayAmt { get; set; }
        public decimal TransferPayAmt { get; set; }
        public decimal GrabAmt { get; set; }
        public decimal PandaAmt { get; set; }
        public decimal LineManAmt { get; set; }
        public decimal GoJekAmt { get; set; }
        public decimal RobinAmt { get; set; }
        public decimal ShopeeAmt { get; set; }
        public Nullable<decimal> AVGPerBIll { get; set; }
    }


}
