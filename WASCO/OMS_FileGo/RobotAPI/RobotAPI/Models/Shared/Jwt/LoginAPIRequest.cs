namespace RobotAPI.Models.Shared.Jwt {
    public class LoginAPIRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? RememberMe { get; set; }
        public string? Apps { get; set; }
    }
}
