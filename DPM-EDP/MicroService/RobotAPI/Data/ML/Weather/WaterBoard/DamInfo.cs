namespace RobotAPI.Data.ML.Weather.WaterBoard {
    public class DamInfo {
        public double dam_use { get; set; }
        public double dam_storage { get; set; }
        public double dam_use_percentage { get; set; }
        public int count_total_dam { get; set; }
        public string datestring { get; set; }
        public List<WaterStatusInDam> water_status_count_station { get; set; }
    }

    public class WaterStatusInDam {

        public string logo_status { get; set; }
        public string color { get; set; }
        public string water_status { get; set; }
        public int count_station { get; set; }
    }
}
