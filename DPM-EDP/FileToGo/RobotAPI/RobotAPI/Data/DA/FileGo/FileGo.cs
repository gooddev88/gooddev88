using RobotAPI.Data.XFilesCenterDB.TT;

namespace RobotAPI.Data.DA.FileGo {
    public class FileGo {

        #region Get
        

        public static vw_xfiles_ref? GetFileId(string rcom, string com, string doctype, string docId) {
            vw_xfiles_ref? refx = new vw_xfiles_ref();
            using (XfilescenterContext db = new XfilescenterContext()) {
                refx = db.vw_xfiles_ref.Where(o => o.is_active && o.rcom_id == rcom && o.com_id == com && o.doc_id == docId && o.doc_type == doctype  ).OrderByDescending(o => o.created_date).FirstOrDefault();
            }
            return refx;
        }
     
        #endregion
    }
}
