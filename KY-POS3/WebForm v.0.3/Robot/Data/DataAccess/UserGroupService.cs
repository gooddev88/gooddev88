using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess
{
    public static class UserGroupService
    {

        public class I_FiterSet
        {
            public String RoleID { get; set; }
            public String SearchText { get; set; }
            public bool ShowDisAble { get; set; }
        }

        public class I_userGroupSet
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
        public class XUserGroupInCompany : UserGroupInCompany
        {
            public bool X { get; set; }
            public string RComID { get; set; }
            public string CompanyName { get; set; }
            public string CompanyType { get; set; }
            public string CompanyTypeName { get; set; }
        }
        public class XUserGroupInBoard : UserGroupInBoard
        {
            public bool X { get; set; }
            public string Name { get; set; }
        }
        public class XMenu : UserGroupPermission
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

        #region Global var


        public static I_userGroupSet DocSet { get { return (I_userGroupSet)HttpContext.Current.Session["usergroup_docset"]; } set { HttpContext.Current.Session["usergroup_docset"] = value; } }
        public static I_FiterSet FilterSet { get { return (I_FiterSet)HttpContext.Current.Session["usergroup_filterset"]; } set { HttpContext.Current.Session["usergroup_filterset"] = value; } }
        public static bool IsNewDoc { get { return HttpContext.Current.Session["isnewdoc"] == null ? false : (bool)HttpContext.Current.Session["isnewdoc"]; } set { HttpContext.Current.Session["isnewdoc"] = value; } }
        #endregion


        #region Query
        public static I_userGroupSet GetDocSetByID(string groupId)
        {
            if (DocSet == null)
            {
                NewTransaction();
            }
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            try
            {
                using (GAEntities db = new GAEntities())  {
                    DocSet.Group = db.UserGroupInfo.Where(o => o.UserGroupID == groupId).FirstOrDefault();
                    DocSet.User = db.vw_UserInGroup.Where(o => o.UserGroupId == groupId && o.IsActive && o.IsInGroup == true && o.RComID == rcom).ToList();
                    DocSet.XMenu = ListMenu(groupId);
                    DocSet.XCompany = ListCompany(groupId);
                    DocSet.XBoard = ListBoard(groupId);
                    DocSet.Log = DocSet.Log = TransactionInfoService.ListLogByDocID(groupId);
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



        public static List<XMenu> ListMenu(string groupId)
        {
            List<XMenu> result = new List<XMenu>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                    var uimenu = db.UserGroupPermission.Where(o => o.UserGroupID == groupId && o.RComID == rcom).ToList();
                    var menu = db.UserMenu.Where(o => o.IsActive).OrderBy(o => o.GroupSort).ToList();
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


        public static List<XUserGroupInCompany> ListCompany(string groupId)
        {
            List<XUserGroupInCompany> result = new List<XUserGroupInCompany>();
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                using (GAEntities db = new GAEntities())
                {
                    var h = DocSet.Group;
                    var coms = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.IsActive && o.TypeID == "BRANCH").OrderBy(o => o.CompanyID).ToList();
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

        public static List<XUserGroupInBoard> ListBoard(string groupId)
        {
            List<XUserGroupInBoard> result = new List<XUserGroupInBoard>();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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


        public static List<vw_UserGroupInfo> ListDoc()
        {
            List<vw_UserGroupInfo> result = new List<vw_UserGroupInfo>();
            var f = FilterSet;
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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
        public static I_BasicResult Save(string action)
        {

            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = DocSet.Group;
            DocSet = CalDocSet(DocSet);
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    if (action == "insert")
                    {
                        r = CheckDup();
                        if (r.Result == "fail")
                        {
                            return r;
                        }
                        db.UserGroupInfo.Add(DocSet.Group);
                        db.SaveChanges();

                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.UserGroupID, TableID = "USERGROUPINFO", ParentID = h.UserGroupID, TransactionDate = DateTime.Now, Action = "Create user group" });
                    }

                    if (action == "update")
                    {
                        var qh = db.UserGroupInfo.Where(o => o.UserGroupID == h.UserGroupID && o.RComID == h.RComID).FirstOrDefault();
                        qh.GroupName = h.GroupName;
                        qh.Sort = h.Sort;
                        qh.IsActive = h.IsActive;
                        qh.ModifiedDate = DateTime.Now;
                        qh.ModifiedBy = LoginService.LoginInfo.CurrentUser;
                        db.SaveChanges();

                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = h.UserGroupID, TableID = "USERGROUPINFO", ParentID = h.UserGroupID, TransactionDate = DateTime.Now, Action = "Update user group" });
                        var rp = InsertAllPermission(DocSet);
                        if (rp.Result == "fail")
                        {
                            r.Result = "fail";
                            r.Message1 = r.Message1 + " " + rp.Message1;
                        }
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
            DocSet.OutputAction = r;
            return r;

        }

        public static I_BasicResult AutoAddBoard(string groupId)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            using (GAEntities db = new GAEntities())
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
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
                finally
                {
                    db.Configuration.AutoDetectChangesEnabled = true;
                }
            }
            return result;
        }



        private static I_BasicResult CheckDup()
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                var h = DocSet.Group;
                using (GAEntities db = new GAEntities())
                {
                    var exist = db.UserGroupInfo.Where(o => o.UserGroupID == h.UserGroupID && o.RComID == rcom).FirstOrDefault();
                    if (exist != null)
                    {
                        result.Result = "fail";
                        result.Message1 = "Duplicate user group iD";
                        if (exist.IsActive == false)
                        {
                            result.Message1 = h.UserGroupID + ": There is already this code in the database. But is disabled for use";
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
        public static I_BasicResult CheckDupSort()
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
                var h = DocSet.Group;
                using (GAEntities db = new GAEntities())
                {
                    var exist = db.UserGroupInfo.Where(o => o.Sort == h.Sort && o.UserGroupID != h.UserGroupID && o.RComID == rcom).FirstOrDefault();
                    if (exist != null)
                    {
                        result.Result = "fail";
                        result.Message1 = "Group sort " + h.Sort + " duplicate in database.";
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

        public static UserGroupInfo NewUsergroup()
        {
            UserGroupInfo n = new UserGroupInfo();
            n.RComID = "";
            n.UserGroupID = "";
            n.GroupName = "";
            n.LineToken = "";
            n.Sort = GetNextSort();
            n.CreatedBy = LoginService.LoginInfo.CurrentUser;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }
        public static UserGroupInDocStep NewDocStep()
        {
            UserGroupInDocStep n = new UserGroupInDocStep();
            n.UserGroupID = "";
            n.StepID = "";
            n.StepName = "";
            n.IsActive = true;
            return n;
        }
        public static int GetNextSort()
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

        public static void NewTransaction()
        {
            DocSet = new I_userGroupSet();

            DocSet.Action = "";
            DocSet.Group = NewUsergroup();
            DocSet.User = new List<vw_UserInGroup>();
            DocSet.XMenu = new List<XMenu>();
            DocSet.XCompany = new List<XUserGroupInCompany>();
            DocSet.XBoard = new List<XUserGroupInBoard>();
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        }


        public static void Convert2SaveInDocStep()
        {
            DocSet = new I_userGroupSet();
            DocSet.Action = "";
            DocSet.Group = NewUsergroup();
            DocSet.User = new List<vw_UserInGroup>();
            DocSet.XMenu = new List<XMenu>();
            DocSet.XCompany = new List<XUserGroupInCompany>();
            DocSet.XBoard = new List<XUserGroupInBoard>();
            DocSet.Log = new List<TransactionLog>();
            DocSet.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        }
        public static List<UserGroupPermission> ConvertX2Menu(I_userGroupSet input)
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

        public static List<UserGroupInCompany> ConvertX2Company(I_userGroupSet input)
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
        public static List<UserGroupInBoard> ConvertX2Board(I_userGroupSet input)
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
        public static I_BasicResult InsertAllPermission(I_userGroupSet input)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                var menu = ConvertX2Menu(input);
                var com = ConvertX2Company(input);
                var board = ConvertX2Board(input);
                var r1 = InsertMenuToGroup(menu);
                var r2 = InsertCompanyToGroup(com);
                var r3 = InsertBoardToGroup(board);
                var r4 = AutoAddBoard(input.Group.UserGroupID);
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
        public static void CheckUnCheckCompany(bool check)
        {
            var h = DocSet.Group;
            foreach (var sw in DocSet.XCompany)
            {
                sw.X = check;
            }
        }
        public static I_userGroupSet CalDocSet(I_userGroupSet input)
        {
            var h = input.Group;
            foreach (var l in input.XBoard)
            {
                l.RComID = h.RComID;
            }
            foreach (var l in input.XCompany)
            {
                l.RComID = h.RComID;
            }
            foreach (var l in input.XMenu)
            {
                l.RComID = h.RComID;
            }
            return input;
        }
        #endregion

        #region  permission manangement
        public static I_BasicResult InsertMenuToGroup(List<UserGroupPermission> group_menu)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = DBDapperService.GetDBConnectFromAppConfig();

            var groupid = group_menu.FirstOrDefault();
            if (groupid == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            string rcom = groupid.RComID;
            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserGroupPermission where RComID = @rcom and UserGroupID=@groupid";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("groupid", groupid.UserGroupID);
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
        public static I_BasicResult InsertCompanyToGroup(List<UserGroupInCompany> ugicom)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string strCon = DBDapperService.GetDBConnectFromAppConfig();

            var groupid = ugicom.FirstOrDefault();
            if (groupid == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            string rcom = groupid.RCompanyID;

            try
            {

                using (SqlConnection conn = new SqlConnection(strCon))
                {
                    conn.Open();
                    string sql = "delete from UserGroupInCompany where RCompanyID = @rcom and UserGroupID=@groupid";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("groupid", groupid.UserGroupID);
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


        public static I_BasicResult InsertBoardToGroup(List<UserGroupInBoard> giboard)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var group = giboard.FirstOrDefault();
            if (group == null)
            {
                //r.Result = "fail";
                //r.Message1 = "Menu not found";
                return r;
            }
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.UserGroupInBoard.RemoveRange(db.UserGroupInBoard.Where(o => o.UserGroupID == group.UserGroupID && o.RComID == group.RComID));
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


        public static I_BasicResult CreateDefaultGrouopForAllRCom()
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var rcoms = db.CompanyInfo.Where(o => o.TypeID == "COMPANY" && o.IsActive).ToList();
                    var menuInfo = db.UserMenu.Where(o => o.IsActive).OrderBy(o => o.MenuID).ToList();
                    foreach (var c in rcoms)
                    {//สร้าง group supperman ให้กับทุก Rcom
                        var ugroup = UserGroupService.NewUsergroup();
                        ugroup.RComID = c.CompanyID;
                        ugroup.UserGroupID = "SUPERMAN";
                        ugroup.GroupName = "Superman";
                        db.UserGroupInfo.RemoveRange(db.UserGroupInfo.Where(o => o.RComID == c.CompanyID && o.UserGroupID == "SUPERMAN"));
                        db.UserGroupInfo.Add(ugroup);
                        db.SaveChanges();
                        List<UserGroupPermission> gMenu = new List<UserGroupPermission>();
                        foreach (var m in menuInfo)
                        {
                            UserGroupPermission n = new UserGroupPermission();
                            n.UserGroupID = ugroup.UserGroupID;
                            n.RComID = ugroup.RComID;
                            n.MenuID = m.MenuID;
                            n.IsCreate = m.NeedCreatePermission;
                            n.IsEdit = m.NeedEditPermission;
                            n.IsDelete = m.NeedDeletePermission;
                            n.IsPrint = m.NeedPrintPermission;
                            gMenu.Add(n);
                        }
                        var xx = InsertMenuToGroup(gMenu);//insert usergroup menu
                        var com = db.CompanyInfo.Where(o => o.RCompanyID == c.RCompanyID && o.IsActive).ToList();
                        List<UserGroupInCompany> gCom = new List<UserGroupInCompany>();
                        foreach (var cs in com)
                        {
                            UserGroupInCompany n = new UserGroupInCompany();
                            n.CompanyID = cs.CompanyID;
                            n.RCompanyID = ugroup.RComID;
                            n.UserGroupID = ugroup.UserGroupID;
                            n.IsActive = true;

                            gCom.Add(n);
                        }
                        var yy = InsertCompanyToGroup(gCom);//insert company to usergroup
                        //insert board to group
                        var boards = db.DashBoard.Where(o => o.IsActive).ToList();
                        List<UserGroupInBoard> uBoards = new List<UserGroupInBoard>();
                        foreach (var b in boards)
                        {
                            UserGroupInBoard n = new UserGroupInBoard();
                            n.DashBoardID = b.DashBoardID;
                            n.RComID = ugroup.RComID;
                            n.UserGroupID = ugroup.UserGroupID;
                            uBoards.Add(n);
                        }
                        var zz = InsertBoardToGroup(uBoards);

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
        #endregion
    }
}