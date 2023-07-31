namespace RobotAPI.Data.ML.Weather.WaterBoard {
    public class DiasterSummary {
        //ข้อมูลสรุปตามประเภทภัย
        public int count_flood { get; set; }//น้ำท่วม
        public int count_windstorm { get; set; }//พายุ
        public int count_fire { get; set; }//ไฟใหม้
     public int count_wildfire { get; set; }//ไฟป่า
        public int count_landslide { get; set; }//ดินโคลนถล่ม
        public int count_chemical { get; set; }//สารเคมี
        public int count_building_collapse { get; set; }//อาคารถล่ม
        public int count_drought { get; set; }//ภัยแล้ง
        public int count_tsunami { get; set; }//tsunami
        public int count_winter { get; set; }//winter
        public int count_epidemic { get; set; }//โรคระบาด
        public int count_toal_disaster { get; set; }//sum all community
    }
    public class CountDisaster {
        public string disaster_type { get; set; }
        public int count_type { get; set; }
    }
}
