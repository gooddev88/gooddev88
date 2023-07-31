using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
	public class PromotionPattern {
		public int ID { get; set; }
		public string RCompanyID { get; set; }
		public string CompanyID { get; set; }
		public string PatternID { get; set; }
		public string PatternDesc { get; set; }
		public bool? IsActive { get; set; }
	}

}
