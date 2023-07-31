namespace GridInLine.Data {
    public class TestService {
        public class MyMenu {
            public bool IsSelect { get; set; }
            public string MenuID { get; set; }
            public string Name { get; set; }
            public string Remark { get; set; }
            public string Ontop { get; set; }
            public decimal Qty { get; set; }
        }
        public class MyOption { 
            public string value { get; set; }
            public string Desc { get; set; }
        }
        public List<MyMenu> menu { get; set; }
        public List<MyOption> myoption { get; set; }

        public List<MyMenu> CreateTextList() {
            var m = new List<MyMenu>();
            m.Add(new MyMenu { IsSelect = false, MenuID = "m001", Name = "บลูด๊อกผัดเผ็ด" ,Remark="",Ontop="T2", Qty=100 });
            m.Add(new MyMenu { IsSelect = true, MenuID = "m002", Name = "ชิวาวายัดไส้ปลาหมึก", Remark = "", Ontop = "T1", Qty = 400 });
            m.Add(new MyMenu { IsSelect = true, MenuID = "m003", Name = "บางแก้ผัดฉ่า", Remark = "", Ontop = "T2", Qty = 20 });
            return m;
        }
        public List<MyOption> CreateOption() {
            var m = new List<MyOption>();
            m.Add(new MyOption { value = "T1", Desc = "ใส่ทองคำเปลว"});
            m.Add(new MyOption { value = "T2", Desc = "ใส่เรเดี่ยม" }); 
            return m;
        }
    }
}
