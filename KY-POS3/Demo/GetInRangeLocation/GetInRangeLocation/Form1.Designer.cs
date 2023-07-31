namespace GetInRangeLocation {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            button1 = new Button();
            textBox1 = new TextBox();
            txtLat = new TextBox();
            lbllat = new Label();
            label1 = new Label();
            txtLon = new TextBox();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(417, 227);
            button1.Name = "button1";
            button1.Size = new Size(150, 46);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(143, 227);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 39);
            textBox1.TabIndex = 1;
            // 
            // txtLat
            // 
            txtLat.Location = new Point(155, 41);
            txtLat.Name = "txtLat";
            txtLat.Size = new Size(200, 39);
            txtLat.TabIndex = 2;
            // 
            // lbllat
            // 
            lbllat.AutoSize = true;
            lbllat.Location = new Point(57, 44);
            lbllat.Name = "lbllat";
            lbllat.Size = new Size(40, 32);
            lbllat.TabIndex = 3;
            lbllat.Text = "lat";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(57, 101);
            label1.Name = "label1";
            label1.Size = new Size(46, 32);
            label1.TabIndex = 5;
            label1.Text = "lan";
            // 
            // txtLon
            // 
            txtLon.Location = new Point(155, 98);
            txtLon.Name = "txtLon";
            txtLon.Size = new Size(200, 39);
            txtLon.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 234);
            label2.Name = "label2";
            label2.Size = new Size(102, 32);
            label2.TabIndex = 6;
            label2.Text = "distance";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(33, 160);
            label3.Name = "label3";
            label3.Size = new Size(618, 32);
            label3.TabIndex = 7;
            label3.Text = "distance ตั้งต้น 13.707706330254576, 100.3759570576719";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtLon);
            Controls.Add(lbllat);
            Controls.Add(txtLat);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private TextBox txtLat;
        private Label lbllat;
        private Label label1;
        private TextBox txtLon;
        private Label label2;
        private Label label3;
    }
}