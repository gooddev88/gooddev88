using ApiGateWay.Data.DA;
using ApiGateWay.Models;
using ApiGateWay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateWay.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase {
        [HttpPost]
        [Route("categories")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetCategoriesAuthentication([FromBody] AuthUser user) {
            if (user.Username != "categories_user" || user.Password != "456") {
                return BadRequest(new AuthToken {   Token="",IsSuccess=false,ExpirationDate=System.DateTime.Now.AddYears(-999) });
            }
           
            return new ApiTokenService().GenerateToken(user);
        }

        [HttpPost]
        [Route("products")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetProductsAuthentication([FromBody] AuthUser user) {

         var result=  UserService.CheckLogin(user);
            if (result.Result=="fail") {
                return BadRequest(new AuthToken { Token = "", IsSuccess = false, ExpirationDate = System.DateTime.Now.AddYears(-999) });
            }
           

            return new ProductsApiTokenService().GenerateToken(user);
        }

        [HttpPost]
        [Route("jwt")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetJwtAuthentication([FromBody] AuthUser user) {
            if (user.Username != "products_user" || user.Password != "123") {
                return BadRequest(new AuthToken { Token = "", IsSuccess = false, ExpirationDate = System.DateTime.Now.AddYears(-999) });
            }

            return new ProductsApiTokenService().GenerateToken(user);
        }
    }
}
