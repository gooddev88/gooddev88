 
using RobotWasm.Shared.Data.GaDB;
using System.Collections.Generic;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.POS {
    public class I_POSSaleSet {
        public POS_SaleHeadModel Head { get; set; }
        public List<POS_SaleLineModel> Line { get; set; }
        public POS_SaleLineModel LineActive { get; set; }
        public POSMenuItem SelectItem { get; set; }
        public List<POS_SalePaymentModel> Payment { get; set; }
        public POS_SalePaymentModel PaymentActive { get; set; }
        public List<TransactionLog> Log { get; set; }
        public I_BasicResult OutputAction { get; set; }
    }

    public class POS_SaleHeadModel : POS_SaleHead {
        public String ImageUrlShipTo { get; set; } = "frontstore.png";
    }
    public class POS_SaleLineModel : POS_SaleLine {

        public String KitchenMessageLogo { get; set; }
        public bool IsEditAble { get; set; }
        public String ImageUrl { get; set; }
        public string ImageSource { get; set; }

    }
    public class POS_SalePaymentModel : POS_SalePayment {
        public string PaymentTypeName { get; set; }
    }

    public class POSMenuItem {
        public string ItemID { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string CustId { get; set; }
        public string TypeName { get; set; }
        public string GroupName { get; set; }
        public string CateID { get; set; }
        public string GroupID { get; set; }
        public string TypeID { get; set; }
        public string AccGroup { get; set; }
        public decimal SellQty { get; set; }
        public decimal SellAmt { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string PriceTaxCondType { get; set; }
        public decimal Weight { get; set; }
        public string RefID { get; set; }
        public string WUnit { get; set; }
        public string Unit { get; set; }
        public bool IsStockItem { get; set; }
        public bool IsActive { get; set; }
    }


}
