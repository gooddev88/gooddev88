using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.ServiceHelper;
using Robot.Master.DA;
using Robot.POSC.DA;
using static Robot.POSC.DA.POSSaleService;

namespace Robot.POSC {
    public partial class POSSaleDeletePermance : MyBasePage {
        public static string PreviousPage { get { return HttpContext.Current.Session["posc_delete_previouspage"] == null ? "" : (string)HttpContext.Current.Session["posc_delete_previouspage"]; } set { HttpContext.Current.Session["posc_new_previouspage"] = value; } }
        public static List<vw_POS_SaleHead> DocList { get { return (List<vw_POS_SaleHead>)HttpContext.Current.Session["possale_delete_doclist"]; } set { HttpContext.Current.Session["possale_delete_doclist"] = value; } }
        public static I_FilterSet Filter { get { return (I_FilterSet)HttpContext.Current.Session["possale_delete_filter"]; } set { HttpContext.Current.Session["possale_delete_filter"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            LoadDevDropDownList();
            SetQueryString();
            if (!Page.IsPostBack) {
                CloseAlert();

                LoadDropDownList();
                LoadDefaultFilter();
                LoadData(); 
            }
        }
        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
        }
        private void SetActiveControl() {

        }
        private void CheckPermission() {

        }

        private void LoadData() {

            SetDefaultFilter();
            DocList = POSSaleService.ListDoc(Filter).ToList();
            if (Filter.ShipTo == "")
            {
                DocList = DocList.Where(o => o.ShipToLocID == "").ToList();
            }
            GridBinding();
            BindData();
            SetActiveControl(); 
        }
        private void BindData() {
            
        }

        private void LoadDropDownList() {
            cboCompany.DataSource = CompanyService.ListCompanyInfoUIC("BRANCH", true);
            cboCompany.DataBind();
            cboShipTo.DataSource = ShipToService.ListShipTo();
            cboShipTo.DataBind();
            cboMac.DataSource = POSMachineService.ListMachine();
            cboMac.DataBind(); 
        }

        private void LoadDefaultFilter() {
            if (Filter == null)
            {
                Filter = POSSaleService.NewFilterSet();
            }
            dtInvDate.Value = Filter.Begin;
            cboShipTo.SelectedValue = Filter.ShipTo;
            cboCompany.SelectedValue = Filter.Company;
            cboMac.SelectedValue = Filter.MacNo;

        }
        private void SetDefaultFilter() {
            Filter = POSSaleService.NewFilterSet();
            if (dtInvDate.Value!=null)
            {
                Filter.Begin=Convert.ToDateTime( dtInvDate.Value  ).Date ;
            }

            Filter.ShipTo= cboShipTo.SelectedValue ;
            Filter.Company=cboCompany.SelectedValue ;
            Filter.MacNo=cboMac.SelectedValue;
        }
        private void LoadDevDropDownList() { 
        }

        private string ValidateControl() { 
            return "";
        }
 
        private void ShowAlert(string msg, string type) {
            lblAlertBody.Text = msg;
            msg = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert() {
            lblAlertBody.Text = ""; 
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }


      

        protected void btnDelete_Click(object sender, EventArgs e) {
            if (dtInvDate.Value == null) {
                ShowAlert("Error Date", "Error");
                return;
            }
            var comInfo = CompanyService.GetCompanyInfo(cboCompany.SelectedValue);
            if (string.IsNullOrEmpty(comInfo.ShortCode))
            {
                ShowAlert("ตั้งชื่อย่อ สาขาก่อนเปิดบิลขาย", "Error");
                return;
            }
            
            string mac = cboMac.SelectedValue;
            string comId = cboCompany.SelectedValue;
            string shipto = cboShipTo.SelectedValue;
            DateTime billDate = Convert.ToDateTime(dtInvDate.Value).Date;
            POSSaleService.DeletePermanent(comId, mac, shipto, billDate, 1);
            LoadData();            
        }


        protected void grdDetail_DataBinding(object sender, EventArgs e) {
            grdDetail.DataSource = DocList.Where(o => o.INVID != "").OrderByDescending(o=>o.INVID).ToList();
        }

        private void GridBinding() {
            grdDetail.DataSource = DocList.Where(o => o.INVID != "").OrderByDescending(o => o.INVID).ToList();
            grdDetail.DataBind();
        }

        protected void cboCompany_SelectedIndexChanged(object sender, EventArgs e) {
            LoadData();
        }

        protected void cboShipTo_SelectedIndexChanged(object sender, EventArgs e) {
            LoadData();
        }

        protected void cboMac_SelectedIndexChanged(object sender, EventArgs e) {
            LoadData();
        }

        protected void dtInvDate_DateChanged(object sender, EventArgs e) {
            LoadData();
        }
    }
}