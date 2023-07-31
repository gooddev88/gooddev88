using Dapper;
using Npgsql;
using RobotAPI.Data.CimsDB.TT;
using System.Data;

namespace RobotAPI.Data.DA.Front {
    public class ApiStoreService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }


        public static List<api_store> ListAllApi() {
            List<api_store> result = new List<api_store>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();                   
                    sql = @"
                            select * from api_store where is_active='1'
                    ";
                    var dynamicParameters = new DynamicParameters();
                   
                    result = conn.Query<api_store>(sql).ToList(); 
                }


            } catch (Exception) {

                throw;
            }

            return result;

        }
    }
}
