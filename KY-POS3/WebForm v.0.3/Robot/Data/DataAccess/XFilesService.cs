﻿using Dapper;
using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using static Robot.Data.BL.I_Result;

namespace Robot.Data.DataAccess {
    public static class XFilesService {
        #region Class




        public class XFilesSet {
            public XFiles XFiles { get; set; }
            public XFilesRef XFilesRef { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }
        public class XUploadOption {
            public string Topic { get; set; }
            public string Description { get; set; }
            public bool IsReplace { get; set; }
            public bool IsFinishUpload { get; set; }
            public int ImgWeight { get; set; }
            public int ImgHeight { get; set; }
        }

        public class XUploadInfo {
            public XFilesRef XFilesRef { get; set; }
            public XUploadOption UploadOption { get; set; }
            public I_BasicResult OutputAction { get; set; }
        }

        #endregion


        public static XUploadInfo UploadInfo { get { return (XUploadInfo)HttpContext.Current.Session["upload_info"]; } set { HttpContext.Current.Session["upload_info"] = value; } }


        #region   File
        public static string GetConnectionString(string conName) {
            //conName= GAConnectionString 
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings[conName].ConnectionString;
            //string con = System.Configuration.ConfigurationManager.ConnectionStrings["myDbConnection"].ConnectionString;

            return conStr;
        }

        public static string CreateDBConnect2FileDB() {
            string conStr = "";
            using (GAEntities db = new GAEntities()) {
                var c = db.XFileLocation.Where(o => o.IsCurrent && o.IsActive).FirstOrDefault();
                conStr = $"data source={c.DBServer};initial catalog={c.DBName};persist security info=True;user id={c.LoginName};password={c.LoginPass};MultipleActiveResultSets=True";
            }
            return conStr;
        }
        public static string CreateDBConnect2FileDB(vw_XFilesRef i) {
            string conStr = "";
            using (GAEntities db = new GAEntities()) {
                var c = db.XFileLocation.Where(o => o.DBName == i.DBName && o.IsActive).FirstOrDefault();
                conStr = $"data source={i.DBServer};initial catalog={i.DBName};persist security info=True;user id={c.LoginName};password={c.LoginPass};MultipleActiveResultSets=True";
            }
            return conStr;
        }
        public static XFileLocation GetCurrentFileLocation() {
            XFileLocation result = new XFileLocation();
            using (GAEntities db = new GAEntities()) {
                result = db.XFileLocation.Where(o => o.IsCurrent == true && o.IsActive == true && o.PathType == "DB").FirstOrDefault();

            }
            return result;
        }


        public static XFiles GetFileImageThumb(string xfileId) {
            XFiles result = new XFiles();
            try {
                string conStr = CreateDBConnect2FileDB();
                using (var connection = new SqlConnection(conStr)) {
                    string sql = "select FileID,FileName,FileType,FileExt,OriginFileName,DataThumb,DataInBase64 from XFiles where FileID=@FileID ";

                    var query = connection.Query<XFiles>(sql, new { @FileID = xfileId }).FirstOrDefault();

                }
            } catch {
            }
            return result;
        }
        public static XFiles GetFile(string xfileId) {
            XFiles result = new XFiles();
            var fileref = GetFileRefInfo(xfileId);
            try {
                string conStr = CreateDBConnect2FileDB(fileref);
                using (var connection = new SqlConnection(conStr)) {
                    string sql = "select FileID,FileName,FileType,FileExt,OriginFileName,Data,DataThumb,DataInBase64 ,CreatedDate ,CreatedBy from XFiles where FileID=@FileID ";

                    result = connection.Query<XFiles>(sql, new { @FileID = xfileId }).FirstOrDefault();

                }
            } catch {
            }
            return result;
        }
        public static I_BasicResult UpdateFileThumb(string fileId, byte[] dataThumb) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            var fileref = GetFileRefInfo(fileId);
            try {
                string conStr = CreateDBConnect2FileDB(fileref);
                using (var connection = new SqlConnection(conStr)) {
                    string sql = @"update XFiles 
                                    set DataThumb=@DataThumb
                                where FileID=@FileID ";
                    var rr = connection.Query(sql, new { @FileID = fileId, @DataThumb = dataThumb }).FirstOrDefault();

                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }
            return result;
        }
        public static long AddNewFile(XFiles f) {
            string strCon = CreateDBConnect2FileDB();
            using (SqlConnection conn = new SqlConnection(strCon)) {
                conn.Open();

                string sql = @"insert into  XFiles  (
                               AppID
                              ,FileID
                              ,FileName
                              ,FileType
                              ,FileExt
                              ,FilePath
                              ,OriginFileName
                              ,OriginFileExt
                              ,OriginFilePath
                              ,Data
                              ,DataThumb
                              ,DataInBase64
                              ,Remark
                              ,CreatedBy
                              ,CreatedDate
                              ,ModifiedBy
                              ,ModifiedDate
                              ,IsActive
	                          ) values(
                               @AppID
                              , @FileID
                              , @FileName
                              , @FileType
                              , @FileExt
                              , @FilePath
                              , @OriginFileName
                              , @OriginFileExt
                              , @OriginFilePath
                              , @Data
                              , @DataThumb
                              , @DataInBase64
                              , @Remark
                              , @CreatedBy
                              , @CreatedDate
                              , @ModifiedBy
                              , @ModifiedDate
                              , @IsActive
                         )";
                return conn.Execute(sql, new {
                    @AppID = f.AppID,
                    @FileID = f.FileID,
                    @FileName = f.FileName,
                    @FileType = f.FileType,
                    @FileExt = f.FileExt,
                    @FilePath = f.FilePath,
                    @OriginFileName = f.OriginFileName,
                    @OriginFileExt = f.OriginFileExt,
                    @OriginFilePath = f.OriginFilePath,
                    @Data = f.Data,
                    @DataThumb = f.DataThumb,
                    @DataInBase64 = f.DataInBase64,
                    @Remark = f.Remark,
                    @CreatedBy = f.CreatedBy,
                    @CreatedDate = f.CreatedDate,
                    @ModifiedBy = f.ModifiedBy,
                    @ModifiedDate = f.ModifiedDate,
                    @IsActive = f.IsActive

                });

            }
        }


        public static int DeleteFile(string fileId) {
            int r = -2;
            try {
                string conStr = CreateDBConnect2FileDB();
                using (var connection = new SqlConnection(conStr)) {
                    string sql = "delete XFiles where FileID = @FileID";
                    r = connection.Execute(sql, new { @FileID = fileId });
                }
            } catch {
            }
            return r;
        }
        #endregion



        #region Query 
        public static XFilesSet GetDocSet(string fileId) {
            XFilesSet n = NewTransaction("");
            try {
                using (GAEntities db = new GAEntities()) {
                    // n.XFiles = db.XFiles.Where(o => o.FileID == fileId).FirstOrDefault();
                    n.XFilesRef = db.XFilesRef.Where(o => o.FileID == fileId).FirstOrDefault();
                    n.XFiles = GetFile(n.XFiles.FileID);
                }
            } catch (Exception ex) {
            }
            return n;
        }


        public static XFiles GetFileFromDocInfo(string docId, string doctype) {
            XFiles result = new XFiles();
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            var com = LoginService.LoginInfo.CurrentCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                var fidS = db.XFilesRef.Where(o => o.DocID == docId
                                                                        && o.DocType == doctype
                                                                        && o.RCompanyID == rcom
                                                                        && (o.CompanyID == com || o.CompanyID == "")
                                                                        && o.IsActive
                                                                        ).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (fidS == null) {
                    return null;
                }
                result = GetFile(fidS.FileID);
            }
            return result;
        }


        public static string ConvertImageByte2Base64(XFiles file) {
            if (file.Data == null) {
                return "";
            }
            return @"data:Image/" + file.FileExt + ";base64," + Convert.ToBase64String(file.Data);
        }

        public static Image ConvertImageByte2Image(XFiles file) {
            Image result = null;
            if (file.Data == null) {
                return result;
            }
            using (var ms = new MemoryStream(file.Data)) {
                result = Image.FromStream(ms);
            }
            return result;
        }


        public static XFiles GetFileFromDocInfo(string docId, string docType, string docCate) {
            XFiles result = new XFiles();
            using (GAEntities db = new GAEntities()) {
                var fidS = db.XFilesRef.Where(o => o.DocType == docType && o.DocID == docId && o.DocCate == docCate && o.IsActive).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (fidS == null) {
                    return null;
                }
                result = GetFile(fidS.FileID);
            }
            return result;
        }

        public static XFiles GetFileImageThumbFromDocInfo(string docId, string docType, string docCate) {
            XFiles result = new XFiles();
            using (GAEntities db = new GAEntities()) {
                var fidS = db.XFilesRef.Where(o => o.DocType == docType && o.DocID == docId && o.DocCate == docCate && o.IsActive).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (fidS == null) {
                    return null;
                }
                result = GetFileImageThumb(fidS.FileID);
            }
            return result;
        }
        public static List<XFiles> ListFileFromDocInfo(string docId, string docType, string docCate) {
            List<XFiles> result = new List<XFiles>();
            using (GAEntities db = new GAEntities()) {
                var fidS = db.XFilesRef.Where(o => o.DocType == docType && o.DocID == docId && o.DocCate == docCate && o.IsActive).OrderByDescending(o => o.CreatedDate).ToList();
                foreach (var f in fidS) {
                    result.Add(GetFile(f.FileID));
                }

            }
            return result;
        }

        public static List<XFilesRef> ListFileRefByDocAndTableSource(string docid, string sourcetable, string cateId = "") {
            List<XFilesRef> result = new List<XFilesRef>();
            using (GAEntities db = new GAEntities()) {
                result = db.XFilesRef.Where(o => o.DocID == docid && o.SourceTable == sourcetable
                                                                                    && (o.DocCate == cateId || cateId == "")
                                            ).OrderByDescending(o => o.CreatedDate).ToList();
            }
            return result;
        }
        public static List<XFilesRef> ListFileRefByDocAndTableSource(List<string> docids, string sourcetable, string cateId = "") {
            List<XFilesRef> result = new List<XFilesRef>();
            using (GAEntities db = new GAEntities()) {
                result = db.XFilesRef.Where(o => docids.Contains(o.DocID) && o.SourceTable == sourcetable
                                                                                    && (o.DocCate == cateId || cateId == "")
                                            ).OrderByDescending(o => o.CreatedDate).ToList();
            }
            return result;
        }
        public static XFilesRef GetFileRefByDocAndTableSource(string docid, string sourcetable, string docCate = "") {
            XFilesRef xref = new XFilesRef();
            using (GAEntities db = new GAEntities()) {
                xref = db.XFilesRef.Where(o => o.DocID == docid && o.SourceTable == sourcetable
                                                                                        && (o.DocCate == docCate || docCate == "")
                                                                                        ).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
            }
            return xref;
        }


        public static string GetFileRefByDocAndTableSource2B64(string comId, string docid, string doctype, bool getThumb = false) {
            string b64Base = "";
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            using (GAEntities db = new GAEntities()) {
                var xref = db.vw_XFilesRef.Where(o => o.DocID == docid

                                                        && o.RCompanyID == rcom
                                                        && (o.CompanyID == comId || comId == "")
                                                        && (o.DocType == doctype)
                                                        ).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                if (xref != null) {
                    if (getThumb) {
                        var f = XFilesService.GetFile(xref.FileID);
                        b64Base = XFilesService.ConvertImageByte2Base64(f);
                    } else {
                        var f = XFilesService.GetFileImageThumb(xref.FileID);
                        b64Base = XFilesService.ConvertImageByte2Base64(f);
                    }

                }
            }
            return b64Base;
        }

        public static vw_XFilesRef GetFileRefInfo(string fileId) {
            vw_XFilesRef n = new vw_XFilesRef();
            try {
                using (GAEntities db = new GAEntities()) {
                    n = db.vw_XFilesRef.Where(o => o.FileID == fileId).FirstOrDefault();
                }
            } catch (Exception ex) {
            }
            return n;
        }


        public static XFilesSet GoNext(string fileId) {
            XFilesSet result = NewTransaction("");
            try {
                using (GAEntities db = new GAEntities()) {
                    var curr = db.XFilesRef.Where(o => o.FileID == fileId).FirstOrDefault();
                    var next = db.XFilesRef.Where(o => o.ID > curr.ID && o.DocID == curr.DocID && o.DocType == curr.DocType).OrderBy(o => o.ID).FirstOrDefault();

                }
            } catch (Exception ex) {
            }

            return result;
        }
        #endregion

        #region  action
        public static I_BasicResult Save(XFilesSet data, bool isReplace) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    data = CalDocSet(data);
                    var fileR = data.XFilesRef;
                    string fileId = fileR.FileID;
                    var chkExitFile = db.XFilesRef.Where(o => o.DocID == fileR.DocID && o.SourceTable == fileR.SourceTable && o.DocType == fileR.DocType && o.DocCate == fileR.DocCate).ToList();
                    var x = data.XFilesRef;
                    if (chkExitFile.Count > 0) {
                        //ถ้ามี file id แล้วและไม่ replace
                        if (!isReplace) {
                            //ให้ gen file ใหม่
                            data.XFiles.FileID = Guid.NewGuid().ToString();
                            data.XFilesRef.FileID = data.XFiles.FileID;
                            db.XFilesRef.Add(data.XFilesRef);
                            db.SaveChanges();
                            AddNewFile(data.XFiles);
                        } else {//ถ้าให้ replace
                                //set fileID เดิมให้ก่อนจะลบแล้ว insert ใหม่
                            var existF = chkExitFile.OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                            data.XFiles.FileID = existF.FileID;
                            data.XFilesRef.FileID = existF.FileID;

                            db.XFilesRef.RemoveRange(chkExitFile);
                            db.XFilesRef.Add(data.XFilesRef);
                            db.SaveChanges();
                            foreach (var c in chkExitFile) {
                                DeleteFile(c.FileID);
                            }
                            AddNewFile(data.XFiles);
                        }

                    } else {// ถ้า ไม่มี file id 
                        db.XFilesRef.Add(data.XFilesRef);
                        db.SaveChanges();
                        AddNewFile(data.XFiles);
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }

            }
            return result;
        }
        public static XFilesSet CalDocSet(XFilesSet data) {
            //1 copy data from xfile to file ref
            var x = data.XFiles;
            data.XFilesRef.FileName = x.FileName;
            data.XFilesRef.FileExt = x.FileExt;
            data.XFilesRef.FilePath = x.FilePath;
            data.XFilesRef.OriginFileName = x.OriginFileName;
            data.XFilesRef.OriginFileExt = x.OriginFileExt;
            data.XFilesRef.OriginFilePath = x.OriginFilePath;
            return data;
        }
        public static I_BasicResult DeleteFileByFileId(string fileId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (GAEntities db = new GAEntities()) {
                    db.XFilesRef.Remove(db.XFilesRef.Where(o => o.FileID == fileId).FirstOrDefault());
                    db.SaveChanges();
                    DeleteFile(fileId);

                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }

            }
            return result;
        }
        public static I_BasicResult DeleteFileByDocInfo(string docId, string docType, string docCate) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {

                using (GAEntities db = new GAEntities()) {
                    var fidS = db.XFilesRef.Where(o => o.DocType == docType && o.DocID == docId && o.DocCate == docCate).Select(o => o.FileID).ToList();

                    foreach (var fileId in fidS) {

                        db.XFilesRef.Remove(db.XFilesRef.Where(o => o.FileID == fileId).FirstOrDefault());
                        db.SaveChanges();
                        DeleteFile(fileId);
                    }


                }

            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }

            }
            return result;
        }
        #endregion

        #region new trancation

        public static XFilesSet NewTransaction(string createdBy) {
            XFilesSet n = new XFilesSet();
            n.XFilesRef = NewXFilesRef(createdBy);
            n.XFiles = NewXFiles(n.XFilesRef);

            return n;
        }
        public static XFilesRef NewXFilesRef(string createdBy) {
            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;
            XFilesRef n = new XFilesRef();
            var loc = GetCurrentFileLocation();
            n.FileID = Guid.NewGuid().ToString();
            n.RCompanyID = rcom;
            n.DocID = "";
            n.RootPathID = loc.RootPathID;
            n.DBName = loc.DBName;
            n.DBServer = loc.DBServer;
            n.AppID = loc.AppID;
            n.DocLineNum = 0;
            n.CompanyID = "";
            n.SourceTable = "";
            n.DocType = "";
            n.DocCate = "";
            n.Remark = "";
            n.FileType = "FILE";
            n.CreatedBy = createdBy;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
            return n;
        }
        public static XFiles NewXFiles(XFilesRef i) {

            XFiles n = new XFiles();
            n.FileID = i.FileID;
            n.FileName = i.FileID;
            n.AppID = i.AppID;
            n.FileExt = "";
            n.FilePath = "";
            n.OriginFileName = "";
            n.OriginFileExt = "";
            n.OriginFilePath = "";
            n.FileType = "FILE";
            n.Data = null;
            n.DataInBase64 = "";
            n.Remark = "";
            n.CreatedBy = i.CreatedBy;
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;

            return n;
        }

        public static XUploadInfo NewXUploadInfo(string createdBy) {

            var rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;

            XUploadInfo n = new XUploadInfo();
            n.XFilesRef = NewXFilesRef(createdBy);

            n.XFilesRef.RCompanyID = rcom;
            n.XFilesRef.FileType = "FILE";
            n.XFilesRef.CompanyID = "";
            n.XFilesRef.DocID = "";
            n.XFilesRef.DocType = "";
            n.XFilesRef.DocCate = "";
            n.XFilesRef.Remark = "";
            n.XFilesRef.CreatedBy = "";
            n.XFilesRef.CreatedDate = DateTime.Now;

            n.UploadOption = new XUploadOption();
            n.UploadOption.Topic = "";
            n.UploadOption.Description = "";
            n.UploadOption.IsReplace = true;
            n.UploadOption.IsFinishUpload = false;
            n.UploadOption.ImgWeight = 0;
            n.UploadOption.ImgHeight = 0;
            n.OutputAction = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            return n;
        }

        public static XFilesRef CopyUploadInfo2FileRef(XFilesRef u, XFilesRef o) {

            //f.FileID = Guid.NewGuid().ToString();
            o.DocID = u.DocID;
            o.DocLineNum = u.DocLineNum;
            o.CompanyID = u.CompanyID;
            o.SourceTable = u.SourceTable;
            o.DocType = u.DocType;
            o.DocCate = u.DocCate;
            o.Remark = u.Remark;
            o.FileType = u.FileType;
            o.CreatedBy = u.CreatedBy;

            return o;
        }
        #endregion


        #region newtransaction template
        public static XUploadInfo NewTemplateUploadCustomerProfile(string custId, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = custId;
            u.XFilesRef.SourceTable = "CUSTOMERS";
            u.XFilesRef.DocType = "CUSTOMERS_PHOTO_PROFILE";
            u.XFilesRef.DocCate = "CUSTOMERS_PHOTO_PROFILE";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "IMG";


            u.UploadOption.Topic = "Upload customer profile";
            u.UploadOption.IsReplace = true;
            u.UploadOption.IsFinishUpload = false;
            u.UploadOption.ImgHeight = 800;
            u.UploadOption.ImgWeight = 800;

            return u;
        }

        public static XUploadInfo NewTemplateUploadCompanyProfile(string comid, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = comid;
            u.XFilesRef.SourceTable = "COMPANY";
            u.XFilesRef.DocType = "COMPANY";
            u.XFilesRef.DocCate = "COMPANY_PHOTO_PROFILE";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "IMG";
            u.UploadOption.Topic = "Upload Company profile";
            u.UploadOption.IsReplace = true;
            u.UploadOption.IsFinishUpload = false;
            u.UploadOption.ImgHeight = 800;
            u.UploadOption.ImgWeight = 800;
            return u;
        }

        public static XUploadInfo NewTemplateUploadVendorProfile(string vendorId, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = vendorId;
            u.XFilesRef.SourceTable = "VENDORS";
            u.XFilesRef.DocType = "VENDORS_PHOTO_PROFILE";
            u.XFilesRef.DocCate = "VENDORS_PHOTO_PROFILE";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "IMG";


            u.UploadOption.Topic = "Upload vendor profile";
            u.UploadOption.IsReplace = true;
            u.UploadOption.IsFinishUpload = false;
            u.UploadOption.ImgHeight = 600;
            u.UploadOption.ImgWeight = 600;

            return u;
        }

        public static XUploadInfo NewTemplateUploadItemsProfile(string vendorId, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = vendorId;
            u.XFilesRef.SourceTable = "ITEMS";
            u.XFilesRef.DocType = "ITEMS_PHOTO_PROFILE";
            u.XFilesRef.DocCate = "ITEMS_PHOTO_PROFILE";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "IMG";


            u.UploadOption.Topic = "Upload items profile";
            u.UploadOption.IsReplace = true;
            u.UploadOption.IsFinishUpload = false;
            u.UploadOption.ImgHeight = 600;
            u.UploadOption.ImgWeight = 600;

            return u;
        }

        public static XUploadInfo NewTemplateUploadMasterTypeProfile(string custId, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = custId;
            u.XFilesRef.SourceTable = "MASTERTYPE";
            u.XFilesRef.DocType = "MASTERTYPE_PHOTO_PROFILE";
            u.XFilesRef.DocCate = "MASTERTYPE_PHOTO_PROFILE";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "IMG";

            u.UploadOption.Topic = "Master type image";
            u.UploadOption.IsReplace = true;
            u.UploadOption.IsFinishUpload = false;


            return u;
        }

        public static XUploadInfo NewTemplateUploadVendorItemProfile(string custId, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = custId;
            u.XFilesRef.SourceTable = "VENDORITEM";
            u.XFilesRef.DocType = "VENDORITEM_PHOTO_PROFILE";
            u.XFilesRef.DocCate = "VENDORITEM_PHOTO_PROFILE";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "IMG";

            u.UploadOption.Topic = "VendorItem image";
            u.UploadOption.IsReplace = true;
            u.UploadOption.IsFinishUpload = false;


            return u;
        }

        public static XUploadInfo NewTemplateUploadClock(string fileId, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = fileId;
            u.XFilesRef.SourceTable = "CLOCK";
            u.XFilesRef.DocType = "CLOCK";
            u.XFilesRef.DocCate = "CLOCK";
            u.XFilesRef.Remark = "";
            u.XFilesRef.FileType = "IMG";


            u.UploadOption.Topic = "Upload clock in out";
            u.UploadOption.IsReplace = true;
            u.UploadOption.IsFinishUpload = false;
            u.UploadOption.ImgHeight = 600;
            u.UploadOption.ImgWeight = 600;

            return u;
        }
        public static XUploadInfo NewTemplateUploadFileDocument(string fileId, string docType, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = fileId;
            u.XFilesRef.SourceTable = "DOCUMENT";
            u.XFilesRef.DocType = docType;
            u.XFilesRef.DocCate = "FILE";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "FILE";


            u.UploadOption.Topic = "Upload Contranct request document";
            u.UploadOption.IsReplace = false;
            u.UploadOption.IsFinishUpload = false;
            u.UploadOption.ImgHeight = 600;
            u.UploadOption.ImgWeight = 600;

            return u;
        }


        public static XUploadInfo NewTemplateUploadFilePlant(string fileId, string docType, string fullname, string createdBy) {
            var u = NewXUploadInfo(createdBy);
            u.XFilesRef.DocID = fileId;
            u.XFilesRef.SourceTable = "PLANT";
            u.XFilesRef.DocType = docType;
            u.XFilesRef.DocCate = "IMG_PLANT";
            u.XFilesRef.Remark = fullname;
            u.XFilesRef.FileType = "IMG";

            u.UploadOption.Topic = "Upload IMG Plant document";
            u.UploadOption.IsReplace = false;
            u.UploadOption.IsFinishUpload = false;
            u.UploadOption.ImgHeight = 600;
            u.UploadOption.ImgWeight = 600;

            return u;
        }
        #endregion

        #region image helper
        public static byte[] ResizeImage(byte[] data, int width, int height) {
            if (width == 0 || height == 0) {
                return data;
            }
            var img = ImageService.ConvertByteToImage(data);
            var resize = ImageService.ResizeImage(img, width, height, true);
            data = ImageService.ConvertImageToByte(resize);
            return data;
        }
        #endregion


        #region File download
        //public static XFiles GetFileByFileID(string fileId) {
        //    XFiles result = new XFiles();
        //    using (GAFileStoreEntities db = new GAFileStoreEntities()) {
        //        result = db.XFiles.Where(o => o.FileID == fileId && o.IsActive).FirstOrDefault();
        //    }
        //    return result;

        //}

        public static void DownLoadServerFile(string server_part, string file_name) {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=\"" + file_name + "\"");
            byte[] data = req.DownloadData(server_part);
            response.BinaryWrite(data);
            response.End();
        }


        public static void DownLoadFile(XFiles file) {
            try {
                HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.ContentType = "application/" + file.OriginFileExt;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.OriginFileName + file.OriginFileExt + "\"");
                HttpContext.Current.Response.AddHeader("Content-Length", Convert.ToString(file.Data.Length));
                HttpContext.Current.Response.BinaryWrite(file.Data);
                HttpContext.Current.Response.End();
            } catch (Exception ex) {

            }



        }
        public static I_BasicResult ConvertByte2File(string docId, string type) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                //  string rcom = LoginService.LoginInfo.CurrentRootCompany.CompanyID;

                using (GAEntities db = new GAEntities()) {

                    List<vw_XFilesRef> fileRef = new List<vw_XFilesRef>();
                    if (!string.IsNullOrEmpty(docId)) {
                        fileRef = db.vw_XFilesRef.Where(o => o.DocID == docId && o.DocType == type).ToList();
                    } else {
                        fileRef = db.vw_XFilesRef.Where(o => o.DocCate == type).ToList();
                    }

                    foreach (var r in fileRef) {
                        var x = GetFile(r.FileID);
                        if (x == null) {
                            continue;
                        }
                        #region Create Directory
                        String pathOriginal = HttpContext.Current.Server.MapPath($"~/ImageStorage/{r.RCompanyID}"); //Path full size
                        String pathThumb = HttpContext.Current.Server.MapPath($"~/ImageStorage/{r.RCompanyID}/Thumb"); //Path thumb size
                                                                                                                       //Check if directory exist
                        if (!Directory.Exists(pathOriginal)) {
                            Directory.CreateDirectory(pathOriginal); //Create directory if it doesn't exist
                        }
                        if (!Directory.Exists(pathThumb)) {
                            Directory.CreateDirectory(pathThumb); //Create directory if it doesn't exist
                        }
                        #endregion
                        string imageName = r.DocID + x.FileExt;
                        //set the image path original size
                        string imgPathOriginal = Path.Combine(pathOriginal, imageName);
                        File.WriteAllBytes(imgPathOriginal, x.Data);
                        //var imgThumbData = ResizeImage(x.Data, 200, 200);
                        string imgPathThumb = Path.Combine(pathThumb, imageName);
                        var dataThumb = SaveCroppedImage(x.Data, 150, 150, imgPathThumb);
                        var rr = UpdateFileThumb(r.FileID, dataThumb);
                        //  File.WriteAllBytes(imgPathThumb, imgThumbData);
                        //if (x.DataThumb != null) {
                        //    string imgPathThumb = Path.Combine(pathThumb, imageName);
                        //    File.WriteAllBytes(imgPathThumb, x.DataThumb);
                        //} else {
                        //  var imgThumbData=  ResizeImage(x.Data, 200, 200);
                        //    string imgPathThumb = Path.Combine(pathThumb, imageName);
                        //    File.WriteAllBytes(imgPathThumb, x.DataThumb);
                        //}
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }

            }

            return result;
        }






        //public static void saveJpegAndCrop(string path, byte[] data,int width,int height, long quality) {
        //    TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
        //    Bitmap img = (Bitmap)tc.ConvertFrom(data);
        //    //crop image before


        //    // Encoder parameter for image quality
        //    EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);

        //    // Jpeg image codec
        //    ImageCodecInfo jpegCodec = getEncoderInfo("image/jpeg");

        //    if (jpegCodec == null)
        //        return;

        //    EncoderParameters encoderParams = new EncoderParameters(1);
        //    encoderParams.Param[0] = qualityParam;

        //    img.Save(path, jpegCodec, encoderParams);
        //}

        //public static   ImageCodecInfo getEncoderInfo(string mimeType) {
        //    // Get image codecs for all image formats
        //    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        //    // Find the correct image codec
        //    for (int i = 0; i < codecs.Length; i++)
        //        if (codecs[i].MimeType == mimeType)
        //            return codecs[i];
        //    return null;
        //}


        //private static Image cropImage(Image img, Rectangle cropArea) {

        //    Bitmap bmpImage = new Bitmap(img);
        //    Bitmap bmpCrop = bmpImage.Clone(cropArea,
        //    bmpImage.PixelFormat)
        //    return (Image)(bmpCrop);
        //}

        //public static Image Crop(byte[] data, int width, int height, int x, int y) {
        //    Bitmap bmp = null;
        //    try {
        //        Image image = null;
        //        using (var ms = new MemoryStream(data)) {
        //            image= Image.FromStream(ms);
        //        }

        //          bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
        //        bmp.SetResolution(80, 60);

        //        Graphics gfx = Graphics.FromImage(bmp);
        //        gfx.SmoothingMode = SmoothingMode.AntiAlias;
        //        gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //        gfx.DrawImage(image, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
        //        // Dispose to free up resources
        //        image.Dispose();
        //        bmp.Dispose();
        //        gfx.Dispose();


        //    } catch (Exception ex) { 
        //        return null;
        //    }
        //    return bmp;
        //}
        public static Bitmap MakeSquarePhoto(Bitmap bmp, int size) {
            Bitmap res = new Bitmap(size, size);
            try {

                Graphics g = Graphics.FromImage(res);
                g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size);
                int t = 0, l = 0;
                if (bmp.Height > bmp.Width)
                    t = (bmp.Height - bmp.Width) / 2;
                else
                    l = (bmp.Width - bmp.Height) / 2;
                g.DrawImage(bmp, new Rectangle(0, 0, size, size), new Rectangle(l, t, bmp.Width - l * 2, bmp.Height - t * 2), GraphicsUnit.Pixel);

            } catch { }
            ImageToByte(bmp);
            return res;
        }
        public static byte[] ImageToByte(Image img) {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        public static Bitmap ByteToBitmap(byte[] imgByte) {
            //using (MemoryStream memstr = new MemoryStream(imgByte)) {
            //    Image img = Image.FromStream(memstr);
            //    return img;
            //} 
            Bitmap bmp;
            using (var ms = new MemoryStream(imgByte)) {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }
        public static byte[] CreateThumb(byte[] input,int size) {
            byte[] output=null;
            try {
                var bitmap = ByteToBitmap(input);
                var bitmap_crop = MakeSquarePhoto(bitmap, size);
                output = ImageToByte(bitmap_crop);
            } catch (Exception) { 
            }

            return output;
        }
         
        public static byte[] SaveCroppedImage(byte[] data, int maxWidth, int maxHeight, string filePath) {
            byte[] returnImg = null;
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            Bitmap image = (Bitmap)tc.ConvertFrom(data);


            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders()
                                     .Where(codecInfo =>
                                     codecInfo.MimeType == "image/jpeg").First();
            Image finalImage = image;
            System.Drawing.Bitmap bitmap = null;
            try {
                int left = 0;
                int top = 0;
                int srcWidth = maxWidth;
                int srcHeight = maxHeight;
                bitmap = new System.Drawing.Bitmap(maxWidth, maxHeight);
                double croppedHeightToWidth = (double)maxHeight / maxWidth;
                double croppedWidthToHeight = (double)maxWidth / maxHeight;

                if (image.Width > image.Height) {
                    srcWidth = (int)(Math.Round(image.Height * croppedWidthToHeight));
                    if (srcWidth < image.Width) {
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    } else {
                        srcHeight = (int)Math.Round(image.Height * ((double)image.Width / srcWidth));
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    }
                } else {
                    srcHeight = (int)(Math.Round(image.Width * croppedHeightToWidth));
                    if (srcHeight < image.Height) {
                        srcWidth = image.Width;
                        top = (image.Height - srcHeight) / 2;
                    } else {
                        srcWidth = (int)Math.Round(image.Width * ((double)image.Height / srcHeight));
                        srcHeight = image.Height;
                        left = (image.Width - srcWidth) / 2;
                    }
                }
                using (Graphics g = Graphics.FromImage(bitmap)) {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    new Rectangle(left, top, srcWidth, srcHeight), GraphicsUnit.Pixel);
                }
                finalImage = bitmap;
            } catch { }
            try {
                using (EncoderParameters encParams = new EncoderParameters(1)) {
                    encParams.Param[0] = new EncoderParameter(Encoder.Quality, (long)100);
                    //quality should be in the range 
                    //[0..100] .. 100 for max, 0 for min (0 best compression)
                    if (filePath != "") {
                        finalImage.Save(filePath, jpgInfo, encParams);

                    } else {
                        var mst = new MemoryStream();
                        finalImage.Save(mst, jpgInfo, encParams);
                        returnImg = mst.ToArray();
                        mst.Dispose();
                    }
                }

            } catch { }
            //using (var mst = new MemoryStream()) {
            //    finalImage.Save(mst, finalImage.RawFormat);
            //    returnImg = mst.ToArray();
            //}
            if (bitmap != null) {
                bitmap.Dispose();
            }
            return returnImg;
        }
        #endregion
    }
}