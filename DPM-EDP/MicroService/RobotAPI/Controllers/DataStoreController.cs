using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RobotAPI.Data;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Accident;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Models;
using RobotAPI.Models.Accident;
using System.Data;

namespace RobotAPI.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DataStoreController : ControllerBase {
        private readonly CIMSContext cimsContext;
        private readonly ConfigurationManager configuration;
        public DataStoreController(CIMSContext _dataStoreContext, ConfigurationManager _configuration) {
            cimsContext = _dataStoreContext;
            configuration = _configuration;
        }
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(Globals.CimsConn);
            }
        }

        //public List<SmlMrpTrans> getAll() {
        //    List<SmlMrpTrans> MrtTranData = new List<SmlMrpTrans>();
        //    try {
        //        using (IDbConnection dbConnection = Connection) {
        //            dbConnection.Open();
        //            string sqlCmdUser = string.Format("select doc_no,doc_date,doc_ref1,doc_ref2,status,is_cancel,total_value,wid_wh_code,wid_shelf_code,fg_wh_code,fg_shelf_code,total_amount,remark,remark_1,remark_2,remark_3,remark_4,remark_5,project_code,branch_code FROM mrp_trans Order by doc_no desc");
        //            MrtTranData = dbConnection.Query<SmlMrpTrans>(sqlCmdUser).ToList();
        //        }
        //    } catch (Exception) {

        //    }
        //    return MrtTranData;
        //}


        [AllowAnonymous]
        [HttpGet]
        [Route("GetAccidentAll")]
        public async Task<AccidentDataSet> GetAccidentAll()
        {
            List<accident_data> data = new List<accident_data>();
            AccidentDataSet accidentsSet = new AccidentDataSet();
            try
            {
                data = cimsContext.accident_data.ToList();
                accidentsSet.accident_data = data;
            }
            catch (Exception ex)
            {
                var rr = ex.Message;
            }
            return accidentsSet;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<accident_data>> DataStore() {
            List<accident_data> data = new List<accident_data>();
            DateTime being = new DateTime(2020, 2, 1);
            DateTime end = new DateTime(2020, 2, 28);
            try {
                data = cimsContext.accident_data.Where(o => o.accident_date <= being && o.accident_date >= end).ToList();
                // data = cimsContext.data_accidents.ToList();

            } catch (Exception ex) {

                var rr = ex.Message;
            }
            return data;
        }


        [AllowAnonymous]
        [HttpPost("GetTopProvinceHasAccident")]
        public List< AccidentCountProvinceResult> GetTopProvinceHasAccident([FromBody] AccidentCountProvinceParam input) {
//type
//most_event => อุบัติเหตสูงสุด
//most_deceased => เสียชีวิตสูงสุด
//most_injured => บาดเจ็บสูงสุด
            List< AccidentCountProvinceResult> result = new List< AccidentCountProvinceResult>();
            try {
                result = AccidentDashboardService.GetTopProvinceHasAccident(input.DateBegin, input.DateEnd, input.Top,input.Type);

            } catch {

            }

            return result;
        }
    }

    public class AccidentDataSet{
        public List<accident_data> accident_data { get;set; }
        }
}
