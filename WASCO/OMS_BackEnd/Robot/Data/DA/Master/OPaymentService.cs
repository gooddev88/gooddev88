using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.Master
{
    public class OPaymentService
    {
        public static string sessionActiveId = "activepaymentid";
        public static OPaymentType NewInfo(string rcom, string com)
        {
            OPaymentType n = new OPaymentType();
            n.RCompanyID = rcom;
            n.CompanyID = com;
            n.Code = "";
            n.Type = "";
            n.Cate = "";
            n.Name = "";
            n.DRCR = "";
            n.AccID = "";
            n.Sort = 0;
            n.Remark = "";
            n.IsActive = true;
            return n;
        }

        public static List<OPaymentType> InitPaymentDataNewCompany(string rcom, string com)
        {
            var pp = new List<OPaymentType>();
            var n = NewInfo(rcom, com);
            n.Type = "PAYMENT";
            n.Code = "CASH";
            n.Name = "เงินสด";
            n.Sort = 10;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "PAYMENT";
            n.Code = "CHEQUE";
            n.Name = "เช็ค";
            n.Sort = 20;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "PAYMENT";
            n.Code = "TRANSFER";
            n.Name = "เงินโอน";
            n.Sort = 30;
            pp.Add(n);
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "PAYMENT";
            n.Code = "OTHER";
            n.Name = "อื่นๆ";
            n.Sort = 40;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "TAX";
            n.Code = "WHT";
            n.Name = "หัก ณ ที่จ่าย";
            n.Sort = 110;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "ADJUST";
            n.Code = "TRANSFER_FEE";
            n.Name = "ค่าธรรมเนียมโอน";
            n.Sort = 1110;
            pp.Add(n);
            //**********//
            n = NewInfo(rcom, com);
            n.Type = "ADJUST";
            n.Code = "DIFF_DIGIT";
            n.Name = "ส่วนต่างเศษสตางค์";
            n.Sort = 1120;
            pp.Add(n);

            return pp;
        }

        public static List<OPaymentType> ListPayType(string type,string rcom,string com)
        {
            List<OPaymentType> result = new List<OPaymentType>();
            using (GAEntities db = new GAEntities())
            {
                result = db.OPaymentType.Where(o => o.IsActive ==true
                                                        && o.RCompanyID == rcom
                                                        && o.CompanyID == com
                                                        && (o.Type == type || type == "")
                                                        ).OrderBy(o => o.Sort).ToList();
            }
            return result;

        }

        public static OPaymentType GetPayType(string code, string rcom, string com)
        {
            OPaymentType result = new OPaymentType();
            using (GAEntities db = new GAEntities())
            {
                result = db.OPaymentType.Where(o => o.Code == code
                                                            && o.RCompanyID == rcom
                                                            && o.CompanyID == com
                                                        ).FirstOrDefault();
            }
            return result;
        }

    }
}
