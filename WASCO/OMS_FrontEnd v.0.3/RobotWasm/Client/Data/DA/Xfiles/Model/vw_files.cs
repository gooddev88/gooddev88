namespace RobotWasm.Client.Data.DA.Xfiles.Model {
    public partial class vw_files {
        public int id { get; set; }
        public string file_id { get; set; }
        public string loc_id { get; set; }
        public string owner_id { get; set; }
        public string rcom_id { get; set; }
        public string com_id { get; set; }
        public string app_id { get; set; }
        public string doc_id { get; set; }
        public int doc_linenum { get; set; }
        public string source_table { get; set; }
        public string doc_type { get; set; }
        public string doc_cate { get; set; }
        public string remark { get; set; }
        public string db_server { get; set; }
        public string db_name { get; set; }
        public string login_name { get; set; }
        public string login_password { get; set; }
        public string root_path { get; set; }
        public string root_url { get; set; }
        public string file_type { get; set; }
        public string file_name { get; set; }
        public string file_ext { get; set; }
        public string file_path { get; set; }
        public string origin_filename { get; set; }
        public string origin_file_ext { get; set; }
        public string origin_filepath { get; set; }

        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
    }
}
