using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess
{
    public static class AddressInfoService
    {
        #region Class
        [Serializable]
        public class AddressComponent
        {
            public string AddrNo { get; set; }

            public string AddrMoo { get; set; }
            public string SubDistrict { get; set; }
            public string District { get; set; }
            public string Province { get; set; }
            public string Zipcode { get; set; }
            public string Country { get; set; }

        }
        #endregion


        #region  Method GET
        public static vw_ThaiPostAddress GetViewThaiPostByID(int ID)
        {
            vw_ThaiPostAddress result = new vw_ThaiPostAddress();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_ThaiPostAddress.Where(o => o.ID == ID).FirstOrDefault();
            }
            return result;
        }

        public static string Convert_FullAddr(AddressComponent data)
        {
            string result = "";
            result = data.AddrNo;
            if (data.Province == "กรุงเทพมหานคร")
            {
                result = result + " " + "แขวง" + data.SubDistrict;
            }
            else
            {
                result = result + " " + "ต." + data.SubDistrict;
            }
            if (data.Province == "กรุงเทพมหานคร")
            {
                result = result + " " + "เขต" + data.District;
            }
            else
            {
                result = result + " " + "อ." + data.District;
            }
            result = result + " " + "จ." + data.Province;
            result = result + " " + data.Zipcode;

            return result;
        }

        public static List<Addr_Country> ListCountry()
        {
            List<Addr_Country> result = new List<Addr_Country>();
            using (GAEntities db = new GAEntities())
            {
                result = db.Addr_Country.OrderBy(o => o.CountryName).ToList();
            }
            return result;
        }

        public static List<Addr_Province> ListProvince()
        {
            List<Addr_Province> result = new List<Addr_Province>();
            using (GAEntities db = new GAEntities())
            {
                result = db.Addr_Province.OrderBy(o => o.ProvinceName).ToList();
            }
            return result;
        }

        public static List<Addr_District> ListAddrBySearch(string search)
        {
            List<Addr_District> result = new List<Addr_District>();
            using (GAEntities db = new GAEntities())
            {

                result = db.Addr_District.Where(o => o.PostCode.Contains(search)
                                              || o.ProvinceID.Contains(search)
                                              || o.AmphoeID.Contains(search)
                                              || o.DistrictID.Contains(search)
                                              || search == "").ToList();

            }
            return result;
        }

        public static vw_ThaiPostAddress GetAddressInfoById(int id)
        {
            vw_ThaiPostAddress result = new vw_ThaiPostAddress();
            try
            {
                using (GAEntities db = new GAEntities())
                {
                    result = db.vw_ThaiPostAddress.Where(o => o.ID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        #endregion
    }
}