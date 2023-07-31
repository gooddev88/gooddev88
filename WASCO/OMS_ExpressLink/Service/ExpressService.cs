using DBF.Data.GADB;
using DBF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DBF.Service {
    public class ExpressService {
        public static string GetExpressPath() {
            string result = "";
            //string path = @"C:\Users\tammon.y\Desktop\AUDIT";
            string path = Properties.Settings.Default.ExpressDBPath;
            result = @"Provider=VFPOLEDB.1;Data Source=" + path;
            return result;
        }
        public static bool IsSOLock(string soId) {
            bool isLock = true;
            try {
                string query = $"select * from OESO where SONUM='{soId}'";
                DataTable datalist = GetDataFromFPQuery(query);

                foreach (DataRow row in datalist.Rows) {
                    if (row["DOCSTAT"].ToString() == "N") {
                        isLock = false;
                    }
                }
            } catch (Exception ex) {
                throw ex;
            }
            return isLock;
        }
        public static void DeleteSO(string soId) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            bool isLock = IsSOLock(soId);
            if (isLock) {
                return;
            }
            OleDbConnection yourConnectionHandler = new OleDbConnection(GetExpressPath());
            yourConnectionHandler.Open();
            if (yourConnectionHandler.State == ConnectionState.Open) {
                try {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = yourConnectionHandler;
                    command.CommandText = $"delete from  OESO where SONUM='{soId}'";
                    command.ExecuteNonQuery();
                    command.CommandText = $"delete from  OESOIT where SONUM='{soId}'";
                    command.ExecuteNonQuery();

                    yourConnectionHandler.Close();
                } catch (Exception ex) {
                    yourConnectionHandler.Close();
                    throw ex;
                }
            }

        }
        public static DataTable GetDataFromFPQuery(string mySQL) {

            DataTable YourResultSet = new DataTable();
            try {
                OleDbConnection yourConnectionHandler = new OleDbConnection(GetExpressPath());
                yourConnectionHandler.Open();
                if (yourConnectionHandler.State == ConnectionState.Open) {
                    OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                    OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);
                    DA.Fill(YourResultSet);
                    yourConnectionHandler.Close();
                }
            } catch (Exception ex) {
                throw ex;
            }
            return YourResultSet;
        }
        #region SO
        public static I_BasicResult LinkSOToExpress() {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                using (OMS_Entities db = new OMS_Entities()) {
                    var soIds = db.OSOHead.Where(o => o.IsLink == false && o.IsPrint==true && o.Status == "OPEN" && o.IsActive == true).Select(o => o.OrdID).ToList();
                    foreach (var soId in soIds) {
                        //output.Add(CreateSODocSet(soId));

                        try {
                            var doc = CreateSODocSet(soId);
                            DeleteSO(doc.Head.OrdID);
                            InsertSOHead(doc.Head);
                            InsertSOLine(doc.Head, doc.Line);
                            UpdateLinkStatus(doc.Head.OrdID);
                        } catch (Exception) {
                        }

                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }

            return result;
        }
        public static SODocSet CreateSODocSet(string soId) {
            SODocSet doc = new SODocSet();
            try {
                using (OMS_Entities db = new OMS_Entities()) {
                    doc.Head = db.vw_OSOHead.Where(o => o.OrdID == soId).FirstOrDefault();
                    doc.Line = db.vw_OSOLine.Where(o => o.OrdID == soId && o.IsActive == true).OrderBy(o=>o.LineNum).ToList();

                }
            } catch (Exception ex) {
                throw ex;
            }

            return doc;
        }
        public static I_BasicResult PackDB(string tablename) {

            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" }; 
            OleDbConnection conn = new OleDbConnection(GetExpressPath());
            conn.Open();
            if (conn.State == ConnectionState.Open) {
                try {

                    OleDbCommand command = new OleDbCommand();
                    //  string sql_command = $"Set Exclusive On; Pack {tablename}.dbf";
                    //  string sql_command = $"PACK  {tablename}";
                    string sql_command = $"Set Exclusive On; Pack   {tablename}";
                    command.Connection = conn;
                    command.CommandText = sql_command;
                    command.ExecuteNonQuery();
                     
                } catch (Exception ex) {
                    conn.Close();
                    if (ex.InnerException != null) {
                        result.Message1 = ex.InnerException.ToString();
                    } else {
                        result.Message1 = ex.Message;
                    }
                } finally {
                    conn.Close();
                }
               
            }
            return result;
        }
        public static I_BasicResult UpdateLinkStatus(string soId) {
            I_BasicResult result = new I_BasicResult();
            try {
                using (OMS_Entities db = new OMS_Entities()) {
                    var head = db.OSOHead.Where(o => o.OrdID == soId).FirstOrDefault();
                    if (head != null) {
                        head.IsLink = true;
                        head.LinkDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                result.Result = "fail";
                if (ex.InnerException != null) {
                    result.Message1 = ex.InnerException.ToString();
                } else {
                    result.Message1 = ex.Message;
                }
            }

            return result;
        }
        [Obsolete]
        public static void InsertSOHead(vw_OSOHead data) {
            OleDbConnection yourConnectionHandler = new OleDbConnection(DBFService.GetExpressPath());
            yourConnectionHandler.Open();

            if (yourConnectionHandler.State == ConnectionState.Open) {
                try {
                    string strInsert =
                               @" 
                                insert into OESO ( 
                                                SORECTYP,
                                                SONUM,
                                                SODAT,
                                                FLGVAT,
                                                DEPCOD,
                                                SLMCOD,
                                                CUSCOD,
                                                SHIPTO,
                                                YOUREF,
                                                RFF,
                                                AREACOD,
                                                PAYTRM,
                                                DLVDAT,
                                                DLVTIM,
                                                DLVDAT_IT,
                                                NXTSEQ,
                                                AMOUNT,
                                                DISC,
                                                DISCAMT,
                                                TOTAL,
                                                AMTRAT0,
                                                VATRAT,
                                                VATAMT,
                                                NETAMT,
                                                NETVAL,
                                                CMPLDAT,
                                                DOCSTAT,
                                                DLVBY,
                                                USERID,
                                                CHGDAT,
                                                USERPRN,
                                                PRNDAT,
                                                PRNCNT,
                                                PRNTIM,
                                                AUTHID,
                                                APPROVE,
                                                BILLTO,
                                                ORGNUM

                                            ) values(
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ({}),
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ?,
                                                ({}),
                                                ?,
                                                ?,
                                                ?,
                                                ({}),
                                                ?,
                                                ?

                                            )

                                            ";
                    OleDbCommand command = yourConnectionHandler.CreateCommand();
                    //command.CommandText = "SET NULL OFF";
                    //command.ExecuteNonQuery();
                    //OleDbCommand command = new OleDbCommand(strInsert, yourConnectionHandler);
                    command.CommandText = strInsert;
                    command.Parameters.AddWithValue("SORECTYP", "0");
                    command.Parameters.AddWithValue("SONUM", data.OrdID);
                    command.Parameters.AddWithValue("SODAT", data.OrdDate);
                    command.Parameters.AddWithValue("FLGVAT", "1");
                    command.Parameters.AddWithValue("DEPCOD", data.DepID);
                    command.Parameters.AddWithValue("SLMCOD", data.SalesID1);
                    command.Parameters.AddWithValue("CUSCOD", data.CustID);
                    command.Parameters.AddWithValue("SHIPTO", "");
                    command.Parameters.AddWithValue("YOUREF", data.POID);
                    command.Parameters.AddWithValue("RFF", "");
                    command.Parameters.AddWithValue("AREACOD", data.AreaID);
                    command.Parameters.AddWithValue("PAYTRM", Convert.ToDecimal(data.TermID));
                    command.Parameters.AddWithValue("DLVDAT", data.OrdDate);
                    command.Parameters.AddWithValue("DLVTIM", "");
                    command.Parameters.AddWithValue("DLVDAT_IT", "");
                    command.Parameters.AddWithValue("NXTSEQ", "1");
                    command.Parameters.AddWithValue("AMOUNT", data.NetTotalAmtIncVat);
                    command.Parameters.AddWithValue("DISC", "");
                    command.Parameters.AddWithValue("DISCAMT", 0);
                    command.Parameters.AddWithValue("TOTAL", data.NetTotalAmtIncVat);
                    command.Parameters.AddWithValue("AMTRAT0", 0);
                    command.Parameters.AddWithValue("VATRAT", data.VatRate);
                    command.Parameters.AddWithValue("VATAMT", data.NetTotalVatAmt);
                    command.Parameters.AddWithValue("NETAMT", data.NetTotalAmtIncVat);
                    command.Parameters.AddWithValue("NETVAL", data.NetTotalAmt);
                    //command.Parameters.AddWithValue("CMPLDAT", new DateTime(1900, 1, 1));
                    command.Parameters.AddWithValue("DOCSTAT", "N");
                    command.Parameters.AddWithValue("DLVBY", data.DeliveryBy);
                    command.Parameters.AddWithValue("USERID", "AUDIT");
                    command.Parameters.AddWithValue("CHGDAT", DateTime.Now.Date);
                    command.Parameters.AddWithValue("USERPRN", "");
                    //command.Parameters.AddWithValue("PRNDAT", new DateTime(1900, 1, 1));
                    command.Parameters.AddWithValue("PRNCNT", 0);
                    command.Parameters.AddWithValue("PRNTIM", "");
                    command.Parameters.AddWithValue("AUTHID", "");
                    //                    command.Parameters.AddWithValue("APPROVE", new DateTime(1900, 1, 1));
                    command.Parameters.AddWithValue("BILLTO", "");
                    command.Parameters.AddWithValue("ORGNUM", 0);



                    command.ExecuteNonQuery();
                    yourConnectionHandler.Close();
                } catch (Exception ex) {
                    yourConnectionHandler.Close();
                    throw ex;
                }
            }
        }
        [Obsolete]
        public static void InsertSOLine(vw_OSOHead head, List<vw_OSOLine> lines) {
            OleDbConnection yourConnectionHandler = new OleDbConnection(DBFService.GetExpressPath());
            yourConnectionHandler.Open();

            if (yourConnectionHandler.State == ConnectionState.Open) {
                try {
                    int i = 1;
                    foreach (var l in lines) {
                        l.LineNum = i;
                        i++;
                    }
                    foreach (vw_OSOLine data in lines.OrderBy(o=>o.LineNum)) {


                        string strInsert =
                                   @" 
                                insert into OESOIT ( 
                                                SORECTYP,
                                                SONUM,
                                                SEQNUM,
                                                SODAT,
                                                DLVDAT,
                                                CUSCOD,
                                                STKCOD,
                                                LOCCOD,
                                                STKDES,
                                                DEPCOD,
                                                VATCOD,
                                                FREE,
                                                ORDQTY,
                                                CANCELQTY,
                                                CANCELTYP,
                                                CANCELDAT,
                                                REMQTY,
                                                TFACTOR,
                                                UNITPR,
                                                TQUCOD,
                                                DISC,
                                                DISCAMT,
                                                TRNVAL,
                                                PACKING

                                            ) values(
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ({}),
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?,
                                                    ?


                                            )

                                            ";
                        OleDbCommand command = yourConnectionHandler.CreateCommand();
                        //command.CommandText = "SET NULL OFF";
                        //command.ExecuteNonQuery();
                        //OleDbCommand command = new OleDbCommand(strInsert, yourConnectionHandler);
                        command.CommandText = strInsert;
                        //command.Parameters.Add("@SORECTYP", OdbcType.Text).Value = "0";
                        //command.Parameters.Add("@SONUM", OdbcType.Text).Value = data.OrdID;
                        //command.Parameters.Add("@SEQNUM", OdbcType.Text).Value = data.LineNum.ToString();
                        //command.Parameters.Add("@SODAT", OdbcType.Date).Value = data.OrdDate;
                        //command.Parameters.Add("@DLVDAT", OdbcType.Date).Value = data.OrdDate;
                        //command.Parameters.Add("@CUSCOD", OdbcType.Text).Value = data.CustID;
                        //command.Parameters.Add("@STKCOD", OdbcType.Text).Value = data.ItemID;
                        //command.Parameters.Add("@LOCCOD", OdbcType.Text).Value = data.LocID;
                        //command.Parameters.Add("@STKDES", OdbcType.Text).Value = data.ItemName;
                        //command.Parameters.Add("@DEPCOD", OdbcType.Text).Value = head.DepID;
                        //command.Parameters.Add("@VATCOD", OdbcType.Text).Value = "";
                        //command.Parameters.Add("@FREE", OdbcType.Text).Value = "";
                        //command.Parameters.Add("@ORDQTY", OdbcType.Double).Value = Convert.ToDouble(data.Qty);
                        //command.Parameters.Add("@CANCELQTY", OdbcType.Double).Value = Convert.ToDouble(0);
                        //command.Parameters.Add("@CANCELTYP", OdbcType.Text).Value = "";
                        ////command.Parameters.Add("@CANCELDAT", OdbcType.DateTime).Value = DateTime.Now.Date;
                        //command.Parameters.Add("@REMQTY", OdbcType.Double).Value = Convert.ToDouble(data.Qty);
                        //command.Parameters.Add("@TFACTOR", OdbcType.Double).Value = Convert.ToDouble(1);
                        //command.Parameters.Add("@UNITPR", OdbcType.Double).Value = Convert.ToDouble(data.TotalAmtIncVat);


                        //#region utf-8
                        //string ansiString = data.Unit;
                        //byte[] ansiBytes = Encoding.Default.GetBytes(ansiString);
                        //string utf8String = Encoding.UTF8.GetString(ansiBytes);
                        //#endregion
                        //#region tis-620 
                        //Encoding tis620 = Encoding.GetEncoding("TIS-620");
                        //byte[] tis620Bytes = tis620.GetBytes(data.Unit);
                        //string tis620String = tis620.GetString(tis620Bytes);

                        //Encoding dbfEncoding = Encoding.GetEncoding(874); // TIS-620 encoding for Thai language
                        //byte[] dbfBytes = dbfEncoding.GetBytes("คก");
                        //#endregion

                        //command.Parameters.Add("@TQUCOD", OdbcType.Text).Value = dbfBytes;

                        //command.Parameters.Add("@DISC", OdbcType.Text).Value = "";
                        //command.Parameters.Add("@DISCAMT", OdbcType.Double).Value = Convert.ToDouble(0);
                        //command.Parameters.Add("@TRNVAL", OdbcType.Double).Value = Convert.ToDouble(data.TotalAmtIncVat);
                        //command.Parameters.Add("@PACKING", OdbcType.Text).Value = "";


                        command.Parameters.AddWithValue("SORECTYP", "0");
                        command.Parameters.AddWithValue("SONUM", data.OrdID);
                        command.Parameters.AddWithValue("SEQNUM", data.LineNum.ToString().PadLeft(3, ' '));
                        command.Parameters.AddWithValue("SODAT", data.OrdDate);
                        command.Parameters.AddWithValue("DLVDAT", data.OrdDate);
                        command.Parameters.AddWithValue("CUSCOD", data.CustID);
                        command.Parameters.AddWithValue("STKCOD", data.ItemID);
                        command.Parameters.AddWithValue("LOCCOD", data.LocID);
                        command.Parameters.AddWithValue("STKDES", data.ItemName);
                        command.Parameters.AddWithValue("DEPCOD", head.DepID);
                        command.Parameters.AddWithValue("VATCOD", "");
                        command.Parameters.AddWithValue("FREE", "");
                        command.Parameters.AddWithValue("ORDQTY", data.Qty);
                        command.Parameters.AddWithValue("CANCELQTY", 0);
                        command.Parameters.AddWithValue("CANCELTYP", "");
                        //command.Parameters.AddWithValue("CANCELDAT", DateTime.Now.Date);
                        command.Parameters.AddWithValue("REMQTY", data.Qty);
                        command.Parameters.AddWithValue("TFACTOR", 1);
                        command.Parameters.AddWithValue("UNITPR", data.PriceIncVat);
                        #region tis 620
                        //string input = "คร";
                        //Encoding tis620 = Encoding.GetEncoding("TIS-620");
                        //byte[] tis620Bytes = tis620.GetBytes(input);
                        //string tis620String = tis620.GetString(tis620Bytes);

                        // Encoding dbfEncoding = Encoding.GetEncoding(874); // TIS-620 encoding for Thai language
                        // byte[] dbfBytes = dbfEncoding.GetBytes("คร");
                        //string tis620String = dbfEncoding.GetString(dbfBytes);


                        //string test = "ขจ";

                        ////Get the UTF8 encoding
                        //UTF8Encoding encoder = new System.Text.UTF8Encoding();
                        //byte[] utf8 = encoder.GetBytes(test);

                        ////Convert the UTF8 encoding to tis620
                        //byte[] tis620 = Encoding.Convert(Encoding.UTF8,
                        //Encoding.GetEncoding(874),
                        //utf8);


                        //string input = "ขจ";

                        //// Convert string to ANSI encoding
                        //Encoding ansi = Encoding.Default;
                        //byte[] ansiBytes = ansi.GetBytes(input);

                        //// Convert ANSI bytes back to string
                        //string ansiString = ansi.GetString(ansiBytes);

                        // Convert string to OEM encoding
                        //Encoding oem = Encoding.GetEncoding(437);
                        //byte[] oemBytes = oem.GetBytes(input);

                        //// Convert OEM bytes back to string
                        //string oemString = oem.GetString(oemBytes);


                        //TIS620-encoded string is now in byte array tis620
                        #endregion

                        command.Parameters.AddWithValue("TQUCOD", data.Unit);
                        command.Parameters.AddWithValue("DISC", "");
                        command.Parameters.AddWithValue("DISCAMT", 0);
                        command.Parameters.AddWithValue("TRNVAL", data.TotalAmtIncVat);
                        command.Parameters.AddWithValue("PACKING", "");
                        command.ExecuteNonQuery();
                    }
                    yourConnectionHandler.Close();



                     


                } catch (Exception ex) {
                    yourConnectionHandler.Close();
                    throw ex;
                }
            }
        }
        #endregion

        #region location
        public static List<LocationInfo> GetLocation() {
            List<LocationInfo> result = new List<LocationInfo>();
            try {
                string query = "select * from istab where TABTYP='21'";
                DataTable datalist = GetDataFromFPQuery(query);

                foreach (DataRow row in datalist.Rows) {
                    LocationInfo n = new LocationInfo();
                    n.LocID = row["TYPCOD"].ToString().Trim();
                    n.RCompanyID = "WASCO-GROUP";
                    n.CompanyID = "WASCO";
                    n.LocCode = row["SHORTNAM"].ToString().Trim();
                    n.LocTypeID = "";
                    n.ParentID = "";
                    n.Name = row["SHORTNAM2"].ToString().Trim();
                    n.Remark = "";
                    n.CreatedBy = "AUTO";
                    n.CreatedDate = DateTime.Now;
                    n.ModifiedBy = "AUTO";
                    n.ModifiedDate = DateTime.Now;
                    n.IsActive = true;
                    result.Add(n);
                }

            } catch (Exception ex) {
                throw ex;
            }
            return result;

        }
        public static I_BasicResult UpdateLocation() {

            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<LocationInfo> exp_data = GetLocation();
                using (OMS_Entities db = new OMS_Entities()) {
                    var oms_location = db.LocationInfo.ToList();
                    foreach (var exp in exp_data) {
                        var oms_exist = oms_location.Where(o => o.LocCode == exp.LocCode && o.RCompanyID == exp.RCompanyID).FirstOrDefault();
                        if (oms_exist != null) {
                            oms_exist.LocID = exp.LocID;
                            oms_exist.LocCode = exp.LocCode;
                            oms_exist.Name = exp.Name;
                        } else {
                            db.LocationInfo.Add(exp);
                        }
                        db.SaveChanges();
                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        #endregion
        #region Customer
        public static List<SelectOption> GetCustBrand() {
            List<SelectOption> result = new List<SelectOption>();
            try {
                string query = @"select * from istab where TABTYP='45'";
                DataTable datalist = GetDataFromFPQuery(query);
                foreach (DataRow row in datalist.Rows) {
                    SelectOption n = new SelectOption();
                    n.Value = row["TYPCOD"].ToString().Trim();
                    n.Description = row["SHORTNAM"].ToString().Trim();
                    result.Add(n);
                }
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public static List<CustomerInfo> GetCust() {
            List<CustomerInfo> result = new List<CustomerInfo>();
            try {
                var brands = GetCustBrand();
                string query = "select * from ARMAS ";
                DataTable datalist = GetDataFromFPQuery(query);
                foreach (DataRow row in datalist.Rows) {
                    CustomerInfo n = new CustomerInfo();
                    n.RCompanyID = "WASCO-GROUP";
                    n.CompanyID = "WASCO";
                    n.CustomerID = row["CUSCOD"].ToString().Trim();
                    n.CustCode = "";
                    n.OrgType = "";
                    n.TypeID = "CUSTOMER";
                    n.ParentID = "";
                    n.Birthdate = null;
                    n.ShortCode = "";
                    n.BrnCode = "";
                    n.BrnDesc = "สำนักงานใหญ่";
                    n.RefID = "";
                    n.TitleTh = row["PRENAM"].ToString().Trim();
                    n.NameTh1 = row["CUSNAM"].ToString().Trim();
                    n.NameTh2 = "";
                    n.NameTh3 = "";
                    n.TitleEn = "";
                    n.NameEn1 = "";
                    n.NameEn2 = "";
                    n.NameEn3 = "";
                    n.PersonTypeID = "";
                    var custyp = row["CUSTYP"].ToString().Trim();
                    var bname = brands.Where(o => o.Value == custyp).FirstOrDefault();
                    if (bname != null) {
                        n.GroupID = bname.Description;
                    } else { n.GroupID = ""; }


                    n.SubGroupID = "";
                    n.ProductGroupID = "";
                    n.TaxID = row["TAXID"].ToString().Trim();
                    n.Currency = "TH";
                    n.CardExpireDate = null;
                    n.CardIssue = "";
                    n.TaxTypeID = "VAT7";
                    n.PaymentTermID = row["PAYTRM"].ToString().Trim();
                    n.PaymentGrade = "";
                    n.CreditLimit = Convert.ToDecimal(row["CRLINE"].ToString());
                    n.ContactPerson = row["CONTACT"].ToString().Trim();
                    n.AddrFull = "";
                    n.AddrNo = "";
                    n.AddrMoo = "";
                    n.Building = "";
                    n.RoomNo = "";
                    n.FloorNo = "";
                    n.Village = "";
                    n.Soi = "";
                    n.Yaek = "";
                    n.Road = "";
                    n.AddrTumbon = "";
                    n.AddrAmphoe = "";
                    n.AddrProvince = "";
                    n.AddrPostCode = row["ZIPCOD"].ToString().Trim();
                    n.AddrCountry =
                    n.BillAddr1 = row["ADDR01"].ToString().Trim();
                    n.BillAddr2 = row["ADDR02"].ToString().Trim();
                    n.BillMemo = row["REMARK"].ToString().Trim();
                    n.BillContact = "";
                    n.PaymentMethod = "";
                    n.Tel1 = row["TELNUM"].ToString().Trim();
                    n.Tel2 = "";
                    n.Mobile = "";
                    n.Email = "";
                    n.Fax = row["DLVBY"].ToString().Trim();
                    n.LineID = "";
                    n.SalePersonID = row["SLMCOD"].ToString().Trim();
                    n.AreaID = row["AREACOD"].ToString().Trim();
                    n.BankCode = "";
                    n.BookIBankID = "";
                    n.Occupation = "";
                    n.Gender = "";
                    n.Marriage = "";
                    n.Lang = "";
                    n.Race = "";
                    n.Nationality = "";
                    n.Status = "OPEN";
                    n.IsHold = false;
                    n.Photo = null;
                    n.Geolocation = "";
                    n.Point = 0;
                    n.IsLockPrice = true;
                    n.AccID_Debtor = "";
                    n.AccID_Creditor = "";
                    n.AccID_Wht = "";
                    n.Acc_Side = "";
                    n.CreatedBy = row["CREBY"].ToString().Trim();
                    n.CreatedDate = Convert.ToDateTime(row["CREDAT"].ToString());
                    n.ModifiedBy = row["USERID"].ToString().Trim();
                    if (row["CHGDAT"] != null) {
                        n.ModifiedDate = Convert.ToDateTime(row["CHGDAT"].ToString());
                    }
                    n.IsActive = true;
                    result.Add(n);
                }
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public static I_BasicResult UpdateCust() {

            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<CustomerInfo> exp_data = GetCust();
                using (OMS_Entities db = new OMS_Entities()) {
                    var oms_cust = db.CustomerInfo.ToList();
                    foreach (var exp in exp_data) {
                        var oms_exist = oms_cust.Where(o => o.CustomerID == exp.CustomerID && o.RCompanyID == exp.RCompanyID).FirstOrDefault();
                        if (oms_exist != null) {
                            oms_exist.CustomerID = exp.CustomerID;
                            oms_exist.TitleTh = exp.TitleTh;
                            oms_exist.NameTh1 = exp.NameTh1;
                            oms_exist.GroupID = exp.GroupID;
                            oms_exist.TaxID = exp.TaxID;
                            oms_exist.PaymentTermID = exp.PaymentTermID;
                            oms_exist.CreditLimit = exp.CreditLimit;
                            oms_exist.ContactPerson = exp.ContactPerson;
                            oms_exist.AddrPostCode = exp.AddrPostCode;
                            oms_exist.BillAddr1 = exp.BillAddr1;
                            oms_exist.BillAddr2 = exp.BillAddr2;
                            oms_exist.BillMemo = exp.BillMemo;
                            oms_exist.Tel1 = exp.Tel1;
                            oms_exist.Fax = exp.Fax;
                            oms_exist.SalePersonID = exp.SalePersonID;
                            oms_exist.AreaID = exp.AreaID; 
                            oms_exist.CreatedBy = exp.CreatedBy;
                            oms_exist.CreatedDate = exp.CreatedDate;
                            oms_exist.ModifiedBy = exp.ModifiedBy;
                            oms_exist.ModifiedDate = exp.ModifiedDate;
                            //oms_exist.IsActive = true;
                            db.SaveChanges();
                        } else {
                            db.CustomerInfo.Add(exp);
                            db.SaveChanges();
                        }

                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        #endregion
        #region  Item
        public static List<ItemInfo> GetItem() {
            List<ItemInfo> result = new List<ItemInfo>();
            try {

                string query = "select * from STMAS ";
                DataTable datalist = GetDataFromFPQuery(query);
                foreach (DataRow row in datalist.Rows) {
                    ItemInfo n = new ItemInfo();



                    n.ItemID = row["STKCOD"].ToString().Trim();
                    n.ItemCode = "";
                    n.RCompanyID = "WASCO-GROUP";
                    n.CompanyID = "WASCO";
                    n.Model = "";
                    n.Color = "";
                    n.Size = "";
                    n.RefID = "";
                    n.Name1 = row["STKDES"].ToString().Trim();
                    n.Name2 = row["STKDES2"].ToString().Trim();
                    n.TypeID = "FG";
                    n.CateID = row["SUPCOD"].ToString().Trim();
                    n.Group1ID = "";
                    n.Group2ID = "";
                    n.Group3ID = "";
                    n.BrandID = row["STKGRP"].ToString().Trim();
                    n.Cost = Convert.ToDecimal(row["LPURPR"].ToString().Trim());
                    n.Price = 0;
                    n.PriceIncVat = Convert.ToDecimal(row["SELLPR1"].ToString().Trim());
                    n.PriceProIncVat = Convert.ToDecimal(row["SELLPR2"].ToString().Trim());
                    n.UnitID = row["QUCOD"].ToString().Trim();
                    n.StkUnitID = row["QUCOD"].ToString().Trim();
                    n.VendorID = "";
                    n.Weight = 0;
                    n.Dimension = 0;
                    n.TaxTypeID = "";
                    n.TaxGroupID = "";
                    n.IsKeepStock = true;
                    n.Remark1 = "";
                    n.Remark2 = "";
                    n.AccID_Sale = "";
                    n.AccID_Purchase = "";
                    n.Acc_Side = "";
                    n.PhotoID = null;
                    n.CreatedBy = row["CREBY"].ToString().Trim();
                    n.CreatedDate = Convert.ToDateTime(row["CREDAT"].ToString());
                    n.ModifiedBy = row["USERID"].ToString().Trim();
                    n.ModifiedDate = Convert.ToDateTime(row["CHGDAT"].ToString());
                    n.Status = "";
                    n.IsHold = false;

                    if (Convert.ToDateTime(row["INACTDAT"]) != new DateTime(1899, 12, 30)) {
                        n.IsActive = false;
                    } else {
                        n.IsActive = true;
                    }



                    n.IsActive = true;
                    result.Add(n);
                }
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public static I_BasicResult UpdateItem() {

            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<ItemInfo> exp_data = GetItem();
                using (OMS_Entities db = new OMS_Entities()) {
                    var oms_item = db.ItemInfo.ToList();
                    foreach (var exp in exp_data) {
                        var oms_exist = oms_item.Where(o => o.ItemID == exp.ItemID && o.RCompanyID == exp.RCompanyID).FirstOrDefault();
                        if (oms_exist != null) {

                            oms_exist.ItemID = exp.ItemID;
                            oms_exist.Name1 = exp.Name1;
                            oms_exist.Name2 = exp.Name2;
                            oms_exist.CateID = exp.CateID;
                            oms_exist.BrandID = exp.BrandID;
                            oms_exist.Cost = exp.Cost;
                            oms_exist.PriceIncVat = exp.PriceIncVat;
                            oms_exist.PriceProIncVat = exp.PriceProIncVat;
                            oms_exist.UnitID = exp.UnitID;
                            oms_exist.StkUnitID = exp.StkUnitID;
                            oms_exist.CreatedBy = exp.CreatedBy;
                            oms_exist.CreatedDate = exp.CreatedDate;
                            oms_exist.ModifiedBy = exp.ModifiedBy;
                            oms_exist.ModifiedDate = exp.ModifiedDate;
                            db.SaveChanges();
                        } else {
                            db.ItemInfo.Add(exp);
                            db.SaveChanges();
                        }

                    }
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        #endregion
        #region  User
        public static List<UserInfo> GetUser() {
            List<UserInfo> result = new List<UserInfo>();
            try {

                string query = "select * from OESLM ";
                DataTable datalist = GetDataFromFPQuery(query);
                foreach (DataRow row in datalist.Rows) {
                    UserInfo n = new UserInfo();

                    n.Username = row["SLMCOD"].ToString().Trim();
                    n.Password = "1811dc19b1db1521d014d1c21001361db1d813113e1d0155";
                    n.EmpCode = row["SLMCOD"].ToString().Trim();
                    n.RCompanyID = "WASCO-GROUP";
                    n.CompanyID = "WASCO";
                    n.Title = "";
                    n.FirstName = row["SLMNAM"].ToString().Trim();
                    n.LastName = "";
                    n.Title_En = "";
                    n.FirstName_En = "";
                    n.FullName_En = "";
                    n.NickName = "";
                    n.Gender = "";
                    n.DepartmentID = "";
                    n.SubDepartmentID = "";
                    n.PositionID = row["POSITN"].ToString().Trim();
                    n.JobLevel = "";
                    n.JobType = "";
                    n.IsProgramUser = true;
                    n.IsNewUser = true;
                    n.JobStartDate = null;
                    n.ResignDate = null;
                    n.AddrFull = row["ADDR01"].ToString().Trim() + row["ADDR02"].ToString().Trim() + row["ADDR03"].ToString().Trim();
                    n.AddrNo = "";
                    n.AddrMoo = "";
                    n.AddrTumbon = "";
                    n.AddrAmphoe = "";
                    n.AddrProvince = "";
                    n.AddrPostCode = "";
                    n.AddrCountry = "";
                    n.Tel = "";
                    n.Mobile = row["TELNUM"].ToString().Trim();
                    n.Email = "";
                    n.Birthdate = null;
                    n.MaritalStatus = "";
                    n.CitizenId = row["TAXID"].ToString().Trim();
                    n.BookBankNumber = "";
                    n.AuthenType = "DB";
                    n.ApproveBy = "";
                    n.UseTimeStamp = false;
                    n.ImageProfile = "";
                    n.LineToken = "";
                    n.IsSuperMan = false;
                    n.DefaultCompany = "";
                    n.DefaultMenu = "";
                    n.UserType = "";
                    n.RelateID = "";
                    n.JwtToken = "";
                    n.JwtRefreshToken = "";
                    n.JwtTokenExpiryDate = null;
                    n.CreatedBy = row["USERID"].ToString().Trim();
                    n.CreatedDate = Convert.ToDateTime(row["CREATE"].ToString());
                    n.ModifiedBy = row["USERID"].ToString().Trim();
                    n.ModifiedDate = Convert.ToDateTime(row["CHGDAT"].ToString());
                    if (Convert.ToDateTime(row["INACTDAT"]) != new DateTime(1899, 12, 30)) {
                        n.IsActive = false;
                    } else {
                        n.IsActive = true;
                    }

                    result.Add(n);
                }
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public static I_BasicResult UpdateUser() {

            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<UserInfo> exp_data = GetUser();
                using (OMS_Entities db = new OMS_Entities()) {
                    var oms_user = db.UserInfo.ToList();
                    var oms_user_rcom = db.UserInRCom.ToList();
                    foreach (var exp in exp_data) {
                        var oms_exist = oms_user.Where(o => o.Username == exp.Username).FirstOrDefault();
                        if (oms_exist != null) {
                            oms_exist.Username = exp.Username;
                            oms_exist.EmpCode = exp.EmpCode;
                            oms_exist.FirstName = exp.FirstName;
                            oms_exist.SubDepartmentID = exp.SubDepartmentID;
                            oms_exist.PositionID = exp.PositionID;
                            oms_exist.AddrFull = exp.AddrFull;
                            oms_exist.Mobile = exp.Mobile;
                            oms_exist.CitizenId = exp.CitizenId;
                            oms_exist.CreatedBy = exp.CreatedBy;
                            oms_exist.CreatedDate = exp.CreatedDate;
                            oms_exist.ModifiedBy = exp.ModifiedBy;
                            oms_exist.ModifiedDate = exp.ModifiedDate;

                        } else {
                            db.UserInfo.Add(exp);
                        }
                        var oms_exit_inrcom = oms_user_rcom.Where(o => o.UserName == exp.Username && o.RComID == "WASCO-GROUP").FirstOrDefault();
                        if (oms_exit_inrcom == null) {
                            UserInRCom uir = new UserInRCom();
                            uir.RComID = "WASCO-GROUP";
                            uir.UserName = exp.Username;
                            db.UserInRCom.Add(uir);
                        }
                        var has_in_group = db.UserInGroup.Where(o => o.UserName == exp.Username && o.UserGroupID == "SALE").FirstOrDefault();
                        if (has_in_group == null) {
                            UserInGroup g = new UserInGroup();
                            g.UserGroupID = "SALE";
                            g.RComID = exp.RCompanyID;
                            g.UserName = exp.Username;
                            g.IsActive = true;
                            db.UserInGroup.Add(g);

                        }

                    }
                    db.SaveChanges();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
        #endregion



        #region  Stock
        public static List<STKBal> GetStockBal() {
            List<STKBal> result = new List<STKBal>();
            try {

                string query = "select * from STLOC ";
                DataTable datalist = GetDataFromFPQuery(query);
                foreach (DataRow row in datalist.Rows) {
                    STKBal n = new STKBal();
                    n.RComID = "WASCO-GROUP";
                    n.ComID = "WASCO";
                    n.LocID = row["LOCCOD"].ToString().Trim();
                    n.SubLocID = "";
                    n.ItemID = row["STKCOD"].ToString().Trim();
                    n.LotNo = "";
                    n.SerialNo = "";
                    n.OrdQty = 0;
                    n.InstQty = 0;
                    n.BalQty = Convert.ToDecimal(row["LOCBAL"].ToString());
                    n.RetQty = 0;
                    n.Unit = "";
                    n.IsActive = true;


                    result.Add(n);
                }
            } catch (Exception ex) {
                throw ex;
            }
            return result;
        }
        public static I_BasicResult UpdateStock() {
            I_BasicResult r = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
            try {
                List<STKBal> exp_data = GetStockBal();
                using (OMS_Entities db = new OMS_Entities()) {
                    db.STKBal.AddRange(exp_data);
                    db.SaveChanges();
                    db.sp_ResetStock();
                }
            } catch (Exception ex) {
                r.Result = "fail";
                if (ex.InnerException != null) {
                    r.Message1 = ex.InnerException.ToString();
                } else {
                    r.Message1 = ex.Message;
                }
            }
            return r;
        }
		#endregion
		public static I_BasicResult TestConnectSQL() {
			I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
			try {
				using (OMS_Entities db = new OMS_Entities()) {
                  var count= db.OSOHead.Count();
					 result.Message1 = count.ToString();
				}
			} catch (Exception ex) {
				result.Result = "fail";
				if (ex.InnerException != null) {
					result.Message1 = ex.InnerException.ToString();
				} else {
					result.Message1 = ex.Message;
				}
			}

			return result;
		}
	}
}
