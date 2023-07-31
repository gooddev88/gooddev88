namespace RobotAPI.Data.ML.Portal {
    public class DPM020DataSet {
      

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

        public class DataRow {
            public string STATIONID { get; set; }
            public string LOCATION { get; set; }
            public string PROVINCE_ID { get; set; }
            public string? PROVINCE_NAME { get; set; }
            public string? AMPHUR_ID { get; set; }
            public string? AMPHUR_NAME { get; set; }
            public string? TUMBOL_ID { get; set; }
            public string? TUMBOL_NAME { get; set; }
            public string? LATITUDE { get; set; }
            public string? LONGITUDE { get; set; }
            public string AWSSERIALNAME { get; set; }//หมายเลขเครื่องตรวจวัดสภาพอากาศ
            public string AWSRECORDTIME { get; set; }//เวลาตรวจวัดข้อมูลสภาพอากาศ
            public string AWSPTEMPERATURE { get; set; }//อุณหภูมิตัวเครื่องตรวจวัดสภาพอากาศ (°C)

            public string AWSPRESSURE { get; set; }//ความกดอากาศ (hPa)
            public string AWSRAINFALL { get; set; }//ประมาณฝน (mm)
            public string AWSTEMPERATURE { get; set; }//อุณหภูมิ(°C)

            public string AWSRELATVIEHUMIDITY { get; set; }//ความช้ืน (%)

            public string AWSWINDSPEED { get; set; }//ความเร็วลม (m/s)

            public string AWSWINDDIRECTION { get; set; }//ทิศทางลม
            public string AWSACTIVESTATUS { get; set; }//สถานะระบุเครื่องตรวจวดัสภาพอากาศปกติหรือไม
            public string AWSLASTUPDATE { get; set; }//เวลาที่บันทึกข้อมูลสภาพอากาศ
            public string AQSERIALNAME { get; set; }//หมายเลขเครื่องตรวจวัดคุณภาพอากาศ
            public string AQTEMPERATURE { get; set; }// อุณหภูมิที่ตรวจได้จากเครื่องตรวจวัดคุณภาพอากาศ (°C)

            public string AQDEWPOINT { get; set; }//จุดน้า ค้าง (°C)

            public string AQNO2 { get; set; }//ไนโตรเจนไดออกไซต์(ppb)

            public string AQO3 { get; set; }//โอโซน (ppb)
            public object AQOX { get; set; }//โอโซน (ppb)
            public string AQRELATIVEHUMIDITY { get; set; }//ความช้ืนที่ตรวจไดจ้ากเครื่องตรวจวดัคุณภาพอากาศ (%)
            public string AQPM1 { get; set; }//ค่า PM 1.0 (µg/m³)
            public string AQPM25 { get; set; }//ค่า PM 2.5 (µg/m³)
            public string AQPM10 { get; set; }//ค่า PM 10 (µg/m³)    
            public string AQTSP { get; set; }//ไม่ใช
            public string AQACTIVESTATUS { get; set; }//สถานะระบุเครื่องตรวจวดัคุณภาพอากาศปกติหรือไม่

            public string AQLASTUPDATE { get; set; }//เวลาที่บันทึกข้อมูลคุณภาพอากาศ
            public string ACCUMULATERAINFALL3HOURS { get; set; }//ประมาณฝนสะสม 3 ชวั่ โมง (mm)

            public string ACCUMULATERAINFALL24HOURS { get; set; }//ประมาณฝนสะสม 24 ชวั่ โมง (mm)

            public string ACCUMULATERAINFALLLASTUPDATE { get; set; }//เวลาที่ประมวลผลฝนสะสมล่าสุด
            public string ACCUMULATERAINFALLRECORDTIME { get; set; }//เวลาที่บนั ทึกขอ้ มูลฝนสะสมล่าสุด
            public string REGION { get; set; }//ภาคที่สถานีต้งัอยู่N - ภาคเหนือ, NE - ภาคตะวันออกเฉียงเหนือ, C - ภาคกลาง, E - ภาคตะวันออก, S - ภาคใต้, W - ภาคตะวันตก
            public string ID { get; set; }//ไม่ใช้
            public string AQRECORDTIME { get; set; }
            public string STATION_CODE { get; set; }
            public string AWSBATTERYVOLT { get; set; }

            public string ACCUMULATERAINFALL24HOURS_DESC { get; set; }//ประมาณน้ำฝน 24 ชั่วโมง (คำบรรยาย)
            public string AQPM25_DESC { get; set; }//ค่า PM 2.5 (µg/m³) (คำบรรยาย)
        }


        public class Water {
          
                public double rainfall12h { get; set; }
                public double rainfall15m { get; set; }
                public double rainfall24h { get; set; }
                public DateTime rainfall_datetime { get; set; }
                public int tele_station_id { get; set; }
            
        }

        public class Dam {
            public string dam_date { get; set; }
            public int dam_id { get; set; }
            public double? dam_inflow { get; set; }
            public double? dam_level { get; set; }
            public double? dam_released { get; set; }
            public double? dam_spilled { get; set; }
            public double? dam_storage { get; set; }
        }


    }
}
