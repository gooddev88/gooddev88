﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.GaDB {
    public partial class LoginSessionLog
    {
        public int ID { get; set; }
        public string LogInID { get; set; }
        public string Username { get; set; }
        public string Data { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime LastReqDate { get; set; }
        public bool? IsActive { get; set; }
    }
}