using RobotWasm.Shared.Data.DimsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.BoardData {
    public class vw_widget_in_user_display  {
 
 
        public string board_id { get; set; }
        public string widget_id { get; set; }
        public string widget_desc { get; set; }
        public int? colspan { get; set; }
        public int? rowspan { get; set; }
        public int? sort { get; set; }
        public int? has_param { get; set; }
 
        public string? group_id { get; set; }
        public string? group_name { get; set; }
        public int? is_active { get; set; }
    }
}
