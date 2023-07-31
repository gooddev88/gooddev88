namespace RobotAPI.Data.ML.Portal {
    public class DPM004DataSet {
              
        public class DocSet {
            public int status { get; set; }
            public object message { get; set; }
            public List<DataRow> rows { get; set; }
        }

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
            public object AREA { get; set; }
            public object TOILETAMOUNT { get; set; }
            public object DISTANCEFROMTOILET { get; set; }
            public string HAVEWATER { get; set; }
            public object WATERTYPE { get; set; }
            public object WATERPERDAYFORCONSUMTION { get; set; }
            public object WATERPERDAYFORSHELTER { get; set; }
            public string PERSONAMOUNT { get; set; }
        }


    }
}
