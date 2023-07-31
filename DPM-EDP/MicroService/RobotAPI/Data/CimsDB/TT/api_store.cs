using System.Collections;

namespace RobotAPI.Data.CimsDB.TT {
    public partial class api_store {
        public int id { get; set; }
        /// <summary>
        /// เจ้าของ api
        /// </summary>
        public string owner_code { get; set; }
        /// <summary>
        /// รหัสเชื่อมต่อ
        /// </summary>
        public string connection_code { get; set; }
        public string api_url { get; set; }
        public string remark { get; set; }
        public int is_active { get; set; }
        public string api_code { get; set; }
        public string api_name { get; set; }
        public string url_page { get; set; }
    }
}
