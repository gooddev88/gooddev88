using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotWasm.Shared.Data.GaDB {
	public class Promotions {
		public int ID { get; set; }
		public string ProID { get; set; }
		public string RCompanyID { get; set; }
		public string CompanyID { get; set; }
		public string PatternID { get; set; }
		public string ProDesc { get; set; }
		public decimal XValue { get; set; }
		public decimal YValue { get; set; }
		public string CreatedBy { get; set; }
		public DateTime DateBegin { get; set; }
		public DateTime DateEnd { get; set; }
		public DateTime CreatedDate { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public bool IsActive { get; set; }
	}

}
