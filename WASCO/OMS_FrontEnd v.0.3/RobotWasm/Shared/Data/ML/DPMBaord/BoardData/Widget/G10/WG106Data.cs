namespace RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget.G10 {
    public class WG106Data {
        //จำนวนผู้เสียชีวิตจากอุบัติเหตุทางถนน

        // public string vehicle_subtype { get; set; }
        // public DateTime accident_date { get; set; }
        //public int count_result { get; set; } 
        public string a_date { get; set; }
        public string a_type_name { get; set; }
        public int a_count { get; set; }

        public static List<WG106Data> GetResultFromAPI() {
            List<WG106Data> output = new List<WG106Data>();
            DateTime start_date = new DateTime(2022, 4, 2);
            for (int i = 0; i < 3; i++) {
                output.Add(new WG106Data { a_count = GetRandom(), a_date = start_date.AddDays(i).ToString("dd/MM/yyyy"), a_type_name = "รถยนต์" });
                output.Add(new WG106Data { a_count = GetRandom(), a_date = start_date.AddDays(i).ToString("dd/MM/yyyy"), a_type_name = "รถจักรยานยนต์" });
                output.Add(new WG106Data { a_count = GetRandom(), a_date = start_date.AddDays(i).ToString("dd/MM/yyyy"), a_type_name = "รถ6ล้อ" });
            }
            return output;
        }
        public static int GetRandom() {
            Random rnd = new Random();
            return rnd.Next(1, 101);
        }
    }
}
