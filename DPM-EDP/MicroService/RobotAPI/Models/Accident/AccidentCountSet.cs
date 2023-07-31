using RobotAPI.Data.CimsDB.TT;

namespace RobotAPI.Models.Accident {
    public class AccidentCountSetParam {
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public bool isGetAllProvince { get; set; }
        public string[] Province { get; set; }
    }

    public class AccidentCountSetResult {
        public int most_event { get; set; }
        public int most_injured { get; set; }
        public int most_death { get; set; }
        public decimal death_Rate { get; set; }
    }

    public class AccidentCauseSetResult {
        public string cause { get; set; }
        public int count_result { get; set; }
    }

    public class AccindentTimeRngSet {
        public string accident_timerange { get; set; }
        public int Count_Result { get; set; }
    }

    public class AccindentPatiesAgeRange {
        public string paties_agerange { get; set; }
        public int Count_Result { get; set; }
    }

    public class AccindentBehaviors {
        public string behavior { get; set; }
        public int count_result { get; set; }
    }
    public class AccindentVehicleSubtype {
        public string vehicle_subtype { get; set; }
        public int Count_Result { get; set; }
    }


    public class AccindentRoadTypeSet {
        public string road_type { get; set; }
        public int Count_Result { get; set; }
    }


    public class AccindentLocationSet {
        public string subdistrict { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
  
    }
    public class EventInProvince {
        public string province { get; set; }
        public int event_qty { get; set; }
        public int death_qty { get; set; }
        public int injured_qty { get; set; }
    }
}
