namespace RobotWasm.Client.Service.Authen.Model {
    public class RegisterResult {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
