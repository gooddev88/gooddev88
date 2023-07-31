using Dapper;
using Npgsql;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.ML.Board.BoardParam;
using RobotAPI.Data.ML.Board.Widget;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;
using System.Data;
using System.Globalization;
using Vertica.Data.VerticaClient;
using static Dapper.SqlMapper;

namespace RobotAPI.Data.DA.Data {
    public class Data500Service {
        ClientService _clientService;

        public Data500Service(ClientService clientService) {
            _clientService = clientService;
        }

        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection(Globals.CimsConn);
            }
        }
        //async public Task<List<Data507Set.DataRow>> Data507(Data507Set.Param input) {
        //    //รายงาน 7 ภัย    
        //    List<Data507Set.DataRow> output = new List<Data507Set.DataRow>();
        //    //string query = @"SELECT * FROM ETL_NEWS ";
        //    string disaster_type = DataHelper. CreateWhereIn(input.distype, ',');
        //    string province = DataHelper.CreateWhereIn(input.province, ',');
        //    string datebegin = input.begin.ToString("yyyy-MM-dd");
        //    string dateend = input.end.ToString("yyyy-MM-dd");
        //    string query = @"select * from etl.dpm_disaster_report ";
        //    query = query + @"where 1=1 ";
        //    query = query + $"and start_date between '{datebegin}' and '{dateend}' ";
        //    if (disaster_type != "") {
        //        query = query + @"and disaster_type in (" + disaster_type + @") ";
        //    }
        //    if (province != "") {
        //        query = query + @"and province in (" + province + @") ";
        //    }
        //    if (!string.IsNullOrEmpty(input.search)) {
        //        query = query + $@"
        //                            and (
        //                                   status_office like '%{input.search}%'
        //                              or  amphur like '%{input.search}%'  
        //                              or  tambon like '%{input.search}%'  
        //                             )
        //                         ";
        //    }


        //    VerticaConnectionStringBuilder builder = new VerticaConnectionStringBuilder();
        //    var conn = Globals.CMSConn.Split(",");
        //    builder.Host = conn[0];
        //    builder.Database = conn[1];
        //    builder.User = conn[2];
        //    builder.Password = conn[3];
        //    VerticaConnection _conn = new VerticaConnection(builder.ToString());
        //    _conn.Open();
        //    VerticaCommand command = _conn.CreateCommand();
        //    command.CommandText = query;
        //    VerticaDataReader dr = command.ExecuteReader();
        //    int rows = 1;

        //    while (dr.Read()) {
        //        Data507Set.DataRow o = new Data507Set.DataRow();
        //        // o.id = dr["id"].ToString();
        //        o.id = rows.ToString();
        //        o.dpm_id = dr["dpm_id"].ToString();
        //        o.province = dr["province"].ToString();
        //        o.disaster_type = dr["disaster_type"].ToString();
        //        o.start_date = dr["start_date"] == null ? null : Convert.ToDateTime(dr["start_date"]);

        //        o.helpannouncedate = dr["helpannouncedate"].ToString() == "" ? null : Convert.ToDateTime(dr["helpannouncedate"]);
        //        o.end_date = dr["end_date"].ToString() == "" ? null : Convert.ToDateTime(dr["end_date"]);
        //        o.amphur = dr["amphur"].ToString();
        //        o.tambon_code = dr["tambon_code"].ToString();
        //        o.tambon = dr["tambon"].ToString();
        //        o.muban_count = Convert.ToInt32(dr["muban_count"]);
        //        o.last_upd_date = dr["last_upd_date"].ToString() == "" ? null : Convert.ToDateTime(dr["last_upd_date"]);
        //        o.dpm_timestamp = dr["dpm_timestamp"].ToString() == "" ? null : Convert.ToDateTime(dr["dpm_timestamp"]);
        //        o.PID = dr["PID"].ToString();
        //        o.AID = dr["AID"].ToString();
        //        o.org = dr["org"].ToString();
        //        o.community_count = Convert.ToInt32(dr["community_count"]);
        //        o.status_office = dr["status_office"].ToString();
        //        o.flood_type = dr["flood_type"].ToString();
        //        o.TID = dr["TID"].ToString();
        //        o.org_id = dr["org_id"].ToString();
        //        o.key_check = dr["key_check"].ToString();

        //        ////o.id = dr["id"].ToString();
        //        //o.news_id = dr["news_id"].ToString();
        //        //o.news_date = Convert.ToDateTime(dr["news_date"]);
        //        //o.title = dr["title"].ToString();
        //        //o.detail = dr["detail"].ToString();
        //        //o.pic_link = dr["pic_link"].ToString();
        //        //o.news_link = dr["news_link"].ToString();
        //        //o.type = dr["type"].ToString();
        //        //o.source = dr["source"].ToString();
        //        //o.tag = dr["tag"].ToString();
        //        //o.n_timestamp = Convert.ToDateTime(dr["n_timestamp"]);
        //        output.Add(o);
        //        ++rows;
        //    }

        //    dr.Close();
        //    _conn.Close();
        //    return output;
        //}




        public Data501Set.DocSet Data501(string? datefrom, string? dateto) {
            //น้ำเขื่อน / etl.dss_rid_dam
            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data501Set.DocSet output = new Data501Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dss_earthquake
                                                    where earthquake_date between @datebegin and @dateend
                                                      {0}
                    ", limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<dss_earthquake>(sql, dynamicParameters).ToList();

                }

            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data502Set.DocSet Data502(string? datefrom, string? dateto) {
            //น้ำเขื่อน / etl.dss_rid_dam
            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data502Set.DocSet output = new Data502Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dss_rid_dam
                                                    where wd_date between @datebegin and @dateend
                                                      {0}
                    ", limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<dss_rid_dam>(sql, dynamicParameters).ToList();

                }

            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }

        public Data503Set.DocSet Data503(string? datefrom, string? dateto) {
            //น้ำเขื่อน / etl.dss_rid_dam
            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data503Set.DocSet output = new Data503Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dss_water
                                                    where h_date between @datebegin and @dateend
                                                      {0}
                    ", limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<dss_water>(sql, dynamicParameters).ToList();

                }

            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }

        public Data504Set.DocSet Data504(string? datefrom, string? dateto) {
            //ปริมาณน้ำฝน  / etl.dss_rain
            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data504Set.DocSet output = new Data504Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dss_rain
                                                    where data_date between @datebegin and @dateend
                                                    {0}
                    ", limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<dss_rain>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data505Set.DocSet Data505(string? datefrom, string? dateto) {
            //คุณภาพอากาศ / etl.dss_weather_quality
            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data505Set.DocSet output = new Data505Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dss_weather_quality
                                                    where wq_date between @datebegin and @dateend
                                                    {0}
                    ", limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<dss_weather_quality>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data506Set.DocSet Data506(string? datefrom, string? dateto) {
            // เสี่ยงแล้ง / etl.dpm_drought_risk
            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data506Set.DocSet output = new Data506Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dpm_drought_risk
                                                    where data_update between @datebegin and @dateend
                                                    {0}
                    ", limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<dpm_drought_risk>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data507Set.DocSet Data507(Data507Set.Param param) {
            //รายงาน 7 ภัย
            Data507Set.DocSet output = new Data507Set.DocSet();
            try {
                string limit_row = "";
           
                output.message = "ok";
                output.status = 1; 

                string disaster_type = DataHelper.CreateWhereIn(param.distype, ',');
                string province = DataHelper.CreateWhereIn(param.province, ',');
                string datebegin = param.begin.ToString("yyyy-MM-dd");
                string dateend = param.end.ToString("yyyy-MM-dd");
                string query = @"select * from etl.dpm_disaster_report ";
                if (param.distype.ToUpper().Replace(",","")=="X") {//show sample data
                    query = query + @"where 1=1 ";
                    query = query + $"and start_date between '{datebegin}' and '{dateend}' ";
                    query = query + " limit 100";
                } else {
                    query = query + @"where 1=1 ";
                    query = query + $"and start_date between '{datebegin}' and '{dateend}' ";
                    if (disaster_type != "") {
                        query = query + @"and disaster_type in (" + disaster_type + @") ";
                    }
                    if (province != "") {
                        query = query + @"and province in (" + province + @") ";
                    }
                    if (!string.IsNullOrEmpty(param.search)) {
                        query = query + $@"
                                    and (
	     	                                    status_office like '%{param.search}%'
		                                    or  amphur like '%{param.search}%'  
		                                    or  tambon like '%{param.search}%'  
	                                    )
                                 ";
                    }
                }
             

                using (IDbConnection conn = Connection) {
                  
                    conn.Open();
                    //sql = string.Format(@"
                    //                                select 
                    //                                     *
                    //                                from etl.acd_accident_person
                    //                                where accident_date between @datebegin and @dateend
                    //                                {0}
                    //", limit_row);
                    //var dynamicParameters = new DynamicParameters();
                    //dynamicParameters.Add("datebegin", param.date_begin);
                    //dynamicParameters.Add("dateend", param.date_end);
                    //output.rows = conn.Query<dpm_disaster_report>(sql, dynamicParameters).ToList();
                    output.rows = conn.Query<dpm_disaster_report>(query).ToList();
                    // var xx = conn.Query(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data508Set.DocSet Data508(string? datefrom, string? dateto) {
            //เสี่ยงอุทกภัย  / etl.dpm_flood_risk
            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data508Set.DocSet output = new Data508Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dpm_flood_risk
                                                    where data_update between @datebegin and @dateend
                                                    {0}
                    ", limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<dpm_flood_risk>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data509Set.DocSet Data509(string? datefrom, string? dateto, string? province) {
            //acd_accident_person 
            province = province == null ? "" : province;
            #region create province filter
            var ppp = province.Split(",").ToArray();
            string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
            string strProvince = "";
            if (parampv.Replace("'", "").Trim() != "") {
                strProvince = " and province in  (" + parampv + ")  ";
            }
            #endregion


            string limit_row = "";
            if (string.IsNullOrEmpty(datefrom) || string.IsNullOrEmpty(dateto)) {
                limit_row = " LIMIT 100";
                datefrom = "20000101";
                dateto = "29991231";
            }
            Data509Set.DocSet output = new Data509Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
                Data501Set.Param param = param = GetParamQString(datefrom, dateto);
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.acd_accident_person
                                                    where accident_date between @datebegin and @dateend
                                                    {0}{1}
                    ", strProvince, limit_row);
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datebegin", param.date_begin);
                    dynamicParameters.Add("dateend", param.date_end);
                    output.rows = conn.Query<acd_accident_person>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public view_weather_quality? Data510(string? pname, string? aname) {
            //get pm2.5 ล่าสุดตามจังหวัด

            view_weather_quality? output = new view_weather_quality();
         
            try {
          
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                    select *
                                    from etl.dss_weather_quality
                                    where province_th='{0}'
                                    and pm25   is not null
                                    order by wq_date desc,pm25 desc
                                    limit 1 
                                                    
                    ", pname);

                    output = conn.Query<view_weather_quality>(sql).FirstOrDefault();
                }
            } catch (Exception ex) { 
            }
            return output;
        }
        public view_weather_quality? Data511(double? lat, double? lon) {
            //get pm2.5 ล่าสุดตาม geolcation
            view_weather_quality? output = new view_weather_quality();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                 select * , ( point(station_lat , station_long) <-> point({0}, {1}) )*111.325 AS distance
                                from etl.view_weather_quality
                                order by distance limit 1;
                                                    
                    ", lat,lon);

                    output = conn.Query<view_weather_quality>(sql).FirstOrDefault();
                }
            } catch (Exception ex) {
            }
            return output;
        }

        public Data550Set.DocSet Data550(string? search, string? province) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด 
            search = search == null ? "" : search;
            province = province == null ? "" : province;
            #region create province filter
            var ppp = province.Split(",").ToArray();
            string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
            string strProvince = "";
            if (parampv.Replace("'", "").Trim() != "") {
                strProvince = " and province_name in  (" + parampv + ")  ";
            }
            #endregion


            string limit_row = "";
            string where_like = "";
            if (search.ToLower() == "x") {
                limit_row = " LIMIT 500";
            } else if (search.ToLower() != "") {
                where_like = @" and ( typedesc like '%" + search + @"%' ";
                where_like = where_like + @" sv_name like '%" + search + @"%' ";
                where_like = where_like + @" brand like '%" + search + @"%' ) ";

            } else {
                where_like = "";
            }

            Data550Set.DocSet output = new Data550Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                        select  
                                         e.stock_id
                                        ,e.stock_date
                                        ,e.stockdoc_item_id
                                        ,e.stuff_id
                                        ,e.stuff_id_parent
                                        ,e.stuff_code
                                        ,e.stuff_name
                                        ,e.stuff_price
                                        ,e.count_in
                                        ,e.count_out
                                        ,e.create_user
                                        ,e.create_time
                                        ,e.is_clear
                                        ,e.locate_id
                                        ,e.locate_name
                                        ,e.province_name
                                        ,s.section_name
                                        ,s.section_code
                                        ,e.etl_date
                                        from etl.est_stock_cms e
                                        left join public.a_section s on e.province_name=s.pname
                                                    where 1=1
                                                    {0}{1}{2}
                    ", strProvince, where_like, limit_row);
                    var dynamicParameters = new DynamicParameters();
                    output.rows = conn.Query<est_stock_cms>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }

        public Data551Set.DocSet Data551(string? search, string? sectioncode) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามศูนย์
            search = search == null ? "" : search;
            sectioncode = sectioncode == null ? "" : sectioncode;
            #region create province filter
            var ppp = sectioncode.Split(",").ToArray();
            string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
            string strProvince = "";
            if (parampv.Replace("'", "").Trim() != "") {
                strProvince = " and section_code in  (" + parampv + ")  ";
            }
            #endregion


            string limit_row = "";
            string where_like = "";
            if (search.ToLower() == "x") {
                limit_row = " LIMIT 500";
            } else if (search.ToLower() != "") {
                where_like = @" and ( stuff_name like '%" + search + @"%' ";
                where_like = where_like + @" stuff_code like '%" + search + @"%' ";
                where_like = where_like + @" brand section_name '%" + search + @"%' ) ";

            } else {
                where_like = "";
            }

            Data551Set.DocSet output = new Data551Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                            select  
                                             e.stock_id
                                            ,e.stock_date
                                            ,e.stockdoc_item_id
                                            ,e.stuff_id
                                            ,e.stuff_id_parent
                                            ,e.stuff_code
                                            ,e.stuff_name
                                            ,e.stuff_price
                                            ,e.count_in
                                            ,e.count_out
                                            ,e.create_user
                                            ,e.create_time
                                            ,e.is_clear
                                            ,e.locate_id
                                            ,e.locate_name
                                            ,e.province_name
                                            ,s.section_name
                                            ,s.section_code
                                            ,e.etl_date
                                            from etl.est_stock_cms e
                                            left join public.a_section s on e.province_name=s.pname
                                                    where 1=1
                                                    {0}{1}{2}
                    ", strProvince, where_like, limit_row);
                    var dynamicParameters = new DynamicParameters();
                    output.rows = conn.Query<Data551Set.est_stock_cms_c>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data561Set.DocSet Data561(string? search,string? province) {
            //acd_accident_person 
            search = search == null ? "" : search;
            province = province == null ? "" : province;
            #region create province filter
            var ppp = province.Split(",").ToArray();
            string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
            string strProvince = "";
            if (parampv.Replace("'", "").Trim() != "") {
                strProvince = " and province_name in  (" + parampv + ")  ";
            }
            #endregion


            string limit_row = "";
            string where_like = "";
            if (search.ToLower()== "x") {
                limit_row = " LIMIT 500";
            } else if(search.ToLower() != "") {
                  where_like = @" and ( vol_firstname like '%" + search + @"%' ";
                where_like = where_like + @" vol_lastname like '%" + search + @"%' ";
                where_like = where_like + @" vol_id_card like '%" + search + @"%' ) ";

            } else {
                where_like = "";
            }

            Data561Set.DocSet output = new Data561Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {
             
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                                    select 
                                                         *
                                                    from etl.dpm_volunteer
                                                    where 1=1
                                                    {0}{1}{2}
                    ", strProvince, where_like, limit_row);
                    var dynamicParameters = new DynamicParameters(); 
                    output.rows = conn.Query<dpm_volunteer>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        
        public Data570Set.DocSet Data570(string? search, string? province) {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด
            search = search == null ? "" : search;
            province = province == null ? "" : province;
            #region create province filter
            var ppp = province.Split(",").ToArray();
            string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
            string strProvince = "";
            if (parampv.Replace("'", "").Trim() != "") {
                strProvince = " and sprovincename in  (" + parampv + ")  ";
            }
            #endregion


            string limit_row = "";
            string where_like = "";
            if (search.ToLower() == "x") {
                limit_row = " LIMIT 500";
            } else if (search.ToLower() != "") {
                where_like = @" and ( typedesc like '%" + search + @"%' ";
                where_like = where_like + @" sv_name like '%" + search + @"%' ";
                where_like = where_like + @" brand like '%" + search + @"%' ) ";

            } else {
                where_like = "";
            }

            Data570Set.DocSet output = new Data570Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                        select  
                                         e.sk1_id
                                        ,e.equipid
                                        ,e.typedesc
                                        ,e.ecde
                                        ,e.amount
                                        ,e.brand
                                        ,e.mcmodel
                                        ,e.chassisno
                                        ,e.licno
                                        ,e.licdesc
                                        ,e.licdate
                                        ,e.engbrand
                                        ,e.engmodel
                                        ,e.fueltype
                                        ,e.engno
                                        ,e.cc
                                        ,e.hp
                                        ,e.motorbrand
                                        ,e.motormodel
                                        ,e.moterno
                                        ,e.motorpower
                                        ,e.transmiss
                                        ,e.transmissdesc
                                        ,e.sk2
                                        ,e.daterate
                                        ,e.datefirst
                                        ,e.pricefirst
                                        ,e.transferfrom
                                        ,e.oldcode
                                        ,e.daterec
                                        ,e.srcbudget
                                        ,e.budgetdesc
                                        ,e.sources
                                        ,e.sourcesdesc
                                        ,e.createby
                                        ,e.createdate
                                        ,e.updateby
                                        ,e.updatedate
                                        ,e.active 
                                        ,e.status
                                        ,e.sv_name
                                        ,e.sv_posi
                                        ,e.ad_name
                                        ,e.ad_posi
                                        ,e.desctha
                                        ,e.dispose
                                        ,e.datedispose
                                        ,e.dpid
                                        ,e.mtypeid
                                        ,e.typename_th
                                        ,e.typecode
                                        ,e.groupcode
                                        ,e.mgroupid
                                        ,e.groupname
                                        ,e.motorunit
                                        ,e.unitid
                                        ,e.unitname
                                        ,e.inttials
                                        ,e.sprovincename
                                        ,s.section_name
                                        ,s.section_code
                                        ,e.sampname
                                        ,e.stambonname
                                        ,e.is_lot
                                        from etl.emc_sk1all_cms e
                                        left join public.a_section s on e.sprovincename=s.pname
                                                    where 1=1
                                                    {0}{1}{2}
                    ", strProvince, where_like, limit_row);
                    var dynamicParameters = new DynamicParameters();
                    output.rows = conn.Query<emc_sk1all_cms>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }
        public Data571Set.DocSet Data571(string? search, string? sectioncode) {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามศูนย์
            search = search == null ? "" : search;
            sectioncode = sectioncode == null ? "" : sectioncode;
            #region create province filter
            var ppp = sectioncode.Split(",").ToArray();
            string parampv = string.Join(",", ppp.Select(item => "'" + item + "'"));
            string strProvince = "";
            if (parampv.Replace("'", "").Trim() != "") {
                strProvince = " and section_code in  (" + parampv + ")  ";
            }
            #endregion


            string limit_row = "";
            string where_like = "";
            if (search.ToLower() == "x") {
                limit_row = " LIMIT 500";
            } else if (search.ToLower() != "") {
                where_like = @" and ( typedesc like '%" + search + @"%' ";
                where_like = where_like + @" sv_name like '%" + search + @"%' ";
                where_like = where_like + @" brand like '%" + search + @"%' ) ";

            } else {
                where_like = "";
            }

            Data571Set.DocSet output = new Data571Set.DocSet();
            output.message = "ok";
            output.status = 1;
            try {

                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                        select  
                                         e.sk1_id
                                        ,e.equipid
                                        ,e.typedesc
                                        ,e.ecde
                                        ,e.amount
                                        ,e.brand
                                        ,e.mcmodel
                                        ,e.chassisno
                                        ,e.licno
                                        ,e.licdesc
                                        ,e.licdate
                                        ,e.engbrand
                                        ,e.engmodel
                                        ,e.fueltype
                                        ,e.engno
                                        ,e.cc
                                        ,e.hp
                                        ,e.motorbrand
                                        ,e.motormodel
                                        ,e.moterno
                                        ,e.motorpower
                                        ,e.transmiss
                                        ,e.transmissdesc
                                        ,e.sk2
                                        ,e.daterate
                                        ,e.datefirst
                                        ,e.pricefirst
                                        ,e.transferfrom
                                        ,e.oldcode
                                        ,e.daterec
                                        ,e.srcbudget
                                        ,e.budgetdesc
                                        ,e.sources
                                        ,e.sourcesdesc
                                        ,e.createby
                                        ,e.createdate
                                        ,e.updateby
                                        ,e.updatedate
                                        ,e.active 
                                        ,e.status
                                        ,e.sv_name
                                        ,e.sv_posi
                                        ,e.ad_name
                                        ,e.ad_posi
                                        ,e.desctha
                                        ,e.dispose
                                        ,e.datedispose
                                        ,e.dpid
                                        ,e.mtypeid
                                        ,e.typename_th
                                        ,e.typecode
                                        ,e.groupcode
                                        ,e.mgroupid
                                        ,e.groupname
                                        ,e.motorunit
                                        ,e.unitid
                                        ,e.unitname
                                        ,e.inttials
                                        ,e.sprovincename
                                        ,s.section_name
                                        ,s.section_code
                                        ,e.sampname
                                        ,e.stambonname
                                        ,e.is_lot
                                        from etl.emc_sk1all_cms e
                                        left join public.a_section s on e.sprovincename=s.pname
                                                    where 1=1
                                                    {0}{1}{2}
                    ", strProvince, where_like, limit_row);
                    var dynamicParameters = new DynamicParameters();
                    output.rows = conn.Query<Data571Set.emc_sk1all_cms_c>(sql, dynamicParameters).ToList();
                }
            } catch (Exception ex) {
                output.message = ex.Message;
                output.status = 0;
            }
            return output;
        }

        public static Data501Set.Param GetParamQString(string datebegin, string dateend) {
            Data501Set.Param param = new Data501Set.Param();
            try {
                if (!string.IsNullOrEmpty(datebegin)) {
                    param.date_begin = DateTime.ParseExact(datebegin, "yyyyMMdd", CultureInfo.InvariantCulture);
                } else {
                    param.date_begin = new DateTime(DateTime.Now.Year, 1, 1);
                }
                if (!string.IsNullOrEmpty(dateend)) {
                    param.date_end = DateTime.ParseExact(dateend, "yyyyMMdd", CultureInfo.InvariantCulture);
                } else {
                    param.date_end = new DateTime(DateTime.Now.Year, 12, 31);
                }
            } catch (Exception ex) {

            }
            return param;
        }
    }
}
