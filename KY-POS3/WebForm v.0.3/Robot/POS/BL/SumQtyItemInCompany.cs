using Dapper;
using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Robot.POS.BL
{
    public class SumQtyItemInCompany
    {
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }       
        public decimal Price { get; set; }
        public decimal HK_OrdQty { get; set; }
        public decimal HK_OrdAmt { get; set; }
        public decimal LP_OrdQty { get; set; }
        public decimal LP_OrdAmt { get; set; }
        public decimal BP_OrdQty { get; set; }
        public decimal BP_OrdAmt { get; set; }
        public decimal CK_OrdQty { get; set; }
        public decimal CK_OrdAmt { get; set; }
        public decimal AN_OrdQty { get; set; }
        public decimal AN_OrdAmt { get; set; }
        public decimal SA_OrdQty { get; set; }
        public decimal SA_OrdAmt { get; set; }
        public decimal INTER_OrdQty { get; set; }
        public decimal INTER_OrdAmt { get; set; }
        public decimal EV_OrdQty { get; set; }
        public decimal EV_OrdAmt { get; set; }
        public decimal SL_OrdQty { get; set; }
        public decimal SL_OrdAmt { get; set; }
        public decimal NA_OrdQty { get; set; }
        public decimal NA_OrdAmt { get; set; }
        public decimal ALLCOMPANY_OrdQty { get; set; }
        public decimal ALLCOMPANY_OrdAmt { get; set; }


        public static List<SumQtyItemInCompany> ListData(DateTime dateBegin, DateTime dateEnd,string type)
        {
            List<SumQtyItemInCompany> result = new List<SumQtyItemInCompany>();
            try
            {
                string queryname = "";
                string itemType = "";
                if (type=="ORDERPRINT") {
                    queryname = "OrderPrint";
                    itemType = "FGK";
                }
                if (type == "PURCHASEPRINT") {
                    queryname = "PurchasePrint";
                    itemType = "RMK";
                }
                if (type == "POPRINT")
                {
                    queryname = "POPrint1";
                    //itemType = "RMK";
                }
                string conStr = DBDapperService.GetDBConnectFromAppConfig();
                string strSQL = DBDapperService.GetSQLCommand(queryname);
                using (var conn = new SqlConnection(conStr))
                {
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("DateBegin", dateBegin);
                    dynamicParameters.Add("DateEnd", dateEnd);
                    dynamicParameters.Add("Type", itemType);
                    result = conn.Query<SumQtyItemInCompany>(strSQL, dynamicParameters).ToList();
                    return result;
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }
    }


 
}