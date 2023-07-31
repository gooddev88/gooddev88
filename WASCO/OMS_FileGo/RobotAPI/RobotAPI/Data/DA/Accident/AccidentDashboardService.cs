using Dapper;
using Npgsql;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Models.Accident;
using System.Data;
using System.Data.SqlClient;

namespace RobotAPI.Data.DA.Accident
{
    public class AccidentDashboardService
    {
        public static IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection((Globals.DataStoreConn));
            }
        }

        public static List<AccidentCountProvinceResult> GetTopProvinceHasAccident(DateTime datebegin, DateTime dateend, int top, string type)
        {
            List<AccidentCountProvinceResult> result = new List<AccidentCountProvinceResult>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    if (type == "most_event")
                    { //อุบัติเหตสูงสุด

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

                    if (type == "most_deceased")
                    {//เสียชีวิตสูงสุด

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
                    if (type == "most_injured")
                    {//บาดเจ็บสูงสุด

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


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public static List<AccidentCountResult> GetCountType(DateTime datebegin, DateTime dateend)
        {
            List<AccidentCountResult> result = new List<AccidentCountResult>();
            try
            {
                using (IDbConnection conn = Connection)
                {
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


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }



        public static List<AccidentCountResult> GetCountEventInEachHour(DateTime datebegin, DateTime dateend)
        {
            List<AccidentCountResult> result = new List<AccidentCountResult>();
            try
            {
                using (IDbConnection conn = Connection)
                {
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


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }



        public static string[] GetProvince()
        {
            string[] result;
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";

                    conn.Open();
                    sql = @"
                            select province_id from mas_province order by province_id
                    ";
                    var res =  conn.Query<string>(sql).ToList();
                    result = res.ToArray();
                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;

        }
        public static AccidentCountSetResult AccidentCountSet(AccidentCountSetParam input)
        {
            AccidentCountSetResult result = new AccidentCountSetResult();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    string ProvinceList = "";
                    if (!input.isGetAllProvince)
                    {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  ("+ parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select 
	                        (select count(*) as count_result from accident_data where accident_date between @datebegin and @dateend {0} ) as most_event
	                        ,(select sum(parties_qty) sum_result from accident_data where injured_status = 'บาดเจ็บ' and accident_date between @datebegin and @dateend {0} ) as most_injured
	                        ,(select sum(parties_qty) sum_result from accident_data where injured_status = 'เสียชีวิต' and accident_date between @datebegin and @dateend {0} ) as most_death
	                        , round((select sum(parties_qty) sum_result from accident_data where injured_status = 'เสียชีวิต' and accident_date between @datebegin and @dateend {0} ) * 100 
	                         / (select count(*) as count_result from accident_data where accident_date between @datebegin and @dateend  {0} ) , 2 ) as death_Rate
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);

                    result = conn.Query<AccidentCountSetResult>(sql, dynamicParameters).FirstOrDefault();

                }

                 
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        internal static List<AccidentCaseSetResult> GetAccidentCauseSet(AccidentCountSetParam input)
        {
            List<AccidentCaseSetResult> result = new List<AccidentCaseSetResult>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince)
                    {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  (" + parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select to_char(accident_date , 'DD/MM') as accident_date ,accident_cause , count(accident_cause) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend {0}
                        group by  to_char(accident_date , 'DD/MM') , accident_cause order by to_char(accident_date , 'DD/MM'),accident_cause
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccidentCaseSetResult>(sql, dynamicParameters).ToList();

                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        internal static List<AccindentTimeRngSet> GetAccindentTimeRngSet(AccidentCountSetParam input)
        {
            List<AccindentTimeRngSet> result = new List<AccindentTimeRngSet>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince)
                    {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  (" + parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select accident_timerange , count(accident_timerange) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend {0}
                        group by  accident_timerange order by accident_timerange
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentTimeRngSet>(sql, dynamicParameters).ToList();

                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        internal static List<AccindentPatiesAgeRange> GetAccindentPatiesAgeRange(AccidentCountSetParam input)
        {
            List<AccindentPatiesAgeRange> result = new List<AccindentPatiesAgeRange>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince)
                    {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  (" + parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select paties_agerange , count(paties_agerange) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend {0}
                        group by  paties_agerange order by paties_agerange
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentPatiesAgeRange>(sql, dynamicParameters).ToList();

                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        internal static List<AccindentBehaviors> GetAccindentBehaviors(AccidentCountSetParam input)
        {
            List<AccindentBehaviors> result = new List<AccindentBehaviors>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince)
                    {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  (" + parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select behaviors , count(behaviors) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend {0}
                        group by  behaviors order by behaviors
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentBehaviors>(sql, dynamicParameters).ToList();

                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        internal static List<AccindentVehicleSubtype> GetAccindentVehicleSubtypeSet(AccidentCountSetParam input)
        {
            List<AccindentVehicleSubtype> result = new List<AccindentVehicleSubtype>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince)
                    {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  (" + parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select vehicle_subtype , count(vehicle_subtype) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend {0}
                        group by  vehicle_subtype order by vehicle_subtype
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentVehicleSubtype>(sql, dynamicParameters).ToList();

                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        internal static List<AccindentRoadTypeSet> GetAccindentRoadTypeSet(AccidentCountSetParam input)
        {
            List<AccindentRoadTypeSet> result = new List<AccindentRoadTypeSet>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince)
                    {
                        string parampv = string.Join(",", input.Province.Select(item => "'" + item + "'"));
                        strProvince = " and province in  (" + parampv + ")  ";
                    }
                    conn.Open();
                    sql = string.Format(@"
                        select road_type , count(road_type) as Count_Result from accident_data 
                        where accident_date between @datebegin and @dateend {0}
                        group by  road_type order by road_type
                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", input.DateBegin);
                    dynamicParameters.Add("dateend", input.DateEnd);
                    result = conn.Query<AccindentRoadTypeSet>(sql, dynamicParameters).ToList();

                }


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        internal static List<accident_data> GetAccindentAllSet(AccidentCountSetParam input)
        {
            List<accident_data> result = new List<accident_data>();
            try
            {
                using (IDbConnection conn = Connection)
                {
                    string sql = "";
                    string strProvince = "";
                    if (!input.isGetAllProvince)
                    {
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


            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
    }
}
