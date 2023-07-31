using Dapper;
using Microsoft.Data.SqlClient;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Server.Helper;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Login;
using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Login {
    public class LogInService {

        public static LoginSet Login(string username, string password, string appid, string rcom = "", string com = "") {
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
                   
                    var rcom_Info = db.CompanyInfo.Where(o => o.CompanyID == currRcomID && o.TypeID == "COMPANY").FirstOrDefault();
                    loginInfo.CurrentRootCompany = rcom_Info;

                    loginInfo.UserInCompany = ListUserInCompany(query.Username, currRcomID);
                    if (loginInfo.UserInCompany.Count() == 0 && query.IsSuperMan == false) {
                        loginInfo.LoginResult = "fail";
                        loginInfo.LoginResultInfo = "No user setup in company";
                        return loginInfo;
                    }
                    var first_com = loginInfo.UserInCompany.FirstOrDefault();
                    if (com != "") {
                        first_com = com;
                    }
                    var coms = db.CompanyInfo.Where(o => o.CompanyID == first_com && o.RCompanyID == currRcomID).FirstOrDefault();
                    loginInfo.CurrentCompany = coms;
                    loginInfo.LoginResult = "ok";
                    if (query.IsNewUser == true) {
                        loginInfo.LoginResult = "new";
                    }

                    loginInfo.CurrentUser = query.Username;
                    loginInfo.CurrentTransactionDate = DateTime.Now.Date;
                    //loginInfo.UserInMenu = ListUserInMenu(appid,"KY", query.Username);
                    loginInfo.UserInMenu = ListUserInMenu(appid,loginInfo.CurrentRootCompany.CompanyID, query.Username);
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
        #region List Data
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

        public static List<vw_PermissionInMenu> ListUserInMenu(string appid, string rcom, string username) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {
                using (GAEntities db = new GAEntities()) {
                    result = db.vw_PermissionInMenu.Where(o => o.Username == username
                                                                && (o.App == appid || appid == "")
                                                                && o.RComID == rcom
                                                                ).ToList();
                }
            } catch (Exception ex) {

            }
            return result;
        }
        public static List<vw_PermissionInMenu> ListUserInMenuDisplay(List<vw_PermissionInMenu> input) {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try {

                result = input.Where(o => o.IsOpen == true).GroupBy(x => new { x.Username, x.MenuID, x.GroupID, x.TypeID, x.MenuName
                    , x.MenuDesc1, x.MenuDesc2, x.Url, x.Icon, x.GroupSort, x.App , x.IsOpen, x.IsCreate, x.IsEdit, x.IsPrint, x.IsDelete})
                                    .Select(r => new vw_PermissionInMenu {
                                        IsOpen = r.LastOrDefault().IsOpen,
                                        IsCreate = r.Key.IsCreate,
                                        IsEdit = r.Key.IsEdit,
                                        IsPrint = r.Key.IsPrint,
                                        IsDelete = r.Key.IsDelete,
                                        Username = r.Key.Username,
                                        MenuID = r.Key.MenuID,
                                        GroupID = r.Key.GroupID,
                                        TypeID = r.Key.TypeID,
                                        MenuName = r.Key.MenuName,
                                        MenuDesc1 = r.Key.MenuDesc1,
                                        MenuDesc2 = r.Key.MenuDesc2,
                                        Url = r.Key.Url,
                                        Icon = r.Key.Icon,
                                        App = r.Key.App,
                                        GroupSort = r.Key.GroupSort
                                    }).ToList();

            } catch (Exception ex) {

            }
            return result;
        }

        //public string GetFirstBoardUrl() {
        //    string url = "";
        //    if (LoginInfo == null) {
        //        url = "Dashboard/StartBoard";
        //    } else {
        //        if (LoginInfo?.UserInBoard.Count == 0) {
        //            url = "Dashboard/StartBoard";
        //        } else {
        //            LoginInfo.CurrentBoard = LoginInfo?.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();
        //            url = LoginInfo.CurrentBoard.BoardUrl;

        //        }
        //    }
        //    return url;
        //}
        public static List<vw_PermissionInBoard> ListUserInBoard(string username) {
            List<vw_PermissionInBoard> result = new List<vw_PermissionInBoard>();
            try {
                using (GAEntities db = new GAEntities()) {
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
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            return result;
        }
        public async static Task<List<UserMenu>> ListMenuInParent(LoginSet xlogin, string groupId) {
            List<UserMenu> result = new List<UserMenu>();

            var menus = xlogin.UserInMenuDisplay.Where(o => o.GroupID.ToLower() == groupId.ToLower() && o.TypeID != "MENUGROUP").ToList();
            foreach (var m in menus) {
                UserMenu r = new UserMenu();
                if (result.Where(o => o.MenuID == m.MenuID).FirstOrDefault() != null) {
                    continue;
                }
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



        #endregion

        
    }
}
