using Dapper;
using MySql.Data.MySqlClient;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Data.DA.Data {
    public class Data200Service {
        ClientService _clientService;

        public Data200Service(ClientService clientService) {
            _clientService = clientService;
        }
         
        async public Task<Data201Set.DocSet> Data201(string? datefrom, string? dateto) {
            //สถิติการแจ้งเตือนตามประเภทภัยพิบัติ
            Data201Set.DocSet r = new Data201Set.DocSet();
            try {
                if (string.IsNullOrEmpty(datefrom)) {
                    datefrom = DateTime.Now.Date.AddMonths(-1).ToString("yyyyMMdd");
                }
                if (string.IsNullOrEmpty(dateto)) {
                    dateto = DateTime.Now.Date.ToString("yyyyMMdd");
                }


                var conn = ApiConnService.GetApiInfo("data201");

                string url = $"{conn.source_base_url}{conn.source_api_url}?datefrom={datefrom}&dateto={dateto}";
                var responds = await _clientService.GetAllAsync<Data201Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data201Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }


        async public Task<Data202Set.DocSet> Data202(string? datefrom, string? dateto) {
            //สถิติการแจ้งเตือนภัยตามจังหวัด
            Data202Set.DocSet r = new Data202Set.DocSet();
            try {
                if (string.IsNullOrEmpty(datefrom)) {
                    datefrom = DateTime.Now.Date.AddMonths(-1).ToString("yyyyMMdd");
                }
                if (string.IsNullOrEmpty(dateto)) {
                    dateto = DateTime.Now.Date.ToString("yyyyMMdd");
                }
                var conn = ApiConnService.GetApiInfo("data202");

                string url = $"{conn.source_base_url}{conn.source_api_url}?datefrom={datefrom}&dateto={dateto}";
                var responds = await _clientService.GetAllAsync<Data202Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data202Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }


        async public Task<Data203Set.DocSet> Data203() {
            //สถิติอาชีพของผู้ใช้งาน
            Data203Set.DocSet r = new Data203Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data203");

                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<Data203Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data203Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
        async public Task<Data204Set.DocSet> Data204() {
            //สถิติเพศของผู้ใช้งาน
            Data204Set.DocSet r = new Data204Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data204");

                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<Data204Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data204Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }


        async public Task<Data205Set.DocSet> Data205() {
            //สถิติจังหวัดที่เปิดรับการแจ้งเตือน
            Data205Set.DocSet r = new Data205Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data205");

                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<Data205Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data205Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;

        }

        async public Task<Data206Set.DocSet> Data206(string lat,string lon) {
            //หา ตำบล อำเภอ จังหวัดจาก gps
            Data206Set.DocSet r = new Data206Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data206");

                string url = $"{conn.source_base_url}{conn.source_api_url}?lat={lat}&lon={lon}";
                var responds = await _clientService.GetAllAsync<Data206Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data206Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;

        }
    }
}