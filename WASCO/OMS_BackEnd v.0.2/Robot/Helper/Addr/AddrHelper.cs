namespace Robot.Helper.AddrHep {
    public class AddrHelper {

        public static string CreateFullAddr(string HouseNo,string Village,string Building,string RoomNo,string FloorNo,
            string Soi,string Yaek,string Road,string VendAddrMoo,string SubDistrict,string District,string Province,string Postcode) {
            string addr_full = "";
            if (!string.IsNullOrEmpty(HouseNo)) {
                addr_full = addr_full + " เลขที่ " + HouseNo;
            }
            if (!string.IsNullOrEmpty(Village)) {
                if (Village.Replace(" ", "")!="") {
                    addr_full = addr_full + " หมู่บ้าน " + Village;
                } 
            }
            if (!string.IsNullOrEmpty(Building)) { 
                if (Building.Replace(" ", "") != "") {
                    addr_full = addr_full + " อาคาร " + Building;
                }
            }
            if (!string.IsNullOrEmpty(RoomNo )) { 
                if (RoomNo.Replace(" ", "") != "") {
                    addr_full = addr_full + " ห้อง " + RoomNo;
                }
            }
            if (!string.IsNullOrEmpty(FloorNo)) { 
                if (FloorNo.Replace(" ", "") != "") {
                    addr_full = addr_full + " ชั้น " + FloorNo;
                }
            }
            if (!string.IsNullOrEmpty(Soi)) {
                if (Soi.Replace(" ", "") != "") {
                    addr_full = addr_full + " ซอย " + Soi;
                } 
            }
            if (!string.IsNullOrEmpty(Yaek)) { 
                if (Yaek.Replace(" ", "") != "") {
                    addr_full = addr_full + " แยก " + Yaek;
                }
            }

            if (!string.IsNullOrEmpty(Road)) { 
                if (Road.Replace(" ", "") != "") {
                    addr_full = addr_full + " ถนน " + Road;
                }
            }

            if (!string.IsNullOrEmpty(VendAddrMoo)) {
                addr_full = addr_full + " หมู่ " + VendAddrMoo;
            }
            if (Province == "กรุงเทพมหานคร") {
                addr_full = addr_full + " แขวง" + SubDistrict;
                addr_full = addr_full + " เขต" + District;
            } else {
                addr_full = addr_full + " ต." + SubDistrict;
                addr_full = addr_full + " อ." + District;
            }
            addr_full = addr_full + " จ." + Province;
            if (!string.IsNullOrEmpty(Postcode)) {
                addr_full = addr_full + " " + Postcode;
            }
            return  addr_full;

        }
    }
}
