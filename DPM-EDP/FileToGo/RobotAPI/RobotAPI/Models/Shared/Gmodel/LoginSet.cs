using RobotAPI.Data.MainDB.TT;

namespace RobotAPI.Models.Shared.Gmodel {
    public class LoginSet {
        public String CurrentUser { get; set; }
        public UserInfo CurrentUserInfo { get; set; }
        public CompanyInfo? CurrentRootCompany { get; set; }
        public CompanyInfo? CurrentCompany { get; set; }
        public DateTime CurrentTransactionDate { get; set; }
        public List<String> UserInCompany { get; set; }
        public List<String> UserInRCompany { get; set; }
        public List<vw_PermissionInMenu> UserInMenu { get; set; }
        public List<vw_PermissionInBoard> UserInBoard { get; set; }
        public vw_PermissionInBoard CurrentBoard { get; set; }
        public List<String> UserInDocStep { get; set; }
        public List<UserMenu> UserMenu { get; set; }
        public String Token { get; set; }
        public String RefreshToken { get; set; }
        public String UserImageURL { get; set; }
        public String AppLogoImage { get; set; }
        public String BackgroundImage { get; set; }
        public String LoginResult { get; set; }
        public String LoginResultInfo { get; set; }
        public String PageError { get; set; }
        public String AppID { get; set; }
        public String LogInByApp { get; set; }
        public String RefUserID { get; set; }
        public decimal CurrentVatRate { get; set; }
        public String CurrentMacNo { get; set; }
        public String LatestPage { get; set; }

    }
}
