using Blazored.SessionStorage;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Robot.Data.GADB.TT;
using Robot.Data.ML.Login;
using Robot.Helper;
using Robot.Helper.Hash;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class LogInService {
        ISessionStorageService sessionStorage;
        NavigationManager nav;
        Blazored.LocalStorage.ILocalStorageService localStorage;
        public LogInService(ISessionStorageService _sessionStorage, Blazored.LocalStorage.ILocalStorageService _localStorage, NavigationManager _nav) {
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

        public static LoginSet Login(string username, string password, string appid, string rcom = "") {
            //password ="hhhhhHhhhh" คือ login โดยไม่ต้องมี Username (hack mode) 
            // LoginSet LoginInfo = new LoginSet();
            var loginInfo = NewLoginSet();
            loginInfo.LoginResult = "ok";
            string encrypt_password = Hash.hashPassword("MD5", password);
            try {
                using (GAEntities db = new GAEntities()) {

                    var query = db.UserInfo.Where(o => o.Username == username && o.IsProgramUser && o.IsActive == true).FirstOrDefault();
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


                    loginInfo.LoginResult = "ok";
                    if (query.IsNewUser == true) {
                        loginInfo.LoginResult = "new";
                    }

                    loginInfo.CurrentUser = query.Username;
                    loginInfo.CurrentTransactionDate = DateTime.Now.Date;
                    loginInfo.UserInMenu = ListUserInMenu(appid, currRcomID, query.Username);
                    loginInfo.UserInMenuDisplay = ListUserInMenuDisplay(loginInfo.UserInMenu);
                    loginInfo.UserInBoard = ListUserInBoard(query.Username);
                    loginInfo.CurrentBoard = loginInfo.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();

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
        #region  Line login
        public static LoginSet LoginByLineId(string lineId ) {
            LoginSet loginInfo = NewLoginSet();
            loginInfo.LoginResult = "ok";
            using (GAEntities db = new GAEntities()) {
                var lineUser = db.LineLogIn.Where(o => o.IsActive && o.LineID == lineId && o.UserType == "USER").FirstOrDefault();
                if (lineUser == null) {
                    loginInfo.LoginResult = "fail";
                    loginInfo.LoginResultInfo = "You are not register.";
                    return loginInfo;
                } else {
                    lineUser.LatestLogin = DateTime.Now;
                    db.LineLogIn.Update(lineUser);
                    db.SaveChanges();
                }
                loginInfo = Login(lineUser.UserID, "silent", "");
                loginInfo.RefUserID = lineId;
            }
            return loginInfo;
        }
        #endregion


        public LoginCrossReturn CreateCrossAppReq(LoginCrossRequest request) {
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

        async public void SetSessionDocSet(LoginSet data) {
            string json = JsonConvert.SerializeObject(data);
            await sessionStorage.SetItemAsync("loginset", json);
        }
        async public Task<LoginSet> GetSessionDocSet() {
            var strdoc = await sessionStorage.GetItemAsync<string>("loginset");
            return JsonConvert.DeserializeObject<LoginSet>(strdoc);
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
        public string GetRootApp(string pageurl) {
            string rootdomain = nav.BaseUri.ToUpper().ToString().Replace("SALE/", "");
            string subApp = "APP";
            string url = rootdomain + subApp + pageurl;
            return url;
        }
        public static List<vw_PermissionInMenu> ListUserInMenu(string appid, string rcom, string username) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.vw_PermissionInMenu.Where(o => o.Username == username
                                                                //&& (o.AppID == appid || appid == "")
                                                                && !string.IsNullOrEmpty(o.Url)
                                                                && o.RComID==rcom
                                                             
                                                               
                                                                ).ToList();
                   

                }
            } catch (Exception ex) {

            }
         return result;
        }
        public static List<vw_PermissionInMenu> ListUserInMenuDisplay(List<vw_PermissionInMenu> input) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {

                //result = input.GroupBy(x => new { x.IsOpen, x.Username, x.MenuID, x.GroupID, x.TypeID, x.MenuName, x.MenuDesc1, x.MenuDesc2, x.Url, x.Icon, x.GroupSort })
                //                    .Select(r => new vw_PermissionInMenu {
                //                        IsOpen = r.Key.IsOpen,
                //                        Username = r.Key.Username,
                //                        MenuID = r.Key.MenuID,
                //                        GroupID = r.Key.GroupID,
                //                        TypeID = r.Key.TypeID,
                //                        MenuName = r.Key.MenuName,
                //                        MenuDesc1 = r.Key.MenuDesc1,
                //                        MenuDesc2 = r.Key.MenuDesc2,
                //                        Url = r.Key.Url,
                //                        Icon = r.Key.Icon,
                //                        GroupSort = r.Key.GroupSort
                //                    }).ToList();

                result = input.Where(o => o.IsOpen == true).GroupBy(x => new { x.Username, x.MenuID, x.GroupID, x.TypeID, x.MenuName, x.MenuDesc1, x.MenuDesc2, x.Url, x.Icon, x.GroupSort ,x.App})
                         .Select(r => new vw_PermissionInMenu {
                             IsOpen = r.LastOrDefault().IsOpen,
                             Username = r.Key.Username,
                             MenuID = r.Key.MenuID,
                             GroupID = r.Key.GroupID,
                             TypeID = r.Key.TypeID,
                             MenuName = r.Key.MenuName,
                             MenuDesc1 = r.Key.MenuDesc1,
                             MenuDesc2 = r.Key.MenuDesc2,
                             Url = r.Key.Url,
                             Icon = r.Key.Icon,
                             App=r.Key.App,
                             GroupSort = r.Key.GroupSort
                         }).ToList();
             
            } catch (Exception ex) {

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
                    var rr = db.vw_PermissionInBoard.Where(o => o.Username == username && !string.IsNullOrEmpty(o.BoardUrl)).GroupBy(o => new { o.Username, o.DashBoardID, o.BoardPage, o.BoardSort, o.BoardName, o.BoardUrl })
                                                        .Select(g => new { g.Key.Username, g.Key.DashBoardID, g.Key.BoardPage, g.Key.BoardSort, g.Key.BoardName, g.Key.BoardUrl }).OrderBy(o => o.BoardSort).ToList();
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

        public bool CanEdit(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsEdit).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsEdit);
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

        
        public bool CanDelete(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsDelete).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsDelete);
            }
            return result;
        }

        public static UserMenu GetMenuInfo(LoginSet xlogin, string menu) {
            UserMenu result = new UserMenu { MenuID = "", Name = "" };

            var menu_info = xlogin.UserInMenu.Where(o => o.MenuID == menu).FirstOrDefault();

            if (menu_info == null) {
                return result;
            }
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            result.GroupID = menu_info.GroupID;
            return result;
        }
        public static UserMenu GetMenuGroup(LoginSet xlogin, string group) {
            UserMenu result = new UserMenu { MenuID = "", Name = "" };
            var menu_info = xlogin.UserInMenu.Where(o => o.GroupID == group && o.TypeID == "MENUGROUP").FirstOrDefault();
            if (menu_info == null) {
                return result;
            }
            //2103
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            return result;
        }
        public async static Task<List<UserMenu>> ListMenuGroup(LoginSet xlogin ) {
            List<UserMenu> result = new List<UserMenu>();

            var menus = xlogin.UserInMenuDisplay.Where(o =>   o.TypeID == "MENUGROUP" && o.Url != "").OrderBy(o => o.GroupSort).ToList();
            foreach (var m in menus) {
                UserMenu r = new UserMenu();
                r.MenuID = m.MenuID;
                r.Name = m.MenuName;
                r.MenuCode = m.MenuCode;
                r.Desc1 = m.MenuDesc1;
                r.Desc2 = m.MenuDesc2;
                r.Url = m.Url;
                r.Icon = m.Icon;
                result.Add(r);
            }

            return result;
        }
        public async static Task<List<UserMenu>> ListMenuInParent(LoginSet xlogin, string groupId) {
            List<UserMenu> result = new List<UserMenu>();

            var menus = xlogin.UserInMenuDisplay.Where(o => o.GroupID.ToLower() == groupId.ToLower() && o.TypeID != "MENUGROUP" && o.Url!="").OrderBy(o=>o.SubGroupID).ToList();
            foreach (var m in menus) {
                UserMenu r = new UserMenu();
                r.MenuID = m.MenuID;
                r.Name = m.MenuName;
                r.MenuCode = m.MenuCode;
                r.Desc1 = m.MenuDesc1;
                r.Desc2 = m.MenuDesc2;
                if (r.MenuID== "2103") {//2103 ผู้อนุมัติสั่งซื้อ
                    r.Url = String.Format("POSREPORTS/PO/ReportPOSumByStore/{0}/1/web2", xlogin.CurrentRootCompany.CompanyID);
                   // r.Url =  @"POSREPORTS/PO/ReportPOSumByStore/" + xlogin.CurrentRootCompany.CompanyID + @"/1/web2";
                }
                r.Url = m.Url;
                r.Icon = m.Icon;
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



        #region รอลบ
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
                            db.LoginSessionLog.Update(d);
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
                            db.Update(d);
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



      

    }
}
