using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Export;
using DevExpress.Web;
using System.Drawing;
using DevExpress.XtraPrinting;
using Robot.Data;
using Robot.Data.DataAccess;

namespace Robot.CRM {
    public partial class CustomerDetail : MyBasePage {

        protected void Page_Load(object sender, EventArgs e)
        {
            hddmenu.Value = Request.QueryString["menu"];
            hddid.Value = Request.QueryString["id"];

            LoadDropDownDevList();

            popup1.ShowOnPageLoad = false;
            if (!IsPostBack)
            {
                CloseAlert();
                LoadDropDownList();
                hddPreviouspage.Value = Session["CustomerStock_previouspage"].ToString();
                LoadData();
            }

        }


        public string SetMenu(object menu_id)
        {
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null)
            {
                result = data.IsOpen.ToString();
            }
            return result;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }
        private void LoadDropDownList()
        {

        }
        private void LoadDropDownDevList()
        {
            //cboCompany.DataSource = CompanyInfoService.MiniSelectList("BRANCH", false).ToList(); ;
            //cboCompany.DataBind();
            var custbrn = (List<CustomerAddrInfo>)Session[hddid.Value + "customeraddr"];
        }
        private void ClearSession()
        {
            Session[hddmenu.Value + "SOrd_head"] = null;
            Session[hddmenu.Value + "SOrd_lines"] = null;
        }
        private void SetStatusCaption(CustomerInfo data)
        {
            string strstatus = "<span class=" + "\"" + "badge badge-pill badge-info" + "\"" + ">++NEW++</span>";
            if (data == null)
            {
                //litStatus.Text = Server.HtmlDecode(strstatus);
                return;
            }
            if (!data.IsActive)
            {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-danger" + "\"" + ">DELETED</span>";
            }
            if (data.Status == "CLOSED")
            {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-dark" + "\"" + ">CLOSE</span>";
            }
            if (data.Status == "PENDING")
            {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-warning" + "\"" + ">PENDING</span>";
            }
            if (data.Status == "OPEN")
            {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-success" + "\"" + ">OPEN</span>";
            }
            if (data.Status == "CANCEL")
            {
                strstatus = "<span class=" + "\"" + "badge badge-pill badge-danger" + "\"" + ">CANCEL</span>";
            }
            //litStatus.Text = Server.HtmlDecode(strstatus);
        }


        private void LoadData()
        {
            if (hddid.Value == "")
            {
                ClearSession();
                SetupControl("NEW");
                NewHead();                
            }

            if (hddid.Value == "")
            {
                return;
            }

            CustomerInfo head = new CustomerInfo();
            head = CustomerInfoService.GetDataByID(hddid.Value);


            SetupControl("EDIT");
            Session[hddmenu.Value + "Cus_head"] = head;

            BindData();
           
        }

        private void BindData()
        {
            var head = (CustomerInfo)Session[hddmenu.Value + "Cus_head"];

            if (hddid.Value != "")
            {
                txtCustomerID.Text = head.CustomerID;
                txtCustomerID.ReadOnly = false;
                SetStatusCaption(null);
            }
            else
            {
                txtCustomerID.Text = hddid.Value;
            }

            //cboCompany.Value = head.CompanyID;
            txtNameTh1.Text = head.NameTh1;
            txtNameTh2.Text = head.NameTh2;
            dtBirthday.Value = head.Birthdate;
            txtTaxID.Text = head.TaxID;
            txtMobile.Text = head.Mobile;
            txtEmail.Text = head.Email;
            txtAddrNo.Text = head.AddrNo;
            txtAddrMoo.Text = head.AddrMoo;
            txtAddrTumbon.Text = head.AddrTumbon;
            txtAddrAmphoe.Text = head.AddrAmphoe;
            txtAddrProvince.Text = head.AddrProvince;
            txtAddrPostCode.Text = head.AddrPostCode;
            lblPoint.Text = Convert.ToDecimal( head.CrmPoint).ToString("N2");
        }
        private void SetupControl(string statusdoc)
        {
            if (statusdoc == "NEW")
            {
                btnDel.Visible = false;
            }
            if (statusdoc == "EDIT")
            {
                //cboCompany.Enabled = false;
                btnDel.Visible = true;
            }
        }


        protected void btnPostCodeRefresh_Click(object sender, EventArgs e)
        {
            var data = (Addr_District)Session["p_sel_addr"];
            if (data == null)
            {
                return;
            }
            txtAddrTumbon.Text = data.DistrictID;
            txtAddrAmphoe.Text = data.DistrictID;
            txtAddrPostCode.Text = data.PostCode;
            txtAddrProvince.Text = data.ProvinceID;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(hddPreviouspage.Value);
        }



        #region Search area
  

        #endregion

        #region fucntion move doc helper

        private void NewHead()
        {
            CustomerInfo newdata = CustomerInfoService.NewCusHead();

            Session[hddmenu.Value + "Cus_head"] = newdata;
            BindData();
        }

        #endregion

        #region Control move line helper 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Validate Data
                string resultValid = ValidData();
                if (resultValid != "")
                {
                    ShowAlert(resultValid, "Error");
                    return;
                }
                #endregion

                var isnew = PrepairDataSave();

                var head = (CustomerInfo)Session[hddmenu.Value + "Cus_head"];
                if (head.CustomerID == "")
                {
                    return;
                }
                List<string> r1 = new List<string>();

                using (GAEntities db = new GAEntities())
                {
                    if (isnew)
                    {
                        r1 = CustomerInfoService.SaveCus(head, "insert");
                    }
                    else
                    {
                        r1 = CustomerInfoService.SaveCus(head, "update");
                    }
                    if (r1[0] == "R0")
                    {//save successufull
                        ShowAlert(r1[1], "Error");                        
                    }
                    else
                    {
                        if (isnew)
                        {
                            ShowAlert("บันทึกข้อมูลสำเร็จ", "Success");
                            string url = "/CRM/CustomerDetail.aspx?id=" + head.CustomerID + "&menu=" + hddmenu.Value;
                            Response.Redirect(url);
                            ShowAlert("บันทึกข้อมูลสำเร็จ", "Success");
                        }
                        else
                        {
                            LoadData();
                            ShowAlert("บันทึกข้อมูลสำเร็จ", "Success");
                        }
                    }
                }

            } catch (Exception ex)
            {
                var resultValid = "Save Error becuase : " + ex.Message.ToString();
                ShowAlert(resultValid, "Error");
            }        
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            var r = CustomerInfoService.Delete(hddid.Value);
            if (r[0] == "R1")
            {
                ShowAlert("ลบข้อมูลสำเร็จ", "Success");
                string myurl = "/CRM/CustomerList.aspx?id=" + "" + "&menu=" + hddmenu.Value;
                Response.RedirectPermanent(myurl);
            }
            else
            {
                //ShowAlertMain("Delete fail : " + r[1], false);
                ShowAlert("Error" + r[1], "Error");
                return;
            }
        }

        private void ShowAlert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "ShowAlert('" + msg + "','" + type + "');", true);
        }
        private void CloseAlert()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "CloseAlert();", true);
        }

        protected void btnBackList_Click(object sender, EventArgs e)
        {
            Response.RedirectPermanent(hddPreviouspage.Value);
        }


        #endregion

        private string ValidData()
        {
            //if (txtMobile.Text == "")
            //{
            //    return "";
            //}

            return "";
        }


        private bool PrepairDataSave()
        {
            string user = LoginService.GetCurrentloginUser();

            bool isnewrec = false;//true =new trans / false = edit trans 
            var head = (CustomerInfo)Session[hddmenu.Value + "Cus_head"];
            if (head == null)
            {
                head = CustomerInfoService.NewCusHead();
            }

            if (hddid.Value == "")
            {
                isnewrec = true;
                if (txtCustomerID.Text.Trim() != "" && txtCustomerID.Text.Trim() != "++NEW++")
                {//
                    head.CustomerID = txtCustomerID.Text.Trim();
                }
                else
                {//gen
                    var comdata = CompanyInfoService.GetDataByComID("");
                    head.CustomerID = IDGeneratorServiceV2.GenNewID("CUSTOMER_INFO", head.CompanyID, true, DateTime.Now.Date, "th")[1];
                    if (head.CustomerID == "")
                    {
                        ShowAlert("No ducument type found.", "Error");
                        return true;
                    }
                }

            }

            #region head data

            //head.CompanyID = "";
            //if (cboCompany.Value != null)
            //{
            //    head.CompanyID = cboCompany.Value.ToString();
            //}
            head.NameTh1 = txtNameTh1.Text.Trim();
            head.NameTh2 = txtNameTh2.Text.Trim();
            head.TaxID = txtTaxID.Text.Trim();
            head.Mobile = txtMobile.Text.Trim();
            head.Email = txtEmail.Text.Trim();
            head.Birthdate = dtBirthday.Date;
            head.AddrNo = txtAddrNo.Text.Trim();
            head.AddrMoo = txtAddrMoo.Text.Trim();
            head.AddrTumbon = txtAddrTumbon.Text.Trim();
            head.AddrAmphoe = txtAddrAmphoe.Text.Trim();
            head.AddrProvince = txtAddrProvince.Text.Trim();
            head.AddrPostCode = txtAddrPostCode.Text.Trim();


            #endregion

            BindData();
            Session[hddmenu.Value + "SOrd_head"] = head;
            return isnewrec;
        }

    }

}