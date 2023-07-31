using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;
using RobotWasm.Shared.Data.ML.Shared;
using Blazored.LocalStorage;
using RobotWasm.Shared.Data.ML.Param;
using Blazored.SessionStorage;
using RobotWasm.Shared.Data.ML.Promotion;

namespace RobotWasm.Client.Data.DA.Promotion {
    public class PromotionService {
        private readonly HttpClient _http;
        private ILocalStorageService _localStorage;
        private ISessionStorageService _sessionStorage;
        public PromotionService(HttpClient Http, ILocalStorageService localStorage, ISessionStorageService sessionStorage) {
            _http = Http;
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;
        }
        public I_PromotionSet PromotionSet { get; set; }
        //public List<ItemDesplay> ListProduct { get; set; } = new List<ItemDesplay>();
        I_PromotionSet promotionSet { get; set; } = new I_PromotionSet();


        #region Promotion
        async public Task<List<Promotions>> ListPromotion(string rcom, string com, DateTime tranDate) {
            List<Promotions>? result = new List<Promotions>();
            try {
                PromotionParam p = new PromotionParam { RComID = rcom, ComID = com, TranDate = tranDate };
                string url = $"api/Promotion/ListPromotion";
                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                result = JsonSerializer.Deserialize<List<Promotions>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) { }
            return result;
        }
        async public Task<I_PromotionSet> GetPromotion(string rcom, string com, string proid) {
            I_PromotionSet? result = new I_PromotionSet();
            try {

                ItemParam p = new ItemParam { RComID = rcom, ComID = com, ProID = proid, ItemID = proid };
                string url = $"api/SO/ListSaleItem";

                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);

                result = JsonSerializer.Deserialize<I_PromotionSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_PromotionSet> GetPromotionSet(string rcom, string com, string brandid, string cateid, string locid, string proid) {
            I_PromotionSet? result = new I_PromotionSet();
            try {

                ItemParam p = new ItemParam { RComID = rcom, ComID = com, ProID = proid, BrandID = brandid, CateID = cateid, LocID = locid, ItemID = proid };
                string url = $"api/Promotion/GetPromotionSet";

                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);

                result = JsonSerializer.Deserialize<I_PromotionSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }
        #endregion

    }
}
