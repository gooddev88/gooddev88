using Dapper;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.ML.Data;
using RobotAPI.Data.ML.Eoc;
using RobotAPI.Services.Api;
using System.Collections.Generic;

namespace RobotAPI.Data.DA.Data {
    public class EOCService {
        ClientService _clientService;

        public EOCService(ClientService clientService) {
            _clientService = clientService;
        }

        async public Task<List<WarningPublish>> WarningPublish(string? search ) { 
            List<WarningPublish> output = new List<WarningPublish>();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("WarningPublish");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<List<WarningPublish>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output= null;
                } else {
                    output = (List<WarningPublish>)responds.Result; 
                } 
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }
        async public Task<List<WarningEvent>> WarningEvent(string? search = "") {
            List<WarningEvent> output = new List<WarningEvent>();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("WarningPublish");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<List<WarningEvent>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<WarningEvent>)responds.Result;
                } 
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }


        async public Task<List<Expense>> Expense(string? search = "") {
            List<Expense> output = new List<Expense>();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("Expense");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<List<Expense>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<Expense>)responds.Result; 
                }
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }

        async public Task<List<Indicator>> Indicator(string? search = "") {
            List<Indicator> output = new List<Indicator>();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("Indicator");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<List<Indicator>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<Indicator>)responds.Result; 
                }
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }

        async public Task<List<IDP>> IDP(string? search = "") {
            List<IDP> output = new List<IDP>();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("IDP");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                var responds = await _clientService.GetAllAsync<List<IDP>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<IDP>)responds.Result; 
                }
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }

        async public Task<List<Help>> Help(string? province = "") {
            List<Help> output = new List<Help>();
            try {
                province = province == null ? "" : province.ToLower();
                var conn = ApiConnService.GetApiInfo("Help");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                url = url + $"?province={province}";
                var responds = await _clientService.GetAllAsync<List<Help>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<Help>)responds.Result; 
                }
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }

        async public Task<List<Shelter>> Shelter(string? province = "") {
            List<Shelter> output = new List<Shelter>();
            try { 
                province = province == null ? "" : province.ToLower();
                var conn = ApiConnService.GetApiInfo("Shelter");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                url = url + $"&province={province}";
                var responds = await _clientService.GetAllStringAsync<List<Shelter>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<Shelter>)responds.Result;
                }
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }

        async public Task<List<Hospital>> Hospital(string? province = "") {
            //  ข้อมูลศูนย์พักพิง Shelter Information.
                        List<Hospital> output = new List<Hospital>();
            try {
                province = province == null ? "" : province.ToLower();
                var conn = ApiConnService.GetApiInfo("Hospital");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                url = url + $"&province={province}";
                var responds = await _clientService.GetAllStringAsync<List<Hospital>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<Hospital>)responds.Result;
                }
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }
        async public Task<List<Contact>> Contact(string? province = "") {
            //ข้อมูลสถานพยาบาล Hospital Info.
            List<Contact> output = new List<Contact>();
            try {
                province = province == null ? "" : province.ToLower();
                var conn = ApiConnService.GetApiInfo("Hospital");
                string url = $"{conn.source_base_url}{conn.source_api_url}";
                url = url + $"&province={province}";
                var responds = await _clientService.GetAllStringAsync<List<Contact  >>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    output = null;
                } else {
                    output = (List<Contact>)responds.Result;
                }
            } catch (Exception ex) {
                output = null;
            }
            return output;
        }
    }
}