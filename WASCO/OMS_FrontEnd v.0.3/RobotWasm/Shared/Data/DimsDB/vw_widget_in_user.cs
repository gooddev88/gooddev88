using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.DimsDB {
    public class vw_widget_in_user {
        public int id { get; set; }
        public string widget_id { get; set; }
        public string board_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string username { get; set; }

 public string orientation { get; set; }

        public int rowspan { get; set; }
        public int colspan { get; set; }
        public int sort { get; set; }

        public int is_active { get; set; }
    }
}
