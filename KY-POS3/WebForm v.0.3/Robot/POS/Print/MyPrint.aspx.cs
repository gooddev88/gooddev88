
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using Robot.Data;
using Robot.Data.DataAccess;
using Robot.Data.ServiceHelper;
using Robot.POS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Robot.POS.Print
{
    public partial class MyPrint : System.Web.UI.Page
    {
        public static string PreviousPrintPage { get { return (string)HttpContext.Current.Session["posprint_previouspage"]; } set { HttpContext.Current.Session["posprint_previouspage"] = value; } }
        public static I_ReportFilter ReportFilterX { get { return (I_ReportFilter)HttpContext.Current.Session["posprint_printfilter"]; } set { HttpContext.Current.Session["posprint_printfilter"] = value; } }
        public class I_ReportFilter
        {
            public string ReportID { get; set; }
            public string RCompanyID { get; set; }
            public string CompanyID { get; set; }
            public string DocID { get; set; }
            public string DocType { get; set; }
            public string RefID { get; set; }
            public string FilterBy { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }
        }
        public static void NewReportFItler()
        {
            if (ReportFilterX == null)
            {
                ReportFilterX = new I_ReportFilter();
                ReportFilterX.ReportID = "";
                ReportFilterX.DocID = "";
                ReportFilterX.RefID = "";
                ReportFilterX.DocType = "";
                ReportFilterX.RCompanyID = "";
                ReportFilterX.CompanyID = "";
                ReportFilterX.FilterBy = "";
                ReportFilterX.Begin = DateTime.Now.Date;
                ReportFilterX.End = DateTime.Now.Date;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SetQueryString2HiddenFiled();
            LoadDevDropDownList();
            if (!IsPostBack)
            {
                LoadData();
            }
            SetActiveControl();
        }

        private void SetQueryString2HiddenFiled()
        {
            hddid.Value = Request.QueryString["id"];
            hddmenu.Value = Request.QueryString["menu"];
            hddReportName.Value = Request.QueryString["report"];
        }

        private void SetActiveControl()
        {
            if (hddReportName.Value == "POPrint1")
            {
                divfilter.Visible = true;
            }
            else
            {
                divfilter.Visible = false;
            }
        }


        private void LoadData() {

            SetActiveControl();

            var ff = ReportFilterX;
            DateTime begin;
            if (Session["datebegin"] != null)
            {
                begin = (DateTime)Session["datebegin"];
            }
            DateTime end;
            if (Session["dateend"] != null)
            {
                end = (DateTime)Session["dateend"];
            }

            string doctype;
            if (Session["doctype"] != null)
            {
                doctype = (string)Session["doctype"];
            }

            int i = 1;

            switch (hddReportName.Value)
            {
      
                case "PrintShipOrder":
                    PrintShipOrder printshiporder = new PrintShipOrder();
                    printshiporder.initData(ff.DocID);
                    docviewer.OpenReport(printshiporder);
                    break;
                case "PrintStoreOrder":
                    var ordH = POSOrderService.DocSet.head;
                    var printorder = new PrintOrder();
                    printorder.initData(ordH.OrdID); 
                    docviewer.OpenReport(printorder);
                    break;
                case "OrderPrint":
                    PrintSumQtyItemInCompany printsumqtyitemIncompany = new PrintSumQtyItemInCompany();
                    printsumqtyitemIncompany.initData(ff.Begin, ff.End,ff.DocType);
                    docviewer.OpenReport(printsumqtyitemIncompany);
                    break;
                case "POPrint1":
                    PrintPOFormOrder printpoformorder = new PrintPOFormOrder();

                    string vendor = "";
                    if (cboVendor.Value != null)
                    {
                        vendor = cboVendor.Value.ToString();
                    }

                    printpoformorder.initData(ff.Begin, ff.End, ff.DocType, vendor);
                    docviewer.OpenReport(printpoformorder);
                    break;

                case "PurchasePrint":
                    PrintOrderRM printorderrm = new PrintOrderRM();
                    printorderrm.initData(ff.Begin,ff.End);
                    docviewer.OpenReport(printorderrm);
                    break;
                case "PrintStockBalance":
                    PrintStockBalance printstockbalance = new PrintStockBalance();
                    printstockbalance.initData();
                    docviewer.OpenReport(printstockbalance);
                    break;

                case "INVGA01": // Invoice
                    List<int> idinv_list = new List<int> { 1, 2 };

                    INVGA01 firstinv_report = new INVGA01();
                    foreach (var o in idinv_list)
                    {
                        if (i == 1)
                        {
                            firstinv_report.initData(ff.DocID, "", "INVGA01");
                            firstinv_report.DisplayName = ff.DocID;
                            firstinv_report.CreateDocument();
                        }
                        else if (i == 2)
                        {
                            INVGA01 next_report = new INVGA01();
                            next_report.initData(ff.DocID, "copy", "INVGA01");
                            firstinv_report.DisplayName = ff.DocID;
                            next_report.CreateDocument();
                            firstinv_report.Pages.AddRange(next_report.Pages);
                        }

                        i++;
                    }
                    firstinv_report.PrintingSystem.ContinuousPageNumbering = true;
                    docviewer.OpenReport(firstinv_report);
                    break;

                case "RCGA01"://ใบเสร็จรับเงินขายสินค้า
                    List<int> idrc_list = new List<int> { 1, 2 };

                    RCGA01 firstrc_report = new RCGA01();
                    foreach (var o in idrc_list)
                    {
                        if (i == 1)
                        {
                            firstrc_report.initData(ff.DocID, ff.CompanyID, ff.RCompanyID, "", "original", "RCGA01", ff.RefID);
                            firstrc_report.DisplayName = ff.DocID;
                            firstrc_report.CreateDocument();
                        }

                        else if (i == 2)
                        {
                            RCGA01 next_report = new RCGA01();
                            next_report.initData(ff.DocID, ff.CompanyID, ff.RCompanyID, "", "copy", "RCGA01", ff.RefID);
                            firstrc_report.DisplayName = ff.DocID;
                            next_report.CreateDocument();
                            firstrc_report.Pages.AddRange(next_report.Pages);
                        }

                        i++;
                    }
                    firstrc_report.PrintingSystem.ContinuousPageNumbering = true;
                    docviewer.OpenReport(firstrc_report);

                    break;

                case "RCGA02"://ใบเสร็จรับเงินงานบริการ
                    List<int> idrc2_list = new List<int> { 1, 2, 3, 4 };

                    RCGA01 firstrc2_report = new RCGA01();
                    foreach (var o in idrc2_list)
                    {
                        if (i == 1)
                        {
                            firstrc2_report.initData(ff.DocID, ff.CompanyID, ff.RCompanyID, "ลูกค้า", "original", "RCGA02", ff.RefID);
                            firstrc2_report.DisplayName = ff.DocID;
                            firstrc2_report.CreateDocument();
                        }
                        else if (i == 2)
                        {
                            RCGA01 next_report = new RCGA01();
                            next_report.initData(ff.DocID, ff.CompanyID, ff.RCompanyID, "ลูกค้า", "copy", "RCGA02", ff.RefID);
                            firstrc2_report.DisplayName = ff.DocID;
                            next_report.CreateDocument();
                            firstrc2_report.Pages.AddRange(next_report.Pages);
                        }
                        else if (i == 3)
                        {
                            RCGA01 next_report = new RCGA01();
                            next_report.initData(ff.DocID, ff.CompanyID, ff.RCompanyID, "บัญชี", "copy", "RCGA02", ff.RefID);
                            firstrc2_report.DisplayName = ff.DocID;
                            next_report.CreateDocument();
                            firstrc2_report.Pages.AddRange(next_report.Pages);
                        }
                        else
                        {
                            RCGA01 next_report = new RCGA01();
                            next_report.initData(ff.DocID, ff.CompanyID, ff.RCompanyID, "การเงิน", "copy", "RCGA02", ff.RefID);
                            firstrc2_report.DisplayName = ff.DocID;
                            next_report.CreateDocument();
                            firstrc2_report.Pages.AddRange(next_report.Pages);
                        }
                        i++;
                    }
                    firstrc2_report.PrintingSystem.ContinuousPageNumbering = true;
                    docviewer.OpenReport(firstrc2_report);

                    break;
            }

        }

        private void LoadDevDropDownList()
        {
            cboVendor.DataSource = VendorInfoV2Service.ListViewVendorByID("");
            cboVendor.DataBind();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ShowAlert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (PreviousPrintPage == null)
            {
                PreviousPrintPage = "";
            }
            Response.RedirectPermanent(PreviousPrintPage); 
        }

    }
}