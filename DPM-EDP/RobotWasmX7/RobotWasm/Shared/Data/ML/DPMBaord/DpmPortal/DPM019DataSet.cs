namespace RobotWasm.Shared.Data.ML.DPMBaord.DpmPortal {
    public class DPM019DataSet {
        //ข้อมูลศูนย์พักพิงทั่วประเทศ
        public class DataRow {
            public string SHELTERID { get; set; }
            public string SHELTERNAME { get; set; }
            public string PROVINCECODE { get; set; }
            public string TAMBONCODE { get; set; }
            public string POSTALCODE { get; set; }
            public string LATITUDE { get; set; }
            public string LONGITUDE { get; set; }
            public string OFFICER { get; set; }
            public string TEL { get; set; }
            public string SHELTERTYPE { get; set; }
            public string AREA { get; set; }
            public string TOILETAMOUNT { get; set; }
            public string DISTANCEFROMTOILET { get; set; }
            public string HAVEWATER { get; set; }
            public string WATERTYPE { get; set; }
            public string WATERPERDAYFORCONSUMTION { get; set; }
            public string WATERPERDAYFORSHELTER { get; set; }
            public string PERSONAMOUNT { get; set; }
            public string YEAR { get; set; }
        }

        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }
    }
}
