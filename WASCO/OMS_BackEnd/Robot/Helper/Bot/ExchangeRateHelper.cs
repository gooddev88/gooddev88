using Robot.Data.GADB.TT;
using Robot.Data.ML.Bot;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Robot.Helper.Bot {
    public class ExchangeRateHelper {

        public static async Task<decimal> GetCurrency(string currency, string sellorbuy, DateTime mydate) {
          decimal rate = 0;
            int k = 0;
            while (rate==0) {
                rate = await Task.Run(() => GetCurrencyFromApi(currency, sellorbuy, mydate.AddDays(k*-1)));
                k++;
                if (k>10) {
                    break;
                }
            }
            return rate;
            
        }


            public static async Task<decimal> GetCurrencyFromApi (string currency,string sellorbuy,DateTime mydate) {
 
            try {
                Link api_info = new Link();
                using (GAEntities db = new GAEntities()) {
                      api_info = db.Link.Where(o => o.LinkName == "bot_exchange_rate").FirstOrDefault();
                    if (api_info==null) {
                        return -999;
                    }
                }
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => {
                    return true;
                };
                var client = new HttpClient((httpClientHandler));
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("X-IBM-Client-Id", api_info.Token);
                string date = mydate.ToString("yyyy-MM-dd");
                string url = api_info.AppLink + $"?start_period={date}&end_period={date}&currency={currency.ToUpper()}";
                  
                var response = await client.GetAsync(url);
 
                if (response.StatusCode.ToString() == "Unauthorized") {
                    return -888; 
                } else {
                    response.EnsureSuccessStatusCode();
                    var output_str = await response.Content.ReadAsStringAsync ();
                    var output = await response.Content.ReadFromJsonAsync<CurrentExchange.Root>();
                    if (sellorbuy=="sell") {
                        if (output.result.data.data_detail.FirstOrDefault().selling=="") {
                            return 0;
                        } else {
                            var rate = Convert.ToDecimal(output.result.data.data_detail.FirstOrDefault().buying_transfer);
                            decimal c_rate = rate;
                            if (currency.ToUpper()=="JPY") {
                                c_rate = rate / 100;
                            }
                            return c_rate;
                        }
                    } else {
                        if (output.result.data.data_detail.FirstOrDefault().selling == "") {
                            return 0;
                        } else {
                            var rate = Convert.ToDecimal(output.result.data.data_detail.FirstOrDefault().selling);
                            decimal c_rate = rate;
                            if (currency.ToUpper() == "JPY") {
                                c_rate = rate / 100;
                            }
                            return c_rate;
                        }
                    } 
                }
            } catch (Exception ex) {
                var httpResMsg = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                return -777;
            }
        
        }
        

    }
}
