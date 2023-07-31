using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class FireBaseService {



        //public I_BasicResult Sendnotify(DAlertHead h) {
        //    I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
        //    try {


        //        List<ClientId> Client = getClientId(h.AlertID);
        //        Sendnotify(h, Client);
        //    } catch (Exception ex) {
        //        r.Result = "fail";
        //        if (ex.InnerException != null) {
        //            r.Message1 = ex.InnerException.ToString();
        //        } else {
        //            r.Message1 = ex.Message;
        //        }

        //    }

        //    return r;
        //}

        

        public static void sendfirenotify(string jsonMsg) {
            try {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Globals.FirebaseUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Globals.FirebaseWebToken);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                    streamWriter.Write(jsonMsg);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    var result = streamReader.ReadToEnd();
                }

            } catch (Exception ex) {
                throw;
            }
        }

        public string ValidateToken(string token) {
            string statuscode = "";

            try {
                string url = @"https://iid.googleapis.com/iid/info/" + token;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Globals.FirebaseWebToken);

                //using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                //    streamWriter.Write(jsonMsg);
                //}
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    var result = streamReader.ReadToEnd();
                }
                statuscode = httpResponse.StatusCode.ToString();
                if (statuscode.ToLower() == "ok") {
                    statuscode = "ok";
                } else {
                    statuscode = "fail";
                }

            } catch (Exception ex) {
                statuscode = "fail";
            }
            return statuscode;
        }
        public static List<ClientInfo> GetClient(string rcom,string comId) {
            List<ClientInfo> result = new List<ClientInfo>();
            var limitDate = DateTime.Now.Date.AddDays(-2);
            using (GAEntities db=new GAEntities()) {
                var cleint = db.MacRegister.Where(o => o.RComID == rcom && o.ComID == comId 
                                    && o.LastLoginDate>=limitDate
                                    && o.IsActive && !string.IsNullOrEmpty( o.NotificationToken)).ToList();
                foreach (var c in cleint) {
                    result.Add(new ClientInfo { OS = "Andoird", Token= c.NotificationToken });
                }
            }
            return result;
        }
        public static I_BasicResult Sendnotify(string rcom, string comId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var Client = GetClient(rcom, comId);
                double deci_round = Math.Ceiling((double)Client.Count / 1000);
                int int_round = Convert.ToInt32(Math.Ceiling(deci_round));
                int countClient = Client.Count;
                string id = "";
                int index = 0;
                for (int i = 0; i < int_round; i++) {
                    for (int j = 0; j < 1000; j++) {
                        if (countClient > index) {
                            if (j == 0) {
                                id = "\"" + Client[index].Token.Trim() + "\"";
                            } else {
                                id = id + ",\"" + Client[index].Token.Trim() + "\"";
                            }
                        } else {
                            continue;
                        }
                        index++;
                    }
                    string registration_ids = "\"registration_ids\":[" + id + "],";
                    string msg = "\"" + "notification" + "\":{";
                    msg = msg + "\"" + "body" + "\":" + "\"" + "กำลังอัพเดทรายการขาย" + "\",";
                    msg = msg + "\"" + "title" + "\":" + "\"" + "มีรายการขายมาใหม่" + "\",";
                    msg = msg + "\"" + "priority" + "\":" + "\"" + "high" + "\",";
                    msg = msg + "\"" + "sound" + "\":" + "\"" + "default" + "\",";
                    msg = msg + "\"" + "badge" + "\":" + "\"" + "0" + "\",";
                    msg = msg + "\"" + "color" + "\":" + "\"" + "#CD4275" + "\"";
                    msg = msg + "},";

                 string data = "\"" + "data" + "\":{";
                    data = data + "\"" + "command" + "\":" + "\"" + "refresh_data" + "\"";
                    data = data + "}";


                    string jsonMsg = "{" + registration_ids +   msg + data+ "}";
                 //   jsonMsg = jsonMsg + data;






                    sendfirenotify(jsonMsg);
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException == null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }

            }
            return result;

        }
        //    List<ClientId> getClientId(string AlertId, string strOS) {
        //        List<ClientId> result = new List<ClientId>();
        //        try {
        //            string conStr = Globals.GAEntitiesConn;
        //            using (var connection = new SqlConnection(conStr)) {
        //                var dynamicParameters = new DynamicParameters();
        //                string sqlCmd = VQueryService.GetCommand("SP_GetFCMTokenByAlertID");
        //                int countParam = 1;
        //                if (countParam > 0) {
        //                    for (int i = 0; i < countParam; i++) {
        //                        dynamicParameters.Add("alertid", AlertId);
        //                    }
        //                }
        //                result = connection.Query<ClientId>(sqlCmd, dynamicParameters).ToList();
        //                result = result.Where(x => x.OS == strOS).ToList();
        //            }
        //        } catch (Exception e) {

        //        }
        //        return result;
        //    }
        //    List<ClientId> getClientId(string AlertId) {
        //        List<ClientId> result = new List<ClientId>();
        //        try {
        //            string conStr = Globals.GAEntitiesConn;
        //            using (var connection = new SqlConnection(conStr)) {
        //                var dynamicParameters = new DynamicParameters();
        //                string sqlCmd = VQueryService.GetCommand("SP_GetFCMTokenByAlertID");
        //                int countParam = 1;
        //                if (countParam > 0) {
        //                    for (int i = 0; i < countParam; i++) {
        //                        dynamicParameters.Add("alertid", AlertId);
        //                    }
        //                }
        //                result = connection.Query<ClientId>(sqlCmd, dynamicParameters).ToList();

        //            }
        //        } catch (Exception e) {

        //        }
        //        return result;
        //    }

        //}

        public class ClientInfo {
            public string OS { get; set; }
            public string Token { get; set; }
        }
    }
}
