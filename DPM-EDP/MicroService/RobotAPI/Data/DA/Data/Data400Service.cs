using Dapper;
using MySql.Data.MySqlClient;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Data.DA.Data {
    public class Data400Service {
        ClientService _clientService;

        public Data400Service(ClientService clientService) {
            _clientService = clientService;
        }
         
        async public Task<Data401Set.DocSet> Data401(string? department, string? supdepartment, int? year) {
            //สถิติการลา
            Data401Set.DocSet r = new Data401Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data401");

                string url = $"{conn.source_base_url}{conn.source_api_url}?department={department}&supdepartment={supdepartment}&year={year}";
                var responds = await _clientService.GetAllAsync<Data401Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data401Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

     async public Task<Data402Set.DocSet> Data402(int? year, int? month) {
            //สถิติวันลาของหน่วยงาน
            Data402Set.DocSet r = new Data402Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data402");

                string url = $"{conn.source_base_url}{conn.source_api_url}?year={year}&month={month}";
                var responds = await _clientService.GetAllAsync<Data402Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data402Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

        async public Task<Data403Set.DocSet> Data403(int? year, int? month) {
            //สถิติวันลาของส่วนงาน
            Data403Set.DocSet r = new Data403Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data403");

                string url = $"{conn.source_base_url}{conn.source_api_url}?year={year}&month={month}";
                var responds = await _clientService.GetAllAsync<Data403Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data403Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
        async public Task<Data404Set.DocSet> Data404(int? year, string? leavetype) {
            //สถิติการลาประจำวัน (ประเภทวันลา)
            Data404Set.DocSet r = new Data404Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data404");

                string url = $"{conn.source_base_url}{conn.source_api_url}?year={year}&leavetype={leavetype}";
                var responds = await _clientService.GetAllAsync<Data404Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data404Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

        async public Task<Data405Set.DocSet> Data405(int? year, string? leavetype) {
            //สถิติการลาแต่ละประเภท (ประเภทพนักงาน)
            Data405Set.DocSet r = new Data405Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data405");

                string url = $"{conn.source_base_url}{conn.source_api_url}?year={year}&leavetype={leavetype}";
                var responds = await _clientService.GetAllAsync<Data405Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data405Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

        async public Task<Data406Set.DocSet> Data406(int? year, string? leavetype) {
            //สถิติการลาแบ่งตามเพศ
            Data406Set.DocSet r = new Data406Set.DocSet();
            try {
                var conn = ApiConnService.GetApiInfo("data406");

                string url = $"{conn.source_base_url}{conn.source_api_url}?year={year}&leavetype={leavetype}";
                var responds = await _clientService.GetAllAsync<Data406Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data406Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
    }
}