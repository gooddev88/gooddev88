using System;

namespace Robot.Data.DATASTOREDB.TT {
	public class data_accidents {

		public Int32 id { get; set; }
		public Int32? no { get; set; }
		public string? accidentid { get; set; }
		public DateTime? accident_date { get; set; }
		public TimeSpan? accident_time { get; set; }
		public string? date_rage { get; set; }
		public string? district { get; set; }
		public string? branch { get; set; }
		public string? province { get; set; }
		public string? roadtype { get; set; }
		public string? roadskin { get; set; }
		public string? noticepoint { get; set; }
		public string? weather { get; set; }
		public string? visibility { get; set; }
		public decimal? damage_value { get; set; }
		public string? cause { get; set; }
		public string? is_drink { get; set; }
		public string? is_overspeed { get; set; }
		public string? is_violating_traffic_lights { get; set; }
		public string? is_violating_traffic_sign { get; set; }
		public string? is_drivebackwards { get; set; }
		public string? is_illegal_overtaking { get; set; }
		public string? is_phoneondrive { get; set; }
		public string? is_closecut { get; set; }
		public string? is_substance_abuse { get; set; }
		public string? is_does_off { get; set; }
		public string? is_overweight { get; set; }
		public string? is_roadblock { get; set; }
		public string? is_poorvisibility { get; set; }
		public string? is_badcar { get; set; }
		public string? is_badroad { get; set; }
		public string? vehicle_type { get; set; }
		public string? license_plate { get; set; }
		public string? parties_status { get; set; }
		public string? sex { get; set; }
		public Int32? age { get; set; }
		public string? agerang { get; set; }
		public string? nationality { get; set; }
		public string? accident_personrole { get; set; }
		public string? victim_status { get; set; }
		public string? accident_resident { get; set; }
		public string? peath_place { get; set; }
		public string? send_hospital_by { get; set; }
		public string? accident_behavior { get; set; }
		public string? is_break_the_law { get; set; }
		public string? is_not_helmet { get; set; }
		public string? is_motorcycles_not_safe { get; set; }
		public string? is_not_seat_belts { get; set; }
		public string? is_no_driving_license { get; set; }
		public string? is_sit_on_pickup { get; set; }
		public string? other { get; set; }
		public Int32? number_person_in_accident { get; set; }
	}
}
