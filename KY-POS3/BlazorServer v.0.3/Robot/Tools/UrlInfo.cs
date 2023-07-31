using Microsoft.AspNetCore.Components;
namespace Robot.Tools {
    public class UrlInfo {
        NavigationManager _nav;
        public UrlInfo(NavigationManager nav) {
            _nav = nav;

        }
        public string GetRootApp(string pageurl) {
            string rootdomain = _nav.BaseUri.ToUpper().ToString().Replace("SALE/", "");
            string subApp = "APP";
            string url = rootdomain + subApp + pageurl;
            return url;
        }
    }
}
