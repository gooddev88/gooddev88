namespace Robot.Data.ML.DBD {
    public class JuristicData {

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class AddressDetail {
            public string addressName { get; set; }
            public object buildingName { get; set; }
            public object roomNo { get; set; }
            public object floor { get; set; }
            public object villageName { get; set; }
            public string houseNumber { get; set; }
            public string moo { get; set; }
            public object soi { get; set; }
            public object street { get; set; }
            public string subDistrict { get; set; }
            public string district { get; set; }
            public string province { get; set; }
        }

        public class JuristicInfo {
            public string juristicID { get; set; }
            public string juristicNameTH { get; set; }
            public string juristicNameEN { get; set; }
            public string juristicType { get; set; }
            public string registerDate { get; set; }
            public string juristicStatus { get; set; }
            public string registerCapital { get; set; }
            public string standardObjective { get; set; }
            public StandardObjectiveDetail standardObjectiveDetail { get; set; }
            public AddressDetail addressDetail { get; set; }
        }

        public class StandardObjectiveDetail {
            public string objectiveDescription { get; set; }
        }


    }
}
