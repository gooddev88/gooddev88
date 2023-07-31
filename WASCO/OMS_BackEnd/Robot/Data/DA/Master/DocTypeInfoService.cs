using Robot.Data.ML;
using Robot.Data.GADB.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot.Data.DA.Master
{
    public class DocTypeInfoService
    {

        public static List<DocTypeInfo> ListDocTypeByParentID(string parentId)
        {
            List<DocTypeInfo> result = new List<DocTypeInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => (o.ParentID == parentId || parentId == "") && o.IsActive == true).OrderBy(o => o.Sort).ToList();
            }
            return result;
        }

        public static DocTypeInfo GetDocTypeInfo(string docId)
        {
            DocTypeInfo result = new DocTypeInfo();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => (o.DocTypeID == docId) && o.IsActive == true).FirstOrDefault();
            }
            return result;
        }
        public static string GetDocTypeDescription(string docId) {
            string output = "";
            using (GAEntities db = new GAEntities()) {
                var query = db.DocTypeInfo.Where(o => (o.DocTypeID == docId) && o.IsActive == true).FirstOrDefault();
                if (query != null) {
                    output = query.Name + " ("+query.DocTypeID + ")";
                }
            }
            return output;
        }

        public static List<DocTypeInfo> ListDocTypeByISGL()
        {
            List<DocTypeInfo> result = new List<DocTypeInfo>();
            using (GAEntities db = new GAEntities())
            {
                result = db.DocTypeInfo.Where(o => o.IsGL == true).OrderBy(o => o.Sort).ToList();
            }
            return result;
        }

    }
}
