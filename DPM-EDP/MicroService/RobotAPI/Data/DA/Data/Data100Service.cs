using Dapper;
using MySql.Data.MySqlClient;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;
using System.Globalization;

namespace RobotAPI.Data.DA.Data {
    public class Data100Service {
        ClientService _clientService;

        public Data100Service(ClientService clientService) {
            _clientService = clientService;
        }

        //public static MySqlConnection Connection {
        //    get {
        //        return new MySqlConnection((Globals.CimsConn));
        //    }
        //}

        //สถิติเวลาการใช้ห้องประชุม
        async public Task<List<Data101Set.DataRow>> Data101(string? datefrom, string? dateto) {

            List<Data101Set.DataRow> output = new List<Data101Set.DataRow>();
            try {
                DateTime c_datefrom = DateTime.Now.Date.AddMonths(-1);
                DateTime c_dateto = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(datefrom)) {
                    c_datefrom = DateTime.ParseExact(datefrom, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(dateto)) {
                    c_dateto = DateTime.ParseExact(dateto, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                string sql = "";

                using (var conn = new MySqlConnection(Globals.RoomConn)) {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datefrom", c_datefrom);
                    dynamicParameters.Add("dateto", c_dateto);
                    sql = string.Format(@"
                                            SELECT 
                                                        startDate as meeting_date,
                                                        sum(TIMEDIFF(TIMESTAMP(endDate,endTime),TIMESTAMP(startDate,startTime))/3600 )  as use_time
                                            FROM booking_room
                                            where date(startDate) between @datefrom and @dateto
                                            group by startDate
                              
                        ");
                    output = conn.Query<Data101Set.DataRow>(sql, dynamicParameters).ToList();
                }
            } catch (Exception) {
            }
            return output;
        }






        
        async public Task<List<Data102Set.DataRow>> Data102(string? datefrom, string? dateto) {
            //5 อันดับห้องประชุมที่ถูกใช้งานมากที่สุด    
            List<Data102Set.DataRow> output = new List<Data102Set.DataRow>();
 
            
            try {
                DateTime c_datefrom = DateTime.Now.Date.AddMonths(-1);
                DateTime c_dateto = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(datefrom)) {
                    c_datefrom = DateTime.ParseExact(datefrom, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(dateto)) {
                    c_dateto = DateTime.ParseExact(dateto, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                string sql = "";

                using (var conn = new MySqlConnection(Globals.RoomConn)) {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datefrom", c_datefrom);
                    dynamicParameters.Add("dateto", c_dateto);
                    sql = string.Format(@"
                                                SELECT 
			                                                b.roomId as room_id,
			                                                r.roomName as room_name,
                                                      count(*) as count_use
                                                FROM booking_room b
                                                inner join room r on b.roomId=r.roomId and r.active=1
                                                where date(startDate) between @datefrom and @dateto
                                                group by b.roomId,r.roomName 
                                                order by count(*) desc
                                                limit 5
                        ");
                    output = conn.Query<Data102Set.DataRow>(sql,dynamicParameters).ToList();
                }
            } catch (Exception) { 
            } 
            return output;
        }

     
        async public Task<List<Data103Set.DataRow>> Data103(string? datefrom, string? dateto) {
            //สถิติจำนวนครั้งการใช้ห้องประชุม
            List<Data103Set.DataRow> output = new List<Data103Set.DataRow>();
 
            
            try {
                DateTime c_datefrom = DateTime.Now.Date.AddMonths(-1);
                DateTime c_dateto = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(datefrom)) {
                    c_datefrom = DateTime.ParseExact(datefrom, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(dateto)) {
                    c_dateto = DateTime.ParseExact(dateto, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                string sql = "";
                using (var conn = new MySqlConnection(Globals.RoomConn)) {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datefrom", c_datefrom);
                    dynamicParameters.Add("dateto", c_dateto);
                    sql = string.Format(@"
                                   SELECT 
			                                                b.roomId as room_id,
			                                                r.roomName as room_name,
																											
                                                      count(*) as count_use
                                                FROM booking_room b
                                                inner join room r on b.roomId=r.roomId and r.active=1
												 where date(startDate) between @datefrom and @dateto
                                                group by b.roomId,r.roomName 
                                                order by count(*) desc
                        ");
                    output = conn.Query<Data103Set.DataRow>(sql,dynamicParameters).ToList();
                }
            } catch (Exception) { 
            } 
            return output;
        }

        async public Task<List<Data104Set.DataRow>> Data104(string? datefrom, string? dateto) {
            //จำนวนคนที่ใช้ห้องประชุม
            List<Data104Set.DataRow> output = new List<Data104Set.DataRow>();
            try {
                DateTime c_datefrom = DateTime.Now.Date.AddMonths(-1);
                DateTime c_dateto = DateTime.Now.Date;
                if (!string.IsNullOrEmpty(datefrom)) {
                    c_datefrom = DateTime.ParseExact(datefrom, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(dateto)) {
                    c_dateto = DateTime.ParseExact(dateto, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                string sql = "";
                using (var conn = new MySqlConnection(Globals.RoomConn)) {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("datefrom", c_datefrom);
                    dynamicParameters.Add("dateto", c_dateto);
                    sql = string.Format(@"
                                                             SELECT 
			                                                
			                                                r.startDate as start_date,
                                                      count(*) as count_use
                                                FROM booking_room r
												 where date(startDate) between @datefrom and @dateto
                                                group by r.startDate
                                                order by r.startDate desc
                        ");
                    output = conn.Query<Data104Set.DataRow>(sql, dynamicParameters).ToList();
                }
            } catch (Exception) {
            }
            return output;
        }
    }
}
