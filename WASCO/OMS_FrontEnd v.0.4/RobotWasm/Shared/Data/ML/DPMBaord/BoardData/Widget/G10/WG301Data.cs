﻿namespace RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget.G10 {
    public class WG301Data {
        //จำนวน อปพร. แยกตามเพศ

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string GROUP_NAME { get; set; }
            public string PROVINCE_ID { get; set; }
            public string PROVINCE_NAME { get; set; }
            public string AMPHUR_ID { get; set; }
            public string AMPHUR_NAME { get; set; }
            public string TAMBOL_ID { get; set; }
            public string TAMBOL_NAME { get; set; }
            public string VOLUNTEER_ALL { get; set; }
            public string SUM_MALE { get; set; }
            public string SUM_FEMALE { get; set; }
            public string NO_GENDER { get; set; }
        }
        public class ChartDataRow {
 
            public string sex { get; set; }
            public int count_result { get; set; }
       
        }

    }

}