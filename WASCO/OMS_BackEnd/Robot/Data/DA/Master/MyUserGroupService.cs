using Dapper;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Blazored.SessionStorage;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using static Robot.Data.ML.I_Result;
using Robot.Data.DA.Login;
using static Robot.Data.DA.Master.MyUserService;

namespace Robot.Data.DA.Master {
    public class MyUserGroupService {
        public static string sessionActiveId = "activeusergroupid";
        ISessionStorageService sessionStorage;
        public class I_FiterSet {
            public String RoleID { get; set; }
            public String SearchText { get; set; }
            public bool ShowDisAble { get; set; }
        }

        public class I_userGroupSet {
            public UserGroupInfo Group { get; set; }
            public List<vw_UserInGroup> User { get; set; }
            public List<TransactionLog> Log { get; set; }
            public I_BasicResult OutputAction { get; set; }
            public List<XMenu> XMenu { get; set; }
            public List<XUserGroupInCompany> XCompany { get; set; }
            public List<XUserGroupInLoc> XLoc { get; set; }
            public List<XUserGroupInBoard> XBoard { get; set; }
            public List<XUserInRCom> XRCompany { get; set; }
        }


        public I_userGroupSet DocSet { get; set; }

        public MyUserGroupService(ISessionStorageService _sessionStorage) {
            sessionStorage = _sessionStorage;

        }

        public class SelectOption {
            public string Value { get; set; }
            public string Desc { get; set; }
        }

        public class XUserInRCom : UserInRCom {
            public bool X { get; set; }

            public string RCompanyName { get; set; }
        }

        public class XUserGroupInCompany : UserGroupInCompany {
            public bool X { get; set; }
            public string RComID { get; set; }
            public string CompanyName { get; set; }
            public string CompnayGroup { get; set; }
            public string CompanyType { get; set; }
            public string CompanyTypeName { get; set; }
        }

        public class XUserGroupInLoc : UserGroupInLoc {
            public bool X { get; set; }
            public string Name { get; set; }
            public string RCompanyID { get; set; }

            public string UserGroupID { get; set; }
            public string LocID { get; set; }
        }

        public class XUserGroupInBoard : UserGroupInBoard {
            public bool X { get; set; }
            public string Name { get; set; }
        }
        public class XMenu : UserGroupPermission {
            public bool X { get; set; }

            public string MenuCode { get; set; }
            public string MenuName { get; set; }



            public string MenuDesc1 { get; set; }
            public string MenuDesc2 { get; set; }
            public string MenuTypeID { get; set; }
            public string MenuGroupID { get; set; }
            public int MenuGroupSort { get; set; }
            public int MenuSubGroupSort { get; set; }


            public bool NeedOpenPermission { get; set; }
            public bool NeedCreatePermission { get; set; }
            public bool NeedEditPermission { get; set; }
            public bool NeedDeletePermission { get; set; }
            public bool NeedPrintPermission { get; set; }

            public string CaptionOpenPermission { get; set; }
            public string CaptionCreatePermission { get; set; }
            public string CaptionEditPermission { get; set; }
            public string CaptionDeletePermission { get; set; }
            public string CaptionPrintPermission { get; set; }
            public int Sort { get; set; }


        }

        #region Get&ListData

        public I_userGroupSet GetDocSet(string groupId, string rcom) {
            I_userGroupSet n = new I_userGroupSet();
            using (GAEntities db = new GAEntities()) {
                n.Group = db.UserGroupInfo.Where(o => o.UserGroupID == groupId).FirstOrDefault();
                n.User = db.vw_UserInGroup.Where(o => o.UserGroupId == groupId && o.IsActive && o.IsInGroup == true && o.RComID == rcom).ToList();
                n.XMenu = ListMenu(groupId, rcom);
                n.XCompany = ListCompany(groupId, rcom);
                n.XBoard = ListBoard(groupId, rcom);
                n.XLoc = ListLoc(groupId, rcom);
            }
            return n;
        }

        async public void SetSessionDocSet(I_userGroupSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await sessionStorage.SetItemAsync("usergroupdocset", json);
        }

        async public Task<I_userGroupSet> GetSessionDocSet() {
            var strdoc = await sessionStorage.GetItemAsync<string>("usergroupdocset");
            return JsonConvert.DeserializeObject<I_userGroupSet>(strdoc);
        }

        public static UserGroupInfo GetUserGroupInfo(string UserGroupID) {
            UserGroupInfo result = new UserGroupInfo();
            using (GAEntities db = new GAEntities()) {
                result = db.UserGroupInfo.Where(o => o.UserGroupID == UserGroupID && o.IsActive == true).FirstOrDefault();
            }
            return result;
        }

        public static List<XMenu> ListMenu(string groupId, string rcom) {
            List<XMenu> result = new List<XMenu>();
            try {
                using (GAEntities db = new GAEntities()) {
                    var uimenu = db.UserGroupPermission.Where(o => o.UserGroupID == groupId && o.RComID == rcom).ToList();
                    var menu = db.UserMenu.Where(o => o.IsActive == true).OrderBy(o => o.GroupSort).ThenBy(o => o.SubGroupSort).ToList();
                    foreach (var m in menu) {
                        XMenu n = new XMenu();
                        var q = uimenu.Where(o => o.MenuID == m.MenuID).FirstOrDefault();

                        n.RComID = rcom;
                        n.MenuID = m.MenuID;
                        n.MenuCode = m.MenuCode;
                        n.MenuName = m.Name;
                        n.Sort = m.GroupSort;
                        n.UserGroupID = groupId;

                        n.MenuGroupID = m.GroupID;
                        n.MenuDesc1 = m.Desc1;
                        n.MenuDesc2 = m.Desc2;
                        n.MenuTypeID = m.TypeID;
                        n.MenuGroupSort = m.GroupSort;
                        n.MenuSubGroupSort = m.SubGroupSort;



                        n.NeedCreatePermission = m.NeedCreatePermission;
                        n.NeedOpenPermission = m.NeedOpenPermission;
                        n.NeedEditPermission = m.NeedEditPermission;
                        n.NeedPrintPermission = m.NeedPrintPermission;
                        n.NeedDeletePermission = m.NeedDeletePermission;

                        n.CaptionOpenPermission = m.CaptionOpenPermission;
                        n.CaptionCreatePermission = m.CaptionCreatePermission;
                        n.CaptionEditPermission = m.CaptionEditPermission;
                        n.CaptionDeletePermission = m.CaptionDeletePermission;
                        n.CaptionPrintPermission = m.CaptionPrintPermission;

                        n.IsCreate = q == null ? false : q.IsCreate;
                        n.IsOpen = q == null ? false : q.IsOpen;
                        n.IsEdit = q == null ? false : q.IsEdit;
                        n.IsDelete = q == null ? false : q.IsDelete;
                        n.IsPrint = q == null ? false : q.IsPrint;
                        result.Add(n);

                    }

                }
            } catch (Exception) { }
            return result;
        }


        public static List<XUserGroupInCompany> ListCompany(string groupId, string rcom) {
            List<XUserGroupInCompany> result = new List<XUserGroupInCompany>();
            try {
                using (GAEntities db = new GAEntities()) {
                    List<string> filter = new List<string> { "BRANCH", "COMPANY" };
                    var coms = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.IsActive == true && filter.Contains(o.TypeID)).OrderBy(o => o.CompanyID).ToList();
                    var gicoms = db.UserGroupInCompany.Where(o => o.RCompanyID == rcom && o.UserGroupID == groupId).ToList();

                    foreach (var c in coms) {
                        XUserGroupInCompany n = new XUserGroupInCompany();
                        var gic = gicoms.Where(o => o.CompanyID == c.CompanyID).FirstOrDefault();
                        n.UserGroupID = groupId;
                        n.RCompanyID = c.RCompanyID;
                        n.CompnayGroup = c.GroupCode;
                        n.X = gic != null ? true : false;
                        n.CompanyName = c.GroupCode + " " + c.Name1 + c.Name2;
                        n.CompanyType = c.TypeID;
                        n.CompanyTypeName = c.TypeID;
                        n.CompanyID = c.CompanyID;
                        n.IsActive = true;
                        result.Add(n);
                    }
                }
            } catch (Exception) { }
            return result;
        }

        public static List<XUserGroupInBoard> ListBoard(string groupId, string rcom) {
            List<XUserGroupInBoard> result = new List<XUserGroupInBoard>();
            using (GAEntities db = new GAEntities()) {
                var gib = db.UserGroupInBoard.Where(o => o.UserGroupID == groupId && o.RComID == rcom).ToList();
                var boards = db.DashBoard.Where(o => o.IsActive == true).ToList();
                foreach (var c in boards) {
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

        public static List<XUserGroupInLoc> ListLoc(string groupId, string rcom) {
            List<XUserGroupInLoc> result = new List<XUserGroupInLoc>();
            using (GAEntities db = new GAEntities()) {
                var gib = db.UserGroupInLoc.Where(o => o.UserGroupID == groupId && o.RCompanyID == rcom).ToList();
                var loc = db.LocationInfo.Where(o => o.IsActive == true).ToList();
                foreach (var c in loc) {
                    XUserGroupInLoc n = new XUserGroupInLoc();
                    var g_inB = gib.Where(o => o.LocID == c.LocID).FirstOrDefault();
                    n.X = g_inB == null ? false : true;
                    n.RCompanyID = rcom;
                    n.CompanyID = c.CompanyID;
                    n.X = g_inB != null ? true : false;
                    n.LocID = c.LocID;
                    n.Name = c.Name;
                    n.UserGroupID = groupId;
                    result.Add(n);
                }
            }
            return result;
        }
        public static List<vw_UserGroupInfo> ListDoc(string Search, LogInService.LoginSet login) {
            List<vw_UserGroupInfo> result = new List<vw_UserGroupInfo>();
            var rcom = login.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                result = db.vw_UserGroupInfo.Where(o =>
                                        (o.GroupName.Contains(Search)
                                            || o.UserGroupID.Contains(Search)
                                            || Search == ""
                                            )
                                            && o.RComID == rcom
                                            //&& (o.IsActive == true || f.ShowDisAble == true)
                                            ).OrderByDescending(o => o.Sort).ToList();

            }
            return result;
        }


        #endregion

        #region Save
        public static I_BasicResult Save(I_userGroupSet doc, string rcom, string user) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.Group;

            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserGroupInfo.Where(o => o.UserGroupID == doc.Group.UserGroupID && o.RComID == rcom).FirstOrDefault();
                    if (u == null) {
                        db.UserGroupInfo.Add(doc.Group);
                        db.SaveChanges();
                    } else {
                        u.GroupName = h.GroupName;
                        u.Sort = h.Sort;
                        u.ModifiedBy = user;
                        u.ModifiedDate = DateTime.Now;
                        u.IsActive = h.IsActive;
                        db.SaveChanges();

                        var rp = InsertAllPermission(doc,doc.Group.RComID,doc.Group.UserGroupID);

                        if (rp.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = result.Message1 + " " + rp.Message1;
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

        public static List<UserGroupPermission> ConvertX2Menu(I_userGroupSet input) {
            List<UserGroupPermission> result = new List<UserGroupPermission>();
            foreach (var s in input.XMenu) {
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

        public static List<UserGroupInCompany> ConvertX2Company(I_userGroupSet input) {
            List<UserGroupInCompany> result = new List<UserGroupInCompany>();
            foreach (var s in input.XCompany) {
                if (s.X) {
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
        public static List<UserGroupInBoard> ConvertX2Board(I_userGroupSet input) {
            List<UserGroupInBoard> result = new List<UserGroupInBoard>();
            var h = input.Group;
            foreach (var s in input.XBoard) {
                var n = new UserGroupInBoard();
                if (s.X) {
                    n.UserGroupID = h.UserGroupID;
                    n.RComID = s.RComID;
                    n.UserGroupID = s.UserGroupID;
                    n.DashBoardID = s.DashBoardID;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<UserGroupInLoc> ConvertX2Loc(I_userGroupSet input) {
            List<UserGroupInLoc> result = new List<UserGroupInLoc>();
            var h = input.Group;
            foreach (var s in input.XLoc) {
                var n = new UserGroupInLoc();
                if (s.X) {
                    n.UserGroupID = h.UserGroupID;
                    n.RCompanyID = s.RCompanyID;
                    n.CompanyID = s.CompanyID;
                    n.UserGroupID = s.UserGroupID;
                    n.LocID = s.LocID;
                    result.Add(n);
                }
            }
            return result;
        }



        public static void SaveLog(TransactionLog data, string rcom, string username) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                data.RCompanyID = rcom;
                data.TableID = data.TableID == null ? "" : data.TableID;
                data.TransactionID = data.TransactionID == null ? "" : data.TransactionID;
                data.TransactionDate = data.TransactionDate == null ? data.CreatedDate : data.TransactionDate;
                data.CreatedBy = username;
                data.CreatedDate = DateTime.Now;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.CompanyID = data.CompanyID == null ? "" : data.CompanyID;
                data.ParentID = data.ParentID == null ? "" : data.ParentID;
                data.Action = data.ActionType == null ? "" : data.ActionType;
                data.Action = data.Action == null ? "" : data.Action;
                data.ChangeValue = data.ChangeValue == null ? "" : data.ChangeValue;
                data.IsActive = true;
                using (GAEntities db = new GAEntities()) {
                    db.TransactionLog.Add(data);
                    var r = db.SaveChanges();
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
        }

        #endregion

        #region  permission manangement

        public static I_BasicResult InsertMenuToGroup(List<UserGroupPermission> group_menu, string rcom, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string conStr = Globals.GAEntitiesConn;

            //var group = group_menu.FirstOrDefault();
            //if (groupid == null) {
            //    //r.Result = "fail";
            //    //r.Message1 = "Menu not found";
            //    return r;
            //}
            //string rcom = group.RComID;
            try {

                using (SqlConnection conn = new SqlConnection(conStr)) {
                    conn.Open();
                    string sql = "delete from UserGroupPermission where RComID = @rcom and UserGroupID=@groupid";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("groupid", groupid);
                    var row = conn.Execute(sql, dynamicParameters);
                    if (group_menu.Count == 0) {
                        return r;
                    }
                    foreach (var f in group_menu) {

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
                        conn.Execute(sql, new {
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

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }
        public static I_BasicResult InsertCompanyToGroup(List<UserGroupInCompany> ugicom,string rcom, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            string conStr = Globals.GAEntitiesConn;

            //var groupid = ugicom.FirstOrDefault();
            //if (groupid == null) {
            //    //r.Result = "fail";
            //    //r.Message1 = "Menu not found";
            //    return r;
            //}
            //string rcom = groupid.RCompanyID;

            try {

                using (SqlConnection conn = new SqlConnection(conStr)) {
                    conn.Open();
                    string sql = "delete from UserGroupInCompany where RCompanyID = @rcom and UserGroupID=@groupid";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("rcom", rcom);
                    dynamicParameters.Add("groupid", groupid);
                    var rd = conn.Execute(sql, dynamicParameters);
                    if (ugicom.Count==0) {
                        return r;
                    }
                    foreach (var f in ugicom) {

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
                        conn.Execute(sql, new {
                            @RCompanyID = f.RCompanyID,
                            @CompanyID = f.CompanyID,
                            @UserGroupID = f.UserGroupID,
                            @IsActive = f.IsActive

                        });
                    }

                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }


        public static I_BasicResult InsertBoardToGroup(List<UserGroupInBoard> giboard, string rcom, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            //var group = giboard.FirstOrDefault();
            //if (group == null) {
            //    return r;
            //}
            try {
                using (GAEntities db = new GAEntities()) {
                    db.UserGroupInBoard.RemoveRange(db.UserGroupInBoard.Where(o => o.UserGroupID == groupid && o.RComID == rcom));
                    db.UserGroupInBoard.AddRange(giboard);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        public static I_BasicResult InsertLocToGroup(List<UserGroupInLoc> giLoc, string rcom, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            //var group = giboard.FirstOrDefault();
            //if (group == null) {
            //    return r;
            //}
            try {
                using (GAEntities db = new GAEntities()) {
                    db.UserGroupInLoc.RemoveRange(db.UserGroupInLoc.Where(o => o.UserGroupID == groupid && o.RCompanyID == rcom));
                    db.UserGroupInLoc.AddRange(giLoc);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        public static I_BasicResult CreateDefaultGrouopForAllRCom(string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var rcoms = db.CompanyInfo.Where(o => o.TypeID == "COMPANY" && o.IsActive == true).ToList();
                    var menuInfo = db.UserMenu.Where(o => o.IsActive == true).OrderBy(o => o.MenuID).ToList();
                    foreach (var c in rcoms) {//สร้าง group supperman ให้กับทุก Rcom
                        var ugroup = NewUsergroup("");
                        ugroup.RComID = c.CompanyID;
                        ugroup.UserGroupID = "SUPERMAN";
                        ugroup.GroupName = "Superman";
                        db.UserGroupInfo.RemoveRange(db.UserGroupInfo.Where(o => o.RComID == c.CompanyID && o.UserGroupID == "SUPERMAN"));
                        db.UserGroupInfo.Add(ugroup);
                        db.SaveChanges();
                        List<UserGroupPermission> gMenu = new List<UserGroupPermission>();
                        foreach (var m in menuInfo) {
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
                        var xx = InsertMenuToGroup(gMenu, ugroup.RComID, ugroup.UserGroupID);//insert usergroup menu
                        var com = db.CompanyInfo.Where(o => o.RCompanyID == c.RCompanyID && o.IsActive == true).ToList();
                        List<UserGroupInCompany> gCom = new List<UserGroupInCompany>();
                        foreach (var cs in com) {
                            UserGroupInCompany n = new UserGroupInCompany();
                            n.CompanyID = cs.CompanyID;
                            n.RCompanyID = ugroup.RComID;
                            n.UserGroupID = ugroup.UserGroupID;
                            n.IsActive = true;

                            gCom.Add(n);
                        }
                        var yy = InsertCompanyToGroup(gCom, ugroup.RComID,ugroup.UserGroupID);//insert company to usergroup
                        //insert board to group
                        var boards = db.DashBoard.Where(o => o.IsActive).ToList();
                        List<UserGroupInBoard> uBoards = new List<UserGroupInBoard>();
                        foreach (var b in boards) {
                            UserGroupInBoard n = new UserGroupInBoard();
                            n.DashBoardID = b.DashBoardID;
                            n.RComID = ugroup.RComID;
                            n.UserGroupID = ugroup.UserGroupID;
                            uBoards.Add(n);
                        }
                        var zz = InsertBoardToGroup(uBoards, ugroup.RComID, ugroup.UserGroupID);

                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        public static I_BasicResult InsertAllPermission(I_userGroupSet input,string rcom,string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var menu = ConvertX2Menu(input);
                var coms = ConvertX2Company(input);
                var board = ConvertX2Board(input);
                var loc = ConvertX2Loc(input);
                var r1 = InsertMenuToGroup(menu, rcom, groupid);
                var r2 = InsertCompanyToGroup(coms,rcom,groupid);
                var r3 = InsertBoardToGroup(board, rcom, groupid);
                var r4 = InsertLocToGroup(loc, rcom, groupid);
                //var r4 = AutoAddBoard(input.Group.UserGroupID);
                if (r1.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r1.Message1;
                }
                if (r2.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r2.Message1;
                }
                if (r3.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r3.Message1;
                }
                if (r4.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r4.Message1;
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }

            }
            return r;
        }

        #endregion

        #region New transaction

        public static UserGroupInfo NewUsergroup(string rcom) {
            UserGroupInfo n = new UserGroupInfo();
            n.RComID = rcom;
            n.UserGroupID = "";
            n.GroupName = "";
            n.LineToken = "";
            n.Sort = GetNextSort();
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }

        public static I_userGroupSet NewTransaction(string rcom) {
            I_userGroupSet n = new I_userGroupSet();
            n.Group = NewUsergroup(rcom);
            n.User = new List<vw_UserInGroup>();
            n.XMenu = new List<XMenu>();
            n.XCompany = new List<XUserGroupInCompany>();
            n.XBoard = new List<XUserGroupInBoard>();
            n.XLoc = new List<XUserGroupInLoc>();
            n.Log = new List<TransactionLog>();

            return n;
        }

        public static int GetNextSort() {
            int result = 1;
            try {
                using (GAEntities db = new GAEntities()) {
                    var q = db.UserGroupInfo.OrderByDescending(o => o.Sort).FirstOrDefault();
                    if (q != null) {
                        result = q.Sort + 1;
                    }
                }
            } catch (Exception) {
            }
            return result;
        }

        public static I_BasicResult CheckGroupID(string group_id, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var exist = db.UserGroupInfo.Where(o => o.UserGroupID == group_id && o.RComID == rcom).FirstOrDefault();
                    if (exist != null) {
                        result.Result = "fail";
                        result.Message1 = "ชื่อกลุ่มซ้ำในระบบแล้ว";
                    }
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

        public static I_BasicResult CheckDupSort(UserGroupInfo h, string rcom) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var exist = db.UserGroupInfo.Where(o => o.Sort == h.Sort && o.RComID == rcom).FirstOrDefault();
                    if (exist != null) {
                        result.Result = "fail";
                        result.Message1 = "Group sort " + h.Sort + " duplicate in database.";
                        var max = db.UserGroupInfo.Max(o => o.Sort);
                        result.Message2 = (max + 1).ToString();
                    }
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
        #endregion



    }
}
