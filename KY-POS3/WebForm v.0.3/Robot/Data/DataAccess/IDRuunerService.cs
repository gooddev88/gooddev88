using Robot.Master.DA;
using Robot.POSC.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess
{
    public static class IDRuunerService
    {

        public static string CreateRunning(string macno, string prefix, string digit, string year, string month, string comId, bool isrun_next, string funcId)
        {
            string result = "";
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities())
            {

                var chk_existid = db.IDGenerator.Where(o => o.RComID == rcom
                                                            && o.Prefix == prefix
                                                            && o.Year == year
                                                            && o.MacNo == macno
                                                            && o.Month == month
                                                            && o.ComID == comId).FirstOrDefault();

                int new_running = 1;
                if (chk_existid != null)
                {//next runnumber
                    new_running = Convert.ToInt32(chk_existid.DigitRunNumber);
                }
                else
                {//start 1  
                    new_running = 1;
                }

                result = prefix + new_running.ToString(digit);
                if (isrun_next)
                {
                    if (chk_existid == null)
                    {//ถ้าไม่มี record เดิม
                        var newid = new IDGenerator
                        {

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
                    }
                    else
                    {//ถ้ามี record เดิมให้ update
                        chk_existid.DigitRunNumber = (Convert.ToInt32(chk_existid.DigitRunNumber) + 1);
                        chk_existid.LatestDate = DateTime.Now; 
                        db.SaveChanges();
                    }

                }
            }
            return result;

        }




        public static List<string> GenPOSSaleID(string docType, string storeId, string macno, string shiptoId, bool isrun_next, DateTime transDate)
        {
            List<string> result = new List<string> { "R1", "" };

            var shortShiptoId = ShipToService.ListShipTo().Where(o => o.ShipToID == shiptoId).FirstOrDefault().ShortID;
            string year = DatetimeInfoService.Convert2ThaiYear(transDate).ToString();
            var comInfo = CompanyService.GetCompanyInfo(storeId);
            year = year.Substring(year.Length - 2);
            string month = transDate.Month.ToString("00");
            string day = transDate.Day.ToString("00");
            string prefix = shortShiptoId + comInfo.ShortCode + macno + year + month + day;
            try
            {
                switch (docType)
                {
                    case "ORDER": //Order   
                        prefix = "B" + prefix + "-";
                        result[1] = CreateRunning(macno, prefix, "000", "", "", storeId, isrun_next, "SALE");
                        break;
                    case "INV": //Invoice   
                        prefix = prefix + "-";
                        result[1] = CreateRunning(macno, prefix, "000", "", "", storeId, isrun_next, "SALE");
                        break;
                    case "TAX": //Full tax invoice   
                        prefix = "T" + prefix + "-";
                        result[1] = CreateRunning(macno, prefix, "000", "", "", storeId, isrun_next, "SALE");
                        break;
                }
            }
            catch (Exception ex)
            {
                result[0] = "R0";
                result[1] = "";
            }
            return result;
        }




        //public static List<string> GetNewID(string docType, string comId, bool isrun_next, DateTime transDate, string year_culture)
        //{
        public static List<string> GetNewID(string docType, string com, bool isrun_next, string year_culture, DateTime docDate)
        {
            List<string> result = new List<string> { "R1", "" };
            var rcom = LoginService.LoginInfo.CurrentRootCompany;

            string month = DatetimeInfoService.Month2Digit(docDate);
            var comInfo = CompanyService.GetCompanyInfo(com);
            string year2Digit = "";
            string year4Digit = "";
            if (year_culture.ToLower() == "th")
            {
                year2Digit = DatetimeInfoService.ThYear2Digit(docDate);
                year4Digit = DatetimeInfoService.ThYear4Digit(docDate);

                if (year_culture.ToLower() == "en")
                {
                    year2Digit = DatetimeInfoService.EngYear2Digit(docDate);
                    year4Digit = DatetimeInfoService.EngYear4Digit(docDate);
                }

                string prefix = "";
                //string prefix =   comInfo.ShortCode + macno + year + month + day;
                try
                {
                    switch (docType)
                    {
                        case "COMPANY": // company
                            prefix = "COM-";
                            result[1] = CreateRunning("", prefix, "00000", year2Digit, month, "", isrun_next, "COMPANY");
                            break;

                        case "BOM": // Bom
                            prefix = "BOM-";
                            result[1] = CreateRunning("", prefix, "00000", year2Digit, month, "", isrun_next, "BOM");
                            break;

                        case "ORD": // ORD
                            prefix = "O" + comInfo.ShortCode + year2Digit + month + "-";
                            result[1] = CreateRunning("", prefix, "000", "", "", com, isrun_next, "ORDER");
                            break;

                        case "PO": // PO
                            prefix = "PO" + comInfo.ShortCode + year2Digit + month + "-";
                            result[1] = CreateRunning("", prefix, "000", year2Digit, month, "", isrun_next, "PO");
                            break;

                        case "ADJUST": // ADJUST
                            prefix = "ADJ" + comInfo.ShortCode + year2Digit + month + "-";
                            result[1] = CreateRunning("", prefix, "000", "", "", com, isrun_next, "STOCK");
                            break;

                        case "VENDOR_INFO":
                            prefix = "V-";
                            result[1] = CreateRunning("", prefix, "00000", year2Digit, month, "", isrun_next, "VENDOR");
                            break;
                        case "CUSTOMER_INFO":
                            prefix = "C-";
                            result[1] = CreateRunning("", prefix, "00000", year2Digit, month, "", isrun_next, "VENDOR");
                            break;
                        case "QA":
                            prefix = "QA-";
                            result[1] = CreateRunning("", prefix, "00000", year2Digit, month, "", isrun_next, "VENDOR");
                            break;
                        case "PROMOTION":
                            prefix = "PRO-";
                            result[1] = CreateRunning("", prefix, "00000", year2Digit, month, "", isrun_next, "VENDOR");
                            break;
                        case "OINV":
                            prefix = "I" + comInfo.ShortCode + year2Digit + month + "-";
                            result[1] = CreateRunning("", prefix, "000", year2Digit, month, "", isrun_next, "OINV");
                            break;
                        case "ORC":
                            prefix = "RC" + comInfo.ShortCode + year2Digit + month + "-";
                            result[1] = CreateRunning("", prefix, "000", year2Digit, month, "", isrun_next, "ORC");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    result[0] = "R0";
                    result[1] = "";
                }

                
            }
            return result;

        }
    }
}