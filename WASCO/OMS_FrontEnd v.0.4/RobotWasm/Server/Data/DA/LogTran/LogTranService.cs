using RobotWasm.Server.Data.TFEDBF;
using RobotWasm.Shared.Data.TFEDBF;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.LogTran {
    public class LogTranService {
        public static List<y_trans_logs> ListLogs(string doc_id, string app_id, string module) {
            List<y_trans_logs> output = new List<y_trans_logs>();
            try {
                doc_id = doc_id == null ? "" : doc_id.ToLower();
                app_id = app_id == null ? "" : app_id.ToLower();
                module = module == null ? "" : module.ToLower();
                using (TFEDBFContext db = new TFEDBFContext()) {
                    output = db.y_trans_logs.Where(o => o.doc_id.ToLower() == doc_id && o.app_id.ToLower() == app_id && o.module.ToLower() == module).ToList();
                }
            } catch (Exception ex) {

            }
            return output;
        }

        public static List<y_trans_logs> ListLogsHistory(string app_id, string search) {
            List<y_trans_logs> output = new List<y_trans_logs>();
            try {
                app_id = app_id == null ? "" : app_id.ToLower();
                search = search == null ? "" : search.ToLower();
                using (TFEDBFContext db = new TFEDBFContext()) {
                    output = db.y_trans_logs.Where(o => o.app_id.ToLower() == app_id
                                                        && (
                                                                       o.module.ToLower().Contains(search)
                                                                    || o.fullname.ToLower().Contains(search)
                                                                    || o.module.ToLower().Contains(search)
                                                                    || search == ""
                                                        )
                                                        ).OrderByDescending(o => o.log_date).Take(1000).ToList();
                }
            } catch (Exception ex) {

            }
            return output;
        }

        public static I_BasicResult CreateLog(y_trans_logs input) {
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
                using (TFEDBFContext db = new TFEDBFContext()) {
                    db.y_trans_logs.Add(input);
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
