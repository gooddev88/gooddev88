using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace RobotWasm.Client.Service.Authen {
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage) {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken)) {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
        }

        public async void MarkUserAsAuthenticated(UserInfo loginInfo) {
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
            //var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            //NotifyAuthenticationStateChanged(authState);

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, loginInfo.Username) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            //_localStorage.SetItemAsync(Globals.AuthToken, loginInfo.Token);
            await _localStorage.SetItemAsync(Globals.AuthUsername, loginInfo.Username);
            await _localStorage.SetItemAsync(Globals.AuthFullname, loginInfo.FirstName + " " + loginInfo.LastName);
            await _localStorage.SetItemAsync(Globals.AuthLoginSet, "");


            var dummy1 = await _localStorage.GetItemAsync<string>(Globals.Dummy1);
            var dummy2 = await _localStorage.GetItemAsync<string>(Globals.Dummy2);
            if (string.IsNullOrEmpty(dummy1)) {
              await  _localStorage.SetItemAsync(Globals.Dummy1, Guid.NewGuid().ToString());
            }
            if (string.IsNullOrEmpty(dummy2)) {
                await _localStorage.SetItemAsync(Globals.Dummy2, Guid.NewGuid().ToString());
            }
            //_localStorage.SetItemAsync(Globals.RefreshToken, loginInfo.RefreshToken);
            //_localStorage.SetItemAsync(Globals.UserImageURL, loginInfo.UserImageURL);

            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut() {
            //var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            //var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            //NotifyAuthenticationStateChanged(authState);


            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            //_localStorage.SetItemAsync(Globals.AuthUsername, "X");
            //_localStorage.SetItemAsync(Globals.AuthFullname, "ลงชื่อเข้าใช้ระบบ");
            _localStorage.SetItemAsync(Globals.AuthUsername, "");
            _localStorage.SetItemAsync(Globals.AuthFullname, "");

            _localStorage.RemoveItemAsync(Globals.AuthToken);
            _localStorage.RemoveItemAsync(Globals.AuthUsername);
            _localStorage.RemoveItemAsync(Globals.AuthFullname);
            _localStorage.RemoveItemAsync(Globals.AuthLoginSet);
            _localStorage.RemoveItemAsync(Globals.RefreshToken);
            _localStorage.RemoveItemAsync(Globals.UserImageURL);
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt) {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null) {
                if (roles.ToString().Trim().StartsWith("[")) {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles) {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                } else {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64) {
            switch (base64.Length % 4) {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
