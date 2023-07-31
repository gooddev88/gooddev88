using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Robot.Data.ML.I_Result;

namespace Robot.Data.DA {
    public class FileService {

        public static XFileLocation GetCurrentFileLocation()
        {
            XFileLocation root_path = new XFileLocation();
            using (GAEntities db = new GAEntities())
            {
                root_path = db.XFileLocation.Where(o => o.IsCurrent == true && o.PathType == "DB").FirstOrDefault();
            }
            return root_path;
        }

        public static I_BasicResult SaveFileRef(XFilesRef f ) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db = new GAEntities())
                {
                     
                    var getFile = db.XFilesRef.Where(o => o.FileName == f.FileName).FirstOrDefault();
                    if (getFile == null)
                    {
                        db.XFilesRef.Add(f);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                r.Result = "fail";
                if (ex.InnerException!=null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }
           
            return r;
        }
        public static I_BasicResult SetCompleteFileRef(List<string> fielname,string docID) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            using (GAEntities db = new GAEntities()) {
                var getFile = db.XFilesRef.Where(o => fielname.Contains( o.FileName ) ).ToList();
                foreach (var g in getFile) {
                    g.DocID = docID;
                    g.FileID = g.FileName;
                    db.Update(g);
                    db.SaveChanges();
                }
             
            }
            return r;
        }
        public static I_BasicResult DeleteFileRef(string filename) {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            using (GAEntities db = new GAEntities()) {
                var getFile = db.XFilesRef.Where(o => o.FileName == filename).FirstOrDefault();
                if (getFile != null) {
                     
                    db.XFilesRef.Remove(getFile);
                    db.SaveChanges();
                }  
            }
            return r;
        }
        public static I_BasicResult DeleteFileRefOnDisk(string filename)
        {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try
            {
                using (GAEntities db =new GAEntities())
                {
                    var f = db.vw_XFilesRef.Where(o => o.FileName == filename).FirstOrDefault();
                    if (f==null)
                    {
                        r.Result = "fail";
                        r.Message1 = "File does not exist";
                        return r;
                    }
               
                    var fileName = $"{f.FileName}{f.OriginFileExt}";
                    var path = Path.Combine(f.RootPath, f.FilePath); 
                    var filePath = Path.Combine(path, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
               
            }
            catch (Exception ex)
            {

                r.Result = "fail";
                if (ex.InnerException != null)
                {
                    r.Message1 = ex.InnerException.ToString();
                }
                else
                {
                    r.Message1 = ex.Message;
                }
            }

            return r; 
        }
        public static vw_XFilesRef GetFileRefID(string filename) {
            vw_XFilesRef r = new vw_XFilesRef();
            using (GAEntities db = new GAEntities()) {
                  r = db.vw_XFilesRef.Where(o => o.FileName == filename).FirstOrDefault(); 
            }
            return r;
        }
        public static XFileLocation GetFileLocation() {
            XFileLocation root_path = new XFileLocation();
            using (GAEntities db = new GAEntities()) {
                  root_path = db.XFileLocation.Where(o => o.RootPathID == "PT2001" && o.PathType == "PATH").FirstOrDefault();
                 
            }
            return root_path;
        }

        public static XFilesRef NewFileRef(string originFileName,string fileExt,string appId, string rcomId,string doctype,string filetype="FILE") {
            XFilesRef n = new XFilesRef();
            string rcom = "DPM";
         
            
            using (GAEntities db =new GAEntities()) {
                var root_path = db.XFileLocation.Where(o => o.RootPathID == "PT2001" && o.PathType == "PATH").FirstOrDefault();
                if (root_path==null) {
                 return     null;
                }
         
            string year = Convert.ToDateTime(DateTime.Now, new CultureInfo("en-US")).Year.ToString("0000");
            string month = "M"+DateTime.Now.Month.ToString("00");
            n.RCompanyID = rcomId;
            n.CompanyID = "";
            n.AppID = appId;
            n.DBServer = "";
            n.DBName = "";
            n.RootPathID = root_path.RootPathID;
            n.FileID = Guid.NewGuid().ToString(); 
            n.DocID = ""; 
            n.DocLineNum = 0;
            n.SourceTable = doctype;
            n.DocType = doctype;
            n.DocCate = "";
            n.FileType = filetype;
            n.FileName = n.FileID;
            n.FileExt = fileExt;
                n.FilePath = appId.ToUpper() + @"\" + rcom.ToUpper() + @"\" + doctype + @"\" + year + @"\" + month;
                n.SubUrl = appId.ToUpper() + @"/" + rcom.ToUpper() + @"/" + doctype + @"/" + year + @"/" + month;
                n.OriginFileName = originFileName;
            n.OriginFileExt = fileExt;
            n.OriginFilePath = "";
            n.Remark = "";
            n.CreatedBy = "";
            n.CreatedDate = DateTime.Now;
            n.ModifiedBy = "";
            n.ModifiedDate = null;
            n.IsActive = true;
                   }
            return n;
        }
    }
}
