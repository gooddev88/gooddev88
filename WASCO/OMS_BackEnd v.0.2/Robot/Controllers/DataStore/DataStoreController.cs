using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Robot.Data.DA.DataStore;
using Robot.Data.DATASTOREDB.TT;
using Robot.Data.ML.DataStoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Controllers.DataStore {
    [Route("api/DataStore/[controller]")]
    [ApiController]
    public class DataStoreController : ControllerBase {
        private readonly DataStoreContext dataStoreContext;
        public DataStoreController(DataStoreContext _dataStoreContext) {
            dataStoreContext = _dataStoreContext;
        }
        //internal IDbConnection Connection {
        //    get {5
        //        return new NpgsqlConnection((Globals.DataStoreConn));
        //    }
        //}

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
        public async Task<AccidentDataSet> GetAccidentAll() {
            List<data_accidents> data = new List<data_accidents>();
            AccidentDataSet accidentsSet = new AccidentDataSet();
            try {
                data = dataStoreContext.data_accidents.ToList();
                accidentsSet.data_Accidents = data;
            } catch (Exception ex) {
                var rr = ex.Message;
            }
            return accidentsSet;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<data_accidents>> DataStore() {
            IEnumerable<data_accidents> data= null;
            DateTime being = new DateTime(2020, 2, 1);
            DateTime end = new DateTime(2020, 2, 28);
            try {
                data = dataStoreContext.data_accidents.Where(o => o.accident_date <= being && o.accident_date >= end).ToArray();
                // data = dataStoreContext.data_accidents.ToList();

            } catch (Exception ex) {

                var rr = ex.Message;
            }
            return data;
        }


        [AllowAnonymous]
        [HttpPost("GetTopProvinceHasAccident")]
        public List<AccidentCountProvinceResult> GetTopProvinceHasAccident([FromBody] AccidentCountProvinceParam input) {
            //type
            //most_event => อุบัติเหตสูงสุด
            //most_deceased => เสียชีวิตสูงสุด
            //most_injured => บาดเจ็บสูงสุด
            List<AccidentCountProvinceResult> result = new List<AccidentCountProvinceResult>();
            try {
                result = AccidentDashboardService.GetTopProvinceHasAccident(input.DateBegin, input.DateEnd, input.Top, input.Type);

            } catch {

            }

            return result;
        }
    }

    public class AccidentDataSet {
        public List<data_accidents> data_Accidents { get; set; }
    }
}
