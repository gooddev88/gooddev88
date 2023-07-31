using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.API.Model
{
    public class UserInComM {
        public int ID { get; set; }
        public string RCompanyID { get; set; }
        public string CompanyID { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }

    }
}
