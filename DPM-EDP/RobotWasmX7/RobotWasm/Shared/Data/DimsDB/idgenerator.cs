using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.DimsDB {
    public partial class idgenerator
    {
        public int id { get; set; }
        public string? rcomid { get; set; }
        public string? comid { get; set; }
        public string? prefix { get; set; }
        public string? year { get; set; }
        public string? month { get; set; }
        public string? macno { get; set; }
        public string? funcid { get; set; }
        public int? digit_run_number { get; set; }
        public string description { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? latest_date { get; set; }
    }
}
