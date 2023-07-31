using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Robot.Data.GAUTHEN.BL {
    public class UserSet {
        public string AppID { get; set; }
        public UserInfo UserInfos { get; set; }
        public List<UserInRcom> UserInRCom { get; set; }

    }
}