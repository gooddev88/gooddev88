using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Robot.Service.FileGo;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Robot.Controllers.Today {
    [Route("api/[controller]")]
    [ApiController]
    public class TodayNewsController : ControllerBase {

        [AllowAnonymous]
        [HttpGet("GetNewsCateUrlFromFileGo/{cate}")]
      async  public Task<string> GetNewsCateUrlFromFileGo(string cate) {
            string rcom = "DPM";
            var base_url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}"; 
            string url;
          
            //Uri baseUri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, String.Empty));
            var refx = FileGo.GetFileId(rcom, "", FileGo.Type_NewsCate, cate);
            if (refx==null) {
                url = base_url+"/assets/img/dpmlogo_circle.png";
            } else {
                url = refx.RootUrlPublic + @"/api/xfiles/XFilesService/GetFile/" + refx.FileID;
            }
            


            //#region http client
            //HttpClient _httpClient;
            //var httpClientHandler = new HttpClientHandler();
            //httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => {
            //    return true;
            //};
            //_httpClient = new HttpClient((httpClientHandler));
            //_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            //#endregion

            //var response = await _httpClient.GetAsync(url);
            ////response.EnsureSuccessStatusCode();



            return url;
        }




        //async Task Fuck() {

        //    try {
        

        //        #region http client
        //        HttpClient _httpClient;
        //        var httpClientHandler = new HttpClientHandler();
        //        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => {
        //            return true;
        //        };
        //        _httpClient = new HttpClient((httpClientHandler));
        //        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        //        #endregion
                 
        //        var response = await _httpClient.GetAsync(url);
        //        response.EnsureSuccessStatusCode();

        //        //if (response.StatusCode.ToString() != "OK") {

        //        //} else {
        //        //    string json_back = await response.Content.ReadAsStringAsync();
        //        //    ImageUrlx = JsonConvert.DeserializeObject<string>(json_back);

        //        //}


        //    } catch (Exception ex) {
        //        string xx = ex.Message;
        //    }

        //}
    }
}
