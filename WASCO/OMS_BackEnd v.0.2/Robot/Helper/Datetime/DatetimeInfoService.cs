using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Robot.Helper.Datetime {
    public class DatetimeInfoService {
        #region Class
        public class BasicDataBinding
        {
            public string ValueText { get; set; }
            public int ValueNum { get; set; }
            public string Desc { get; set; }
        }

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
            string year = GetEngYear(date);
            result = year + "-" + date.Month.ToString("00");
            return result;
        }
        public static string GetPeriodYearMonthDay(DateTime date) {
            string result = "";
            string year = Convert2ThaiYear(date).ToString();
            year = year.Substring(year.Length - 2);
            result = year + date.Month.ToString("00") + date.Day.ToString("00");
            return result;
        }

        #endregion


        //calculate age
        public static string CalculateAge(DateTime? birthDate) {
            try {
                if (birthDate == null) {
                    return "ไม่ทราบวันเกิด";
                }

                DateTime dateOfBirth = Convert.ToDateTime(birthDate);

                DateTime currentDate = DateTime.Now;

                TimeSpan difference = currentDate.Subtract(dateOfBirth);

                // This is to convert the timespan to datetime object  
                DateTime age = DateTime.MinValue + difference;

                // Min value is 01/01/0001  
                // Actual age is say 24 yrs, 9 months and 3 days represented as timespan  
                // Min Valye + actual age = 25 yrs , 10 months and 4 days.  
                // subtract our addition or 1 on all components to get the actual date.  

                int ageInYears = age.Year - 1;
                int ageInMonths = age.Month - 1;
                int ageInDays = age.Day - 1;


                return $"อายุ {ageInYears} ปี {ageInMonths} เดือน {ageInDays} วัน";
            } catch (Exception) {

                return "ไม่ทราบวันเกิด";
            }

        }

        //*****add on ********************

        public static string EngDateddMMyyy(DateTime mydate) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            return mydate.ToString("ddMMyyyy");
        }
        public static string ThDateddMMyyy(DateTime mydate) {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            return mydate.ToString("ddMMyyyy");
        }
        public static string Day2Digit(DateTime mydate) {
            return mydate.Day.ToString("00");
        }
        public static string Month2Digit(DateTime mydate) {
            return mydate.Month.ToString("00");
        }
        public static string ThYear4Digit(DateTime mydate) {

            if (mydate.Year > 2500) {
                return (mydate.Year).ToString();
            } else {
                return (mydate.Year + 543).ToString();
            }
            #region old code
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            //return mydate.Date.Year.ToString();
            #endregion
        }
        public static string EngYear4Digit(DateTime mydate) {

            if (mydate.Year > 2500) {
                return (mydate.Year - 543).ToString();
            } else {
                return (mydate.Year).ToString();
            }
            #region old code
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //return mydate.Date.Year.ToString();
            #endregion

        }
        public static string ThYear2Digit(DateTime mydate) {
            if (mydate.Year > 2500) {
                return (mydate.Year).ToString().Substring(2, 2);
            } else {

                return (mydate.Year + 543).ToString().Substring(2, 2);

            }
            #region old code
            //  string myyear = mydate.ToString("yy", new CultureInfo("th-TH"));
            //return myyear.Substring(myyear.Length - 2);
            //  return myyear;
            #endregion

        }
        public static string EngYear2Digit(DateTime mydate) {
            if (mydate.Year > 2500) {
                return (mydate.Year - 543).ToString().Substring(2, 2);
            } else {
                return (mydate.Year).ToString().Substring(2, 2);
            }

        }
        public static string EnYear2DigitV2(DateTime mydate)
        {

            int input_year = mydate.Year;
            string myyear = "";
            if (input_year > 2500)
            {
                myyear = (input_year - 543).ToString();
            } else {
                myyear = input_year.ToString();
            }
            return myyear.Substring(myyear.Length - 2);
        }

        public static string ThYear2DigitV2(DateTime mydate)
        {
            int input_year = mydate.Year;
            string myyear = "";
            if (input_year <= 2500)
            {
                myyear = (input_year + 543).ToString();
            } else {
                myyear = input_year.ToString();
            }
            return myyear.Substring(myyear.Length - 2);
        }
        public static bool IsOkDate(DateTime inputDate)
        {
            bool result = false;
            var checkyear = Convert.ToInt32(inputDate.ToString("yyyy", CultureInfo.InvariantCulture));
            if (checkyear >= 1900 && checkyear <= 2100)
            {
                result = true;
            }
            return result;
        }


        public static List<BasicDataBinding> ListEngMonthSelect()
        {
            List<BasicDataBinding> result = new List<BasicDataBinding>();

            result.Add(new BasicDataBinding { ValueNum = 0, Desc = "" });
            result.Add(new BasicDataBinding { ValueNum = 1, Desc = "มกราคม" });
            result.Add(new BasicDataBinding { ValueNum = 2, Desc = "กุมภาพันธ์" });
            result.Add(new BasicDataBinding { ValueNum = 3, Desc = "มีนาคม" });
            result.Add(new BasicDataBinding { ValueNum = 4, Desc = "เมษายน" });
            result.Add(new BasicDataBinding { ValueNum = 5, Desc = "พฤษภาคม" });
            result.Add(new BasicDataBinding { ValueNum = 6, Desc = "มิถุนายน" });
            result.Add(new BasicDataBinding { ValueNum = 7, Desc = "กรกฎาคม" });
            result.Add(new BasicDataBinding { ValueNum = 8, Desc = "สิงหาคม" });
            result.Add(new BasicDataBinding { ValueNum = 9, Desc = "กันยายน" });
            result.Add(new BasicDataBinding { ValueNum = 10, Desc = "ตุลาคม" });
            result.Add(new BasicDataBinding { ValueNum = 11, Desc = "พฤศจิกายน" });
            result.Add(new BasicDataBinding { ValueNum = 12, Desc = "ธันวาคม" });
            return result;
        }

    }
}
