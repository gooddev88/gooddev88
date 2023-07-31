﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Robot.Data.GADB.TT
{
    public partial class POS_StkAdjustHead
    {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string AdjID { get; set; }
        public string DocType { get; set; }
        public string Description { get; set; }
        public string LocID { get; set; }
        public string Remark { get; set; }
        public string Memo { get; set; }
        public DateTime AdjDate { get; set; }
        public decimal AdjQty { get; set; }
        public decimal AdjAmt { get; set; }
        public decimal? ActualQty { get; set; }
        public decimal? ActualAmt { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        /// <summary>
        /// OPEN / ACCEPTED / SHIPPING / RECEIVED / CANCEL
        /// </summary>
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
    }
}