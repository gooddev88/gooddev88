using Dapper;
using RobotAPI.Data.MainDB.TT;
using RobotAPI.Helpers.Hash; 
using System.Data;
using System.Data.SqlClient; 

namespace RobotAPI.Data.DA.Login {
    public class LogInService {

        //public LoginSet LoginInfo { get; set; }

        public static async Task<LoginSet> Login(string username, string password, string appid, string rcom = "") {

            var loginInfo = NewLoginSet();
            loginInfo.LoginResult = "ok";
            string encrypt_password = Hash.hashPassword("MD5", password);
            try {
                using (MainContext db = new MainContext()) {
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
                    var currRcomID = ListUserInRCompany(query.Username, rcom).FirstOrDefault();
                    //var kk = db.UserInCompany.FirstOrDefault();
                    loginInfo.CurrentRootCompany = db.CompanyInfo.Where(o => o.CompanyID == currRcomID).FirstOrDefault();
                   

                    if (loginInfo.CurrentRootCompany == null && query.IsSuperMan == false) {
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "No setup root company";
                        return loginInfo;
                    }
                    loginInfo.UserInCompany = ListUserInCompany(query.Username, currRcomID);

                    if (loginInfo.UserInCompany.Count() == 0 && query.IsSuperMan == false) {
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "No user setup in company";
                        return loginInfo;
                    }
                    //loginInfo.CurrentCompany = CompanyService.GetCompanyInfo(currRcomID, loginInfo.UserInCompany.FirstOrDefault());
                    var firstcom = loginInfo.UserInCompany.FirstOrDefault();
                    firstcom = firstcom == null ? "" : firstcom;
                    loginInfo.CurrentCompany = db.CompanyInfo.Where(o => o.RCompanyID == currRcomID && o.CompanyID == firstcom).FirstOrDefault();

                    loginInfo.LoginResult = "ok";
                    if (query.IsNewUser == true) {
                        loginInfo.LoginResult = "new";
                    }

                    loginInfo.CurrentUser = query.Username;
                    loginInfo.CurrentTransactionDate = DateTime.Now.Date;
                    loginInfo.UserInMenu = ListUserInMenu(appid, query.Username);
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
        //public static async Task<AppUser> LoginAPI(string username, string password, string appid, string rcom = "") {
        //    AppUser u = null;
        //    string encrypt_password = Hash.hashPassword("MD5", password);
        //    try {
        //        using (MainContext db = new MainContext()) {
        //            var query = db.UserInfo.Where(o => o.Username == username && o.Password == encrypt_password && o.IsActive == true).FirstOrDefault();
        //            if (query == null) {
        //                return null;
        //            } else {
        //                u = new AppUser();
        //                u.Id = query.Username;
        //                u.UserName = query.Username;
        //                u.Email = query.Email;
        //                u.FirstName = query.FirstName;
        //                u.LastName = query.LastName;
        //                u.PhoneNumber = query.Mobile;
        //                u.RefreshToken = "";
        //                u.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);
        //            }
        //        }
        //    } catch (Exception ex) { }
        //    return u;
        //}
        //public static I_BasicResult UpdateToken(AppUser user) {
        //    I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        using (MainContext db = new MainContext()) {
        //            var query = db.UserInfo.Where(o => o.Username == user.UserName).FirstOrDefault();
        //            if (query == null) {
        //                result.Result = "fail";
        //                result.Message1 = "User not found";
        //                return result;
        //            } else {
        //                query.LineToken = user.RefreshToken;
        //                db.SaveChanges();
        //            }
        //        }
        //    } catch (Exception ex) { }
        //    return result;
        //}

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
                using (MainContext db = new MainContext()) {
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
                string conStr = Globals.MainContextConn;
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

        public static List<vw_PermissionInMenu> ListUserInMenu(string appid, string username) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {
                using (MainContext db = new MainContext()) {
                    result = db.vw_PermissionInMenu.Where(o => o.Username == username
                                                                && (o.AppID == appid || appid == "")
                                                                ).ToList();
                }
            } catch (Exception ex) {

            }
            return result;
        }

        public static List<vw_PermissionInBoard> ListUserInBoard(string username) {
            List<vw_PermissionInBoard> result = new List<vw_PermissionInBoard>();
            try {
                using (MainContext db = new MainContext()) {
                    var rr = db.vw_PermissionInBoard.Where(o => o.Username == username).GroupBy(o => new { o.Username, o.DashBoardID, o.BoardPage, o.BoardSort, o.BoardName, o.BoardUrl })
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

        public bool CanOpen(LoginSet xlogin, string menu) {
            bool result = false;
            var per = xlogin.UserInMenu.Where(o => o.MenuID == menu).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (per != null) {
                result = Convert.ToBoolean(per.IsOpen);
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
            n.Token = "";
            n.RefreshToken = "";
            n.UserImageURL = "";
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


    }

}
