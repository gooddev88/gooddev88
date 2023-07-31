using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class DatetimeInfoService {
        #region Class


        #endregion


        #region  Method GET
        public static int Convert2ThaiYear(DateTime getDate) {
            if (getDate.Year > 2500) {
                return getDate.Year;
            } else {
                return getDate.Year + 543;
            }
        }

        public static string Convert2StringEngYearMMddyyyy(DateTime getDate) {
            string cyear = "";
            if (getDate.Year > 2500) {
                cyear = (getDate.Year - 543).ToString();
            } else {
                cyear = getDate.Year.ToString();
            }
            return getDate.Month.ToString() + "/" + getDate.Day.ToString() + "/" + cyear;
        }
        public static string Convert2StringEngYearyyyyMMdd(DateTime getDate) {
            string cyear = "";
            if (getDate.Year > 2500) {
                cyear = (getDate.Year - 543).ToString();
            } else {
                cyear = getDate.Year.ToString();
            }
            return cyear+"/"+getDate.Month.ToString("00") + "/" + getDate.Day.ToString("00")   ;
        }
        public static string GetEngYear(DateTime getDate) {
            string cyear = "";
            if (getDate.Year > 2500) {
                cyear = (getDate.Year - 543).ToString();
            } else {
                cyear = getDate.Year.ToString();
            }
            return cyear;
        }
        public static string Convert2StringEngYearddMMyyyy(DateTime getDate) {
            string cyear = "";
            if (getDate.Year > 2500) {
                cyear = (getDate.Year - 543).ToString();
            } else {
                cyear = getDate.Year.ToString();
            }
            return getDate.Day.ToString() + "/" + getDate.Month.ToString() + "/" + cyear;
        }
        public static DateTime? ConvertStringToDate(string strDate) {
            DateTime temp;
            if (DateTime.TryParse(strDate, out temp)) {
                string cyear = "";
                if (temp.Year <= 2500) {
                    cyear = (temp.Year + 543).ToString();
                } else {
                    cyear = temp.Year.ToString();
                }
                return new DateTime(Convert.ToInt32(cyear), temp.Month, temp.Day);
            } else {
                return null;
            }
        }

        public static int GetWeekOfMonth(DateTime date) {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }


        public static string GetPeriodYearMonth(DateTime date) {
            string result = "";
           string year= GetEngYear(date);
            result = year + "-" + date.Month.ToString("00");
            return result;
        }
        public static string GetPeriodYearMonthDay(DateTime date) {
            string result = ""; 
            string year = Convert2ThaiYear(date).ToString();
            year = year.Substring(year.Length - 2);
            result = year +  date.Month.ToString("00")+date.Day.ToString("00");
            return result;
        }
        public static DateTime? Validate(object date) {
            DateTime? result = null;
            try {
                var mydate = Convert.ToDateTime(date);
                var engyear = Convert.ToInt32(GetEngYear(mydate));
                if (engyear >= 2000 && engyear <= 2100) {
                    result = mydate;
                }
            } catch { }
            return result;
        }





        public static string Month2Digit(DateTime mydate) {
            return DateTime.Now.Month.ToString("00");
        }
        public static string ThYear4Digit(DateTime mydate) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            return mydate.Date.Year.ToString();
        }
        public static string EngYear4Digit(DateTime mydate) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            return mydate.Date.Year.ToString();
        }
        public static string ThYear2Digit(DateTime mydate) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            string myyear = mydate.Year.ToString();
            return myyear.Substring(myyear.Length - 2);
        }
        public static string EngYear2Digit(DateTime mydate) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            string myyear = mydate.Year.ToString();
            return myyear.Substring(myyear.Length - 2);
        }
        #endregion
    }
}