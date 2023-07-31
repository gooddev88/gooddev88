namespace DBF.Main {
    partial class SyncData {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncData));
            this.txtExpressDBPath = new System.Windows.Forms.TextBox();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnResetStock = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnX = new System.Windows.Forms.Button();
            this.btnSyncOrder = new System.Windows.Forms.Button();
            this.btnSyncUser = new System.Windows.Forms.Button();
            this.btnSyncCustomer = new System.Windows.Forms.Button();
            this.btnSyncItem = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtExpressDBPath
            // 
            this.txtExpressDBPath.Location = new System.Drawing.Point(24, 36);
            this.txtExpressDBPath.Name = "txtExpressDBPath";
            this.txtExpressDBPath.Size = new System.Drawing.Size(583, 40);
            this.txtExpressDBPath.TabIndex = 1;
            this.txtExpressDBPath.Leave += new System.EventHandler(this.txtExpressDBPath_Leave);
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSelectPath.Location = new System.Drawing.Point(613, 30);
            this.btnSelectPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(45, 46);
            this.btnSelectPath.TabIndex = 3;
            this.btnSelectPath.Text = "...";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtExpressDBPath);
            this.groupBox1.Controls.Add(this.btnSelectPath);
            this.groupBox1.Location = new System.Drawing.Point(22, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(693, 91);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Express DB Path";
            // 
            // btnResetStock
            // 
            this.btnResetStock.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnResetStock.Location = new System.Drawing.Point(409, 304);
            this.btnResetStock.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnResetStock.Name = "btnResetStock";
            this.btnResetStock.Size = new System.Drawing.Size(306, 66);
            this.btnResetStock.TabIndex = 9;
            this.btnResetStock.Text = "Reset Stock";
            this.btnResetStock.UseVisualStyleBackColor = true;
            this.btnResetStock.Click += new System.EventHandler(this.btnResetStock_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.button1.Location = new System.Drawing.Point(46, 304);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(178, 66);
            this.button1.TabIndex = 10;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnX
            // 
            this.btnX.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnX.Location = new System.Drawing.Point(239, 304);
            this.btnX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(153, 66);
            this.btnX.TabIndex = 11;
            this.btnX.Text = "X";
            this.btnX.UseVisualStyleBackColor = true;
            this.btnX.Visible = false;
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // btnSyncOrder
            // 
            this.btnSyncOrder.BackColor = System.Drawing.Color.Bisque;
            this.btnSyncOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSyncOrder.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSyncOrder.Image = global::DBF.Properties.Resources.send;
            this.btnSyncOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncOrder.Location = new System.Drawing.Point(46, 213);
            this.btnSyncOrder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSyncOrder.Name = "btnSyncOrder";
            this.btnSyncOrder.Size = new System.Drawing.Size(669, 83);
            this.btnSyncOrder.TabIndex = 8;
            this.btnSyncOrder.Text = "ส่งออเดอร์เข้า Express";
            this.btnSyncOrder.UseVisualStyleBackColor = false;
            this.btnSyncOrder.Click += new System.EventHandler(this.btnSyncOrder_Click);
            // 
            // btnSyncUser
            // 
            this.btnSyncUser.BackColor = System.Drawing.Color.CadetBlue;
            this.btnSyncUser.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSyncUser.Image = global::DBF.Properties.Resources.user_icon1;
            this.btnSyncUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncUser.Location = new System.Drawing.Point(482, 133);
            this.btnSyncUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSyncUser.Name = "btnSyncUser";
            this.btnSyncUser.Size = new System.Drawing.Size(233, 63);
            this.btnSyncUser.TabIndex = 7;
            this.btnSyncUser.Text = "ดึงเซลส์แมน";
            this.btnSyncUser.UseVisualStyleBackColor = false;
            this.btnSyncUser.Click += new System.EventHandler(this.btnSyncUser_Click);
            // 
            // btnSyncCustomer
            // 
            this.btnSyncCustomer.BackColor = System.Drawing.Color.CadetBlue;
            this.btnSyncCustomer.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSyncCustomer.Image = global::DBF.Properties.Resources.customer;
            this.btnSyncCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncCustomer.Location = new System.Drawing.Point(281, 133);
            this.btnSyncCustomer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSyncCustomer.Name = "btnSyncCustomer";
            this.btnSyncCustomer.Size = new System.Drawing.Size(195, 63);
            this.btnSyncCustomer.TabIndex = 6;
            this.btnSyncCustomer.Text = "ดึงลูกค้า";
            this.btnSyncCustomer.UseVisualStyleBackColor = false;
            this.btnSyncCustomer.Click += new System.EventHandler(this.btnSyncCustomer_Click);
            // 
            // btnSyncItem
            // 
            this.btnSyncItem.BackColor = System.Drawing.Color.CadetBlue;
            this.btnSyncItem.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnSyncItem.Image = global::DBF.Properties.Resources.product;
            this.btnSyncItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSyncItem.Location = new System.Drawing.Point(46, 133);
            this.btnSyncItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSyncItem.Name = "btnSyncItem";
            this.btnSyncItem.Size = new System.Drawing.Size(229, 63);
            this.btnSyncItem.TabIndex = 5;
            this.btnSyncItem.Text = "ดึงสินค้า";
            this.btnSyncItem.UseVisualStyleBackColor = false;
            this.btnSyncItem.Click += new System.EventHandler(this.btnSyncItem_Click);
            // 
            // SyncData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(750, 381);
            this.Controls.Add(this.btnX);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnResetStock);
            this.Controls.Add(this.btnSyncOrder);
            this.Controls.Add(this.btnSyncUser);
            this.Controls.Add(this.btnSyncCustomer);
            this.Controls.Add(this.btnSyncItem);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SyncData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sync Express";
            this.Load += new System.EventHandler(this.SyncData_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtExpressDBPath;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSyncItem;
        private System.Windows.Forms.Button btnSyncCustomer;
        private System.Windows.Forms.Button btnSyncUser;
        private System.Windows.Forms.Button btnSyncOrder;
        private System.Windows.Forms.Button btnResetStock;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnX;
	}
}

