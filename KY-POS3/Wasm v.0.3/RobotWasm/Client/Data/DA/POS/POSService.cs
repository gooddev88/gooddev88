using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;
using RobotWasm.Shared.Data.ML.Shared;
using Blazored.LocalStorage;
using static RobotWasm.Shared.Data.DA.POSFuncService;
using RobotWasm.Shared.Data.DA;
using RobotWasm.Shared.Data.ML.Param;

namespace RobotWasm.Client.Data.DA.POS {
    public class POSService {

        private readonly HttpClient _http;
        private ILocalStorageService _localStorage;
        public POSService(HttpClient Http, ILocalStorageService localStorage) {
            _http = Http;
            _localStorage = localStorage;
        }

        async public Task<string> SetLocalSttorageShowProductImage() {
            var isshowproduct = await _localStorage.GetItemAsync<string>("isshowproductimage");
            if (isshowproduct == null) {
                await _localStorage.SetItemAsync("isshowproductimage", "1");
                return "1";
            } else {
                if (isshowproduct == "1") {
                    await _localStorage.SetItemAsync("isshowproductimage", "0");
                    return "0";
                } else {
                    await _localStorage.SetItemAsync("isshowproductimage", "1");
                    return "1";
                }
            }
        }

        async public Task<string> GetLocalSttorageShowProductImage() {
            var isshowproduct = await _localStorage.GetItemAsync<string>("isshowproductimage");
            return isshowproduct;
        }

        public I_POSSaleSet DocSet { get; set; }
        public List<POSMenuItem> Menu { get; set; } = new List<POSMenuItem>();
        public List<MasterTypeLine> ItemCate { get; set; } = new List<MasterTypeLine>();
        public List<SelectOption> Tenders { get; set; } = new List<SelectOption>();

        async public Task<I_POSSaleSet> GetDocSet(string docid, string rcom) {
            I_POSSaleSet result = new I_POSSaleSet();
            try {
                var res = await _http.GetAsync($"api/POS/GetDocSet?docid={docid}&rcom={rcom}");
                result = JsonSerializer.Deserialize<I_POSSaleSet>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        //async public Task<I_POSSaleSet> NewTransaction(LoginSet data,string version, string? shipto) {
        //    shipto = shipto == null ? "" : shipto;
        //    I_POSSaleSet doc = new I_POSSaleSet();
        //    try {
        //        string strPayload = JsonSerializer.Serialize(data);
        //        string url = $"api/POS/NewTransaction?version={version}&shipto={shipto}";
        //        var response = await _http.PostAsJsonAsync(url, strPayload);
        //        var status = response.StatusCode;

        //        if (status.ToString().ToLower() != "ok") {
        //        } else {
        //            doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
        //                PropertyNameCaseInsensitive = true,
        //                ReferenceHandler = ReferenceHandler.Preserve
        //            });
        //        }
        //    } catch (Exception ex) {
        //        var x = ex.Message;
        //    }
        //    return doc;
        //}

        async public Task<List<POSMenuItem>> ListMenuItem(string rcom, string comId, string custId) {
            List<POSMenuItem> result = new List<POSMenuItem>();
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            custId = custId == null ? "" : custId;         
            try {
                MenuItemParam param = new MenuItemParam { RComID = rcom, ComID = comId, CustID = custId };
                string url = $"api/POS/ListMenuItem";
                string strPayload = JsonSerializer.Serialize(param);
                var res = await _http.PostAsJsonAsync(url, strPayload);
                result = JsonSerializer.Deserialize<List<POSMenuItem>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<List<MasterTypeLine>> ListItemCate(string rcom) {
            List<MasterTypeLine> result = new List<MasterTypeLine>();
            try {
                var res = await _http.GetAsync($"api/POS/ListItemCate?rcom={rcom}");
                result = JsonSerializer.Deserialize<List<MasterTypeLine>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<List<POS_SaleLog>> ListPOS_SaleLog(string rcom, string com, string billId) {
            List<POS_SaleLog> result = new List<POS_SaleLog>();
            try {
                var res = await _http.GetAsync($"api/POS/ListPOS_SaleLog?rcom={rcom}&com={com}&billId={billId}");
                result = JsonSerializer.Deserialize<List<POS_SaleLog>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_BasicResult> DeleteDoc(string docId, string rcom, string modifiedby, string? remark) {
            remark = remark == null ? "" : remark;
            I_BasicResult? result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var res = await _http.GetAsync($"api/POS/DeleteDoc?docId={docId}&rcom={rcom}&modifiedby={modifiedby}&remark={remark}");
                result = JsonSerializer.Deserialize<I_BasicResult>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_POSSaleSet> GenNumberOrderID(I_POSSaleSet data) {
            I_POSSaleSet doc = new I_POSSaleSet();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/GenNumberOrderID";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_POSSaleSet> GenNumberInvoiceID(I_POSSaleSet data) {
            I_POSSaleSet doc = new I_POSSaleSet();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/GenNumberInvoiceID";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_POSSaleSet>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> SavePos(I_POSSaleSet data,string action) {
            I_BasicResult doc = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/SavePos?action={action}";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<POS_SaleHeadModel>> ListBill(I_BillFilterSet data) {
            List<POS_SaleHeadModel> doc = new List<POS_SaleHeadModel>();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/ListBill";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<List<POS_SaleHeadModel>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        #region Kitchen
        async public Task<List<POS_SaleLineModel>> ListOrderForKitchen(KitchenStatusParam data) {
            List<POS_SaleLineModel> doc = new List<POS_SaleLineModel>();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/ListOrderForKitchen";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<List<POS_SaleLineModel>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<I_BasicResult> UpdatePOSLineStatus(KitchenStatusParam data) {
            I_BasicResult doc = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/UpdatePOSLineStatus";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        #endregion

        async public Task<List<POS_SaleHeadModel>> ListPendingCheckBill(string rcom, string com, string macno) {
            List<POS_SaleHeadModel> result = new List<POS_SaleHeadModel>();
            try {
                var res = await _http.GetAsync($"api/POS/ListPendingCheckBill?rcom={rcom}&com={com}&macno={macno}");
                result = JsonSerializer.Deserialize<List<POS_SaleHeadModel>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return result;
        }

        async public Task<I_BasicResult> SaveTaxSlip(I_POSSaleSet data) {
            I_BasicResult doc = new I_BasicResult();
            try {
                string strPayload = JsonSerializer.Serialize(data);
                string url = $"api/POS/SaveTaxSlip";
                var response = await _http.PostAsJsonAsync(url, strPayload);
                var status = response.StatusCode;

                if (status.ToString().ToLower() != "ok") {
                } else {
                    doc = JsonSerializer.Deserialize<I_BasicResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                }
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        #region Filter
        async public void SetSessionFiterSet(I_BillFilterSet data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await _localStorage.SetItemAsync("posbill_Fiter", json);
        }
        async public Task<I_BillFilterSet>? GetSessionFiterSet() {
            I_BillFilterSet? result = POSFuncService.NewFilterSet();
            var strdoc = await _localStorage.GetItemAsync<string>("posbill_Fiter");
            if (strdoc != null) {
                result = JsonSerializer.Deserialize<I_BillFilterSet>(strdoc, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } else {
                result = POSFuncService.NewFilterSet();
            }
            return result;
        }

        async public void SetLocalStorageFiterKitchen(KitchenStatusParam data) {
            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string json = System.Text.Json.JsonSerializer.Serialize(data, jso);
            await _localStorage.SetItemAsync("kitchen_Fiter", json);
        }
        async public Task<KitchenStatusParam>? GetLocalStorageFiterKitchen() {
            KitchenStatusParam? result = POSFuncService.NewFilterKitchen();
            var strdoc = await _localStorage.GetItemAsync<string>("kitchen_Fiter");
            if (strdoc != null) {
                result = JsonSerializer.Deserialize<KitchenStatusParam>(strdoc, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } else {
                result = POSFuncService.NewFilterKitchen();
            }
            return result;
        }

        #endregion

    }
}
