﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RobotAPI.Data.MainDB.TT
{
    public partial class vw_PermissionGroupInMenu
    {
        public int RN { get; set; }
        public string UserGroupId { get; set; }
        public string GroupName { get; set; }
        public string MenuID { get; set; }
        public string MenuCode { get; set; }
        public string RComID { get; set; }
        public string MenuName { get; set; }
        public string GroupID { get; set; }
        public int GroupSort { get; set; }
        public string SubGroupID { get; set; }
        public int SubGroupSort { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool? IsOpen { get; set; }
        public bool NeedOpenPermission { get; set; }
        public bool? IsCreate { get; set; }
        public bool NeedCreatePermission { get; set; }
        public bool? IsEdit { get; set; }
        public bool NeedEditPermission { get; set; }
        public bool? IsDelete { get; set; }
        public bool NeedDeletePermission { get; set; }
        public bool? IsPrint { get; set; }
        public bool NeedPrintPermission { get; set; }
        public bool IsActive { get; set; }
    }
}