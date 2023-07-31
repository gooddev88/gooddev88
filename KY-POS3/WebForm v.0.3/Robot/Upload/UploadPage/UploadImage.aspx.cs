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

namespace Robot.Upload.UploadPage {
    public partial class UploadImage : Page {
        public static  string PreviousPageX { get { return HttpContext.Current.Session["uploadimage_previouspage"]==null?"": (string)HttpContext.Current.Session["uploadimage_previouspage"]; } set { HttpContext.Current.Session["uploadimage_previouspage"] = value; } }
        public static XFilesSet DocSet { get { return (XFilesSet)HttpContext.Current.Session["updateimage_docset"]; } set { HttpContext.Current.Session["updateimage_docset"] = value; } }
        
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString(); 
            if (!IsPostBack) { 
                LoadDropDownList();
                LoadData();
            }
            SetActiveControl();
        }

        private void SetQueryString() {
     
            hddPreviouspage.Value = HttpContext.Current.Request.Url.AbsoluteUri;  
            //hdddoctype.Value = Request.QueryString["doctype"];
            //hddfileid.Value = Request.QueryString["fileid"];
            //hdddocid.Value = Request.QueryString["docid"];
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
            
        }
        private void BindData() {
            lblHeader.Text = UploadInfo.UploadOption.Topic;
        }

        //private void setStep1() { 
        //    divSelectType.Visible = true;
        //    divUpload.Visible = false;
        //    divUploadCompleted.Visible = false;
        //}
        private void setStepUpload() {

  
            divUpload.Visible = true;
            divShowUploadCompleted.Visible = false;
        }
        private void setStepFinish() {
      
            divUpload.Visible = false;
            divShowUploadCompleted.Visible = true;
        
        }
        private void LoadDropDownList() {

            //cboDocFileType.DataSource = DocFileInfoService.MiniSelectList("");
            //cboDocFileType.DataBind();

        }

        private void PrepairDataSave() {
            string userLoging = "ONLINE";
            if (LoginService.LoginInfo.CurrentUser != null)
            {
                userLoging = LoginService.LoginInfo.CurrentUser;
            }

            DocSet = XFilesService.NewTransaction(userLoging);
            DocSet.XFilesRef = DocSet.XFilesRef = XFilesService.CopyUploadInfo2FileRef(UploadInfo.XFilesRef, DocSet.XFilesRef); 
            //DocSet.XFiles.FileID = DocSet.XFilesRef.FileID;
            //DocSet.XFiles.FileName= DocSet.XFilesRef.FileID;
        }

       
     
     

        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e) {
        
            PrepairDataSave();
            DocSet.XFiles.Data = ResizeImage(UploadControl.UploadedFiles[0].FileBytes, UploadInfo.UploadOption.ImgWeight, UploadInfo.UploadOption.ImgHeight);
            if (UploadInfo.XFilesRef.FileType=="IMG") {
                DocSet.XFiles.DataThumb = ResizeImage(UploadControl.UploadedFiles[0].FileBytes, 200, 200);
            }
            DocSet.XFiles.OriginFileExt = Path.GetExtension(e.UploadedFile.FileName);
            DocSet.XFiles.FileExt = DocSet.XFiles.OriginFileExt;
            DocSet.XFiles.OriginFileName = e.UploadedFile.FileName;
            //DocSet.XFiles.DataInBase64 =   Convert.ToBase64String(DocSet.XFiles.Data); 
         //   DocSet.XFiles.FileName = UploadInfo.DocID + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
             
            try { 
                long sizeInKilobytes = e.UploadedFile.ContentLength / 1024;
                string sizeText = sizeInKilobytes.ToString() + " KB";
                e.CallbackData = DocSet.XFiles.OriginFileName + "|" + "|" + sizeText;
              var r=  XFilesService.Save(  DocSet, UploadInfo.UploadOption.IsReplace);
                if (r.Result=="ok") {
                    UploadInfo.UploadOption.IsFinishUpload = true;
                }
            } catch (Exception ex) {
                UploadInfo.OutputAction.Result = "fail";
                if (ex.InnerException!=null) {
                    UploadInfo.OutputAction.Message1 = ex.InnerException.ToString();
                } else {
                    UploadInfo.OutputAction.Message1 = ex.Message;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(PreviousPageX);
        }

    }
}
