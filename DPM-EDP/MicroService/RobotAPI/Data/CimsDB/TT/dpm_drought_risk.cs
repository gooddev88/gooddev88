﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RobotAPI.Data.CimsDB.TT {
    public partial class dpm_drought_risk
    {
        public Guid? id { get; set; }
        public string province { get; set; }
        public string amphor { get; set; }
        public string tumbon { get; set; }
        public decimal? moo_no { get; set; }
        public string village { get; set; }
        public string org { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public string risk_level { get; set; }
        public DateTime? data_update { get; set; }
    }
}