using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RobotAPI.Data.DA.Cims;
using RobotAPI.Data.DA.Portal;
using RobotAPI.Data.DataStoreDB.TT;
using RobotAPI.Data.ML.Portal;
using RobotAPI.Services.Api;
using System.Xml.Linq;

namespace RobotAPI.Controllers.DeptFirst {
    [Route("api/[controller]")]
    [ApiController]
    public class PortalController : ControllerBase {
        ClientService _clientService;
        PortalService _portalService;

        public PortalController(ClientService clientService, PortalService portalService) {
            _clientService = clientService;
            _portalService= portalService;
        }
        #region in portal
        [AllowAnonymous]
        [HttpGet("dpm003")]
        async public Task<ActionResult> dpm003(string? search = "") {
            //ข้อมูลจำนวนเจ้าหน้าที่ อปพร.
            DPM003DataSet.DocSet r = new DPM003DataSet.DocSet();
            try {
                r =await Task.Run(() => _portalService.GetDPM003(search)); 
            } catch (Exception ex) {
                r = null;
                BadRequest(r);
            } 
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm004")]
        async public Task<ActionResult> dpm004(string? search = "") {
            //ข้อมูลศูนย์พักพิงรายจังหวัด
            DPM004DataSet.DocSet r = new DPM004DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm004");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM004DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM004DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>

                                                         o.SHELTERID.ToLower().Contains(search)
                                                        || o.SHELTERNAME.ToLower().Contains(search)
                                                        || o.PROVINCECODE.ToLower().Contains(search)
                                                        || o.TAMBONCODE.ToLower().Contains(search)
                                                        || o.POSTALCODE.ToLower().Contains(search)
                                                        || o.OFFICER.ToLower().Contains(search)
                                                        || o.TEL.ToLower().Contains(search)
                                                        || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm005")]
        async public Task<ActionResult> dpm005(string? search = "") {
            //ข้อมูลพื้นที่ที่ได้รับการปกป้อง
            DPM005DataSet.DocSet r = new DPM005DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm005");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM005DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM005DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>

                                                         o.PLACENAME.ToLower().Contains(search)
                                                        || o.SUBPLACENAME.ToLower().Contains(search)
                                                        || o.PROVINCECODE.ToLower().Contains(search)
                                                        || o.TAMBONCODE.ToLower().Contains(search)
                                                        || o.POSTALCODE.ToLower().Contains(search)
                                                        || o.NAME.ToLower().Contains(search)
                                                        || o.ADDRESS.ToLower().Contains(search)
                                                        || o.TEL.ToLower().Contains(search)
                                                        || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm006")]
        async public Task<ActionResult> dpm006(string? search = "") {
            //ข้อมูลจำนวนผู้เชี่ยวชาญในหน่วยงาน
            DPM006DataSet.DocSet r = new DPM006DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm006");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM006DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM006DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>

                                                         o.ORGANIZATIONID.ToLower().Contains(search)
                                                        || o.ORGANIZATIONNAME.ToLower().Contains(search)
                                                        || o.PROVINCECODE.ToLower().Contains(search)
                                                        || o.TEL.ToLower().Contains(search)
                                                        || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm008")]
        async public Task<ActionResult> dpm008(string? search = "") {
            //ข้อมูลศูนย์ป้องกันและบรรเทาสาธารณภัยเขต สานักงานป้องกันและบรรเทาสาธารณภัยจังหวัด
            DPM008DataSet.DocSet r = new DPM008DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm008");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM008DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM008DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.PROVINCEID.ToLower().Contains(search)
                                                         || o.PROVINCENAME.ToLower().Contains(search)
                                                         || o.NAME.ToLower().Contains(search)
                                                         || o.TEL.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm009")]
        async public Task<ActionResult> dpm009(string? search = "") {
            //ข้อมูลรายงานสาธารณภัย ย้อนหลัง 7 วัน
            DPM009DataSet.DocSet r = new DPM009DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm009");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM009DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM009DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.DISASTER_CODE.ToLower().Contains(search)
                                                         || o.CYEAR.ToLower().Contains(search)
                                                         || o.ISSUES.ToLower().Contains(search)
                                                         || o.PROVINCE.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm010")]
        async public Task<ActionResult> dpm010(string? search = "") {
            //รายชื่อพื้นที่ จำนวนหมู่บ้าน เกิดสถานการณ์ย้อนหลัง 24 ชั่วโมง
            DPM010DataSet.DocSet r = new DPM010DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm010");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM010DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM010DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.ANNOUNCECODE.ToLower().Contains(search)
                                                         || o.PROVINCE_NAME.ToLower().Contains(search)
                                                         || o.AMPHUR_NAME.ToLower().Contains(search)
                                                         || o.TAMBOL_NAME.ToLower().Contains(search)
                                                         || o.STATUS_NAME.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm011")]
        async public Task<ActionResult> dpm011(string? search = "") {
            //รายชื่อพื้นที่ จำนวนชุมชน เกิดสถานการณ์ย้อนหลัง 24 ชั่วโมง
            DPM011DataSet.DocSet r = new DPM011DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm011");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM011DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM011DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.ANNOUNCECODE.ToLower().Contains(search)
                                                         || o.PROVINCE_NAME.ToLower().Contains(search)
                                                         || o.AMPHUR_NAME.ToLower().Contains(search)
                                                         || o.ORG_FNAME.ToLower().Contains(search)
                                                         || o.STATUS_NAME.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm012")]
        async public Task<ActionResult> dpm012(string? search = "") {
            //ข้อมูลการประกาศพื้นที่เฝ้าระวัง
            DPM012DataSet.DocSet r = new DPM012DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm012");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM012DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM012DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.DISASTER_NAME.ToLower().Contains(search)
                                                         || o.PROVINCE_CODE.ToLower().Contains(search)
                                                         || o.STATUS_NAME.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm013")]
        async public Task<ActionResult> dpm013(string? search = "") {
            //รายชื่อหมู่บ้านที่ประกาศภัย
            DPM013DataSet.DocSet r = new DPM013DataSet.DocSet();
            try {
                r =await Task.Run(()=> _portalService.GetDPM013(search));

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm014")]
        async public Task<ActionResult> dpm014(string? search = "") {
            //รายชื่อชุมชนที่ประกาศภัย
            DPM014DataSet.DocSet r = new DPM014DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm014");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM014DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM014DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.ANNOUNCECODE.ToLower().Contains(search)
                                                         || o.PROVINCE_NAME.ToLower().Contains(search)
                                                         || o.AMPHUR_NAME.ToLower().Contains(search)
                                                         || o.ORG_FNAME.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm015")]
        async public Task<ActionResult> dpm015(string? search = "") {
            //ข้อมูลการประกาศภัยสำหรับ DLA
            DPM015DataSet.DocSet r = new DPM015DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm015");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM015DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM015DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.ANNOUNCECODE.ToLower().Contains(search)
                                                         || o.TYPE_DISASTER_NAME.ToLower().Contains(search)
                                                         || o.PROVINCE_NAME.ToLower().Contains(search)
                                                         || o.AMPHUR_NAME.ToLower().Contains(search)
                                                         || o.ORG_FNAME.ToLower().Contains(search)
                                                         || o.STATUS_NAME.ToLower().Contains(search)
                                                         || o.COMMUNITY_NAME.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm016")]
        async public Task<ActionResult> dpm016(string? search = "") {
            //ข้อมูลจำนวนอาสาสมัคร ป้องกันภัยฝ่ายพลเรือน
            DPM016DataSet.DocSet r = new DPM016DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm016");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM016DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM016DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.GROUP_NAME.ToLower().Contains(search)
                                                         || o.PROVINCE_NAME.ToLower().Contains(search)
                                                         || o.TAMBOL_NAME.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }



        [AllowAnonymous]
        [HttpGet("dpm018")]
        async public Task<ActionResult> dpm018(string? search = "") {
            //ข้อมูลเครื่องจักรกล
            DPM018DataSet.DocSet r = new DPM018DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("dpm018");

                //string url = @"http://portal.disaster.go.th/portal/wsjson?queryCode=DPM018&user=xws-0068&password=364182df";
                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<DPM018DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (DPM018DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                        o.M_TYPE_NAME_TH.ToLower().Contains(search)
                                                        || o.M_TYPE_NAME_TH.ToLower().Contains(search)
                                                        || o.M_TYPE_CODE.ToLower().Contains(search)
                                                        || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("dpm019")]
        async public Task<ActionResult> dpm019(string? search = "") {
            //ข้อมูลศูนย์พักพิงทั่วประเทศ
            DPM019DataSet.DocSet r = new DPM019DataSet.DocSet();
            try {
                r = await Task.Run(() => _portalService.GetDPM019(search));
            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }
        [AllowAnonymous]
        [HttpGet("dpm020")]
        async public Task<ActionResult?> dpm020(string? pname,string? aname,string? tname ) {
            //ข้อมูลเครื่องจักรกล
            DPM020DataSet.DocSet? r = new DPM020DataSet.DocSet();
           
            try {
             r= await Task.Run(()=>   _portalService. GetDPM020(pname, aname, tname)); 
                //var conn = ApiConnService.GetApiInfo("dpm020");

                ////string url = @"http://portal.disaster.go.th/portal/wsjson?queryCode=DPM018&user=xws-0068&password=364182df";
                //string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                //var responds = await _clientService.GetAllAsync<DPM020DataSet.DocSet>(url, conn.source_api_token);
                //if (responds.StatusCode.ToString().ToUpper() != "OK") {
                //    BadRequest(r);
                //} else {
                //    var q = (DPM020DataSet.DocSet)responds.Result;
                //    #region search
                //    r.rows = q.rows.Where(o =>
                //                                        (o.PROVINCE_NAME == pname  || pname=="")
                //                                       && (o.AMPHUR_NAME  == aname || aname == "")
                //                                       && (o.TUMBOL_NAME == tname || tname == "")
                //                                      ).ToList();
                //    #endregion
                //    Ok(r);
                //}

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }
        [AllowAnonymous]
        [HttpGet("est006")]
        async public Task<ActionResult> est006(string? search = "") {
            //ข้อมูลรายละเอียดการประกาศภัยเพิ่มเติม
            EST006DataSet.DocSet r = new EST006DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("est006");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<EST006DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (EST006DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.DISASTER_ID.ToLower().Contains(search)
                                                         || o.AMPHUR_NAME.ToLower().Contains(search)
                                                         || o.TAMBON_NAME.ToLower().Contains(search)
                                                         || o.MOOBAN_ID.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("est007")]
        async public Task<ActionResult> est007(string? search = "") {
            //ข้อมูลสิ่งของสำรองจ่ายจำแนกรายหมวดสิ่งของ ตามจังหวัด
            EST007DataSet.DocSet r = new EST007DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("est007");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<EST007DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (EST007DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.STUFF_NAME.ToLower().Contains(search)
                                                         || o.LOCATE_NAME.ToLower().Contains(search)
                                                         || o.STUFF_ID.ToLower().Contains(search)
                                                         || o.CENTER_ID.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("est008")]
        async public Task<ActionResult> est008(string? search = "") {
            //ข้อมูลสิ่งของสำรองจ่ายจำแนกรายหมวดสิ่งของ ตามจังหวัด
            EST008DataSet.DocSet r = new EST008DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("est008");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<EST008DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (EST008DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.STUFF_NAME.ToLower().Contains(search)
                                                         || o.LOCATE_NAME.ToLower().Contains(search)
                                                         || o.STUFF_ID.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        [AllowAnonymous]
        [HttpGet("nrd001")]
        async public Task<ActionResult> nrd001(string? search = "") {
            //ข้อมูลความจำเป็นพื้นฐานรายชื่อหมู่บ้าน
            NRD001DataSet.DocSet r = new NRD001DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("nrd001");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<NRD001DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (NRD001DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.TAMBON_NAME.ToLower().Contains(search)
                                                         || o.MOOBAN_ID.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        #endregion

        #region not in portal

        [AllowAnonymous]
        [HttpGet("cvl004")]
        async public Task<ActionResult> cvl004(string? search = "") {
            //ข้อมูลจำนวนมิสเตอร์เตือนภัย จำแนกตามศูนย์
            CVL004DataSet.DocSet r = new CVL004DataSet.DocSet();
            try {
                search = search == null ? "" : search.ToLower();
                var conn = ApiConnService.GetApiInfo("cvl004");

                string url = $"{conn.source_base_url}{conn.source_api_url}&user={conn.source_api_username}&password={conn.source_api_password}";
                var responds = await _clientService.GetAllAsync<CVL004DataSet.DocSet>(url, conn.source_api_token);
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    BadRequest(r);
                } else {
                    var q = (CVL004DataSet.DocSet)responds.Result;
                    #region search
                    r.rows = q.rows.Where(o =>
                                                         o.CENTER_ID.ToLower().Contains(search)
                                                         || o.CENTER_NAME.ToLower().Contains(search)
                                                         || o.CENTER_SHORT_NAME.ToLower().Contains(search)
                                                         || search == ""
                                                      ).ToList();
                    #endregion
                    Ok(r);
                }

            } catch (Exception ex) {
                BadRequest(r);
            }
            return Ok(r);
        }

        #endregion


    }
}
