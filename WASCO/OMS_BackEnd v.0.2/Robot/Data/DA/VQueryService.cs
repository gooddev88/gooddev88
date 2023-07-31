using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Data.DA
{
    public class VQueryService
    {
        public static string GetCommand(string ReportId)
        {
            try
            {
                using (var connection = new SqlConnection(Globals.GAEntitiesConn))
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("Id", ReportId);
                    string strSQL = "SELECT * FROM V_Query where QueryId = @Id ";
                    dynamic result = connection.Query<dynamic>(strSQL, dynamicParameters).FirstOrDefault();
                    return result.Command;
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
