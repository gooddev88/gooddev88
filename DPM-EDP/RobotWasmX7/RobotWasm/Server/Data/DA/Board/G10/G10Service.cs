using Npgsql;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData.BoardParam;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget;
using System.Data;
using System.Globalization;

namespace RobotWasm.Server.Data.DA.Board.G10 {
    public class G10Service {
        public static IDbConnection Connection {
            get {
                return new NpgsqlConnection((Globals.CimsConn));
            }
        }
        public static List<WG101Data> GetWG101(string board_id) {
            List<WG101Data> output = new List<WG101Data>();
         var param = GetParam(board_id);
            return output;

        }
    
          public static ParamG10 GetParam(string board_id) {
            ParamG10 param = new ParamG10();
            try { 
            var get_param = CustomBoardService.GetWidgetParam(board_id, "wg101");
            if (get_param != null) {
                    var d_begin = get_param.Where(o => o.param_id == "date_begin").FirstOrDefault();
                    var d_end = get_param.Where(o => o.param_id == "date_end").FirstOrDefault();
                    param.DateBegin = DateTime.ParseExact(d_begin.data, "yyyyMMdd", CultureInfo.InvariantCulture);
                    param.DateEnd = DateTime.ParseExact(d_end.data, "yyyyMMdd", CultureInfo.InvariantCulture);
                }

            } catch (Exception ex) {

            }
        return param;
        }
    }
}
