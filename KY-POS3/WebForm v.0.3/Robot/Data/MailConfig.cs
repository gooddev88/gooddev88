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
    
    public partial class MailConfig
    {
        public int ID { get; set; }
        public string MailCode { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string SenderName { get; set; }
        public string StmptServer { get; set; }
        public int PortNumber { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string ProviderID { get; set; }
        public bool IsActive { get; set; }
    }
}