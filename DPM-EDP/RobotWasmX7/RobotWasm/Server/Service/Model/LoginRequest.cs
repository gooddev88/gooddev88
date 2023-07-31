using System.ComponentModel.DataAnnotations;

namespace RobotWasm.Shared.Data.ML.FileGo {
    public class LoginRequest {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool? RememberMe { get; set; }
        public string? Apps { get; set; }
    }
}
