using System;
using System.Linq;
using System.Web;

using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;

namespace Robot.Master
{
    public partial class MasterTypeList : MyBasePage {

        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString2HiddenFiled();
            LoadDropDownDevList();
            if (!IsPostBack) {
                LoadDefaultFilter();                
                LoadData();
            }
        }
        private void SetQueryString2HiddenFiled() {
            hddmenu.Value = Request.QueryString["menu"];
            hdddoctype.Value = Request.QueryString["doctype"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(MasterTypeService.PreviousListPage);
        }
        private void LoadDefaultFilter() {
 
         
      
            if (MasterTypeService.FilterSet == null) {
                MasterTypeService.NewFilterSet();
            }

            txtSearch2.Text = MasterTypeService.FilterSet.SearchText;         
        }
        private void SetDefaultFilter() {
            MasterTypeService.NewFilterSet();
            MasterTypeService.FilterSet.SearchText = txtSearch2.Text.Trim();        
        }

        private void LoadDropDownDevList() {

        }


        private void LoadData() {
            try {
                SetDefaultFilter();
                MasterTypeService.ListDoc();
                GridBinding();
                SetActiveControl();
           
            } catch (Exception ex) {
                var err = ex.Message;
            }
        }

        private void GridBinding() {
            grdDetail.DataSource = MasterTypeService.DocList;
            grdDetail.DataBind();
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e) {
            LoadData();

        }

        private void SetActiveControl()
        {
            lblHeaderCaption.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;

        }
        protected void btnExcel_Click(object sender, EventArgs e) {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e) {
            if (e.CommandArgs.CommandName == "Select") {
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString();
                MasterTypeService.GetDocSetByID(id);
                MasterTypeService.PreviousDetailPage = HttpContext.Current.Request.Url.PathAndQuery;
                string url = $"../Master/MasterTypeDetail?menu={hddmenu.Value}";
                Response.Redirect(url);                
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = MasterTypeService.DocList;
        }
      
    }

}