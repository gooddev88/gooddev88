
using DevExpress.Web;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Master.DA;
using Robot.POSC.DA;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Robot.Communication.Report
{
    public partial class ReportSalesSummary : System.Web.UI.Page {

        public static string PreviousPage { get { return HttpContext.Current.Session["ReportSalesSummary_previouspage"] == null ? "" : (string)HttpContext.Current.Session["ReportSalesSummary_previouspage"]; } set { HttpContext.Current.Session["ReportSalesSummary_previouspage"] = value; } }        
        public static List<SP_RptPOS133A_Result> ListDoc { get { return (List<SP_RptPOS133A_Result>)HttpContext.Current.Session["payhistory_list"]; } set { HttpContext.Current.Session["payhistory_list"] = value; } }
        public static DateTime ShowDate { get { return HttpContext.Current.Session["ReportSalesSummary_date"] == null ? DateTime.Now.Date : (DateTime)HttpContext.Current.Session["ReportSalesSummary_date"]; } set { HttpContext.Current.Session["ReportSalesSummary_date"] = value; } }

        protected void Page_Load(object sender, EventArgs e) {
            LoadDropDownDevList();
            //CloseAlert();
            SetQueryString();
            if (!IsPostBack) {
                LoadDropDownList();
                LoadData();
            }
        }

        private void SetQueryString() {
            hddid.Value = Request.QueryString["id"];
        }

        private void LoadDropDownList() {

        }

        private void LoadDropDownDevList() {

        }

        private void LoadData() {
            ListDoc = SaleReportService.ReportPOS133A("", ShowDate, ShowDate);
            GridBinding();
            
            BindData();           
        }

        private void BindData() {
            lblshowdate.Text = Convert.ToDateTime(ShowDate).ToString("dd/MM/yyyy");

            lblTopic.Text = "รายงานสรุปยอดขาย";
            var currUser = LoginService.LoginInfo.CurrentUser;
        }

        protected void btnBackList_Click(object sender, EventArgs e) {
            Response.RedirectPermanent(PreviousPage);
        }

        private void ShowAlert(string msg, string type) {

            lblAlertmsg.Text = msg;
            if (type == "Success") {
                lblAlertmsg.ForeColor = Color.Green;
            }
            if (type == "Error") {
                lblAlertmsg.ForeColor = Color.Red;
            }
            if (type == "Warning") {
                lblAlertmsg.ForeColor = Color.YellowGreen;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('','" + type + "');", true);
        }
        //private void CloseAlert() {
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        //}

        private void GridBinding() {
            grdline.DataSource = ListDoc;
            grdline.DataBind();
            CalTotalPay();
        }

        protected void grdline_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e) {
            (grdline.FindControl("grdlistPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            GridBinding();
        }

        protected void grdLine_ItemCommand(object sender, ListViewCommandEventArgs e) {
            if (e.CommandName == "del") {
                string id = e.CommandArgument.ToString();

                //var rs = POS_ORDERService.DeleteBillPayment(id);
                //if (rs.Result == "ok") {
                //    LoadData();
                //} else {
                //    ShowAlert(rs.Message1, "Error");
                //}
            }
        }

        protected void grdLine_ItemDataBound(object sender, ListViewItemEventArgs e) {
            string logodel = "<i class=\"far fa-trash-alt fa-2x\" style=\"color: Gray\"></i>";
          
            //Label lblStatusText = (e.Item.FindControl("lblStatusText") as Label);
            //Literal lblPayLogo = (e.Item.FindControl("lblPayLogo") as Literal);
            //var dataItem = e.Item.DataItem;
            //var statusText = ((vw_POS_ORDERPayment)dataItem).PayByName;


            //lblPayLogo.Text = logodel;
        }
        private void CalTotalPay() {
            //var sumpay = ListDoc.Sum(o => o.PayAmt);
            //lblSumPay.Text = "ยอดเงินทั้งหมด " + Convert.ToDecimal(sumpay).ToString("n2") + " บาท";
        }

        protected void grdline_DataBinding(object sender, EventArgs e) {

        }

        protected void btnChangeDate_Click(object sender, EventArgs e) {
            ChangeDate.PreviousPage = HttpContext.Current.Request.Url.PathAndQuery;
            string url = $"~/Communication/Report/ChangeDate?menu={hddmenu.Value}";
            Response.Redirect(url);
        }
    }
}