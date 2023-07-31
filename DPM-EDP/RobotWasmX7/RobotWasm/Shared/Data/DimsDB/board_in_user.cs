using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.DimsDB {
    public partial class board_in_user {
        public int id { get; set; }
        /// <summary>
        /// รหัส Dashboard
        /// </summary>
        public string board_id { get; set; }
        /// <summary>
        /// ชื่อผู้ใช้งาน
        /// </summary>
        public string username { get; set; }
        public string layout_json_h { get; set; }

        public string layout_json_v { get; set; }
        public int sort { get; set; }
        public int is_active { get; set; }
    }
}
