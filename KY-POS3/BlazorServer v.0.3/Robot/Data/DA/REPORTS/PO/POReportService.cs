using Dapper;
using Robot.Data.ML;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.REPORTS.PO {
    public class POReportService {
        public static List<PORequestSummaryByStore> ListDataPOByStore(DateTime Begin, DateTime End, string rcom, string com,   int isPendingOnly) {
            List<PORequestSummaryByStore> result = new List<PORequestSummaryByStore>();
            try {
                string conStr = Globals.GAEntitiesConn;
                if (isPendingOnly == 1) {
                    //Begin = new DateTime(1999, 1, 1);
                    //End = new DateTime(2099, 12, 31);
                    Begin = DateTime.Now.Date.AddDays(-2);
                    End = DateTime.Now.Date;
                }
                using (var conn = new SqlConnection(conStr)) {
                    string sqlCmd = VQueryService.GetCommand("POSumByStore");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("dateBegin", Begin);
                    dynamicParameters.Add("dateEnd", End);
                    dynamicParameters.Add("RComID", rcom);
                    dynamicParameters.Add("ComID", com); 
                    dynamicParameters.Add("IsPendingOnly", isPendingOnly);
                    result = conn.Query<PORequestSummaryByStore>(sqlCmd, dynamicParameters).ToList();
                }
            } catch (Exception ex) {

            }
            return result;
        }
        public static List<POSummaryByStoreWithVendor> ListDataPOByStoreConvert(List<PORequestSummaryByStore> input) {
            List<POSummaryByStoreWithVendor> output = new List<POSummaryByStoreWithVendor>();
            foreach (var i in input) {
                
                var o = output.Where(o => o.StoreID == i.ToLocID).FirstOrDefault();
                if (o == null) {
                    o = new POSummaryByStoreWithVendor();
                    o.RComID = i.RComID;
                    o.ComID = i.ComID;
                    o.StoreID = i.ToLocID;
                    o.StoreName = i.CompanyName;
                    o.PODate = i.PODate;
                    if (i.VendorID == "SUP") {
                        o.Vendor1Name = i.VendorName;
                        o.TotalInVendor1 = i.Amt;
                    } else if (i.VendorID == "ตลาดไท") {
                        o.Vendor2Name = i.VendorName;
                        o.TotalInVendor2 = i.Amt;
                    } else {
                        o.Vendor3Name = i.VendorName;
                        o.TotalInVendor3 = i.Amt;
                    }
                    output.Add(o);
                } else {
                    if (i.VendorID == "SUP") {
                        o.Vendor1Name = i.VendorName;
                        o.TotalInVendor1 = i.Amt;
                    } else if (i.VendorID == "ตลาดไท") {
                        o.Vendor2Name = i.VendorName;
                        o.TotalInVendor2 = i.Amt;
                    } else {
                        o.Vendor3Name = i.VendorName;
                        o.TotalInVendor3 = i.Amt;
                    }
                }
          
    }
            foreach (var o in output) {
                o.TotalInVendor1=o.TotalInVendor1==null ? 0:o.TotalInVendor1;
                o.TotalInVendor2 = o.TotalInVendor2 == null ? 0 : o.TotalInVendor2;
                o.TotalInVendor3 = o.TotalInVendor3 == null ? 0 : o.TotalInVendor3;
                o.Total = o.TotalInVendor1 + o.TotalInVendor2 + o.TotalInVendor3;
            }

            return output;
        }
        public static List<PORequestSummaryByStoreInDetail> ListPOByStoreInDetail(DateTime podate, string rcom, string com,string VendorId,   int isPendingOnly) {
            List<PORequestSummaryByStoreInDetail> result = new List<PORequestSummaryByStoreInDetail>();
            try {
                string conStr = Globals.GAEntitiesConn; 

                using (var conn = new SqlConnection(conStr)) {
                    string sqlCmd = VQueryService.GetCommand("PODetailByStore");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("dateBegin", podate);
                    dynamicParameters.Add("dateEnd", podate);
                    dynamicParameters.Add("RComID", rcom);
                    dynamicParameters.Add("ComID", com);
                    dynamicParameters.Add("VendorId", VendorId);
                    dynamicParameters.Add("IsPendingOnly", isPendingOnly); 
                    result = conn.Query<PORequestSummaryByStoreInDetail>(sqlCmd, dynamicParameters).OrderBy(o => o.VendorName).ToList();
                }
            } catch (Exception ex) {

            }
            return result;
        }
        public static List<PORequestSummaryByItem> ListDataPOSumByItem(DateTime Begin, DateTime End, string rcom, string com, string item, int isPendingOnly) {
            List<PORequestSummaryByItem> result = new List<PORequestSummaryByItem>();
            try {
                string conStr = Globals.GAEntitiesConn;
                if (isPendingOnly == 1) {
                    //Begin = new DateTime(1999, 1, 1);
                    //End = new DateTime(2099, 12, 31);
                    Begin = DateTime.Now.Date.AddDays(-2);
                    End = DateTime.Now.Date;
                }
                using (var conn = new SqlConnection(conStr)) {
                    string sqlCmd = VQueryService.GetCommand("POSumByItem");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("dateBegin", Begin);
                    dynamicParameters.Add("dateEnd", End);
                    dynamicParameters.Add("RComID", rcom);
                    dynamicParameters.Add("ComID", com);
                    dynamicParameters.Add("IsPendingOnly", isPendingOnly);
                    result = conn.Query<PORequestSummaryByItem>(sqlCmd, dynamicParameters).OrderBy(o => o.ComID).ThenBy(o=>o.ItemName).ToList();
                }
            } catch (Exception ex) { 
            }
            return result;
        }


        public static List<SalesSummary> ListReportSalesSummaryl(string rcom, string com, DateTime dtbegin, DateTime dtend)
        {
            List<SalesSummary> result = new List<SalesSummary>();
            try
            {
                string conStr = Globals.GAEntitiesConn;

                using (var conn = new SqlConnection(conStr))
                {
                    string sqlCmd = VQueryService.GetCommand("SalesSummary");
                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("XrcomID", rcom);
                    dynamicParameters.Add("XcomID", com);
                    dynamicParameters.Add("XBegin", dtbegin);
                    dynamicParameters.Add("XEnd", dtend);
                    result = conn.Query<SalesSummary>(sqlCmd, dynamicParameters).OrderBy(o => o.BranchName).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

    }
}
