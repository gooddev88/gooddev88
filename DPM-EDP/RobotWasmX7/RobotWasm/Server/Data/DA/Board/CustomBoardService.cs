using Dapper;
using Npgsql;
using RobotWasm.Client.Pages.DPMBoard.CustomBoard.Param;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData.BoardParam;
using RobotWasm.Shared.Data.ML.DPMBaord.CustomBoard;
using System.Data;
using System.Globalization;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.Board {
    public class CustomBoardService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }


        public static CustomBoardDocSet GetDocset(string boardid) {
            CustomBoardDocSet output = new CustomBoardDocSet();
            try {
                using (cimsContext db = new cimsContext()) {
                    output.CustomBoard = db.custom_board_in_user.Where(o => o.board_id == boardid).FirstOrDefault();
                    if (output.CustomBoard != null) {
                        output.CustomWidgets = db.vw_custom_widget_in_user.Where(o => o.board_id == boardid && o.has_widget==1).ToList();
                        var widget_ids = db.vw_custom_widget_in_user.Where(o=>o.has_widget==1).Select(o => o.widget_id ).Distinct().ToList();
                        //  output.CustomWidgetParam = db.custom_widget_param_in_user.Where(o => o.username == output.CustomBoard.username && widget_ids.Contains(o.widget_id)).ToList();
                    } else {
                        output.CustomWidgets = new List<vw_custom_widget_in_user>();
                        //    output.CustomWidgetParam = new List<custom_widget_param_in_user>();
                    }
                    //  output.CustomWidgetMaster = db.custom_widget_master.Where(o => o.is_active == 1).ToList();
                }
            } catch (Exception ex) {
            } 
            return output;
        }


        public static List<custom_widget_master_select> ListWidgetMasterForAdd(string baord_id) {
            List<custom_widget_master_select> output = new List<custom_widget_master_select>();
            try {
                using (cimsContext db = new cimsContext()) {
                    var uwd = db.custom_widget_in_user.Where(o => o.board_id == baord_id ).Select(o => o.widget_id).ToList();
                    var query = db.custom_widget_master.Where(o => o.has_widget==1 && o.is_active == 1).ToList();
                    foreach (var q in query) {
                        custom_widget_master_select o = new custom_widget_master_select();
                        o.is_select = false;
                        o.board_id = baord_id;
                        o.widget_id = q.widget_id;
                        o.widget_desc = q.widget_desc;
                        o.h_colspan = q.h_colspan;
                        o.h_rowspan = q.h_rowspan;
                        o.v_colspan = q.v_colspan;
                        o.v_rowspan = q.v_rowspan;
                        o.h_sort = q.h_sort;
                        o.v_sort = q.v_sort;
                        o.group_id = q.group_id;
                        o.group_name = q.group_name;
                        o.is_active = q.is_active;
                        output.Add(o);
                    }
                    output.RemoveAll(o => uwd.Contains(o.widget_id));
                }
            } catch (Exception ex) {
            }
            return output;

        }

        public static List<vw_custom_board_in_user> ListCustomBoardInUser(string user) {
            List<vw_custom_board_in_user> output = new List<vw_custom_board_in_user>();
            try {
                using (cimsContext db = new cimsContext()) {
                    output = db.vw_custom_board_in_user.Where(o => o.username == user && o.is_active == 1).ToList();
                }
            } catch (Exception ex) {

            }

            return output.OrderBy(o => o.board_id).ToList();
        }
        public static I_BasicResult AddUserInBoard(string user, string boardid) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var chk_exist = db.board_in_user.Where(o => o.board_id == boardid && o.username == user).FirstOrDefault();
                    if (chk_exist == null) {
                        board_in_user n = new board_in_user();
                        n.board_id = boardid;
                        n.username = user;
                        n.layout_json_h = "";
                        n.sort = 0;
                        n.is_active = 1;
                        db.board_in_user.Add(n);
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
        public static I_BasicResult CreateUpdateBoard(custom_board_in_user data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var chk_exist = db.custom_board_in_user.Where(o => o.board_id == data.board_id).FirstOrDefault();
                    if (chk_exist == null) {
                        db.custom_board_in_user.Add(data);
                        db.SaveChanges();
                    } else {
                        chk_exist.board_name = data.board_name;
                        chk_exist.board_desc = data.board_desc;
                        chk_exist.is_active = data.is_active;
                        db.SaveChanges();
                    }
                }
                CreateDefaultParamAll(data.board_id);
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
        public static I_BasicResult RemoveCustomBoard(string board_id) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.custom_board_in_user.RemoveRange(db.custom_board_in_user.Where(o => o.board_id == board_id));
                    db.custom_widget_param_in_user.RemoveRange(db.custom_widget_param_in_user.Where(o => o.board_id == board_id));
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
        public static custom_widget_master GetWidgetInfo(string widget_id) {
            custom_widget_master output = new custom_widget_master();
            try {
                using (cimsContext db = new cimsContext()) {
                    output= db.custom_widget_master.Where(o => o.widget_id == widget_id).FirstOrDefault(); 
                }
            } catch (Exception ex) { 
            }
            return output;
        }

        public static I_BasicResult AddWidget(List<vw_custom_widget_in_user> data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var m_data = data.FirstOrDefault();
                if (m_data == null) {
                    return r;
                }
                using (cimsContext db = new cimsContext()) {
                    var baord_info = db.custom_board_in_user.Where(o => o.board_id == m_data.board_id).FirstOrDefault();
                    db.custom_widget_in_user.RemoveRange(db.custom_widget_in_user.Where(o => o.board_id == m_data.board_id && o.widget_id == m_data.widget_id));

                    foreach (var d in data) {
                        custom_widget_in_user n = new custom_widget_in_user();

                        n.board_id = baord_info.board_id;
                        n.username = baord_info.username;
                        n.board_id = d.board_id;
                        n.widget_id = d.widget_id;
                        n.v_rowspan = d.v_rowspan;
                        n.v_colspan = d.v_colspan;
                        n.h_rowspan = d.h_rowspan;
                        n.h_colspan = d.h_colspan;
                        n.h_sort = d.h_sort;
                        n.v_sort = d.v_sort;
                        n.is_active = d.is_active;
                        db.custom_widget_in_user.Add(n);

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
        public static I_BasicResult SaveWidget(List<vw_custom_widget_in_user> data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var m_data = data.FirstOrDefault();
                if (m_data == null) {
                    return r;
                }
                using (cimsContext db = new cimsContext()) {
                    var baord_info = db.custom_board_in_user.Where(o => o.board_id == m_data.board_id).FirstOrDefault();
                    db.custom_widget_in_user.RemoveRange(db.custom_widget_in_user.Where(o => o.board_id == m_data.board_id));

                    foreach (var d in data) {
                        custom_widget_in_user n = new custom_widget_in_user();

                        n.board_id = baord_info.board_id;
                        n.username = baord_info.username;
                        n.board_id = d.board_id;
                        n.widget_id = d.widget_id;
                        n.v_rowspan = d.v_rowspan;
                        n.v_colspan = d.v_colspan;
                        n.h_rowspan = d.h_rowspan;
                        n.h_colspan = d.h_colspan;
                        n.h_sort = d.h_sort;
                        n.v_sort = d.v_sort;
                        n.is_active = d.is_active;
                        db.custom_widget_in_user.Add(n);

                    }
                    db.SaveChanges();
                    CreateDefaultParamAll(m_data.board_id);
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
        public static I_BasicResult RemoveWidget(string board_id, string widget_id) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.custom_widget_in_user.RemoveRange(db.custom_widget_in_user.Where(o => o.board_id == board_id && o.widget_id == widget_id));
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



        #region create param

        public static I_BasicResult CreateDefaultParamAll(string board_id) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                CreateDefaultParamG10(board_id);
                CreateDefaultParamG50(board_id);
            } catch (Exception ex) {
            }
            return r;
        }
        /// <summary>
        /// default param for กลุ่ม widget อุบัติเหตุ
        /// </summary> 
        /// 
        public static I_BasicResult CreateDefaultParamG10(string board_id) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string group_id = "10";
                //  ParamGroup10
                using (cimsContext db = new cimsContext()) {
                    var widget_in_group = db.custom_widget_master.Where(o => o.group_id == group_id && o.is_active == 1).ToList();
                    var board_inf = db.custom_board_in_user.Where(o => o.board_id == board_id).FirstOrDefault();
                    var exist_param = db.custom_widget_param_in_user.Where(o => o.board_id == board_id && o.group_id == group_id).ToList();
                    List<custom_widget_param_in_user> new_defualt_param = new List<custom_widget_param_in_user>();
                    ParamG10 g10 = new ParamG10();
                    g10.DateBegin = new DateTime(DateTime.Now.Year, 1, 1);
                    g10.DateEnd = new DateTime(DateTime.Now.Year, 12, 31);
                    foreach (var wg in widget_in_group) {
                        //add dateto param for default
                        var exist = exist_param.Where(o => o.widget_id == wg.widget_id && o.param_id == "filter_option").FirstOrDefault();
                        if (exist != null) {
                            continue;
                        }
                        custom_widget_param_in_user d = new custom_widget_param_in_user();
                        d.username = board_inf.username;
                        d.board_id = board_inf.board_id;
                        d.widget_id = wg.widget_id;
                        d.param_id = "filter_option";
                        d.data = "ระบุวันที่";//ปีใหม่, สงกรานต์
                        d.data_type = "datetime";
                        d.group_id = wg.group_id;
                        d.group_name = wg.group_name;
                        d.is_active = 1;
          
                        new_defualt_param.Add(d);
                        //var newDate = DateTime.ParseExact("20111120",  "yyyyMMdd",  CultureInfo.InvariantCulture);
                    }

                    foreach (var wg in widget_in_group) {
                        //add dateto param for default
                        var exist = exist_param.Where(o => o.widget_id == wg.widget_id && o.param_id == "date_begin").FirstOrDefault();
                        if (exist != null) {
                            continue;
                        }
                        custom_widget_param_in_user d = new custom_widget_param_in_user();
                        d.username = board_inf.username;
                        d.board_id = board_inf.board_id;
                        d.widget_id = wg.widget_id;
                        d.param_id = "date_begin";
                        d.data = g10.DateBegin.ToString("yyyyMMdd");
                        d.data_type = "datetime";
                        d.group_id = wg.group_id;
                        d.group_name = wg.group_name;
                        d.is_active = 1;
                        new_defualt_param.Add(d);
                        //var newDate = DateTime.ParseExact("20111120",  "yyyyMMdd",  CultureInfo.InvariantCulture);
                    }
                    //add dateend param for default
                    foreach (var wg in widget_in_group) {
                        var exist = exist_param.Where(o => o.widget_id == wg.widget_id && o.param_id == "date_end").FirstOrDefault();
                        if (exist != null) {
                            continue;
                        }
                        custom_widget_param_in_user d = new custom_widget_param_in_user();
                        d.username = board_inf.username;
                        d.board_id = board_inf.board_id;
                        d.widget_id = wg.widget_id;
                        d.param_id = "date_end";
                        d.data = g10.DateEnd.ToString("yyyyMMdd");
                        d.data_type = "datetime";
                        d.group_id = wg.group_id;
                        d.group_name = wg.group_name;
                        d.is_active = 1;
                        new_defualt_param.Add(d);
                        //var newDate = DateTime.ParseExact("20111120",  "yyyyMMdd",  CultureInfo.InvariantCulture);
                    }
                    //add all province for default 
                    #region create province filter
                    var pnames = CustomBoardService.ListProvince("name");
                    int i = 1;
                    string allprovince = "";
                    foreach (var p in pnames) {
                        if (i == 1) {
                            allprovince = p;
                        } else {
                            allprovince = allprovince+"," + p;
                        }
                        i++;
                    }
                    #endregion
                    foreach (var wg in widget_in_group) {
                        var exist = exist_param.Where(o => o.widget_id == wg.widget_id && o.param_id == "provinces").FirstOrDefault();
                        if (exist != null) {
                            continue;
                        }
                        custom_widget_param_in_user d = new custom_widget_param_in_user();
                        d.username = board_inf.username;
                        d.board_id = board_inf.board_id;
                        d.widget_id = wg.widget_id;
                        d.param_id = "provinces";
                        d.data = allprovince;
                        d.data_type = "string";
                        d.group_id = wg.group_id;
                        d.group_name = wg.group_name;
                        d.is_active = 1;
                        new_defualt_param.Add(d);
                        //var newDate = DateTime.ParseExact("20111120",  "yyyyMMdd",  CultureInfo.InvariantCulture);
                    }

                    db.custom_widget_param_in_user.AddRange(new_defualt_param);
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

        /// <summary>
        /// default param for กลุ่ม widget กลุ่ม 50
        /// </summary> 
        /// 
        public static I_BasicResult CreateDefaultParamG50(string board_id) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string group_id = "50";
                //  ParamGroup10
                using (cimsContext db = new cimsContext()) {
                    var widget_in_group = db.custom_widget_master.Where(o => o.group_id == group_id && o.is_active == 1).ToList();
                    var board_inf = db.custom_board_in_user.Where(o => o.board_id == board_id).FirstOrDefault();
                    var exist_param = db.custom_widget_param_in_user.Where(o => o.board_id == board_id && o.group_id == group_id).ToList();
                    List<custom_widget_param_in_user> new_defualt_param = new List<custom_widget_param_in_user>();
                    ParamG50 g50 = new ParamG50();
                    g50.DateBegin = new DateTime(DateTime.Now.Year, 1, 1);
                    g50.DateEnd = new DateTime(DateTime.Now.Year, 12, 31);
                 

                    foreach (var wg in widget_in_group) {
                        //add dateto param for default
                        var exist = exist_param.Where(o => o.widget_id == wg.widget_id && o.param_id == "date_begin").FirstOrDefault();
                        if (exist != null) {
                            continue;
                        }
                        custom_widget_param_in_user d = new custom_widget_param_in_user();
                        d.username = board_inf.username;
                        d.board_id = board_inf.board_id;
                        d.widget_id = wg.widget_id;
                        d.param_id = "date_begin";
                        d.data = g50.DateBegin.ToString("yyyyMMdd");
                        d.data_type = "datetime";
                        d.group_id = wg.group_id;
                        d.group_name = wg.group_name;
                        d.is_active = 1;
                        new_defualt_param.Add(d);
                        //var newDate = DateTime.ParseExact("20111120",  "yyyyMMdd",  CultureInfo.InvariantCulture);
                    }
                    //add dateend param for default
                    foreach (var wg in widget_in_group) {
                        var exist = exist_param.Where(o => o.widget_id == wg.widget_id && o.param_id == "date_end").FirstOrDefault();
                        if (exist != null) {
                            continue;
                        }
                        custom_widget_param_in_user d = new custom_widget_param_in_user();
                        d.username = board_inf.username;
                        d.board_id = board_inf.board_id;
                        d.widget_id = wg.widget_id;
                        d.param_id = "date_end";
                        d.data = g50.DateEnd.ToString("yyyyMMdd");
                        d.data_type = "datetime";
                        d.group_id = wg.group_id;
                        d.group_name = wg.group_name;
                        d.is_active = 1;
                        new_defualt_param.Add(d);
                        //var newDate = DateTime.ParseExact("20111120",  "yyyyMMdd",  CultureInfo.InvariantCulture);
                    }
                   

                    db.custom_widget_param_in_user.AddRange(new_defualt_param);
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

        public static List<string> ListProvince(string field) {
            List<string> output = new List<string>();
            try {
                using (cimsContext db = new cimsContext()) {
                    if (field == "name") {
                        output = db.a_province.OrderBy(o => o.pname).Select(o => o.pname).ToList();
                    }
                }
            } catch (Exception ex) {
            }
            return output;
        }
        #endregion


        #region param
        public static List<custom_widget_param_in_user> GetWidgetParam(string board_id, string widget_id) {
            List<custom_widget_param_in_user> output = new List<custom_widget_param_in_user>();
            try {
                using (cimsContext db = new cimsContext()) {
                    output = db.custom_widget_param_in_user.Where(o => o.board_id == board_id && o.widget_id == widget_id && o.is_active == 1).ToList();
                }
            } catch (Exception ex) {

            }
            return output;
        }
        public static I_BasicResult SaveWidgetParam(List<custom_widget_param_in_user> data, int? save_all_in_group) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            save_all_in_group = save_all_in_group == null ? 0 : save_all_in_group;
            try {
                using (cimsContext db = new cimsContext()) {
                    var group_id = data.FirstOrDefault().group_id;
                var board_id = data.FirstOrDefault().board_id;
                    var widget_id = data.FirstOrDefault().widget_id;
                    if (save_all_in_group==1) {//save ค่าเดียวกันทุก widget ในกลุ่ม
                    if (group_id == "10") {//อุบัติเหตุ
                            db.custom_widget_param_in_user.RemoveRange(db.custom_widget_param_in_user.Where(o => o.board_id == board_id));
                            db.SaveChanges();
                         CreateDefaultParamG10(board_id);
                            var query = db.custom_widget_param_in_user.Where(o => o.board_id == board_id  ).ToList();
                            foreach (var q in query) {
                                var new_data = data.Where(o => o.board_id == board_id && o.widget_id == q.widget_id && o.data_type == q.data_type && o.group_id == q.group_id).FirstOrDefault();
                                if (new_data!=null) {
                                    q.data = new_data.data;
                                }  
                            }
                            db.SaveChanges();
                    }
                    } else {
                        db.custom_widget_param_in_user.RemoveRange(db.custom_widget_param_in_user.Where(o => o.board_id == board_id && o.widget_id==widget_id));
                        db.custom_widget_param_in_user.AddRange(data); 
                        db.SaveChanges();
                    } 
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException!=null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
             

            }
            return r;
        }
        #endregion
    }
}
