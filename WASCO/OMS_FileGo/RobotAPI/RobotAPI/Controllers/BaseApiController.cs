using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RobotAPI.Controllers {
    public class BaseApiController : ControllerBase {
        protected string ApiUserName => FindClaim(ClaimTypes.NameIdentifier);
        public BaseApiController() {
            //        ClaimAuth();
        }

        //private void ClaimAuth() {
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null) {
        //        IEnumerable<Claim> claims = identity.Claims;


        //    }
        //}
        private string FindClaim(string claimName) {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(claimName);
            if (claim == null) {
                return null;
            } else {
                IEnumerable<Claim> claims = claimsIdentity.Claims;
            }
            return claim.Value;

        }


        private void AllClaim() {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null) {
                IEnumerable<Claim> claims = identity.Claims;
                var xx = claims.FirstOrDefault().Value;
            }

        }
    }
}