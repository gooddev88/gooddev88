namespace RobotAPI.Data.ML.Weather.WaterBoard {
    public class RainFallInfo {

        public int count_total_station { get; set; }
        public string raining_status_today_icon { get; set; }
        public string raining_status_today { get; set; }
        public string raining_status_tomorrow { get; set; }
        public string raining_status_next_2days { get; set; }
        public List<RainFal_station> rainfal_in_station { get; set; }
        public List<RainingStatusInStation> raining_status_count_station { get; set; }

    }

    public class RainFal_station {

        public string station_id { get; set; }
        public string station_name { get; set; }
        public string color { get; set; }
        public double rainfal_level { get; set; }
    }
    public class RainingStatusInStation {

        public string logo_status { get; set; }
        public string color { get; set; }
        public string raining_status { get; set; }
        public int count_station { get; set; }
    }
}
