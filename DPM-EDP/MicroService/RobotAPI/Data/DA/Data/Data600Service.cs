using Dapper;
using MySql.Data.MySqlClient;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;

namespace RobotAPI.Data.DA.Data {
    public class Data600Service {
        ClientService _clientService;

        public Data600Service(ClientService clientService) {
            _clientService = clientService;
        }

        async public Task<Data606Set.DocSet> Data606() {
            //แสดงแผนก
            Data606Set.DocSet r = new Data606Set.DocSet { message = "ok", status = 1, rows = new List<Data606Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data606");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.Post<List<Data606Set.DataRow>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r.rows = (List<Data606Set.DataRow>)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
        async public Task<Data602Set.DocSet> Data602(string? year = "") {
            //ข้อมูลสัดส่วนงบประมาณหลังโอนเปลี่ยนแปลง BudgetTransfer
            Data602Set.DocSet r = new Data602Set.DocSet { message = "ok", status = 1, rows = new List<Data602Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data602");

                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }


                var responds = await _clientService.Post<List<Data602Set.DataRow>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r.rows = (List<Data602Set.DataRow>)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
        async public Task<Data601Set.DocSet> Data601(string? year = "") {
            //ข้อมูลภาพรวมการใช้งบประมาณ BudgetOverall
            Data601Set.DocSet r = new Data601Set.DocSet { message = "ok", status = 1, rows = new List<Data601Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data601");

                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }


                var responds = await _clientService.Post<List<Data601Set.DataRow>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r.rows = (List<Data601Set.DataRow>)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

        async public Task<Data603Set.DocSet> Data603(string? year = "") {
            //ข้อมูลภาพรวมการใช้งบประมาณตามประเภทงบ
            Data603Set.DocSet r = new Data603Set.DocSet { message = "ok", status = 1, rows = new List<Data603Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data603");

                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }


                var responds = await _clientService.Post<List<Data603Set.DataRow>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r.rows = (List<Data603Set.DataRow>)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }

        async public Task<Data604Set.DocSet> Data604(string? year = "") {
            //ข้อมูลเป้าหมายตาม ครม.
            Data604Set.DocSet r = new Data604Set.DocSet { message = "ok", status = 1, rows = new List<Data604Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data604"); 
                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }
                var responds = await _clientService.Post<List<Data604Set.DataRow>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r.rows = (List<Data604Set.DataRow>)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
        async public Task<Data605Set.DocSet> Data605(string? year = "") {
            //ข้อมูลเป้าหมายตาม ครม.
            Data605Set.DocSet r = new Data605Set.DocSet { message = "ok", status = 1, rows = new List<Data605Set.DataRow>() };
            try {
                var conn = ApiConnService.GetApiInfo("data605");
                string url = "";
                if (year == "") {
                    url = $"{conn.source_base_url}{conn.source_api_url}";
                } else {
                    url = $"{conn.source_base_url}{conn.source_api_url}?year={year}";
                }
                var responds = await _clientService.Post<List<Data605Set.DataRow>>(url, conn.source_api_token, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r.message = responds.Result;
                    r.status = 0;
                } else {
                    r.rows = (List<Data605Set.DataRow>)responds.Result;
                }

            } catch (Exception ex) {
                r.message = ex.Message;
                r.status = 0;
            }
            return r;
        }
    }
}