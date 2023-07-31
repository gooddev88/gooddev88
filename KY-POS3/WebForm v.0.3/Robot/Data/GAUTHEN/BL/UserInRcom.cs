using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.GAUTHEN.BL {
    public partial class UserInRcom {
        public int Id { get; set; }
        public string AppId { get; set; }
        public string RcomId { get; set; }
        public string Username { get; set; }
        public bool IsLock { get; set; }
        public bool IsActive { get; set; }
    }
}