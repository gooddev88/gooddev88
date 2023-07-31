using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.API.Model
{
    public class CompanyInfoM {
        public string CompanyID { get; set; }
        public string BrnCode { get; set; }
        public string ParentID { get; set; }
        public string TypeID { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string TaxID { get; set; }
     public string BillAddr1 { get; set; }
        public string BillAddr2 { get; set; }
        public string AddrFull { get; set; }
        public string AddrFull2 { get; set; }
        public string AddrNo { get; set; }
        public string AddrTanon { get; set; }
        public string AddrTumbon { get; set; }
        public string AddrAmphoe { get; set; }
        public string AddrProvince { get; set; }
        public string AddrPostCode { get; set; }
        public string AddrCountry { get; set; }
        public string BookBankNo { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Email { get; set; }
        public string PriceTaxCondType { get; set; }
        
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }

    }
}
