using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Cims.Board;
using System.Globalization;

namespace RobotAPI.Data.ML.Board.BoardParam {
    public class ParamG10 {
        //บอร์ดอุบัติเหตุ
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string FilterOption { get; set; }
        public string  Provinces { get; set; }


        #region  param meter management
        public static ParamG10 GetParamBoard(string board_id, string widget_id) {
            ParamG10 param = new ParamG10();
            try {
                var get_param = CustomBoardService.GetWidgetParam(board_id, widget_id);
                if (get_param != null) {
                    var d_begin = get_param.Where(o => o.param_id == "date_begin").FirstOrDefault();
                    var d_end = get_param.Where(o => o.param_id == "date_end").FirstOrDefault();
                    var filter_option = get_param.Where(o => o.param_id == "filter_option").FirstOrDefault();
                    param.DateBegin = DateTime.ParseExact(d_begin.data, "yyyyMMdd", CultureInfo.InvariantCulture);
                    param.DateEnd = DateTime.ParseExact(d_end.data, "yyyyMMdd", CultureInfo.InvariantCulture);
                    param.FilterOption = filter_option == null ? "" : filter_option.data;

                    #region create province filter
                    var pp = get_param.Where(o => o.param_id == "provinces").FirstOrDefault();
                    param.Provinces = "";
                    if (pp != null) {
                        param.Provinces = pp.data;
                    }
                    if (string.IsNullOrEmpty(param.Provinces)) {
                        var pnames = CustomBoardService.ListProvince("name");
                        int i = 1;
                        foreach (var p in pnames) {
                            if (i == 1) {
                                param.Provinces = p;
                            } else {
                                param.Provinces = param.Provinces + "," + p;
                            }
                            i++;
                        }
                    }
                    #endregion


                }

            } catch (Exception ex) {

            }
            return param;
        }
        public static ParamG10 GetParamQString(string datebegin, string dateend, string province) {
            ParamG10 param = new ParamG10();
            try {
                var get_param = new custom_widget_param_in_user();
                if (get_param != null) {
                    param.FilterOption = "";
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


                    param.Provinces = province;
                }
            } catch (Exception ex) {

            }
            return param;
        }
        #endregion
    }
}
