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
using RobotWasm.Shared.Data.ML.Master.User;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Helper;
using RobotWasm.Server.Data.DA.LogTran;

namespace RobotWasm.Server.Data.DA.Master {
    public class UserService {

        public I_UserSet DocSet { get; set; }

        public UserService() {

        }

        #region Get List
        public static I_UserSet GetDocSet(string username) {
            I_UserSet n = new I_UserSet();
            using (cimsContext db = new cimsContext()) {

                n.User = db.user_info.Where(o => o.username == username && o.is_active == 1).FirstOrDefault();
                n.XGroup = ListGroup(username);
            }
            return n;
        }

        public static List<xuser_in_group> ListGroup(string username) {
            List<xuser_in_group> result = new List<xuser_in_group>();
            try {
                List<string> groupExclude = new List<string> { "anonymous" };
                using (cimsContext db = new cimsContext()) {
                    var group = db.usergroup_info.Where(o => !groupExclude.Contains(o.group_id)
                                                   && o.is_active == 1).ToList();
                    var userin_group = db.user_in_group.Where(o => o.username == username).ToList();

                    foreach (var c in group) {
                        xuser_in_group n = new xuser_in_group();
                        var uig = userin_group.Where(o => o.group_id == c.group_id).FirstOrDefault();
                        n.X = uig != null ? true : false;
                        n.username = username;
                        n.group_id = c.group_id;
                        n.Name = c.group_name;
                        result.Add(n);
                    }
                }
            } catch (Exception) { }
            return result;
        }


        public static List<user_info> ListDoc(string Search) {
            List<user_info> result = new List<user_info>();
            using (cimsContext db = new cimsContext()) {
                result = db.user_info.Where(o =>
                                        (o.username.Contains(Search)
                                            || o.first_name.Contains(Search)
                                            || o.last_name.Contains(Search)
                                            || o.email.Contains(Search)
                                            || o.comgroupid.Contains(Search)
                                            || o.companyid.Contains(Search)
                                            || Search == ""
                                            )
                                            && (o.is_active == 1)
                                            ).OrderByDescending(o => o.username).ToList();

            }
            return result;
        }

        #endregion

        #region Save

        public static I_BasicResult Save(I_UserSet doc) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var h = doc.User;

            try {
                using (cimsContext db = new cimsContext()) {
                    var u = db.user_info.Where(o => o.username == h.username).FirstOrDefault();
                    if (u == null) {
                        doc.User.password = Hash.hashPassword("MD5","123456");
                        db.user_info.Add(doc.User);
                        db.SaveChanges();
                        var rs = LogTranService.CreateTransLog(h.username,h.created_by, "ผู้ใช้งานระบบ", "เพิ่มผู้ใช้งานระบบ");
                    } else {
                        u.first_name = h.first_name;
                        u.last_name = h.last_name;
                        u.email = h.email;
                        u.is_newuser = h.is_newuser;
                        u.comgroupid = h.comgroupid;
                        u.companyid = h.companyid;
                        u.modified_by = h.modified_by;
                        u.modified_date = DateTime.Now;
                        u.is_active = h.is_active;
                        db.SaveChanges();

                        var rp = InsertAllPermission(doc,h.username);

                        if (rp.Result == "fail") {
                            result.Result = "fail";
                            result.Message1 = result.Message1 + " " + rp.Message1;
                        }
                        var rs = LogTranService.CreateTransLog(u.username, u.modified_by, "ผู้ใช้งานระบบ", "แก้ไขผู้ใช้งานระบบ");
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

        public static I_BasicResult ReSetPassword(I_UserSet doc)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    var u = db.user_info.Where(o => o.username == doc.User.username).FirstOrDefault();
                        u.password = Hash.hashPassword("MD5", doc.User.password);
                        u.is_newuser = doc.User.is_newuser;
                        db.SaveChanges();

                        var rs = LogTranService.CreateTransLog(u.username, u.modified_by, "ผู้ใช้งานระบบ", "รีเซ้ตรหัส ผู้ใช้งานระบบ");
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

        public static I_BasicResult InsertAllPermission(I_UserSet input,string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var menu = ConvertX2UserInGroup(input);
                var r1 = InsertUserToGroup(menu, username);
                if (r1.Result == "fail") {
                    r.Result = "fail";
                    r.Message1 = r.Message1 + " " + r1.Message1;
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

        public static List<user_in_group> ConvertX2UserInGroup(I_UserSet input) {
            List<user_in_group> result = new List<user_in_group>();
            var h = input.User;
            foreach (var s in input.XGroup) {
                var n = new user_in_group();
                if (s.X) {
                    n.username = h.username;
                    n.group_id = s.group_id;
                    result.Add(n);
                }
            }
            return result;
        }

        public static I_BasicResult InsertUserToGroup(List<user_in_group> uigroup, string username) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    foreach (var l in uigroup.Select(o => o.group_id).ToList()) { 
                    var ui = db.usergroup_info.Where(o => o.group_id == l).FirstOrDefault();
                        ui.count_user = uigroup.Where(o => o.username == username).Count();
                    }
                    db.user_in_group.RemoveRange(db.user_in_group.Where(o => o.username == username));
                    db.user_in_group.AddRange(uigroup);
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

        public static I_BasicResult DeleteUser(string username)
        {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (cimsContext db = new cimsContext())
                {
                    var head = db.user_info.Where(o => o.username == username).FirstOrDefault();
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

    }
}
