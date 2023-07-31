using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Accident;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Models.Accident;


namespace RobotAPI.Controllers.Accident {
    [Route("api/cims/[controller]")]
    [ApiController]
    public class G10Controller : ControllerBase {


        #region ใช้งานวันนี้
        /// <summary>
        /// ช่วงอายุที่ประสบอุบัติเหตุ
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccindentPatiesAgeRangeSet")]
        public List<AccindentPatiesAgeRange> GetAccindentPatiesAgeRange([FromBody] AccidentCountSetParam input) {
            List<AccindentPatiesAgeRange> result = new List<AccindentPatiesAgeRange>();
            try {
                result = AccidentDashboardService.GetAccindentPatiesAgeRange(input);
            } catch {
            }
            return result;
        }
        [AllowAnonymous]
        [HttpPost("GetAccidentCounSet")]
        public AccidentCountSetResult GetAccidentCounSet([FromBody] AccidentCountSetParam input) {
            AccidentCountSetResult result = new AccidentCountSetResult();
            try {
                result = AccidentDashboardService.AccidentCountSet(input);
            } catch {
            }
            return result;
        }

        /// <summary>
        /// พฤติกรรมที่ก่อให้เกิดอุบัติเหตุ
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccindentBehaviors")]
        public List<AccindentBehaviors> GetAccindentBehaviors([FromBody] AccidentCountSetParam input) {
            List<AccindentBehaviors> result = new List<AccindentBehaviors>();
            try {
                result = AccidentDashboardService.GetAccindentBehaviors(input);
            } catch {
            }
            return result;
        }
        /// <summary>
        /// ชนิดเส้นทางงานเกิดอุบัติเหตุ
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccindentRoadTypeSet")]
        public List<AccindentRoadTypeSet> GetAccindentRoadTypeSet([FromBody] AccidentCountSetParam input) {
            List<AccindentRoadTypeSet> result = new List<AccindentRoadTypeSet>();
            try {
                result = AccidentDashboardService.GetAccindentRoadTypeSet(input);
            } catch {
            }
            return result;
        }

        /// <summary>
        /// สาเหตุที่ทำให้เกิดอุบัติเหตุ
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccidentCause")]
        public List<AccidentCauseSetResult> GetAccidentCause([FromBody] AccidentCountSetParam input) {
            List<AccidentCauseSetResult> result = new List<AccidentCauseSetResult>();
            try {
                result = AccidentDashboardService.GetAccidentCause(input);
            } catch {
            }
            return result;
        }
        /// <summary>
        /// จำนวนผู้เสียชีวิต บาดเจ็บ และจำนวนอุบัติเหตุในแต่ละจังหวัด
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetEventInProvince")]
        public List<EventInProvince> GetEventInProvince([FromBody] AccidentCountSetParam input) {
            List<EventInProvince> result = new List<EventInProvince>();
            try {
                result = AccidentDashboardService.GetEventInProvince(input);
            } catch {
            }
            return result;
        }


        /// <summary>
        /// ช่วงเวลาที่เกิดอุบัติเหตุ
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccindentTimeRngSet")]
        public List<AccindentTimeRngSet> GetAccindentTimeRngSet([FromBody] AccidentCountSetParam input) {
            List<AccindentTimeRngSet> result = new List<AccindentTimeRngSet>();
            try {
                result = AccidentDashboardService.GetAccindentTimeRngSet(input);
            } catch {
            }
            return result;
        }

        /// <summary>
        /// ประเภทย่อยของยานพาหนะที่ประสบอุบัติเหตุ
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccindentVehicleSubtypeSet")]
        public List<AccindentVehicleSubtype> GetAccindentVehicleSubtypeSet([FromBody] AccidentCountSetParam input) {
            List<AccindentVehicleSubtype> result = new List<AccindentVehicleSubtype>();
            try {
                result = AccidentDashboardService.GetAccindentVehicleSubtypeSet(input);
            } catch {
            }
            return result;
        }


        /// <summary>
        /// จุดเกิดเหตุตาม ตำบล อำเภอ จังหวัด
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccindentLocationSet")]
        public List<AccindentLocationSet> GetAccindentLocationSet([FromBody] AccidentCountSetParam input) {
            List<AccindentLocationSet> result = new List<AccindentLocationSet>();
            try {
                result = AccidentDashboardService.GetAccindentLocationSet(input);
            } catch {
            }
            return result;
        }
        #endregion

         








        #region ในนี้อนาคตจะลบทิ้งนะจ้าาา
        [AllowAnonymous]
        [HttpPost("TopProvinceHasAccident")]
        public List<AccidentCountProvinceResult> TopProvinceHasAccident([FromBody] AccidentCountProvinceParam input) {
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
        [AllowAnonymous]
        [HttpPost("CountType")]
        public List<AccidentCountResult> CountType([FromBody] AccidentCountParam input) {

            List<AccidentCountResult> result = new List<AccidentCountResult>();
            try {
                result = AccidentDashboardService.GetCountType(input.DateBegin, input.DateEnd);

            } catch {

            }

            return result;
        }
        [AllowAnonymous]
        [HttpPost("CountEvnetInHour")]
        public List<AccidentCountResult> CountEvnetInHour([FromBody] AccidentCountParam input) {

            List<AccidentCountResult> result = new List<AccidentCountResult>();
            try {
                result = AccidentDashboardService.GetCountEventInEachHour(input.DateBegin, input.DateEnd);

            } catch {

            }

            return result;
        }


        /// <summary>
        /// แสดง record แบบดิบเต็ม record
        /// </summary>
        [AllowAnonymous]
        [HttpPost("GetAccindentAllSet")]
        public List<accident_data> GetAccindentAllSet([FromBody] AccidentCountSetParam input) {
            List<accident_data> result = new List<accident_data>();
            try {
                result = AccidentDashboardService.GetAccindentAllSet(input);
            } catch {
            }
            return result;
        }

        #endregion

         

    }
}
