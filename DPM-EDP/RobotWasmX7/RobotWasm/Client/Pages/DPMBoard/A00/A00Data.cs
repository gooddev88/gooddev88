using Microsoft.AspNetCore.Components;
using RobotWasm.Shared.Data.ML.DPMBaord.Accident;
using RobotWasm.Client.Service.Api;
using static RobotWasm.Shared.Data.ML.DPMBaord.Accident.AccidentParam;
using System.Text.Json;

namespace RobotWasm.Client.Pages.DPMBoard.A00 {
    public class A00Data {

        public DataSet doc = new DataSet();
        private ClientService _clientService { get; set; }
        private AccidentCountSetParam param { get; set; }
        public A00Data(ClientService clientService) {
            _clientService = clientService;
            doc = NewTransaction();
            param = NewParam();
        }
        #region  class
        public class DataSet {
            public List<AccindentPatiesAgeRange> AgeRange_Data { get; set; }//ช่วงอายุที่เกิดอุบัติเหตุ
            public AccidentCountSetResult Number_Data { get; set; }//จำนวนบาดเจ็บ ตาย และเหตุการณื
            public List<ChartData> DangerBehavior { get; set; }//พฤติกรรมให้เกิดเหตุ
            //public List<ChartData> MostTimeAccindent { get; set; }//ช่วงเวลาเกิดเหตุ
            public List<ChartData> RoadType { get; set; }//ลักษณะถนนที่เกิดเหตุ
            public List<ChartData> EventCause { get; set; }//สาเหตุการเกิดเหตุ
            public List<EventInProvince> EventInProvince { get; set; }//จังหวัดที่เกิดเหตุ
            public List<ChartData> TimeRange { get; set; }//ช่วงเวลาที่เกิดเหตุ
            public List<ChartData> Vehicle { get; set; }//ประเภทยานพาหนะที่เกิดเหตุ
            public List<AccindentLocationSet> EventLocation { get; set; }//จุดเกิดเหตุ

        }
        public class ChartData {
            public string X { get; set; }
            public double Y { get; set; }
        }

        #endregion

        async public Task<DataSet> GetData(DateTime DateBegin, DateTime DateEnd, List<string> Provinces) {
            try {
                param.DateBegin = DateBegin;
                param.DateEnd = DateEnd;
                param.Province = Provinces;
                await Task.Run(() => Get_AgeRange());
                await Task.Run(() => Get_Number());
                await Task.Run(() => Get_DangerBehavior()); 
                await Task.Run(() => Get_RoadType());
                await Task.Run(() => Get_EventCause());
                await Task.Run(() => Get_EventInProvince());
                await Task.Run(() => Get_TimeRange());
                await Task.Run(() => Get_Vehicle());
            await Task.Run(() => Get_EventLocation());

            } catch (Exception ex) {
            }
            return doc;
        }
         
        #region  get data
        async private Task Get_AgeRange() {
            try {
                doc.AgeRange_Data = new List<AccindentPatiesAgeRange>();
                var query = await Task.Run(() => _clientService.Post<List<AccindentPatiesAgeRange>>($"{Globals.BaseURL}/api/cims/accident/GetAccindentPatiesAgeRangeSet", param));
                doc.AgeRange_Data = (List<AccindentPatiesAgeRange>)query.Result;
            } catch (Exception ex) {
            }
        }
        async private Task Get_Number() {
            try {
                doc.Number_Data = new AccidentCountSetResult();
                var query = await _clientService.Post<AccidentCountSetResult>($"{Globals.BaseURL}/api/cims/accident/GetAccidentCounSet", param);
                doc.Number_Data = (AccidentCountSetResult)query.Result;
            } catch (Exception ex) {
            }
        }
        async private Task Get_DangerBehavior() {
            try {
                doc.DangerBehavior = new List<ChartData>();
                var query = await Task.Run(() => _clientService.Post<List<AccindentBehaviors>>($"{Globals.BaseURL}/api/cims/accident/GetAccindentBehaviors", param));
                var data_api = (List<AccindentBehaviors>)query.Result; 
                foreach (var d in data_api) {
                    ChartData n = new ChartData();
                    n.X = d.behavior;
                    n.Y = d.count_result;
                    doc.DangerBehavior.Add(n);
                }

            } catch (Exception ex) {
            }
        }

        async private Task Get_RoadType() {
            try {

                doc.RoadType = new List<ChartData>();
                var query = await Task.Run(() => _clientService.Post<List<AccindentRoadTypeSet>>($"{Globals.BaseURL}/api/cims/accident/GetAccindentRoadTypeSet", param));
                var data_api = (List<AccindentRoadTypeSet>)query.Result;
                foreach (var d in data_api) {
                    ChartData n = new ChartData();
                    n.X = d.road_type;
                    n.Y = d.Count_Result;
                    doc.RoadType.Add(n);
                }

            } catch (Exception ex) {
            }
        }
        async private Task Get_EventCause() {
            try {
                doc.EventCause = new List<ChartData>();
                var query = await Task.Run(() => _clientService.Post<List<AccidentCauseSetResult>>($"{Globals.BaseURL}/api/cims/accident/GetAccidentCause", param));
                var data_api = (List<AccidentCauseSetResult>)query.Result;
                foreach (var d in data_api) {
                    ChartData n = new ChartData();
                    n.X = d.cause;
                    n.Y = d.count_result;
                    doc.EventCause.Add(n);
                }


            } catch (Exception ex) {
            }
        }
        async private Task Get_EventInProvince() {
            try {
                doc.EventInProvince = new List<EventInProvince>();
                var query = await Task.Run(() => _clientService.Post<List<EventInProvince>>($"{Globals.BaseURL}/api/cims/accident/GetEventInProvince", param));
                doc.EventInProvince = (List<EventInProvince>)query.Result;
            } catch (Exception ex) {
            }
        }
        async private Task Get_TimeRange() {
            try {

                doc.TimeRange = new List<ChartData>();
                string url = $"{Globals.BaseURL}/api/cims/accident/GetAccindentTimeRngSet";
              //var ccc=  JsonSerializer.Serialize(param);
                var query = await Task.Run(() => _clientService.Post<List<AccindentTimeRngSet>>(url, param));
                var data_api = (List<AccindentTimeRngSet>)query.Result;
                foreach (var d in data_api) {
                    ChartData n = new ChartData();
                    n.X = d.accident_timerange;
                    n.Y = d.Count_Result;
                    doc.TimeRange.Add(n);
                }
            } catch (Exception ex) {
            }
        }
        async private Task Get_Vehicle() {
            try {

                doc.Vehicle = new List<ChartData>();
                var query = await Task.Run(() => _clientService.Post<List<AccindentVehicleSubtype>>($"{Globals.BaseURL}/api/cims/accident/GetAccindentVehicleSubtypeSet", param));
                var data_api = (List<AccindentVehicleSubtype>)query.Result;
                foreach (var d in data_api) {
                    ChartData n = new ChartData();
                    n.X = d.vehicle_subtype;
                    n.Y = d.Count_Result;
                    doc.Vehicle.Add(n);
                }
            } catch (Exception ex) {
            }
        }
       async private Task Get_EventLocation() {
            try {

                doc.EventLocation = new List<AccindentLocationSet>();
                var query = await Task.Run(() => _clientService.Post<List<AccindentLocationSet>>($"{Globals.BaseURL}/api/cims/accident/GetAccindentLocationSet", param));
                doc.EventLocation = (List<AccindentLocationSet>)query.Result;
             
            } catch (Exception ex) {
            }
        }
        #endregion


        private DataSet NewTransaction() {
            DataSet doc = new DataSet();
            doc.AgeRange_Data = new List<AccindentPatiesAgeRange>();
            doc.Number_Data = new AccidentCountSetResult();
            doc.DangerBehavior = new List<ChartData>();
            doc.RoadType = new List<ChartData>();
            doc.EventCause = new List<ChartData>();
            doc.EventInProvince = new List<EventInProvince>();

            doc.TimeRange = new List<ChartData>();
            doc.Vehicle = new List<ChartData>();
            doc.EventLocation = new List<AccindentLocationSet>();
            return doc;
        }
        private AccidentCountSetParam NewParam() {
            AccidentCountSetParam p = new AccidentCountSetParam();
            p.DateBegin = DateTime.Now.Date.AddYears(-1);
            p.DateBegin = DateTime.Now.Date.AddYears(-1);
            p.Province = new List<string>();
            p.isGetAllProvince = false;

            return p;
        }
    }
}
