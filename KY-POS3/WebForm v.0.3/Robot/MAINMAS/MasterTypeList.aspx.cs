using System;
using System.Linq;
using System.Web;

using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;

namespace Robot.MAINMAS
{
    public partial class MasterTypeList : MyBasePage {
        public static string PreviousPage { get { return (string)HttpContext.Current.Session["mastertypelist_previouspage"]; } set { HttpContext.Current.Session["mastertypelist_previouspage"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            SetQueryString();
            LoadDropDownDevList();
            if (!IsPostBack) {
                LoadDefaultFilter();                
                LoadData();
            }
        }
        private void SetQueryString() {
            hddmenu.Value = Request.QueryString["menu"];
            hdddoctype.Value = Request.QueryString["doctype"];
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.Redirect(PreviousPage);
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
       
                SetDefaultFilter();
                MasterTypeService.ListDoc();
                GridBinding();
                SetActiveControl();
           
        
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
                MasterTypeDetail.MasterHead= MasterTypeService.GetMasterTypeHead(id);
                 MasterTypeService.GetDocSetByID(id);
                MasterTypeDetail.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
                string url = $"../MAINMAS/MasterTypeDetail?menu={hddmenu.Value}";
                Response.Redirect(url);                
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = MasterTypeService.DocList;
        }
      
    }

}