using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.ML.Master.MasterType;
 
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.GaDB;
using System.Net.Http.Json;

namespace RobotWasm.Client.Data.DA.Master {
    public class MasterTypeService {

        private readonly HttpClient _http;
        public MasterTypeService(HttpClient Http) {
            _http = Http;
        }
   
        public I_MasterTypeSet DocSet { get; set; }

        async public Task<I_MasterTypeSet> GetDocSet(string docid,string rcom) {
            docid = docid == null ? "" : docid;
            rcom = rcom == null ? "" : rcom;
            I_MasterTypeSet? doc = new I_MasterTypeSet();
            try {
                var res = await _http.GetAsync($"api/MasterType/GetDocSet?docid={docid}&rcom={rcom}");
                doc = JsonSerializer.Deserialize<I_MasterTypeSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }


        async public Task<I_MasterTypeSet> SaveMaster(I_MasterTypeSet data) {
            I_MasterTypeSet? doc = new I_MasterTypeSet();
            try {
                var res = await _http.GetAsync($"api/MasterType/SaveMaster");
                doc = JsonSerializer.Deserialize<I_MasterTypeSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult?> ReOrder(List<MasterTypeLine> data)
        {
            I_BasicResult? output = new I_BasicResult();
            try
            {
                string strPayload = JsonSerializer.Serialize(data);
                var response = await _http.PostAsJsonAsync($"api/MasterType/ReOrder", strPayload);
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


        async public Task<List<MasterTypeLine>> ListMasterType(string rcom, string masid, bool isShowBlank) {
            rcom = rcom == null ? "" : rcom;
            List<MasterTypeLine>? output = new List<MasterTypeLine>();
            try {
                var res = await _http.GetAsync($"api/MasterType/ListType?rcom={rcom}&masid={masid}&isShowBlank={isShowBlank}");
                output = JsonSerializer.Deserialize<List<MasterTypeLine>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        async public Task<List<MasterTypeHead>> ListDoc(string? search) {
            List<MasterTypeHead>? output = new List<MasterTypeHead>();
            try {
                var res = await _http.GetAsync($"api/MasterType/ListDoc?search={search}");
                output = JsonSerializer.Deserialize<List<MasterTypeHead>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }

        #region Newtransaction

        public static I_MasterTypeSet NewTransaction(string rcom)
        {
            var doc = new I_MasterTypeSet();
            doc.Head = new MasterTypeHead();
            doc.Line = new List<MasterTypeLine>();
            doc.LineActive = NewLine("ALL", rcom, doc.Line);
            return doc;
        }



        public static MasterTypeLine NewLine(string useFor, string rcom, List<MasterTypeLine> line)
        {
            MasterTypeLine n = new MasterTypeLine();

            n.MasterTypeID = "";
            if (useFor.ToUpper() == "ALL")
            {
                n.RComID = "";
            }
            else
            {
                n.RComID = rcom;
            }
            n.ValueTXT = "";
            n.ValueNUM = 0;
            n.Description1 = "";
            n.Description2 = "";
            n.Description3 = "";
            n.Description4 = "NEW";
            n.ParentID = "";
            n.ParentValue = "";
            n.Sort = GenSort(line);
            n.RefID = "";
            n.IsSysData = false;
            n.RefID = "";
            n.IsActive = true;
            return n;
        }
        public static int GenSort(List<MasterTypeLine> line)
        {
            int result = 1;
            try
            {
                var max_linenum = line.Max(o => o.Sort);
                result = max_linenum + 1;
            }
            catch { }
            return result;
        }
        public static MasterTypeLine AddLineItem(string checkExistItem, string usefor, string rcom, I_MasterTypeSet input)
        {
            input.Line.RemoveAll(o => o.Description4 == "NEW");
            input.LineActive = input.Line.Where(o => o.ValueTXT == checkExistItem).FirstOrDefault();
            if (input.LineActive == null)
            {//new line
                input.Line.Add(NewLine(usefor, rcom, input.Line));
                input.LineActive = input.Line.Where(o => o.Description4 == "NEW").OrderByDescending(o => o.Sort).FirstOrDefault();
            }

            return input.LineActive;
        }

        #endregion

    }
}
