using RobotAPI.Data.DA.Portal;
using RobotAPI.Data.ML.Portal;
using RobotAPI.Data.ML.Weather.WaterBoard;
using RobotAPI.Services.Api;
using System.Globalization;

namespace RobotAPI.Data.DA.Weather.Waterboard {
    public class WaterService {

        PortalService _portalService;
        ClientService _clientService;
        public WaterService(ClientService clientService, PortalService portalService) {
            _clientService = clientService;
            _portalService = portalService;
        }
        async public Task<BasicInfo> GetBasicInfo() {
            var data = await Task.Run(() => GetWater_Rainfall());
            var h24 = data.Where(o => o.rainfall24h > 0).OrderByDescending(o => o.rainfall24h).FirstOrDefault();
            BasicInfo output = new BasicInfo();
            output.rain_level_2daysago_max = h24.rainfall24h;
            output.rain_level_3daysago_avg = 77;
            output.rain_level_7daysago_avg = 999;
            output.max_temp_of_week = 999;
            output.min_temp_of_week = 44;
            output.volumn_of_water_usage = 55;
            output.volumn_of_water_usage_percentage = 88;
            output.number_of_flood_district = 22;
            output.number_of_drought_district = 11;
            output.province_name = "ทั่วประเทศ";
            return output;
        }
        public static DamInfo GetDamInfo() {
            DamInfo output = new DamInfo();
            output.dam_use = 777;
            output.dam_storage = 888;
            output.dam_use_percentage = 55;
            output.count_total_dam = 543;
            output.datestring = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            output.water_status_count_station = new List<WaterStatusInDam>();
            output.water_status_count_station.Add(new WaterStatusInDam { color = "#3B9FE3", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/water-level/1-water-level.png", count_station = 10, water_status = "น้ำล้นตลิ่ง" }); ;
            output.water_status_count_station.Add(new WaterStatusInDam { color = "#EF6E0F", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/water-level/2-water-level.png", count_station = 20, water_status = "น้ำมาก" }); ;
            output.water_status_count_station.Add(new WaterStatusInDam { color = "#08EF5F", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/water-level/3-water-level.png", count_station = 30, water_status = "น้ำปกติ" }); ;
            output.water_status_count_station.Add(new WaterStatusInDam { color = "#9408EF", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/water-level/5-water-level.png", count_station = 40, water_status = "น้ำน้อย" }); ;
            output.water_status_count_station.Add(new WaterStatusInDam { color = "#EF08AB", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/water-level/water-level-24-icon.png", count_station = 50, water_status = "น้ำน้อยวิกฤติ" }); ;
            output.water_status_count_station.Add(new WaterStatusInDam { color = "#EF08AB", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/water-level/no-information-icon.png", count_station = 10, water_status = "ไม่มีข้อมูล" }); ;
            return output;
        }


        async public Task<RainFallInfo> GetRainFallInfo() {

            RainFallInfo output = new RainFallInfo();
            var data = await Task.Run(() => _portalService.GetDPM020());
            var maxrain = data.rows.OrderByDescending(o => o.ACCUMULATERAINFALL3HOURS);

            output.raining_status_today_icon = @"https://admin.disaster.go.th/assets/img/weather/weatherlogo/1.png";
            output.raining_status_today = "ฝนปานกลาง";
            output.raining_status_tomorrow = "ท้องฟ้าแจ่มใส";
            output.raining_status_next_2days = "ฝนตกน้อย";

            if (data != null) {
                output.count_total_station = data.rows.Count;
            } else {
                output.count_total_station = 0;
            }
            output.raining_status_count_station = new List<RainingStatusInStation>();


            int mostrain = 0;
            int morerain = 0;
            int muduimrain = 0;
            int litlerain = 0;
            int norain = 0;
            if (data != null) {
                mostrain = data.rows.Where(o => o.ACCUMULATERAINFALL24HOURS_DESC == "ฝนตกหนักมาก").Count();
                morerain = data.rows.Where(o => o.ACCUMULATERAINFALL24HOURS_DESC == "ฝนตกหนัก").Count();
                muduimrain = data.rows.Where(o => o.ACCUMULATERAINFALL24HOURS_DESC == "ฝนตกปานกลาง").Count();
                litlerain = data.rows.Where(o => o.ACCUMULATERAINFALL24HOURS_DESC == "ฝนตกเล็กน้อย").Count();
                norain = data.rows.Where(o => o.ACCUMULATERAINFALL24HOURS_DESC == "ไม่มีฝน").Count();
            }

            output.raining_status_count_station.Add(new RainingStatusInStation { count_station = mostrain, raining_status = "ฝนตกหนักมาก", color = "#3B9FE3", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/raining/raining-5.png" }); ;
            output.raining_status_count_station.Add(new RainingStatusInStation { count_station = morerain, raining_status = "ฝนตกหนัก", color = "#EF6E0F", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/raining/raining-4.png" }); ;
            output.raining_status_count_station.Add(new RainingStatusInStation { count_station = muduimrain, raining_status = "	ฝนตกปานกลาง", color = "#08EF5F", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/raining/raining-3.png" }); ;
            output.raining_status_count_station.Add(new RainingStatusInStation { count_station = litlerain, raining_status = "	ฝนตกเล็กน้อย", color = "#9408EF", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/raining/raining-2.png" }); ;
            output.raining_status_count_station.Add(new RainingStatusInStation { count_station = norain, raining_status = "ไม่มีฝน", color = "#EF08AB", logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/raining/no-information-icon.png" }); ;

            output.rainfal_in_station = new List<RainFal_station>();
            if (data != null) {
                int i = 1;
                foreach (var row in data.rows.Where(o => !string.IsNullOrEmpty(o.PROVINCE_NAME)).OrderByDescending(o => o.ACCUMULATERAINFALL24HOURS)) {
                    if (i > 3) {
                        break;
                    }
                    if (i == 1) {
                        output.rainfal_in_station.Add(new RainFal_station { color = "#3B9FE3", station_id = row.STATIONID, station_name = $"{row.PROVINCE_NAME} => {row.AMPHUR_NAME}", rainfal_level = Convert.ToDouble(row.ACCUMULATERAINFALL24HOURS) });
                    }
                    if (i == 2) {
                        output.rainfal_in_station.Add(new RainFal_station { color = "#EF6E0F", station_id = row.STATIONID, station_name = $"{row.PROVINCE_NAME} => {row.AMPHUR_NAME}", rainfal_level = Convert.ToDouble(row.ACCUMULATERAINFALL24HOURS) });
                    }
                    if (i == 3) {
                        output.rainfal_in_station.Add(new RainFal_station { color = "#08EF5F", station_id = row.STATIONID, station_name = $"{row.PROVINCE_NAME} => {row.AMPHUR_NAME}", rainfal_level = Convert.ToDouble(row.ACCUMULATERAINFALL24HOURS) });
                    }
                    i++;
                }
            } else {
                output.rainfal_in_station.Add(new RainFal_station { color = "#3B9FE3", station_id = "1", station_name = "X", rainfal_level = 0 });
                output.rainfal_in_station.Add(new RainFal_station { color = "#EF6E0F", station_id = "2", station_name = "X", rainfal_level = 0 });
                output.rainfal_in_station.Add(new RainFal_station { color = "#08EF5F", station_id = "3", station_name = "X", rainfal_level = 0 });
            }

            return output;
        }

        async public Task<PMInfo> GetPMInfo() {

            PMInfo output = new PMInfo();
            var data = await Task.Run(() => _portalService.GetDPM020());
            data.rows = data.rows.Where(o => o.AQPM25_DESC != "").ToList();

            if (data != null) {
                output.count_total_station = data.rows.Count;
            } else {
                output.count_total_station = 0;
            }
            output.pm_status_count_station = new List<PMStatusInStation>();


            int mostpm = 0;
            int morepm = 0;
            int muduimpm = 0;
            int litlepm = 0;
            int nopm = 0;
            if (data != null) {
                mostpm = data.rows.Where(o => o.AQPM25_DESC == "มีผลกระทบต่อสุขภาพ").Count();
                morepm = data.rows.Where(o => o.AQPM25_DESC == "เริ่มมีผลกระทบต่อสุขภาพ").Count();
                muduimpm = data.rows.Where(o => o.AQPM25_DESC == "ปานกลาง").Count();
                litlepm = data.rows.Where(o => o.AQPM25_DESC == "คุณภาพอากาศดี").Count();
                nopm = data.rows.Where(o => o.AQPM25_DESC == "คุณภาพอากาศดีมาก").Count();
            }

            output.pm_status_count_station.Add(new PMStatusInStation { count_station = mostpm, pm_status = "มีผลกระทบต่อสุขภาพ", color = "#3B9FE3", logo_status = @"https://admin.disaster.go.th/assets/img/weather/pmlogo/4.png" }); ;
            output.pm_status_count_station.Add(new PMStatusInStation { count_station = morepm, pm_status = "เริ่มมีผลกระทบต่อสุขภาพ", color = "#EF6E0F", logo_status = @"https://admin.disaster.go.th/assets/img/weather/pmlogo/3.png" }); ;
            output.pm_status_count_station.Add(new PMStatusInStation { count_station = muduimpm, pm_status = "ปานกลาง", color = "#08EF5F", logo_status = @"https://admin.disaster.go.th/assets/img/weather/pmlogo/2.png" }); ;
            output.pm_status_count_station.Add(new PMStatusInStation { count_station = litlepm, pm_status = "คุณภาพอากาศดี", color = "#9408EF", logo_status = @"https://admin.disaster.go.th/assets/img/weather/pmlogo/1.png" }); ;
            output.pm_status_count_station.Add(new PMStatusInStation { count_station = nopm, pm_status = "คุณภาพอากาศดีมาก", color = "#EF08AB", logo_status = @"https://admin.disaster.go.th/assets/img/weather/pmlogo/0.png" }); ;

            output.pm_in_station = new List<PM_station>();
            if (data != null) {
                int i = 1;
                foreach (var row in data.rows.Where(o => !string.IsNullOrEmpty(o.PROVINCE_NAME)).OrderByDescending(o => o.ACCUMULATERAINFALL24HOURS)) {
                    if (i > 3) {
                        break;
                    }
                    if (i == 1) {
                        output.pm_in_station.Add(new PM_station { color = "#3B9FE3", station_id = row.STATIONID, station_name = $"{row.PROVINCE_NAME} => {row.AMPHUR_NAME}", rainfal_level = Convert.ToDouble(row.ACCUMULATERAINFALL24HOURS) });
                    }
                    if (i == 2) {
                        output.pm_in_station.Add(new PM_station { color = "#EF6E0F", station_id = row.STATIONID, station_name = $"{row.PROVINCE_NAME} => {row.AMPHUR_NAME}", rainfal_level = Convert.ToDouble(row.ACCUMULATERAINFALL24HOURS) });
                    }
                    if (i == 3) {
                        output.pm_in_station.Add(new PM_station { color = "#08EF5F", station_id = row.STATIONID, station_name = $"{row.PROVINCE_NAME} => {row.AMPHUR_NAME}", rainfal_level = Convert.ToDouble(row.ACCUMULATERAINFALL24HOURS) });
                    }
                    i++;
                }
            } else {
                output.pm_in_station.Add(new PM_station { color = "#3B9FE3", station_id = "1", station_name = "X", rainfal_level = 0 });
                output.pm_in_station.Add(new PM_station { color = "#EF6E0F", station_id = "2", station_name = "X", rainfal_level = 0 });
                output.pm_in_station.Add(new PM_station { color = "#08EF5F", station_id = "3", station_name = "X", rainfal_level = 0 });
            }

            return output;
        }

        async public Task<List<DPM020DataSet.Water>?> GetRainFallInfo_Extend() {
            List<DPM020DataSet.Water>? output = new List<DPM020DataSet.Water>();
            output = await Task.Run(() => GetWater_Rainfall());
            return output;

        }

        public static StormInfo GetStormInfo() {
            StormInfo output = new StormInfo();
            output.storm_status = "พายุหมาเหงา";
            output.logo_status = @"https://admin.disaster.go.th/assets/img/weather/waterlogo/icons/storm.png";
            return output;
        }



        async public Task<DiasterSummary?> DisasterSummary(string? province) {
            province = province == null ? "" : province;
            DiasterSummary? output = new DiasterSummary();
            var query = await Task.Run(() => _portalService.GetDPM013());
   
            query.rows = query.rows.Where(o =>
                                            (o.PROVINCE_NAME == province || province == "")
                                            ).ToList();
            output.count_flood = query.rows.Where(o => o.ANNOUNCECODE.StartsWith("FL")).Count();//น้ำท่วม
            output.count_landslide = query.rows.Where(o => o.ANNOUNCECODE.StartsWith("LS")).Count();//น้ำท่วม 
            output.count_winter = query.rows.Where(o => o.ANNOUNCECODE.StartsWith("WC")).Count();//ภัยหนาว
            //output.count_windstorm = query.rows.Where(o => o.ANNOUNCECODE.StartsWith("")).Count(); //พายุ
            output.count_windstorm = 0; //พายุ
            output.count_fire = query.rows.Where(o => o.ANNOUNCECODE.StartsWith("FR")).Count();//ไฟใหม้
   output.count_wildfire = query.rows.Where(o => o.ANNOUNCECODE.StartsWith("FW")).Count();//ไฟป่า
            output.count_chemical = 0;//สารเคมี
            output.count_building_collapse = 0;//อาคารถล่ม
            output.count_drought = query.rows.Where(o => o.ANNOUNCECODE.StartsWith("DR")).Count();//ภัยแล้ง
            output.count_tsunami =0;//tsunami
            output.count_toal_disaster = query.rows.Where(o=>   o.ANNOUNCECODE.StartsWith("DR")
                                                                || o.ANNOUNCECODE.StartsWith("FL")
                                                                || o.ANNOUNCECODE.StartsWith("LS")
                                                                || o.ANNOUNCECODE.StartsWith("WC")
                                                                || o.ANNOUNCECODE.StartsWith("FR")
                                                                )
                                                    .Count();//sum all community
            return output;
            //FL.อุทกภัย
            //FR  อัคคีภัย
            //WC ภัยหนาว
            //LS ดินโคนถล่ม
            //DR ภัยแล้ง
            //FW ไฟป่า

        }

        async public Task<DiasterSummary?> DisasterSummary2(string? province) {
            province = province == null ? "" : province;
            DiasterSummary? output = new DiasterSummary();
            output = await Task.Run(() => _portalService.GetDPM013SourceDB());

             
            return output;
            //FL.อุทกภัย
            //FR  อัคคีภัย
            //WC ภัยหนาว
            //LS ดินโคนถล่ม
            //DR ภัยแล้ง
            //FW ไฟป่า

        }

        #region thaiwater
        async public Task<List<DPM020DataSet.Water>?> GetWater_Rainfall() {
            //ปริมาณน้ำฝน
            List<DPM020DataSet.Water>? r = new List<DPM020DataSet.Water>(); 
            try { 
                string url = $"https://api-v3.thaiwater.net/api/v1/thaiwater30/api_service?mid=18&id3107&eid=pIrUoKkm6joSdUcDpCGgEWoSrIsPnXy__oVNSUgMCMh7NEiUaEbFYyLbGnwJ-mSGcq8v-SFB3j01I9-ZXdmf_g";
                var responds = await _clientService.GetAllAsync<List<DPM020DataSet.Water>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r = null;
                } else {
                    r = (List<DPM020DataSet.Water>)responds.Result;
                }
            } catch (Exception ex) {
                r = null;
            }
            return r;
        }
        async public Task<List<DPM020DataSet.Dam>?> GetWater_Dam() {
            //น้ำเชื่อน
            List<DPM020DataSet.Dam>? r = new List<DPM020DataSet.Dam>();
            try { 
                string url = $"https://api-v3.thaiwater.net/api/v1/thaiwater30/api_service?mid=74&id3110&eid=KuFZvhRzHXWvpVPgS3ZSiH4tO3DawFRyw0ktUHGH6LdfB58rro5vLGd6FesKVUWqJ3ZeAQZCe20ZhFZQH0xR9w";
                var responds = await _clientService.GetAllAsync<List<DPM020DataSet.Dam>>(url, "");
                if (responds.StatusCode.ToString().ToUpper() != "OK") {
                    r = null;
                } else {
                    r = (List<DPM020DataSet.Dam>)responds.Result;
                }
            } catch (Exception ex) {
                r = null;
            }
            return r;
        }
        #endregion
    }
}
