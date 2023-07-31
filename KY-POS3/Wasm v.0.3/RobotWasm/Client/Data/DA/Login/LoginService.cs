using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Client.Service.Api;
using RobotWasm.Client.Service.Authen;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Client.Data.DA.Login {
    public class LoginService {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        private readonly HttpClient _http;
        private NavigationManager _nav;
        public LoginService(ClientService clientService,
            HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,

                           HttpClient http,
                           NavigationManager nav,
                           ILocalStorageService localStorage) {
            _httpClient = httpClient;
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _nav = nav;
        }

        public LoginSet? LogInInfo { get; set; }
        public async Task<LoginSet?> DoLogin(LoginRequest data_login) {
            try {
                string strPayload = JsonSerializer.Serialize(data_login);
                var res = await _http.PostAsync($"api/Login/Login", new StringContent(strPayload, Encoding.UTF8, "application/json"));
                if (!res.IsSuccessStatusCode) {
                    LogInInfo = NewLoginSet();
                } else {
                    LogInInfo = JsonSerializer.Deserialize<LoginSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (LogInInfo == null) {
                        LogInInfo = NewLoginSet();
                        return LogInInfo;
                    }
                    if (LogInInfo.LoginResult == "fail") {
                        LogInInfo = NewLoginSet();
                        return LogInInfo;
                    }
                    ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(LogInInfo.CurrentUserInfo);
                }
            } catch (Exception ex) {
                var xx = ex.Message;
            }
            return LogInInfo;
        }

        async public Task CheckLogin() {
            if (LogInInfo == null) {
                var user = await _localStorage.GetItemAsync<string>(Globals.AuthUsername);
                LoginRequest n = new LoginRequest();
                var rcom = await _localStorage.GetItemAsync<string>(Globals.localStorageRcomID);
                var com = await _localStorage.GetItemAsync<string>(Globals.localStorageComID);
                n = new LoginRequest { UserName = user, Password = "silent", Apps = "", Rcom = rcom, Com = com, RememberMe = true };
                LogInInfo = await Task.Run(() => DoLogin(n));
                if (LogInInfo == null) {
                    _nav.NavigateTo("Login", true);
                }
            }
        }

        public string GetFirstBoardUrl() {
            string url = "";
            if (LogInInfo == null) {
                url = "Dashboard/StartBoard";
            } else {
                if (LogInInfo?.UserInBoard.Count == 0) {
                    url = "Dashboard/StartBoard";
                } else {
                    LogInInfo.CurrentBoard = LogInInfo?.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();
                    url = LogInInfo.CurrentBoard.BoardUrl; 
                }
            }
            return url;
        }
        public async Task Logout() {

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _nav.NavigateTo("/Login", true);
        }



        public static LoginSet NewLoginSet() {

            LoginSet n = new LoginSet();
            //n.CurrentUser = "X";
            n.CurrentUser = "";
            n.CurrentUserInfo = NewUserInfo();
            n.CurrentRootCompany = new CompanyInfo { RCompanyID = "", CompanyID = "" };
            n.CurrentCompany = new CompanyInfo { RCompanyID = "", CompanyID = "" };
            n.UserInRCompany = new List<string> { "" };
            n.UserInCompany = new List<string> { "" };
            n.CurrentTransactionDate = DateTime.Now.Date;
            n.UserInMenu = NewPermissionMenu();
            n.UserInMenuDisplay = NewPermissionMenu();
            n.UserInBoard = new List<vw_PermissionInBoard>();
            n.CurrentBoard = new vw_PermissionInBoard();
            n.UserMenu = new List<UserMenu>();
            n.AppLogoImage = "../../../Image/Logo/app_logo.png";
            n.BackgroundImage = "";
            n.LoginResult = "";
            n.LoginResultInfo = "";
            n.PageError = "";
            n.AppID = "";
            n.LogInByApp = "WEB";
            n.RefUserID = "";
            n.LatestPage = "Login";

            return n;
        }
        public static UserInfo NewUserInfo() {
            UserInfo n = new UserInfo();
            n.Username = "X";
            n.FullName = "";
            return n;
        }
        public static List<vw_PermissionInMenu> NewPermissionMenu() {
            List<vw_PermissionInMenu> nn = new List<vw_PermissionInMenu>();
            var n = new vw_PermissionInMenu();
            return nn;
        }

    }
}
