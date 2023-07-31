using System.ComponentModel.DataAnnotations;

namespace RobotWasm.Shared.Data.ML.Login {
    public class LoginRequest { 
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Rcom { get; set; } = "";
        public string Com { get; set; } = "";
        public bool? RememberMe { get; set; }
        public string? Apps { get; set; }
    }
}
