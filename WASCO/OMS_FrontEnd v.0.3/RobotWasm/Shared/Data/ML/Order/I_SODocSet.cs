using RobotWasm.Shared.Data.GaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Order {

    public class I_SODocSet {
        public vw_OSOHead Head { get; set; }
        public List<vw_OSOLine> Line { get; set; }
        public vw_OSOLine LineActive { get; set; }
        public List<vw_OSOLot> Lot { get; set; }
        public vw_OSOLot LotActive { get; set; }

        public List<TransactionLog> Log { get; set; }
        public I_BasicResult OutputAction { get; set; }
    }
   
    public class I_SODocFiterSet {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string DocType { get; set; }
        public string SearchText { get; set; }
        public string Status { get; set; }
        public string LockShowInSale { get; set; }
        public bool ShowActive { get; set; }
    }

    public class ItemDesplay {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string BrandID { get; set; }
        public string LocID { get; set; }
        public string TypeID { get; set; }
        public string CateID { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIncVat { get; set; }
        public decimal PriceProIncVat { get; set; }
        public bool IsSpecialPrice { get; set; }
        public decimal Qty { get; set; }
        public decimal DiscAmt { get; set; }
        public decimal BalQty { get; set; }
        public string ProID { get; set; }
        public string ProName { get; set; }
        public string PatternID { get; set; }
        public string ImageUrl { get; set; }
        public string UnitID { get; set; }
    }
}
