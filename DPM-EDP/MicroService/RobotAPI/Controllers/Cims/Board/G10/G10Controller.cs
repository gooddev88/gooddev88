using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RobotAPI.Data.CimsDB.TT;
using RobotAPI.Data.DA.Accident;
using RobotAPI.Data.DA.Cims.Board.G10;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.ML.Board.Widget;
using RobotAPI.Models.Accident;


namespace RobotAPI.Controllers.Cims.Board.G10 {
  
    [Route("api/edp/group/[controller]")]
    [ApiController]
    public class G10Controller : ControllerBase {
         

        #region  G10 
        // จำนวนอุบัติเหตสูงสุด 3 จังหวัด 
        [AllowAnonymous]
        [HttpGet("WG101Data")]
        public List<WG101Data> WG101Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG101(board_id, datebegin, dateend, province);

        }

        // จังหวัดที่มีผู้เสียชีวิตสูงสุด 3 อันดับ
        [AllowAnonymous]
        [HttpGet("WG102Data")]
        public List<WG102Data> WG102Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG102(board_id, datebegin, dateend, province);
        }

         
        // จำนวนอุบัติเหตสูงสุด 3 จังหวัด 
        [AllowAnonymous]
        [HttpGet("WG103Data")]
        public List<WG103Data> WG103Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG103(board_id, datebegin, dateend, province); 
        }

      //จำนวนอุบัติเหตุแยกตามสถานะภาพผู้ประสบเหตุ
        [AllowAnonymous]
        [HttpGet("WG104Data")]
        public List<WG104Data> WG104Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG104(board_id, datebegin, dateend, province);

        }

        //จำนวนอุบัติเหตุทางถนนตามเดือน
        [AllowAnonymous]
        [HttpGet("WG105Data")]
        public List<WG105Data> WG105Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG105(board_id, datebegin, dateend, province);

        }

        //จำนวนผู้เสียชีวิตจากอุบัติเหตุทางถนน 
        [AllowAnonymous]
        [HttpGet("WG106Data")]
        public List<WG106Data> WG106Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG106(board_id, datebegin, dateend, province);
        }

        // จำนวนอุบัติเหตุตามช่วงอายุ 
        [AllowAnonymous]
        [HttpGet("WG107Data")]
        public List<WG107Data> WG107Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG107(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามเพศและช่วงอายุ
        [AllowAnonymous]
        [HttpGet("WG108Data")]
        public List<WG108Data> WG108Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG108(board_id, datebegin, dateend, province);
        }

        //จำนวนอุบัติเหตุแยกตามประเภทยานพาหนะ
        [AllowAnonymous]
        [HttpGet("WG109Data")]
        public List<WG109Data> WG109Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG109(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามสาเหตุการเกิดอุบัติเหตุ
        [AllowAnonymous]
        [HttpGet("WG110Data")]
        public List<WG110Data> WG110Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG110(board_id, datebegin, dateend, province);
        }
        //จังหวัดที่มีผู้บาดเจ็บสูงสุด 3 อันดับ
        [AllowAnonymous]
        [HttpGet("WG111Data")]
        public List<WG111Data> WG111Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG111(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามประเภทถนน
        [AllowAnonymous]
        [HttpGet("WG112Data")]
        public List<WG112Data> WG112Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG112(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามประเภทผิวจราจร
        [AllowAnonymous]
        [HttpGet("WG113Data")]
        public List<WG113Data> WG113Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG113(board_id, datebegin, dateend, province);
        }
        //จุดเกิดอุบัติเหตุ (ละติจูด ลองจิจูด)
        [AllowAnonymous]
        [HttpGet("WG114Data")]
        public List<WG114Data> WG114Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG114(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามประเภทจุดเกิดเหตุ 
        [AllowAnonymous]
        [HttpGet("WG115Data")]
        public List<WG115Data> WG115Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG115(board_id, datebegin, dateend, province);
        }
        //จุดเกิดอุบัติเหตุ (ละติจูด ลองจิจูด)
        [AllowAnonymous]
        [HttpGet("WG116Data")]
        public List<WG116Data> WG116Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG116(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามสถานที่เสียชีวิต 
        [AllowAnonymous]
        [HttpGet("WG117Data")]
        public List<WG117Data> WG117Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG117(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามประเภทผู้นำส่ง 
        [AllowAnonymous]
        [HttpGet("WG118Data")]
        public List<WG118Data> WG118Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG118(board_id, datebegin, dateend, province);
        }
        //จำนวนอุบัติเหตุแยกตามมูลค่าความเสียหาย
        [AllowAnonymous]
        [HttpGet("WG119Data")]
        public List<WG119Data> WG119Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG119(board_id, datebegin, dateend, province);
        }
        //จำนวนผู้บาดเจ็บจากอุบัติเหตุทางถนน
        [AllowAnonymous]
        [HttpGet("WG120Data")]
        public List<WG120Data> WG120Data(string? board_id, string? datebegin, string? dateend, string? province) {
            return G10Service.GetWG120(board_id, datebegin, dateend, province);
        }
        #endregion
        #region G20
        //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด  
        [AllowAnonymous]
        [HttpGet("WG201Data")]
        public List<WG201Data> WG201Data(string? board_id, string? province) {
            return G10Service.GetWG201(board_id, province);
        }
        //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด  
        [AllowAnonymous]
        [HttpGet("WG202Data")]
        public List<WG202Data> WG202Data(string? board_id, string? unit) {
            return G10Service.GetWG202(board_id, unit);
        }
        #endregion
        #region G30
        //จำนวน อปพร. แยกตามเพศ 
        [AllowAnonymous]
        [HttpGet("WG301Data")]
        public List<WG301Data> WG301Data(string? board_id,string? province ) {
            return G10Service.GetWG301(board_id,  province);
        }
        //จำนวน อปพร. แยกตามอายุ เพศ 
        [AllowAnonymous]
        [HttpGet("WG302Data")]
        public List<WG302Data> WG302Data(string? board_id, string? province) {
            return G10Service.GetWG302(board_id, province);
        }
        //จำนวนปอพร. แยกตามภูมิภาค
        [AllowAnonymous]
        [HttpGet("WG303Data")]
        public List<WG303Data> WG303Data(string? board_id) {
            return G10Service.GetWG303(board_id);
        }
        //จำนวน อปพร. แบ่งตามจำนวนหลักสูตรที่เข้าอบรม
        [AllowAnonymous]
        [HttpGet("WG304Data")]
        public List<WG304Data> WG304Data(string? board_id) {
            return G10Service.GetWG304(board_id);
        }
        //จำนวน อปพร. แบ่งตามปีประสบการณ์ 
        [AllowAnonymous]
        [HttpGet("WG305Data")]
        public List<WG305Data> WG305Data(string? board_id, string? province) {
            return G10Service.GetWG305(board_id, province);
        }
        #endregion
        #region G40
        //ภาพรวมสถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด
        [AllowAnonymous]
        [HttpGet("WG401Data")]
        public List<WG401Data> WG401Data(string? board_id, string? province) {
            return G10Service.GetWG401(board_id, province);
        }
        //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด
        [AllowAnonymous]
        [HttpGet("WG402Data")]
        public List<WG402Data> WG402Data(string? board_id, string? province) {
            return G10Service.GetWG402(board_id, province);
        }
        //ภาพรวมสถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด 
        [AllowAnonymous]
        [HttpGet("WG403Data")]
        public List<WG403Data> WG403Data(string? board_id, string? province) {
            return G10Service.GetWG403(board_id, province);
        }
       //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามศูนย์
        [AllowAnonymous]
        [HttpGet("WG404Data")]
        public List<WG404Data> WG404Data(string? board_id ) {
            return G10Service.GetWG404(board_id);
        }

        #endregion
        #region G50
        //จำนวนการรายงานการเกิดภัยพิบัติ แบ่งตามประเภทภัยพิบัติ
        [AllowAnonymous]
        [HttpGet("WG507Data")]
        public List<WG507Data> WG507Data(string? board_id, string? datebegin, string? dateend ) {
            return G10Service.GetWG507(board_id, datebegin, dateend);
        }
        #endregion


        #region ใช้งานวันนี้
        ///// <summary>
        ///// ช่วงอายุที่ประสบอุบัติเหตุ
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetAccindentPatiesAgeRangeSet")]
        //public List<AccindentPatiesAgeRange> GetAccindentPatiesAgeRange([FromBody] AccidentCountSetParam input) {
        //    List<AccindentPatiesAgeRange> result = new List<AccindentPatiesAgeRange>();
        //    try {
        //        result = AccidentDashboardService.GetAccindentPatiesAgeRange(input);
        //    } catch {
        //    }
        //    return result;
        //}
        //[AllowAnonymous]
        //[HttpPost("GetAccidentCounSet")]
        //public AccidentCountSetResult GetAccidentCounSet([FromBody] AccidentCountSetParam input) {
        //    AccidentCountSetResult result = new AccidentCountSetResult();
        //    try {
        //        result = AccidentDashboardService.AccidentCountSet(input);
        //    } catch {
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// พฤติกรรมที่ก่อให้เกิดอุบัติเหตุ
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetAccindentBehaviors")]
        //public List<AccindentBehaviors> GetAccindentBehaviors([FromBody] AccidentCountSetParam input) {
        //    List<AccindentBehaviors> result = new List<AccindentBehaviors>();
        //    try {
        //        result = AccidentDashboardService.GetAccindentBehaviors(input);
        //    } catch {
        //    }
        //    return result;
        //}
        ///// <summary>
        ///// ชนิดเส้นทางงานเกิดอุบัติเหตุ
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetAccindentRoadTypeSet")]
        //public List<AccindentRoadTypeSet> GetAccindentRoadTypeSet([FromBody] AccidentCountSetParam input) {
        //    List<AccindentRoadTypeSet> result = new List<AccindentRoadTypeSet>();
        //    try {
        //        result = AccidentDashboardService.GetAccindentRoadTypeSet(input);
        //    } catch {
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// สาเหตุที่ทำให้เกิดอุบัติเหตุ
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetAccidentCause")]
        //public List<AccidentCauseSetResult> GetAccidentCause([FromBody] AccidentCountSetParam input) {
        //    List<AccidentCauseSetResult> result = new List<AccidentCauseSetResult>();
        //    try {
        //        result = AccidentDashboardService.GetAccidentCause(input);
        //    } catch {
        //    }
        //    return result;
        //}
        ///// <summary>
        ///// จำนวนผู้เสียชีวิต บาดเจ็บ และจำนวนอุบัติเหตุในแต่ละจังหวัด
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetEventInProvince")]
        //public List<EventInProvince> GetEventInProvince([FromBody] AccidentCountSetParam input) {
        //    List<EventInProvince> result = new List<EventInProvince>();
        //    try {
        //        result = AccidentDashboardService.GetEventInProvince(input);
        //    } catch {
        //    }
        //    return result;
        //}


        ///// <summary>
        ///// ช่วงเวลาที่เกิดอุบัติเหตุ
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetAccindentTimeRngSet")]
        //public List<AccindentTimeRngSet> GetAccindentTimeRngSet([FromBody] AccidentCountSetParam input) {
        //    List<AccindentTimeRngSet> result = new List<AccindentTimeRngSet>();
        //    try {
        //        result = AccidentDashboardService.GetAccindentTimeRngSet(input);
        //    } catch {
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// ประเภทย่อยของยานพาหนะที่ประสบอุบัติเหตุ
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetAccindentVehicleSubtypeSet")]
        //public List<AccindentVehicleSubtype> GetAccindentVehicleSubtypeSet([FromBody] AccidentCountSetParam input) {
        //    List<AccindentVehicleSubtype> result = new List<AccindentVehicleSubtype>();
        //    try {
        //        result = AccidentDashboardService.GetAccindentVehicleSubtypeSet(input);
        //    } catch {
        //    }
        //    return result;
        //}


        ///// <summary>
        ///// จุดเกิดเหตุตาม ตำบล อำเภอ จังหวัด
        ///// </summary>
        //[AllowAnonymous]
        //[HttpPost("GetAccindentLocationSet")]
        //public List<AccindentLocationSet> GetAccindentLocationSet([FromBody] AccidentCountSetParam input) {
        //    List<AccindentLocationSet> result = new List<AccindentLocationSet>();
        //    try {
        //        result = AccidentDashboardService.GetAccindentLocationSet(input);
        //    } catch {
        //    }
        //    return result;
        //}
        #endregion

    }
}
