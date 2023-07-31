using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using RobotWasm.Client;
 
using RobotWasm.Client.Service.Api;
using RobotWasm.Server.Data.CimsDB.TT;
using RobotWasm.Server.Data.GaDB;
using RobotWasm.Shared.Data.DimsDB;
using RobotWasm.Shared.Data.GaDB;
using RobotWasm.Shared.Data.ML.FileGo;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Server.Service.FileGo {
    public class FileGoService {
        //private readonly HttpClient _httpClient;

        //public  FileGoService(HttpClient httpClient) {
        //    _httpClient = httpClient;
        //}

        //private readonly ClientService _clientService;
        //public FileGo(ClientService clientService ) {
        //    _clientService = clientService; 
        //}
        private readonly ClientService _clientService;
        public FileGoService(ClientService clientService ) {
            _clientService = clientService;
         
        }
        #region type
        public static string Type_MasterProfile { get; set; } = "MASTERTYPE_PHOTO_PROFILE";
        public static string Type_VendorProfile { get; set; } = "VENDORS_PHOTO_PROFILE";
        public static string Type_ItemProfile { get; set; } = "ITEMS_PHOTO_PROFILE";

        public static string Type_CompanyProfile { get; set; } = "COMPANY_PHOTO_PROFILE";
        public static string Type_CustomerProfile { get; set; } = "CUSTOMERS_PHOTO_PROFILE";

        public static string Type_NewsCate { get; set; } = "NEWCATE_PHOTO_PROFILE";
        public static string Type_PersonProfile { get; set; } = "PERSON_PHOTO_PROFILE";

        public static string Type_leavedocument { get; set; } = "LEAVE_DOCUMENT";

        public static string Type_PublishDocument { get; set; } = "PUBLISH_DOCUMENT";
        #endregion


        #region Authen Api
        public async Task<I_BasicResult> LoginApiFileGo() {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
        HttpClient _httpClient=new HttpClient();
        var login = GetFileGoLoginPostgres();
                LoginFileGoRequest data_login = new LoginFileGoRequest { Apps = Globals.AppID, UserName = login.login_name, Password = login.login_pass, RememberMe = true };

                string url = $"{login.rooturl_public}/api/authen/Login/jwtApilogin"; 
                var response = await _httpClient.PostAsJsonAsync(url, data_login); 
                if (response.StatusCode.ToString().ToUpper() != "OK") {
                    r.Result = "fail";
                    r.Message1 = response.StatusCode.ToString();
                    
                } else {
                    var res = await response.Content.ReadFromJsonAsync<MyTokenResponse>();
                    var rr = SetJwtPostgress(res.Token, res.RefreshToken, res.RefreshTokenExpiryTime);
                }
                 
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        public static XFileLocation GetFileGoLogin() {
            XFileLocation? output = new XFileLocation();
            using (GAEntities db = new GAEntities()) {
                output = db.XFileLocation.Where(o => o.IsActive && o.IsCurrent && o.PathType == "FILEGO").FirstOrDefault();
                output.RootUrlPublic = String.IsNullOrEmpty(output.RootUrlPublic) ? output.RootUrl : output.RootUrlPublic;
            }
            return output;
        }

        // top add
        public static xfile_location GetFileGoLoginPostgres() {
            xfile_location? output = new xfile_location();
            using (cimsContext db = new cimsContext()) {
                output = db.xfile_location.Where(o => o.is_active == 1 && o.is_current==1&& o.rootpath_id == "FILEGO01").FirstOrDefault();
                output.rooturl_public = String.IsNullOrEmpty(output.rooturl_public) ? output.rooturl : output.rooturl_public;
            }
            return output;
        }

        async public Task<xfiles?> GetFileInBytePostgres(string rcom, string com, string doctype, string docId)
        {
            //ดึงไฟล์เป็น binary
            xfiles? output = new xfiles();
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            using (cimsContext db = new cimsContext())
            {
                var f = db.vw_xfile_ref.Where(o => o.is_active == 1 && o.rcom_id == rcom && o.com_id == com && o.doc_id == docId && o.doctype == doctype && o.rootpath_id == "FILEGO01").OrderByDescending(o => o.created_date).FirstOrDefault();
                if (f != null)
                {
                    string url = f.rooturl_public + @"/api/xfiles/XFilesService/GetFileInByte/" + f.file_id;
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", f.jwttoken);
                    var response = await _httpClient.GetAsync(url);
                    if (response.StatusCode.ToString() == "Unauthorized")
                    {
                        return null;
                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();
                        output = await response.Content.ReadFromJsonAsync<xfiles>();
                    }
                }
                else
                {
                    return null;
                }
            }
            return output;
        }

        public static I_BasicResult SetJwtPostgress(string token, string refreshToken, DateTime expiryDate) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    var loc = db.xfile_location.Where(o => o.is_active==1 && o.is_current==1 && o.rootpath_id == "FILEGO01").FirstOrDefault();
                    loc.jwttoken = token;
                    loc.jwt_refresh_token = refreshToken;
                    loc.jwttoken_expirydate = expiryDate;
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }

            return r;
        }
        public static I_BasicResult SetJwt(string token, string refreshToken, DateTime expiryDate) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var loc = db.XFileLocation.Where(o => o.IsActive && o.IsCurrent && o.PathType == "FILEGO").FirstOrDefault();
                    loc.JwtToken = token;
                    loc.JwtRefreshToken = refreshToken;
                    loc.JwtTokenExpiryDate = expiryDate;
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }

            return r;
        }



        #endregion
        #region Get

        public static List<vw_XFilesRef> ListFilesRef(string rcom, string com, string doctype, string docId) {
            List<vw_XFilesRef> result = new List<vw_XFilesRef>();
            using (GAEntities db = new GAEntities()) {
                result = db.vw_XFilesRef.Where(o => o.IsActive && o.RCompanyID == rcom && o.CompanyID == com && o.DocID == docId && o.DocType == doctype && o.PathType == "FILEGO").ToList();
            }
            return result;
        }
        async public Task<xfiles?> GetFileInByte(string rcom, string com, string doctype, string docId) {
            //ดึงไฟล์เป็น binary
            xfiles? output = new xfiles();
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            
            using (GAEntities db = new GAEntities()) {
                var f = db.vw_XFilesRef.Where(o => o.IsActive && o.RCompanyID == rcom && o.CompanyID == com && o.DocID == docId && o.DocType == doctype && o.PathType == "FILEGO").OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (f != null) {
                    string url = f.RootUrlPublic + @"/api/xfiles/XFilesService/GetFileInByte/" + f.FileID;
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", f.JwtToken);
                    var response = await _httpClient.GetAsync(url);  
                    if (response.StatusCode.ToString() == "Unauthorized") {
                        return null;
                    } else {
                        response.EnsureSuccessStatusCode();
                        output = await response.Content.ReadFromJsonAsync<xfiles>(); 
                    } 
                } else {
                    return null;
                }
            }
            return output;
        }


        public static vw_XFilesRef GetFileId(string rcom, string com, string doctype, string docId) {
            vw_XFilesRef refx = new vw_XFilesRef();
            using (GAEntities db = new GAEntities()) {
                refx = db.vw_XFilesRef.Where(o => o.IsActive && o.RCompanyID == rcom && o.CompanyID == com && o.DocID == docId && o.DocType == doctype && o.PathType == "FILEGO").OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            }
            return refx;
        }
        public static string GetFileUrl(string rcom, string com, string doctype, string docId) {
            string url = "";
            using (GAEntities db = new GAEntities()) {
                var f = db.vw_XFilesRef.Where(o => o.IsActive && o.RCompanyID == rcom && o.CompanyID == com && o.DocID == docId && o.DocType == doctype && o.PathType == "FILEGO").OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (f != null) {
                    url = f.RootUrlPublic + @"/api/xfiles/XFilesService/GetFile/" + f.FileID;
                }
            }
            return url;
        }



        public static string GetFileUrl(string file_id) {
            string url = "";
            using (GAEntities db = new GAEntities()) {
                var f = db.vw_XFilesRef.Where(o => o.FileID == file_id).FirstOrDefault();
                if (f != null) {
                    url = f.RootUrlPublic + @"/api/xfiles/XFilesService/GetFile/" + f.FileID;
                }
            }
            return url;
        }

        #region Postgres file
        public static string GetFileUrlPostgres(string file_id) {
            string url = "";
            using (cimsContext db = new cimsContext()) {
                var f = db.vw_xfile_ref.Where(o => o.file_id == file_id).FirstOrDefault();
                if (f != null) {
                    url = f.rooturl_public + @"/api/xfiles/XFilesService/GetFile/" + f.file_id;
                }
            }
            
            return url;
        }

        async public Task<I_BasicResult> DeleteFileByFileIdPostgres(string rcom, string? com, string doctype, string docid, string fileid, string deleted_by) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                com = com == null ? "" : com;
                using (cimsContext db = new cimsContext()) {
                    var f = db.vw_xfile_ref.Where(o => o.rcom_id == rcom && o.com_id == com && o.doctype == doctype && o.doc_id == docid && o.is_active == 1).OrderByDescending(o => o.created_date).FirstOrDefault();
                    if (f == null) {
                        return r;
                    }
                    List<string> file_ids = new List<string> { fileid };
                    var login_filego = GetFileGoLoginPostgres();
                    var query = await Task.Run(() => _clientService.Post<I_BasicResult>($"{f.rooturl_public}/api/xfiles/XFilesService/DeleteFile/", file_ids, login_filego.jwttoken));
                    if (query.StatusCode == "OK") {
                        var ff = db.xfile_ref.Where(o => o.file_id == f.file_id).FirstOrDefault();
                        if (ff != null) {
                            ff.modified_by = deleted_by;
                            ff.modified_date = DateTime.Now;
                            ff.is_active = 0;
                            db.SaveChanges();
                        }
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }

            return r;
        }

        public static vw_xfile_ref GetXfileRefPostgres(string rcom, string com, string doctype, string docId)
        {
            com = com == null ? "" : com;
            vw_xfile_ref? refx = new vw_xfile_ref();
            using (cimsContext db = new cimsContext())
            {
                refx = db.vw_xfile_ref.Where(o => o.is_active == 1 && o.rcom_id == rcom && o.com_id == com && o.doc_id == docId && o.doctype == doctype).OrderByDescending(o => o.created_date).FirstOrDefault();
            }
            return refx;
        }

        #endregion

        public static string GetThumbUrl(string rcom, string com, string doctype, string docId) {
            string url = "";
            using (GAEntities db = new GAEntities()) {
                var f = db.vw_XFilesRef.Where(o => o.IsActive && o.RCompanyID == rcom && o.CompanyID == com && o.DocID == docId && o.DocType == doctype && o.PathType == "FILEGO").OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (f != null) {
                    url = f.RootUrlPublic + @"/api/xfiles/XFilesService/GetThumb/" + f.FileID;
                }
            }
            return url;
        }
        #endregion
        #region  Upload File
        async public Task<I_BasicResult> UploadFileGoSQL(List<FilesInfo> files, string user, string token_filego) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var login_filego = GetFileGoLogin();
                string dataEndpointUri = $"{login_filego.RootUrl}/api/xfiles/XFilesService/UploadFileToDB";

                var query = await Task.Run(() => _clientService.Post<I_BasicResult>(dataEndpointUri, files, token_filego));
                if (query.StatusCode != "OK") {
                    r.Result = "fail";
                    r.Message1 = query.StatusCode;
                } else {
                    var rr = (I_BasicResult)query.Result;
                    if (rr.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = rr.Message1;
                        //ShowMessage(false, update_result.Message1);
                    } else {
                        //ShowMessage(true, "Upload Success");
                        var xfile_ref = Convert2XFilesRef(files, user);
                        var rrr = SaveXFileRef(xfile_ref);
                        if (rrr.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = rrr.Message1;
                        }
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }

        async public Task<I_BasicResult> UploadFileGoPostgres(List<FilesInfo> files, string user) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                var login_filego = GetFileGoLoginPostgres();
                string dataEndpointUri = $"{login_filego.rooturl_public}/api/xfiles/XFilesService/UploadFileToDB";

                var query = await Task.Run(() => _clientService.Post<I_BasicResult>(dataEndpointUri, files, login_filego.jwttoken));
                if (query.StatusCode != "OK") {
                    r.Result = "fail";
                    r.Message1 = query.StatusCode;
                } else {
                    var rr = (I_BasicResult)query.Result;
                    if (rr.Result == "fail") {
                        r.Result = "fail";
                        r.Message1 = rr.Message1;
                        //ShowMessage(false, update_result.Message1);
                    } else {
                        //ShowMessage(true, "Upload Success");
                        var xfile_ref = Convert2XFilesRefPostgres(files, user);
                        var rrr = SaveXFileRefPostgres(xfile_ref);
                        if (rrr.Result == "fail") {
                            r.Result = "fail";
                            r.Message1 = rrr.Message1;
                        }
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        #endregion
        #region Save update delete
        public static I_BasicResult SaveXFileRef(List<XFilesRef> data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.XFilesRef.AddRange(data);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }

            return r;
        }

        public static I_BasicResult SaveXFileRefPostgres(List<xfile_ref> data) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (cimsContext db = new cimsContext()) {
                    db.xfile_ref.AddRange(data);
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }

            return r;
        }
        async public Task<I_BasicResult> DeleteOldFileAfterSave(string rcom, string com, string doctype, string docid, string deleted_by) {
            //ใช้ลบ file อัพโหลดแบบทับไฟล์เก่าเช่นรูปโปรไฟล์บริษัท หรือ สินค้า 
            //ลบแบบหลายไฟล์
            I_BasicResult? r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
     
            try {
                using (GAEntities db = new GAEntities()) {
                    var all_file = db.vw_XFilesRef.Where(o => o.RCompanyID == rcom && o.CompanyID == com && o.DocType == doctype && o.DocID == docid && o.IsActive).ToList();
                    var latest_file = all_file.OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                    if (latest_file != null) {
                        all_file.Remove(latest_file);
                    } else {
                        return r;
                    }
                    var delete_file_ids = all_file.Select(o => o.FileID).ToList();
                    var jwt_token = all_file.FirstOrDefault()==null?"": all_file.FirstOrDefault().JwtToken;
                    HttpClient _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt_token);
                    
                    string url = $"{latest_file.RootUrl}/api/xfiles/XFilesService/DeleteFile/"; 
                    var response = await Task.Run(() => _httpClient.PostAsJsonAsync (url, delete_file_ids));

                  

                    if (response.StatusCode.ToString().ToUpper() == "OK") {
                        if (delete_file_ids.Count > 0) {
                            var update = db.XFilesRef.Where(o => delete_file_ids.Contains(o.FileID)).ToList();
                            foreach (var f in update) {
                                f.ModifiedBy = deleted_by;
                                f.ModifiedDate = DateTime.Now;
                                f.IsActive = false;

                            }
                            db.SaveChanges();
                        }
                        r = await response.Content.ReadFromJsonAsync<I_BasicResult>();
                    }

                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }

            return r;
        }
        async public Task<I_BasicResult> DeleteFile(string rcom, string com, string doctype, string docid, string deleted_by) {
            //ใช้ลบ file แบบทีละไฟล์อย่างเช่นลบรูปสินค้า
            I_BasicResult? r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var f = db.vw_XFilesRef.Where(o => o.RCompanyID == rcom && o.CompanyID == com && o.DocType == doctype && o.DocID == docid && o.IsActive).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                    if (f == null) {
                        return r;
                    }
                    List<string> file_ids = new List<string> { f.FileID };

                    HttpClient _httpClient = new HttpClient();
                    _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", f.JwtToken);

                    string url= $"{f.RootUrl}/api/xfiles/XFilesService/DeleteFile/";

                    var response = await Task.Run(() => _httpClient.PostAsJsonAsync(url, file_ids));
                    if (response.StatusCode.ToString().ToUpper() == "OK") {
                        var ff = db.XFilesRef.Where(o => o.FileID == f.FileID).FirstOrDefault();
                        if (ff != null) {
                            ff.ModifiedBy = deleted_by;
                            ff.ModifiedDate = DateTime.Now;
                            ff.IsActive = false;
                            db.SaveChanges();
                        }
                        r = await response.Content.ReadFromJsonAsync<I_BasicResult>();
                    }
                }

            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }

            return r;
        }
        #endregion




        #region Init
     
        public static FilesInfo NewFilesInfo(string doctype, string rcom, string com, string docid) {
            FilesInfo n = new FilesInfo();
            var loc = GetFileGoLogin();
            n.file_id = Guid.NewGuid().ToString().ToLower();
            n.owner_id = loc.LoginName;
            n.rcom_id = rcom;
            n.com_id = com;
            n.app_id = loc.AppID;
            n.doc_id = docid;
            n.doc_linenum = 0;
            n.doc_type = doctype;
            n.doc_cate = "";
            n.file_type = "";

            n.remark = "";
            n.fileName = "";
            n.data = null;
            return n;
        }

        public static FilesInfo NewFilesInfoPostgres(string doctype, string rcom, string com, string docid) {
            FilesInfo n = new FilesInfo();
            var loc = GetFileGoLoginPostgres();
            n.file_id = Guid.NewGuid().ToString().ToLower();
            n.owner_id = loc.login_name;
            n.rcom_id = rcom;
            n.com_id = com;
            n.app_id = loc.appid;
            n.doc_id = docid;
            n.doc_linenum = 0;
            n.doc_type = doctype;
            n.doc_cate = "";
            n.file_type = "";

            n.remark = "";
            n.fileName = "";
            n.data = null;
            return n;
        }

        public static List<XFilesRef> Convert2XFilesRef(List<FilesInfo> ii, string created_by) {
            List<XFilesRef> oo = new List<XFilesRef>();
            foreach (var i in ii) {
                XFilesRef o = NewXFilesRef(created_by);

                o.FileExt = "";
                o.FilePath = "";
                o.SubUrl = "";
                o.FileName = i.fileName;
                o.OriginFileName = i.fileName;
                o.OriginFileExt = "";
                o.OriginFilePath = "";
                o.CreatedBy = o.CreatedBy;
                o.FileID = i.file_id;
                o.RCompanyID = i.rcom_id;
                o.DocID = i.doc_id;
                o.DocLineNum = 0;
                o.CompanyID = i.com_id;
                o.DocType = i.doc_type;
                o.DocCate = i.doc_cate;
                o.Remark = i.remark;
                o.FileType = i.file_type;
                o.IsActive = true;
                oo.Add(o);
            }
            return oo;
        }

        public static List<xfile_ref> Convert2XFilesRefPostgres(List<FilesInfo> ii, string created_by) {
            List<xfile_ref> oo = new List<xfile_ref>();
            foreach (var i in ii) {
                xfile_ref o = NewXFilesRefPostgres(created_by);

                o.fileext = "";
                o.filetype = "";
                o.suburl = "";
                o.filename = i.fileName;
                o.origin_filename = i.fileName;
                o.origin_fileext = "";
                o.filepath = "";
                o.created_by = o.created_by;
                o.file_id = i.file_id;
                o.rcom_id = i.rcom_id;
                o.doc_id = i.doc_id;
                o.doc_linenum = 0;
                o.com_id = i.com_id;
                o.doctype = i.doc_type;
                o.doccate = i.doc_cate;
                o.remark = i.remark;
                o.filetype = i.file_type;
                o.is_active = 1;
                oo.Add(o);
            }
            return oo;
        }
        public static XFilesRef NewXFilesRef(string create_by) {
            //var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            XFilesRef n = new XFilesRef();
            var loc = GetFileGoLogin();
            n.FileID = "";
            n.RCompanyID = "";
            n.DocID = "";
            n.RootPathID = loc.RootPathID;
            n.DBName = "";
            n.DBServer = "";
            n.AppID = loc.AppID;
            n.DocLineNum = 0;
            n.CompanyID = "";
            n.SourceTable = "";
            n.DocType = "";
            n.DocCate = "";
            n.Remark = "";
            n.FileType = "";
            n.CreatedBy = create_by;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }
    public static xfile_ref NewXFilesRefPostgres(string create_by) {
            //var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            xfile_ref n = new xfile_ref();
            var loc = GetFileGoLoginPostgres();
            n.file_id = "";
            n.rcom_id = "";
            n.doc_id = "";
            n.rootpath_id = loc.rootpath_id;
            n.dbname = "";
            n.dbserver = "";
            n.appid = loc.appid;
            n.doc_linenum = 0;
            n.com_id = "";
            n.source_table = "";
            n.doctype = "";
            n.doccate = "";
            n.remark = "";
            n.filetype = "";
            n.created_by = create_by;
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = null;
            n.is_active = 1;
            return n;
        }
        #endregion

        #region helper
        public static string Convert2Base64(xfiles input) {
            string output = "";
            try {
                if (input != null) {
                    output = Convert.ToBase64String(input.data);
                } else {
                    byte[] imageArray = System.IO.File.ReadAllBytes($"{System.IO.Directory.GetCurrentDirectory()}{@"\wwwroot\assets\img\dogx.png"}");
                    output = Convert.ToBase64String(imageArray);

                }
            } catch (Exception) {


            }

            return output;
        }
        #endregion



    }
}
