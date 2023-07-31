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
using System.Web.UI.HtmlControls;
using Robot.POS.DA;

namespace Robot.POS {
    public partial class RCSelectInv : MyBasePage {
        public static string PreviousPageX { get { return (string)HttpContext.Current.Session["rcselectinv_previouspage"]; } set { HttpContext.Current.Session["rcselectinv_previouspage"] = value; } }     
        public static List<vw_OINVHead> DocList { get { return (List<vw_OINVHead>)HttpContext.Current.Session["oinv_select_list"]; } set { HttpContext.Current.Session["oinv_select_list"] = value; } }
        public static I_Filter Filter { get { return (I_Filter)HttpContext.Current.Session["rcselectinvfilter"]; } set { HttpContext.Current.Session["rcselectinvfilter"] = value; } }
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                LoadDefaultFilter();
                LoadDropDownList();
                LoadData();
            }
        }

        public class I_Filter {
            public String SearchText { get; set; }
            public String Company { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
        }
        private void NewFilterSet() {
            Filter = new I_Filter();
            Filter.SearchText = "";
            Filter.Company = "";
            Filter.Begin = DateTime.Now.Date.AddMonths(-6);
            Filter.End = DateTime.Now.Date;
        }

        private void LoadDropDownList() {

        }



        private void LoadDefaultFilter() {
            if (Filter == null) {
                NewFilterSet();
            }
            dtBegin.Value = Filter.Begin;
            dtEnd.Value = Filter.End;

        }

        private void SetDefaultFilter() {
           Filter.Begin=dtBegin.Date;
           Filter.End= dtEnd.Date;
         
        }
        private void BindData() {
            var h = ORCService.DocSet.Head;
            lblCustomer.Text = h.CustomerName + " " + h.CustomerID;
        }

        private void LoadData() { 
               SetDefaultFilter();
            DocList = ORCService.ListInvoice(Filter.Begin, Filter.End);
            BindGrid();


        }
        private void BindGrid() {
            grdLine2.DataSource = DocList;
            grdLine2.DataBind();
        }

        #region PopUp management

        private void ClosePopup(string command) {
            ClientScript.RegisterStartupScript(GetType(), "ANY_KEY", string.Format("window.parent.OnClosePopupEventHandler('{0}');", command), true);
        }
        #endregion


        private void AddData() {


            foreach (ListViewDataItem item in grdLine2.Items) {

                Label invid = item.FindControl("lblINV_ID") as Label;
                HtmlInputCheckBox ckSelect = item.FindControl("ckSelect") as HtmlInputCheckBox;
                List<string> invs = new List<string>();
                if (ckSelect.Checked) {
                    invs.Add(invid.Text);

                }
                ORCService.AddRCLineFromInv(invs);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e) {
            AddData();
            Response.RedirectPermanent(PreviousPageX);

        }
        protected void btnClose_Click(object sender, EventArgs e) {
           
            Response.Redirect(PreviousPageX);
        }

         
        private void ShowAlert1(string msg, bool result) {
            lblInfoSave.Text = msg;
            if (result) {
                lblInfoSave.ForeColor = Color.Green;
            } else {
                lblInfoSave.ForeColor = Color.Red;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e) {
    
            LoadData();
        }
    }

}