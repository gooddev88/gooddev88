using Robot.Communication.DA;
using Robot.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Communication.API.Line {
    public static class LineMsgAPI {
        #region Line Massaging API
        public static I_BasicResult SendLineMsgAPI(string msg, string lineId, string appId) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var s = CommConfigService.GetSenderInfo("line_msg_api", appId);

                string responseTextJson = "";
                var replyMessage = string.Empty;
                string jsonresult = "";

                jsonresult += "{\"type\":\"text\",\"text\":\"" + msg + "\"}";
                responseTextJson = "{\"to\":\"" + lineId +
                       "\",\"messages\":[" + jsonresult + "]}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(s.ApiUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + s.Token);
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


        public static I_BasicResult SendLineMsgAPIByPO(string msg, string appId)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                string lineId = "";

                var s = CommConfigService.GetSenderInfo("line_msg_api", appId);

                string responseTextJson = "";
                var replyMessage = string.Empty;
                string jsonresult = "";

                jsonresult += "{\"type\":\"text\",\"text\":\"" + msg + "\"}";
                responseTextJson = "{\"to\":\"" + lineId +
                       "\",\"messages\":[" + jsonresult + "]}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(s.ApiUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + s.Token);
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(responseTextJson);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                r.Result = "fail";
                if (ex.InnerException != null)
                {

                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        public static I_BasicResult SendLineMsgAPIByUser(string msg,string username , string appId) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                string lineId = "";
                using (GAEntities db =new GAEntities()) {
                    var lineLogin = db.LineLogIn.Where(o => o.UserID == username && o.IsActive).FirstOrDefault();
                    if (lineLogin != null) {
                        lineId = lineLogin.LineID;
                    }
                }
                if (lineId=="") {
                    r.Result = "fail";
                    r.Message1 = "ไม่พบการลงทะเบียน Line";
                    return r;
                }
                var s = CommConfigService.GetSenderInfo("line_msg_api", appId);

                string responseTextJson = "";
                var replyMessage = string.Empty;
                string jsonresult = "";

                jsonresult += "{\"type\":\"text\",\"text\":\"" + msg + "\"}";
                responseTextJson = "{\"to\":\"" + lineId +
                       "\",\"messages\":[" + jsonresult + "]}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(s.ApiUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Bearer " + s.Token);
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
        public static I_BasicResult SendLineGroupMsgOnly(string message, string appId) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var s = CommConfigService.GetSenderInfo("line_group_notify", appId);


                var request = (HttpWebRequest)WebRequest.Create(s.ApiUrl);
                var postData = string.Format("message={0}", message);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + s.Token);

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
        public static I_BasicResult SendLineGroupWithImg(string message, byte[] image) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var s = CommConfigService.GetSenderInfo("line_group_notify", "BS");
                if (s == null) {
                    r.Result = "fail";
                    r.Message1 = "no config line_group_notify";

                    return r;
                }
                using (var client = new System.Net.Http.HttpClient()) {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", s.Token);
                    using (var form = new MultipartFormDataContent()) {
                        string filename = Guid.NewGuid().ToString() + ".png";
                        //message

                        form.Add(new StringContent(message), "message");
                        //images
                        if (image != null) {
                            form.Add(new ByteArrayContent(image), "imageFile", filename);
                        }

                        //send form
                        var response = client.PostAsync(s.ApiUrl, form).Result;
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



        #region Message Handler
        public static string MessageHelper(string inputMsg) {

            string returnMessage;
            returnMessage = inputMsg.Replace("\r\n", "\\n").Replace("\n", "\\n");
            return returnMessage;
        }

        #endregion
    }
}