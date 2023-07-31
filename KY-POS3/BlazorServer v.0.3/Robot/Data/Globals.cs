using Blazored.SessionStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using static Robot.Data.DA.LogInService;

namespace Robot
{

    public static class Globals
    {
        internal static IServiceProvider _serviceProvider = null;
        static LoginSet loginSet = null;
        public static string GAEntitiesConn { get; set; }
        public static string FirebaseUrl { get; set; }
        public static string FirebaseWebToken { get; set; }
  public static string ApiPrintMasterBaseUrl { get; set; }
        public static string AppID { get; set; }
        public static string BlazorServer_Front { get; set; }

        #region name for local storage 
        public static string AuthToken = "authToken";
        public static string AuthUsername = "authusername";
        public static string AuthFullname = "authfullname";
        public static string AuthLoginSet = "authloginset";
        public static string RefreshToken = "refreshToken";
        public static string UserImageURL = "userImageURL";

        public static string RememberUserLogin = "x";
        public static string RememberPasswordLogin = "y";
        #endregion






        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
        {
            var provider = serviceProvider ?? _serviceProvider;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }

        public static async void getDataSession()
        {
            using (var serviceScope = GetScope())
            {
                IConfiguration ses = serviceScope.ServiceProvider.GetService<IConfiguration>();
                var xx = ses["GAEntities:ConnectionString"];

                //ProtectedSessionStorage ii = serviceScope.ServiceProvider.GetService<ProtectedSessionStorage>();
                //var yy = await ii.GetAsync<string>("Username");
                //string uu = yy.Success ? yy.Value : "";
                StateContainer st = serviceScope.ServiceProvider.GetService<StateContainer>();
                var ddd = st.loginSet;
            }
        }

        public static void SetSession(string strval,string strData)
        {
            //HttpContext.Session.SetString(strval, strData);

        }


    }
}
