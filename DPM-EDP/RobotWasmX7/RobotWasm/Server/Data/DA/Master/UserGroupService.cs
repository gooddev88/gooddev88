using Dapper;
using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.ApiMaster;
using RobotWasm.Shared.Data.ML.Master;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Master.UserGroup;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Data.DA.LogTran;

namespace RobotWasm.Server.Data.DA.Master {
    public class UserGroupService {

        public I_userGroupSet DocSet { get; set; }

        public UserGroupService() {

        }

        #region Get List
        public static I_userGroupSet GetDocSet(string groupid) {
            I_userGroupSet n = new I_userGroupSet();
            using (cimsContext db = new cimsContext()) {

                n.Group = db.usergroup_info.Where(o => o.group_id == groupid && o.is_active == 1).FirstOrDefault();
                n.UserInGroup = db.vw_user_in_group.Where(o => o.group_id == groupid && o.is_active == 1).ToList();
                if (n.Group == null) {
                    n = NewTransaction();
                } else {
                    n.XMenu = ListMenu(groupid);
                    n.XBoard = ListBoard(groupid);
                    n.XApi = ListApi(groupid);
                }
            }
            return n;
        }

        public static List<xusergroup_in_menu> ListMenu(string groupId) {
            List<xusergroup_in_menu> result = new List<xusergroup_in_menu>();
            try {
                using (cimsContext db = new cimsContext()) {
                    var uimenu = db.usergroup_in_menu.Where(o => o.group_id == groupId).ToList();
                    var menu = db.usermenu.Where(o => o.is_active == 1).OrderBy(o => o.group_sort).ThenBy(o => o.menu_sort).ToList();
                    foreach (var m in menu) {
                        xusergroup_in_menu n = new xusergroup_in_menu();
                        var q = uimenu.Where(o => o.menu_id == m.menu_id).FirstOrDefault();

                        n.menu_id = m.menu_id;
                        n.MenuCode = m.menu_code;
                        n.MenuName = m.menu_name;
                        n.Sort = Convert.ToInt32(m.group_sort);
                        n.group_id = groupId;

                        n.MenuGroupID = m.group_id;
                        n.MenuDesc1 = m.menu_description1;
                        n.MenuDesc2 = m.menu_description2;
                        n.MenuTypeID = m.type_id;
                        n.MenuGroupSort = Convert.ToInt32(m.group_sort);
                        n.MenuSort = Convert.ToInt32(m.menu_sort);

                        n.NeedCreatePermission = Convert.ToInt32(m.need_create);
                        n.NeedOpenPermission = Convert.ToInt32(m.need_open);
                        n.NeedEditPermission = Convert.ToInt32(m.need_edit);
                        n.NeedPrintPermission = Convert.ToInt32(m.need_print);
                        n.NeedDeletePermission = Convert.ToInt32(m.need_delete);

                        n.CaptionOpenPermission = m.text_open;
                        n.CaptionCreatePermission = m.text_create;
                        n.CaptionEditPermission = m.text_edit;
                        n.CaptionDeletePermission = m.text_delete;
                        n.CaptionPrintPermission = m.text_print;

                        n.isCreateBind = q == null ? false : Convert.ToBoolean(q.is_create);
                        n.isOpenBind = q == null ? false : Convert.ToBoolean(q.is_open);
                        n.isEditBind = q == null ? false : Convert.ToBoolean(q.is_edit);
                        n.isDeleteBind = q == null ? false : Convert.ToBoolean(q.is_delete);
                        n.isPrintBind = q == null ? false : Convert.ToBoolean(q.is_print);

                        n.is_create = q == null ? 0 : q.is_create;
                        n.is_open = q == null ? 0 : q.is_open;
                        n.is_edit = q == null ? 0 : q.is_edit;
                        n.is_delete = q == null ? 0 : q.is_delete;
                        n.is_print = q == null ? 0 : q.is_print;
                        result.Add(n);
                    }
                }
            } catch (Exception) { }
            return result;
        }

        public static List<xusergroup_in_board> ListBoard(string groupId) {
            List<xusergroup_in_board> result = new List<xusergroup_in_board>();
            using (cimsContext db = new cimsContext()) {
                var gib = db.usergroup_in_board.Where(o => o.group_id == groupId).ToList();
                var boards = db.board_master.Where(o => o.is_active == 1).ToList();
                foreach (var c in boards) {
                    xusergroup_in_board n = new xusergroup_in_board();
                    var g_inB = gib.Where(o => o.board_id == c.board_id).FirstOrDefault();
                    n.X = g_inB == null ? false : true;
                    n.X = g_inB != null ? true : false;
                    n.board_id = c.board_id;
                    n.Name = c.name;
                    n.group_id = groupId;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<xapi_master> ListApi(string groupId) {
            List<xapi_master> result = new List<xapi_master>();
            using (cimsContext db = new cimsContext()) {
                var gia = db.usergroup_in_api.Where(o => o.group_id == groupId).ToList();
                var apis = db.api_master.Where(o => o.is_active == 1).ToList();
                foreach (var a in apis) {
                    xapi_master n = new xapi_master();
                    var g_inA = gia.Where(o => o.api_id == a.api_id).FirstOrDefault();
                    n.X = g_inA == null ? false : true;
                    n.X = g_inA != null ? true : false;
                    n.api_id = a.api_id;
                    n.api_name = a.api_name;
                    n.GroupID = groupId;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<usergroup_info> ListDoc(string Search) {
            List<usergroup_info> result = new List<usergroup_info>();
            using (cimsContext db = new cimsContext()) {
                result = db.usergroup_info.Where(o =>
                                        (o.group_id.Contains(Search)
                                            || o.group_name.Contains(Search)
                                            || o.group_description.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderByDescending(o => o.group_sort).ToList();

            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_userGroupSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.Group;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.usergroup_info.Where(o => o.group_id == doc.Group.group_id).FirstOrDefault();
                    if (u == null) {
                        db.usergroup_info.Add(doc.Group);
                        if (doc.UserInGroup.Count() > 0)
                        {
                            u.count_user = doc.UserInGroup.Count();
                        }
                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(h.group_id,h.created_by, "กลุ่มผู้ใช้งานระบบ", "เพิ่มกลุ่มผู้ใช้งานระบบ");
                    } else {
                        u.group_name = h.group_name;
                        u.group_description = h.group_description;
                        u.group_sort = h.group_sort;
                        u.modified_by = h.modified_by;
                        u.modified_date = DateTime.Now;
                        if (doc.UserInGroup.Count() > 0)
                        {
                            u.count_user = doc.UserInGroup.Count();
                        }
                        u.is_active = h.is_active;
                        db.SaveChanges();

                        var rp = InsertAllPermission(doc,doc.Group.group_id);

                        if (rp.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = result.Message1 + " " + rp.Message1;
                        }
                        var rs = LogTranService.CreateTransLog(u.group_id, u.modified_by, "กลุ่มผู้ใช้งานระบบ", "แก้ไขกลุ่มผู้ใช้งานระบบ");
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

        public static I_BasicResult InsertAllPermission(I_userGroupSet input,string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var menu = ConvertX2Menu(input);
                var board = ConvertX2Board(input);
                var apis = ConvertX2Api(input);
                var r1 = InsertMenuToGroup(menu,groupid);
                var r2 = InsertBoardToGroup(board,groupid);
                var r3 = InsertCompanyToGroup(apis, groupid);
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

        public static List<usergroup_in_menu> ConvertX2Menu(I_userGroupSet input) {
            List<usergroup_in_menu> result = new List<usergroup_in_menu>();
            foreach (var s in input.XMenu) {
                var n = new usergroup_in_menu();
                n.group_id = s.group_id;
                n.menu_id = s.menu_id;
                n.is_open = Convert.ToInt32(s.isOpenBind);
                n.is_create = Convert.ToInt32(s.isCreateBind);
                n.is_edit = Convert.ToInt32(s.isEditBind);
                n.is_delete = Convert.ToInt32(s.isDeleteBind);
                n.is_print = Convert.ToInt32(s.isPrintBind);
                result.Add(n);
            }
            return result;
        }

        public static List<usergroup_in_board> ConvertX2Board(I_userGroupSet input) {
            List<usergroup_in_board> result = new List<usergroup_in_board>();
            var h = input.Group;
            foreach (var s in input.XBoard) {
                var n = new usergroup_in_board();
                if (s.X) {
                    n.group_id = h.group_id;
                    n.board_id = s.board_id;
                    result.Add(n);
                }
            }
            return result;
        }

        public static List<usergroup_in_api> ConvertX2Api(I_userGroupSet input) {
            List<usergroup_in_api> result = new List<usergroup_in_api>();
            var h = input.Group;
            foreach (var s in input.XApi) {
                var n = new usergroup_in_api();
                if (s.X) {
                    n.group_id = h.group_id;
                    n.api_id = s.api_id;
                    result.Add(n);
                }
            }
            return result;
        }

        #region  permission manangement

        public static I_BasicResult InsertMenuToGroup(List<usergroup_in_menu> group_menu, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            //string conStr = Globals.CimsConn;

            try {
                using (cimsContext db = new cimsContext()) {
                    db.usergroup_in_menu.RemoveRange(db.usergroup_in_menu.Where(o => o.group_id == groupid));
                    db.usergroup_in_menu.AddRange(group_menu);
                    db.SaveChanges();
                }
                //using (SqlConnection conn = new SqlConnection(conStr)) {
                //    conn.Open();
                //    string sql = "delete from usergroup_in_menu where group_id=@groupid";
                //    var dynamicParameters = new DynamicParameters();
                //    dynamicParameters.Add("groupid", groupid);
                //    var row = conn.Execute(sql, dynamicParameters);
                //    if (group_menu.Count == 0) {
                //        return r;
                //    }
                //    foreach (var f in group_menu) {

                //        sql = @"insert into  usergroup_in_menu  (
                //               group_id
                //              ,menu_id
                //              ,is_open
                //              ,is_create
                //              ,is_edit
                //              ,is_delete
                //              ,is_print
                //           ) values(
                //               @group_id
                //              ,@menu_id
                //              ,@IsOpen
                //              ,@IsCreate
                //              ,@IsEdit
                //              ,@IsDelete
                //              ,@IsPrint                             
                //         )";
                //        conn.Execute(sql, new {
                //            @group_id = f.group_id,
                //            @menu_id = f.menu_id,
                //            @IsOpen = f.is_open,
                //            @IsCreate = f.is_create,
                //            @IsEdit = f.is_edit,
                //            @IsDelete = f.is_delete,
                //            @IsPrint = f.is_print
                //        });
                //    }
                //}
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

        public static I_BasicResult InsertBoardToGroup(List<usergroup_in_board> giboard, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.usergroup_in_board.RemoveRange(db.usergroup_in_board.Where(o => o.group_id == groupid));
                    db.usergroup_in_board.AddRange(giboard);
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

        public static I_BasicResult InsertCompanyToGroup(List<usergroup_in_api> giapi, string groupid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.usergroup_in_api.RemoveRange(db.usergroup_in_api.Where(o => o.group_id == groupid));
                    db.usergroup_in_api.AddRange(giapi);
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

        #endregion

        public static I_BasicResult DeleteUserGroup(string usergroup)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    var head = db.usergroup_info.Where(o => o.group_id == usergroup).FirstOrDefault();
                    head.is_active = 0;
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
                    result.Message1 = ex.Message;
                }
            }

            return result;
        }


        #endregion

        #region New
        public static I_userGroupSet NewTransaction() {
            I_userGroupSet n = new I_userGroupSet();
            n.Group = NewUsergroup();
            n.UserInGroup = new List<vw_user_in_group>();
            n.XMenu = new List<xusergroup_in_menu>();
            n.XApi = new List<xapi_master>();
            n.XBoard = new List<xusergroup_in_board>();
            return n;
        }

        public static usergroup_info NewUsergroup() {
            usergroup_info n = new usergroup_info();
            n.group_id = "";
            n.group_name = "";
            n.group_description = "";
            n.group_sort = 0;
            n.created_by = "";
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_active = 1;
            return n;
        }

        public static short GenSort() {
            short result = 1;
            try {
                using (cimsContext db = new cimsContext()) {
                    var h = db.usergroup_info.ToList();
                    var max_linenum = h.Max(o => o.group_sort);
                    result = Convert.ToInt16(max_linenum + 1);
                }
            } catch { }
            return result;
        }

        #endregion

    }
}
