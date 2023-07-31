using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Windows.Forms;
using DBF.Model;

namespace DBF {
    public class DBFService {
        public static string GetExpressPath() {
            string result = "";

            //    string path = @"C:\Users\tammon.y\Desktop\db-wasco\db_files\";
            //   string path = @"C:\Users\tammon.y\Desktop\AUDIT\";
            string path = Properties.Settings.Default.ExpressDBPath;
            result = @"Provider=VFPOLEDB.1;Data Source=" + path + ";Collating Sequence=general;";
            //  var  x= "Provider=vfpoledb;Data Source=C:\\MyDataDirectory\\MyTable.dbf;Collating Sequence=machine;"
            return result;
        }
        public static string GetExpressPath2() {
            string result = "";

            //    string path = @"C:\Users\tammon.y\Desktop\db-wasco\db_files\";
            string path = @"C:\Users\tammon.y\Desktop\AUDIT";
            result = @"Driver={Microsoft dBASE Driver (*.dbf)};Dbq=" + path;


            return result;
        }
        public static DataTable GetDataFromFPQuery(string mySQL) {

            DataTable YourResultSet = new DataTable();
            try {
                OleDbConnection yourConnectionHandler = new OleDbConnection(GetExpressPath());

                // if including the full dbc (database container) reference, just tack that on
                //      OleDbConnection yourConnectionHandler = new OleDbConnection(
                //          "Provider=VFPOLEDB.1;Data Source=C:\\SomePath\\NameOfYour.dbc;" );

                // Open the connection, and if open successfully, you can try to query it
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


        public static void Insert() {
            OleDbConnection yourConnectionHandler = new OleDbConnection(GetExpressPath());
            yourConnectionHandler.Open();
            if (yourConnectionHandler.State == ConnectionState.Open) {
                try {
                    string strInsert = "INSERT INTO istab (TABTYP,TYPCOD,SHORTNAM,SHORTNAM2,TYPDES,TYPDES2,FLD01,FLD02,DEPCOD,STATUS) VALUES (?, ?,?,?,?,?,?,?,?,?)";
                    OleDbCommand command = new OleDbCommand(strInsert, yourConnectionHandler);
                    command.Parameters.Add("@TABTYP", OdbcType.Text).Value = Guid.NewGuid().ToString().Substring(0, 5);
                    command.Parameters.Add("@TYPCOD", OdbcType.Text).Value = "S66";
                    command.Parameters.Add("@SHORTNAM", OdbcType.Text).Value = "77";
                    command.Parameters.Add("@SHORTNAM2", OdbcType.Text).Value = "";
                    command.Parameters.Add("@TYPDES", OdbcType.Text).Value = "กค";
                    command.Parameters.Add("@TYPDES2", OdbcType.Text).Value = "คค";
                    command.Parameters.Add("@FLD01", OdbcType.Text).Value = "";
                    command.Parameters.Add("@FLD02", OdbcType.Numeric).Value = 0.0;
                    command.Parameters.Add("@DEPCOD", OdbcType.Text).Value = "";
                    command.Parameters.Add("@STATUS", OdbcType.Text).Value = "";
                    command.ExecuteNonQuery();
                    yourConnectionHandler.Close();
                } catch (Exception ex) {
                    yourConnectionHandler.Close();
                    MessageBox.Show(ex.Message);
                }
            }

        }

        public static void Delete() {
            OleDbConnection yourConnectionHandler = new OleDbConnection(GetExpressPath());
            yourConnectionHandler.Open();
            if (yourConnectionHandler.State == ConnectionState.Open) {
                try {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = yourConnectionHandler;
                    command.CommandText = $"delete from istab where TYPCOD='S66'";
                    command.ExecuteNonQuery();


                    yourConnectionHandler.Close();
                } catch (Exception ex) {
                    yourConnectionHandler.Close();
                    MessageBox.Show(ex.Message);
                }
            }

        }
      
        public static string Select() {

            string result = "";
            string query = "select * from istab where SHORTNAM='77'";

            DataTable datalist = GetDataFromFPQuery(query);
            foreach (DataRow row in datalist.Rows) {
                var value1 = row["TABTYP"];
                var value2 = row["SHORTNAM2"];
                result = result + $"Value1: {value1}, Value2: {value2}" + Environment.NewLine;

            }
            return result;
        }
        public static string Select2() {

            string result = "";
            string query = "select * from OESOIT where TQUCOD   ='ข\"'";

            DataTable datalist = GetDataFromFPQuery(query);
            foreach (DataRow row in datalist.Rows) {
                result = row["TQUCOD"].ToString();

            }
            return result;
        }
        public static void SampleComplexQuery() {
            string dateBegin = Convert2StringEngYearMMddyyyy(DateTime.Now.Date.AddDays(-180));
            string dateEnd = Convert2StringEngYearMMddyyyy(DateTime.Now.Date);
            DateTime startUserDate = new DateTime(2018, 1, 1);
            string dateStartUse = Convert2StringEngYearMMddyyyy(startUserDate);

            string query = "select stcrd.docnum,artrnrm.remark ,stcrd.stkcod,stcrd.stkdes,stcrd.netval, aptrn.docdat "
                + "from stcrd inner join aptrn on stcrd.docnum = aptrn.docnum "
                + "left join artrnrm on stcrd.docnum=artrnrm.docnum  "
                + "where stcrd.docnum like 'CC%' and artrnrm.seqnum = '@6' and NOT EMPTY(aptrn.APPROVE) "
                + "and aptrn.docstat <>'C' "
                + "and aptrn.docdat between {" + dateBegin + "} and {" + dateEnd + "} "
                + "and aptrn.docdat >=  {" + dateStartUse + "}";


            //+ "and aptrn.docdat between {12/26/2017" + "} and {"  + "12/26/2017}";
            DataTable datalist = GetDataFromFPQuery(query);

        }



        #region date convert
        public static string Convert2StringEngYearMMddyyyy(DateTime getDate) {
            string cyear = "";
            if (getDate.Year > 2500) {
                cyear = (getDate.Year - 543).ToString();
            } else {
                cyear = getDate.Year.ToString();
            }
            return getDate.Month.ToString() + "/" + getDate.Day.ToString() + "/" + cyear;
        }
        #endregion
    }
}
