using Dapper;
using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Board {
    public class BoardService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }

 
        #region board

        public static List<vw_board_master> GetMasterBoardMenu() {
            List<vw_board_master> output = new List<vw_board_master>();
            try {
                using (cimsContext db = new cimsContext()) {
                    output = db.vw_board_master.Where(o => o.is_active == 1).ToList();
                }
            } catch (Exception ex) { 
            } 
            return output;
        }
        public static vw_board_master? GetDefaultBoard() {
            vw_board_master? output = new vw_board_master();
            try {
                using (cimsContext db = new cimsContext()) {
                    output = db.vw_board_master.Where(o => o.is_default == 1).FirstOrDefault();
                }
            } catch (Exception ex) {
            }
            return output;
        }




        //public static List<api_master> ListApiMasterByCate(string cateid) {
        //    List<api_master> result = new List<api_master>();
        //    try {
        //        using (cimsContext db = new cimsContext()) {
        //            result = db.api_master.Where(o => o.cate == cateid || cateid == "").OrderBy(o => o.api_id).ToList();
        //        }
        //    } catch (System.Exception ex) {
        //        var message = ex.Message;
        //    }

        //    return result;
        //}

        public static List<vw_board_in_user_select> ListBoardInUser(string user) {
            List<vw_board_in_user_select> output = new List<vw_board_in_user_select>();
            try {
                using (cimsContext db = new cimsContext()) {
                    var boards = db.board_master.Where(o => o.is_active == 1).ToList();
                    var uib = db.vw_board_in_user.Where(o => o.username == user  ).ToList();
                    foreach (var b in boards) {
                        vw_board_in_user_select o = new vw_board_in_user_select();
                        o.id = b.id;
                        o.board_id = b.board_id;
                        o.board_name = b.name;
                        o.board_description = b.description;
                        o.board_type = b.board_type;
                        o.is_active = b.is_active;
                        o.IsSelect = Convert.ToBoolean( b.is_active);
                        //var hasboard = uib.Where(o => o.board_id == b.board_id).FirstOrDefault();
                        //if (hasboard != null) {
                        //    o.IsSelect = true;
                        //}
                        output.Add(o);

                    }
                }
            } catch (Exception ex) {

            }

            return output.OrderBy(o => o.board_id).ToList();
        }
     
        public static I_BasicResult RemoveUserInBoard(string user, string boardid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var chk_exist = db.board_in_user.Where(o => o.board_id == boardid && o.username == user).FirstOrDefault();
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
        #endregion


        #region widget
        public static List<vw_widget_in_user_select> GetUserInWidget(string user, string boardid,string orientation) {
            List<vw_widget_in_user_select> output = new List<vw_widget_in_user_select>();
            try {

                List<widget_master> widgets = new List<widget_master>();
                List<vw_widget_in_user> uiw = new List<vw_widget_in_user>();
                List<vw_widget_in_user> default_widgets = new List<vw_widget_in_user>();
                using (IDbConnection conn = Connection) {
               
                    conn.Open();
                    string sql = @"
                        select * from vw_widget_in_user
                        where username = @username and board_id=@board_id and orientation=@orientation and is_active=1
                        order by sort
                    " ;
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("username", user);
                    dynamicParameters.Add("board_id", boardid);
                    dynamicParameters.Add("orientation", orientation);
                    uiw = conn.Query<vw_widget_in_user>(sql, dynamicParameters).ToList();


                      sql = @"
                        select * from vw_widget_in_user
                        where username = @username and board_id=@board_id and orientation=@orientation and is_active=1
                        order by sort
                    ";
                      dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("username", "DEFAULT_LAYOUT");
                    dynamicParameters.Add("board_id", boardid);
                    dynamicParameters.Add("orientation", orientation);
                    default_widgets = conn.Query<vw_widget_in_user>(sql, dynamicParameters).ToList();


                    sql = @"
                        select * from widget_master
                        where  board_id=@board_id  and is_active=1
                    ";
                    dynamicParameters = new DynamicParameters(); 
                    dynamicParameters.Add("board_id", boardid);
                    widgets = conn.Query<widget_master>(sql, dynamicParameters).ToList();
                }


                //using (cimsContext db = new cimsContext()) {
                    //var widgets = db.widget_master.Where(o => o.board_id == boardid && o.is_active == 1).ToList();
                    //var default_widgets = db.vw_widget_in_user.Where(o => o.board_id == boardid && o.is_active == 1 && o.orientation == orientation && o.username == "DEFAULT_LAYOUT").ToList();

                   // var uiw = db.vw_widget_in_user.Where(o => o.username == user && o.board_id == boardid && o.is_active==1 && o.orientation==orientation).OrderBy(o=>o.sort).ToList();
                    int i = 1;
                    foreach (var w in widgets) {
                        vw_widget_in_user_select o = new vw_widget_in_user_select();
                        var user_widget = uiw.Where(x=>x.widget_id==w.widget_id).FirstOrDefault();
                        o.id = w.id;
                        o.widget_id = w.widget_id;
                        o.board_id = w.board_id;
                        o.name = w.name;
                        o.description = w.description;
                        if (o.widget_id== "NumberDeadRate") {
                            var x = "";
                        }

                        if (user_widget!=null) {  //เคย setup widget แล้ว                        
                            o.colspan = user_widget.colspan;
                            o.rowspan = user_widget.rowspan;
                            o.sort = user_widget.sort;
                            o.orientation = orientation;
                            o.is_active = user_widget.is_active;
                            o.IsSelect = Convert.ToBoolean(w.is_active);
                        } else {// ไม่เคย setup widget ให้ดึง default
                            var d_widget = default_widgets.Where(x => x.widget_id == w.widget_id).FirstOrDefault();
                            if (d_widget != null) { // ถ้ามี default widget
                                o.colspan = d_widget.colspan;
                                o.rowspan = d_widget.rowspan;
                                o.sort = d_widget.sort;
                                o.orientation = orientation;
                                o.is_active = d_widget.is_active;
                                o.IsSelect = Convert.ToBoolean(w.is_active);
                            } else {// ยังไม่เคย set default widget
                                o.colspan = 4;
                                o.rowspan = i;
                                o.sort = i;
                                o.orientation = orientation;
                                o.is_active = 1;
                                o.IsSelect = true;
                            }
                        
                        }
                         
                      
                        output.Add(o);
                        i++;

                    }
                //}
            } catch (Exception ex) {

            }

            return output.OrderBy(o=>o.sort).ToList();
        }

        public static List<vw_widget_in_user> GetCustomWidgetInUser(string user, string boardid, string orientation) {
            List<vw_widget_in_user> output = new List<vw_widget_in_user>();
            try {

                List<widget_master> widgets = new List<widget_master>();
                List<vw_widget_in_user> uiw = new List<vw_widget_in_user>();
                List<vw_widget_in_user> default_widgets = new List<vw_widget_in_user>();
                using (IDbConnection conn = Connection) {

                    conn.Open();
                    string sql = @"
                        select * from vw_custom_widget_in_user
                        where username = @username and board_id=@board_id and orientation=@orientation and is_active=1
                        order by sort
                    ";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("username", user);
                    dynamicParameters.Add("board_id", boardid);
                    dynamicParameters.Add("orientation", orientation);
                    uiw = conn.Query<vw_widget_in_user>(sql, dynamicParameters).ToList();


                    sql = @"
                        select * from vw_custom_widget_in_user
                        where username = @username and board_id=@board_id and orientation=@orientation and is_active=1
                        order by sort
                    ";
                    dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("username", "DEFAULT_LAYOUT");
                    dynamicParameters.Add("board_id", boardid);
                    dynamicParameters.Add("orientation", orientation);
                    default_widgets = conn.Query<vw_widget_in_user>(sql, dynamicParameters).ToList();


                    sql = @"
                        select * from vw_custom_widget_in_user
                        where  board_id=@board_id  and is_active=1
                    ";
                    dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("board_id", boardid);
                    widgets = conn.Query<widget_master>(sql, dynamicParameters).ToList();
                }


                //using (cimsContext db = new cimsContext()) {
                //var widgets = db.widget_master.Where(o => o.board_id == boardid && o.is_active == 1).ToList();
                //var default_widgets = db.vw_widget_in_user.Where(o => o.board_id == boardid && o.is_active == 1 && o.orientation == orientation && o.username == "DEFAULT_LAYOUT").ToList();

                // var uiw = db.vw_widget_in_user.Where(o => o.username == user && o.board_id == boardid && o.is_active==1 && o.orientation==orientation).OrderBy(o=>o.sort).ToList();
                int i = 1;
                foreach (var w in widgets) {
                    vw_widget_in_user o = new vw_widget_in_user();
                    var user_widget = uiw.Where(x => x.widget_id == w.widget_id).FirstOrDefault();
                    o.id = w.id;
                    o.widget_id = w.widget_id;
                    o.board_id = w.board_id;
                    o.description = w.name; 
                    if (o.widget_id == "NumberDeadRate") {
                        var x = "";
                    }

                    if (user_widget != null) {  //เคย setup widget แล้ว                        
                        o.colspan = user_widget.colspan; 
                        o.rowspan = user_widget.rowspan;
                        o.sort = user_widget.sort;
                        o.orientation = orientation;
                        o.is_active = user_widget.is_active;
     
                    } else {// ไม่เคย setup widget ให้ดึง default
                        var d_widget = default_widgets.Where(x => x.widget_id == w.widget_id).FirstOrDefault();
                        if (d_widget != null) { // ถ้ามี default widget
                            o.colspan = d_widget.colspan;
                            o.rowspan = d_widget.rowspan;
                            o.sort = d_widget.sort;
                            o.orientation = orientation;
                            o.is_active = d_widget.is_active;
                       
                        } else {// ยังไม่เคย set default widget
                            o.colspan = 4;
                            o.rowspan = i;
                            o.sort = i;
                            o.orientation = orientation;
                            o.is_active = 1;
                          
                        }

                    }


                    output.Add(o);
                    i++;

                }
                //}
            } catch (Exception ex) {

            }

            return output.OrderBy(o => o.sort).ToList();
        }


        public static I_BasicResult SaveWidget(List<vw_widget_in_user_select> data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var m_data = data.FirstOrDefault();
                using (cimsContext db = new cimsContext()) {

                    //var list_exist_widget = db.widget_in_user.Where(o => o.widget_id == m_data.widget_id && o.orientation == m_data.orientation && o.username == m_data.username && o.board_id == m_data.board_id).ToList();
                    db.widget_in_user.RemoveRange(db.widget_in_user.Where(o => o.username == m_data.username && o.board_id == m_data.board_id && o.orientation==m_data.orientation));
                  
                    foreach (var d in data) {
                        widget_in_user n = new widget_in_user();

                        n.widget_id = d.widget_id;
                        n.board_id = d.board_id;
                        n.username = d.username;
                        n.rowspan = d.rowspan;
                        n.colspan = d.colspan;
                        n.sort = d.sort;
                        n.orientation = d.orientation;
                        n.is_active = d.is_active;
                        db.widget_in_user.Add(n); 

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
        //public static I_BasicResult RemoveUserInWidget(string user, string boardid, string widgetid, string orientation) {
        //    I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {
        //        using (cimsContext db = new cimsContext()) {
        //            var chk_exist = db.widget_in_user.Where(o => o.widget_id == widgetid && o.board_id == boardid && o.username == user).FirstOrDefault();
        //            if (chk_exist != null) {
        //                chk_exist.is_active = 0;
        //                db.SaveChanges();
        //            }
        //            db.SaveChanges();
        //        }

        //    } catch (Exception ex) {
        //        r.Result = "fail";
        //        if (ex.InnerException != null) {
        //            r.Message1 = ex.InnerException.ToString();
        //        } else {
        //            r.Message1 = ex.Message;
        //        }
        //    }
        //    return r;
        //}
        #endregion

    }
}
