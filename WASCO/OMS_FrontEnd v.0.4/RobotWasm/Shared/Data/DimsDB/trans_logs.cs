using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.DimsDB {
    public partial class trans_logs {
        public int id { get; set; }
        public string? module { get; set; }
        public string? log_desc { get; set; }
        public DateTime? log_date { get; set; }

        public string? username { get; set; }
        public string? fullname { get; set; }
        public string? app_id { get; set; }
        public string? action { get; set; }

        public string? doc_id { get; set; }


    }
}
