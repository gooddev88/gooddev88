//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBF.Data.GADB
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_OSOLine
    {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string OrdID { get; set; }
        public int LineNum { get; set; }
        public string DocTypeID { get; set; }
        public System.DateTime OrdDate { get; set; }
        public string INVID { get; set; }
        public Nullable<System.DateTime> INVDate { get; set; }
        public string RefDocID { get; set; }
        public int RefDocLineNum { get; set; }
        public string CustID { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemTypeID { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemCateID { get; set; }
        public string ItemGroupID { get; set; }
        public string ItemAccGroupID { get; set; }
        public bool IsStockItem { get; set; }
        public decimal Weight { get; set; }
        public string WUnit { get; set; }
        public decimal Qty { get; set; }
        public decimal QtyShip { get; set; }
        public decimal QtyInvoice { get; set; }
        public decimal QtyShipPending { get; set; }
        public decimal QtyInvoicePending { get; set; }
        public decimal AmtShipPending { get; set; }
        public decimal AmtInvoicePending { get; set; }
        public string Unit { get; set; }
        public string Packaging { get; set; }
        public decimal Price { get; set; }
        public decimal PriceIncVat { get; set; }
        public decimal BaseTotalAmt { get; set; }
        public decimal BaseTotalAmtIncVat { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal VatAmt { get; set; }
        public decimal TotalAmtIncVat { get; set; }
        public decimal VatRate { get; set; }
        public string VatTypeID { get; set; }
        public decimal OntopDiscAmt { get; set; }
        public decimal OntopDiscPer { get; set; }
        public decimal OntopDiscAmtIncVat { get; set; }
        public decimal DiscPer { get; set; }
        public decimal DiscAmt { get; set; }
        public decimal DiscAmtIncVat { get; set; }
        public string DiscCalBy { get; set; }
        public decimal DiscPPU { get; set; }
        public decimal DiscPPUIncVat { get; set; }
        public string LocID { get; set; }
        public string LocName { get; set; }
        public string SubLocID { get; set; }
        public string PackagingNo { get; set; }
        public string LotNo { get; set; }
        public string SerialNo { get; set; }
        public string BatchNo { get; set; }
        public Nullable<bool> IsSpecialPrice { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
        public string PointID { get; set; }
        public string PointName { get; set; }
        public string ShipID { get; set; }
        public int ShipLineNum { get; set; }
        public string DiscApproveBy { get; set; }
        public Nullable<System.DateTime> DiscApproveDate { get; set; }
        public string ImageUrl { get; set; }
        public string Status { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<int> Sort { get; set; }
        public string ProID { get; set; }
        public string ProName { get; set; }
        public string PatternID { get; set; }
        public Nullable<bool> PageBreak { get; set; }
        public bool IsActive { get; set; }
    }
}