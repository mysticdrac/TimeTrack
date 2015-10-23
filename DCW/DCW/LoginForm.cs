using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCW
{
    public partial class LoginForm : UserControl
    {
        MainFrm P;
        public LoginForm(MainFrm p)
        {
            InitializeComponent();
            P = p;
            txbx_user.GotFocus += textBox_GotFocus;
            txbx_pass.GotFocus += textBox_GotFocus;
            txbx_user.LostFocus += textBox_LostFocus;
            txbx_pass.LostFocus += textBox_LostFocus;
        }

        private void textBox_LostFocus(object sender, EventArgs e)
        {
            if (((TextBox)sender).Name.ToLower().Equals("txbx_user"))
            {
                if (this.txbx_user.Text.Trim().Equals(string.Empty))
                {
                    this.txbx_user.ForeColor = Color.FromArgb(231, 226, 226);
                    this.txbx_user.Text = "Username";
                }
            }
            if (((TextBox)sender).Name.ToLower().Equals("txbx_pass"))
            {
                if (this.txbx_pass.Text.Trim().Equals(string.Empty))
                {
                    //this.txbx_pass.UseSystemPasswordChar = false;
                    this.txbx_pass.ForeColor = Color.FromArgb(231, 226, 226);
                    this.txbx_pass.Text = "Password";


                }

            }


        }

        private void textBox_GotFocus(object sender, EventArgs e)
        {
            if (((TextBox)sender).Name.ToLower().Equals("txbx_user"))
            {
                if (this.txbx_user.Text.ToLower().Equals("username"))
                {
                    this.txbx_user.ForeColor = Color.FromArgb(0, 0, 0);
                    this.txbx_user.Text = string.Empty;
                }
            }
            if (((TextBox)sender).Name.ToLower().Equals("txbx_pass"))
            {
                if (this.txbx_pass.Text.ToLower().Equals("password"))
                {
                    //this.txbx_pass.UseSystemPasswordChar = true;
                    this.txbx_pass.ForeColor = Color.FromArgb(0, 0, 0);
                    this.txbx_pass.Text = string.Empty;
                }

            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.remember)
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Username) && !string.IsNullOrEmpty(Properties.Settings.Default.Password))
                {
                    txbx_user.Text = Properties.Settings.Default.Username;
                    txbx_pass.Text = Properties.Settings.Default.Password;
                }
                chkbx_remember.Checked = true;

            }
            else
            {
                this.txbx_user.ForeColor = Color.FromArgb(231, 226, 226);
                this.txbx_pass.ForeColor = Color.FromArgb(231, 226, 226);
            }
        }

        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            P.MainFrm_MouseDown(sender,e);

        }

       

        private void btn_login_Click(object sender, EventArgs e)
        {
            label1.Text = "Login to Server....";
            label1.Visible = true;
            BackgroundWorker _worker = new BackgroundWorker();
            _worker.DoWork += (s, es) => {
                object[] obj = (object[])es.Argument;
                TimeTrackClass cls = new TimeTrackClass();
                es.Result = cls.Login(obj);
                
            };

            _worker.RunWorkerCompleted += (s, es) => {
                object result = (object)es.Result;

                string text = "";
                if (result.GetType().Equals(typeof(int)))
                {
                    switch ((int)result)
                    {
                        case 1:
                            text = "Incorrect Username/Password/Server";
                            break;
                        case 2:
                            text = "Connection Problem";
                            break;
                    }
                }
                else if (result.GetType().Equals(typeof(JsonResp)))
                {

                    text = "logged in";

                }
                else {
                    text = "Server Error";

                }
                label1.Invoke((MethodInvoker)delegate
                {
                    label1.Visible = true;
                    label1.Text = text;
                },text);


                if (result.GetType().Equals(typeof(JsonResp)))
                {
                    P.setJson=(JsonResp)result;
                    P.ShowCtrl();
                }
            };
            _worker.RunWorkerAsync(new object[] { this.txbx_user.Text.Trim(), this.txbx_pass.Text.Trim(), Properties.Settings.Default.Server } );

        }

        

        private void chkbx_remember_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkbx_remember.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.remember = true;
                Properties.Settings.Default.Save();
            }
            else {
                Properties.Settings.Default.remember = false;
                Properties.Settings.Default.Save();

            }
        }

        private void txbx_pass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                if (!string.IsNullOrEmpty(txbx_user.Text.Trim()) && !string.IsNullOrEmpty(txbx_pass.Text.Trim()))
                {
                    btn_login.PerformClick();
                }
                else
                {
                    btn_login.Focus();

                }
            }
        }
    }
}
