using System.Data;
using System.Globalization;
using RobotWasm.Shared.Data.ML.Shared;
using RobotWasm.Server.Data.DA.Master;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.GaDB;

namespace RobotWasm.Server.Data.DA.GenDoc {
    public class IDRuunerService
    {
        public static string CreateRunning(string prefix, string digit, string year, string month, string com, string rcom, bool isrun_next) {
            string result = "";
            using (GAEntities db = new GAEntities()) {
                try {
                    var chk_existid = db.IDGenerator.Where(o => o.Prefix == prefix && o.Year == year && o.Month == month && o.ComID == com && o.RComID == rcom).FirstOrDefault();
                    int new_running = 0;
                    if (chk_existid != null) {

                        //next runnumber
                        new_running = chk_existid.DigitRunNumber;
                    } else {//start 1  
                        new_running = 1;
                    }

                    result = prefix + new_running.ToString(digit);
                    if (isrun_next) {
                        if (chk_existid == null) {
                            var newid = new IDGenerator {
                                Prefix = prefix,
                                Year = year,
                                Month = month,
                                FuncID = "",
                                MacNo = "",
                                ComID = com,
                                RComID = rcom,
                                DigitRunNumber = new_running + 1,
                                Description = "",
                                CreatedDate = DateTime.Now,
                                LatestDate = DateTime.Now
                            };
                            db.IDGenerator.Add(newid);
                            db.SaveChanges();
                        } else {
                            chk_existid.DigitRunNumber = chk_existid.DigitRunNumber + 1;
                            chk_existid.LatestDate = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                } catch (Exception ex) {
                    var e = ex.Message;
                }
            }

            return result;
        }

        public static List<string> GetNewIDV2(string docTypeId, string? rcom, string? comId, DateTime docdate, bool isrun_next, string year_culture) {
            rcom = rcom == null ? "" : rcom;
            comId = comId == null ? "" : comId;
            List<string> result = new List<string> { "R1", "" };
            string month = Month2Digit(docdate);
            string year = "";
            if (year_culture.ToLower() == "th") {
                year = ThYear2DigitV2(docdate);
            }

            if (year_culture.ToLower() == "en") {
                year = EnYear2DigitV2(docdate);
            }

            string prefix = "";
            string digit = "0000";

            try {
                // SO
                if (docTypeId.ToUpper() == "SO1") {
                    prefix = "SO" + year;
                    digit = "00000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
            } catch (Exception ex) {
                result[0] = "R0";
                result[1] = "";
            }

            return result;
        }
        public static List<string> GenPOSSaleID(string docType, string rcom, string storeId, string macno, string shiptoId, bool isrun_next, DateTime transDate) {
            List<string> result = new List<string> { "R1", "" };

            string year = DatetimeInfoService.Convert2ThaiYear(transDate).ToString();
            var comInfo = CompanyService.GetComInfoByComID(rcom, storeId);
            year = year.Substring(year.Length - 2);
            string month = transDate.Month.ToString("00");
            string day = transDate.Day.ToString("00");
            string prefix = shiptoId + comInfo.ShortCode + macno + year + month + day;
            try {
                switch (docType) {
                    case "ORDER": //Order   
                        prefix = "B" + prefix + "-";
                        result[1] = CreateRunningByPOS(macno, prefix, "000", "", "", rcom, comInfo.CompanyID, isrun_next, "SALE");
                        break;
                    case "INV": //Invoice   
                        prefix = prefix + "-";
                        result[1] = CreateRunningByPOS(macno, prefix, "000", "", "", rcom, comInfo.CompanyID, isrun_next, "SALE");
                        break;
                    case "TAX": //Full tax invoice   
                        prefix = "T" + prefix + "-";
                        result[1] = CreateRunningByPOS(macno, prefix, "000", "", "", rcom, comInfo.CompanyID, isrun_next, "SALE");
                        break;
                }
            } catch (Exception ex) {
                result[0] = "R0";
                result[1] = "";
            }
            return result;
        }

        public static string CreateRunningByPOS(string macno, string prefix, string digit, string year, string month, string rcom, string comId, bool isrun_next, string funcId) {
            string result = "";

            using (GAEntities db = new GAEntities()) {

                var chk_existid = db.IDGenerator.Where(o => o.RComID == rcom
                                                            && o.Prefix == prefix
                                                            && o.Year == year
                                                            && o.MacNo == macno
                                                            && o.Month == month
                                                            && o.ComID == comId).FirstOrDefault();

                int new_running = 1;
                if (chk_existid != null) {//next runnumber
                    new_running = Convert.ToInt32(chk_existid.DigitRunNumber);
                } else {//start 1  
                    new_running = 1;
                }

                result = prefix + new_running.ToString(digit);
                if (isrun_next) {
                    if (chk_existid == null) {//ถ้าไม่มี record เดิม
                        var newid = new IDGenerator {

                            Prefix = prefix,
                            Year = year,
                            RComID = rcom,
                            ComID = comId,
                            Month = month,
                            FuncID = funcId,
                            DigitRunNumber = (new_running + 1),
                            MacNo = macno,
                            Description = "",
                            CreatedDate = DateTime.Now,
                            LatestDate = DateTime.Now
                        };
                        db.IDGenerator.Add(newid);
                        db.SaveChanges();
                    } else {//ถ้ามี record เดิมให้ update
                        chk_existid.DigitRunNumber = (Convert.ToInt32(chk_existid.DigitRunNumber) + 1);
                        chk_existid.LatestDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
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
