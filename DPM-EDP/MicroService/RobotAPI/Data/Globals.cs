
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;




namespace RobotAPI.Data {
    public static class Globals {

        public static string CMSConn { get; set; }//vertica
        public static string CimsConn { get; set; } = "";
        //public static string DataStoreConn { get; set; } = ""; 
        public static string MainContextConn { get; set; } = "";
        public static string DPMQContextConn { get; set; } = "";
        public static string RoomConn { get; set; } = "";
        public static string XfilescenterContextConn { get; set; } = "";





        #region type
        public static string Type_MasterProfile { get; set; } = "MASTERTYPE_PHOTO_PROFILE";
        public static string Type_VendorProfile { get; set; } = "VENDORS_PHOTO_PROFILE";
        public static string Type_ItemProfile { get; set; } = "ITEMS_PHOTO_PROFILE";

        public static string Type_CompanyProfile { get; set; } = "COMPANY_PHOTO_PROFILE";
        public static string Type_CustomerProfile { get; set; } = "CUSTOMERS_PHOTO_PROFILE";

        public static string Type_NewsCate { get; set; } = "NEWCATE_PHOTO_PROFILE";
        #endregion
    }
}
