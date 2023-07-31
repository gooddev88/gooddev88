using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using System.Data;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Board {
    public class WidgetService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }

        public static List<vw_widget_in_user_select> GetUserInWidget(string user,string boardid) {
            List<vw_widget_in_user_select> output = new List<vw_widget_in_user_select>();
            try {
                using (cimsContext db = new cimsContext()) {
                    var widgets = db.widget_master.Where(o => o.board_id==boardid && o.is_active == 1).ToList();
                    var uiw = db.vw_widget_in_user.Where(o => o.username == user && o.board_id==boardid ).ToList();
                    foreach (var b in widgets) {
                        vw_widget_in_user_select o = new vw_widget_in_user_select(); 
        o.id = b.id;
                        o.widget_id = b.widget_id;
                        o.board_id = b.board_id;
                        o.name = b.name;
                        o.description = b.description;
                         
                        o.is_active = b.is_active;
                        o.IsSelect = Convert.ToBoolean(b.is_active);
                        output.Add(o);

                    }
                }
            } catch (Exception ex) {

            }

            return output;
        }


        public static I_BasicResult AddUserInWidget(string user ,string boardid , string widgetid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var chk_exist = db.widget_in_user.Where(o => o.widget_id == widgetid && o.board_id==boardid && o.username == user).FirstOrDefault();
                    if (chk_exist == null) {
                        widget_in_user n = new widget_in_user(); 
       
                        n.widget_id = widgetid;
                        n.board_id = boardid;
                        n.username = user;
                        n.is_active = 1;
                        db.widget_in_user.Add(n);
                        db.SaveChanges();
                    } else {
                        chk_exist.is_active = 1;
                        db.SaveChanges();
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
        public static I_BasicResult RemoveUserInWidget(string user, string boardid, string widgetid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var chk_exist = db.widget_in_user.Where(o => o.widget_id==widgetid && o.board_id == boardid && o.username == user).FirstOrDefault();
                    if (chk_exist != null) {
                        chk_exist.is_active = 0;
                        db.SaveChanges();
                    }
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
    }
}
