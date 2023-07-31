using Dapper;
using Microsoft.AspNetCore.Http;

using Robot.Helper.Hash;
using Microsoft.EntityFrameworkCore;
using Robot.Data.DA.Login;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq; 
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.USER {
    public class UserService {

        public static string sessionActiveId = "activeuserid";
        LogInService login;

        public UserSet DocSet { get; set; }
        public UserService(  LogInService _login)
        {

        
            login = _login;
            DocSet = NewTransaction();
        }

        #region Class
        public class I_FiterSet
        {
            public String RoleID { get; set; }
            public String SearchText { get; set; }
            public bool ShowDisAble { get; set; }
        }

        public class UserSet
        {
            public string Action { get; set; }
            public UserInfo User { get; set; }
            public List<TransactionLog> Log { get; set; }
            public List<XMenu> XMenu { get; set; }
            public List<XUserInCompany> XCompany { get; set; }
            public List<XUserInRCom> XRCompany { get; set; }
            public List<XUserInBoard> XBoard { get; set; }
            public List<XUserInGroup> XGroup { get; set; }

        }
        public class XMenu : UserPermission
        {
            public bool isChecked { get; set; } = false;
            public bool X { get; set; } = false;
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
            public bool isChecked { get; set; } = false;
            public bool X { get; set; } = false;

            public string RCompanyName { get; set; }

        }
        public class XUserInCompany : UserInCompany
        {
            public bool isChecked { get; set; } = false;
            public bool X { get; set; } = false;
            public string RComID { get; set; }
            public string CompanyName { get; set; }
            public string CompanyType { get; set; }
            public string CompanyTypeName { get; set; }
        }


        public class XUserInBoard : UserInBoard
        {
            public bool isChecked { get; set; } = false;
            public bool X { get; set; } = false;
            public string Name { get; set; }

        }

        public class XUserInGroup : UserInGroup
        {
            public bool isChecked { get; set; } = false;
            public bool X { get; set; } = false;
            public string Name { get; set; }

        }

        #endregion

        #region Query
        public UserSet GetDocSet(string docId)
        {
            UserSet n = NewTransaction();
            using (GAEntities db = new GAEntities())
            {
                n.User = db.UserInfo.Where(o => o.Username == docId).AsNoTracking().FirstOrDefault();

                //n.XMenu = ListMenu(username);
                //n.XCompany = ListCompany(username);
                //n.XRCompany = ListRCompany(username);
                //n.XBoard = ListBoard(username);
                //n.XGroup = ListGroup(username);

                n.Log = db.TransactionLog.Where(o => o.TransactionID == docId).AsNoTracking().ToList();
                return n;
            }

        }

        public UserInfo GetUserInfo(string userID)
        {
            UserInfo result = new UserInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.UserInfo.Where(o => o.Username == userID && o.IsActive == true).FirstOrDefault();
            }
            return result;
        }

        public vw_UserInfo GetVievUserInfo(string userID)
        {
            vw_UserInfo result = new vw_UserInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_UserInfo.Where(o => o.Username == userID && o.IsActive == true).FirstOrDefault();
            }
            return result;
        }

        public List<vw_UserInfo> ListDoc(I_FiterSet Fiter,string Search)
        {
            List<vw_UserInfo> result = new List<vw_UserInfo>();
            var f = Fiter;

            using (GAEntities db = new GAEntities())
            {
                result = db.vw_UserInfo.Where(o =>
                                       (
                                               o.FirstName.Contains(Search)
                                               || o.LastName.Contains(Search)
                                               || o.FirstName.Contains(Search)
                                               || o.EmpCode.Contains(Search)
                                               || o.Username.Contains(Search)
                                               || Search == ""
                                        )
                                           && o.IsActive == true 
                                           //|| f.ShowDisAble == true
                                            ).OrderByDescending(o => o.CreatedDate).ToList();

            }
            return result;
        }

        public List<XMenu> ListMenu(string username)
        {
            List<XMenu> result = new List<XMenu>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var uimenu = db.UserPermission.Where(o => o.Username == username).ToList();
                    var menu = db.UserMenu.Where(o => o.IsActive == true).OrderBy(o => o.GroupSort).ToList();
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


        public List<XUserInRCom> ListRCompany(string username)
        {
            List<XUserInRCom> result = new List<XUserInRCom>();
            try
            {

                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.User;
                    var coms = db.CompanyInfo.Where(o => o.IsActive == true && o.TypeID == "COMPANY").OrderBy(o => o.CompanyID).ToList();
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
        public List<XUserInCompany> ListCompany(string username)
        {
            List<XUserInCompany> result = new List<XUserInCompany>();
            try
            {
                var rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.User;
                    var coms = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.IsActive == true && o.TypeID == "BRANCH").OrderBy(o => o.CompanyID).ToList();
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
        public List<XUserInGroup> ListGroup(string username)
        {
            List<XUserInGroup> result = new List<XUserInGroup>();
            try
            {
                var rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.User;
                    var group = db.UserGroupInfo.Where(o => o.RComID == rcom && o.IsActive == true).ToList();
                    var userin_group = db.UserInGroup.Where(o => o.RComID == rcom && o.UserName == username && o.IsActive == true).ToList();

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
        public List<XUserInBoard> ListBoard(string username)
        {
            List<XUserInBoard> result = new List<XUserInBoard>();
            var rcom = login.LoginInfo.CurrentRootCompany;
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

        public I_BasicResult Save( UserSet doc)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            doc = CalDocSet(doc);
            var h = doc.User;

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var u = db.UserInfo.Where(o => o.Username == h.Username).FirstOrDefault();
                    if (u ==null)
                    {
                        //result = CheckDup(doc);
                        //if (result.Result == "fail")
                        //{
                        //    return result;
                        //}
                        db.UserInfo.Add(doc.User);
                        db.SaveChanges(); 
                    }
                    else   { 
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
                        u.ModifiedBy = login.LoginInfo.CurrentUser;
                        u.ModifiedDate = DateTime.Now;
                        u.IsActive = true;
                        db.SaveChanges(); 
                    }

                    AddUser2RCom(h.Username);

                    var rp = InsertAllPermission(DocSet);
                    if (rp.Result == "fail")
                    {
                        result.Result = "fail";
                        result.Message1 = result.Message1 + " " + rp.Message1;
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
                    result.Message1 = ex.Message;
                }
            }

            return result;
        }

        public   I_BasicResult DeleteUser(string username) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
     
                using (GAEntities db = new GAEntities()) {
                    var uh = db.UserInfo.Where(o => o.Username ==username).FirstOrDefault();
                    uh.IsActive = false;
                    uh.ModifiedBy = login.LoginInfo.CurrentUser;
                    uh.ModifiedDate = DateTime.Now;
                    db.UserInfo.Update(uh);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";

                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message.ToString();
                }
            }
            return result;
        }

        public I_BasicResult InsertMenuToUser(List<UserPermission> user_menu, string userId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = GetDBConnectFromAppConfig();

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserPermission where  Username=@username";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("Username", userId);
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
        public I_BasicResult InsertRCompanyToUser(List<UserInRCom> uircom, string userId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = GetDBConnectFromAppConfig();

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserInRCom where  UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("UserName", userId);
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
        public I_BasicResult InsertCompanyToUser(List<UserInCompany> uicom, string userid)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = GetDBConnectFromAppConfig();

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserInCompany where   UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("UserName", userid);
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
        public I_BasicResult InsertGroupToUser(List<UserInGroup> uig, string userId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = GetDBConnectFromAppConfig();

            string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserInGroup where RComID = @rcom and UserName=@UserName";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("UserName", userId);
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

        public I_BasicResult InsertBoardToUser(List<UserInBoard> uiboard, string userId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = GetDBConnectFromAppConfig();

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.UserInBoard.RemoveRange(db.UserInBoard.Where(o => o.Username == userId));
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

        public static string GetDBConnectFromAppConfig()
        {
            var strCon = Globals.GAEntitiesConn;
            return strCon;

        }

        public   I_BasicResult InsertAllPermission(UserSet input)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
           
            try
            {
                var menu = ConvertX2Menu(input);
                var com = ConvertX2Company(input);
                var rcom = ConvertX2RCompany(input);
                var board = ConvertX2Board(input);
                var group = ConvertX2Group(input);
                var r1 = InsertMenuToUser(menu, input.User.Username);
                var r2 = InsertCompanyToUser(com, input.User.Username);
                var r3 = InsertBoardToUser(board, input.User.Username);
                var r4 = InsertGroupToUser(group, input.User.Username);
                var r5 = InsertRCompanyToUser(rcom, input.User.Username);

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

        #region New Transaction

        public static I_FiterSet NewFilterSet()
        {
            I_FiterSet filterset = new I_FiterSet();

            filterset.RoleID = "";
            filterset.SearchText = "";
            filterset.ShowDisAble = false;

            return filterset;
        }

        public UserInfo NewUser()
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
            newdata.JobLevel = "";
            newdata.JobType = "";
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
            newdata.JwtToken = null;
            newdata.JwtRefreshToken = null;
            newdata.JwtTokenExpiryDate = null;
       
        newdata.DefaultCompany = null;
            newdata.CreatedBy = login.LoginInfo.CurrentUser;
            newdata.CreatedDate = DateTime.Now;
            newdata.ModifiedBy = "";
            newdata.ModifiedDate = null;
            newdata.IsActive = true;

            return newdata;
        }

        public UserSet NewTransaction()
        {
            UserSet n = new UserSet();
            n.User = NewUser();
            n.XMenu = new List<XMenu>();
            n.XCompany = new List<XUserInCompany>();
            n.XRCompany = new List<XUserInRCom>();
            n.XBoard = new List<XUserInBoard>();
            n.XGroup = new List<XUserInGroup>();
            n.Log = new List<TransactionLog>();
            return n;
        }

        public  List<UserPermission> ConvertX2Menu(UserSet input)
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

        public List<UserInRCom> ConvertX2RCompany(UserSet input)
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
        public List<UserInCompany> ConvertX2Company(UserSet input)
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
        public List<UserInBoard> ConvertX2Board(UserSet input)
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
        public List<UserInGroup> ConvertX2Group(UserSet input)
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
        public List<XUserInCompany> CheckUnCheckCompany(bool check, UserSet input)
        {
            List<XUserInCompany> result = new List<XUserInCompany>();
            foreach (var sw in input.XCompany)
            {
                sw.X = check;
            }

            return input.XCompany;
        }
        public List<XUserInCompany> CheckUnCheckRCompany(bool check, UserSet input)
        {
            List<XUserInCompany> result = new List<XUserInCompany>();
            foreach (var sw in input.XCompany)
            {
                sw.X = check;
            }

            return input.XCompany;
        }

        #endregion

        public static UserSet CalDocSet(UserSet input)
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

        #region User In Rcom
        public I_BasicResult AddUser2RCom(string username)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var rcom1 = db.UserInRCom.Where(o => o.UserName == username && o.RComID == "NDW").FirstOrDefault();
                    if (rcom1 == null)
                    {
                        var n = new UserInRCom { RComID = "NDW", UserName = username };
                        db.UserInRCom.Add(n);
                        db.SaveChanges();
                    }
                    var com1 = db.UserInCompany.Where(o => o.UserName == username && o.CompanyID == "VACCINE" && o.RCompanyID == "NDW").FirstOrDefault();

                    if (com1 == null)
                    {
                        var y = new UserInCompany { CompanyID = "VACCINE", UserName = username,RCompanyID="NDW" ,IsActive=true };
                        db.UserInCompany.Add(y);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)  {
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                }
                else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        #endregion
        #region password

        public I_BasicResult ResetPassword(string username)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    string defaultPass = GetDefualtPassword();
                    var u = db.UserInfo.Where(o => o.Username == username).FirstOrDefault();
                    u.IsNewUser = true;
                    u.Password = Hash.hashPassword("MD5", defaultPass);

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

        public string GetDefualtPassword()
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
