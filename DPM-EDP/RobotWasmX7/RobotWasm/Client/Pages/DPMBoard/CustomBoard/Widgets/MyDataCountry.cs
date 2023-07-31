namespace RobotWasm.Client.Pages.DPMBoard.CustomBoard.Widgets {
    public class MyDataCountry {
        //year: "2017",
        //income: 23.5,
        //expenses: 18.1
        public string province { get; set; }
        public int Count_r { get; set; }

        public List<MyDataCountry> ListData() {
            List<MyDataCountry> rlist = new List<MyDataCountry>();
            rlist.Add(new MyDataCountry { province = "Thai", Count_r = 200 });
            rlist.Add(new MyDataCountry { province = "Japan", Count_r = 500 });
            rlist.Add(new MyDataCountry { province = "England", Count_r = 300 });
            rlist.Add(new MyDataCountry { province = "USA", Count_r = 500 });
            rlist.Add(new MyDataCountry { province = "India", Count_r = 600 });
            rlist.Add(new MyDataCountry { province = "China", Count_r = 1000 });
            return rlist;
        }
    }
}
