using Dapper;
using Npgsql;
using System.Data;

namespace RobotAPI.Data.DA.Cims.Master {
    public class MasterService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }
        public static List<string> GetProvince(string search) {
            List<string>  result =new List<string>();
            try {
                using (IDbConnection conn = Connection) {
                    string sql = "";

                    conn.Open();
                    //sql = @"
                    //        select province_id from mas_province where province_id like '%@search%' order by province_id
                    //";
                    sql = @"
                            select province_id from mas_province   order by province_id
                    ";
                    var dynamicParameters = new DynamicParameters();
                    //   dynamicParameters.Add("search", search);
                    // result = conn.Query<string>(sql, dynamicParameters).ToArray();
                    result = conn.Query<string>(sql).ToList();
                    //result = result.ToArray();

                }


            } catch (Exception) {

                throw;
            }

            return result;

        }
    }
}
