using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.DimsDB {
    public partial class board_authen {
        public int id { get; set; }
        public string authen_id { get; set; }
        public string authen_name { get; set; }
        public string board_username { get; set; }
        public string board_pass { get; set; }
        public string base_url { get; set; }
    }
}
