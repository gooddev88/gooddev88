using System;

namespace ApiGateWay.Models {
    public class AuthToken {
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
