using Dapper;
using Npgsql; 
using Robot.Data.ML.DataStoreModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Robot.Data.DA.DataStore {
    public class AccidentDashboardService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.DataStoreConn));
            }
        }

        public static List<AccidentCountProvinceResult> GetTopProvinceHasAccident(DateTime datebegin, DateTime dateend, int top, string type) {
            List<AccidentCountProvinceResult> result = new List<AccidentCountProvinceResult>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    if (type == "most_event") { //อุบัติเหตสูงสุด

                        conn.Open();
                        sql = @"
                    select
                       province
                    ,count(*) as count_result
                    from data_accidents
                    where accident_date between @datebegin and @dateend
                    group by province
                    order by count(*) desc
                    limit @top
                    ";
                    }

                    if (type == "most_deceased") {//เสียชีวิตสูงสุด

                        conn.Open();
                        sql = @"
                    select
                       province
                    ,count(*) as count_result
                    from data_accidents
                    where accident_date between @datebegin and @dateend
                    group by province
                    order by count(*) desc
                    limit @top
                    ";
                    }
                    if (type == "most_injured") {//บาดเจ็บสูงสุด

                        conn.Open();
                        sql = @"
                    select
                       province
                    ,count(*) as count_result
                    from data_accidents
                    where accident_date between @datebegin and @dateend
                    group by province
                    order by count(*) desc
                    limit @top
                    ";
                    }
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", datebegin);
                    dynamicParameters.Add("dateend", dateend);
                    dynamicParameters.Add("top", top);

                    result = conn.Query<AccidentCountProvinceResult>(sql, dynamicParameters).ToList();
                }


            } catch (Exception) {

                throw;
            }

            return result;
        }


        public static List<AccidentCountResult> GetCountType(DateTime datebegin, DateTime dateend) {
            List<AccidentCountResult> result = new List<AccidentCountResult>();
            try {
                using (IDbConnection conn = Connection) {
                    //string sql = "";

                    //    conn.Open();
                    //    sql = @"
                    //select
                    //   province
                    //,count(*) as count_result
                    //from data_accidents
                    //where accident_date between @datebegin and @dateend
                    //group by province
                    //order by count(*) desc
                    //limit @top
                    //"; 
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("datebegin", datebegin);
                    //dynamicParameters.Add("dateend", dateend); 

                    //result = conn.Query<AccidentCountResult>(sql, dynamicParameters).ToList();
                    result.Add(new AccidentCountResult { Type = "จำนวนอุบัติเหตุ", Count_Result = 100 });
                    result.Add(new AccidentCountResult { Type = "จำนวนผู้เสียชีวิต", Count_Result = 101 });
                    result.Add(new AccidentCountResult { Type = "จำนวนผู้บาดเจ็บ", Count_Result = 102 });

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }

        public static List<AccidentCountResult> GetCountEventInEachHour(DateTime datebegin, DateTime dateend) {
            List<AccidentCountResult> result = new List<AccidentCountResult>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = @"
                            select 
                                  lpad(cast(	date_part('hour', accident_time) as VARCHAR),2,'0') as type
	                            ,count(*) as count_result
                            from data_accidents  
                            where accident_date between @datebegin and @dateend
                            group by date_part('hour', accident_time)
                            order by date_part('hour', accident_time)
                    ";
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", datebegin);
                    dynamicParameters.Add("dateend", dateend);

                    result = conn.Query<AccidentCountResult>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }

    }
}
