using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
    public class vw_q_answer_group {
        public int id { get; set; }
        public string group_id { get; set; }
        public string group_name { get; set; }
        public string group_description { get; set; }
        public string username { get; set; }
        public string area_level { get; set; }
        public string area_code { get; set; }
        public int group_sort { get; set; }
        public string created_by { get; set; }
        public DateTime? created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
    }

}
