using DBF.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBF {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        } 
        private void button1_Click(object sender, EventArgs e) {
            DBFService.Insert();
        }
        private void button2_Click(object sender, EventArgs e) {
            // ReadData();
            DBFService.Select();
        }
   
        private void button3_Click(object sender, EventArgs e) {
            DBFService.Delete();
        }

        private void btnExpress_Click(object sender, EventArgs e) {
            SyncData f = new SyncData();
            f.Show();
        }

        private void btnRead2_Click(object sender, EventArgs e) {
            DBFService.Select2();
        }
    }
}
