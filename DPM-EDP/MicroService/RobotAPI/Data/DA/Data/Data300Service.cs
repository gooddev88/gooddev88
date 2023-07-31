using Dapper;
using MySql.Data.MySqlClient;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Data.DA.Data {
    public class Data300Service {
        ClientService _clientService;

        public Data300Service(ClientService clientService) {
            _clientService = clientService;
        }
         
        async public Task<Data301Set.DocSet> Data301(string? datefrom, string? dateto) {
            //อันดับหมวดหมู่ข่าวเตือนภัย ปภ. Today
            Data301Set.DocSet r = new Data301Set.DocSet { message = "ok", status = 1, rows = new List<Data301Set.DataRow>() }; ;
            try {
                var conn = ApiConnService.GetApiInfo("data301"); 
                string url = $"{conn.source_base_url}{conn.source_api_url}?datefrom={datefrom}&dateto={dateto}";
                var responds = await _clientService.GetAllAsync<Data301Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data301Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }


        async public Task<Data302Set.DocSet> Data302(string datefrom, string dateto) {
            //5 อันดับข่าว ปภ. Today
            Data302Set.DocSet r = new Data302Set.DocSet { message = "ok", status = 1, rows = new List<Data302Set.DataRow>() }; ;
            try {
                var conn = ApiConnService.GetApiInfo("data302");

                string url = $"{conn.source_base_url}{conn.source_api_url}?datefrom={datefrom}&dateto={dateto}";
                var responds = await _clientService.GetAllAsync<Data302Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data302Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }


        async public Task<Data303Set.DocSet> Data303(string datefrom, string dateto) {
            //จำนวนผู้เข้าชมตามช่วงเวลา
            Data303Set.DocSet r = new Data303Set.DocSet { message = "ok", status = 1, rows = new List<Data303Set.DataRow>() }; ;
            try {
                var conn = ApiConnService.GetApiInfo("data303");

                string url = $"{conn.source_base_url}{conn.source_api_url}?datefrom={datefrom}&dateto={dateto}";
                var responds = await _clientService.GetAllAsync<Data303Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data303Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

        async public Task<Data304Set.DocSet> Data304(string datefrom, string dateto) {
            //จำนวนผู้เข้าชมประจำวัน
            Data304Set.DocSet r = new Data304Set.DocSet{ message = "ok", status = 1, rows = new List<Data304Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data304");

                string url = $"{conn.source_base_url}{conn.source_api_url}?datefrom={datefrom}&dateto={dateto}";
                var responds = await _clientService.GetAllAsync<Data304Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data304Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
        async public Task<Data305Set.DocSet> Data305(string datefrom, string dateto) {
            //5 อันดับบราวเซอร์ที่ใช้งาน
            Data305Set.DocSet r = new Data305Set.DocSet { message = "ok", status = 1, rows = new List<Data305Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data305");

                string url = $"{conn.source_base_url}{conn.source_api_url}?datefrom={datefrom}&dateto={dateto}";
                var responds = await _clientService.GetAllAsync<Data305Set.DocSet>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r = (Data305Set.DocSet)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

        async public Task<Data306Set.DocSet> Data306() {
            //ข่าวล่าสุด 100 ลำดับแรก
            Data306Set.DocSet r = new Data306Set.DocSet { message="ok",status=1,rows=new List<Data306Set.DataRow>()};
            try {
                var conn = ApiConnService.GetApiInfo("data306");

                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<List<Data306Set.DataRow>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r.rows = (List<Data306Set.DataRow>)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

    }
}