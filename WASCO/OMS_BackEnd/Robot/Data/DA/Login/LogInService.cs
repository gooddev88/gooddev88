using Blazored.SessionStorage;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using Robot.Data.DA.Master;
using Robot.Data.GADB.TT;
using Robot.Data.ML.Login;
using Robot.Helper;
using Robot.Helper.Hash;
using Robot.Service.FileGo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.Login {
    public class LogInService {
        ISessionStorageService sessionStorage;
        NavigationManager nav;
    
        Blazored.LocalStorage.ILocalStorageService localStorage;
        public LogInService(ISessionStorageService _sessionStorage, Blazored.LocalStorage.ILocalStorageService _localStorage, NavigationManager _nav  ) {
            sessionStorage = _sessionStorage;
            nav = _nav;
            localStorage = _localStorage;
 
        }

        public   LoginSet LoginInfo { get; set; }
        public class LoginSet {
            public String CurrentUser { get; set; }
            public UserInfo CurrentUserInfo { get; set; }
            //  public String CurrentUserFullname { get; set; }
            public CompanyInfo CurrentRootCompany { get; set; }
            //public String CurrentRootCompanyName { get; set; }
            public CompanyInfo CurrentCompany { get; set; }
            //public string CurrentCompanyName { get; set; }
            public DateTime CurrentTransactionDate { get; set; }
            public List<String> UserInCompany { get; set; }
            public List<String> UserInRCompany { get; set; }
            public List<vw_PermissionInMenu> UserInMenu { get; set; }
            public List<vw_PermissionInMenu> UserInMenuDisplay { get; set; }
            public List<vw_PermissionInBoard> UserInBoard { get; set; }
    
            public vw_PermissionInBoard CurrentBoard { get; set; }
            public List<vw_PermissionInLoc> UserInLoc { get; set; }
            public vw_PermissionInLoc CurrentLoc { get; set; }
            public List<String> UserInDocStep { get; set; }
            public List<UserMenu> UserMenu { get; set; }

            public String AppLogoImage { get; set; }
            public String BackgroundImage { get; set; }
            public String LoginResult { get; set; }
            public String LoginResultInfo { get; set; }
            public String PageError { get; set; }
            public String AppID { get; set; }
            public String LogInByApp { get; set; }
            public String RefUserID { get; set; }
            public decimal CurrentVatRate { get; set; }
            public String CurrentMacNo { get; set; }
            public String LatestPage { get; set; }

        }

        #region Check login
        async public Task CheckLogin() {
            if (LoginInfo == null) {
                LoginInfo = await Task.Run(GetLoginSessionLog);
                if (LoginInfo == null) {
                    nav.NavigateTo("Login", true);
                }
            }
        }
        #endregion

        #region  OLogin
        public async void SetLoginInfo(LoginSet login) {
            await sessionStorage.SetItemAsync("XUserInfo", login);
        }

        public async Task<LoginSet> GetLoginInfo() {
            return await sessionStorage.GetItemAsync<LoginSet>("XUserInfo");
        }
        #endregion
     
        public static LoginSet Login(string username, string password,string appid, string rcom = "") {
            //password ="hhhhhHhhhh" คือ login โดยไม่ต้องมี Username (hack mode) 
            // LoginSet LoginInfo = new LoginSet();
          var   loginInfo = NewLoginSet();
            loginInfo.LoginResult = "ok";
            string encrypt_password =  Hash.hashPassword("MD5", password);
            try {
                using (GAEntities db = new GAEntities()) {
               
                 var   query = db.UserInfo.Where(o => o.Username == username && o.IsProgramUser && o.IsActive==true).FirstOrDefault();
                    loginInfo.CurrentUserInfo = query;
                    if (query == null) {//login success  
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "Incorrect username or password";
                        return loginInfo;
                    }


                    if (password != "silent") {//ไม่ใช่ hack mode
                        //login by db
                        if (query.Password != encrypt_password) {
                            loginInfo.LoginResult = "fail";
                            loginInfo.LoginResultInfo = "Incorrect username or password.";
                            return loginInfo;
                        }
                    }
                    //  LoginInfo.CurrentRootCompany = ListUserInRCompany(query.Username, rcom).FirstOrDefault();
                    loginInfo.UserInRCompany = ListUserInRCompany(query.Username, rcom);
                    if (loginInfo.UserInRCompany.Count == 0) {
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "No setup root company";
                        return loginInfo;
                    }
                    var currRcomID = ListUserInRCompany(query.Username, rcom).FirstOrDefault();
                    loginInfo.CurrentRootCompany = CompanyService.GetRCompanyInfo(currRcomID); 

               
                    loginInfo.UserInCompany = ListUserInCompany(query.Username, currRcomID);

                    if (loginInfo.UserInCompany.Count() == 0 && query.IsSuperMan == false) {
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "No user setup in company";
                        return loginInfo;
                    }
                    loginInfo.CurrentCompany = CompanyService.GetCompanyInfo(currRcomID, loginInfo.UserInCompany.FirstOrDefault());
                    if (loginInfo.CurrentCompany==null) {
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "No user setup in company";
                        return loginInfo;
                    }

                    loginInfo.LoginResult = "ok";
                    if (query.IsNewUser==true) {
                        loginInfo.LoginResult = "new";
                    }
                    
                    loginInfo.CurrentUser = query.Username;
                    loginInfo.CurrentTransactionDate = DateTime.Now.Date;
                    loginInfo.UserInMenu = ListUserInMenu(appid, loginInfo.CurrentRootCompany.CompanyID, query.Username);
                    loginInfo.UserInMenuDisplay = ListUserInMenuDisplay(loginInfo.UserInMenu);
                    loginInfo.UserInBoard = ListUserInBoard(query.Username);
                    loginInfo.CurrentBoard = loginInfo.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();
                    loginInfo.UserInLoc = ListUserInLoc(query.Username);
                    loginInfo.CurrentLoc = loginInfo.UserInLoc.OrderBy(o => o.LocID).FirstOrDefault();
                     
                    loginInfo.CurrentMacNo = "Z";
                    loginInfo.CurrentVatRate = 7;
                    loginInfo.BackgroundImage = "";
                 
                }
            } catch (Exception ex) {
                loginInfo.LoginResult = "fail";

                if (ex.InnerException != null) {
                    loginInfo.LoginResultInfo = ex.InnerException.ToString();
                } else {
                    loginInfo.LoginResultInfo = ex.Message.ToString();
                }

            }
            return loginInfo;
        }

        #region  ChangePassword

        public static I_BasicResult UpdatePasswordNew(string username, string password, string newPassword) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                    if (u == null) {
                        result.Result = "fail";
                        result.Message1 = "ไม่พบผู้ใช้งาน";
                    } else {
                        if (u.Password != password) {
                            result.Result = "fail";
                            result.Message1 = "รหัสผ่านไม่ถูกต้อง";
                        } else {
                            u.Password = newPassword;
                            u.IsNewUser = false;
                            db.SaveChanges();
                        }
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        #endregion

    

        async public void SetSessionDocSet(LoginSet data) {
            string json = JsonSerializer.Serialize(data);
 
            await sessionStorage.SetItemAsync("loginset", json);
        }
        async public Task<LoginSet> GetSessionDocSet() {
            var strdoc = await sessionStorage.GetItemAsync<string>("loginset");
            return JsonSerializer.Deserialize<LoginSet>(strdoc);
        }



        public string GetRootApp(string pageurl) {
            string rootdomain = nav.BaseUri.ToUpper().ToString().Replace("SALE/", "");
            string subApp = "APP";
            string url = rootdomain + subApp + pageurl;
            return url;
        }


        public LoginSessionLog NewLoginSessionLog(LoginSet data) {
            LoginSessionLog n = new LoginSessionLog();
            n.LogInID = Guid.NewGuid().ToString();
            n.Username = data.CurrentUser;
            n.Data = System.Text.Json.JsonSerializer.Serialize(data);
            n.CreatedDate = DateTime.Now;
            n.ExpiryDate = DateTime.Now.AddMonths(1);
            n.ModifiedDate = null;
            n.IsActive = true;
            n.LastReqDate = n.CreatedDate;
            return n;
        }
        public static List<string> ListUserInRCompany(string username, string rcom = "") {
            List<string> result = new List<string>();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.UserInRCom.Where(o =>
                                                        o.UserName == username
                                                        && (o.RComID == rcom || rcom == "")
                                                        ).Select(o => o.RComID).ToList();

                }
            } catch (Exception ex) {

            }
            return result;
        }
 

        public static List<string> ListUserInCompany(string username, string rcom) {

            List<string> result = new List<string>();
            try {
                string conStr = Globals.GAEntitiesConn;
                using (var connection = new SqlConnection(conStr)) {

                    var procedure = "[SP_LIST_USERINCOMPANY]";
                    var values = new { Username = username, RCompany = rcom };
                    var query = connection.Query(procedure, values, commandType: CommandType.StoredProcedure).ToList();
                    query.ForEach(q => result.Add(q.CompanyID));
                }
            } catch (Exception ex) {

            }
            return result;
        }

        public static List<vw_PermissionInMenu> ListUserInMenu(string appid,string rcom, string username) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.vw_PermissionInMenu.Where(o => o.Username == username 
                                                                && (o.AppID==appid || appid=="")
                                                                && o.RComID==rcom
                                                                ).ToList();
                }
            } catch (Exception ex) {

            }
            return result;
        }
        public static List<vw_PermissionInMenu> ListUserInMenuDisplay(List<vw_PermissionInMenu> input)
        {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try
            {
               
                result = input.Where(o=>o.IsOpen==true).GroupBy(x => new { x.Username,x.MenuCode, x.MenuID, x.GroupID, x.TypeID, x.MenuName, x.MenuDesc1, x.MenuDesc2, x.Url, x.Icon, x.GroupSort,x.AppID })
                                    .Select(r => new vw_PermissionInMenu
                                    {
                                        IsOpen = r.LastOrDefault().IsOpen,
                                        Username = r.Key.Username,
                                        MenuID = r.Key.MenuID,
                                        MenuCode = r.Key.MenuCode,
                                        GroupID = r.Key.GroupID,
                                        TypeID = r.Key.TypeID,
                                        MenuName = r.Key.MenuName,
                                        MenuDesc1 = r.Key.MenuDesc1,
                                        MenuDesc2 = r.Key.MenuDesc2,
                                        Url = r.Key.Url,
                                        Icon = r.Key.Icon,
                                        AppID=r.Key.AppID,
                                        GroupSort = r.Key.GroupSort
                                    }).ToList();

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetFirstBoardUrl() {
            string url = "";
            if (LoginInfo == null) {
                url = "Dashboard/StartBoard";
            } else {
                if (LoginInfo?.UserInBoard.Count == 0) {
                    url = "Dashboard/StartBoard";
                } else {
                    LoginInfo.CurrentBoard = LoginInfo?.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();
                    url = LoginInfo.CurrentBoard.BoardUrl;
                    
                }
            }
            return url;
        }
        public static List<vw_PermissionInBoard> ListUserInBoard(string username) {
            List<vw_PermissionInBoard> result = new List<vw_PermissionInBoard>();
            try {
                using (GAEntities db = new GAEntities()) {
                    var rr = db.vw_PermissionInBoard.Where(o => o.Username == username).GroupBy(o => new { o.Username, o.DashBoardID, o.BoardPage, o.BoardSort, o.BoardName ,o.BoardUrl})
                                                        .Select(g => new { g.Key.Username, g.Key.DashBoardID, g.Key.BoardPage, g.Key.BoardSort, g.Key.BoardName,g.Key.BoardUrl }).OrderBy(o=>o.BoardSort).ToList();
                    int i = 1;
                    foreach (var r in rr) {
                        vw_PermissionInBoard n = new vw_PermissionInBoard();
                        n.RN = i;
                        n.Username = r.Username;
                        n.DashBoardID = r.DashBoardID;
                        n.BoardSort = r.BoardSort;
                        n.BoardPage = r.BoardPage;
                        n.BoardName = r.BoardName;
                        n.BoardUrl = r.BoardUrl;
                        result.Add(n);
                        i++;
                    }
                }
            } catch (Exception ex) {
            }
            return result;
        }

        public static List<vw_PermissionInLoc> ListUserInLoc(string username) {
            List<vw_PermissionInLoc> result = new List<vw_PermissionInLoc>();
            try {
                using (GAEntities db = new GAEntities()) {
                    var rr = db.vw_PermissionInLoc.Where(o => o.Username == username).GroupBy(o => new { o.Username, o.GroupID, o.LocID, o.LocName })
                                                        .Select(g => new { g.Key.Username, g.Key.GroupID, g.Key.LocID, g.Key.LocName }).OrderBy(o => o.LocID).ToList();
                    int i = 1;
                    foreach (var r in rr) {
                        vw_PermissionInLoc n = new vw_PermissionInLoc();
                        n.RN = i;
                        n.Username = r.Username;
                        n.GroupID = r.GroupID;
                        n.LocID = r.LocID;
                        n.LocName = r.LocName;

                        result.Add(n);
                        i++;
                    }
                }
            } catch (Exception ex) {
            }
            return result;
        }
        public static List<string> ListUserInCustAreaID(string username) {
            List<string> result = new List<string>();

            return result;
        }

        public bool CanOpen(LoginSet xlogin, string menu)
        {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (per != null)
            {
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

        public bool CanEdit(LoginSet xlogin, string menu)
        {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsEdit).FirstOrDefault();
            if (per != null)
            {
                result = Convert.ToBoolean(per.IsEdit);
            }
            return result;
        }

        public bool CanDelete(LoginSet xlogin, string menu)
        {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsDelete).FirstOrDefault();
            if (per != null)
            {
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
        public static UserMenu GetMenuInfo(LoginSet xlogin, string menu)
        {
            UserMenu result = new UserMenu { MenuID = "", Name = "" };

            var menu_info = xlogin.UserInMenu.Where(o => o.MenuID == menu).FirstOrDefault();

            if (menu_info == null)
            {
                return result;
            }
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            result.GroupID= menu_info.GroupID;
            return result;
        }
        public static UserMenu GetMenuGroup(LoginSet xlogin, string group) {
            UserMenu result = new UserMenu { MenuID = "", Name = "" };

            var menu_info = xlogin.UserInMenu.Where(o => o.GroupID == group && o.TypeID== "MENUGROUP").FirstOrDefault();

            if (menu_info == null) {
                return result;
            }
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            return result;
        }
        public async static Task< List<UserMenu>> ListMenuInParent(LoginSet xlogin, string groupId) {
            List<UserMenu> result = new List<UserMenu>();

            var menus = xlogin.UserInMenuDisplay.Where(o => o.GroupID.ToLower()== groupId.ToLower() && o.TypeID!= "MENUGROUP").ToList();
            foreach (var m in menus) {
                UserMenu r = new UserMenu();
                if (result.Where(o=>o.MenuID==m.MenuID).FirstOrDefault()!=null) {
                    continue;
                }
                r.MenuID = m.MenuID;
                r.Name = m.MenuName;
                r.MenuCode = m.MenuCode;
                r.Desc1 = m.MenuDesc1;
                r.Desc2 = m.MenuDesc2;
                r.Url = m.Url;
                r.Icon=m.Icon;
                result.Add(r);
            } 
         
            return result;
        }
        public static LoginSet NewLoginSet() {

            LoginSet n = new LoginSet();
            n.CurrentUser = "";
            n.CurrentUserInfo = new UserInfo();
            n.CurrentRootCompany = new CompanyInfo();
            n.CurrentCompany = new CompanyInfo(); 
            n.UserInRCompany = new List<string>();
            n.UserInCompany = new List<string>();
            n.CurrentTransactionDate = DateTime.Now.Date;
            n.UserInMenu = new List<vw_PermissionInMenu>();
            n.UserInMenuDisplay = new List<vw_PermissionInMenu>();
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



        #region LoginSessionLog
        public async Task<I_BasicResult> SetLoginSessionLog(LoginSet data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var loginId = await localStorage.GetItemAsync<string>("loginsession");
                DateTime today = DateTime.Now;
                using (GAEntities db = new GAEntities()) {

                    if (string.IsNullOrEmpty(loginId)) {
                        LoginSessionLog n = NewLoginSessionLog(data);
                        db.LoginSessionLog.Add(n);
                        db.SaveChanges();
                        await localStorage.SetItemAsync("loginsession", n.LogInID);
                    } else {
                        var d = db.LoginSessionLog.Where(o => o.LogInID == loginId && o.IsActive == true && o.ExpiryDate >= today).FirstOrDefault();
                        if (d != null) {
                            d.Data = System.Text.Json.JsonSerializer.Serialize(data);
                            d.ModifiedDate = DateTime.Now;
                            //db.LoginSessionLog.Update(d);
                            db.SaveChanges();
                        } else {

                            LoginSessionLog n = NewLoginSessionLog(data);
                            db.LoginSessionLog.Add(n);
                            db.SaveChanges();
                            await localStorage.SetItemAsync("loginsession", n.LogInID);
                        }
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;

        }

        public async Task<LoginSet> GetLoginSessionLog() {
            LoginSet loginSet = null;
            try {
                var loginId = await localStorage.GetItemAsync<string>("loginsession");
                DateTime today = DateTime.Now;
                using (GAEntities db = new GAEntities()) {

                    if (string.IsNullOrEmpty(loginId)) {
                        return null;
                    } else {
                        var d = db.LoginSessionLog.Where(o => o.LogInID == loginId && o.IsActive == true && o.ExpiryDate >= today).FirstOrDefault();
                        if (d == null) {
                            return null;
                        } else {
                            loginSet = System.Text.Json.JsonSerializer.Deserialize<LoginSet>(d.Data);

                        }
                    }
                }
            } catch (Exception ex) {
                var e = ex.Message;
            }

            return loginSet;

        }
        public async Task<LoginSet> SetLogOutSessionLog() {
            LoginSet loginSet = null;
            try {
                var loginId = await localStorage.GetItemAsync<string>("loginsession");

                DateTime today = DateTime.Now;
                using (GAEntities db = new GAEntities()) {

                    if (string.IsNullOrEmpty(loginId)) {
                        return null;
                    } else {
                        var d = db.LoginSessionLog.Where(o => o.LogInID == loginId && o.IsActive == true && o.ExpiryDate >= today).FirstOrDefault();
                        if (d != null) {
                            d.IsActive = false;
                            d.ModifiedDate = DateTime.Now;
                            d.ExpiryDate = DateTime.Now;
                            //db.Update(d);
                            db.SaveChanges();
                            await localStorage.RemoveItemAsync("loginsession");
                            return null;

                        }
                    }
                }
            } catch (Exception ex) {
                var e = ex.Message;
            }

            return loginSet;

        }
        #endregion



        #region  login api
        public   LoginCrossReturn CreateCrossAppReq(LoginCrossRequest request) {
            LoginCrossReturn output = new LoginCrossReturn();
            try {
                using (GAEntities db = new GAEntities()) {
                    LogInCrossAppReq n = new LogInCrossAppReq {
                        ReqID = Guid.NewGuid().ToString(),
                        ReqTime = DateTime.Now,
                        AppID = request.AppID,
                        ExpiryTime = DateTime.Now.AddMinutes(60),
                        RComID = request.RComID,
                        Username = request.Username,
                        CallBackUrl = request.BackPage,
                        UseTime = null,
                        ForwardToUrl = request.ToPage
                    };

                    db.LogInCrossAppReq.Add(n);
                    db.SaveChanges();
                    output.Status = "ok";
                    output.ReqID = n.ReqID;
                    output.GoToUrl = Globals.BlazorServer_Front + $"/Login/LoginFromApp/{n.ReqID}";
                    output.Messeage = "";
                }
            } catch (Exception ex) {
                output.Status = "fail";
                if (ex.InnerException != null) {
                    output.Messeage = ex.InnerException.ToString();
                } else {
                    output.Messeage = ex.Message;
                }
            }
            return output;
        }

        public static LogInCrossAppReq GetCrossAppReq(string req_id) {
            LogInCrossAppReq output = new LogInCrossAppReq();
            try {
                using (GAEntities db = new GAEntities()) {
                    DateTime cur_datetime = DateTime.Now;
                    output = db.LogInCrossAppReq.Where(o => o.ReqID == req_id && o.ExpiryTime >= cur_datetime).FirstOrDefault();
                }
            } catch (Exception ex) { }
            return output;
        }

        async public static Task<string> CreateCrossToFrontEnd(string rcom, string com, string username, string to_page) {
            string output = "";
            try {
                LoginCrossRequest data = new LoginCrossRequest();
                data.Username = username;
                data.RComID = rcom;
                data.AppID = "OMS";
                data.ToPage = to_page;
                data.BackPage = "";
                string url = $"{Globals.BlazorServer_Front}/api/AppsLogin";
                HttpClient http = new HttpClient();
                var response = await http.PostAsJsonAsync(url, data);
                var str_json = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() != "OK") {
                    output = "error";
                    return output;
                }
                var result_api = JsonSerializer.Deserialize<LoginCrossReturn>(str_json);
                output = Globals.BlazorServer_Front + $"/Account/MyLogin/LoginFromApp?reqid={result_api.ReqID}";

            } catch (Exception ex) { }
            return output;
        }

        #endregion

    }
}
