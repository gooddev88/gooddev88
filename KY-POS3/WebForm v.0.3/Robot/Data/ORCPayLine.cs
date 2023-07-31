//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Robot.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ORCPayLine
    {
        public int ID { get; set; }
        public string RCID { get; set; }
        public int LineNum { get; set; }
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string DocType { get; set; }
        public System.DateTime RCDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string WHTRefNo { get; set; }
        public decimal TaxBaseAmt { get; set; }
        public decimal TaxRate { get; set; }
        public string PayBy { get; set; }
        public string PayByType { get; set; }
        public string PayByDesc { get; set; }
        public string PayByCate { get; set; }
        public string PayMemo { get; set; }
        public decimal PayAmt { get; set; }
        public string PayToBankCode { get; set; }
        public string PayToBookID { get; set; }
        public string PayToBookName { get; set; }
        public System.DateTime PayDate { get; set; }
        public Nullable<System.DateTime> ClearingDate { get; set; }
        public Nullable<System.DateTime> StatementDate { get; set; }
        public string CustBankCode { get; set; }
        public string CustBankName { get; set; }
        public string CustBankBranch { get; set; }
        public string PaymentRefNo { get; set; }
        public Nullable<System.DateTime> ChqDate { get; set; }
        public Nullable<System.DateTime> ChqDepositDate { get; set; }
        public Nullable<System.DateTime> ChqExpired { get; set; }
        public Nullable<System.DateTime> ChqReturnDate { get; set; }
        public string ChqReturnReason { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string CompletedMemo { get; set; }
        public string Currency { get; set; }
        public decimal RateExchange { get; set; }
        public string RateBy { get; set; }
        public System.DateTime RateDate { get; set; }
        public string DataSource { get; set; }
        public string RCStatus { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}