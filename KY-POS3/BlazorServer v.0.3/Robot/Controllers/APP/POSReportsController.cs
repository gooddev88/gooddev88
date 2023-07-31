using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Controllers.APP
{
    [Route("api/[controller]")]
    [ApiController]
    public class POSReportsController : ControllerBase
    {
        [HttpPost("POS133")]
        public SP_RptPOS133_Result POS133([FromBody] ReportFilter filter)
        {//รายงานสรุปยอดขาย
            SP_RptPOS133_Result result = new SP_RptPOS133_Result();
            try
            {
                using (var connection = new SqlConnection(Globals.GAEntitiesConn))
                {
                    var dynamicParameters = new DynamicParameters();
                    var values = new { rcomID = filter.RComID, comID = filter.ComID, Begin = filter.DateBegin, End = filter.DateEnd };
                    string strSQL = "exec [dbo].[SP_RptPOS133] @rcomID , @comID, @Begin, @End";
                    result = connection.Query<SP_RptPOS133_Result>(strSQL, values).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        [HttpPost("POS134")]
        public List<SP_RptPOS134Result> POS134([FromBody] ReportFilter filter)
        {//รายงานสรุปยอดขาย
            List<SP_RptPOS134Result> result = new List<SP_RptPOS134Result>();
            try
            {
                using (var connection = new SqlConnection(Globals.GAEntitiesConn))
                {
                    var dynamicParameters = new DynamicParameters();
                    var values = new { rcomID = filter.RComID, comID = filter.ComID, Begin = filter.DateBegin, End = filter.DateEnd };
                    string strSQL = "exec [dbo].[SP_RptPOS134] @rcomID , @comID, @Begin, @End";
                    result = connection.Query<SP_RptPOS134Result>(strSQL, values).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public class ReportFilter
        {
            public string RComID { get; set; }
            public string ComID { get; set; }

            public DateTime DateBegin { get; set; }
            public DateTime DateEnd { get; set; }
        }
    }
}
