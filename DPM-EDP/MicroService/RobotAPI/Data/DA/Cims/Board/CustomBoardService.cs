using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DataStoreDB.TT;

namespace RobotAPI.Data.DA.Cims.Board {
    public class CustomBoardService {



        #region param
        public static List<custom_widget_param_in_user> GetWidgetParam(string board_id, string widget_id) {
            List<custom_widget_param_in_user> output = new List<custom_widget_param_in_user>();
            try {
                using (CIMSContext db = new CIMSContext()) {
                    output = db.custom_widget_param_in_user.Where(o => o.board_id == board_id && o.widget_id == widget_id && o.is_active == 1).ToList();
                }
            } catch (Exception ex) {

            }
            return output;
        }


        public static List<string> ListProvince (string field) {
            List<string> output = new List<string>();
            try {
                using (CIMSContext db = new CIMSContext()) {
                    if (field=="name") {
                        output = db.a_province.OrderBy(o => o.pname).Select(o => o.pname).ToList();
                    } 
                }
            } catch (Exception ex) { 
            }
            return output;
        }
        #endregion
    }
}
