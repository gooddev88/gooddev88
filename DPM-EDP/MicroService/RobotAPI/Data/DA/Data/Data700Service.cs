using Dapper;
using MySql.Data.MySqlClient;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.ML.Data;
using RobotAPI.Services.Api;
using System.Collections.Generic;

namespace RobotAPI.Data.DA.Data {
    public class Data700Service {
        ClientService _clientService;

        public Data700Service(ClientService clientService) {
            _clientService = clientService;
        }

        async public Task<List<dqt_data_logs>> ListLog(string datebegin,string dateend,string status,string search) {

            List<dqt_data_logs> output = new List<dqt_data_logs>();
            try {
                var conn = ApiConnService.GetApiInfo("data701");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.Post<List<Data606Set.DataRow>>(url, conn.source_api_token, "");
                //if (responds.StatusCode.ToString().ToUpper() != "OK") {
                //    r.message = responds.Result;
                //    r.status = 0;
                //} else {
                //    r.rows = (List<Data606Set.DataRow>)responds.Result;
                //}

            } catch (Exception ex) {
                //r.message = ex.Message;
                //r.status = 0;
            }
            return output;
        }
       
    }
}