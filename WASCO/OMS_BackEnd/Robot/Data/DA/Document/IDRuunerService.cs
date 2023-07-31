using Microsoft.Extensions.Configuration;
using Robot.Data.DA.Master;
using Robot.Data.GADB.TT;
using Robot.Helper;
using Robot.Helper.Datetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.Document {
    public class IDRuunerService {
        public string ThYear2Digit { get; private set; }

        public static string CreateRunning(string prefix, string digit, string year, string month, string com, string rcom, bool isrun_next) {
            string result = "";
            //  string comId = LoginService.LoginInfo.CurrentCompany;

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
                            //db.IDGenerator.Update(chk_existid);
                            db.SaveChanges();

                        }

                    }
                } catch (Exception ex) {
                    var e = ex.Message;
                }
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

        //public static List<string> GetNewIDV2(string docTypeId,string rcom, string comId, DateTime docdate, bool isrun_next, string year_culture) {
        //    List<string> result = new List<string> { "R1", "" };
        //    string month = DatetimeInfoService.Month2Digit(docdate);
        //    string year = "";
        //    if (year_culture.ToLower() == "th") {
        //        year = DatetimeInfoService.ThYear2DigitV2(docdate);
        //    }

        //    if (year_culture.ToLower() == "en") {
        //        year = DatetimeInfoService.EnYear2DigitV2(docdate);
        //    }

        //    var comInfo = CompanyService.GetComInfoByComID(rcom,comId);
        //    string prefix = "";
        //    string digit = "0000";

        //    try {

        //        if (docTypeId.ToUpper() == "ALERT") {
        //            prefix = "AA" + year + month + "-";
        //            digit = "000";
        //            result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
        //        }
        //        if (docTypeId.ToUpper() == "OORDER") {
        //            prefix = "SO" + comInfo.ShortCode + year + month + "-";
        //            digit = "0000";
        //            result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
        //        }

        //        if (docTypeId.ToUpper() == "ITEM")
        //        {
        //            prefix = "I" + comId + year + month + "-";
        //            digit = "0000";
        //            result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
        //        }


        //    } catch (Exception ex) {
        //        result[0] = "R0";
        //        result[1] = "";
        //    }

        //    return result;
        //}


        public static List<string> GetNewIDV2(string docTypeId, string rcom, string comId, DateTime docdate, bool isrun_next, string year_culture) {
            List<string> result = new List<string> { "R1", "" };
            string day =DatetimeInfoService.Day2Digit(docdate);
            string month = DatetimeInfoService.Month2Digit(docdate);
            string year = "";
            if (year_culture.ToLower() == "th") {
                year = DatetimeInfoService.ThYear2DigitV2(docdate);
            }

            if (year_culture.ToLower() == "en") {
                year = DatetimeInfoService.EnYear2DigitV2(docdate);
            }

            var comInfo = CompanyService.GetComInfoByComID(comId);
            string prefix = "";
            string digit = "0000";

            try {
                if (docTypeId.ToUpper() == "PROMOTION") {
                    prefix = "PRO" + comInfo.ShortCode + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
                if (docTypeId.ToUpper() == "TRANSFER") {
                    prefix = "TR" + comInfo.ShortCode + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
                if (docTypeId.ToUpper() == "ADJUST") {
                    prefix = "ADJ" + comInfo.ShortCode + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

         
               
                if (docTypeId.ToUpper() == "OORDER") {
                    prefix = "SO" + comInfo.ShortCode + year + month  + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
            

             

                // master
                if (docTypeId.ToUpper() == "OCUSTOMER") {
                    prefix = "C" + year + month + "-";
                    digit = "00000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }
           
                if (docTypeId.ToUpper() == "OCOMPANY") {
                    prefix = "O" + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

                if (docTypeId.ToUpper() == "ITEM") {
                    prefix = "I" + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

                if (docTypeId.ToUpper() == "DOCGO") {
                    prefix = "D" + comId + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

         

                if (docTypeId.ToUpper() == "REQ") {
                    prefix = "REQ" + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

                if (docTypeId.ToUpper() == "WTH") {
                    prefix = "WTH" + year + month + "-";
                    digit = "0000";
                    result[1] = CreateRunning(prefix, digit, "", "", comId, rcom, isrun_next);
                }

            } catch (Exception ex) {
                result[0] = "R0";
                result[1] = "";
            }

            return result;
        }

    }
}
