using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.GADB.TT {
    public class MacRegister {
        public int ID { get; set; }
        public string DeviceID { get; set; }
        public string MacID { get; set; }
        public string RComID { get; set; }
        public string ComID { get; set; }
        public bool IsUse { get; set; }

        public string UserLogin { get; set; }
        public string NotificationToken { get; set; }

        public string DeviceModel { get; set; }
        public string DeviceName { get; set; }
        public string? AppVersion { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
