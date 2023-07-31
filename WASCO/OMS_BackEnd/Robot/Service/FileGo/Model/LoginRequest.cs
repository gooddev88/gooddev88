using System.ComponentModel.DataAnnotations;

namespace Robot.Service.FileGo.Model {
    public class LoginRequest {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool? RememberMe { get; set; }
        public string? Apps { get; set; }
    }
}
