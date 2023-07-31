using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.API.Model
{
    public class UserInfoM {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public bool IsActive { get; set; }



    }
}
