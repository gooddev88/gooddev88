﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
    public partial class CompanyInfo {
        public int ID { get; set; }
        public string? RCompanyID { get; set; }
        /// <summary>
        /// Company+Brn 
        /// </summary>
        public string? CompanyID { get; set; }
        public string? GroupCode { get; set; }
        /// <summary>
        /// CompanyType
        /// </summary>
        public string? ComCode { get; set; }
        public string? BrnCode { get; set; }
        public string? ParentID { get; set; }
        public string? ShortCode { get; set; }
        /// <summary>
        /// GROUP/COMPANY/BRANCH
        /// </summary>
        public string? TypeID { get; set; }
        /// <summary>
        /// ชื่อที่ใช้ในการค้า
        /// </summary>
        public string? Name1 { get; set; }
        /// <summary>
        /// ชื่อจดทะเบียน
        /// </summary>
        public string? Name2 { get; set; }
        /// <summary>
        /// ชื่อที่ใช้ในการค้า
        /// </summary>
        public string? NameEn1 { get; set; }
        /// <summary>
        /// ชื่อจดทะเบียน
        /// </summary>
        public string? NameEn2 { get; set; }
        public string? BillAddr1 { get; set; }
        public string? BillAddr2 { get; set; }
        public string? TaxID { get; set; }
        /// <summary>
        /// Exclude / Include
        /// </summary>
        public string? TaxCalType { get; set; }
        public string? TaxGroupS { get; set; }
        public string? TaxGroupP { get; set; }
        public string? AddrFull { get; set; }
        public string? AddrFull2 { get; set; }
        /// <summary>
        /// เลขที่
        /// </summary>
        public string? AddrNo { get; set; }
        /// <summary>
        /// ถนน
        /// </summary>
        public string? AddrTanon { get; set; }
        /// <summary>
        /// เขต/ตำบล
        /// </summary>
        public string? AddrTumbon { get; set; }
        /// <summary>
        /// แขวง/อำเภอ
        /// </summary>
        public string? AddrAmphoe { get; set; }
        /// <summary>
        /// จังหวัด
        /// </summary>
        public string? AddrProvince { get; set; }
        /// <summary>
        /// รหัสไปรษณีย์
        /// </summary>
        public string? AddrPostCode { get; set; }
        /// <summary>
        /// ประเทศ
        /// </summary>
        public string? AddrCountry { get; set; }
        public string? Currency { get; set; }
        /// <summary>
        /// Tel1
        /// </summary>
        public string? Tel1 { get; set; }
        /// <summary>
        /// Tel1
        /// </summary>
        public string? Tel2 { get; set; }
        /// <summary>
        /// เบอร์มือถือ
        /// </summary>
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Fax { get; set; }
        /// <summary>
        /// หมายเหตุลูกค้า
        /// </summary>
        public string? Remark1 { get; set; }
        /// <summary>
        /// หมายเหตุลูกค้า
        /// </summary>
        public string? Remark2 { get; set; }
        /// <summary>
        /// เป็นศูนย์กระจายพัสดุ
        /// </summary>
        public bool?  IsWH { get; set; }
        public string? SalePersonID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
    }
}