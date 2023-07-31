using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Robot.Data.ML.DeptFirst;
using Robot.Service.Api;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Robot.Controllers.DeptFirst {
    [Route("api/[controller]")]
    [ApiController]
    public class DeptfirstController : ControllerBase {
        ClientService _clientService;
      
        public DeptfirstController(ClientService clientService ) {
            _clientService = clientService; 
        }


        [HttpGet("dpm018")]
     async   public Task<ActionResult> dpm018() {
            DPM018DataSet.Root r = new DPM018DataSet.Root();
            try {
                string url = @"http://portal.disaster.go.th/portal/wsjson?queryCode=DPM018&user=xws-0068&password=364182df";

                #region http client
                HttpClient _httpClient;
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => {
                    return true;
                };
                _httpClient = new HttpClient((httpClientHandler));
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                #endregion


                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                if (response.StatusCode.ToString() != "OK") {
                    return BadRequest(response);
                } else {
                    string json_back = await response.Content.ReadAsStringAsync();
                    r = JsonConvert.DeserializeObject<DPM018DataSet.Root>(json_back);

                }
           
                 
            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

         



    }
}
