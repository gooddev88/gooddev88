using RobotWasm.Client.Service.Api;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.LogTran {
    public class LogTranService {
        public static List<trans_logs> ListLogs(string doc_id, string app_id, string module) {
            List<trans_logs> output = new List<trans_logs>();
            try {
                doc_id = doc_id == null ? "" : doc_id.ToLower();
                app_id = app_id == null ? "" : app_id.ToLower();
                module = module == null ? "" : module.ToLower();
                using (cimsContext db = new cimsContext()) {
                    output = db.trans_logs.Where(o => o.doc_id.ToLower() == doc_id && o.app_id.ToLower() == app_id && o.module.ToLower() == module).ToList();
                }
            } catch (Exception ex) {

            }
            return output;
        }

        public static List<vw_trans_logs> ListLogsHistory(string app_id,string search,DateTime? DateFrom, DateTime? DateTo) {
            List<vw_trans_logs> output = new List<vw_trans_logs>();
            try {
                app_id = app_id == null ? "" : app_id.ToLower();
                search = search == null ? "" : search.ToLower();
                DateFrom = DateFrom == null ? DateTime.Now.Date : DateFrom;
                DateTo = DateTo == null ? DateTime.Now.Date : DateTo;
                using (cimsContext db = new cimsContext()) {
                    if (!string.IsNullOrEmpty(search)) {
                        output = db.vw_trans_logs.Where(o => o.app_id.ToLower() == app_id
                                    && (
                                                   o.module.ToLower().Contains(search)
                                                || o.name.ToLower().Contains(search)
                                                || o.doc_id.ToLower().Contains(search)
                                                || o.username.ToLower().Contains(search)
                                                || o.log_desc.ToLower().Contains(search)
                                                || search == ""
                                    )
                                    && o.username != "x"
                                    ).OrderByDescending(o => o.log_date).Take(1000).ToList();
                    } else {
                        output = db.vw_trans_logs.Where(o =>
                        o.app_id.ToLower() == app_id
                        && (o.log_date.Value.Date >= DateFrom && o.log_date.Value.Date <= DateTo)
                        && o.username != "x"
                        ).OrderByDescending(o => o.log_date).ToList();
                    }
                }
            } catch (Exception ex) {

            }
            return output;
        }

        public static I_BasicResult CreateTransLog(string docid, string user,string module,string action)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                trans_logs n = new trans_logs();
                n.module = module;
                n.app_id = "edp";
                n.doc_id = docid;
                n.username = user;
                n.action = module;
                n.log_date = DateTime.Now;
                n.log_desc = action;

                r = CreateLog(n);
            }
            catch (Exception ex)
            {
                r.Result = "fail";
                r.Message1 = ex.Message;
            }
            return r;
        }

        public static I_BasicResult CreateLog(trans_logs input) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                if (string.IsNullOrEmpty(input.module)) {
                    r.Result = "fail";
                    r.Message1 = "ค่า module ไม่สามารถเป็นค่าว่างได้";
                    return r;
                }
                if (string.IsNullOrEmpty(input.app_id)) {
                    r.Result = "fail";
                    r.Message1 = "ค่า app_id ไม่สามารถเป็นค่าว่างได้";
                    return r;
                }
                if (input.log_date == null) {
                    r.Result = "fail";
                    r.Message1 = "ค่า log_date ไม่สามารถเป็นค่าว่างได้";
                    return r;
                }
                if (string.IsNullOrEmpty(input.doc_id)) {
                    r.Result = "fail";
                    r.Message1 = "ค่า doc_id ไม่สามารถเป็นค่าว่างได้";
                    return r;
                }
                if (string.IsNullOrEmpty(input.username)) {
                    r.Result = "fail";
                    r.Message1 = "ค่า username ไม่สามารถเป็นค่าว่างได้";
                    return r;
                }
                if (string.IsNullOrEmpty(input.action)) {
                    r.Result = "fail";
                    r.Message1 = "ค่า aciton ไม่สามารถเป็นค่าว่างได้";
                    return r;
                }
                using (cimsContext db = new cimsContext()) {
                    db.trans_logs.Add(input);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.ToString();
                }
            }
            return r;
        }
    }

}
