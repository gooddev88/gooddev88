using System.ComponentModel.DataAnnotations;

namespace ApiGateWay.Models {
    public class AuthUser {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
