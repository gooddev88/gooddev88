namespace RobotWasm.Shared.Data.ML.DPMBaord.CIMS {
    public class WarningPublish {

        public class DataRow {
            public string dataid { get; set; }
            public string publish_id { get; set; }
            public string publish_dt { get; set; }
            public string publish_issue { get; set; }
            public string publish_label { get; set; }
            public string fax_file64 { get; set; }
            public string fax_filename { get; set; }
            public object fax_file64_rc { get; set; }
            public string fax_filename_rc { get; set; }
            public string report_file64 { get; set; }
            public string report_filename { get; set; }
            public string map_file64 { get; set; }
            public string map_filename { get; set; }
            public string news_file64 { get; set; }
            public string news_filename { get; set; }
            public string incident_link { get; set; }
            public string entrydate { get; set; }
        }

    }
}
