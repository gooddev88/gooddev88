using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.DPMBaord.ExclusiveBoard;
using RobotWasm.Shared.Data.DimsDB;
using System.Net.Http.Json;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.Board {
    public class BoardMasterService {

        private readonly HttpClient _http;
        public BoardMasterService(HttpClient Http) {
            _http = Http;
        }
   

        public I_Board_MasterSet DocSet { get; set; }


        async public Task<I_Board_MasterSet> GetDocSet(string docid) {
            docid = docid == null ? "" : docid;
            I_Board_MasterSet doc = new I_Board_MasterSet();
            try {
                var res = await _http.GetAsync($"api/BoardMaster/GetDocSet?docid={docid}");
                doc = JsonSerializer.Deserialize<I_Board_MasterSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_Board_MasterSet> SaveBoardMaster(I_Board_MasterSet data,bool isnew) {
            I_Board_MasterSet doc = new I_Board_MasterSet();
            try {
                var res = await _http.GetAsync($"api/BoardMaster/SaveBoardMaster");
                doc = JsonSerializer.Deserialize<I_Board_MasterSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> DeleteDoc(string docid,string modified_by) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/BoardMaster/DeleteDoc?docid={docid}&modified_by={modified_by}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_BasicResult?> ReOrder(List<board_master> data)
        {
            I_BasicResult? output = new I_BasicResult();
            try
            {
                string strPayload = JsonSerializer.Serialize(data);
                var response = await _http.PostAsJsonAsync($"api/BoardMaster/ReOrder", strPayload);
                output = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            finally
            {
            }
            return output;
        }

        async public Task<I_Board_MasterSet> GetLatestFiles(I_Board_MasterSet f)
        {
            I_Board_MasterSet? output = new I_Board_MasterSet();
            try
            {
                string url = $"api/BoardMaster/GetLatestFiles";
                var res = await _http.PostAsJsonAsync(url, f);
                output = JsonSerializer.Deserialize<I_Board_MasterSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<vw_board_master>> ListDoc(string? search)
        {
            List<vw_board_master> output = new List<vw_board_master>();
            try
            {
                var res = await _http.GetAsync($"api/BoardMaster/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<vw_board_master>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<short> GenSort() {
            short output = 0;
            try {
                var res = await _http.GetAsync($"api/BoardMaster/GenSort");
                output = JsonSerializer.Deserialize<short>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        public static I_Board_MasterSet NewTransaction(string boardtype) {
            I_Board_MasterSet n = new I_Board_MasterSet();
            n.head = NewHead(boardtype);
            n.files = new List<vw_xfile_ref>();
            return n;
        }

        public static board_master NewHead(string boardtype) {
            board_master n = new board_master();

            n.board_id = ""; 
            n.name = "";
            n.description = "";
            n.board_type = boardtype;
            n.sort = 0;
            n.board_url = "";
            n.img_path = "";
            n.page = "";
            n.authen_id = "dpm_prod";
            n.is_active = 1;
            n.is_default = 0;

            return n;
        }

        public static List<board_master> ConvertViewToHead(List<vw_board_master> input)
        {
            List<board_master> result = new List<board_master>();
            foreach (var l in input)
            {
                board_master n = new board_master();

                n.board_id = l.board_id;
                n.name = l.name;
                n.description = l.description;
                n.board_type = l.board_type;
                n.sort = Convert.ToInt32(l.sort);
                n.board_url = l.board_url;
                n.img_path = l.img_path;
                n.page = l.page;
                n.authen_id = l.authen_id;
                n.is_active = l.is_active; ;
                n.is_default = l.is_default; ;
                result.Add(n);
            }

            return result;
        }

    }
}
