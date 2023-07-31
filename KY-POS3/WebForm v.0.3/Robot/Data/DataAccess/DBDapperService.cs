using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess {
    public class DBDapperService {
        public class VQuery {
            public int ID { get; set; }
            public string QueryID { get; set; }
            public string Command { get; set; }
            public string IsActive { get; set; }
        }
        public static string CreateDBConnect2FileDB() {
            string conStr = "";
            using (GAEntities db = new GAEntities()) {
                var c = db.XFileLocation.Where(o => o.IsCurrent && o.IsActive).FirstOrDefault();
                conStr = $"data source={c.DBServer};initial catalog={c.DBName};persist security info=True;user id={c.LoginName};password={c.LoginPass};MultipleActiveResultSets=True";
            }
            return conStr;
        }

        public static string CreateDBConnectFromAppConfig() {

            string dbserver = ConfigurationManager.AppSettings["con_db_server"].ToString();
            string dbname = ConfigurationManager.AppSettings["con_db_database"].ToString();
            string dbuser = ConfigurationManager.AppSettings["con_db_username"].ToString();
            string dbpass = ConfigurationManager.AppSettings["con_db_password"].ToString();
            return $"data source={dbserver};initial catalog={dbname};persist security info=True;user id={dbuser};password={dbpass};MultipleActiveResultSets=True";
        }
        public static string GetDBConnectFromAppConfig() {
            var strCon =  ConfigurationManager.ConnectionStrings["GAConnectionString"].ConnectionString;
            return strCon;

        }
        public static string GetSQLCommand(string queryId) {
            try {
                string conStr = GetDBConnectFromAppConfig();
                using (var conn = new SqlConnection(conStr)) {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("QueryID", queryId);
                    string strSQL = "select * FROM V_Query where QueryID = @QueryID ";
                    VQuery queryCommand = conn.Query<VQuery>(strSQL, dynamicParameters).FirstOrDefault();
                    return queryCommand.Command;
                }
            } catch (Exception e) {
                return e.Message;
            }
        }

        public static void SampleExecuteComman() {
            try {
                string conStr = GetDBConnectFromAppConfig();
                using (var connection = new SqlConnection(conStr)) {
                    var dynamicParameters = new DynamicParameters();
                    string sqlCmd = GetSQLCommand("SP_VoteUpdateScore");
                    int countParam = sqlCmd.Split('@').Length - 1;
                    if (countParam > 0) {
                        for (int i = 0; i < countParam; i++) {
                            dynamicParameters.Add("para" + i.ToString(), "TEST");
                        }
                    }
                    dynamic result = connection.Execute(sqlCmd, dynamicParameters);

                }
            } catch (Exception e) {
                var x = e.Message;
            }
        }
        #region  Sample Query
        public static List<V_Query> SampleQuerySimpleQuery(string queryId) {
            List<V_Query> query_R = new List<V_Query>();
            try {
                string conStr = GetDBConnectFromAppConfig();
                using (var conn = new SqlConnection(conStr)) {
                    string strSQL = GetSQLCommand("TESTQ");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("ID", queryId);

                    query_R = conn.Query<V_Query>(strSQL, dynamicParameters).ToList();

                }
            } catch (Exception e) {

            }
            return query_R;
        }
        public static void SampleQueryMultiParam() {
            try {
                string conStr = CreateDBConnectFromAppConfig();
                using (var connection = new SqlConnection(conStr)) {
                    var dynamicParameters = new DynamicParameters();
                    string sqlCmd = GetSQLCommand("TESTQ");
                    int countParam = sqlCmd.Split('@').Length - 1;
                    if (countParam > 0) {
                        for (int i = 0; i < countParam; i++) {
                            dynamicParameters.Add("para" + i.ToString(), "TEST");
                        }
                    }
                    dynamic result = connection.Query<dynamic>(sqlCmd, dynamicParameters).FirstOrDefault();
                    Console.WriteLine(result.Id);
                }
            } catch (Exception e) {

            }
        }
        #endregion

    }
}