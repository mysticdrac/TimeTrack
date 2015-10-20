namespace DCW
{
    partial class ControlForm
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_capture = new System.Windows.Forms.Button();
            this.cbbx_capture = new System.Windows.Forms.ComboBox();
            this.chkbx_capture = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbltime = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 142);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 16);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer1.Size = new System.Drawing.Size(307, 123);
            this.splitContainer1.SplitterDistance = 84;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.splitContainer1_MouseDown);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btn_capture);
            this.groupBox2.Controls.Add(this.cbbx_capture);
            this.groupBox2.Controls.Add(this.chkbx_capture);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(307, 84);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Screenshot";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Minutes";
            // 
            // btn_capture
            // 
            this.btn_capture.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_capture.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_capture.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_capture.Location = new System.Drawing.Point(173, 16);
            this.btn_capture.Name = "btn_capture";
            this.btn_capture.Size = new System.Drawing.Size(131, 65);
            this.btn_capture.TabIndex = 2;
            this.btn_capture.Text = "Start Capture";
            this.btn_capture.UseVisualStyleBackColor = true;
            this.btn_capture.Click += new System.EventHandler(this.btn_capture_Click);
            // 
            // cbbx_capture
            // 
            this.cbbx_capture.Enabled = false;
            this.cbbx_capture.FormattingEnabled = true;
            this.cbbx_capture.Items.AddRange(new object[] {
            "1",
            "3",
            "5",
            "7",
            "9",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60"});
            this.cbbx_capture.Location = new System.Drawing.Point(35, 52);
            this.cbbx_capture.Name = "cbbx_capture";
            this.cbbx_capture.Size = new System.Drawing.Size(57, 21);
            this.cbbx_capture.TabIndex = 1;
            // 
            // chkbx_capture
            // 
            this.chkbx_capture.AutoSize = true;
            this.chkbx_capture.Location = new System.Drawing.Point(17, 29);
            this.chkbx_capture.Name = "chkbx_capture";
            this.chkbx_capture.Size = new System.Drawing.Size(88, 17);
            this.chkbx_capture.TabIndex = 0;
            this.chkbx_capture.Text = "Auto Capture";
            this.chkbx_capture.UseVisualStyleBackColor = true;
            this.chkbx_capture.CheckStateChanged += new System.EventHandler(this.chkbx_capture_CheckStateChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbltime);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(307, 35);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            // 
            // lbltime
            // 
            this.lbltime.AutoSize = true;
            this.lbltime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltime.Location = new System.Drawing.Point(230, 10);
            this.lbltime.Name = "lbltime";
            this.lbltime.Size = new System.Drawing.Size(71, 20);
            this.lbltime.TabIndex = 1;
            this.lbltime.Text = "00:00:00";
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.groupBox1);
            this.MaximumSize = new System.Drawing.Size(313, 142);
            this.MinimumSize = new System.Drawing.Size(313, 142);
            this.Name = "ControlForm";
            this.Size = new System.Drawing.Size(313, 142);
            this.Load += new System.EventHandler(this.ControlForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ControlForm_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbbx_capture;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btn_capture;
        internal System.Windows.Forms.Label lbltime;
        internal System.Windows.Forms.CheckBox chkbx_capture;
    }
}
