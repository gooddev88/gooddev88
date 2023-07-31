using RobotWasm.Shared.Data.DimsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.ML.DPMBaord.CustomBoard {
    public class CustomBoardDocSet {
        //fuck
        public custom_board_in_user CustomBoard { get; set; }
        public List<vw_custom_widget_in_user>? CustomWidgets { get; set; }
        //public List<custom_widget_param_in_user>? CustomWidgetParam { get; set; }
        //public List<custom_widget_master>? CustomWidgetMaster { get; set; }
    }
}
