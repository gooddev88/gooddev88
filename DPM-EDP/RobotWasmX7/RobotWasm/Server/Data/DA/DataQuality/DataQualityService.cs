using Dapper;
using Npgsql;
using RobotWasm.Client.Service.Api;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Data.DA.DataQuality {
    public class DataQualityService {

        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection(Globals.CimsConn);
            }
        }

        public static List<dqt_data_logs> ListDataQuality(string? search, DateTime? datebegin, DateTime? dateend) {
            search = search == null ? "" : search.ToLower();
            datebegin = datebegin == null ? DateTime.Now.Date : datebegin;
            dateend = dateend == null ? DateTime.Now.Date : dateend;

            List<dqt_data_logs> output = new List<dqt_data_logs>();
            string append = "";
            //if (!string.IsNullOrEmpty(search)) {
            //    append = " and data_set_id like '%@search%'";
            //} 
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();

                    if (!string.IsNullOrEmpty(search)) {
                        sql =String.Format( @"
                                    select 
		                                     log_id 
		                                    ,job_id
                                            ,data_set_id
                                            ,job_date
                                            ,job_time
                                            ,job_result
                                            ,job_message
                                            ,filename
                                    from etl.dqt_data_logs
                                    where 
                                     (
                                         lower(data_set_id)  like '%{0}%'
                                      or  lower(job_result)  like '%{0}%'  
                                      or  lower(job_message)  like '%{0}%'  
                                       
                                     )


                                    order by job_date desc
                  ",search);
                    } else {
                        sql = @"
                                    select 
		                                     log_id 
		                                    ,job_id
                                            ,data_set_id
                                            ,job_date
                                            ,job_time
                                            ,job_result
                                            ,job_message
                                            ,filename
                                    from etl.dqt_data_logs
                                    where job_date between @datebegin and @dateend                    
                                    order by job_date desc
                  ";
                    }
                    var dynamicParameters = new DynamicParameters();
                    if (!string.IsNullOrEmpty(search)) {
                        dynamicParameters.Add("search", search);
                    } else {
                        dynamicParameters.Add("datebegin", datebegin);
                        dynamicParameters.Add("dateend", dateend);
                    }
                    output = conn.Query<dqt_data_logs>(sql, dynamicParameters).ToList();

                }
            } catch (Exception ex) {
            }
            return output;
        }


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

        public static List<trans_logs> ListLogsHistory(string app_id,string search,DateTime? DateFrom, DateTime? DateTo) {
            List<trans_logs> output = new List<trans_logs>();
            try {
                app_id = app_id == null ? "" : app_id.ToLower();
                search = search == null ? "" : search.ToLower();
                DateFrom = DateFrom == null ? DateTime.Now.Date : DateFrom;
                DateTo = DateTo == null ? DateTime.Now.Date : DateTo;
                using (cimsContext db = new cimsContext()) {
                    if (!string.IsNullOrEmpty(search)) {
                        output = db.trans_logs.Where(o => o.app_id.ToLower() == app_id
                                    && (
                                                   o.module.ToLower().Contains(search)
                                                || o.fullname.ToLower().Contains(search)
                                                || o.module.ToLower().Contains(search)
                                                || search == ""
                                    )
                                    ).OrderByDescending(o => o.log_date).Take(1000).ToList();
                    } else {
                        output = db.trans_logs.Where(o =>
                        o.app_id.ToLower() == app_id
                        && (o.log_date.Value.Date >= DateFrom && o.log_date.Value.Date <= DateTo)
                        ).OrderByDescending(o => o.log_date).ToList();
                    }
                }
            } catch (Exception ex) {

            }
            return output;
        }



    }
}
