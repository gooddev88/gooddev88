﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Robot.Data.GADB.TT
{
    public partial class MasterTypeLine
    {
        public int ID { get; set; }
        public string RComID { get; set; }
        public string MasterTypeID { get; set; }
        public string ValueTXT { get; set; }
        public decimal ValueNUM { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }
        public int Sort { get; set; }
        public string ParentID { get; set; }
        public string ParentValue { get; set; }
        public string RefID { get; set; }
        public bool? IsSysData { get; set; }
        public bool? IsActive { get; set; }
    }
}