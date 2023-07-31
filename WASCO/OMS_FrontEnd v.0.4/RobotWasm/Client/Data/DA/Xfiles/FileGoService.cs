using Blazored.LocalStorage;
using RobotWasm.Client.Service.Api;
using RobotWasm.Client.Service.Authen;
using RobotWasm.Shared.Data.ML.FileGo;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Client.Data.DA.Xfiles {
    public class FileGoService { 
        private readonly HttpClient _http; 
        public FileGoService( HttpClient http) {
            _http = http; 
        }
        public async Task<I_BasicResult> LoginApiFileGo() {
            I_BasicResult? output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/FileGo/LoginApiFileGo");
                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                output.Result = "fail";
                output.Message1 = ex.Message;
            }
         
            return output;
        }

        async public Task<FilesInfo> NewFilesInfoPostgres(string doctype, string rcom, string? com, string docid) {
            com = com == null ? "" : com;
            FilesInfo? doc = new FilesInfo();
            try {
                var res = await _http.GetAsync($"api/FileGo/NewFilesInfoPostgres?doctype={doctype}&rcom={rcom}&com={com}&docid={docid}");
                doc = JsonSerializer.Deserialize<FilesInfo>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> DeleteFileByFileIdPostgres(string rcom, string com, string doctype, string docid, string fileid, string deleted_by) {
            I_BasicResult? output = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/FileGo/DeleteFileByFileIdPostgres?rcom={rcom}&com={com}&doctype={doctype}&docid={docid}&fileid={fileid}&deleted_by={deleted_by}");
                output = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        

        public async Task<string> GetFileUrlPostgres(string file_id) {
            string url = "";
            try {
                  url = await _http.GetStringAsync($"api/FileGo/GetFileUrlPostgres?file_id={file_id}");
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return url;
        }

        async public Task<I_BasicResult> SaveToFileGo(List<FilesInfo> data,string user) {
            I_BasicResult? r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string url = $"api/file/Upload/SaveToFileGoPostgres?user={user}";
                var res = await _http.PostAsJsonAsync(url, data);
                r = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return r;
        }

    }
}
