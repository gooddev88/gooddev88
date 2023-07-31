using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.API.Model
{
    public class UserInMenuM {
        public int ID { get; set; }
        public string Username { get; set; }
        public string MenuID { get; set; }
        public string MenuName { get; set; }
        public int IsOpen { get; set; }
        public int IsCreate { get; set; }
        public int IsEdit { get; set; }
        public int IsPrint { get; set; }
        public int IsDelete { get; set; }
    }
}
