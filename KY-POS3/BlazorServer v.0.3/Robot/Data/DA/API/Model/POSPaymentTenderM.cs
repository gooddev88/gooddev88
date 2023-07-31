 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace RobotAPI.Data.DA.ModelM
{

    public class POSPaymentTenderM {
        public int ID { get; set; }
        public string INVID { get; set; }
        public string INVUnqHID { get; set; }
        public string CompanyID { get; set; }
        public string TenderType { get; set; }
        public decimal PayAmt { get; set; }
        public decimal InvAmt { get; set; }
        public decimal ChangeAmt { get; set; }
        public bool IsActive { get; set; }
    }

}