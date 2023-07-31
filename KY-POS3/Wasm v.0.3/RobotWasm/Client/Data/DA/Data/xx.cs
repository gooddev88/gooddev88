namespace RobotWasm.Client.Data.DA.Data {
    public partial class xx {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string FirstName_En { get; set; }
        public string LastName_En { get; set; }
        public string FullName_En { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string DepartmentID { get; set; }
        public string PositionID { get; set; }
        public bool IsProgramUser { get; set; }
        public bool? IsNewUser { get; set; }
        public DateTime? JobStartDate { get; set; }
        public DateTime? ResignDate { get; set; }
        public string AddrFull { get; set; }
        /// <summary>
        /// เลขที่
        /// </summary>
        public string AddrNo { get; set; }
        public string AddrMoo { get; set; }
        /// <summary>
        /// เขต/ตำบล
        /// </summary>
        public string AddrTumbon { get; set; }
        /// <summary>
        /// แขวง/อำเภอ
        /// </summary>
        public string AddrAmphoe { get; set; }
        /// <summary>
        /// จังหวัด
        /// </summary>
        public string AddrProvince { get; set; }
        /// <summary>
        /// รหัสไปรษณีย์
        /// </summary>
        public string AddrPostCode { get; set; }
        /// <summary>
        /// ประเทศ
        /// </summary>
        public string AddrCountry { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
        public string MaritalStatus { get; set; }
        public string CitizenId { get; set; }
        public string BookBankNumber { get; set; }
        /// <summary>
        /// DB/LDAP
        /// </summary>
        public string AuthenType { get; set; }
        public string ApproveBy { get; set; }
        public bool? UseTimeStamp { get; set; }
        public string ImageProfile { get; set; }
        public string LineToken { get; set; }
        public bool? IsSuperMan { get; set; }
        public string DefaultCompany { get; set; }
        public string DefaultMenu { get; set; }
        /// <summary>
        /// USER / CUSTOMER /VENDOR
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// VENDORID / CUSTOMERID
        /// </summary>
        public string RelateID { get; set; }
        public string? JwtToken { get; set; }
        public string? JwtRefreshToken { get; set; }
        public DateTime? JwtTokenExpiryDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
    }

    //public class UserInfoPagination
    //{
    //    public int TotalCount { get; set; }
    //    public List<UserInfo> Data { get; set; }
    //}

    public class UserInfoPagination<T> where T : class
    {
        public int TotalRecords { get; set; }
        public int CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public T Data { get; set; }
        public UserInfoPagination(T data, int totalRecords, int currentPageNumber, int pageSize)
        {
            Data = data;
            TotalRecords = totalRecords;
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize;

            // total pages count
            TotalPages = Convert.ToInt32(Math.Ceiling(((double)TotalRecords / (double)pageSize)));

            HasNextPage = CurrentPageNumber < TotalPages;
            HasPreviousPage = CurrentPageNumber > 1;
        }
    }
}
