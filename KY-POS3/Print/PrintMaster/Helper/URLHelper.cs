using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintMaster.Helper {
    public class URLHelper {
        public static string GetBaseUrl() {
            string baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            return baseUrl;
        }
        public static string GetBaseUrlNoAppPath() {
            string baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
            return baseUrl;
        }

    }
}