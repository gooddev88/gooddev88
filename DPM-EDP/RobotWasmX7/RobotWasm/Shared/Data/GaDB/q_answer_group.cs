﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.GaDB {
    public partial class q_answer_group
    {
        public int id { get; set; }
        public string group_id { get; set; }
        public string group_name { get; set; }
        /// <summary>
        /// จังหวัด
        /// อำเภอ
        /// ตำบล
        /// หมู่บ้าน
        /// อปท
        /// ชุมชน
        /// </summary>
        public string username { get; set; }
        public string area_level { get; set; }
        public string area_code { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
    }
}