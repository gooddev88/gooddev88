using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data;
using RobotAPI.Data.DA.FileGo;

namespace RobotAPI.Controllers.Today {
    [Route("api/[controller]")]
    [ApiController]
    public class TodayNewsController : ControllerBase {
        [AllowAnonymous]
        [HttpGet("GetNewsCateUrlFromFileGo/{cate}")]
        async public Task<string> GetNewsCateUrlFromFileGo(string cate) {
            string rcom = "DPM";
            var base_url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string url;

            //Uri baseUri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, String.Empty));
            var refx = FileGo.GetFileId(rcom, "", Globals.Type_NewsCate, cate);
            if (refx == null) {
                url = base_url + "/assets/img/dpmlogo_circle.png";
            } else {
                url = base_url + @"/api/xfiles/XFilesService/GetFile/" + refx.file_id;
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
    }
}
