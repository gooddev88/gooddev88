using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.GaDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using System.Net;
using System.Text;

namespace RobotWasm.Server.Data.DA.Line {
    public static class LineNotify {
        #region Line Massaging API
        public static I_BasicResult SendLineMsgAPI(string msg, string lineId) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                LineGroupInfo gInfo;
                using (GAEntities db = new GAEntities()) {
                    gInfo = db.LineGroupInfo.Where(o => o.RComID == "NDW" && o.LineGroupID == "ADMIN").FirstOrDefault();
                }


                string responseTextJson = "";
                var replyMessage = string.Empty;
                string jsonresult = "";

                jsonresult += "{\"type\":\"text\",\"text\":\"" + msg + "\"}";
                responseTextJson = "{\"to\":\"" + lineId +
                       "\",\"messages\":[" + jsonresult + "]}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(gInfo.GroupRef2);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + gInfo.LineTokenID);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                    streamWriter.Write(responseTextJson);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    var result = streamReader.ReadToEnd();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {

                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        public static List<string> _messageHandler(string message) {
            string returnMessage;
            var listMessage = new List<string>();
            message = message.ToLower();

            if (message.Contains("hello")) {
                returnMessage = "Hi สวัสดีน้อง.";
                listMessage.Add(returnMessage);
            }
            return listMessage;
        }
        #endregion
        #region Line Group Notify

        public static I_BasicResult SendLineGroupMsgOnly(string rcom, string message, string groupId = "OMS-ORDER") {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                LineGroupInfo gInfo;
                using (GAEntities db = new GAEntities()) {
                    gInfo = db.LineGroupInfo.Where(o => o.RComID == rcom && o.LineGroupID == groupId).FirstOrDefault();
                }
                if (gInfo==null) {
                    r.Result = "fail";
                    r.Message1 = "No line token found.";
                    return r;
                }

                var request = (HttpWebRequest)WebRequest.Create(gInfo.GroupRef2);
                var postData = string.Format("message={0}", message);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + gInfo.LineTokenID);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {

                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        public static I_BasicResult SendLineGroupWithImg(string message, byte[] image, string groupId = "line_group_notify") {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                LineGroupInfo gInfo;
                using (GAEntities db = new GAEntities()) {
                    gInfo = db.LineGroupInfo.Where(o => o.RComID == "PRV" && o.LineGroupID == "ADMIN").FirstOrDefault();
                }

                using (var client = new System.Net.Http.HttpClient()) {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", gInfo.LineTokenID);
                    using (var form = new MultipartFormDataContent()) {
                        string filename = Guid.NewGuid().ToString() + ".png";
                        //message

                        form.Add(new StringContent(message), "message");
                        //images
                        if (image != null) {
                            form.Add(new ByteArrayContent(image), "imageFile", filename);
                        }

                        //send form
                        var response = client.PostAsync(gInfo.GroupRef2, form).Result;
                        var contents = response.Content.ReadAsStringAsync().Result;
                        //Console.WriteLine("Result :{0}", contents);
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {

                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;

        }
        #endregion
        #region line group ของเก่า
        public static void LineNotifyOldVersion(string msg, string token) {

            try {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            } catch (Exception ex) {
                //Console.WriteLine(ex.ToString());
            }
        }
        #endregion
    }
}
