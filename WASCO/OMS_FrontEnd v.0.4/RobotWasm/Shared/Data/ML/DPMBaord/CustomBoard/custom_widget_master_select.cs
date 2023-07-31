using RobotWasm.Shared.Data.DimsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.CustomBoard {
    public class custom_widget_master_select : custom_widget_master {
        public bool is_select { get; set; }
        public string? board_id { get; set; }
    }
}
