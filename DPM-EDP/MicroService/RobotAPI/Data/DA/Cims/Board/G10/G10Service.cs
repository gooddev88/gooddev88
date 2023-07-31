using Dapper;
using Npgsql;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.ML.Board.BoardParam;
using RobotAPI.Data.ML.Board.Widget;
using RobotAPI.Data.ML.Data;
using RobotAPI.Helpers.Date;
using System.Data;
using System.Globalization;

namespace RobotAPI.Data.DA.Cims.Board.G10 {
    public class G10Service {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection(Globals.CimsConn);
            }
        }
        public static List<WG101Data> GetWG101(string? board_id, string? datebegin, string? dateend, string? province) {
            //3 จังหวัดเกิดอบุติเหตุสูงสุด
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;
            List<WG101Data> output = new List<WG101Data>();
           
            try {
                ParamG10 param = new ParamG10();
                if (board_id!="") {
                    param = ParamG10.GetParamBoard(board_id, "wg101");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                 
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                        province
                                                        ,count(*) as count_result
                                                    from etl.acd_accident_person
                                                    where accident_date between @datebegin and @dateend
                                                             {0}
                                                    group by province
                                                    order by  count(*) desc
                                                    limit 3

                    ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG101Data>(sql, dynamicParameters).ToList();
                }

            } catch (Exception) {
            }
            return output;
        }

        public static List<WG103Data> GetWG103(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกรายชั่วโมง
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;
   
            List<WG103Data> output = new List<WG103Data>();
    
            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg103");
                } else {
                    param = ParamG10.GetParamQString(datebegin,dateend,province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    
                    conn.Open();
                    sql = string.Format(@"
                        select 
                              accident_timerange  as TimeRange
                            , count(accident_timerange) as Count_Result 
                        from etl.acd_accident_person 
                        where accident_date between @datebegin and @dateend {0}
                        group by  accident_timerange order by accident_timerange
                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG103Data>(sql, dynamicParameters).ToList();

                } 
            } catch (Exception) {
            }
            return output;
        }



        public static List<WG104Data> GetWG104(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามสถานะภาพผู้ประสบเหตุ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province; 
            List<WG104Data> output = new List<WG104Data>(); 
            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg104");
                } else {
                    param = ParamG10.GetParamQString(datebegin,dateend,province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    
                    conn.Open();
                    sql = string.Format(@" 
                                    with a as (
                                    select  
                                            a.injured_status as injured_status
                                            ,count(*) as count_result
                                    from etl.acd_accident_person a
                                    where accident_date between @datebegin and @dateend {0}
                                    group by a.injured_status
                                    ), t as (
                                        select  
                                                sum(count_result) as count_total 
                                        from a  
                                    limit 1 
                                    )
                                    select 
                                         a.injured_status
                                        ,round((100 * a.count_result) / t.count_total,2) as percent
                                        ,a.count_result
                                        ,t.count_total
                                    from a
                                    cross join t 
                  ", strProvince);

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG104Data>(sql, dynamicParameters).ToList();

                } 
            } catch (Exception ex) {
                var xx = ex.Message;
            }
            return output;
        }

 public static List<WG105Data> GetWG105(string? board_id, string? datebegin, string? dateend, string? province) {
          //จำนวนอุบัติเหตุทางถนนตามเดือน
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province; 
            List<WG105Data> output = new List<WG105Data>(); 
            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg105");
                } else {
                    param = ParamG10.GetParamQString(datebegin,dateend,province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    
                    conn.Open();
                    sql = string.Format(@" 
                                            select 
                                               EXTRACT(MONTH from accident_date) as in_month
                                              ,EXTRACT(YEAR from accident_date) as in_year 
                                             ,count(*) as count_result 
                                            from etl.acd_accident_person
                                    where accident_date between @datebegin and @dateend {0}
                                            group by    EXTRACT(MONTH from accident_date) ,  EXTRACT(YEAR from accident_date)
 
                  ", strProvince);

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG105Data>(sql, dynamicParameters).OrderBy(o=>o.in_month).ToList();
                    foreach (var o in output) {
                        o.in_month_name = DateHelper.ConvertThaiMonthShortText(o.in_month);
                        o.in_month_color = DateHelper.ConvertThaiMonthColor(o.in_month);
                    }
                } 
            } catch (Exception ex) {
                var xx = ex.Message;
            }
            return output;
        }




   public static List<WG102Data> GetWG102(string? board_id, string? datebegin, string? dateend, string? province) {
            // จังหวัดที่มีผู้เสียชีวิตสูงสุด 3 อันดับ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;
            //จำนวนอุบัติเหตุแยกรายชั่วโมง
            List<WG102Data> output = new List<WG102Data>();
    
            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg102");
                } else {
                    param = ParamG10.GetParamQString(datebegin,dateend,province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    
                    conn.Open();
                    sql = string.Format(@"
                                    select 
		                                     province 
		                                    ,count(injured_status) as Count_Result 
                                    from etl.acd_accident_person
                                    where accident_date between @datebegin and @dateend {0}
                                                        and injured_status = 'เสียชีวิต'
                                    group by province
                                    order by count(injured_status) desc
                                    limit 3
                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG102Data>(sql, dynamicParameters).ToList();

                } 
            } catch (Exception) {
            }
            return output;
        }

        public static List<WG106Data> GetWG106(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุทางถนนตามเดือน
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;
            List<WG106Data> output = new List<WG106Data>();
            try {
               
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg106");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    //  sql = string.Format(@"  
                    //              SELECT
                    //                  vehicle_subtype
                    //                  ,accident_date
                    //                  ,count(*) as count_result
                    //              from etl.acd_accident_person
                    //              where accident_date between @datebegin and @dateend {0}
                    //              group by accident_date,vehicle_subtype  
                    //              order by accident_date 
                    //", strProvince);
                    //SELECT
                    //    vehicle_subtype
                    //    ,accident_date
                    //    ,count(*) as count_result
                    //from etl.acd_accident_person
                    //where accident_date between @datebegin and @dateend {0}
                    //group by accident_date,vehicle_subtype  
                    //order by accident_date 

                    sql = string.Format(@"    
                                select
                                     date_part('month', accident_date)  as in_month
									,date_part('year', accident_date)  as in_year
                                    ,count(*) as count_result
                                from etl.acd_accident_person
                                where injured_status='เสียชีวิต'
                                      and accident_date between @datebegin and @dateend {0}
                                group by date_part('month', accident_date) ,date_part('year', accident_date)
                  ", strProvince);


                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    var query = conn.Query<WG106Data>(sql, dynamicParameters).OrderBy(o=>o.in_year).ThenBy(o=>o.in_month).ToList();
                    foreach (var q in query){
                        WG106Data n = new WG106Data();
                        n.in_year = q.in_year;
                        n.in_month = q.in_month;
                        n.timename = q.in_year.ToString() + "-" + q.in_month.ToString("00");
                        n.count_result = q.count_result;
                        output.Add(n);
                    } 
                } 
        
            } catch (Exception ex) {
                var xx = ex.Message;
            }
            return output;
        }
        public static List<WG107Data> GetWG107(string? board_id, string? datebegin, string? dateend, string? province) {
            // จำนวนอุบัติเหตุตามช่วงอายุ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;
 
            List<WG107Data> output = new List<WG107Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg107");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@"
							  select
                                             paties_agerange as age_range
                                            ,count(*) as count_result
                                from etl.acd_accident_person
                                where accident_date between @datebegin and @dateend {0}
                                group by paties_agerange  
                                order by paties_agerange 
                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG107Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }

        public static List<WG108Data> GetWG108(string? board_id, string? datebegin, string? dateend, string? province) {
            // จำนวนอุบัติเหตุตามช่วงอายุ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG108Data> output = new List<WG108Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg108");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@"
 
                                            select

                                                            paties_agerange as age_range
                                                            ,parties_sex as sex
                                                            ,count(*) as count_result
                                            from etl.acd_accident_person
                                            where accident_date between @datebegin and @dateend {0}
                                            group by paties_agerange, parties_sex
                                            order by paties_agerange, parties_sex


                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG108Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG109Data> GetWG109(string? board_id, string? datebegin, string? dateend, string? province) {
            // จำนวนอุบัติเหตุแยกตามประเภทยานพาหนะ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG109Data> output = new List<WG109Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg109");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@"
 
                                            SELECT 
			                                            vehicle_subtype as vehicle
		                                            ,count(*) as count_result	
                                            FROM etl.acd_accident_person
                                            where accident_date between @datebegin and @dateend {0}
                                                                group by vehicle_subtype
                                                                order by vehicle_subtype


                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG109Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG110Data> GetWG110(string? board_id, string? datebegin, string? dateend, string? province) {
            // จำนวนอุบัติเหตุแยกตามสาเหตุการเกิดอุบัติเหตุ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG110Data> output = new List<WG110Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg110");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 

                                            SELECT 
                                                             a_cause 
                                                            ,count(*) as count_result
                                            FROM   etl.acd_accident_person, unnest(string_to_array(accident_cause, ' | ')) a_cause
                                            where accident_date between @datebegin and @dateend {0}
                                                                group by a_cause
                                            order by a_cause
 
                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG110Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
   public static List<WG111Data> GetWG111(string? board_id, string? datebegin, string? dateend, string? province) {
            //จังหวัดที่มีผู้บาดเจ็บสูงสุด 3 อันดับ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG111Data> output = new List<WG111Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg111");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 
                                                SELECT 
			                                                injured_status as injured_status
			                                                ,province
		                                                    ,count(*) as count_result	 
                                                 FROM etl.acd_accident_person
                                                 where injured_status = 'บาดเจ็บ'
                                                 and accident_date between @datebegin and @dateend {0}
                                                                    group by injured_status,province
                                                                    order by count(*) desc
                                                                    limit 3


                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG111Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG112Data> GetWG112(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามประเภทถนน
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG112Data> output = new List<WG112Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg112");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'","").Trim()!="") {
                      strProvince = " and province in  (" + parampv + ")  ";
                }
               
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 
                                                SELECT 
		 
		                                                 scene_of_road
		                                                ,count(*) as count_result	 
                                                FROM etl.acd_accident_person
                                                where   accident_date between @datebegin and @dateend {0}
                                                                    group by scene_of_road
                                                                    order by count(*) desc
                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG112Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }

     public static List<WG113Data> GetWG113(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามประเภทผิวจราจร
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG113Data> output = new List<WG113Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg113");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                if (parampv.Replace("'","").Trim()!="") {
                      strProvince = " and province in  (" + parampv + ")  ";
                }
               
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 
                                                
                                            select 
                                                             road_surface
                                                             ,count(*) as count_result
                                            from etl.acd_accident_person
                                            where   accident_date between @datebegin and @dateend {0}
                                            group by road_surface	
                                            order by road_surface

                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG113Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }

        public static List<WG114Data> GetWG114(string? board_id, string? datebegin, string? dateend, string? province) {
            //จุดเกิดอุบัติเหตุ (ละติจูด ลองจิจูด)
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG114Data> output = new List<WG114Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg114");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                string strProvince2 = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince2 = " and a.province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 
                                                
                                     select
                                             a.province,
		                                     a.lat as lat,
		                                     a.lon as lon,
		                                     x.count_result
                                    from etl.acd_accident_person a
                                    left join (
                                    SELECT 
		                                    province
		                                    ,count(*) as count_result
                                    FROM 
                                    etl.acd_accident_person
                                    where   accident_date between @datebegin and @dateend {0}
                                    group by province
                                    ) as x on a.province=x.province
                                    where   a.accident_date between @datebegin and @dateend {1}

                  ", strProvince, strProvince2);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG114Data>(sql, dynamicParameters).OrderBy(o=>o.province).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG115Data> GetWG115(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามประเภทจุดเกิดเหตุ
             
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG115Data> output = new List<WG115Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg115");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";

                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }

                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@"  
                              
                                    select 
                                         x.scene_of_road
                                         ,count(*) as count_result
                                    from etl.acd_accident_person x
                                    where x.accident_date between @datebegin and @dateend  
                                    and x.scene_of_road IS NOT NULL {0}
                                    group by x.scene_of_road 
                                    order by x.scene_of_road

                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG115Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception ex) {
                var xx = ex.Message;
            }
            return output;
        }
        public static List<WG116Data> GetWG116(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามภูมิลำเนา
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG116Data> output = new List<WG116Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg116");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
       
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 
                                                
                                    select
                                         x.province, 
		                                     count(*) as count_result
                                    from etl.acd_accident_person x
                                    where x.accident_date between @datebegin and @dateend {0}
                                    group by province 
                                    order by x.province

                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG116Data>(sql, dynamicParameters) .ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG117Data> GetWG117(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามสถานที่เสียชีวิต 
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG117Data> output = new List<WG117Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg117");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                string strProvince2 = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince2 = " and a.province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 
                                                
								  select
                                         x.death_place as death_place, 
		                                     count(*) as count_result
                                   from etl.acd_accident_person x
                                   where accident_date between @datebegin and @dateend and x.death_place <>''  {0}
                                    group by x.death_place 
                                    order by x.death_place 

                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG117Data>(sql, dynamicParameters).OrderBy(o => o.death_place).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG118Data> GetWG118(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามประเภทผู้นำส่ง
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG118Data> output = new List<WG118Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg118");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                string strProvince2 = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince2 = " and a.province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                   sql = string.Format(@" 
                                                
								  select
                                         x.help_by as help_by, 
		                                     count(*) as count_result
                                   from etl.acd_accident_person x
                                   where accident_date between @datebegin and @dateend {0}
                                    group by x.help_by 
                                    order by x.help_by 

                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG118Data>(sql, dynamicParameters).OrderBy(o => o.help_by).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG119Data> GetWG119(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนอุบัติเหตุแยกตามมูลค่าความเสียหาย
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG119Data> output = new List<WG119Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg119");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                string strProvince2 = "";
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince2 = " and a.province in  (" + parampv + ")  ";
                }
                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@" 
                                    select 
			                                    a_cause
			                                    ,round( avg(cast ( damage_value as decimal)),0) as damage_value
			                                    ,count(*) as count_result
                                    from (
                                                select 
                                                a_cause 
                                                ,damage_value
                                                FROM   etl.acd_accident_person, unnest(string_to_array(accident_cause, ' | ')) a_cause
                                                where accident_date between @datebegin and @dateend {0}
                                    ) as x
                                    group by a_cause
                                    order by a_cause		

                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG119Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception) {
            }
            return output;
        }

        public static List<WG120Data> GetWG120(string? board_id, string? datebegin, string? dateend, string? province) {
            //จำนวนผู้บาดเจ็บจากอุบัติเหตุทางถนน
             
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            province = province == null ? "" : province;

            List<WG120Data> output = new List<WG120Data>();

            try {
                ParamG10 param = new ParamG10();
                if (board_id != "") {
                    param = ParamG10.GetParamBoard(board_id, "wg120");
                } else {
                    param = ParamG10.GetParamQString(datebegin, dateend, province);
                }
                #region create province filter
                var ppp = param.Provinces.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";

                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province in  (" + parampv + ")  ";
                }

                #endregion
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    sql = string.Format(@"  
                                    select 
                                            x.vehicle_subtype
                                            ,count(*) as count_result
                                    from etl.acd_accident_person x
                                    where x.accident_date between @datebegin and @dateend  
                                    and injured_status ='บาดเจ็บ' {0}
                                    group by x.vehicle_subtype
                                    order by x.vehicle_subtype

                  ", strProvince);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG120Data>(sql, dynamicParameters).ToList();

                }
            } catch (Exception ex) {
                var xx = ex.Message;
            }
            return output;
        }

        #region wg20
        public static List<WG201Data> GetWG201(string? board_id, string? province) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด  
            board_id = board_id == null ? "" : board_id;
            province = province == null ? "" : province;
            List<WG201Data> output = new List<WG201Data>();
            try {
                #region create province filter
                var ppp = province.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province_name in  (" + parampv + ")  ";
                }

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();

                    sql = string.Format(@"  
                                                        select 
                                                                e.province_name as province_name
                                                                ,stuff_name as stuff_name
                                                                ,sum(e.count_in-e.count_out) as remain 
                                                        from etl.est_stock_cms e
                                                        left join public.a_section s on e.province_name=s.pname
                                                        where 1=1 {0}
                                                        group by  e.province_name ,stuff_name
                                                        order by e.province_name  
                        ", strProvince);
                    output = conn.Query< WG201Data>(sql).OrderBy(o => o.province_name).ToList(); 

                }

            } catch (Exception) {
            }
            return output;
        }
        public static List<WG202Data> GetWG202(string? board_id, string? unit) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามศูนย์เขต
            board_id = board_id == null ? "" : board_id;
            unit = unit == null ? "" : unit;
            List<WG202Data> output = new List<WG202Data>();
            try {
                #region create province filter
                var ppp = unit.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and s.section_code in  (" + parampv + ")  ";
                }

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();

                    sql = string.Format(@"   
                                            select 
                                                    s.section_name as unit
                                                    ,s.section_code as unit_code
                                                    ,stuff_name as stuff_name
                                                    ,sum(e.count_in-e.count_out) as remain 
                                            from etl.est_stock_cms e
                                            left join public.a_section s on e.province_name=s.pname
                                            where 1=1 {0} and s.section_code is not null
                                            group by  s.section_name,s.section_code ,stuff_name
                                            order by s.section_code 
                        ", strProvince);
                    output = conn.Query<WG202Data>(sql).OrderBy(o => o.unit_code).ToList(); 
                }

            } catch (Exception) {
            }
            return output;
        }
        #endregion

        #region wg301
        public static List<WG301Data> GetWG301(string? board_id, string? province) {
            //จำนวน อปพร. แยกตามเพศ 
            board_id = board_id == null ? "" : board_id;
            province = province == null ? "" : province;
            List<WG301Data> output = new List<WG301Data>();
            try {
                #region create province filter
                var ppp = province.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province_name in  (" + parampv + ")  ";
                }
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@" 
                                        select  
                                                 gender as gender,
                                                 count(*) as count_result
                                        from etl.dpm_volunteer
                                        where 1=1  {0}
                                        group by gender
 
                  ", strProvince); 
                    output = conn.Query<WG301Data>(sql).ToList();
                }
            } catch (Exception) {
            }
            return output;
        }

        public static List<WG302Data> GetWG302(string? board_id, string? province) {
            //จำนวน อปพร. แยกตามอายุ เพศ

            board_id = board_id == null ? "" : board_id;
            province = province == null ? "" : province;
            List<WG302Data> output = new List<WG302Data>();
            try {
                #region create province filter
                var ppp = province.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province_name in  (" + parampv + ")  ";
                }
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@" 
SELECT
year_old as year_old_sort
,case 
        when year_old=0 then '1-14 ปี'
        when year_old=1 then '15-19 ปี'
        when year_old=2 then '20-29 ปี'
        when year_old=3 then '30-39 ปี'
        when year_old=4 then '40-49 ปี'
        when year_old=5 then '50-59 ปี'
        when year_old=6 then '60-69 ปี'
        when year_old=7 then '70 ปีขึ้นไป'
        else 'ไม่ระบุข้อมูล' end as year_old_lable
,gender
,gender_sort
,count_result
from ( 
select 
case  
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=0 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=14  then 0  
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=15 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=19  then 1 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=20 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=29  then 2 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=30 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=39  then 3 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=40 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=49  then 4 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=50 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=59  then 5 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=60 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=69  then 6
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=70 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=99  then 7
        else 999 end as year_old,
        gender,
        case gender when 'ชาย' then 1 when 'หญิง' then 2 else 3 end as gender_sort,
        count(*) as count_result
from etl.dpm_volunteer
 where 1=1    {0}
group by 
case  
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=0 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=14  then 0  
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=15 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=19  then 1 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=20 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=29  then 2 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=30 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=39  then 3 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=40 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=49  then 4 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=50 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=59  then 5 
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=60 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=69  then 6
        when date_part('year',age(CURRENT_DATE,vol_birth_date ))>=70 and date_part('year',age(CURRENT_DATE,vol_birth_date ))<=99  then 7
        else 999 end
        ,gender
        ,case gender when 'ชาย' then 1 when 'หญิง' then 2 else 3 end
 
) as x
order by year_old,gender_sort
 
                  ", strProvince);
                    output = conn.Query<WG302Data>(sql).ToList();
                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG303Data> GetWG303(string? board_id) {
            //จำนวนปอพร. แยกตามภูมิภาค
            board_id = board_id == null ? "" : board_id;
            List<WG303Data> output = new List<WG303Data>();
            try {
             
             
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"  
                                              select 
                                                 case when r.region_code is null then '99' else  r.region_code end as region_code
                                                ,COALESCE(r.region_name,'ไม่ระบุ' ) as region_name
                                                ,count(*) as count_result 
                                        from etl.dpm_volunteer v
                                        left join public.a_region r on v.province_id=r.pcode
                                        group by COALESCE(r.region_name,'ไม่ระบุ' ),r.region_code
                                        order by r.region_code 
 
                  ");
                    output = conn.Query<WG303Data>(sql).ToList();
                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG304Data> GetWG304(string? board_id) {
            //จำนวน อปพร. แบ่งตามจำนวนหลักสูตรที่เข้าอบรม 
            board_id = board_id == null ? "" : board_id;
            List<WG304Data> output = new List<WG304Data>();
            try {


                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();

                    //  sql = string.Format(@"  
                    //                            select 
                    //                               case when r.region_code is null then '99' else  r.region_code end as region_code
                    //                              ,COALESCE(r.region_name,'ไม่ระบุ' ) as region_name
                    //                              ,count(*) as count_result 
                    //                      from etl.dpm_volunteer v
                    //                      left join public.a_region r on v.province_id=r.pcode
                    //                      group by COALESCE(r.region_name,'ไม่ระบุ' ),r.region_code
                    //                      order by r.region_code 

                    //");
                    // output = conn.Query<WG304Data>(sql).ToList();
                    output.Add(new WG304Data { course_code = "หลักสูตร 1", count_result = 10 });
                    output.Add(new WG304Data { course_code = "หลักสูตร 2", count_result = 20 });
                    output.Add(new WG304Data { course_code = "หลักสูตร 3", count_result = 30 });
                    output.Add(new WG304Data { course_code = "หลักสูตร 4", count_result = 40 });
                    output.Add(new WG304Data { course_code = "หลักสูตร 5", count_result = 50 });
                }
            } catch (Exception) {
            }
            return output;
        }
        public static List<WG305Data> GetWG305(string? board_id, string? province) {
            //จำนวน อปพร. แบ่งตามปีประสบการณ์ 
            board_id = board_id == null ? "" : board_id;
            province = province == null ? "" : province;
            List<WG305Data> output = new List<WG305Data>();
            try {
                #region create province filter
                var ppp = province.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and province_name in  (" + parampv + ")  ";
                }
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@" 

SELECT
year_experience
,case 
        when year_experience=0 then '0-5 ปี'
        when year_experience=1 then '6-9 ปี'
        when year_experience=2 then '10-15 ปี'
        when year_experience=3 then '16-20 ปี'
        when year_experience=4 then '21-25 ปี'
        when year_experience=5 then '25 ปีขึ้นไป'
        else 'ไม่ระบุข้อมูล' end as year_experience_lable
,count_result
from ( 
select 
case  
        when date_part('year',age(CURRENT_DATE,register_date ))>=0 and date_part('year',age(CURRENT_DATE,register_date ))<=5  then 0  
        when date_part('year',age(CURRENT_DATE,register_date ))>=6 and date_part('year',age(CURRENT_DATE,register_date ))<=9  then 1 
        when date_part('year',age(CURRENT_DATE,register_date ))>=10 and date_part('year',age(CURRENT_DATE,register_date ))<=15  then 2 
        when date_part('year',age(CURRENT_DATE,register_date ))>=16 and date_part('year',age(CURRENT_DATE,register_date ))<=16  then 3 
        when date_part('year',age(CURRENT_DATE,register_date ))>=21 and date_part('year',age(CURRENT_DATE,register_date ))<=25  then 4 
        when date_part('year',age(CURRENT_DATE,register_date ))>=25 and date_part('year',age(CURRENT_DATE,register_date ))<=99  then 5 
        else 99 end as year_experience,
        count(*) as count_result
from etl.dpm_volunteer
 where 1=1  {0}
group by 
case  
        when date_part('year',age(CURRENT_DATE,register_date ))>=0 and date_part('year',age(CURRENT_DATE,register_date ))<=5  then 0  
        when date_part('year',age(CURRENT_DATE,register_date ))>=6 and date_part('year',age(CURRENT_DATE,register_date ))<=9  then 1 
        when date_part('year',age(CURRENT_DATE,register_date ))>=10 and date_part('year',age(CURRENT_DATE,register_date ))<=15  then 2 
        when date_part('year',age(CURRENT_DATE,register_date ))>=16 and date_part('year',age(CURRENT_DATE,register_date ))<=16  then 3 
        when date_part('year',age(CURRENT_DATE,register_date ))>=21 and date_part('year',age(CURRENT_DATE,register_date ))<=25  then 4 
        when date_part('year',age(CURRENT_DATE,register_date ))>=25 and date_part('year',age(CURRENT_DATE,register_date ))<=99  then 5 
        else 99 end 
 
) as x
 
                  ", strProvince);
                    output = conn.Query<WG305Data>(sql).ToList();
                }
            } catch (Exception) {
            }
            return output;
        }

        #endregion


        #region wg40
        public static List<WG401Data> GetWG401(string? board_id, string? province) {
            //ภาพรวมสถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด
            board_id = board_id == null ? "" : board_id;
            province = province == null ? "" : province;
            List<WG401Data> output = new List<WG401Data>();
            try {
                #region create province filter
                var ppp = province.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and sprovincename in  (" + parampv + ")  ";
                }
          
                    using (IDbConnection conn = Connection) {
                        string sql = "";
                        conn.Open();

                    sql = string.Format(@"  
                                            select 
                                                    e.sprovincename as province
                                                    ,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end as status
                                                    ,count(*) as count_result
                                            from etl.emc_sk1all_cms e
                                             where 1=1    {0}
                                            group by  e.sprovincename,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end 

                        ", strProvince);
                    output = conn.Query<WG401Data>(sql).ToList();
                   
                    //output.Add(new WG401Data { province = "นครปฐม", status = "พร้อมใช้งาน", count_result = 8 });
                    //    output.Add(new WG401Data { province = "นครปฐม", status = "ไม่พร้อมใช้งาน", count_result = 10 });

                    }
                
                } catch (Exception) {
                }
                return output;
            }
        public static List<WG402Data> GetWG402(string? board_id, string? province) {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด 
            board_id = board_id == null ? "" : board_id;
            province = province == null ? "" : province;
            List<WG402Data> output = new List<WG402Data>();
            try {
                #region create province filter
                var ppp = province.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and sprovincename in  (" + parampv + ")  ";
                }

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();

                    sql = string.Format(@"  
                                            select 
                                                    e.sprovincename as province
                                                    ,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end as status
                                                    ,count(*) as count_result
                                            from etl.emc_sk1all_cms e
                                             where 1=1    {0}
                                            group by  e.sprovincename,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end 

                        ", strProvince);
                    output = conn.Query<WG402Data>(sql).OrderBy(o=>o.province).ToList();
                    //output.Add(new WG402Data { province = "นนทบุรี", status = "พร้อมใช้งาน", count_result = 10 });
                    //output.Add(new WG402Data { province = "นนทบุรี", status = "ไม่พร้อมใช้งาน", count_result = 20 });
                    //output.Add(new WG402Data { province = "นครปฐม", status = "พร้อมใช้งาน", count_result = 8 });
                    //output.Add(new WG402Data { province = "นครปฐม", status = "ไม่พร้อมใช้งาน", count_result = 10 });

                }

            } catch (Exception) {
            }
            return output;
        }
        public static List<WG403Data> GetWG403(string? board_id, string? province) {
            //ภาพรวมสถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด 
            board_id = board_id == null ? "" : board_id;
            province = province == null ? "" : province;
            List<WG403Data> output = new List<WG403Data>();
            try {
                #region create province filter
                var ppp = province.Split(",").ToArray();
                string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
                string strProvince = "";
                #endregion
                if (parampv.Replace("'", "").Trim() != "") {
                    strProvince = " and sprovincename in  (" + parampv + ")  ";
                }

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();

                    sql = string.Format(@"  

                                select 
                                        e.groupname as equipment
                                        ,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end as status
                                        ,count(*) as count_result
                                from etl.emc_sk1all_cms e
                                where typedesc is not null
                                      and  1=1    {0}
                                group by  e.groupname,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end 
                        ", strProvince);
                    output = conn.Query<WG403Data>(sql).OrderBy(o=>o.equipment).ToList();
                    //output.Add(new WG403Data { equipment = "เรือ", status = "พร้อมใช้งาน", count_result = 10 });
                    //output.Add(new WG403Data { equipment = "เรือ", status = "ไม่พร้อมใช้งาน", count_result = 20 });
                    //output.Add(new WG403Data { equipment = "รถ", status = "พร้อมใช้งาน", count_result = 8 });
                    //output.Add(new WG403Data { equipment = "รถ", status = "ไม่พร้อมใช้งาน", count_result = 10 });

                }

            } catch (Exception) {
            }
            return output;
        }
        public static List<WG404Data> GetWG404(string? board_id) {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามศูนย์ 
            board_id = board_id == null ? "" : board_id;
            List<WG404Data> output = new List<WG404Data>();
            try {


                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();

                    sql = string.Format(@"  
                                    select 
                                        s.section_name as unit
                                        ,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end as status
                                        ,count(*) as count_result
                        from etl.emc_sk1all_cms e
                        left join public.a_section s on e.sprovincename=s.pname
                        where section_name is not null
                                and  1=1    
                        group by  s.section_name,s.section_code,case active when 1 then 'พร้อมใช้งาน' else 'ไม่พร้อมใช้งาน' end 
                        order by s.section_code

                    ");
                    output = conn.Query<WG404Data>(sql).ToList();
                    //output.Add(new WG404Data { unit = "ศูนย์เขต 1", status = "พร้อมใช้งาน", count_result = 10 });
                    //output.Add(new WG404Data { unit = "ศูนย์เขต 1", status = "ไม่พร้อมใช้งาน", count_result = 20 });
                    //output.Add(new WG404Data { unit = "ศูนย์เขต 2", status = "พร้อมใช้งาน", count_result = 30 });
                    //output.Add(new WG404Data { unit = "ศูนย์เขต 2", status = "ไม่พร้อมใช้งาน", count_result = 40 }); 
                }
            } catch (Exception) {
            }
            return output;
        }
        #endregion
        #region wg507
        public static List<WG507Data> GetWG507(string? board_id, string? datebegin, string? dateend) {
            //จำนวนการรายงานการเกิดภัยพิบัติ แบ่งตามประเภทภัยพิบัติ
            board_id = board_id == null ? "" : board_id;
            datebegin = datebegin == null ? "" : datebegin;
            dateend = dateend == null ? "" : dateend;
            List<WG507Data> output = new List<WG507Data>();
            try {
                ParamG50 param = new ParamG50();
                if (board_id != "") {
                    param = ParamG50. GetParamBoard(board_id, "wg507");
                } else {
                    param = ParamG50.GetParamQString(datebegin, dateend);
                }
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@" 
                                    select
                                             x.disaster_type, 
		                                     count(*) as count_result
                                    from etl.dpm_disaster_report x
                                    where x.start_date between @datebegin and @dateend 
                                    group by disaster_type 
                                    order by x.disaster_type
                  ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.DateBegin);
                    dynamicParameters.Add("dateend", param.DateEnd);
                    output = conn.Query<WG507Data>(sql, dynamicParameters).ToList();
                }
            } catch (Exception) {
            }
            return output;
        }
        #endregion



      




    }
}
