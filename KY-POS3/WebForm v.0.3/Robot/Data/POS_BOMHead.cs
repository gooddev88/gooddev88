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
    
    public partial class POS_BOMHead
    {
        public int ID { get; set; }
        public string ComID { get; set; }
        public string RComID { get; set; }
        public string BomID { get; set; }
        public string UserForModule { get; set; }
        public string ItemIDFG { get; set; }
        public string ItemUnitFG { get; set; }
        public string Description { get; set; }
        public string Remark1 { get; set; }
        public bool IsDefault { get; set; }
        public int Priority { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
}