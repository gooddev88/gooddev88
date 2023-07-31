using Dapper;
using Npgsql;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.ML.Portal;
using RobotAPI.Data.ML.Weather.WaterBoard;
using RobotAPI.Models.Accident;
using RobotAPI.Services.Api;
using System.Data;
using Vertica.Data.VerticaClient;

namespace RobotAPI.Data.DA.Portal {
    public class PortalService {
        ClientService _clientService;

        public PortalService(ClientService clientService) {
            _clientService = clientService;
        }
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }
        async public Task<DPM003DataSet.DocSet> GetDPM003(string? search = "") {
            DPM003DataSet.DocSet r = new DPM003DataSet.DocSet();
            search = search == null ? "" : search.ToLower();
            var conn = ApiConnService.GetApiInfo("dpm003");

            string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
            var responds = await _clientService.GetAllAsync<DPM003DataSet.DocSet>(url, conn.source_api_token);
            if (responds.StatusCode.ToString().ToUpper() != "OK") {
                r = null;
            } else {
                var q = (DPM003DataSet.DocSet)responds.Result;
                #region search
                r.rows = q.rows.Where(o =>

                                                     o.NAME.ToLower().Contains(search)
                                                    || o.ADDRESS.ToLower().Contains(search)
                                                    || o.PROVINCE_NAME.ToLower().Contains(search)
                                                    || o.AMPHUR_NAME.ToLower().Contains(search)
                                                    || o.DISTRICT_NAME.ToLower().Contains(search)
                                                    || o.TEL.ToLower().Contains(search)
                                                    || search == ""
                                                  ).ToList();
                #endregion 
            }
            return r;
        }
        async public Task<DPM013DataSet.DocSet> GetDPM013(string? search = "") {
            DPM013DataSet.DocSet r = new DPM013DataSet.DocSet(); 
            search = search == null ? "" : search.ToLower();
            var conn = ApiConnService.GetApiInfo("dpm013");

            string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
            var responds = await _clientService.GetAllAsync<DPM013DataSet.DocSet>(url, conn.source_api_token);
            if (responds.StatusCode.ToString().ToUpper() != "OK") {
                r.status = 0;
                r.message = responds.StatusCode.ToString().ToUpper();
            } else {
                var q = (DPM013DataSet.DocSet)responds.Result;
                #region search
                r.rows = q.rows.Where(o =>
                                                     o.ANNOUNCECODE.ToLower().Contains(search)
                                                     || o.PROVINCE_NAME.ToLower().Contains(search)
                                                     || o.AMPHUR_NAME.ToLower().Contains(search)
                                                     || o.TAMBOL_NAME.ToLower().Contains(search)
                                                     || o.STATUS_NAME.ToLower().Contains(search)
                                                     || search == ""
                                                  ).ToList();
                #endregion

            }
            return r;
        }
        async public Task<DiasterSummary> GetDPM013SourceDB(string? search = "") {
            DiasterSummary output = new DiasterSummary();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";
                    conn.Open();
                    sql = string.Format(@"
                                select 
                                     disaster_type 
		                                 ,count(*) as count_type
                                from etl.dpm_013 
                                group by disaster_type 

                    "); 
                    var query = conn.Query< CountDisaster>(sql).ToList();
                    foreach (var q in query) {
                        if (q.disaster_type=="FL") {
                            output.count_flood =Convert.ToInt32( q.count_type);//น้ำท่วม
                        }

                        if (q.disaster_type == "LS") {
                            output.count_landslide = Convert.ToInt32(q.count_type);//ดินถล่ม
                        }
                        if (q.disaster_type == "WC") {
                            output.count_winter = Convert.ToInt32(q.count_type);//ภัยหนาว
                        }
                       // output.count_windstorm = 0;
                        if (q.disaster_type == "FR") {
                            output.count_fire = Convert.ToInt32(q.count_type);//ไฟใหม้
                        }
                      //  output.count_wildfire = 0;
                        if (q.disaster_type == "FW") {
                            output.count_wildfire = Convert.ToInt32(q.count_type);//ไฟป่า
                        }

                       // output.count_chemical = 0;//สารเคมี
                        //output.count_building_collapse = 0;//อาคารถล่ม
                        if (q.disaster_type == "DR") {
                            output.count_drought = Convert.ToInt32(q.count_type);//ภัยแล้ง
                        }
                        output.count_epidemic = 0;
                        output.count_tsunami = 0;//tsunami
                      
                    }
                    output.count_toal_disaster = query.Where(o => o.disaster_type=="DR"
                                                                          || o.disaster_type=="FL"
                                                                          || o.disaster_type == "FW"
                                                                          || o.disaster_type=="LS"
                                                                          || o.disaster_type=="WC"
                                                                          || o.disaster_type == "FR"
                                                                          )
                                                              .Sum(o=>o.count_type);//sum all community
                                                                       //FL.อุทกภัย
                                                                       //FR  อัคคีภัย
                                                                       //WC ภัยหนาว
                                                                       //LS ดินโคนถล่ม
                                                                       //DR ภัยแล้ง
                                                                       //FW ไฟป่า
                }


            } catch (Exception ex) {

                var xx = ex.Message;
            }

            return output;
        }
        async public Task<DPM019DataSet.DocSet> GetDPM019(string? search = "") {
            //ข้อมูลศูนย์พักพิงทั่วประเทศ
            DPM019DataSet.DocSet r = new DPM019DataSet.DocSet { message="ok",status=1,rows =new List<DPM019DataSet.DataRow>()};
            search = search == null ? "" : search.ToLower();
            var conn = ApiConnService.GetApiInfo("dpm019");

            string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
            var responds = await _clientService.GetAllAsync<DPM019DataSet.DocSet>(url, conn.source_api_token);
            if (responds.StatusCode.ToString().ToUpper() != "OK") {
                r.status = 0;
                r.message = responds.StatusCode.ToString().ToUpper();
            } else {
             r = (DPM019DataSet.DocSet)responds.Result;
                #region search
                //if (!string.IsNullOrEmpty(search)) {
                //    r.rows = q.rows.Where(o =>
                //                                      ( o.SHELTERNAME.Contains(search)
                //                                       || o.SHELTERID.Contains(search)
                //                                       || o.TAMBONCODE.Contains(search)
                //                                       || o.PROVINCECODE.Contains(search) 
                //                                       )
                //                                    ).ToList();
                //}
         
                #endregion

            }
            return r;
        }

        async public Task<DPM020DataSet.DocSet?> GetDPM020(string? pname="", string? aname = "", string? tname = "") {
            //ข้อมูลเครื่องจักรกล
            DPM020DataSet.DocSet? r = new DPM020DataSet.DocSet();
            pname = pname == null ? "" : pname;
            aname = aname == null ? "" : aname;
            tname = tname == null ? "" : tname; 
            try { 
                var conn = ApiConnService.GetApiInfo("dpm020"); 
                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM020DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r = null;
                } else {
                    var q = (DPM020DataSet.DocSet)responds.Result; 
                    r.rows = q.rows.Where(o =>
                                                        (o.PROVINCE_NAME == pname || pname == "")
                                                       && (o.AMPHUR_NAME == aname || aname == "")
                                                       && (o.TUMBOL_NAME == tname || tname == "")
                                                      ).ToList();
                    foreach (var row in r.rows) {
                        double level = 0;
                        double.TryParse(row.ACCUMULATERAINFALL24HOURS, out level);
                        row.ACCUMULATERAINFALL24HOURS_DESC = GetRailFallLevelDescription(level);

                        double level_pm = 0;
                        if (row.AQPM25!=null) {
                            double.TryParse(row.AQPM25, out level_pm);
                            row.AQPM25_DESC = GetPM25Description(level_pm);
                        } else {
                            row.AQPM25_DESC = "";
                        } 
                    }
                }
            } catch (Exception ex) {
                r = null;
            }
            return r;
        }
      


        //for dpm020 rain level
        public string GetRailFallLevelDescription(double level24hr) {
           
            string rainDesc = "";
            if (level24hr == 0) {
                rainDesc = "ไม่มีฝน";
            } else if (level24hr > 0 && level24hr <= 10) {
                rainDesc = "ฝนตกเล็กน้อย";
            }
            else if (level24hr > 10 && level24hr <= 35) {
                rainDesc = "ฝนตกปานกลาง";
            } else if (level24hr > 35 && level24hr <= 90) {
                rainDesc = "ฝนตกหนัก";
            } else {
                rainDesc = "ฝนตกหนักมาก";
            }
            return rainDesc;
        }

        //for dpm020  pm 2.5
        public string GetPM25Description(double level) {

            string rainDesc = "";
            if (level >= 0 && level <= 25) {
                rainDesc = "คุณภาพอากาศดีมาก";
            } else if (level >= 26 && level <= 50) {
                rainDesc = "คุณภาพอากาศดี";
            } else if (level >= 51 && level <= 100) {
                rainDesc = "ปานกลาง";
            } else if (level >= 101 && level <= 200) {
                rainDesc = "เริ่มมีผลกระทบต่อสุขภาพ";
            } else {
                rainDesc = "มีผลกระทบต่อสุขภาพ";
            }
            return rainDesc;
        }

    }
}
