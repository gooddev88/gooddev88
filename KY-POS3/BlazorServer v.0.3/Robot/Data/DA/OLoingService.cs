using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static Robot.Data.DA.LogInService;

namespace Robot.Data.DA {
    public class OLoingService {
        ISessionStorageService sessionStorage;
        NavigationManager nav;
        ILocalStorageService localStorage;
        public OLoingService(ISessionStorageService _sessionStorage, ILocalStorageService _localStorage, NavigationManager _nav) {
            sessionStorage = _sessionStorage;
            nav = _nav;
            localStorage = _localStorage;
        }
        public LoginSet LoginInfo { get; set; }

    
        public async void SetLoginInfo(LoginSet login) {
            await sessionStorage.SetItemAsync("XUserInfo", login);
        }

        public async Task<LoginSet> GetLoginInfo() {
            return await sessionStorage.GetItemAsync<LoginSet>("XUserInfo");
        }
    }

}
