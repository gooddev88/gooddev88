using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Robot.Data
{
    public class AuthStateProvider : AuthenticationStateProvider {
        private ProtectedLocalStorage _protectedLocalStore;
        private ISessionStorageService _sessionStorage;
        public AuthStateProvider(ISessionStorageService sessionStorage, ProtectedLocalStorage protectedLocalStore) {
            _sessionStorage = sessionStorage;
            _protectedLocalStore = protectedLocalStore;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var username = await _sessionStorage.GetItemAsync<string>("username");
            ClaimsIdentity identity;
            if (username != null) {
                identity = new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, username),
                }, "Robot");
            } else {
                identity = new ClaimsIdentity();
            }
            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }
        public void MarkUserAsAuthenticated(string username) {
            _sessionStorage.SetItemAsync("username", username);
            var identity = new ClaimsIdentity(new[]
                { new Claim(ClaimTypes.Name, username),
                }, "Robot");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        public void MarkUserAsLoggedOut() {
            _sessionStorage.RemoveItemAsync("username");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void ClearRememberLogin()
        {
            _protectedLocalStore.DeleteAsync(Globals.RememberUserLogin);
            _protectedLocalStore.DeleteAsync(Globals.RememberPasswordLogin);
        }
    }
}
