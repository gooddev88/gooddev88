using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
 

namespace Robot.Service.Api {
    public class ClientService {
        private readonly HttpClient httpClient;
        private readonly IConfiguration config;
        private ProtectedLocalStorage _protectedLocalStore;
        public ClientService(IConfiguration _config, ProtectedLocalStorage protectedLocalStore) {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            this.httpClient = new HttpClient((httpClientHandler));
            this.httpClient.DefaultRequestHeaders.Add("Accept", "application / json");
            this._protectedLocalStore = protectedLocalStore;
            config = _config;
        }


        public class ResponseMessage {
            public string StatusCode { get; set; }
            public object Result { get; set; }
            public string Message { get; set; }
        }
        public async Task<ResponseMessage> Post<T>(string dataEndpointUri, object values,string token) {
            try {
                //var token_q = await _protectedLocalStore.GetAsync<string>(Globals.AuthToken);
                //var token= token_q.Success ? token_q.Value : "";
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                //var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(values);
                var response = await httpClient.PostAsJsonAsync(dataEndpointUri, values);

                if (response.StatusCode.ToString() == "Unauthorized") {
                    var res = new ResponseMessage() {
                        StatusCode = response.StatusCode.ToString(),
                        Result = 0,
                        Message = "Unauthorized"
                    };
                    return await Task.FromResult(res);
                } else {
                    response.EnsureSuccessStatusCode();
                    var Result = await response.Content.ReadFromJsonAsync<T>();
                    var res = new ResponseMessage() {
                        StatusCode = response.StatusCode.ToString(),
                        Result = Result,
                        Message = "Success"
                    };
                    return await Task.FromResult(res);
                }
            } catch (Exception ex) {
                var httpResMsg = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                var res = new ResponseMessage() {
                    StatusCode = httpResMsg.StatusCode.ToString(),
                    Result = ex.Message,
                    Message = "Error"
                };
                return await Task.FromResult(res);
            }

        }
        //public async Task<ResponseMessage> Post<T>(string dataEndpointUri, object values,string authen_token) {
        //    try {
        //        // สำหรับการเรียกใช้จาก api ด้วยกัน
               
        //        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authen_token);
        //        var response = await httpClient.PostAsJsonAsync(dataEndpointUri, values);

        //        if (response.StatusCode.ToString() == "Unauthorized") {
        //            var res = new ResponseMessage() {
        //                StatusCode = response.StatusCode.ToString(),
        //                Result = 0,
        //                Message = "Unauthorized"
        //            };
        //            return await Task.FromResult(res);
        //        } else {
        //            response.EnsureSuccessStatusCode();
        //            var Result = await response.Content.ReadFromJsonAsync<T>();
        //            var res = new ResponseMessage() {
        //                StatusCode = response.StatusCode.ToString(),
        //                Result = Result,
        //                Message = "Success"
        //            };
        //            return await Task.FromResult(res);
        //        }
        //    } catch (Exception ex) {
        //        var httpResMsg = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        //        var res = new ResponseMessage() {
        //            StatusCode = httpResMsg.StatusCode.ToString(),
        //            Result = ex.Message,
        //            Message = "Error"
        //        };
        //        return await Task.FromResult(res);
        //    }

        //}
        public async Task<ResponseMessage> GetAllAsync<T>(string dataEndpointUri,string token) {
            //IEnumerable<T> result = default;
            var res = new ResponseMessage();
            try {

           


                //var token_q = await _protectedLocalStore.GetAsync<string>(Globals.AuthToken);
                //var token = token_q.Success ? token_q.Value : ""; 
                 
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await httpClient.GetAsync(dataEndpointUri);

                if (response.StatusCode.ToString() == "Unauthorized") {
                    res.StatusCode = response.StatusCode.ToString();
                    res.Message = "Unauthorized";

                    return await Task.FromResult(res);
                } else {
                    response.EnsureSuccessStatusCode();
                    var Result = await response.Content.ReadFromJsonAsync<T>();

                    res.StatusCode = response.StatusCode.ToString();
                    res.Result = Result;
                    res.Message = "Success";

                    var xx = res;
                    return await Task.FromResult(res);
                }
            } catch (Exception ex) {
                var httpResMsg = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                res.StatusCode = httpResMsg.StatusCode.ToString();
                res.Result = ex.Message;
                res.Message = "Error";

                return await Task.FromResult(res);
            }
        }



    }
}
