using Dapper;
using Npgsql;
using RobotAPI.Data.CimsDB.TT; 
using RobotAPI.Models.Accident;
using System.Data;
using System.Data.SqlClient;

namespace RobotAPI.Data.DA.Accident
{
    public class AccidentDashboardService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }


        #region for accident dashboard
        public static List<AccindentPatiesAgeRange> GetAccindentPatiesAgeRange(AccidentCountSetParam input) {
            List<AccindentPatiesAgeRange> result = new List<AccindentPatiesAgeRange>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    //string strProvince = "";
                    //if (!input.isGetAllProvince) {
                    //    string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                    //    strProvince = " and province in  (" + parampv + ")  ";
                    //}
                    conn.Open();
                    sql = string.Format(@"
                        select paties_agerange , count(paties_agerange) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend 
                        group by  paties_agerange order by paties_agerange
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentPatiesAgeRange>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }
        public static AccidentCountSetResult AccidentCountSet(AccidentCountSetParam input) {
            AccidentCountSetResult result = new AccidentCountSetResult();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    //string strProvince = "";
                    //string ProvinceList = "";
                    //if (!input.isGetAllProvince) {
                    //    string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                    //    strProvince = " and province in  (" + parampv + ")  ";
                    //}
                    conn.Open();
                    sql = string.Format(@"
                        select 
	                        (select count(*) as count_result from accident_data where accident_date between @datebegin and @dateend  ) as most_event
	                        ,(select sum(parties_qty) sum_result from accident_data where injured_status = 'บาดเจ็บ' and accident_date between @datebegin and @dateend  ) as most_injured
	                        ,(select sum(parties_qty) sum_result from accident_data where injured_status = 'เสียชีวิต' and accident_date between @datebegin and @dateend   ) as most_death
	                        , round((select sum(parties_qty) sum_result from accident_data where injured_status = 'เสียชีวิต' and accident_date between @datebegin and @dateend   ) * 100 
	                         / (select count(*) as count_result from accident_data where accident_date between @datebegin and @dateend  ) , 2 ) as death_Rate
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);

                    result = conn.Query<AccidentCountSetResult>(sql, dynamicParameters).FirstOrDefault();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }         
        public static List<AccindentBehaviors> GetAccindentBehaviors(AccidentCountSetParam input) {
            List<AccindentBehaviors> result = new List<AccindentBehaviors>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    //string strProvince = "";
                    //if (!input.isGetAllProvince) {
                    //    string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                    //    strProvince = " and province in  (" + parampv + ")  ";
                    //}
                    conn.Open();
                    //sql = string.Format(@"
                    //    select behaviors , count(behaviors) as Count_Result from accident_data 
                    //    where accident_date between @datebegin and @dateend {0}
                    //    group by  behaviors order by behaviors
                    //", strProvince);

                    sql = string.Format(@"
                                    select 
		                                    count(*) count_result
		                                    ,behavior
                                    from (
                                            select behavior
                                            from   accident_data, unnest(string_to_array(behaviors, ' | ')) behavior
                                            where accident_date between @datebegin and @dateend  
                                    ) as b
                                    group by behavior
                                    order by count(*) desc
                    ");



                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentBehaviors>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }
        public static List<AccindentRoadTypeSet> GetAccindentRoadTypeSet(AccidentCountSetParam input) {
            List<AccindentRoadTypeSet> result = new List<AccindentRoadTypeSet>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    //string strProvince = "";
                    //if (!input.isGetAllProvince) {
                    //    string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                    //    strProvince = " and province in  (" + parampv + ")  ";
                    //}
                    conn.Open();
                    sql = string.Format(@"
                        select road_type , count(road_type) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend  
                        group by  road_type order by road_type
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentRoadTypeSet>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }
        public static List<AccidentCauseSetResult> GetAccidentCause(AccidentCountSetParam input) {
            List<AccidentCauseSetResult> result = new List<AccidentCauseSetResult>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    //string strProvince = "";
                    //if (!input.isGetAllProvince) {
                    //    string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                    //    strProvince = " and province in  (" + parampv + ")  ";
                    //}
                    conn.Open();
                    sql = string.Format(@"
                                select 
		                                    count(*) count_result
		                                    ,cause
                                    from (
                                            select cause
                                            from   accident_data, unnest(string_to_array(accident_cause, ' | ')) cause
                                            where accident_date between @datebegin and @dateend  
                                    ) as b
                                    group by cause
                                    order by count(*) desc
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccidentCauseSetResult>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }

        public static List<EventInProvince> GetEventInProvince(AccidentCountSetParam input) {
            List<EventInProvince> result = new List<EventInProvince>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
 
                    conn.Open();
                    sql = string.Format(@"
	                    select 
	                        province_id as  province
		                    ,(select count(*)    from accident_data o  where p.province_id=o.province and o.accident_date between @datebegin and @dateend ) as event_qty					
		                    ,(select count(*)    from accident_data o  where p.province_id=o.province and o.injured_status='เสียชีวิต' and o.accident_date between @datebegin and @dateend) as death_qty			
		                    ,(select count(*)    from accident_data o  where p.province_id=o.province and o.injured_status='บาดเจ็บ' and o.accident_date between @datebegin and @dateend) as injured_qty
	                    from mas_province p
                        where 1=1  
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<EventInProvince>(sql, dynamicParameters).ToList();
                }


            } catch (Exception) {

                throw;
            }

            return result;
        }
        public static List<AccindentTimeRngSet> GetAccindentTimeRngSet(AccidentCountSetParam input) {
            //copy แล้ว
            List<AccindentTimeRngSet> result = new List<AccindentTimeRngSet>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    //string strProvince = "";
                    //if (!input.isGetAllProvince) {
                    //    string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                    //    strProvince = " and province in  (" + parampv + ")  ";
                    //}
                    conn.Open();
                    sql = string.Format(@"
                        select accident_timerange , count(accident_timerange) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend
                        group by  accident_timerange order by accident_timerange
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentTimeRngSet>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }
        public static List<AccindentVehicleSubtype> GetAccindentVehicleSubtypeSet(AccidentCountSetParam input) {
            List<AccindentVehicleSubtype> result = new List<AccindentVehicleSubtype>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@"
                        select vehicle_subtype , count(vehicle_subtype) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend 
                        group by  vehicle_subtype order by vehicle_subtype
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentVehicleSubtype>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }
        public static List<AccindentLocationSet> GetAccindentLocationSet(AccidentCountSetParam input) {
            List<AccindentLocationSet> result = new List<AccindentLocationSet>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@"
                        select subdistrict ,district,province, lat,lon from accident_data 
                        where accident_date between @datebegin and @dateend 
                    ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentLocationSet>(sql, dynamicParameters).ToList();

                }


            } catch (Exception) {

                throw;
            }

            return result;
        }

        #endregion




        #region ไม่น่าจะใช้แล้ว
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

        public static string[] GetProvince() {
            string[] result;
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    //select distinct province as province_id from accident_data order by province
                    sql = @"

                            select province_id from mas_province order by province_id
                    ";
                    var res = conn.Query<string>(sql).ToList();
                    result = res.ToArray();
                }


            } catch (Exception) {

                throw;
            }

            return result;

        }

  
     
        internal static List<accident_data> GetAccindentAllSet(AccidentCountSetParam input) {
            List<accident_data> result = new List<accident_data>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince) {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  (" + parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select * from accident_data 
                        where accident_date between @datebegin and @dateend {0}
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<accident_data>(sql, dynamicParameters).ToList();
                }


            } catch (Exception) {

                throw;
            }

            return result;
        }

        #endregion

    }
}
