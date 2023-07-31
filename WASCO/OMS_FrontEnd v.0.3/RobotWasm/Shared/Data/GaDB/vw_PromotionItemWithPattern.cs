using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
    public class vw_PromotionItemWithPattern {
        public int ID { get; set; }
        public string? ProID { get; set; }
        public string? PatternID { get; set; }
        public string? ItemID { get; set; }
        public string? ItemCode { get; set; }
        public string? RCompanyID { get; set; }
        public string? CompanyID { get; set; }
        public string? Name1 { get; set; }
        public string? BrandID { get; set; }
        public string? BrandName { get; set; }
        public string? TypeID { get; set; }
        public string? TypeName { get; set; }
        public string? CateID { get; set; }
        public string? CateName { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIncVat { get; set; }
        public decimal? Cost { get; set; }
        public string? UnitID { get; set; }
        public string? StkUnitID { get; set; }
        public string? PhotoID { get; set; }
        public string? PhotoUrl { get; set; }
        public decimal BalQty { get; set; }
        public string? LocID { get; set; }
        public string? LocName { get; set; }
        public bool IsHold { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }

}
