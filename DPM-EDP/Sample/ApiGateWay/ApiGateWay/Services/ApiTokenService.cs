using ApiGateWay.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateWay.Services {
    public class ApiTokenService {
        public AuthToken GenerateToken(AuthUser user) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_categories_api_secret"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expirationDate = DateTime.UtcNow.AddYears(99);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(audience: "categoriesAudience",
                                              issuer: "categoriesIssuer",
                                              claims: claims,
                                              expires: expirationDate,
                                              signingCredentials: credentials);

            var authToken = new AuthToken();
            authToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authToken.ExpirationDate = expirationDate;
            authToken.IsSuccess = true;
            return authToken;
        }
    }
}
