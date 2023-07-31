using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Client.Service.Api;
using RobotWasm.Client.Service.Authen;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components;
using RobotWasm.Shared.Data.GaDB;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

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
            try {
                if (LogInInfo == null) {
                    var user = await _localStorage.GetItemAsync<string>(Globals.AuthUsername);
                    //user = string.IsNullOrEmpty(user) ? "X" : user;
                    LoginRequest n = new LoginRequest();
                    //if (user == "X")
                    //{
                    //    await _localStorage.SetItemAsync(Globals.AuthUsername, "X");
                    //    n.UserName = "X";
                    //    //_nav.NavigateTo("Dashboard/StartBoard", true);
                    //    //return;
                    //}
                    n = new LoginRequest { UserName = user, Password = "silent", Apps = "", RememberMe = true };
                    LogInInfo = await Task.Run(() => DoLogin(n));
                    if (LogInInfo == null) {
                        _nav.NavigateTo("Login", true);
                    }
                }
            } catch (Exception ex) {
                var x = ex.Message; 
            }
      
        }
        #region  check permission
        public bool CanOpen(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsOpen);
            }
            return result;
        }
        public bool CanCreate(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsCreate).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsCreate);
            }
            return result;
        }

        public bool CanEdit(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsEdit).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsEdit);
            }
            return result;
        }

        public bool CanDelete(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsDelete).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsDelete);
            }
            return result;
        }
        public bool CanPrint(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsPrint).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsPrint);
            }
            return result;
        }
        #endregion
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

        #region login cross app
        async public Task<LoginCrossReturn> CreateCrossAppReq(string rcom, string Username, string AppID, string ToPage, string BackPage) {
            LoginCrossReturn? output = new LoginCrossReturn();
            try {
                LoginCrossRequest p = new LoginCrossRequest { RComID = rcom, Username = Username, AppID = AppID, ToPage = ToPage, BackPage = BackPage };
                string strPayload = JsonSerializer.Serialize(p);
                string url = "api/LoginSql/CreateCrossAppReq";
                var res = await _http.PostAsJsonAsync(url, strPayload);
                output = JsonSerializer.Deserialize<LoginCrossReturn>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<LogInCrossAppReq> GetCrossAppReq(string reqid) {
            LogInCrossAppReq? output = new LogInCrossAppReq();
            try {
                var res = await _http.GetAsync($"api/LoginSql/GetCrossAppReq?reqid={reqid}");
                output = JsonSerializer.Deserialize<LogInCrossAppReq>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }
        #endregion

    }
}
