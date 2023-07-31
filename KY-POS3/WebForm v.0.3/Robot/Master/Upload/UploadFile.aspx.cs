using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.FileStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Robot.Data.DataAccess.XFilesService;

namespace Robot.Master.Upload {
    public partial class UploadFile : Page {
        public static string PreviousFinishPage { get { return HttpContext.Current.Session["uploadfile_finish_previouspage"] == null ? "" : (string)HttpContext.Current.Session["uploadfile_finish_previouspage"]; } set { HttpContext.Current.Session["uploadfile_finish_previouspage"] = value; } }
        public static string PreviousPageX { get { return HttpContext.Current.Session["uploadfile_previouspage"] == null ? "" : (string)HttpContext.Current.Session["uploadfile_previouspage"]; } set { HttpContext.Current.Session["uploadfile_previouspage"] = value; } }
        public static XFilesSet DocSet { get { return (XFilesSet)HttpContext.Current.Session["updatefile_docset"]; } set { HttpContext.Current.Session["updatefile_docset"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadData();
            }
         
        }

        private void SetQueryString() {
            hddPreviouspage.Value = HttpContext.Current.Request.Url.AbsoluteUri;
        }

        private void SetActiveControl() {
            if (!UploadInfo.UploadOption.IsFinishUpload) {
                setStepUpload();
            } else {
                setStepFinish();
            }
        }

        private void LoadData() {
            BindData();
            SetActiveControl();

        }
        private void BindData() {
            //lblHeader.Text = UploadInfo.UploadOption.Topic;
        }

        private void setStepUpload() {
            divUpload.Visible = true;
            divShowUploadCompleted.Visible = false;
        }
        private void setStepFinish() {
            divUpload.Visible = false;
            divShowUploadCompleted.Visible = true;
            Response.Redirect(PreviousPageX);
        }

        private void LoadDropDownList() {
        }

        private void PrepairDataSave() {
            string userLoging = "ONLINE";
            if (LoginService.LoginInfo != null) {
                userLoging = LoginService.LoginInfo.CurrentUser;
            }
            DocSet = XFilesService.NewTransaction(userLoging);
            DocSet.XFilesRef = DocSet.XFilesRef = XFilesService.CopyUploadInfo2FileRef(UploadInfo.XFilesRef, DocSet.XFilesRef); 
        }



        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e) {
            
            PrepairDataSave();
          //  String pathThumb = HttpContext.Current.Server.MapPath($"~/ImageStorage/{UploadInfo.XFilesRef.RCompanyID}/Thumb"); //Path thumb size
            if (UploadInfo.XFilesRef.FileType == "IMG") {
                var data = UploadControl.UploadedFiles[0].FileBytes;
                DocSet.XFiles.Data = ResizeImage(data, UploadInfo.UploadOption.ImgWeight, UploadInfo.UploadOption.ImgHeight);

                //DocSet.XFiles.DataThumb = SaveCroppedImage(data, 200, 200, "");
                DocSet.XFiles.DataThumb = CreateThumb(DocSet.XFiles.Data, 200);
                //  ResizeImage(UploadControl.UploadedFiles[0].FileBytes, 200, 200);
            } else {
                DocSet.XFiles.Data = UploadControl.UploadedFiles[0].FileBytes;
            }
            DocSet.XFiles.OriginFileExt = Path.GetExtension(e.UploadedFile.FileName);
            DocSet.XFiles.FileExt = DocSet.XFiles.OriginFileExt;
            DocSet.XFiles.OriginFileName = e.UploadedFile.FileName;
            try {
                long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
                string sizeText = sizeInKilobytes.ToString() + " KB";
                e.CallbackData = DocSet.XFiles.OriginFileName + "|" + "|" + sizeText;

                var r = XFilesService.Save( DocSet, XFilesService.UploadInfo.UploadOption.IsReplace);
                if (r.Result == "ok") {
                    UploadInfo.UploadOption.IsFinishUpload = true;
                }
            } catch (Exception ex) {
                UploadInfo.OutputAction.Result = "fail";
                if (ex.InnerException != null) {
                    UploadInfo.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    UploadInfo.OutputAction.Message1 = ex.Message;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPageX);
        }
    }
}
