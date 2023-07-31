using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA.User {
    public class UserGroupService
    {

        IDRuunerService idr;
        LogInService login;

        public I_UserGroupSet DocSet { get; set; }
        public UserGroupService(IDRuunerService _idr, LogInService _login)
        {

            idr = _idr;
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

        public class I_UserGroupSet
        {
            public string Action { get; set; }
            public UserGroupInfo Group { get; set; }

            public List<vw_UserInGroup> User { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
            public List<XMenu> XMenu { get; set; }
            public List<XUserGroupInCompany> XCompany { get; set; }
            public List<XUserGroupInBoard> XBoard { get; set; }
        }
        public class XMenu : UserGroupPermission
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
        public class XUserGroupInCompany : UserGroupInCompany
        {
            public bool isChecked { get; set; } = false;
            public bool X { get; set; } = false;
            public string RComID { get; set; }
            public string CompanyName { get; set; }
            public string CompanyType { get; set; }
            public string CompanyTypeName { get; set; }
        }
        public class XUserGroupInBoard : UserGroupInBoard
        {
            public bool isChecked { get; set; } = false;
            public bool X { get; set; } = false;
            public string Name { get; set; }
        }

        #endregion

        #region Query
        public I_UserGroupSet GetDocSet(string docId)
        {
            I_UserGroupSet n = NewTransaction();
            using (GAEntities db = new GAEntities())
            {
                n.Group = db.UserGroupInfo.Where(o => o.UserGroupID == docId).AsNoTracking().FirstOrDefault();

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


        public List<XMenu> ListMenu(string groupId)
        {
            List<XMenu> result = new List<XMenu>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
                    var uimenu = db.UserGroupPermission.Where(o => o.UserGroupID == groupId && o.RComID == rcom).ToList();
                    var menu = db.UserMenu.Where(o => o.IsActive == true).OrderBy(o => o.GroupSort).ToList();
                    foreach (var m in menu)
                    {
                        XMenu n = new XMenu();
                        var q = uimenu.Where(o => o.MenuID == m.MenuID).FirstOrDefault();

                        n.MenuID = m.MenuID;
                        n.MenuCode = m.MenuCode;
                        n.MenuName = m.Name;
                        n.Sort = m.GroupSort;
                        n.UserGroupID = groupId;
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


        public List<XUserGroupInCompany> ListCompany(string groupId)
        {
            List<XUserGroupInCompany> result = new List<XUserGroupInCompany>();
            try
            {
                string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.Group;
                    var coms = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.IsActive == true && o.TypeID == "BRANCH").OrderBy(o => o.CompanyID).ToList();
                    var gicoms = db.UserGroupInCompany.Where(o => o.RCompanyID == rcom && o.UserGroupID == groupId).ToList();

                    foreach (var c in coms)
                    {
                        XUserGroupInCompany n = new XUserGroupInCompany();
                        var gic = gicoms.Where(o => o.CompanyID == c.CompanyID).FirstOrDefault();
                        n.UserGroupID = h.UserGroupID;
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

        public List<XUserGroupInBoard> ListBoard(string groupId)
        {
            List<XUserGroupInBoard> result = new List<XUserGroupInBoard>();
            string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                var gib = db.UserGroupInBoard.Where(o => o.UserGroupID == groupId && o.RComID == rcom).ToList();
                var boards = db.DashBoard.Where(o => o.IsActive == true).ToList();
                foreach (var c in boards)
                {
                    XUserGroupInBoard n = new XUserGroupInBoard();
                    var g_inB = gib.Where(o => o.DashBoardID == c.DashBoardID).FirstOrDefault();
                    n.X = g_inB == null ? false : true;
                    n.RComID = rcom;
                    n.X = g_inB != null ? true : false;
                    n.DashBoardID = c.DashBoardID;
                    n.Name = c.Name;
                    n.UserGroupID = groupId;
                    result.Add(n);
                }
            }
            return result;
        }


        public List<vw_UserGroupInfo> ListDoc(I_FiterSet Fiter, string Search)
        {
            List<vw_UserGroupInfo> result = new List<vw_UserGroupInfo>();
            var f = Fiter;

            string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_UserGroupInfo.Where(o =>
                                        (o.GroupName.Contains(f.SearchText)
                                            || o.UserGroupID.Contains(f.SearchText)
                                            || f.SearchText == ""
                                            )
                                            && o.RComID == rcom
                                            && (o.IsActive == true || f.ShowDisAble == true)
                                            ).OrderByDescending(o => o.Sort).ToList();

            }
            return result;
        }

        #endregion

        #region transaction management

        public I_BasicResult Save(I_UserGroupSet doc)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            doc = CalDocSet(doc);
            var h = doc.Group;

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var u = db.UserGroupInfo.Where(o => o.UserGroupID == h.UserGroupID).FirstOrDefault();
                    if (u ==null)
                    {
                        db.UserGroupInfo.Add(doc.Group);
                        db.SaveChanges(); 
                    }
                    else   { 
                        u.GroupName = h.GroupName;
                        u.Sort = h.Sort;
                        u.ModifiedBy = login.LoginInfo.CurrentUser;
                        u.ModifiedDate = DateTime.Now;
                        u.IsActive = true;
                        db.SaveChanges(); 
                    }

                    //AddUser2RCom(h.Username);

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

        public  I_BasicResult DeleteUserGroup(string usergroup) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
     
                using (GAEntities db = new GAEntities()) {
                    var uh = db.UserGroupInfo.Where(o => o.UserGroupID == usergroup).FirstOrDefault();
                    uh.IsActive = false;
                    uh.ModifiedBy = login.LoginInfo.CurrentUser;
                    uh.ModifiedDate = DateTime.Now;
                    db.UserGroupInfo.Update(uh);
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

        public I_BasicResult InsertMenuToGroup(List<UserGroupPermission> group_menu, string groupId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = GetDBConnectFromAppConfig();

            string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserGroupPermission where RComID = @rcom and UserGroupID=@groupid";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("groupid", groupId);
                    var row = conn.Execute(sql, dynamicParameters);
                    foreach (var f in group_menu)
                    {

                        sql = @"insert into  UserGroupPermission  (
                               UserGroupID
                              ,RComID
                              ,MenuID
                              ,IsOpen
                              ,IsCreate
                              ,IsEdit
                              ,IsDelete
                              ,IsPrint
	                          ) values(
                               @UserGroupID
                              ,@RComID
                              , @MenuID
                              , @IsOpen
                              , @IsCreate
                              , @IsEdit
                              , @IsDelete
                              , @IsPrint                             
                         )";
                        conn.Execute(sql, new
                        {
                            @UserGroupID = f.UserGroupID,
                            @RComID = f.RComID,
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
        public I_BasicResult InsertCompanyToGroup(List<UserGroupInCompany> ugicom, string groupid)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = GetDBConnectFromAppConfig();

            string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserGroupInCompany where RCompanyID = @rcom and UserGroupID=@groupid";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("groupid", groupid);
                    var rd = conn.Execute(sql, dynamicParameters);
                    foreach (var f in ugicom)
                    {

                        sql = @"insert into  UserGroupInCompany  (
                               RCompanyID
                              ,CompanyID
                              ,UserGroupID
                              ,IsActive 
	                          ) values(
                                @RCompanyID
                              , @CompanyID
                              , @UserGroupID
                              , @IsActive                           
                         )";
                        conn.Execute(sql, new
                        {
                            @RCompanyID = f.RCompanyID,
                            @CompanyID = f.CompanyID,
                            @UserGroupID = f.UserGroupID,
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


        public I_BasicResult InsertBoardToGroup(List<UserGroupInBoard> giboard, string groupId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.UserGroupInBoard.RemoveRange(db.UserGroupInBoard.Where(o => o.UserGroupID == groupId && o.RComID == rcom));
                    db.UserGroupInBoard.AddRange(giboard);
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

        public I_BasicResult AutoAddBoard(string groupId)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            using (GAEntities db = new GAEntities())
            {
                string rcom = login.LoginInfo.CurrentRootCompany.CompanyID;
                try
                {
                    var chk_boardExist = db.UserGroupInBoard.Where(o => o.UserGroupID == groupId && o.DashBoardID == "B1013").FirstOrDefault();
                    if (chk_boardExist == null)
                    {
                        UserGroupInBoard n = new UserGroupInBoard();
                        n.DashBoardID = "B1013";
                        n.RComID = rcom;
                        n.UserGroupID = groupId;
                        db.UserGroupInBoard.Add(n);
                        db.SaveChanges();
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

            }
            return result;
        }


        public static string GetDBConnectFromAppConfig()
        {
            var strCon = Globals.GAEntitiesConn;
            return strCon;

        }

        public I_BasicResult InsertAllPermission(I_UserGroupSet input)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var menu = ConvertX2Menu(input);
                var com = ConvertX2Company(input);
                var board = ConvertX2Board(input);
                var r1 = InsertMenuToGroup(menu, input.Group.UserGroupID);
                var r2 = InsertCompanyToGroup(com, input.Group.UserGroupID);
                var r3 = InsertBoardToGroup(board, input.Group.UserGroupID);
                //var r4 = AutoAddBoard(input.Group.UserGroupID);
                //if (r1.Result == "fail")
                //{
                //    r.Result = "fail";
                //    r.Message1 = r.Message1 + " " + r1.Message1;
                //}
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
                //if (r4.Result == "fail")
                //{
                //    r.Result = "fail";
                //    r.Message1 = r.Message1 + " " + r4.Message1;
                //}
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

        public UserGroupInfo NewUsergroup()
        {
            UserGroupInfo n = new UserGroupInfo();
            n.RComID = "";
            n.UserGroupID = "";
            n.GroupName = "";
            n.LineToken = "";
            n.Sort = GetNextSort();
            n.CreatedBy = login.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public int GetNextSort()
        {
            int result = 1;
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var q = db.UserGroupInfo.OrderByDescending(o => o.Sort).FirstOrDefault();
                    if (q != null)
                    {
                        result = q.Sort + 1;
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public I_UserGroupSet NewTransaction()
        {
            I_UserGroupSet n = new I_UserGroupSet();
            n.Group = NewUsergroup();
            n.User = new List<vw_UserInGroup>();
            n.XMenu = new List<XMenu>();
            n.XCompany = new List<XUserGroupInCompany>();
            n.XBoard = new List<XUserGroupInBoard>();
            n.Log = new List<TransactionLog>();
            return n;
        }

        public List<UserGroupPermission> ConvertX2Menu(I_UserGroupSet input)
        {
            List<UserGroupPermission> result = new List<UserGroupPermission>();
            foreach (var s in input.XMenu)
            {
                var n = new UserGroupPermission();
                n.UserGroupID = s.UserGroupID;
                n.RComID = s.RComID;
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

        public List<UserGroupInCompany> ConvertX2Company(I_UserGroupSet input)
        {
            List<UserGroupInCompany> result = new List<UserGroupInCompany>();
            foreach (var s in input.XCompany)
            {
                if (s.X)
                {
                    var n = new UserGroupInCompany();
                    n.RCompanyID = s.RCompanyID;
                    n.CompanyID = s.CompanyID;
                    n.UserGroupID = s.UserGroupID;
                    n.IsActive = s.IsActive;
                    result.Add(n);
                }

            }

            return result;
        }
        public List<UserGroupInBoard> ConvertX2Board(I_UserGroupSet input)
        {
            List<UserGroupInBoard> result = new List<UserGroupInBoard>();
            var h = input.Group;
            foreach (var s in input.XBoard)
            {
                var n = new UserGroupInBoard();
                if (s.X)
                {
                    n.UserGroupID = h.UserGroupID;
                    n.RComID = s.RComID;
                    n.UserGroupID = s.UserGroupID;
                    n.DashBoardID = s.DashBoardID;
                    result.Add(n);
                }
            }
            return result;
        }

        #endregion

        public static I_UserGroupSet CalDocSet(I_UserGroupSet input)
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


    }
}
