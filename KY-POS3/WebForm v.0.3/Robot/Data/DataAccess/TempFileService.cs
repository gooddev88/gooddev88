using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Robot.Data.DataAccess {
    public class TempFileService {
        async public static Task<TempFilePrinter> CreateTempFile(string fileurl) {
            
                   TempFilePrinter t = new TempFilePrinter();  
            try {
        
                t.FileUrl = fileurl;
              
                 
                var link = LinkService.GetLinkByLinkName("apibase_url");
                string method = $"/api/tempfile/CreateTempFile";
                var url = link + method;
                var client = new HttpClient();
                string sContentType = "application/json";
                var json_in = JsonConvert.SerializeObject(t);
                var content = new StringContent(json_in, Encoding.UTF8, sContentType);
                var data_back = await client.PostAsync(url, content);
             

                string json_back = await data_back.Content.ReadAsStringAsync();
                t = JsonConvert.DeserializeObject<TempFilePrinter>(json_back);
            } catch (Exception ex) {
                var aa = ex.Message;
                throw;
            }
            return t;
        }
    }
}