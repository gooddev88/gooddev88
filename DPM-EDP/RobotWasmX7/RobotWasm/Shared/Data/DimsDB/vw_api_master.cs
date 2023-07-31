﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.DimsDB {
    public partial class vw_api_master {
        public int? id { get; set; }
        public string? api_id { get; set; }
        public string? owner_code { get; set; }
        public string? source_base_url { get; set; }
        public string? source_authen_type { get; set; }
        public string? source_api_username { get; set; }
        public string? source_api_password { get; set; }
        public string? source_api_token { get; set; }
        public string? source_connection_code { get; set; }
        public string? source_api_url { get; set; }
        public string? base_url { get; set; }
        public string? api_url { get; set; }
        public string? api_name { get; set; }
        public string? api_desc { get; set; }
        public string? api_type { get; set; }
        public string? method { get; set; }
        public string? version { get; set; }
        public string? authen { get; set; }
        public string? data_source { get; set; }
        public string? update_frequency { get; set; }
        public string? parameter_sample { get; set; }
        public string? output_sample { get; set; }
        public string? contact { get; set; }
        public string? cate { get; set; }
        public string? cate_name { get; set; }
        public int? is_publish { get; set; }
        public string? source_cate { get; set; }
        public string? url_page { get; set; }
        public string? remark { get; set; }
        public int? is_active { get; set; }
    }
}