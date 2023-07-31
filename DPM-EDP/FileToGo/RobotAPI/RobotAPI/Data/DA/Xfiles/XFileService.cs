using Dapper;
using RobotAPI.Data.DA.Xfiles.Model;
using RobotAPI.Data.XFilesCenterDB.TT;
using System.Data.SqlClient;
using static RobotAPI.Data.ML.Shared.I_Result;

namespace RobotAPI.Data.DA.Xfiles {
    public class XFilesService {

        #region variable
        public class XFilesSet {
            public xfiles? XFiles { get; set; }
            public xfiles_ref? XFilesRef { get; set; }
            public I_BasicResult? OutputAction { get; set; }
        }
        #endregion



        #region get

        public static xfiles GetFile(string file_id, bool isThumb) {
            //get file data
            xfiles result = new xfiles();
            var conn_str = CreateFileConn(file_id);
            try {

                using (var connection = new SqlConnection(conn_str)) {
                    string sql;
                    if (isThumb) {
                        sql = @"   
                                                select 
                                                        id ,app_id ,file_id ,file_type ,file_name ,file_ext ,file_path ,origin_filename ,origin_file_ext ,origin_filepath,created_by,created_date,modified_by,modified_date,is_active
                                                      ,data_thumb 
                                                  from  xfiles where file_id=@file_id
                                                ";
                    } else {
                        sql = @"   
                                                select 
                                                        id ,app_id ,file_id ,file_type ,file_name ,file_ext ,file_path ,origin_filename ,origin_file_ext ,origin_filepath,created_by,created_date,modified_by,modified_date,is_active
                                                      ,data 
                                                  from  xfiles where file_id=@file_id
                                                ";
                    }
                    result = connection.Query<xfiles>(sql, new { @file_id = file_id }).FirstOrDefault();
                }
            } catch {
            }
            return result;
        }

        public static string CreateFileConn(string file_id) {
            //สร้าง connection ไปยัง database ก้อนที่เก็บไฟล์ไว้
            string conStr = "";
            using (XfilescenterContext db = new XfilescenterContext()) {
                var i = db.vw_xfiles_ref.Where(o => o.file_id == file_id).FirstOrDefault();
                //var i =db.xfiles_ref.Where(o=>o.file_id== "0100de3f-9f9b-4dc3-8c5b-c3a7598500c3").FirstOrDefault();
                conStr = $"data source={i.db_server};initial catalog={i.db_name};persist security info=True;user id={i.login_name};password={i.login_password};MultipleActiveResultSets=True";
            }
            return conStr;
        }
        public static xfiles_ref? GetFileRef(string file_id) {
            xfiles_ref? ouput = new xfiles_ref();
            using (XfilescenterContext db = new XfilescenterContext()) {
                ouput = db.xfiles_ref.Where(o => o.file_id == file_id).FirstOrDefault();
            }
            return ouput;
        }
        //public static vw_files? GetFileInfo(string file_id) {
        //    vw_files? ouput = new vw_files();
        //    using (XfilescenterContext db = new XfilescenterContext()) {
        //        ouput = db.vw_files.Where(o => o.file_id == file_id).FirstOrDefault();
        //    }
        //    return ouput;
        //}
        public static xfile_location? GetCurrentFileLocation() {
            xfile_location? root_path = new xfile_location();
            using (XfilescenterContext db = new XfilescenterContext()) {
                root_path = db.xfile_location.Where(o => o.is_current == true && o.loc_type == "DB").FirstOrDefault();
            }
            return root_path;
        }
        #endregion


        #region inset update delete
        public static I_BasicResult Save(XFilesSet data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (XfilescenterContext db = new XfilescenterContext()) {
                    var chkExist_file = db.xfiles.Where(o => o.file_id == data.XFiles.file_id).FirstOrDefault();

                    if (chkExist_file != null) {

                        chkExist_file.file_type = data.XFiles.file_type;
                        chkExist_file.origin_filename = data.XFiles.origin_filename;
                        chkExist_file.origin_filename = data.XFiles.origin_filename;
                        chkExist_file.origin_file_ext = data.XFiles.origin_file_ext;
                        chkExist_file.file_name = data.XFiles.file_name;
                        chkExist_file.file_ext = data.XFiles.file_ext;
                        chkExist_file.data = data.XFiles.data;
                        chkExist_file.data_thumb = data.XFiles.data_thumb;
                        chkExist_file.data_inbase64 = data.XFiles.data_inbase64;
                        db.SaveChanges();

                    } else {
                        db.xfiles.Add(data.XFiles);
                        db.SaveChanges();
                    }

                    var chkExist_file_ref = db.xfiles_ref.Where(o => o.file_id == data.XFiles.file_id).FirstOrDefault();
                    if (chkExist_file_ref != null) {
                        chkExist_file_ref.upload_completed_time = DateTime.Now;
                        db.SaveChanges();
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
        public static I_BasicResult CreateFileRef(xfiles_ref file_data) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<string> valid_filetype = new List<string> { "image", "file" };
                using (XfilescenterContext db = new XfilescenterContext()) {

                    #region validate data

                    #endregion
                    if (valid_filetype.Contains(file_data.file_type)) {
                        result.Result = "fail";
                        result.Message1 = "file_type can only have value of image or file";

                    }

                    if (valid_filetype.Contains(file_data.owner_id)) {
                        result.Result = "fail";
                        result.Message1 = "can not upload file without owner field";

                    }
                    if (string.IsNullOrEmpty(file_data.app_id)) {
                        result.Result = "fail";
                        result.Message1 = "app_id can not be empty";
                    }

                    var loc_info = GetCurrentFileLocation();

                    if (loc_info == null) {
                        result.Result = "fail";
                        result.Message1 = "file location not configuration";
                    }
                    if (result.Result == "fail") {
                        return result;
                    }
                    #region init data
                    //ถ้าไม่ได้   gen file_id มาให้ก็ให้ api gen ให้เลย
                    if (string.IsNullOrEmpty(file_data.file_id)) {
                        file_data.file_id = Guid.NewGuid().ToString().ToLower();
                    }

                    var chk_exist_ref = db.xfiles_ref.Where(o => o.file_id == file_data.file_id).FirstOrDefault();
                    if (chk_exist_ref != null) {
                        chk_exist_ref.loc_id = loc_info.loc_id;
                        chk_exist_ref.rcom_id = file_data.rcom_id == null ? "" : file_data.rcom_id;
                        chk_exist_ref.com_id = file_data.com_id == null ? "" : file_data.com_id;
                        chk_exist_ref.doc_id = file_data.doc_id == null ? "" : file_data.doc_id;
                        chk_exist_ref.doc_linenum = file_data.doc_linenum == null ? 0 : file_data.doc_linenum;
                        chk_exist_ref.source_table = file_data.source_table == null ? "" : file_data.source_table;
                        chk_exist_ref.doc_type = file_data.doc_type == null ? "" : file_data.doc_type;
                        chk_exist_ref.doc_cate = file_data.doc_cate == null ? "" : file_data.doc_cate;
                        chk_exist_ref.remark = file_data.remark == null ? "" : file_data.remark;
                        chk_exist_ref.modified_by = file_data.modified_by == null ? "" : chk_exist_ref.owner_id;
                        chk_exist_ref.modified_date = DateTime.Now;
                        db.SaveChanges();

                    } else {

                        file_data.loc_id = loc_info.loc_id;
                        file_data.rcom_id = file_data.rcom_id == null ? "" : file_data.rcom_id;
                        file_data.com_id = file_data.com_id == null ? "" : file_data.com_id;
                        file_data.doc_id = file_data.doc_id == null ? "" : file_data.doc_id;
                        file_data.doc_linenum = file_data.doc_linenum == null ? 0 : file_data.doc_linenum;
                        file_data.source_table = file_data.source_table == null ? "" : file_data.source_table;
                        file_data.doc_type = file_data.doc_type == null ? "" : file_data.doc_type;
                        file_data.doc_cate = file_data.doc_cate == null ? "" : file_data.doc_cate;
                        file_data.remark = file_data.remark == null ? "" : file_data.remark;
                        file_data.created_by = file_data.created_by == null ? file_data.owner_id : file_data.created_by;
                        file_data.created_date = DateTime.Now;
                        file_data.modified_by = file_data.modified_by == null ? "" : file_data.modified_by;
                        file_data.modified_date = file_data.created_date;
                        file_data.is_active = true;
                        db.xfiles_ref.Add(file_data);
                        db.SaveChanges();
                    }

                    #endregion




                    result.Message2 = file_data.file_id;
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

        public static I_BasicResult SetUploadCompleted(string fileid) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (XfilescenterContext db = new XfilescenterContext()) {
                    var query = db.xfiles_ref.Where(o => o.file_id == fileid).FirstOrDefault();
                    if (query != null) {
                        query.upload_completed_time = DateTime.Now;
                        db.SaveChanges();
                    } else {
                        result.Result = "fail";
                        result.Message1 = "file not found";
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
        public static I_BasicResult DeleteFile(List<string> file_ids) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (XfilescenterContext db = new XfilescenterContext()) {
              
                  db.xfiles_ref.RemoveRange(db.xfiles_ref.Where(o => file_ids.Contains(o.file_id)));
                    db.xfiles.RemoveRange(db.xfiles.Where(o => file_ids.Contains(o.file_id)));
                    db.SaveChanges(); 
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

        #region convert
        public static xfiles_ref Convert2xFile_ref(FilesInfo i) {
            xfiles_ref o = new xfiles_ref();
            o.file_id = String.IsNullOrEmpty(i.file_id) ? Guid.NewGuid().ToString().ToLower() : i.file_id;
            o.file_type = i.file_type == null ? "" : i.file_type;
            o.owner_id = i.owner_id == null ? "" : i.owner_id;
            o.rcom_id = i.rcom_id == null ? "" : i.rcom_id;
            o.com_id = i.com_id == null ? "" : i.com_id;
            o.doc_id = i.doc_id == null ? "" : i.doc_id;
            o.doc_linenum = i.doc_linenum == null ? 0 : i.doc_linenum;
            o.app_id = String.IsNullOrEmpty(i.app_id) ? "" : i.app_id;
            o.doc_type = i.doc_type == null ? "" : i.doc_type;
            o.doc_cate = i.doc_cate == null ? "" : i.doc_cate;
            o.remark = i.remark == null ? "" : i.remark;
            o.created_by = i.owner_id;
            o.created_date = DateTime.Now;
            o.modified_by = o.modified_by;
            o.modified_date = o.created_date;
            o.is_active = true;
            return o;
        }
        #endregion

        #region  delete

        #endregion




        #region default function

        public static XFilesSet NewTransaction(string rcom, string appid, string createdBy) {
            XFilesSet n = new XFilesSet();
            n.XFilesRef = NewXFilesRef(rcom, appid, createdBy);
            n.XFiles = NewXFiles(n.XFilesRef);
            return n;
        }
        public static xfiles_ref NewXFilesRef(string rcom, string appid, string createdBy) {

            xfiles_ref n = new xfiles_ref();
            var loc = GetCurrentFileLocation();

            n.file_id = Guid.NewGuid().ToString().ToLower();
            n.loc_id = loc.loc_id;
            n.owner_id = createdBy;
            n.rcom_id = rcom;
            n.com_id = "";
            n.app_id = appid;
            n.doc_id = "";
            n.doc_linenum = 0;
            n.source_table = "";
            n.doc_type = "file";
            n.doc_type = "";
            n.doc_cate = "";
            n.remark = "";
            n.upload_completed_time = null;
            n.created_by = createdBy;
            n.created_date = DateTime.Now;
            n.modified_by = "";
            n.modified_date = n.created_date;
            n.is_active = true;

            return n;
        }
        public static xfiles NewXFiles(xfiles_ref i) {

            xfiles n = new xfiles();

            n.app_id = i.app_id;
            n.file_id = i.file_id;
            n.file_type = i.file_type;
            n.file_name = "";
            n.file_ext = "";
            n.file_path = "";
            n.origin_filename = "";
            n.origin_file_ext = "";
            n.origin_filepath = "";
            n.data = null;
            n.data_thumb = null;
            n.data_inbase64 = "";
            n.created_by = i.created_by;
            n.created_date = Convert.ToDateTime(i.created_date);
            n.modified_by = i.modified_by;
            n.modified_date = i.modified_date;
            n.is_active = Convert.ToBoolean(i.is_active);

            return n;
        }

        #endregion
    }
}
