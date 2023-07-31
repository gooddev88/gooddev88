using RobotWasm.Client.Service.Api;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData;
using RobotWasm.Shared.Data.ML.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace RobotWasm.Client.Data.DA.Board {
    public class BoardService {

        HttpClient _http;
        private ClientService _clientService { get; set; }
        public BoardService(HttpClient http, ClientService clientService) {
            _http = http;
            _clientService = clientService;
        }
        public string Select_TableauID { get; set; }
        public string Temp_TableauUrl { get; set; }
        public vw_board_in_user_select Temp_SelectBoard { get; set; }
        public static string GetThisYear() {

            return Convert2ThaiYear(DateTime.Now.Year).ToString();
        }
        public static int Convert2ThaiYear(int year) {

            if (year < 2500) {
                year = year + 543;
            }
            return year;
        }
        public static int Convert2EngYear(int year) {

            if (year > 2500) {
                year = year - 543;
            }
            return year;
        }
        public static List<SelectOption> ListYearOption() {
            var this_year = Convert.ToInt32(GetThisYear());
            List<SelectOption> options = new List<SelectOption>();
            for (int i = 0; i < 10; i++) {
                options.Add(new SelectOption { Value = this_year.ToString(), Description = this_year.ToString(), Sort = i });
                this_year = this_year - 1;
            }
            return options;
        }
        async public Task<List<vw_board_in_user_select>?> ListBoardInUser(string user) {
            List<vw_board_in_user_select>? output = new List<vw_board_in_user_select>();
            try {
                var res = await _http.GetAsync($"api/Board/ListBoardInUser?user={user}");

                output = JsonSerializer.Deserialize<List<vw_board_in_user_select>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        async public Task<string> GetBoardUrltableau(string board_id) {
            string url = "";
            try {
                var res = await _http.GetAsync($"{Globals.BaseURL}/api/Tableau/GetBoardUrlForEdp?board_id={board_id}");

                url = res.Content.ReadAsStringAsync().Result;

            } catch (Exception ex) {
                var x = ex.Message;
            }
            return url;
        }

  

        public static List<SelectOption> ListParamFilterOption() {
            List<SelectOption> paramSelectOption = new List<SelectOption>();
            paramSelectOption = new List<SelectOption> {
          new SelectOption {Description= "ปีใหม่", Value= "ปีใหม่",Sort= 1 },
         new SelectOption {Description=  "สงกรานต์", Value="สงกรานต์",Sort=  2 },
        new SelectOption { Description= "ระบุวันที่",Value= "ระบุวันที่", Sort= 3 }
    };
            return paramSelectOption;
        }

        public static string ConvertDateThai(DateTime input) {
            return input.Day + @"/" + input.Month + @"/" + Convert2ThaiYear(input.Year);

        }
    }
}
