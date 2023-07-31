using Dapper;
using Robot.Data.GAUTHEN.DA;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess
{
    public static class UserService
    {

        public class I_FiterSet
        {
            public String RoleID { get; set; }
            public String SearchText { get; set; }
            public bool ShowDisAble { get; set; }
        }

        public class I_UserSet
        {
            public string Action { get; set; }
            public UserInfo User { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
            public List<XMenu> XMenu { get; set; }
            public List<XUserInCompany> XCompany { get; set; }
            public List<XUserInRCom> XRCompany { get; set; }
            public List<XUserInBoard> XBoard { get; set; }
            public List<XUserInGroup> XGroup { get; set; }

        }
        public class XMenu : UserPermission
        {
            public bool X { get; set; }

            public string MenuCode { get; set; }
            public string MenuName { get; set; }
            public bool NeedOpenPermission { get; set; }
            public bool NeedCreatePermission { get; set; }
            public bool NeedEditPermission { get; set; }
            public bool NeedDeletePermission { get; set; }
            public bool NeedPrintPermission { get; set; }
            public int Sort { get; set; }

        }
        public class XUserInRCom : UserInRCom
        {
            public bool X { get; set; }

            public string RCompanyName { get; set; }

        }
        public class XUserInCompany : UserInCompany
        {
            public bool X { get; set; }
            public string RComID { get; set; }
            public string CompanyName { get; set; }
            public string CompanyType { get; set; }
            public string CompanyTypeName { get; set; }
        }


        public class XUserInBoard : UserInBoard
        {
            public bool X { get; set; }
            public string Name { get; set; }

        }

        public class XUserInGroup : UserInGroup
        {
            public bool X { get; set; }
            public string Name { get; set; }

        }
        #region Global var


        public static I_UserSet DocSet { get { return (I_UserSet)HttpContext.Current.Session["userset_docset"]; } set { HttpContext.Current.Session["userset_docset"] = value; } }
        public static I_FiterSet FilterSet { get { return (I_FiterSet)HttpContext.Current.Session["user_filterset"]; } set { HttpContext.Current.Session["user_filterset"] = value; } }

        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        #endregion

        #region Query
        public static I_UserSet GetDocSetByID(string username)
        {
            if (DocSet == null)
            {
                NewTransaction();
            }
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    DocSet.User = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();

                    DocSet.XMenu = ListMenu(username);
                    DocSet.XCompany = ListCompany(username);
                    DocSet.XRCompany = ListRCompany(username);
                    DocSet.XBoard = ListBoard(username);
                    DocSet.XGroup = ListGroup(username);
                    DocSet.Log = DocSet.Log = TransactionInfoService.ListLogByDocID(username);

                }
            }
            catch (Exception ex)
            {
                DocSet.OutputAction.Result = "fail";
                if (ex.InnerException != null)
                {
                    DocSet.OutputAction.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    DocSet.OutputAction.Message1 = ex.Message;
                }
            }
            return DocSet;
        }

        public static UserInfo GetUserInfo(string userID)
        {

            UserInfo result = new UserInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.UserInfo.Where(o => o.Username == userID && o.IsActive).FirstOrDefault();
            }
            return result;
        }

        public static vw_UserInfo GetVievUserInfo(string userID)
        {

            vw_UserInfo result = new vw_UserInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_UserInfo.Where(o => o.Username == userID && o.IsActive).FirstOrDefault();
            }
            return result;
        }

        public static List<XMenu> ListMenu(string username)
        {
            List<XMenu> result = new List<XMenu>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var uimenu = db.UserPermission.Where(o => o.Username == username).ToList();
                    var menu = db.UserMenu.Where(o => o.IsActive).OrderBy(o => o.GroupSort).ToList();
                    foreach (var m in menu)
                    {
                        XMenu n = new XMenu();
                        var q = uimenu.Where(o => o.MenuID == m.MenuID).FirstOrDefault();

                        n.MenuID = m.MenuID;
                        n.MenuCode = m.MenuCode;
                        n.MenuName = m.Name;
                        n.Sort = m.GroupSort;
                        n.Username = username;
                        n.NeedCreatePermission = m.NeedCreatePermission;
                        n.NeedOpenPermission = m.NeedOpenPermission;
                        n.NeedEditPermission = m.NeedEditPermission;
                        n.NeedPrintPermission = m.NeedPrintPermission;
                        n.NeedDeletePermission = m.NeedDeletePermission;
                        n.IsCreate = q == null ? false : q.IsCreate;
                        n.IsOpen = q == null ? false : q.IsOpen;
                        n.IsEdit = q == null ? false : q.IsEdit;
                        n.IsDelete = q == null ? false : q.IsDelete;
                        n.IsPrint = q == null ? false : q.IsPrint;
                        result.Add(n);

                    }

                }
            }
            catch (Exception) { }
            return result;
        }


        public static List<XUserInRCom> ListRCompany(string username)
        {
            List<XUserInRCom> result = new List<XUserInRCom>();
            try
            {

                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.User;
                    var coms = db.CompanyInfo.Where(o => o.IsActive && o.TypeID == "COMPANY").OrderBy(o => o.CompanyID).ToList();
                    var gircoms = db.UserInRCom.Where(o => o.UserName == username).ToList();

                    foreach (var c in coms)
                    {
                        XUserInRCom n = new XUserInRCom();
                        var girc = gircoms.Where(o => o.RComID == c.CompanyID).FirstOrDefault();
                        n.X = girc != null ? true : false;
                        n.UserName = h.Username;
                        n.RComID = c.CompanyID;
                        n.RCompanyName = c.Name1 + " " + c.Name2;

                        result.Add(n);
                    }
                }
            }
            catch (Exception) { }
            return result;
        }
        public static List<XUserInCompany> ListCompany(string username)
        {
            List<XUserInCompany> result = new List<XUserInCompany>();
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.User;
                    var coms = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.IsActive && o.TypeID == "BRANCH").OrderBy(o => o.CompanyID).ToList();
                    var gicoms = db.UserInCompany.Where(o => o.RCompanyID == rcom && o.UserName == username).ToList();

                    foreach (var c in coms)
                    {
                        XUserInCompany n = new XUserInCompany();
                        var gic = gicoms.Where(o => o.CompanyID == c.CompanyID).FirstOrDefault();
                        n.UserName = h.Username;
                        n.RCompanyID = c.RCompanyID;
                        n.X = gic != null ? true : false;
                        n.CompanyName = c.Name1 + c.Name2;
                        n.CompanyType = c.TypeID;
                        n.CompanyTypeName = c.TypeID;
                        n.CompanyID = c.CompanyID;
                        n.IsActive = true;
                        result.Add(n);
                    }
                }
            }
            catch (Exception) { }
            return result;
        }
        public static List<XUserInGroup> ListGroup(string username)
        {
            List<XUserInGroup> result = new List<XUserInGroup>();
            try
            {
                List<string> groupExclude = new List<string> { "SUPERMAN" };
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.User;
                    var group = db.UserGroupInfo.Where(o => o.RComID == rcom
                                                   && !groupExclude.Contains(o.UserGroupID)
                                                   && o.IsActive).ToList();
                    var userin_group = db.UserInGroup.Where(o => o.RComID == rcom && o.UserName == username && o.IsActive).ToList();

                    foreach (var c in group)
                    {
                        XUserInGroup n = new XUserInGroup();
                        var uig = userin_group.Where(o => o.UserGroupID == c.UserGroupID).FirstOrDefault();
                        n.X = uig != null ? true : false;
                        n.UserName = username;
                        n.RComID = c.RComID;

                        n.Name = c.GroupName;
                        n.UserGroupID = c.UserGroupID;
                        n.IsActive = c.IsActive;
                        result.Add(n);
                    }
                }
            }
            catch (Exception) { }
            return result;
        }
        public static List<XUserInBoard> ListBoard(string username)
        {
            List<XUserInBoard> result = new List<XUserInBoard>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany;
            using (GAEntities db = new GAEntities())
            {
                var gib = db.UserInBoard.Where(o => o.Username == username).ToList();
                var boards = db.DashBoard.Where(o => o.IsActive == true).ToList();
                foreach (var c in boards)
                {
                    XUserInBoard n = new XUserInBoard();
                    var g_inB = gib.Where(o => o.DashBoardID == c.DashBoardID).FirstOrDefault();
                    n.X = g_inB == null ? false : true;
                    n.Username = username;
                    n.X = g_inB != null ? true : false;
                    n.DashBoardID = c.DashBoardID;
                    n.Name = c.Name;
                    result.Add(n);
                }
            }
            return result;
        }



        #endregion

        #region transaction management


        public static List<vw_UserInfo> ListDoc()
        {
            List<vw_UserInfo> result = new List<vw_UserInfo>();
            var f = FilterSet;
            var rcoms = LoginService.LoginInfo.UserInRCompany;
            var currRCom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                var userInRcom = db.UserInRCom.Where(o => o.RComID.Contains(currRCom)).Select(o=>o.UserName).ToList();
                result = db.vw_UserInfo.Where(o =>
                                       (
                                               o.FirstName.Contains(f.SearchText)
                                               || o.LastName.Contains(f.SearchText)
                                               || o.FirstName.Contains(f.SearchText)
                                               || o.EmpCode.Contains(f.SearchText)
                                               || o.Username.Contains(f.SearchText)
                                               || f.SearchText == ""
                                        )
                                        && userInRcom.Contains(o.Username)
                                           && (o.IsActive == true || f.ShowDisAble == true)
                                            ).OrderByDescending(o => o.CreatedDate).ToList();

            }
            return result;
        }


        #endregion

        #region transaction management

        public static I_BasicResult Save(string action)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            DocSet = CalDocSet(DocSet);
            var h = DocSet.User;

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    if (action == "insert")
                    {
                        result = CheckDup();
                        if (result.Result == "fail")
                        {
                            return result;
                        }
                        db.UserInfo.Add(DocSet.User);
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.Username, TableID = "USERINFO", ParentID = h.Username, TransactionDate = DateTime.Now, Action = "Create user info" });

                    }
                    else
                    {
                        var u = db.UserInfo.Where(o => o.Username == h.Username).FirstOrDefault();
                        u.EmpCode = h.EmpCode;
                        u.FirstName = h.FirstName;
                        u.LastName = h.LastName;
                        u.FirstName_En = h.FirstName_En;
                        u.LastName_En = h.LastName_En;
                        u.NickName = h.NickName;
                        u.Gender = h.Gender;
                        u.DepartmentID = h.DepartmentID;
                        u.PositionID = h.PositionID;
                        u.AddrNo = h.AddrNo;
                        u.AddrMoo = h.AddrMoo;
                        u.AddrTumbon = h.AddrTumbon;
                        u.AddrAmphoe = h.AddrAmphoe;
                        u.AddrProvince = h.AddrProvince;
                        u.AddrPostCode = h.AddrPostCode;
                        u.AddrCountry = h.AddrCountry;
                        u.DefaultCompany = h.DefaultCompany;
                        
                        u.Tel = h.Tel;
                        u.Mobile = h.Mobile;
                        u.Email = h.Email;
                        u.CitizenId = h.CitizenId;
                        u.BookBankNumber = h.BookBankNumber;
                        u.MaritalStatus = h.MaritalStatus;
                        u.Birthdate = h.Birthdate;
                        u.JobStartDate = h.JobStartDate;
                        u.ResignDate = h.ResignDate;
                        u.ApproveBy = h.ApproveBy;
                        u.IsProgramUser = h.IsProgramUser;
                        u.UseTimeStamp = h.UseTimeStamp;
                        u.IsNewUser = h.IsNewUser;
                        u.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        u.ModifiedDate = DateTime.Now;
                        u.IsActive = h.IsActive;
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.Username, TableID = "USERINFO", ParentID = h.Username, TransactionDate = DateTime.Now, Action = "Update user info" });
                        var rp = InsertAllPermission(DocSet);

                        if (rp.Result == "fail")
                        {
                            result.Result = "fail";
                            result.Message1 = result.Message1 + " " + rp.Message1;
                        }
                    }
                }



            //  Task.Run(()=>  SyncUser.UpdateUserToGAuthen(""));
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


        private static I_BasicResult CheckDup()
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var h = DocSet.User;
                using (GAEntities db = new GAEntities())
                {
                    var exist = db.UserInfo.Where(o => o.Username == h.Username).FirstOrDefault();
                    if (exist != null)
                    {
                        result.Result = "fail";
                        result.Message1 = "Duplicate user ID";
                        if (exist.IsActive == false)
                        {
                            result.Message1 = h.Username + ": There is already this code in the database. But is disabled for use";
                        }
                    }
                }
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
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        #endregion

        #region New Transaction

        public static void NewFilterSet()
        {
            FilterSet = new I_FiterSet();
            FilterSet.RoleID = "";
            FilterSet.SearchText = "";
            FilterSet.ShowDisAble = false;
        }

        public static UserInfo NewUser()
        {
            UserInfo newdata = new UserInfo();

            newdata.Username = "";
            newdata.Password = "";
            newdata.EmpCode = "";
            newdata.FirstName = "";
            newdata.LastName = "";
            newdata.FullName = "";
            newdata.FirstName_En = "";
            newdata.LastName_En = "";
            newdata.FullName_En = "";
            newdata.NickName = "";
            newdata.Gender = "";
            newdata.DepartmentID = "";
            newdata.PositionID = "";
            newdata.IsProgramUser = false;
            newdata.IsNewUser = true;
            newdata.JobStartDate = null;
            newdata.ResignDate = null;
            newdata.AddrFull = "";
            newdata.AddrNo = "";
            newdata.AddrMoo = "";
            newdata.AddrTumbon = "";
            newdata.AddrAmphoe = "";
            newdata.AddrProvince = "";
            newdata.AddrPostCode = "";
            newdata.AddrCountry = "";
            newdata.Tel = "";
            newdata.Mobile = "";
            newdata.Email = "";
            newdata.Birthdate = null;
            newdata.MaritalStatus = null;
            newdata.CitizenId = null;
            newdata.BookBankNumber = null;
            newdata.ApproveBy = "";
            newdata.IsProgramUser = true;
            newdata.UseTimeStamp = false;
            newdata.ImageProfile = null;
            newdata.LineToken = null;
            newdata.DefaultCompany = null;
            newdata.CreatedBy = LoginService.LoginInfo.CurrentUser;
            newdata.CreatedDate = DateTime.Now;
            newdata.ModifiedBy = "";
            newdata.ModifiedDate = null;
            newdata.IsActive = true;

            return newdata;
        }

        public static void NewTransaction()
        {
            DocSet = new I_UserSet();
            DocSet.Action = "";
            DocSet.User = NewUser();

            DocSet.XMenu = new List<XMenu>();
            DocSet.XCompany = new List<XUserInCompany>();
            DocSet.XRCompany = new List<XUserInRCom>();
            DocSet.XBoard = new List<XUserInBoard>();
            DocSet.XGroup = new List<XUserInGroup>();
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        }



        public static List<UserPermission> ConvertX2Menu(I_UserSet input)
        {
            List<UserPermission> result = new List<UserPermission>();
            foreach (var s in input.XMenu)
            {
                var n = new UserPermission();
                n.Username = s.Username;
                n.MenuID = s.MenuID;
                n.IsOpen = Convert.ToBoolean(s.IsOpen); ;
                n.IsCreate = Convert.ToBoolean(s.IsCreate);
                n.IsEdit = Convert.ToBoolean(s.IsEdit);
                n.IsDelete = Convert.ToBoolean(s.IsDelete);
                n.IsPrint = Convert.ToBoolean(s.IsPrint);
                result.Add(n);
            }
            return result;
        }
        public static List<UserInRCom> ConvertX2RCompany(I_UserSet input)
        {
            List<UserInRCom> result = new List<UserInRCom>();
            foreach (var s in input.XRCompany)
            {
                if (s.X)
                {
                    var n = new UserInRCom();
                    n.RComID = s.RComID;
                    n.UserName = s.UserName;
                    result.Add(n);
                }

            }

            return result;
        }
        public static List<UserInCompany> ConvertX2Company(I_UserSet input)
        {
            List<UserInCompany> result = new List<UserInCompany>();
            foreach (var s in input.XCompany)
            {
                if (s.X)
                {
                    var n = new UserInCompany();
                    n.RCompanyID = s.RCompanyID;
                    n.CompanyID = s.CompanyID;
                    n.UserName = s.UserName;
                    n.IsActive = s.IsActive;
                    result.Add(n);
                }

            }

            return result;
        }
        public static List<UserInBoard> ConvertX2Board(I_UserSet input)
        {
            List<UserInBoard> result = new List<UserInBoard>();
            var h = input.User;
            foreach (var s in input.XBoard)
            {
                var n = new UserInBoard();
                if (s.X)
                {
                    n.Username = h.Username;
                    n.DashBoardID = s.DashBoardID;
                    result.Add(n);
                }
            }
            return result;
        }
        public static List<UserInGroup> ConvertX2Group(I_UserSet input)
        {
            List<UserInGroup> result = new List<UserInGroup>();
            var h = input.User;
            foreach (var s in input.XGroup)
            {
                var n = new UserInGroup();
                if (s.X)
                {
                    n.UserName = s.UserName;
                    n.RComID = s.RComID;
                    n.UserGroupID = s.UserGroupID;
                    n.IsActive = s.IsActive;
                    result.Add(n);
                }
            }
            return result;
        }
        public static void CheckUnCheckCompany(bool check)
        {
            var h = DocSet.User;
            foreach (var sw in DocSet.XCompany)
            {
                sw.X = check;
            }
        }
        public static void CheckUnCheckRCompany(bool check)
        {
            var h = DocSet.User;
            foreach (var sw in DocSet.XRCompany)
            {
                sw.X = check;
            }
        }

        #endregion


        public static I_BasicResult ResetPassword(string username)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    string defaultPass = UserService.GetDefualtPassword();
                    var u = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                    u.IsNewUser = true;
                    u.Password = EncryptService.hashPassword("MD5", defaultPass);

                    db.SaveChanges();
                    result.Message2 = defaultPass;
                }
            }
            catch (Exception ex)
            {
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

        public static I_UserSet CalDocSet(I_UserSet input)
        {
            var h = input.User;
            foreach (var l in input.XBoard)
            {
                /*   l.RComID = h.RComID*/
                ;
            }
            foreach (var l in input.XCompany)
            {
                //l.RComID = h.RComID;
            }
            foreach (var l in input.XMenu)
            {
                //l.RComID = h.RComID;
            }
            return input;
        }



        #region  permission manangement
        public static I_BasicResult InsertMenuToUser(List<UserPermission> user_menu)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = DBDapperService.GetDBConnectFromAppConfig();

            var user = user_menu.FirstOrDefault();
            if (user == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserPermission where  Username=@username";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("Username", user.Username);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in user_menu)
                    {

                        sql = @"insert into  UserPermission  (
                               Username
                              ,MenuID
                              ,IsOpen
                              ,IsCreate
                              ,IsEdit
                              ,IsDelete
                              ,IsPrint
	                          ) values(
                               @Username
                              , @MenuID
                              , @IsOpen
                              , @IsCreate
                              ,@IsEdit
                              , @IsDelete
                              , @IsPrint                             
                         )";
                        conn.Execute(sql, new
                        {
                            @Username = f.Username,
                            @MenuID = f.MenuID,
                            @IsOpen = f.IsOpen,
                            @IsCreate = f.IsCreate,
                            @IsEdit = f.IsEdit,
                            @IsDelete = f.IsDelete,
                            @IsPrint = f.IsPrint
                        });
                    }

                }

            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertRCompanyToUser(List<UserInRCom> uircom)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = DBDapperService.GetDBConnectFromAppConfig();

            var user = uircom.FirstOrDefault();
            if (user == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            string rcom = user.RComID;

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserInRCom where  UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("UserName", user.UserName);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in uircom)
                    {

                        sql = @"insert into  UserInRCom  (
                               RComID
                              ,UserName 
                        
	                          ) values(
                                @RComID
                              , @UserName 
                         )";
                        conn.Execute(sql, new
                        {
                            @RComID = f.RComID,
                            @UserName = f.UserName

                        });
                    }

                }

            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertCompanyToUser(List<UserInCompany> uicom)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = DBDapperService.GetDBConnectFromAppConfig();

            var user = uicom.FirstOrDefault();
            if (user == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            string rcom = user.RCompanyID;

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserInCompany where RCompanyID = @rcom and UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("UserName", user.UserName);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in uicom)
                    {

                        sql = @"insert into  UserInCompany  (
                               UserName
                              ,RCompanyID
                              ,CompanyID 
                              ,IsActive 
	                          ) values(
                                @UserName
                              , @RCompanyID
                              , @CompanyID 
                              , @IsActive
                         )";
                        conn.Execute(sql, new
                        {
                            @UserName = f.UserName,
                            @RCompanyID = f.RCompanyID,
                            @CompanyID = f.CompanyID,
                            @IsActive = f.IsActive

                        });
                    }

                }

            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertGroupToUser(List<UserInGroup> uig)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = DBDapperService.GetDBConnectFromAppConfig();

            var user = uig.FirstOrDefault();
            if (user == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            string rcom = user.RComID;

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserInGroup where RComID = @rcom and UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("UserName", user.UserName);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in uig)
                    {

                        sql = @"insert into  UserInGroup  (
                               RComID
                              ,UserName
                              ,UserGroupID
                              ,IsActive  
	                          ) values(
                                @RComID
                              , @UserName
                              , @UserGroupID
                              , @IsActive 
                         )";
                        conn.Execute(sql, new
                        {
                            @RComID = f.RComID,
                            @UserName = f.UserName,
                            @UserGroupID = f.UserGroupID,
                            @IsActive = f.IsActive,

                        });
                    }

                }

            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        public static I_BasicResult InsertBoardToUser(List<UserInBoard> uiboard)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = DBDapperService.GetDBConnectFromAppConfig();

            var user = uiboard.FirstOrDefault();
            if (user == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }


            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.UserInBoard.RemoveRange(db.UserInBoard.Where(o => o.Username == user.Username));
                    db.UserInBoard.AddRange(uiboard);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertAllPermission(I_UserSet input)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var menu = ConvertX2Menu(input);
                var com = ConvertX2Company(input);
                var rcom = ConvertX2RCompany(input);
                var board = ConvertX2Board(input);
                var group = ConvertX2Group(input);
                var r1 = InsertMenuToUser(menu);
                var r2 = InsertCompanyToUser(com);
                var r3 = InsertBoardToUser(board);
                var r4 = InsertGroupToUser(group);
                var r5 = InsertRCompanyToUser(rcom);

                if (r1.Result == "fail")
                {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r1.Message1;
                }
                if (r2.Result == "fail")
                {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r2.Message1;
                }
                if (r3.Result == "fail")
                {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r3.Message1;
                }
                if (r4.Result == "fail")
                {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r4.Message1;
                }
                if (r5.Result == "fail")
                {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r5.Message1;
                }

            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        #endregion
        #region password
        public static string GetDefualtPassword()
        {
            string result = "";
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    result = db.Config.Where(o => o.ConfigID == "defualtpassword").FirstOrDefault().ValueString1;
                }
            }
            catch (Exception ex) { }
            return result;
        }
        #endregion
    }

}