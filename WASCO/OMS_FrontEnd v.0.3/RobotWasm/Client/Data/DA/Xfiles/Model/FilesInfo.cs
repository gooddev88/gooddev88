namespace RobotWasm.Client.Data.DA.Xfiles.Model {
    public partial class FilesInfo {

        public string? file_id { get; set; }
        public string? owner_id { get; set; }
        public string? rcom_id { get; set; }
        public string? com_id { get; set; }
        public string app_id { get; set; }
        public string? doc_id { get; set; }
        public int? doc_linenum { get; set; }

        public string file_type { get; set; }
        public string? doc_type { get; set; }
        public string? doc_cate { get; set; }
        public string? remark { get; set; }
        public string fileName { get; set; }
        public string data { get; set; }

    }
}
