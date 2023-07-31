using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Data.DataAccess
{
    /// <summary>
    /// ประเภทเอกสาร
    /// </summary>
    public static class DocTypeInfoService
    {
        #region Validate Key

        /// <summary>
        /// Check Duplicate key in  Table : DocTypeInfo
        /// Return List(string) : String[0] is result check : Value [Y/N]
        /// String[1] is infomation alter valid
        /// </summary>
        /// <param name="Key"> DocTypeID ในตาราง DocTypeInfo (Primary Key)</param>
        public static List<string> CheckValidKey(string Key)
        {
            var comlist = LoginService.LoginInfo.UserInCompany;
            List<string> result = new List<string>();
            result[0] = "Y";
            result[1] = "Key is valid.";
            using (GAEntities db = new GAEntities())
            {
                var query = db.DocTypeInfo.Where(o => o.DocTypeID == Key).FirstOrDefault();
                if (query != null)
                {
                    result[0] = "N";
                    result[1] = "Key : " + query.DocTypeID + " was already used by another by " + query.CompanyID;
                }
            }
            return result;
        }

        #endregion Validate Key

        #region Method GET

        public static DocTypeInfo DataByCateKey(string Key)
        {
            DocTypeInfo result = new DocTypeInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => o.DocTypeID == Key && o.DocCate == "GROUP").FirstOrDefault();
            }
            return result;
        }

        public static List<DocTypeInfo> ListDocTypeByParentID(string parentId)
        {
            List<DocTypeInfo> result = new List<DocTypeInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => (o.ParentID == parentId || parentId == "") && o.IsActive).ToList();
            }
            return result;
        }

        public static vw_DocTypeInfo DataViewByKey(string Key)
        {
            vw_DocTypeInfo result = new vw_DocTypeInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_DocTypeInfo.Where(o => o.DocTypeID == Key).FirstOrDefault();
            }
            return result;
        }

        /// <summary>
        /// Get Docparent by doctype id
        /// </summary>
        /// <param name="ID"> DocTypeID ในตาราง DocTypeInfo  </param>
        public static DocTypeInfo GetDocParentByDocTypeID(string ID)
        {
            DocTypeInfo result = new DocTypeInfo();
            using (GAEntities db = new GAEntities())
            {
                var data = db.DocTypeInfo.Where(o => o.DocTypeID == ID && o.IsActive).FirstOrDefault();

                result = db.DocTypeInfo.Where(o => o.DocTypeID == data.ParentID && o.DocCate == "GROUP").FirstOrDefault();
            }
            return result;
        }

        public static DocTypeInfo GetDocTypeByID(string doctypeID)
        {
            DocTypeInfo result = new DocTypeInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => o.DocTypeID == doctypeID).FirstOrDefault();
            }
            return result;
        }

        public static List<DocTypeInfo> ListByParentID(string ParentID)
        {
            List<DocTypeInfo> result = new List<DocTypeInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => (o.ParentID == ParentID || ParentID == "")).ToList();
            }
            return result;
        }

        /// <summary>
        /// List DocTypeInfo DocCate filter DocCate="GROUP"
        /// </summary>
        /// <param name="ID"> DocCate ในตาราง DocTypeInfo  </param>
        public static List<DocTypeInfo> ListCate(string ID)
        {
            List<DocTypeInfo> result = new List<DocTypeInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => (o.DocCate == ID)).ToList();
            }
            return result;
        }

        public static List<DocTypeInfo> ListSearch(string search)
        {
            List<DocTypeInfo> result = new List<DocTypeInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => (o.DocTypeID.Contains(search)
                                                 || o.Name.Contains(search)
                                                 || o.ParentID.Contains(search)
                                                 || o.PrefixNo.Contains(search)
                                                 || o.Remark.Contains(search)
                                                 || search == ""
                                                 )).ToList();
            }
            return result;
        }

        public static List<vw_DocTypeInfo> ListViewSearch(string search)
        {
            List<vw_DocTypeInfo> result = new List<vw_DocTypeInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.vw_DocTypeInfo.Where(o => (o.DocTypeID.Contains(search)
                                              || o.DocName.Contains(search)
                                              || o.ParentID.Contains(search)
                                              || o.PrefixNo.Contains(search)
                                              || o.Remark.Contains(search)
                                              || search == ""
                                              )).ToList();
            }
            return result;
        }

        #endregion Method GET

        #region Save

        public static List<string> Save_doc(DocTypeInfo doc, string action)
        {
            //string comlist = HttpContext.Current.Session["userincompany"].ToString();
            List<string> result = new List<string>();

            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    if (action == "insert")
                    {
                        db.DocTypeInfo.Add(doc);
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = doc.DocTypeID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.CompanyID, Action = "INSERT DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }

                    if (action == "update")
                    {
                        var d = db.DocTypeInfo.Where(o => o.DocTypeID == doc.DocTypeID).FirstOrDefault();
                        d.ApplyToComLevel = doc.ApplyToComLevel;
                        d.DocTypeCode = doc.DocTypeCode;
                        d.DocCate = doc.DocCate;
                        d.ParentID = doc.ParentID;
                        d.Name = doc.Name;
                        d.PrefixNo = doc.PrefixNo;
                        d.DigitRunning = doc.DigitRunning;
                        d.Remark = doc.Remark;
                        d.IsAutoID = doc.IsAutoID;
                        db.SaveChanges();
                        TransactionInfoService.SaveLog(new TransactionLog { TransactionID = doc.DocTypeID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = doc.CompanyID, Action = "UPDATE DATA" });

                        result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                        result[1] = "";
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;
        }

        #endregion Save

        #region Delete

        public static List<string> Delete(string id)
        {
            List<string> result = new List<string>();
            result.Add("");//RESULT ACTION R0=ERROR:R1=SUCCESS
            result.Add("");

            try
            {
                using (GAEntities db = new GAEntities())
                {
                    var d = db.DocTypeInfo.Where(o => o.DocTypeID == id).FirstOrDefault();

                    d.IsActive = false;

                    db.SaveChanges();
                    TransactionInfoService.SaveLog(new TransactionLog { TransactionID = d.DocTypeID, ParentID = "", TransactionDate = DateTime.Now, CompanyID = d.CompanyID, Action = "DELETE DATA" });

                    result[0] = "R1";//RESULT ACTION R0=ERROR:R1=SUCCESS
                    result[1] = "";
                }

                return result;
            }
            catch (Exception ex)
            {
                result[0] = "R0";//RESULT ACTION R0=ERROR:R1=SUCCESS
                result[1] = ex.Message;
            }
            return result;
        }

        #endregion Delete
    }
}