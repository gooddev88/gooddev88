using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class TaxInfoService {
        #region Class
        public class SelectListTax :TaxInfo{
        
        }
        #endregion


        #region  Method GET

        public static List<SelectListTax> MiniSelectList( bool needRefresh) {
            //บังคับให้ refresh data จาก db to session หรือ session เป็น null
            List<SelectListTax> result = new List<SelectListTax>();
            if (needRefresh || HttpContext.Current.Session["tax"] == null) {
                #region Refresh from database      
             
                using (GAEntities db = new GAEntities()) {
                    //for po only // type PURCHASE
                    var query = db.TaxInfo.Where(o => o.IsActive && o.Type == "PURCHASE").OrderBy(o => o.Sort).ToList();
                    //var query = db.TaxInfo.Where(o =>  o.IsActive).OrderBy(o=>o.Sort).ToList();

                    foreach (var q in query.OrderBy(o => o.Sort).ThenBy(o => o.Sort).ToList()) {
                        SelectListTax nd = new SelectListTax();
                        nd.TaxID = q.TaxID;
                        nd.TaxName = q.TaxName;
                        nd.Desc = q.Desc;
                        nd.TaxValue = q.TaxValue;                
                        nd.Sort = q.Sort;
                        nd.IsActive = q.IsActive;

                        result.Add(nd);
                 
                    }
                    SelectListTax blank = new SelectListTax { TaxID = "", TaxName = "", Desc = "", TaxValue = 0, Sort = 0, IsActive = true };
                    result.Add(blank);
                    HttpContext.Current.Session["tax"] = result;
                }
                #endregion
            } else {
                 result = (List<SelectListTax>)HttpContext.Current.Session["tax"];
             
            }
            return result;
        }

        public static List<SelectListTax> MiniSelectListV2(string type, string currency, bool needBlank)
        {
            //type = PURCHASE || SALE
            List<SelectListTax> result = new List<SelectListTax>();
            using (GAEntities db = new GAEntities())
            {
                var query = db.TaxInfo.Where(o => o.IsActive
                                                && o.Type == type
                                                && (o.Currency == currency || currency == "")
                                                ).OrderBy(o => o.Sort).ToList();

                foreach (var q in query.OrderBy(o => o.Sort).ThenBy(o => o.Sort).ToList())
                {
                    SelectListTax nd = new SelectListTax();
                    nd.TaxID = q.TaxID;
                    nd.TaxName = q.TaxName;
                    nd.Desc = q.Desc;
                    nd.TaxValue = q.TaxValue;
                    nd.Sort = q.Sort;
                    nd.IsActive = q.IsActive;
                    result.Add(nd);

                }
                if (needBlank)
                {
                    SelectListTax blank = new SelectListTax { TaxID = "", TaxName = "", Desc = "", TaxValue = 0, Sort = 0, IsActive = true };
                    result.Insert(0, blank);
                }
            }
            return result;
        }

        public static List<TaxInfo> ListSearch(string search) {
            
            List<TaxInfo> result = new List<TaxInfo>();
            using (GAEntities db = new GAEntities()) {
                result = db.TaxInfo.Where(o => (o.IsActive)).OrderBy(o => o.Sort).ToList();
            }
            return result;
        }

       public static decimal GetRateByTaxID(string taxID) {
            List<SelectListTax> tax = (List<SelectListTax>)HttpContext.Current.Session["tax"];
            decimal result = 0;
            if (tax==null) {
                MiniSelectList(true);
                tax = (List<SelectListTax>)HttpContext.Current.Session["tax"];
            }
            var r = tax.Where(o => o.TaxID == taxID).FirstOrDefault();
            if (r!=null) {
                result = r.TaxValue;
            }
            return result;
      
        }


        public static decimal IncludeTaxAmt(decimal amt,decimal vatrate) {

            decimal result =0;
            result =Math.Round( (amt * vatrate) / 100,3);
            return result;
        }
        public static decimal CalVatAmt(decimal amt, decimal vatrate) {

            decimal result = 0;
            result = Math.Round((amt * vatrate) / 100, 3);
            return result;
        }
        #endregion
    }
}