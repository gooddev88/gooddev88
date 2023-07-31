namespace RobotAPI.Data.MainDB.TT {
    public partial class LoginSessionLog {
        public int ID { get; set; }
        public string LogInID { get; set; }
        public string Username { get; set; }
        public string Data { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime LastReqDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
