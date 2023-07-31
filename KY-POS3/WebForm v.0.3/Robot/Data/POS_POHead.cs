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
    
    public partial class POS_POHead
    {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string POID { get; set; }
        public System.DateTime PODate { get; set; }
        public string DocType { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public string OrdID { get; set; }
        public string ToLocID { get; set; }
        public string Remark1 { get; set; }
        public string AcceptedMemo { get; set; }
        public decimal Qty { get; set; }
        public decimal ShipQty { get; set; }
        public decimal GrQty { get; set; }
        public decimal Amt { get; set; }
        public decimal ShipdAmt { get; set; }
        public decimal GrAmt { get; set; }
        public string AcceptedBy { get; set; }
        public Nullable<System.DateTime> AcceptedDate { get; set; }
        public string ShipBy { get; set; }
        public Nullable<System.DateTime> ShpiDate { get; set; }
        public Nullable<System.DateTime> FinishFGDate { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
}