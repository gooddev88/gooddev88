using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Robot.Data.ServiceHelper {
    public   class WebServiceHelper {
        
            public   string Post(Uri url, string value) {
                var request = HttpWebRequest.Create(url);
            Encoding encoding = new UTF8Encoding();
            var byteData = encoding.GetBytes(value);
            //var byteData = Encoding.ASCII.GetBytes(value);
            request.ContentType = "application/json";
            request.Headers.Add("Content-Encoding", "utf-8");
            request.Method = "POST";

                try {
                    using (var stream = request.GetRequestStream()) {
                        stream.Write(byteData, 0, byteData.Length);
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    return responseString;
                } catch (WebException e) {
                    return null;
                }
            }
        public string Delete(Uri url, string value) {
            var request = HttpWebRequest.Create(url);
            Encoding encoding = new UTF8Encoding();
            var byteData = encoding.GetBytes(value);
            //var byteData = Encoding.ASCII.GetBytes(value);
            request.ContentType = "application/json";
            request.Headers.Add("Content-Encoding", "utf-8");
            request.Method = "DELETE";

            try {
                using (var stream = request.GetRequestStream()) {
                    stream.Write(byteData, 0, byteData.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return responseString;
            } catch (WebException e) {
                return null;
            }
        }

        public   string Get(Uri url) {
                var request = HttpWebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = "GET";

                try {
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    return responseString;
                } catch (WebException e) {
                    return null;
                }
            }
      
    }
}