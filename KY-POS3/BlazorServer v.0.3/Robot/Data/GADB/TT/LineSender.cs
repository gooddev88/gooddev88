﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

#nullable disable

namespace Robot.Data.GADB.TT
{
    public partial class LineSender
    {
        public int ID { get; set; }
        public string SenderID { get; set; }
        /// <summary>
        /// EMAIL/
        /// LINE
        /// </summary>
        public string AppID { get; set; }
        public string ApiUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string Secret { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }
}