using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.BL {
    public class A_PO {
        public A_POHead Header { get; set; }
        public List<A_POLine> Line { get; set; }
    }




    public class A_POHead {
        public string COMPANY { get; set; }//Company
        public string PONUMBER { get; set; }//Purchase Order Number
        public string VDCODE { get; set; }// Vendor
        public string TERMSCODE { get; set; }// Terms Code
        public DateTime DATE { get; set; }// Purchase Order Date
        public bool ONHOLD { get; set; }// On Hold
        public string STCODE { get; set; }//Ship-To Location 
        public string DESCRIPTIO { get; set; }// Description
        public string REFERENCE { get; set; }// Reference 
        public decimal FREIGHT { get; set; }//Optional Field
    }

    public class A_POLine {
        public string ITEMNO { get; set; }// Item Number
        public string ITEMDESC { get; set; }//Item Description
        public decimal OQORDERED { get; set; }//   Quantity Ordered
        public decimal UNITCOST { get; set; }// Unit Cost
        public int LINENUM { get; set; }// Line Number 
        public string PERMIT { get; set; }//Optional Field
    }
}