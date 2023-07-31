using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData.BoardParam;
using RobotWasm.Shared.Data.ML.DPMBaord.CustomBoard;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.Board {
    public class CustomBoardService {

        HttpClient _http;
        public CustomBoardService(HttpClient http) {
            _http = http;
        }
        public CustomBoardDocSet? BoardDocset { get; set; }
        async public Task<I_BasicResult> CreateUpdateBoard(custom_board_in_user data) {
            I_BasicResult? output = new I_BasicResult();
            try {
                string url = $"api/CustomBoard/CreateUpdateBoard";
                string strPayload = JsonSerializer.Serialize(data);
                var res = await _http.PostAsync(url, new StringContent(strPayload, Encoding.UTF8, "application/json"));
                var status = res.StatusCode;
                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                output.Result = "fail";
                output.Message1 = ex.Message;
            } finally {
            }
            return output;
        }
        async public Task<I_BasicResult> DeleteCustomBoard(string board_id) {
            I_BasicResult? output = new I_BasicResult();
            try {
                var res = await _http.GetAsync($"api/CustomBoard/RemoveCustomBoard?board_id={board_id}");
                var r = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                output.Result = "fail";
                output.Message1 = ex.Message;
            } finally {
            }
            return output;
        }
        async public Task<List<vw_custom_board_in_user>?> ListCustomBoardInUser(string user) {
            List<vw_custom_board_in_user>? output = new List<vw_custom_board_in_user>();
            try {
                var res = await _http.GetAsync($"api/CustomBoard/ListCustomBoardInUser?user={user}");
                output = JsonSerializer.Deserialize<List<vw_custom_board_in_user>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        async public Task<CustomBoardDocSet?> GetCustomBoard(string board_id) {
            CustomBoardDocSet? output = new CustomBoardDocSet();
            try {
                var res = await _http.GetAsync($"api/CustomBoard/GetCustomBoard?board_id={board_id}");

                output = JsonSerializer.Deserialize<CustomBoardDocSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        async public Task<custom_widget_master> GetWidgetInfo(string widget_id) {
            custom_widget_master? output = new custom_widget_master();
            try {
                var res = await _http.GetAsync($"api/CustomBoard/GetWidgetInfo?widget_id={widget_id}");
                output = JsonSerializer.Deserialize<custom_widget_master>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }
        async public Task<List<custom_widget_master_select>> ListWidgetMaster(string board_id) {
            List<custom_widget_master_select>? output = new List<custom_widget_master_select>();
            try {
                var res = await _http.GetAsync($"api/CustomBoard/ListWidgetMasterForAdd?board_id={board_id}");
                output = JsonSerializer.Deserialize<List<custom_widget_master_select>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        async public Task<I_BasicResult?> RemoveWidget(string board_id, string widget_id) {
            I_BasicResult? output = new I_BasicResult();
            try {
                var res = await _http.GetAsync($"api/CustomBoard/RemoveWidget?board_id={board_id}&widget_id={widget_id}");

                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }
        async public Task<I_BasicResult?> AddWidget(List<custom_widget_master_select> widgets) {
            I_BasicResult? output = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(widgets);
                string url = $"api/CustomBoard/SaveWidget";
                var response = await _http.PostAsJsonAsync($"api/CustomBoard/AddWidget", strPayload);

                output = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }
        async public Task<I_BasicResult?> SaveWidget(List<vw_custom_widget_in_user> widgets) {
            I_BasicResult? output = new I_BasicResult();
            try {

                string strPayload = JsonSerializer.Serialize(widgets);
                //var res = await _http.PostAsJsonAsync($"api/CustomBoard/SaveWidget", new StringContent(strPayload, Encoding.UTF8, "application/json"));
                var response = await _http.PostAsJsonAsync($"api/CustomBoard/SaveWidget", strPayload);
                //   var response = await _http.PostAsync(url, widgets);
                var status = response.StatusCode;


                output = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }

        #region  get param
        async public Task<List<custom_widget_param_in_user>> GetWidgetParam(string board_id, string widget_id) {
            List<custom_widget_param_in_user>? output = new List<custom_widget_param_in_user>();
            try {
                var res = await _http.GetAsync($"api/CustomBoard/GetWidgetParam?board_id={board_id}&widget_id={widget_id}");

                output = JsonSerializer.Deserialize<List<custom_widget_param_in_user>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            } finally {
            }
            return output;
        }
        async public Task<I_BasicResult> SaveWidgetParam(List<custom_widget_param_in_user> data, int save_all_in_group) {
            I_BasicResult? output = new I_BasicResult();
            try {
                string url = $"api/CustomBoard/GetWidgetParam?save_all_in_group={save_all_in_group}";
                string strPayload = JsonSerializer.Serialize(data);
                var res = await _http.PostAsync(url, new StringContent(strPayload, Encoding.UTF8, "application/json"));
                var status = res.StatusCode;
                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                output.Result = "fail";
                output.Message1 = ex.Message;
            } finally {
            }
            return output;
        }
         
        #endregion


        #region board param
        public static List<custom_widget_param_in_user> CreateDefaultParamG10(string board_id, string widget_id,string username) {
            List<custom_widget_param_in_user> output = new List<custom_widget_param_in_user>();
            try {
                string group_id = "10";
                string group_name = "อุบัติเหตุ";
                //  ParamGroup10 
                List<custom_widget_param_in_user> new_defualt_param = new List<custom_widget_param_in_user>();
                ParamG10 g10 = new ParamG10();
                g10.DateBegin = new DateTime(DateTime.Now.Year, 1, 1);
                g10.DateEnd = new DateTime(DateTime.Now.Year, 12, 31);

                custom_widget_param_in_user d = new custom_widget_param_in_user();
                d.username = username;
                d.board_id = board_id;
                d.widget_id = widget_id;
                d.param_id = "filter_option";
                d.data = "ระบุวันที่";//ปีใหม่, สงกรานต์
                d.data_type = "datetime";
                d.group_id = group_id;
                d.group_name = group_name;
                d.is_active = 1;

                new_defualt_param.Add(d);



                d = new custom_widget_param_in_user();
                d.username = username;
                d.board_id = board_id;
                d.widget_id = widget_id;
                d.param_id = "date_begin";
                d.data = g10.DateBegin.ToString("yyyyMMdd");
                d.data_type = "datetime";
                d.group_id = group_id;
                d.group_name = group_name;
                d.is_active = 1;
                new_defualt_param.Add(d);



                d = new custom_widget_param_in_user();
                d.username = username;
                d.board_id = board_id;
                d.widget_id = widget_id;
                d.param_id = "date_end";
                d.data = g10.DateEnd.ToString("yyyyMMdd");
                d.data_type = "datetime";
                d.group_id = group_id;
                d.group_name = group_name;
                d.is_active = 1;
                new_defualt_param.Add(d);



                d = new custom_widget_param_in_user();
                d.username = username;
                d.board_id = board_id;
                d.widget_id = widget_id;
                d.param_id = "provinces";
                d.data = "";
                d.data_type = "string";
                d.group_id = group_id;
                d.group_name = group_name;
                d.is_active = 1;
                new_defualt_param.Add(d);

            } catch (Exception ex) {
                 
                }
            
            return output;
        }
        public static List<custom_widget_param_in_user> CreateDefaultParamG50(string board_id, string widget_id, string username) {
            List<custom_widget_param_in_user> output = new List<custom_widget_param_in_user>();
            try {
                string group_id = "50";
                string group_name = "ภัย";
                //  ParamGroup10 
                List<custom_widget_param_in_user> new_defualt_param = new List<custom_widget_param_in_user>();
                ParamG50 g50 = new ParamG50();
                g50.DateBegin = new DateTime(DateTime.Now.Year, 1, 1);
                g50.DateEnd = new DateTime(DateTime.Now.Year, 12, 31);

                custom_widget_param_in_user d = new custom_widget_param_in_user();
            

                d = new custom_widget_param_in_user();
                d.username = username;
                d.board_id = board_id;
                d.widget_id = widget_id;
                d.param_id = "date_begin";
                d.data = g50.DateBegin.ToString("yyyyMMdd");
                d.data_type = "datetime";
                d.group_id = group_id;
                d.group_name = group_name;
                d.is_active = 1;
                new_defualt_param.Add(d);



                d = new custom_widget_param_in_user();
                d.username = username;
                d.board_id = board_id;
                d.widget_id = widget_id;
                d.param_id = "date_end";
                d.data = g50.DateEnd.ToString("yyyyMMdd");
                d.data_type = "datetime";
                d.group_id = group_id;
                d.group_name = group_name;
                d.is_active = 1;
                new_defualt_param.Add(d);

 

            } catch (Exception ex) {

            }

            return output;
        }
        #endregion
    }
}
