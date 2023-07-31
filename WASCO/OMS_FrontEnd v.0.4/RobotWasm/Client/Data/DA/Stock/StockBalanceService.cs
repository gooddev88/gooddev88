using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using RobotWasm.Shared.Data.ML.Param;
using static RobotWasm.Shared.Data.DA.SOFuncService;
using RobotWasm.Shared.Data.ML.Login;
using Blazored.SessionStorage;
using Telerik.SvgIcons;
using RobotWasm.Shared.Data.ML.Promotion;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using RobotWasm.Shared.Data.ML.Order;

namespace RobotWasm.Client.Data.DA.Stock {
    public class StockBalanceService {

        private readonly HttpClient _http;
        private ISessionStorageService _sessionStorage;
        public StockBalanceService(HttpClient Http, ISessionStorageService sessionStorage) {
            _http = Http;
            _sessionStorage = sessionStorage;
        }

        #region ItemBl

        async public Task<List<vw_STKBal>> ListLot(string? rcom, string? comId,string? itemid,string? loc) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            itemid = itemid == null ? "" : itemid;
            List<vw_STKBal>? doc = new List<vw_STKBal>();
            ItemParam p =new ItemParam { RComID= rcom ,ComID=comId,ItemID=itemid,LocID=loc};
            try {
                string strPayload = JsonSerializer.Serialize(p);
                string url = $"api/StockBalance/ListLot";
                var res = await _http.PostAsJsonAsync(url, strPayload);
                doc = JsonSerializer.Deserialize<List<vw_STKBal>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
                 
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

      
        async public Task<List<LocationInfo>> ListLocID(string rcom, string com, List<string> loc_access) {
            List<LocationInfo>? output = new List<LocationInfo>();

            try {
                string url = $"api/StockBalance/ListLocID";
                LocParamUser p = new LocParamUser { RCom = rcom, Com = com, LocIds = loc_access };
                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                output = await res.Content.ReadFromJsonAsync<List<LocationInfo>>(new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }


        async public Task<List<vw_ItemInfoWithPhotoAndStock>> ListStockBalSale(StockBalParam f,LoginSet login) {
            List<vw_ItemInfoWithPhotoAndStock>? output = new List<vw_ItemInfoWithPhotoAndStock>();
            try {
                f.UIC = login.UserInCompany;
                f.UILoc = login.UserInLoc.Select(o=>o.LocID).ToList();

                string url = $"api/StockBalance/ListStockBalSale";
                string strPayload = JsonSerializer.Serialize(f);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                output = JsonSerializer.Deserialize<List<vw_ItemInfoWithPhotoAndStock>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }
        async public Task<I_BasicResult?> PrintStockBalSale(StockBalParam f, LoginSet login) {
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                f.UIC = login.UserInCompany;
                f.UILoc = login.UserInLoc.Select(o => o.LocID).ToList();

                string url = $"api/StockBalance/PrintStockBalSale";
                string strPayload = JsonSerializer.Serialize(f);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }
        async public Task<I_BasicResult?> PrintStockPromotion(StockBalParam f, LoginSet login) {
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                f.UIC = login.UserInCompany;
                f.UILoc = login.UserInLoc.Select(o => o.LocID).ToList();

                string url = $"api/StockBalance/PrintStockPromotion";
                string strPayload = JsonSerializer.Serialize(f);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                result = JsonSerializer.Deserialize< I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }
        async public void SetSessionItemBalFiterSet(StockBalParam data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = JsonSerializer.Serialize(data, jso);
            await _sessionStorage.SetItemAsync("ItemBal_Fiter", json);
        }
        async public Task<StockBalParam>? GetSessionItemBalFiterSet() {
            StockBalParam? result = NewItemBalFilterSet();
            var strdoc = await _sessionStorage.GetItemAsync<string>("ItemBal_Fiter");
            if (strdoc != null) {
                result = JsonSerializer.Deserialize<StockBalParam>(strdoc, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } else {
                result = NewItemBalFilterSet();
            }
            return result;
        }
        public static StockBalParam NewItemBalFilterSet() {
            StockBalParam n = new StockBalParam();
            n.RCom = "";
            n.Com = "";
            n.Brand = "";
            n.LocID = "";
            n.Type = "";
            n.UIC = new List<string>();
            n.UILoc = new List<string>();
            n.CostCenter = "";
            n.Year = DateTime.Now.Date.Year.ToString();
            n.Month = DateTime.Now.Date.Month.ToString();
            n.SearchText = "";
            n.ShowNotZero = true;


            return n;
        }
        async public Task<List<vw_MasterTypeLine>> ListViewMasterType(string rcom, string masid, bool isShowBlank) {
            rcom = rcom == null ? "" : rcom;
            List<vw_MasterTypeLine>? output = new List<vw_MasterTypeLine>();
            try {
                var res = await _http.GetAsync($"api/MasterType/ListViewMasterType?rcom={rcom}&masid={masid}&isShowBlank={isShowBlank}");
                output = JsonSerializer.Deserialize<List<vw_MasterTypeLine>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return output;
        }
        #endregion
        #region StockPromotion

  
        async public void SetSessionStockPromotionFiterSet(StockBalParam data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = JsonSerializer.Serialize(data, jso);
            await _sessionStorage.SetItemAsync("StockPromotion_Fiter", json);
        }
        async public Task<StockBalParam>? GetSessionStockPromotionFiterSet() {
            StockBalParam? result = NewStockPromotionFilterSet();
            var strdoc = await _sessionStorage.GetItemAsync<string>("StockPromotion_Fiter");
            if (strdoc != null) {
                result = JsonSerializer.Deserialize<StockBalParam>(strdoc, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } else {
                result = NewStockPromotionFilterSet();
            }
            return result;
        }
        public static StockBalParam NewStockPromotionFilterSet() {
            StockBalParam n = new StockBalParam();
            n.RCom = "";
            n.Com = "";
            n.Brand = "";
            n.LocID = "";
            n.Type = "";
            n.UIC = new List<string>();
            n.UILoc = new List<string>();
            n.CostCenter = "";
            n.Year = DateTime.Now.Date.Year.ToString();
            n.Month = DateTime.Now.Date.Month.ToString();
            n.SearchText = "";
            n.ShowNotZero = true;


            return n;
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
        #endregion



        async public Task<I_BasicResult?> PrintStockBal(I_SODocSet doc) {
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string url = $"api/StockBalance/PrintStockBal";
                
                string strPayload = JsonSerializer.Serialize(doc);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }


    }
}
