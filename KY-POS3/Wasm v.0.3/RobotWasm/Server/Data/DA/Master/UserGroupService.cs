using Dapper;
using Npgsql;
using RobotWasm.Shared.Data.GaDB;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Master.UserGroup;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Data.GaDB;

namespace RobotWasm.Server.Data.DA.Master {
    public class UserGroupService {

        public I_userGroupSet DocSet { get; set; }

        public UserGroupService() {

        }

        #region Get List
        public static I_userGroupSet GetDocSet(string groupid,string rcom) {
            I_userGroupSet n = new I_userGroupSet();
            using (GAEntities db = new GAEntities()) {

                n.Group = db.UserGroupInfo.Where(o => o.UserGroupID == groupid && o.IsActive == true).FirstOrDefault();
                    n.XMenu = ListMenu(groupid,rcom);
                    n.XBoard = ListBoard(groupid, rcom);
                    n.XCompany = ListCompany(groupid,rcom);
            }
            return n;
        }

        public static List<XMenu> ListMenu(string groupId, string rcom)
        {
            List<XMenu> result = new List<XMenu>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var uimenu = db.UserGroupPermission.Where(o => o.UserGroupID == groupId && o.RComID == rcom).ToList();
                    var menu = db.UserMenu.Where(o => o.IsActive == true).OrderBy(o => o.GroupSort).ThenBy(o => o.SubGroupSort).ToList();
                    foreach (var m in menu)
                    {
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
            }
            catch (Exception) { }
            return result;
        }

        public static List<XUserGroupInCompany> ListCompany(string groupId, string rcom)
        {
            List<XUserGroupInCompany> result = new List<XUserGroupInCompany>();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var coms = db.CompanyInfo.Where(o => o.RCompanyID == rcom && o.IsActive == true && o.TypeID == "BRANCH").OrderBy(o => o.CompanyID).ToList();
                    var gicoms = db.UserGroupInCompany.Where(o => o.RCompanyID == rcom && o.UserGroupID == groupId).ToList();

                    foreach (var c in coms)
                    {
                        XUserGroupInCompany n = new XUserGroupInCompany();
                        var gic = gicoms.Where(o => o.CompanyID == c.CompanyID).FirstOrDefault();
                        n.UserGroupID = groupId;
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

        public static List<XUserGroupInBoard> ListBoard(string groupId,string rcom)
        {
            List<XUserGroupInBoard> result = new List<XUserGroupInBoard>();
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

        public static List<UserGroupInfo> ListDoc(string search,string rcom) {
            List<UserGroupInfo> result = new List<UserGroupInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.UserGroupInfo.Where(o =>
                                        (o.UserGroupID.Contains(search)
                                            || o.GroupName.Contains(search)
                                            || search == ""
                                            )
                                            && o.RComID == rcom
                                            && (o.IsActive == true)
                                            ).OrderByDescending(o => o.Sort).ToList();
            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_userGroupSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.Group;

            try {
                using (GAEntities db = new GAEntities()) {
                    var u = db.UserGroupInfo.Where(o => o.UserGroupID == doc.Group.UserGroupID).FirstOrDefault();
                    if (u == null) {
                        db.UserGroupInfo.Add(doc.Group);
                        db.SaveChanges();
                    } else {
                        u.GroupName = h.GroupName;
                        u.Sort = h.Sort;
                        u.ModifiedBy = h.ModifiedBy;
                        u.ModifiedDate = DateTime.Now;
                        u.IsActive = h.IsActive;
                        db.SaveChanges();

                        var rp = InsertAllPermission(doc);

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

        public static I_BasicResult InsertAllPermission(I_userGroupSet input) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var menu = ConvertX2Menu(input);
                var com = ConvertX2Company(input);
                var board = ConvertX2Board(input);
                var r1 = InsertMenuToGroup(menu,input.Group.UserGroupID);
                var r2 = InsertCompanyToGroup(com,input.Group.UserGroupID);
                var r3 = InsertBoardToGroup(board,input.Group.UserGroupID);
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

        #region  permission manangement

        public static I_BasicResult InsertMenuToGroup(List<UserGroupPermission> group_menu, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                using (GAEntities db = new GAEntities()) {
                    db.UserGroupPermission.RemoveRange(db.UserGroupPermission.Where(o => o.UserGroupID == groupid));
                    db.UserGroupPermission.AddRange(group_menu);
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

        public static I_BasicResult InsertBoardToGroup(List<UserGroupInBoard> giboard, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.UserGroupInBoard.RemoveRange(db.UserGroupInBoard.Where(o => o.UserGroupID == groupid));
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

        public static I_BasicResult InsertCompanyToGroup(List<UserGroupInCompany> gicom, string groupid)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    db.UserGroupInCompany.RemoveRange(db.UserGroupInCompany.Where(o => o.UserGroupID == groupid));
                    db.UserGroupInCompany.AddRange(gicom);
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

        #endregion

        #endregion

        public static short GenSort() {
            short result = 1;
            try {
                using (GAEntities db = new GAEntities()) {
                    var h = db.UserGroupInfo.ToList();
                    var max_linenum = h.Max(o => o.Sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }

    }
}
