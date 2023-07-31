using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.DimsDB {
    public partial class dqt_data_logs {
        public Guid log_id { get; set; }
        public string? job_id { get; set; }
        public string? data_set_id { get; set; }
        public DateTime? job_date { get; set; }
        public TimeSpan? job_time { get; set; }
        public string? job_result { get; set; }
        public string? job_message { get; set; }
        public string? filename { get; set; }
    }
}
