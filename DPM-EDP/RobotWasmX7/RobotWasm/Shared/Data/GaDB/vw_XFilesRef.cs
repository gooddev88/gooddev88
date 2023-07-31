﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace RobotWasm.Shared.Data.GaDB
{
    public partial class vw_XFilesRef
    {
        public int ID { get; set; }
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string AppID { get; set; }
        public string DBServer { get; set; }
        public string DBName { get; set; }
        public string RootPathID { get; set; }
        public string RootUrl { get; set; }
        public string RootUrlPublic { get; set; }
        public string RootPath { get; set; }
        public string PathType { get; set; }
        public string FileID { get; set; }
        public string DocID { get; set; }
        public int DocLineNum { get; set; }
        public string SourceTable { get; set; }
        public string DocType { get; set; }
        public string DocCate { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileExt { get; set; }
        public string FilePath { get; set; }
        public string SubUrl { get; set; }
        public string OriginFileName { get; set; }
        public string OriginFileExt { get; set; }
        public string OriginFilePath { get; set; }
        public string Remark { get; set; }
        public string FullPathAndFile { get; set; }
        public string FullUrlAndFile { get; set; }
        public string? JwtToken { get; set; }
        public string? JwtRefreshToken { get; set; }
        public string? JwtTokenExpiryDate { get; set; } 
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}