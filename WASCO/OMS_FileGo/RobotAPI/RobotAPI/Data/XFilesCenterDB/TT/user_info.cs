namespace RobotAPI.Data.XFilesCenterDB.TT {
    public class user_info {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
     public string firstname { get; set; }
        public string lastname { get; set; }
        public string role { get; set; }
        public string jwt_token { get; set; }
        public string jwt_refresh_token { get; set; }
        public DateTime? jwt_token_expirydate { get; set; }
        public string created_by { get; set; }
        public DateTime created_date { get; set; }
        public string modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool is_active { get; set; }
    }

}
