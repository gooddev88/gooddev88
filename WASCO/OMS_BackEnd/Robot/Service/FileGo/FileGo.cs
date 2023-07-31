using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Robot.Data.DA.DataHelper;
using Robot.Data.GADB.TT;
using Robot.Service.Api;
using Robot.Service.FileGo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Service.FileGo
{
    public class FileGo {


        private ProtectedLocalStorage _protectedLocalStore;
        private readonly ClientService _clientService;
        public FileGo(ClientService clientService, ProtectedLocalStorage protectedLocalStore) {
            _clientService = clientService;
            _protectedLocalStore = protectedLocalStore;
        }
        #region type
        public static string Type_MasterProfile { get; set; } = "MASTERTYPE_PHOTO_PROFILE";
        public static string Type_VendorProfile { get; set; } = "VENDORS_PHOTO_PROFILE";
        public static string Type_ItemProfile { get; set; } = "ITEMS_PHOTO_PROFILE";

        public static string Type_CompanyProfile { get; set; } = "COMPANY_PHOTO_PROFILE";
     public static string Type_CompanySignatureAll { get; set; } = "SIGNATURE_COMPANY_ALL";
        public static string Type_UserSignatureAll { get; set; } = "SIGNATURE_USER_ALL";
        public static string Type_CustomerProfile { get; set; } = "CUSTOMERS_PHOTO_PROFILE";

        public static string Type_NewsCate { get; set; } = "NEWCATE_PHOTO_PROFILE";
        public static string Type_PersonProfile { get; set; } = "PERSON_PHOTO_PROFILE";

        public static string Type_leavedocument { get; set; } = "LEAVE_DOCUMENT";
        public static string Type_reqdocument { get; set; } = "REQ_DOCUMENT";

        public static string Type_SODocument{ get; set; } = "SO_DOCUMENT";
        #endregion


        #region Authen Api
        public async Task<I_BasicResult> LoginApi() {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {


                var login = GetFileGoLogin();
                LoginRequest data_login = new LoginRequest { Apps = Globals.AppID, UserName = login.LoginName, Password = login.LoginPass, RememberMe = true };

                MyTokenResponse output = new MyTokenResponse { };
                var query = await Task.Run(() => _clientService.Post<MyTokenResponse>($"{login.RootUrl}/api/authen/Login/jwtApilogin", data_login,""));
                if (query.StatusCode != "OK") {
                    output.Success = false;
                    output.Error = query.StatusCode;
                } else {
                    output = (MyTokenResponse)query.Result;
                    if (output.Success) {
                        //await _protectedLocalStore.SetAsync(Globals.AuthToken, output.Token); 
                        var rr = SetJwt(output.Token, output.RefreshToken, output.RefreshTokenExpiryTime);

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
        public static XFileLocation GetFileGoLogin() {
            XFileLocation output = new XFileLocation();
            using (GAEntities db = new GAEntities()) {
                output = db.XFileLocation.Where(o => o.IsActive && o.IsCurrent && o.PathType == "FILEGO").FirstOrDefault();
                output.RootUrlPublic = String.IsNullOrEmpty(output.RootUrlPublic) ? output.RootUrl : output.RootUrlPublic;


            }
            return output;
        }
        public static XFileLocation GetTestLogin() {
            XFileLocation output = new XFileLocation();
            using (GAEntities db = new GAEntities()) {
                output = db.XFileLocation.Where(o => o.IsActive && o.IsCurrent && o.PathType == "TEST").FirstOrDefault();

            }
            return output;
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
        async public Task<xfiles> GetFileInByte(string rcom, string com, string doctype, string docId) {
            //ดึงไฟล์เป็น binary
            xfiles output = new xfiles();
            using (GAEntities db = new GAEntities()) {
                var f = db.vw_XFilesRef.Where(o => o.IsActive && o.RCompanyID == rcom && o.CompanyID == com && o.DocID == docId && o.DocType == doctype && o.PathType == "FILEGO").OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (f != null) {
                    string url = f.RootUrlPublic + @"/api/xfiles/XFilesService/GetFileInByte/" + f.FileID;


                    var query = await Task.Run(() => _clientService.GetAllAsync<xfiles>(url,""));
                    if (query.StatusCode != "OK") {
                        return null;
                    } else {
                        output = (xfiles)query.Result;
                        return output;
                    }
                } else {
                    return null;
                }
            }
            return output;
        }

        public static List<vw_XFilesRef_Extend> ListFileWithFileUrl(string docid, string doctype) {
            //ดึงไฟล์เป็น array can bind direct tag image
            List<vw_XFilesRef_Extend> result = new List<vw_XFilesRef_Extend>();
            using (GAEntities db = new GAEntities()) {
                var XFilesRef = db.vw_XFilesRef.Where(o => o.DocID == docid && o.DocType == doctype && o.IsActive).OrderBy(o => o.CreatedDate).ToList();
                if (XFilesRef.Count() > 0) {
                    foreach (var l in XFilesRef) {
                        vw_XFilesRef_Extend n = new vw_XFilesRef_Extend();
                        n.RCompanyID = l.CompanyID;
                        n.CompanyID = l.CompanyID;
                        n.AppID = l.AppID;
                        n.DBServer = l.DBServer;
                        n.DBName = l.DBName;
                        n.RootPathID = l.RootPathID;
                        n.RootUrl = l.RootUrl;
                        n.RootUrlPublic = l.RootUrlPublic;
                        n.RootPath = l.RootPath;
                        n.PathType = l.PathType;
                        n.FileID = l.FileID;
                        n.DocID = l.DocID;
                        n.DocLineNum = l.DocLineNum;
                        n.SourceTable = l.SourceTable;
                        n.DocType = l.DocType;
                        n.DocCate = l.DocCate;
                        n.FileType = l.FileType;
                        n.FileName = l.FileName;
                        n.FileExt = l.FileExt;
                        n.FilePath = l.FilePath;
                        n.SubUrl = l.SubUrl;
                        n.OriginFileName = l.OriginFileName;
                        n.OriginFileExt = l.OriginFileExt;
                        n.OriginFilePath = l.OriginFilePath;
                        n.Remark = l.Remark;
                        n.FullPathAndFile = l.FullPathAndFile;
                        n.FullUrlAndFile = l.FullUrlAndFile;
                        n.CreatedBy = l.CreatedBy;
                        n.CreatedDate = l.CreatedDate;
                        n.ModifiedBy = l.ModifiedBy;
                        n.ModifiedDate = l.ModifiedDate;
                        n.IsActive = l.IsActive;
                        n.file_api_url = l.RootUrlPublic + "/api/xfiles/XFilesService/GetFile/" + l.FileID;
                        result.Add(n);
                    }
                }
            }
            return result;
        }
        public static vw_XFilesRef GetFileId(string rcom, string com, string doctype, string docId) {
            vw_XFilesRef refx= new vw_XFilesRef();
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
        public static string GetFileUrl(string file_id ) {
            string url = "";
            using (GAEntities db = new GAEntities()) {
                var f = db.vw_XFilesRef.Where(o => o.FileID == file_id).FirstOrDefault();
                if (f != null) {
                    url = f.RootUrlPublic + @"/api/xfiles/XFilesService/GetFile/" + f.FileID;
                }
            }
            return url;
        }
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
        async public   Task<I_BasicResult> UploadFileGo(List<FilesInfo> files,string user,string token_filego) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try { 
                var login_filego = FileGo.GetFileGoLogin();
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
                        var xfile_ref = FileGo.Convert2XFilesRef(files, user);
                        var rrr = FileGo.SaveXFileRef(xfile_ref);
                        if (rrr.Result=="fail") {
                            r.Result = "fail";
                            r.Message1 = rrr.Message1;
                        }
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException!=null) {
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


        async public Task<I_BasicResult> DeleteOldFileAfterSave(string rcom, string com, string doctype, string docid, string deleted_by) {
            //ใช้ลบ file อัพโหลดแบบทับไฟล์เก่าเช่นรูปโปรไฟล์บริษัท หรือ สินค้า 
            //ลบแบบหลายไฟล์
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
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
                    var login_filego = FileGo.GetFileGoLogin();
                    var query = await Task.Run(() => _clientService.Post<I_BasicResult>($"{latest_file.RootUrl}/api/xfiles/XFilesService/DeleteFile/", delete_file_ids, login_filego.JwtToken));
                    if (query.StatusCode == "OK") {

                        if (delete_file_ids.Count > 0) {
                            var update = db.XFilesRef.Where(o => delete_file_ids.Contains(o.FileID)).ToList();
                            foreach (var f in update) {
                                f.ModifiedBy = deleted_by;
                                f.ModifiedDate = DateTime.Now;
                                f.IsActive = false;

                            }
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
        async public Task<I_BasicResult> DeleteFile(string rcom, string com, string doctype, string docid, string deleted_by) {
            //ใช้ลบ file แบบทีละไฟล์อย่างเช่นลบรูปสินค้า
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var f = db.vw_XFilesRef.Where(o => o.RCompanyID == rcom && o.CompanyID == com && o.DocType == doctype && o.DocID == docid && o.IsActive).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                    if (f == null) {
                        return r;
                    }
                    List<string> file_ids = new List<string> { f.FileID };
                    var login_filego = FileGo.GetFileGoLogin();
                    var query = await Task.Run(() => _clientService.Post<I_BasicResult>($"{f.RootUrl}/api/xfiles/XFilesService/DeleteFile/", file_ids, login_filego.JwtToken));
                    if (query.StatusCode == "OK") {
                        var ff = db.XFilesRef.Where(o => o.FileID == f.FileID).FirstOrDefault();
                        if (ff!=null) {
                            ff.ModifiedBy = deleted_by;
                            ff.ModifiedDate = DateTime.Now;
                            ff.IsActive = false;
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

        async public Task<I_BasicResult> DeleteFileByFileId(string rcom, string com, string doctype, string docid,string fileid,string deleted_by) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    var f = db.vw_XFilesRef.Where(o => o.RCompanyID == rcom && o.CompanyID == com && o.DocType == doctype && o.DocID == docid && o.IsActive).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                    if (f == null) {
                        return r;
                    }
                    List<string> file_ids = new List<string> { fileid };
                    var login_filego = FileGo.GetFileGoLogin();
                    var query = await Task.Run(() => _clientService.Post<I_BasicResult>($"{f.RootUrl}/api/xfiles/XFilesService/DeleteFile/", file_ids, login_filego.JwtToken));
//var query = await Task.Run(() => _clientService.Post<I_BasicResult>($"{"https://localhost:7159"}/api/xfiles/XFilesService/DeleteFile/", file_ids, login_filego.JwtToken));
                    if (query.StatusCode == "OK") {
                        var ff = db.XFilesRef.Where(o => o.FileID == f.FileID).FirstOrDefault();
                        if (ff != null) {
                            ff.ModifiedBy = deleted_by;
                            ff.ModifiedDate = DateTime.Now;
                            ff.IsActive = false;
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

        #endregion




        #region Init
        //public static XFilesRef NewXFileRefTemplate(string createdBy) {
        //    var u = NewXFilesRef();
        //    u.DocID = docid;
        //    u.CompanyID = com;
        //    u.RCompanyID = rcom;
        //    u.CreatedBy = createdBy;
        //    u.SourceTable = "";
        //    u.DocType = doctype;
        //    u.DocCate = "";
        //    u.Remark = "";
        //    u.FileType = "image/";
        //    return u;
        //}
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
        //public static List<FilesInfo> Convert2FileInfo(XFilesRef i, string data) {
        //    List<FilesInfo> o = new List<FilesInfo>();
        //    var loc = GetFileGoLogin();
        //    o.Add(new FilesInfo {
        //        data = data,
        //        file_type = i.FileType,
        //        fileName = i.FileName,
        //        app_id = "FileGo",
        //        com_id = "",
        //        rcom_id = "",
        //        doc_id = "",
        //        doc_linenum = 0,
        //        doc_type = "",
        //        doc_cate = "",
        //        file_id = i.FileID,
        //        owner_id = loc.LoginName
        //    });


        //    return o;
        //}

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
