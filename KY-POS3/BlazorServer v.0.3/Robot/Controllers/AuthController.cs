using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Robot.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Robot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        readonly IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly IConfiguration _config;
        public AuthController(IOptions<JwtAuthentication> jwtAuthentication, IConfiguration config)
        {
            _jwtAuthentication = jwtAuthentication;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AuthenAsync(Login login)
        {

            //*** ใส่ คำสั่ง Login จาก username password ในส่วนนี้

            var response = await Task.Run(() =>
            {
                var token = new JwtSecurityToken(
                    issuer: _jwtAuthentication.Value.ValidIssuer,
                    audience: _jwtAuthentication.Value.ValidAudience,
                    claims: new[]{
                            new Claim(JwtRegisteredClaimNames.Sub, login.Username),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    },
                    expires: DateTime.UtcNow.AddDays(1),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: _jwtAuthentication.Value.SigningCredentials);
                return new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expire = token.ValidTo,
                    Username = login.Username,
                };
            });
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        [ActionName("GetValue")]
        public string GetValue()
        {
            return "Authorize นะจ๊ะบักหำน้อย";
        }
    }
}
