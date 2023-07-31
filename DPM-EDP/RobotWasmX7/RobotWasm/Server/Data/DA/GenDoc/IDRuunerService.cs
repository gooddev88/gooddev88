using Dapper;
using Npgsql;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Shared.Data.DimsDB;
using System.Data;
using System.Linq;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;
using Microsoft.Data.SqlClient;
using RobotWasm.Shared.Data.ML.Login;
using RobotWasm.Server.Helper;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;

namespace RobotWasm.Server.Data.DA.GenDoc
{
    public class IDRuunerService
    {
        public static string CreateRunning(string prefix, string digit, string year, string month, string com, string rcom, bool isrun_next)
        {
            string result = "";
            using (cimsContext db = new cimsContext())
            {
                try
                {
                    var chk_existid = db.idgenerator.Where(o => o.prefix == prefix && o.year == year && o.month == month && o.comid == com && o.rcomid == rcom).FirstOrDefault();

                    int new_running = 0;
                    if (chk_existid != null)
                    {
                        //next runnumber
                        new_running = Convert.ToInt32(chk_existid.digit_run_number);
                    }
                    else
                    {//start 1  
                        new_running = 1;
                    }

                    result = prefix + new_running.ToString(digit);
                    if (isrun_next)
                    {
                        if (chk_existid == null)
                        {
                            var newid = new idgenerator
                            {
                                prefix = prefix,
                                year = year,
                                month = month,
                                funcid = "",
                                macno = "",
                                comid = com,
                                rcomid = rcom,
                                digit_run_number = new_running + 1,
                                description = "",
                                created_date = DateTime.Now,
                                latest_date = DateTime.Now
                            };
                            db.idgenerator.Add(newid);
                            db.SaveChanges();

                        }
                        else
                        {
                            chk_existid.digit_run_number = chk_existid.digit_run_number + 1;
                            chk_existid.latest_date = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    var e = ex.Message;
                }
            }


            return result;
        }


        public static List<string> GetNewIDV2(string docTypeId, string? rcom, string? comId, DateTime docdate, bool isrun_next, string year_culture)
        {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            List<string> result = new List<string> { "R1", "" };
            string month = Month2Digit(docdate);
            string year = "";
            if (year_culture.ToLower() == "th")
            {
                year = ThYear2DigitV2(docdate);
            }

            if (year_culture.ToLower() == "en")
            {
                year = EnYear2DigitV2(docdate);
            }

            string prefix = "";
            string digit = "0000";

            try
            {
                // COMPANY
                if (docTypeId.ToUpper() == "COMPANY")
                {
                    prefix = "O" + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
                // COMPANY GROUP
                if (docTypeId.ToUpper() == "COMGROUP")
                {
                    prefix = "GO" + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
                // DOCCATE
                if (docTypeId.ToUpper() == "DOCCATE")
                {
                    prefix = "DOC" + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
                // TABLEAU
                if (docTypeId.ToUpper() == "TABLEAU")
                {
                    prefix = "TB" + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

                // APICATE
                if (docTypeId.ToUpper() == "APICATE")
                {
                    prefix = "DC" + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

                // USERGROUP
                if (docTypeId.ToUpper() == "USERGROUP")
                {
                    prefix = "UG" + "-";
                    digit = "00000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }           

            }
            catch (Exception ex)
            {
                result[0] = "R0";
                result[1] = "";
            }

            return result;
        }


        //*****add on ********************

        public static string EngDateddMMyyy(DateTime mydate)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            return mydate.ToString("ddMMyyyy");
        }
        public static string ThDateddMMyyy(DateTime mydate)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            return mydate.ToString("ddMMyyyy");
        }
        public static string Day2Digit(DateTime mydate)
        {
            return mydate.Day.ToString("00");
        }
        public static string Month2Digit(DateTime mydate)
        {
            return mydate.Month.ToString("00");
        }
        public static string ThYear4Digit(DateTime mydate)
        {

            if (mydate.Year > 2500)
            {
                return (mydate.Year).ToString();
            }
            else
            {
                return (mydate.Year + 543).ToString();
            }
            #region old code
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            //return mydate.Date.Year.ToString();
            #endregion
        }
        public static string EngYear4Digit(DateTime mydate)
        {

            if (mydate.Year > 2500)
            {
                return (mydate.Year - 543).ToString();
            }
            else
            {
                return (mydate.Year).ToString();
            }
            #region old code
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            //return mydate.Date.Year.ToString();
            #endregion

        }
        public static string ThYear2Digit(DateTime mydate)
        {
            if (mydate.Year > 2500)
            {
                return (mydate.Year).ToString().Substring(2, 2);
            }
            else
            {

                return (mydate.Year + 543).ToString().Substring(2, 2);

            }
            #region old code
            //  string myyear = mydate.ToString("yy", new CultureInfo("th-TH"));
            //return myyear.Substring(myyear.Length - 2);
            //  return myyear;
            #endregion

        }
        public static string EngYear2Digit(DateTime mydate)
        {
            if (mydate.Year > 2500)
            {
                return (mydate.Year - 543).ToString().Substring(2, 2);
            }
            else
            {
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
            }
            else
            {
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
            }
            else
            {
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

    }
}
