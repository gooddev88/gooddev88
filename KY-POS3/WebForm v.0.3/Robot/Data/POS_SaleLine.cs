//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Robot.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class POS_SaleLine
    {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string MacNo { get; set; }
        public int LineNum { get; set; }
        public string BillID { get; set; }
        public Nullable<int> RefLineNum { get; set; }
        public string RefLineUnq { get; set; }
        public string INVID { get; set; }
        public string FINVID { get; set; }
        public string LineUnq { get; set; }
        public string BelongToLineNum { get; set; }
        public string DocTypeID { get; set; }
        public System.DateTime BillDate { get; set; }
        public string BillRefID { get; set; }
        public string CustID { get; set; }
        public string TableID { get; set; }
        public string TableName { get; set; }
        public string ShipToLocID { get; set; }
        public string ShipToLocName { get; set; }
        public string Barcode { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemTypeID { get; set; }
        public string ItemCateID { get; set; }
        public string ItemGroupID { get; set; }
        public bool IsStockItem { get; set; }
        public decimal Weight { get; set; }
        public string WUnit { get; set; }
        public string Unit { get; set; }
        public decimal Qty { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIncVat { get; set; }
        public decimal BaseTotalAmt { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal VatAmt { get; set; }
        public decimal TotalAmtIncVat { get; set; }
        public decimal VatRate { get; set; }
        public string VatTypeID { get; set; }
        public decimal OntopDiscAmt { get; set; }
        public decimal OntopDiscPer { get; set; }
        public decimal DiscPer { get; set; }
        public decimal DiscAmt { get; set; }
        public string DiscCalBy { get; set; }
        public bool IsFree { get; set; }
        public decimal ShareGpPer { get; set; }
        public bool IsOntopItem { get; set; }
        public string PromotionID { get; set; }
        public string PatternID { get; set; }
        public string PaternValue { get; set; }
        public int MatchingNumber { get; set; }
        public string ProPackCode { get; set; }
        public bool IsProCompleted { get; set; }
        public string LocID { get; set; }
        public string SubLocID { get; set; }
        public string LotNo { get; set; }
        public string SerialNo { get; set; }
        public string BatchNo { get; set; }
        public string Remark { get; set; }
        public string Memo { get; set; }
        public string ImgUrl { get; set; }
        public int Sort { get; set; }
        public Nullable<int> KitchenFinishCount { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsLineActive { get; set; }
        public bool IsActive { get; set; }
    }
}