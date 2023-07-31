using Robot.Account.Authen.DA;
using Robot.Master.DA;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using static Robot.Account.Authen.DA.LogInCenterService;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess
{

    public static class LoginService
    {

        public static LoginSet LoginInfo { get { return (LoginSet)HttpContext.Current.Session["login_info"]; } set { HttpContext.Current.Session["login_info"] = value; } }
        //public static CompanyRoot ComRoot { get { return (CompanyRoot)HttpContext.Current.Session["comroot"]; } set { HttpContext.Current.Session["comroot"] = value; } }

        public static void Login(string username, string password)
        {
            LoginInfo = LogInCenterService.Login(username, password);
        }
        #region Class
        //public class LoginSet
        //{
        //    public String CurrentUser { get; set; }
        //    public String CurrentRootCompany { get; set; }
        //    public String CurrentUserFullname { get; set; }
        //    public string CurrentCompany { get; set; }
        //    public string CurrentCompanyName { get; set; }
        //    public DateTime CurrentTransactionDate { get; set; }
        //    public List<String> UserInCompany { get; set; }
        //    public List<vw_PermissionInMenu> UserInMenu { get; set; }
        //    public List<vw_PermissionInBoard> UserInBoard { get; set; }
        //    public vw_PermissionInBoard CurrentBoard { get; set; }
        //    public List<String> UserInDocStep { get; set; }
        //    public List<UserMenu> UserMenu { get; set; }
        //    public List<string> UserInCustArea { get; set; }
        //    public bool ISCreateNewDocumentStatusSuccess { get; set; }
        //    public String BackgroundID { get; set; }
        //    public String LoginResult { get; set; }
        //    public String LoginResultInfo { get; set; }
        //    public String PageError { get; set; }
        //    public String AppID { get; set; }
        //    public String LogInByApp { get; set; }
        //    public String RefUserID { get; set; }
        //}

        public const String DashboardMaster = "/DashBoard/DashBoard1011";
        public const String DashboardHOPIMAS = "/DashBoard/DashBoard1012";
        public const String DashboardLOAN = "/DashBoard/DashBoard1014";


        #endregion

        #region manage Login Info
   
        public static I_BasicResult ChangeCurrentCompany(string comId)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var comInfo = CompanyService.GetCompanyInfo(comId);
                var rcomInfo = CompanyService.GetCompanyInfo(comInfo.RCompanyID);
                LoginService.LoginInfo.CurrentRootCompany = comInfo;
                LoginService.LoginInfo.CurrentCompany = rcomInfo;
                
            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message;
                }

            }
            return result;
        }
        public static I_BasicResult ChangeCurrentRCompany(string rcom)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var comInfo = LogInCenterService.Login(LoginService.LoginInfo.CurrentUser, "silent", rcom);

            }
            catch (Exception ex)
            {
                result.Result = "fail";
                if (ex.InnerException != null)
                {
                    result.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    result.Message1 = ex.Message;
                }

            }
            return result;
        }
        public static bool IsSupperman(string rcom,string username) {
            bool r = false;
       
                using (GAEntities db=new GAEntities()) {
                    var sup = db.UserInGroup.Where(o => o.IsActive && o.RComID == rcom && o.UserName == username).FirstOrDefault();
                    if (sup!=null){
                    r = true;
                    }
                }

          
            return r;
        }
 

        #endregion



        public static bool ValidateCredentials(string domain, string username, string password)
        {
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                return context.ValidateCredentials(username, password);
            }
        }
        #region LogOut
        public static void LogOut()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session["login_info"] = null;
            string url = "../../Account/MyLogin/MyLogIn.aspx";
            HttpContext.Current.Response.RedirectPermanent(url);


        }
        #endregion








        // CreateReqToApp
        public static LogInCrossAppReq CreateUserCrossAppReq(string forwardUrl)
        {
            LogInCrossAppReq n = new LogInCrossAppReq();

            try
            {
                //var link = LinkService.GetLinkInfo("uploadfile");
                n = new LogInCrossAppReq
                {
                    ReqID = Guid.NewGuid().ToString(),
                    ReqTime = DateTime.Now,
                    AppID = "KYPOS",
                    ExpiryTime = DateTime.Now.AddMinutes(60),
                    RComID = LoginService.LoginInfo.CurrentRootCompany.CompanyID,
                    Username = LoginService.LoginInfo.CurrentUser,
                    CallBackUrl = "",
                    UseTime = null,
                    ForwardToUrl = forwardUrl

                };

                using (GAEntities db = new GAEntities())
                {
                    db.LogInCrossAppReq.Add(n);
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {

            }

            return n;

        }
    }



}