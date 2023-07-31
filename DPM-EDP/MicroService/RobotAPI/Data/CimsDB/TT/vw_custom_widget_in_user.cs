using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotAPI.Data.CimsDB.TT {
    public class vw_custom_widget_in_user {
        public int id { get; set; }
        public string username { get; set; }
        public string board_id { get; set; }
        public string widget_id { get; set; }
        public string widget_desc { get; set; }
        public int? default_v_colspan { get; set; }
        public int? default_v_rowspan { get; set; }
        public int? default_h_colspan { get; set; }
        public int? default_h_rowspan { get; set; }
        public int? default_v_sort { get; set; }
        public int? default_h_sort { get; set; }
        public int? h_rowspan { get; set; }
        public int? h_colspan { get; set; }
        public int? v_rowspan { get; set; }
        public int? v_colspan { get; set; }
        public int? v_sort { get; set; }
        public int? h_sort { get; set; }
        public string? group_id { get; set; }
        public string? group_name { get; set; } 
        public int? is_active { get; set; }
    }
}
