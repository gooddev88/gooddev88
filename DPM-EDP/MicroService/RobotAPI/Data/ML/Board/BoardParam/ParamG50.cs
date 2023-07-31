using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Cims.Board;
using System.Globalization;

namespace RobotAPI.Data.ML.Board.BoardParam {
    public class ParamG50 {
        // 7 ภัย
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }


        #region  param meter management
        public static ParamG50 GetParamBoard(string board_id, string widget_id) {
            ParamG50 param = new ParamG50();
            try {
                var get_param = CustomBoardService.GetWidgetParam(board_id, widget_id);
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
        public static ParamG50 GetParamQString(string datebegin, string dateend) {
            ParamG50 param = new ParamG50();
            try {
                var get_param = new custom_widget_param_in_user();
                if (get_param != null) {
                    if (!string.IsNullOrEmpty(datebegin)) {
                        param.DateBegin = DateTime.ParseExact(datebegin, "yyyyMMdd", CultureInfo.InvariantCulture);
                    } else {
                        param.DateBegin = new DateTime(DateTime.Now.Year, 1, 1);
                    }
                    if (!string.IsNullOrEmpty(dateend)) {
                        param.DateEnd = DateTime.ParseExact(dateend, "yyyyMMdd", CultureInfo.InvariantCulture);
                    } else {
                        param.DateEnd = new DateTime(DateTime.Now.Year, 12, 31);
                    } 
                }
            } catch (Exception ex) {

            }
            return param;
        }
        #endregion

    }
}
