namespace RobotWasm.Client {
    public static class Globals {

        public static string ApiLogInBaseUrl = @"https://ddpmapi.jtgoodapp.com"; 

        public static string AppID = "DPMQ";

        public static string ApiFileGoBaseUrl = @"https://filego.jtgoodapp.com"; 

      
        public static string BaseURL = "https://edpapim.disaster.go.th";



        #region name for local storage 
        public static string AuthToken = "a991";
        public static string Dummy1 = "a992";
        public static string Dummy2 = "a993";
        public static string AuthUsername = "a994";
        public static string AuthFullname = "a995";
        public static string AuthLoginSet = "a996";
        public static string RefreshToken = "a997";
        public static string UserImageURL = "a998";
        #endregion
         
        #region document id
        public static string ActiveID_ApiMaster = "activeid_apimaster";
        public static string ActiveID_ApiMasterEdit = "activeid_apimasterEdit";
        public static string ActiveID_QGroup = "activeq_groupid";

        public static string ActiveID_MCODE = "activeq_mcode";

        public static string ActiveID_USER = "activeid_user";
        public static string ActiveID_USERGROUP = "activeid_usergroup";

        public static string ActiveID_Document = "activeid_document";
        public static string ActiveID_BoadrID = "activeid_BoadrMaster";

        public static string ActiveID_Valuetxt = "activeid_Valuetxt";
        public static string ActiveID_CateID = "activeid_Cateid";
        public static string Active_DocCateID = "active_DocCateid";
        public static string ActiveID_NEWSID = "activeid_Newsid";

        public static string ActiveID_COMGROUPID = "activeid_ComGroupID";
        public static string ActiveID_COMID = "activeid_ComID";

        public static string ActiveID_DataKey = "activeid_data_key";
        public static string Active_IDCONID = "active_iconid";

        #endregion

        #region search ExclusiveBoard

        public static string Search_BoardExclusive = "search_board_exclusive";

        #endregion

        #region search apidoc

        public static string Search_apidoc = "search_apidoc";

        #endregion

        #region search PublishDocument

        public static string DateFrom_PublishDocument = "dateFrom_publishDocument";
        public static string DateTo_PublishDocument = "dateTo_publishDocument";

        #endregion

        #region default username in ChangePassword

        public static string username_Change_Password = "username_change_password";

        #endregion

    }
}
