namespace DCW
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.label1 = new System.Windows.Forms.Label();
            this.chkbx_remember = new System.Windows.Forms.CheckBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.txbx_pass = new System.Windows.Forms.TextBox();
            this.txbx_user = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(5, 263);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 16);
            this.label1.TabIndex = 33;
            this.label1.Text = "Please Login";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // chkbx_remember
            // 
            this.chkbx_remember.AutoSize = true;
            this.chkbx_remember.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbx_remember.Location = new System.Drawing.Point(87, 229);
            this.chkbx_remember.Name = "chkbx_remember";
            this.chkbx_remember.Size = new System.Drawing.Size(133, 24);
            this.chkbx_remember.TabIndex = 30;
            this.chkbx_remember.Text = "Remember Me";
            this.chkbx_remember.UseVisualStyleBackColor = true;
            this.chkbx_remember.CheckStateChanged += new System.EventHandler(this.chkbx_remember_CheckStateChanged);
            // 
            // btn_login
            // 
            this.btn_login.BackColor = System.Drawing.SystemColors.Control;
            this.btn_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_login.Location = new System.Drawing.Point(97, 290);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(109, 43);
            this.btn_login.TabIndex = 31;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = false;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // txbx_pass
            // 
            this.txbx_pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbx_pass.Location = new System.Drawing.Point(29, 197);
            this.txbx_pass.Name = "txbx_pass";
            this.txbx_pass.Size = new System.Drawing.Size(251, 29);
            this.txbx_pass.TabIndex = 29;
            this.txbx_pass.Text = "Password";
            // 
            // txbx_user
            // 
            this.txbx_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbx_user.Location = new System.Drawing.Point(30, 154);
            this.txbx_user.Name = "txbx_user";
            this.txbx_user.Size = new System.Drawing.Size(251, 29);
            this.txbx_user.TabIndex = 28;
            this.txbx_user.Text = "Username";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(298, 136);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkbx_remember);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.txbx_pass);
            this.Controls.Add(this.txbx_user);
            this.Controls.Add(this.pictureBox1);
            this.Name = "LoginForm";
            this.Size = new System.Drawing.Size(313, 350);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginForm_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkbx_remember;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox txbx_pass;
        private System.Windows.Forms.TextBox txbx_user;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
