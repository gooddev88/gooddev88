using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
    public class vw_STKItemBal {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string LocID { get; set; }
        public string LocName { get; set; }
        public string SubLocID { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string TypeID { get; set; }
        public string CateID { get; set; }
        public string BrandID { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIncVat { get; set; }
        public decimal? OrdQty { get; set; }
        public decimal? InstQty { get; set; }
        public decimal BalQty { get; set; }
        public decimal? RetQty { get; set; }
        public string Unit { get; set; }
        public bool IsActive { get; set; }
    }
}
