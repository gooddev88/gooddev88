using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.GADB.TT {
    public class LogAPIRequest {
        public int ID { get; set; }
        public string APIName { get; set; }
        public string Username { get; set; }
        public string DeviceName { get; set; }
        public DateTime RequestDate { get; set; }
        public bool IsActive { get; set; }
    }

}
