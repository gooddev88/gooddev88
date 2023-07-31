using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;

namespace RobotAPI.Data.DA.Cims {
    public class ApiConnService {
         
        public static vw_api_master_conn? GetApiInfo(string api_code) {
            vw_api_master_conn? refx = new vw_api_master_conn();
            using (CIMSContext db = new CIMSContext()) {
                refx = db.vw_api_master_conn.Where(o=>o.api_id.ToLower()== api_code.ToLower()).FirstOrDefault();
            }
            return refx;
        }
    }
}
