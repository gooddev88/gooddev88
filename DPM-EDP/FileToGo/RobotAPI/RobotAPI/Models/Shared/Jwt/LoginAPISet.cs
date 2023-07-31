namespace RobotAPI.Models.Shared.Jwt {
    public class LoginAPISet {
        public UserAPI User { get; set; }
        public List<RoleAPI> Roles { get; set; }
    }
}
