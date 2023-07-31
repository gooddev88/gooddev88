namespace RobotWasm.Shared.Data.ML.FileGo {
    public class MyTokenResponse {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public bool Success { get; set; }

        public string ErrorCode { get; set; }

        public string Error { get; set; }
        public string UserImageURL { get; set; }

    }
}
