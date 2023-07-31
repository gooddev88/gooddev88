using RobotAPI.Data.MainDB.TT;
using RobotAPI.Data.XFilesCenterDB.TT;
using RobotAPI.Helpers.Hash;
using RobotAPI.Models;
using RobotAPI.Models.Shared.Jwt;

namespace RobotAPI.Services.Jwt {
    public interface IJwtUserService {
        Task<MyTokenResponse> LoginAppAsync(LoginAPIRequest login);
    Task<MyTokenResponse> LoginApiAsync(LoginAPIRequest login);
    }
    public class JwtUserService : IJwtUserService {
        private readonly IJwtTokenService tokenService;

        public JwtUserService(IJwtTokenService tokenService) {
            this.tokenService = tokenService;
        }

        public async Task<MyTokenResponse> LoginAppAsync(LoginAPIRequest login) {
            MyTokenResponse output = new MyTokenResponse { Success = false, Error = "",ErrorCode="",FirstName="",RefreshToken="",Token="",UserName="" };
             
            try {
                string encrypt_password = Hash.hashPassword("MD5", login.Password);
                using (MainContext db = new MainContext()) {
                    var query = db.UserInfo.Where(o => o.Username == login.UserName && o.Password == encrypt_password && o.IsActive == true).FirstOrDefault();
                    if (query == null) {
                        output.ErrorCode = "401";
                        output.Error = "No user found.";
                    } else {
                        var token = await Task.Run(() => tokenService.GenerateTokensAsync(query.Username));
                        output = new MyTokenResponse {
                            Success = true,
                            Token = token.Token,
                            RefreshToken = token.RefreshToken,
                            UserName = query.Username,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            RefreshTokenExpiryTime = token.RefreshTokenExpiryTime,
                            Error = "",
                            
                            ErrorCode ="",
                            UserImageURL=""
                        
                        };
                        query.JwtToken = token.Token;
                        query.JwtRefreshToken = token.RefreshToken;
                        query.JwtTokenExpiryDate = token.RefreshTokenExpiryTime;
                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {

                output.Success = false;
                output.Error = ex.Message;
            } 
            return output; 

        }


        public async Task<MyTokenResponse> LoginApiAsync(LoginAPIRequest login) {
            MyTokenResponse output = new MyTokenResponse { Success = false, Error = "", ErrorCode = "", FirstName = "", RefreshToken = "", Token = "", UserName = "" };

            try {
                string encrypt_password = Hash.hashPassword("MD5", login.Password);
                using (XfilescenterContext db = new XfilescenterContext()) {
                    var query = db.user_info    .Where(o => o.username == login.UserName && o.password == encrypt_password && o.is_active == true).FirstOrDefault();
                    if (query == null) {
                        output.ErrorCode = "401";
                        output.Error = "No user found.";
                    } else {
                        var token = await Task.Run(() => tokenService.GenerateTokensAsync(query.username));
                        output = new MyTokenResponse {
                            Success = true,
                            Token = token.Token,
                            RefreshToken = token.RefreshToken,
                            UserName = query.username,
                            FirstName = query.firstname,
                            LastName = query.lastname,
                            RefreshTokenExpiryTime = token.RefreshTokenExpiryTime,
                            Error = "",
                            ErrorCode = "",
                            UserImageURL = ""

                        };
                        query.jwt_token = token.Token;
                        query.jwt_refresh_token = token.RefreshToken;
                        query.jwt_token_expirydate = token.RefreshTokenExpiryTime;
                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {

                output.Success = false;
                output.Error = ex.Message;
            }
            return output;

        }
    }
}
