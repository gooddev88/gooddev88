namespace DBF {
    partial class Form1 {
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnExpress = new System.Windows.Forms.Button();
            this.btnRead2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(183, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(237, 57);
            this.button1.TabIndex = 0;
            this.button1.Text = "insert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(183, 190);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(237, 57);
            this.button2.TabIndex = 1;
            this.button2.Text = "read";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(183, 127);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(237, 57);
            this.button3.TabIndex = 2;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnExpress
            // 
            this.btnExpress.Location = new System.Drawing.Point(183, 321);
            this.btnExpress.Name = "btnExpress";
            this.btnExpress.Size = new System.Drawing.Size(237, 57);
            this.btnExpress.TabIndex = 3;
            this.btnExpress.Text = "Express";
            this.btnExpress.UseVisualStyleBackColor = true;
            this.btnExpress.Click += new System.EventHandler(this.btnExpress_Click);
            // 
            // btnRead2
            // 
            this.btnRead2.Location = new System.Drawing.Point(183, 253);
            this.btnRead2.Name = "btnRead2";
            this.btnRead2.Size = new System.Drawing.Size(237, 57);
            this.btnRead2.TabIndex = 4;
            this.btnRead2.Text = "read2";
            this.btnRead2.UseVisualStyleBackColor = true;
            this.btnRead2.Click += new System.EventHandler(this.btnRead2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 519);
            this.Controls.Add(this.btnRead2);
            this.Controls.Add(this.btnExpress);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnExpress;
        private System.Windows.Forms.Button btnRead2;
    }
}

