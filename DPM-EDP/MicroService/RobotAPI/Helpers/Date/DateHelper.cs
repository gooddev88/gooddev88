namespace RobotAPI.Helpers.Date {
    public class DateHelper {
        public  static string ConvertThaiMonthShortText(int month_number) {
            string result = "";
            switch (month_number) {
                case 1:
                    result = "ม.ค.";
                    break;  
                    case 2:
                    result = "ก.พ.";
                    break;
                case 3:
                    result = "มี.ค.";
                    break;
                case 4:
                    result = "เม.ย.";
                    break;
                case 5:
                    result = "พ.ค.";
                    break;
                case 6:
                    result = "มิ.ย.";
                    break;
                case 7:
                    result = "ก.ค.";
                    break;
                case 8:
                    result = "ส.ค.";
                    break;
                case 9:
                    result = "ก.ย.";
                    break;
                case 10:
                    result = "ต.ค.";
                    break;
                case 11:
                    result = "พ.ย.";
                    break;
                case 12:
                    result = "ธ.ค.";
                    break;
               
            }
            return result;
        }
        public static string ConvertThaiMonthColor(int month_number) {
            string result = "";
            switch (month_number) {
                case 1:
                    result = "#643BE3";
                    break;
                case 2:
                    result = "#AB3BE3";
                    break;
                case 3:
                    result = "#E33BDE";
                    break;
                case 4:
                    result = "#E33B94";
                    break;
                case 5:
                    result = "#E33B60";
                    break;
                case 6:
                    result = "#E38C3B";
                    break;
                case 7:
                    result = "#E3BB3B";
                    break;
                case 8:
                    result = "#C3E33B";
                    break;
                case 9:
                    result = "#88E33B";
                    break;
                case 10:
                    result = "#3BE364";
                    break;
                case 11:
                    result = "#3BE3D1";
                    break;
                case 12:
                    result = "#3B9FE3";
                    break;

            }
            return result;
        }
    }
}
