namespace RobotAPI.Models.Accident
{
    public class AccidentCountSetParam
    {
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public bool isGetAllProvince { get; set; }
        public string[] Province { get; set; }
    }
    public class AccidentCountSetResult
    {
        public int most_event { get; set; }
        public int most_injured { get; set; }
        public int most_death { get; set; }
        public decimal death_Rate { get; set; }
    }

    public class AccidentCaseSetResult
    {
        public string accident_date { get;set;}
        public string accident_cause { get; set; }
        public int Count_Result { get; set; }
    }

    public class AccindentTimeRngSet
    {
        public string accident_timerange { get; set; }
        public int Count_Result { get; set; }
    }

    public class AccindentPatiesAgeRange
    {
        public string paties_agerange { get; set; }
        public int Count_Result { get; set; }
    }

    public class AccindentBehaviors
    {
        public string behaviors { get; set; }
        public int Count_Result { get; set; }
    }
    public class AccindentVehicleSubtype
    {
        public string vehicle_subtype { get; set; }
        public int Count_Result { get; set; }
    }


    public class AccindentRoadTypeSet
    {
        public string road_type { get; set; }
        public int Count_Result { get; set; }
    }
}
