using System;

namespace Robot.Service.FileGo.Model {
    public partial class xfiles {
        public int id { get; set; }
        public string app_id { get; set; }
        public string file_id { get; set; }
        public string file_type { get; set; }
        public string file_name { get; set; }
        public string file_ext { get; set; }
        public string file_path { get; set; }
        public string origin_filename { get; set; }
        public string origin_file_ext { get; set; }
        public string origin_filepath { get; set; }
        public byte[] data { get; set; }
        public byte[]? data_thumb { get; set; }
        public string? data_inbase64 { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
    }
}
