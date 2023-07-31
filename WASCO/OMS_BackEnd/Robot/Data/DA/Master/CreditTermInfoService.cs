using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.Master
{
    public class CreditTermInfoService
    {

        #region Class
        public class SelectListCreditTerm : CreditTerm
        {


        }
        #endregion

        #region  Method GET
        public static List<SelectListCreditTerm> MiniSelectList(bool isShowBlankRow, string accType)
        {

            List<SelectListCreditTerm> result = new List<SelectListCreditTerm>();
            using (GAEntities db = new GAEntities())
            {
                var query = db.CreditTerm.Where(o => o.IsActive
                                                       && (o.AccType == accType || accType == "")
                                                       ).ToList();
                foreach (var q in query)
                {
                    SelectListCreditTerm n = new SelectListCreditTerm();
                    n.TermID = q.TermID;
                    n.TermDesc = q.TermDesc;
                    n.CalType = q.CalType;
                    n.UseIn = q.UseIn;
                    n.AccType = q.AccType;
                    n.Day = q.Day;
                    result.Add(n);
                }
                if (isShowBlankRow)
                {
                    SelectListCreditTerm blank = new SelectListCreditTerm { TermID = "", TermDesc = "", CalType = "", UseIn = "", AccType = accType, Day = 0 };
                    result.Insert(0, blank);
                }

            }
            return result;
        }

        public static DateTime GetPaymentDueDateFromTerm(string termcode, DateTime datefr, string accType)
        {
            DateTime result;
            using (GAEntities db = new GAEntities())
            {
                var term = db.CreditTerm.Where(o => o.TermID == termcode && o.AccType == accType).FirstOrDefault();
                if (term == null)
                {
                    result = datefr;
                }
                else
                {
                    result = datefr.AddDays(term.Day);
                }
            }
            return result;
        }

        public static BankInfo GetDataByBankCode(string BankCode)
        {
            BankInfo result = new BankInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.BankInfo.Where(o => o.BankCode == BankCode).FirstOrDefault();
            }
            return result;
        }

        #endregion
    }
}
