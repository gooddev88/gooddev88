using Npgsql;
using System.Data;

namespace ApiGateWay.Data.DA {
    public class ApiService {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection(Globals.CimsConn);
            }
        }
    }
}
