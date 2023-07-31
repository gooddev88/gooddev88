namespace RobotAPI.Data.ML.Weather.WaterBoard {
    public class BasicInfo {
  
        public double rain_level_2daysago_max { get; set; }
        public double rain_level_3daysago_avg { get; set; }
        public double rain_level_7daysago_avg { get; set; }
        public double max_temp_of_week { get; set; }
        public double min_temp_of_week { get; set; }
        public double volumn_of_water_usage { get; set; }
        public double volumn_of_water_usage_percentage { get; set; }
        public int number_of_flood_district { get; set; }
        public int number_of_drought_district { get; set; }
        public string province_name { get; set; }
    }
}
