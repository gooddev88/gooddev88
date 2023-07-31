using Dapper;
using Newtonsoft.Json;
using PrintMaster.Data.PrintDB; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
 

namespace PrintMaster.Service.Dapper {
    public class DBDapperService {
        //R401 Print bill 
        public static string GetDBConnectFromAppConfig() {
            var strCon = ConfigurationManager.ConnectionStrings["PrintDapperConnString"].ConnectionString;
            return strCon; 
        }

        //public static string GetSQLCommand(string queryId) {
        //    try {
        //        string conStr = GetDBConnectFromAppConfig();
        //        using (var conn = new SqlConnection(conStr)) {
        //            var dynamicParameters = new DynamicParameters();
        //            dynamicParameters.Add("QueryID", queryId);
        //            string strSQL = "select * FROM V_Query where QueryID = @QueryID ";
        //            VQuery queryCommand = conn.Query<VQuery>(strSQL, dynamicParameters).FirstOrDefault();
        //            return queryCommand.Command;
        //        }
        //    } catch (Exception e) {
        //        return e.Message;
        //    }
        //}

    }
}