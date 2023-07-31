using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Robot.Data.DA {
    public class LocalStorageService {
        ILocalStorageService localStore;
        AuthenticationStateProvider authStateProvider;
  

        public LocalStorageService(ILocalStorageService _localStore, AuthenticationStateProvider _authStateProvider ) {
            localStore = _localStore;
            authStateProvider= _authStateProvider;
          
        }


        public async void SetLogIn(string username) {
             ((AuthStateProvider)authStateProvider).MarkUserAsAuthenticated(username);
            await localStore.SetItemAsync("xusername", username);

        }

        public async void SetLogOut() {
            await localStore.RemoveItemAsync("xusername");

        }
        public async Task<string> GetLoginUser() {
            var value = await localStore.GetItemAsync<string>("xusername");
            return value;
        }
    }
}
