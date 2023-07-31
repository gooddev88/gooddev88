using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Account.Authen.DA
{
    public class LogInCenterService
    {
        public class LoginSet
        {
            public String CurrentUser { get; set; }
            public UserInfo CurrentUserInfo { get; set; }
            // public String CurrentUserFullname { get; set; }
            public CompanyInfo CurrentRootCompany { get; set; }
            //public String CurrentRootCompanyName { get; set; }
            public CompanyInfo CurrentCompany { get; set; }
          //  public string CurrentCompanyName { get; set; }
            public DateTime CurrentTransactionDate { get; set; }
            public List<String> UserInCompany { get; set; }
            public List<String> UserInRCompany { get; set; }
            public List<vw_PermissionInMenu> UserInMenu { get; set; }
            public List<vw_PermissionInBoard> UserInBoard { get; set; }
            public vw_PermissionInBoard CurrentBoard { get; set; }
            public List<String> UserInDocStep { get; set; }
            public List<UserMenu> UserMenu { get; set; }
            // public List<string> UserInCustArea { get; set; }
            // public bool ISCreateNewDocumentStatusSuccess { get; set; }
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
        }



        public static LoginSet Login(string username, string password, string rcom = "")
        {
            //password ="hhhhhHhhhh" คือ login โดยไม่ต้องมี Username (hack mode)
            LoginSet LoginInfo = new LoginSet();
            LoginInfo = NewLoginSet();
            LoginInfo.LoginResult = "ok";
            string encrypt_password = EncryptService.hashPassword("MD5", password);
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    vw_UserInfo query;
                    query = db.vw_UserInfo.Where(o => o.Username == username && o.IsProgramUser && o.IsActive).FirstOrDefault();
                    if (query == null)
                    {//login success  
                        LoginInfo.LoginResult = "fail";
                        LoginInfo.LoginResultInfo = "Incorrect username or password on domain";
                        return LoginInfo;
                    }


                    if (password != "silent")
                    {//ไม่ใช่ hack mode

                        //login by db

                        if (query.Password != encrypt_password)
                        {
                            LoginInfo.LoginResult = "fail";
                            LoginInfo.LoginResultInfo = "Incorrect username or password.";
                            return LoginInfo;
                        }
                    }
                    LoginInfo.UserInRCompany = ListUserInRCompany(query.Username, rcom);
                    var currRcomID = LoginInfo.UserInRCompany.FirstOrDefault();
                    if (currRcomID == null) {
                        LoginInfo.LoginResult = "fail";
                        LoginInfo.LoginResultInfo = "No setup root company";
                        return LoginInfo;
                    }

                    LoginInfo.CurrentRootCompany = CompanyService.GetRCompanyInfo(currRcomID);
                   
                    LoginInfo.UserInCompany = ListUserInCompany(query.Username, currRcomID);
                    //if (LoginInfo.UserInCompany.Count() == 0)
                    //{
                    //    LoginInfo.LoginResult = "fail";
                    //    LoginInfo.LoginResultInfo = "No user setup in company";
                    //    return LoginInfo;
                    //}

                    LoginInfo.CurrentCompany = CompanyService.GetCompanyInfo(currRcomID,LoginInfo.UserInCompany.FirstOrDefault());

                    LoginInfo.LoginResult = "ok";
                        if (query.IsNewUser)
                        {
                            LoginInfo.LoginResult = "new";
                        }

           

                
                    LoginInfo.CurrentUser = query.Username;
                    LoginInfo.CurrentUserInfo = UserService.GetUserInfo(query.Username);

                    LoginInfo.CurrentTransactionDate = DateTime.Now.Date;
                    LoginInfo.UserInMenu = ListUserInMenu(query.Username, currRcomID);
                    LoginInfo.UserInBoard = ListUserInBoard(query.Username);
                    LoginInfo.CurrentBoard = LoginInfo.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();

                    //LoginInfo.UserInDocStep = ListDocStepPermission(query.Username);
                    //LoginInfo.UserInCustArea = ListUserInCustAreaID(query.Username);
                    //LoginInfo.ISCreateNewDocumentStatusSuccess = false;
                    // LoginInfo.UserMenu = ListMenu();
                    //var comInfo = db.CompanyInfo.Where(o => o.CompanyID == LoginInfo.CurrentCompany).FirstOrDefault();
                    //var rcomInfo = db.CompanyInfo.Where(o => o.CompanyID == comInfo.RCompanyID).FirstOrDefault();
                    //LoginInfo.CurrentCompanyName = comInfo.Name1;
                    //LoginInfo.CurrentRootCompanyName = rcomInfo.Name1;
                    LoginInfo.BackgroundImage = "";
                    LoginInfo.CurrentMacNo = "Z";
                    LoginInfo.CurrentVatRate = 7;

                    LoginInfo.AppLogoImage = CompanyService.GetRComLogoThumb(currRcomID);
                   // LoginInfo.AppLogoImage = "../Image/Logo/kylogo.png";

                }
            }
            catch (Exception ex)
            {
                LoginInfo.LoginResult = "fail";

                if (ex.InnerException != null)
                {
                    LoginInfo.LoginResultInfo = ex.InnerException.ToString();
                }
                else
                {
                    LoginInfo.LoginResultInfo = ex.Message.ToString();
                }
                //var log = new TransactionLog { TransactionID = username, ParentID = "", TransactionDate = DateTime.Now, CompanyID = "", ActionType = "LOGIN", Action = "LOGIN FAIL" };
                // TransactionInfoService.SaveLog(log);
            }

            return LoginInfo;
        }

        public static List<string> ListUserInRCompany(string username, string rcom = "")
        {
            List<string> result = new List<string>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    result = db.UserInRCom.Where(o =>
                                                        o.UserName == username
                                                        && (o.RComID == rcom || rcom == "")
                                                        ).Select(o => o.RComID).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static List<string> ListUserInCompany(string username, string rcom)
        {
            List<string> result = new List<string>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var query = db.SP_LIST_USERINCOMPANY(username, rcom);

                    result = query.Select(s => (string)s.CompanyID).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static List<vw_PermissionInMenu> ListUserInMenu(string username ,string rcom)
        {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    result = db.vw_PermissionInMenu.Where(o => o.Username == username && o.RComID== rcom).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static List<vw_PermissionInBoard> ListUserInBoard(string username)
        {
            List<vw_PermissionInBoard> result = new List<vw_PermissionInBoard>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var rr = db.vw_PermissionInBoard.Where(o => o.Username == username).GroupBy(o => new { o.Username, o.DashBoardID, o.BoardPage, o.BoardSort, o.BoardName })
                                                        .Select(g => new { g.Key.Username, g.Key.DashBoardID, g.Key.BoardPage, g.Key.BoardSort, g.Key.BoardName }).ToList();
                    int i = 1;
                    foreach (var r in rr)
                    {
                        vw_PermissionInBoard n = new vw_PermissionInBoard();
                        n.RN = i;
                        n.Username = r.Username;
                        n.DashBoardID = r.DashBoardID;
                        n.BoardSort = r.BoardSort;
                        n.BoardPage = r.BoardPage;
                        n.BoardName = r.BoardName;
                        result.Add(n);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public static List<string> ListUserInCustAreaID(string username)
        {
            List<string> result = new List<string>();

            return result;
        }

        //public static List<UserMenu> ListMenu() {
        //    List<UserMenu> result = new List<UserMenu>();
        //    using (GAEntities db = new GAEntities()) {
        //        result = db.UserMenu.Where(o => o.IsActive).OrderBy(o => o.GroupSort).ThenBy(o => o.SubGroupSort).ToList();
        //    }
        //    return result;
        //}
        public static LoginSet NewLoginSet()
        {
            //    var rcomInfo = CompanyInfoServiceV2.GetRootCom();
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
            n.AppLogoImage = "../../Image/Logo/app_logo.png";
            n.BackgroundImage = "";
            n.LoginResult = "";
            n.LoginResultInfo = "";
            n.PageError = "";
            n.AppID = "";
            n.LogInByApp = "WEB";
            n.RefUserID = "";
            return n;
        }
    }

}