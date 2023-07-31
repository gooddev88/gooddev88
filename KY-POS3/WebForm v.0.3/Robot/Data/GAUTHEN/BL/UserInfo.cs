using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.GAUTHEN.BL {
    public partial class UserInfo {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsLock { get; set; }
        public bool IsActive { get; set; }
    }
}