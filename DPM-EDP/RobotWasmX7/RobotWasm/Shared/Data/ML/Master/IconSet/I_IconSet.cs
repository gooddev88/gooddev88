using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.IconSet
{
    public class I_IconSet
    {
        public icon_set head { get; set; }
        public vw_icon_set vhead { get; set; }
        public List<vw_xfile_ref> files { get; set; }
    }

}
