﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Robot.Data.GADB.TT
{
    public partial class POS_ORDERLine
    {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public string OrdID { get; set; }
        public int LineNum { get; set; }
        public string CustID { get; set; }
        public DateTime OrdDate { get; set; }
        public string DocType { get; set; }
        public string FrLocID { get; set; }
        public string ItemID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OrdQty { get; set; }
        public decimal ShipQty { get; set; }
        public decimal GrQty { get; set; }
        public decimal? BalQty { get; set; }
        public decimal OrdAmt { get; set; }
        public decimal ShipdAmt { get; set; }
        public decimal GrAmt { get; set; }
        public string Unit { get; set; }
        public string INVID { get; set; }
        public string GrUnit { get; set; }


        public bool IsActive { get; set; }
    }
}