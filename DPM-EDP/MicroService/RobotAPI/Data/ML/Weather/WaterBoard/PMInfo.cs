namespace RobotAPI.Data.ML.Weather.WaterBoard {
    public class PMInfo {

        public int count_total_station { get; set; }
 
        public List<PM_station> pm_in_station { get; set; }
        public List<PMStatusInStation> pm_status_count_station { get; set; }

    }

    public class PM_station {

        public string station_id { get; set; }
        public string station_name { get; set; }
        public string color { get; set; }
        public double rainfal_level { get; set; }
    }
    public class PMStatusInStation {

        public string logo_status { get; set; }
        public string color { get; set; }
        public string pm_status { get; set; }
        public int count_station { get; set; }
    }
}
