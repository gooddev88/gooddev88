using RobotAPI.Helpers;
using RobotAPI.Helpers.Jwt;
using RobotAPI.Models.Shared.Jwt;

namespace RobotAPI.Services.Jwt {
    public interface IJwtTokenService {
        Task<UserAPI> GenerateTokensAsync(string userId);
        //Task<ValidateRefreshTokenResponse> ValidateRefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
        //Task<bool> RemoveRefreshTokenAsync(User user);
    }
    public class JwtTokenService : IJwtTokenService {
        // public async Task<Tuple<string, string>> GenerateTokensAsync(string userId) {
        public async Task<UserAPI> GenerateTokensAsync(string userId) {
            var expiryDate = DateTime.Now.Date.AddDays(15);

            var accessToken = await TokenHelper.GenerateAccessToken(userId, expiryDate);
            var refreshToken = await TokenHelper.GenerateRefreshToken();
            UserAPI output = new UserAPI {
                FirstName = "",
                LastName = "",
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = expiryDate,
                Token = accessToken,
                Username = userId
            };
            return output;
            //var userRecord = await tasksDbContext.Users.Include(o => o.RefreshTokens).FirstOrDefaultAsync(e => e.Id == userId);
            //if (userRecord == null)
            //{
            //    return null;
            //}
            //var salt = PasswordHelper.GetSecureSalt();
            //var refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);
            //if (userRecord.RefreshTokens != null && userRecord.RefreshTokens.Any())
            //{
            //    await RemoveRefreshTokenAsync(userRecord);
            //}
            //userRecord.RefreshTokens?.Add(new RefreshToken
            //{
            //    ExpiryDate = DateTime.Now.AddDays(14),
            //    Ts = DateTime.Now,
            //    UserId = userId,
            //    TokenHash = refreshTokenHashed,
            //    TokenSalt = Convert.ToBase64String(salt)
            //});
            //await tasksDbContext.SaveChangesAsync();

            //var token = new Tuple<string, string>(accessToken, refreshToken);

            //return token;
        }
    }
}
