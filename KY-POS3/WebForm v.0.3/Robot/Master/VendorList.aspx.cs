using System;
using System.Linq;
using System.Web;
using System.Web.UI;

using Robot.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;

using Robot.Data.DataAccess;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using DevExpress.Web;
using System.Threading;

namespace Robot.Master
{

    public partial class VendorList : MyBasePage
    {

        public static string PreviousListPage { get { return HttpContext.Current.Session["vendor_previouspage"] == null ? "" : (string)HttpContext.Current.Session["vendor_previouspage"]; } set { HttpContext.Current.Session["vendor_previouspage"] = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetQueryString2HiddenFiled();
            LoadDropDownDevList();
            if (!IsPostBack)
            {
                LoadDefaultFilter();
                LoadData();
            }

        }

        private void SetQueryString2HiddenFiled()
        {
            hddmenu.Value = Request.QueryString["menu"];
            hdddoctype.Value = Request.QueryString["doctype"];
            hddcompany.Value = Request.QueryString["com"];

        }
        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(PreviousListPage);
        }

        private void CheckPermission()
        {

        }
        private void LoadDefaultFilter()
        {

            if (VendorInfoV2Service.MyFilter == null)
            {
                VendorInfoV2Service.NewFilter();
            }
            var f = VendorInfoV2Service.MyFilter;
            CheckPermission();
            txtSearch.Text = f.SearchText;
        }
        private void SetDefaultFilter()
        {
            var f = VendorInfoV2Service.MyFilter;
            f.SearchText = txtSearch.Text.Trim();
        }
        private void SetActiveControl()
        {
            lblHeaderCaption.Text = PermissionService.GetMenuInfo(hddmenu.Value).Name;
            CheckPermission();
        }
        private void LoadDropDownDevList()
        {
        }

        private void LoadData()
        {
            try
            {
                SetDefaultFilter();

                var f = VendorInfoV2Service.MyFilter;
                VendorInfoV2Service.DocList = VendorInfoV2Service.ListVendorInCom(f.SearchText, LoginService.LoginInfo.CurrentCompany.CompanyID, false);
                GridBinding();
                SetActiveControl();
            }
            catch (Exception ex)
            {
            }
        }

        protected void grdDetail_DataBinding(object sender, EventArgs e)
        {
            grdDetail.DataSource = VendorInfoV2Service.DocList;
        }

        private void GridBinding()
        {
            grdDetail.DataSource = VendorInfoV2Service.DocList;
            grdDetail.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SetDefaultFilter();
            LoadData();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            gridExport.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void grdDetail_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            if (e.CommandArgs.CommandName == "Select")
            {
                ASPxGridView grid = (ASPxGridView)sender;
                string id = e.KeyValue.ToString();
                VendorInfoV2Service.GetDocSetByID(id,false);
                VendorInfoV2Service.PreviouseDetailPage = HttpContext.Current.Request.Url.PathAndQuery;
                string myurl = $"~/Master/VendorDetail?id={id}&menu={hddmenu.Value}";
                Response.RedirectPermanent(myurl);
            }
        }


        protected void btnNew_Click(object sender, EventArgs e)
        {
            VendorInfoV2Service.PreviouseDetailPage = HttpContext.Current.Request.Url.PathAndQuery;
            VendorInfoV2Service.DocSet = null;
            string myurl = $"~/Master/VendorDetail?id=&menu={hddmenu.Value}";
            Response.RedirectPermanent(myurl);
        }
        protected void CallbackPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            LoadData();
        }
    }

}