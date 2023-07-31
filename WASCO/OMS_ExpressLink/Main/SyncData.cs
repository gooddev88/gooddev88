using DBF.Data.GADB;
using DBF.Model;
using DBF.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBF.Main {
    public partial class SyncData : Form {
        Stopwatch timer = new Stopwatch();
      
        public SyncData() {
            InitializeComponent();
            LoadData();
        }

        private void LoadData() {
            BindData();
        }
        private void BindData() {
            txtExpressDBPath.Text = Properties.Settings.Default.ExpressDBPath;

        }

   

        private void SyncData_Load(object sender, EventArgs e) {

        }

        private void btnSelectPath_Click(object sender, EventArgs e) {
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()) {
                    // Set the initial directory (optional)
                    folderBrowserDialog.SelectedPath = @"C:\";

                    // Show the folder browser dialog
                    DialogResult result = folderBrowserDialog.ShowDialog();

                    // Check if the user clicked the OK button
                    if (result == DialogResult.OK) {
                        // Get the selected folder path
                        string selectedFolderPath = folderBrowserDialog.SelectedPath;
                        Properties.Settings.Default.ExpressDBPath = selectedFolderPath;
                        Properties.Settings.Default.Save();
                        BindData();
                    }
                }
            }
        }   

        private void txtExpressDBPath_Leave(object sender, EventArgs e) {
            //C:\Users\tammon.y\Desktop\AUDIT


            if (Directory.Exists(txtExpressDBPath.Text.Trim())) {
                Properties.Settings.Default.ExpressDBPath = txtExpressDBPath.Text.Trim();
                Properties.Settings.Default.Save();
            } else {
                MessageBox.Show("Path not found.");
                return;

            }
        }

        private void btnSyncMaster_Click(object sender, EventArgs e) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };
           
            try {
                Cursor.Current = Cursors.WaitCursor;
                var r = ExpressService.UpdateLocation();
                //if (r.Result == "fail") {
                //    result.Result = "fail";
                //    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                //}
                //r = ExpressService.UpdateCust();
                //if (r.Result == "fail") {
                //    result.Result = "fail";
                //    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                //}
                //r = ExpressService.UpdateItem();
                //if (r.Result == "fail") {
                //    result.Result = "fail";
                //    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                //}
                r = ExpressService.UpdateUser();
                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                }

                if (result.Result == "fail") {
                    MessageBox.Show("Sync data error "+result.Message1);
                } else {
                    MessageBox.Show("Sync data successfull.");
                }
        
            } catch (Exception) {
                 
            } finally {
                Cursor.Current = Cursors.Default;
            }
    

        }

        private void btnSyncItem_Click(object sender, EventArgs e) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                Cursor.Current = Cursors.WaitCursor;
                timer.Start();
                var r = ExpressService.UpdateItem();
                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                } 
            } catch (Exception ex) {
                result.Result = "fail";
                result.Message1 = ex.Message;
            } finally {

                timer.Stop();
                if (result.Result == "fail") {
                    MessageBox.Show("Sync data error " + result.Message1);
                } else {
                    TimeSpan timeTaken = timer.Elapsed;
                    MessageBox.Show($"Sync data successfull. {timeTaken.ToString(@"m\:ss\.fff")} นาที");
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSyncCustomer_Click(object sender, EventArgs e) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                Cursor.Current = Cursors.WaitCursor;
                timer.Start();
                var r = ExpressService.UpdateCust();
                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                }
            } catch (Exception ex) {
                result.Result = "fail";
                result.Message1 = ex.Message;
            } finally {

                timer.Stop();
                if (result.Result == "fail") {
                    MessageBox.Show("Sync data error " + result.Message1);
                } else {
                    TimeSpan timeTaken = timer.Elapsed;
                    MessageBox.Show($"Sync data successfull. {timeTaken.ToString(@"m\:ss\.fff")} นาที");
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSyncUser_Click(object sender, EventArgs e) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                Cursor.Current = Cursors.WaitCursor;
                timer.Start();
                var r = ExpressService.UpdateUser();
                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                }
                r = ExpressService.UpdateLocation();
                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                }
            } catch (Exception ex) {
                result.Result = "fail";
                result.Message1 = ex.Message;
            } finally {

                timer.Stop();
                if (result.Result == "fail") {
                    MessageBox.Show("Sync data error " + result.Message1);
                } else {
                    TimeSpan timeTaken = timer.Elapsed;
                    MessageBox.Show($"Sync data successfull. {timeTaken.ToString(@"m\:ss\.fff")} นาที");
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSyncOrder_Click(object sender, EventArgs e) {
       

            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                Cursor.Current = Cursors.WaitCursor;
                timer.Start();
                //var r0 = ExpressService.PackDB("OESO");
                //var r1 = ExpressService.PackDB("OESOIT");
                var r = ExpressService.LinkSOToExpress();

                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                }

            } catch (Exception ex) {
                result.Result = "fail";
                result.Message1 = ex.Message;
            } finally {

                timer.Stop();
                if (result.Result == "fail") {
                    MessageBox.Show("Sync data error " + result.Message1);
                } else {
                    TimeSpan timeTaken = timer.Elapsed;
                    MessageBox.Show($"Sync data successfull. {timeTaken.ToString(@"m\:ss\.fff")} นาที");
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnResetStock_Click(object sender, EventArgs e) {
            I_BasicResult result = new I_BasicResult { Result = "ok", Message1 = "", Message2 = "" };

            try {
                Cursor.Current = Cursors.WaitCursor;
                timer.Start();
                var r = ExpressService.UpdateStock();
                if (r.Result == "fail") {
                    result.Result = "fail";
                    result.Message1 = result.Message1 + Environment.NewLine + r.Message1;
                }
               
            } catch (Exception ex) {
                result.Result = "fail";
                result.Message1 = ex.Message;
            } finally {

                timer.Stop();
                if (result.Result == "fail") {
                    MessageBox.Show("Sync data error " + result.Message1);
                } else {
                    TimeSpan timeTaken = timer.Elapsed;
                    MessageBox.Show($"Sync data successfull. {timeTaken.ToString(@"m\:ss\.fff")} นาที");
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
        //    var r = ExpressService.IsSOLock("SO0013554");
  ExpressService.DeleteSO("SO0013554");
        }

		private void btnX_Click(object sender, EventArgs e) {
            MessageBox.Show(ExpressService.TestConnectSQL().Message1);
		}
	}
}
