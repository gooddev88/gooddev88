using RobotWasm.Client.Service.Api;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.DPMBaord.BoardData.Widget.G10;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RobotWasm.Client.Data.DA.Board.G10 {
    public class G10Service {

        private HttpClient _http;
        private ClientService _clientService;
        public G10Service(HttpClient http, ClientService clientService) {
            _http = http;
            _clientService = clientService;
        }


        #region wg10
        async public Task<List<WG101Data>> GetWG101Data(string board_id) {
            //จังหวัดที่เกิดอุบัติเหตุสูงสุด 3 ลำดับ
            List<WG101Data>? output = new List<WG101Data>();
            try { 
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG101Data>>($"{Globals.BaseURL}/api/edp/group/g10/wg101data?board_id={board_id}"));
                output = (List<WG101Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG102Data>> GetWG102Data(string board_id) {
            //จังหวัดที่มีผู้เสียชีวิตสูงสุด 3 อันดับ
            List<WG102Data>? output = new List<WG102Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg102data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG102Data>>(url));
                output = (List<WG102Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }

        async public Task<List<WG103Data>> GetWG103Data(string board_id) {      
            //จำนวนอุบัติเหตุแยกรายชั่วโมง
            List<WG103Data>? output = new List<WG103Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg103data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG103Data>>(url));
                output = (List<WG103Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }

        async public Task<List<WG104Data>> GetWG104Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามสถานภาพผู้ประสบเหตุ
            List<WG104Data>? output = new List<WG104Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg104data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG104Data>>(url));
                output = (List<WG104Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG105Data>> GetWG105Data(string board_id) {
            //จำนวนอุบัติเหตุทางถนนตามเดือน
            List<WG105Data>? output = new List<WG105Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg105data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG105Data>>(url));
                output = (List<WG105Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG106Data>> GetWG106Data(string board_id) {
            //จำนวนผู้เสียชีวิตจากอุบัติเหตุทางถนน
            List<WG106Data>? output = new List<WG106Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg106data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG106Data>>(url));
                output = (List<WG106Data>)query.Result;
            } catch (Exception ex) {
            }
         //   output = WG106Data.GetResultFromAPI();
            return output;
        }

     async public Task<List<WG107Data>> GetWG107Data(string board_id) {
            //จำนวนอุบัติเหตุตามช่วงอายุ
            List<WG107Data>? output = new List<WG107Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg107data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG107Data>>(url));
                output = (List<WG107Data>)query.Result;
            } catch (Exception ex) {
            }
        
            return output;
        }

        async public Task<List<WG108Data>> GetWG108Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามเพศและช่วงอายุ
            List<WG108Data>? output = new List<WG108Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg108data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG108Data>>(url));
                output = (List<WG108Data>)query.Result;
            } catch (Exception ex) {
            } 
            return output;
        }
        async public Task<List<WG109Data>> GetWG109Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามประเภทยานพาหนะ
            List<WG109Data>? output = new List<WG109Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg109data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG109Data>>(url));
                output = (List<WG109Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG110Data>> GetWG110Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามสาเหตุการเกิดอุบัติเหตุ
            List<WG110Data>? output = new List<WG110Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg110data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG110Data>>(url));
                output = (List<WG110Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG111Data>> GetWG111Data(string board_id) {
            //จังหวัดที่มีผู้บาดเจ็บสูงสุด 3 อันดับ
            List<WG111Data>? output = new List<WG111Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg111data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG111Data>>(url));
                output = (List<WG111Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG112Data>> GetWG112Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามประเภทถนน
            List<WG112Data>? output = new List<WG112Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg112data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG112Data>>(url));
                output = (List<WG112Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG113Data>> GetWG113Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามประเภทผิวจราจร
            List<WG113Data>? output = new List<WG113Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg113data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG113Data>>(url));
                output = (List<WG113Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG114Data>> GetWG114Data(string board_id) {
            //จุดเกิดอุบัติเหตุ (ละติจูด ลองจิจูด)
            List<WG114Data>? output = new List<WG114Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg114data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG114Data>>(url));
                output = (List<WG114Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG115Data>> GetWG115Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามประเภทจุดเกิดเหตุ 
            List<WG115Data>? output = new List<WG115Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg115data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG115Data>>(url));
                output = (List<WG115Data>)query.Result;
            } catch (Exception ex) {
            } 
            return output;
        }

        async public Task<List<WG116Data>> GetWG116Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามภูมิลำเนา
            List<WG116Data>? output = new List<WG116Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg116data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG116Data>>(url));
                output = (List<WG116Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }        async public Task<List<WG118Data>> GetWG118Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามประเภทผู้นำส่ง 
            List<WG118Data>? output = new List<WG118Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg118data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG118Data>>(url));
                output = (List<WG118Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG117Data>> GetWG117Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามสถานที่เสียชีวิต 
            List<WG117Data>? output = new List<WG117Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg117data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG117Data>>(url));
                output = (List<WG117Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG119Data>> GetWG119Data(string board_id) {
            //จำนวนอุบัติเหตุแยกตามมูลค่าความเสียหาย
            List<WG119Data>? output = new List<WG119Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg119data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG119Data>>(url));
                output = (List<WG119Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG120Data>> GetWG120Data(string board_id) {
            //จำนวนผู้บาดเจ็บจากอุบัติเหตุทางถนน
            List<WG120Data>? output = new List<WG120Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg120data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG120Data>>(url));
                output = (List<WG120Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }
        #endregion

        #region wg20
        async public Task<List<WG201Data>> GetWG201Data(string province) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามจังหวัด 

            List<WG201Data>? output = new List<WG201Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg201data?province={province}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG201Data>>(url));
                output = ((List<WG201Data>)query.Result);

            } catch (Exception ex) {
            }
            return output;
 
        }
        async public Task<WG202Data.DocSet?> GetWG202Data(string unit) {
            //จำนวนสิ่งของสำรองจ่าย แบ่งตามศูนย์

            WG202Data.DocSet? output = new WG202Data.DocSet();
            
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg202data?unit={unit}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG202Data.DataRow>>(url));
                output.rows = ((List<WG202Data.DataRow>)query.Result);

                 
                output.branchs = new List<WG202Data.Branch>();

                                   var results = output.rows.GroupBy(n => new { n.unit ,n.unit_code})
                                .Select(g => new {
                                    g.Key.unit,
                                    g.Key.unit_code}).ToList();
                foreach (var r in results) {
                    WG202Data.Branch n = new WG202Data.Branch();
                    n.CENTER_ID=r.unit_code;
                    n.LOCATE_NAME = r.unit;
                    output.branchs.Add(n);
                }


        } catch (Exception ex) {
            }
            return output;
        }
        #endregion
        #region wg30
        async public Task<List<WG301Data>> GetWG301Data() {
            //จำนวน อปพร. แยกตามเพศ

            List<WG301Data>? output = new List<WG301Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg301data";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG301Data>>(url));
                  output = ((List<WG301Data>)query.Result)   ;
              
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG302Data>> GetWG302Data() {
            //จำนวน อปพร. แยกตามอายุ เพศ

            List<WG302Data>? output = new List<WG302Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg302data";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG302Data>>(url));
                output = ((List<WG302Data>)query.Result);

            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG303Data>> GetWG303Data() {
            //จำนวนปอพร. แยกตามภูมิภาค 
            List<WG303Data>? output = new List<WG303Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg303data";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG303Data>>(url));
                output = ((List<WG303Data>)query.Result);

            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG304Data>> GetWG304Data() {
            //จำนวน อปพร. แบ่งตามจำนวนหลักสูตรที่เข้าอบรม 
            List<WG304Data>? output = new List<WG304Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg304data";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG304Data>>(url));
                output = ((List<WG304Data>)query.Result);
                output = output.OrderBy(o => o.course_code).ToList();
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG305Data>> GetWG305Data() {
            //จำนวน อปพร. แบ่งตามปีประสบการณ์ 
            List<WG305Data>? output = new List<WG305Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg305data";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG305Data>>(url));
                output = ((List<WG305Data>)query.Result);
                output = output.OrderBy(o => o.year_experience).ToList();
            } catch (Exception ex) {
            }
            return output;
        }
        #endregion


        #region wg40
        async public Task<List<WG401Data>> GetWG401Data(string province) {
            //ภาพรวมสถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด
            List<WG401Data>? output = new List<WG401Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg401data?province={province}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG401Data>>(url));
                output = ((List<WG401Data>)query.Result);

            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG402Data>> GetWG402Data() {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด 
            List<WG402Data>? output = new List<WG402Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg402data";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG402Data>>(url));
                output = ((List<WG402Data>)query.Result); 
            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG403Data>> GetWG403Data(string province) {
            //ภาพรวมสถานะเครื่องจักรกลสาธารณภัย แบ่งตามจังหวัด 
            List<WG403Data>? output = new List<WG403Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg403data?province={province}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG403Data>>(url));
                output = ((List<WG403Data>)query.Result);

            } catch (Exception ex) {
            }
            return output;
        }
        async public Task<List<WG404Data>> GetWG404Data() {
            //สถานะเครื่องจักรกลสาธารณภัย แบ่งตามศูนย์
             
            List<WG404Data>? output = new List<WG404Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg404data";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG404Data>>(url));
                output = ((List<WG404Data>)query.Result);

            } catch (Exception ex) {
            }
            return output;
        }
        #endregion

        #region wg50
        async public Task<List<WG507Data>> GetWG507Data(string board_id) {
            //จำนวนการรายงานการเกิดภัยพิบัติ แบ่งตามประเภทภัยพิบัติ
            List<WG507Data>? output = new List<WG507Data>();
            try {
                string url = $"{Globals.BaseURL}/api/edp/group/g10/wg507data?board_id={board_id}";
                var query = await Task.Run(() => _clientService.GetAllAsync<List<WG507Data>>(url));
                output = (List<WG507Data>)query.Result;
            } catch (Exception ex) {
            }
            return output;
        }

        #endregion


    }
}
