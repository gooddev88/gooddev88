using RobotWasm.Shared.Data.GaDB;
using System.Text.Json;
using System.Text.Json.Serialization;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net.Http.Json;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Shared.Data.ML.Master;
using System.Collections.Generic;
using RobotWasm.Shared.Data.DA;
using System;
using RobotWasm.Shared.Data.ML.Param;

namespace RobotWasm.Client.Data.DA.Master {
    public class CustomerService {

        private readonly HttpClient _http;
        public CustomerService(HttpClient Http) {
            _http = Http;
        }

        #region GetData

        async public Task<CustomerInfo> GetCustomerInfoByCusID(string? rcom,string? comId,string cusid) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            CustomerInfo doc = new CustomerInfo();
            try {
                var res = await _http.GetAsync($"api/Customer/GetCustomerInfoByCusID?rcom={rcom}&comId={comId}&cusid={cusid}");
                doc = JsonSerializer.Deserialize<CustomerInfo>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        async public Task<List<CustomerInfo>> ListCustomerInfo(string? rcom, string? comId,string brand, string search) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            List<CustomerInfo>? doc = new List<CustomerInfo>();
            try { 
                CustomerParam p = new CustomerParam { Rcom = rcom, Com = comId,Brand=brand, Search = search }; 
                string url = $"api/Customer/ListCustomerInfo";

                string strPayload = JsonSerializer.Serialize(p);
                var res = await _http.PostAsJsonAsync(url, strPayload); 
                doc = JsonSerializer.Deserialize<List<CustomerInfo>>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            } catch (Exception ex) {
                var x = ex.Message;
            }
            return doc;
        }

        #endregion
        #region tax
        async public Task<decimal> GetTaxRate(string? rcom, string? type, string taxid) { 
            decimal result = 0;
            try {
                var res = await _http.GetAsync($"api/Customer/GetTaxRate?rcom={rcom}&type={type}&taxid={taxid}");
                result = JsonSerializer.Deserialize<decimal>(await res.Content.ReadAsStringAsync(), new JsonSerializerOptions {
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
