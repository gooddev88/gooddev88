using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.API.Model
{
    public class ItemPriceInfoM {
        public int ID { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string CompanyID { get; set; }
        public string RCompanyID { get; set; }
        public string CustID { get; set; }
        public string TypeID { get; set; }
        public int UseLevel { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string SubLocID { get; set; }
        public string CateID { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIncVat { get; set; }
        public string PriceTaxCondType { get; set; }
        public string Remark { get; set; }
        public string Image { get; set; }

    }
}
