using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.DpmQDB.TT;

namespace RobotAPI.Data.DA.Tableau {
    public class TrustedAuth {
        public static async Task<string> requestTicket(string sso, string server, string? site = "") {
            try { 
                //Assign parameters and values
                var values = new List<KeyValuePair<string, string>>();
                values.Add(new KeyValuePair<string, string>("username", sso));
                if (!string.IsNullOrEmpty(site)) {
                    values.Add(new KeyValuePair<string, string>("target_site", site));
                }


                //Web Application is HTTP and Tableau is HTTPS, there are certification issues. I need to fake the certs out and return them as true.
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                //Instantiate HttpClient class
                var client = new HttpClient();

                //Encode Content
                var req = new HttpRequestMessage(HttpMethod.Post, server) { Content = new FormUrlEncodedContent(values) };

                //POST request
                var res = await client.SendAsync(req);

                //Get response value
                var responseString = await res.Content.ReadAsStringAsync();

                return responseString;

            } catch (Exception e) {
                //System.IO.File.AppendAllText(@"c:\inetpub\wwwroot\WebApplication\TrustedAuthError.txt", ":::ERROR::: " + System.DateTime.Today.ToString() + ":::" + e.ToString() + Environment.NewLine);
                //Add Log4Net logging
            }

            return "-1";

        }

        public static string addTicket(string ticket, string reportLink) {
            //Add ticket parameter with ticket value. I'm using </object> as my keyword to find and replace
            string addedTicket = reportLink.Replace("</object>", "<param name='ticket' value='" + ticket + "' /></object>");

            return addedTicket;
        }

        #region tammon custom
     async   public  static  Task<string> GenToken() {
            string token = "-1";
            using (DPMQContext db=new DPMQContext()) {
                var q = db.tableau_authen.Where(o => o.authen_id.ToLower() == "dpm_prod").FirstOrDefault();
                if (q==null) {
                    return "-1";
                }
                token = await  Task.Run(()=> requestTicket(q.username, q.api_url, ""));
            } 
            return token;
        }
        async public static Task<string> GetBoardUrl(string board_id) {
            string boar_url = "";
            board_id = board_id.ToLower();
            string token =await Task.Run(() => GenToken());
            using (DPMQContext db = new DPMQContext()) {
                var q = db.tableau_board.Where(o => o.board_id.ToLower() == board_id).FirstOrDefault();
                if (q == null) {
                    return "";
                }
                boar_url = q.baseurl + @"/" + token + q.board_suburl;
            }
            return boar_url;
        }
        async public static Task<string> GenTokenForEdp() {
            string token = "-1";
            using (DPMQContext db = new DPMQContext()) {
                var q = db.tableau_authen.Where(o => o.authen_id.ToLower() == "dpm_prod").FirstOrDefault();
                if (q == null) {
                    return "-1";
                }
                token = await Task.Run(() => requestTicket(q.username, q.api_url, ""));
            }
            return token;
        }
        async public static Task<string> GetBoardUrlForEdp(string board_id) {
            string boar_url = "";
            board_id = board_id.ToLower();
            string token =await Task.Run(() => GenTokenForEdp());
            using (CIMSContext db = new CIMSContext()) {
                var board_info = db.board_master.Where(o => o.board_id.ToLower() == board_id).FirstOrDefault();
                var authen = db.board_authen.Where(o => o.authen_id == board_info.authen_id).FirstOrDefault();

              
                boar_url = authen.base_url + @"/" + token + board_info.board_url;
            }
            return boar_url;
        }

        #endregion

    }
}
