using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace DCW
{
    public partial class MainFrm : Form
    {
        //detect mouse button down for using 
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
           
            DialogResult d = MessageBox.Show("Are you sure You want to quit ?", "Confirmation", MessageBoxButtons.YesNoCancel);
            if (d == DialogResult.Cancel || d == DialogResult.No)
            {
                return;
            }
            else if (d == DialogResult.Yes)
            {

                Application.Exit();
            }
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        
        private void MainFrm_Load(object sender, EventArgs e)
        {
            CenterToScreen();           
            LoadCred(Application.StartupPath+"/data/Timetrack.ini");
            LoginForm frm = new LoginForm(this);
            groupBox2.Controls.Add(frm);
            frm.Dock = DockStyle.Fill;
        }

        void LoadCred(string _filepath)
        {
            string[] _credtemp = System.IO.File.ReadAllLines(_filepath);//.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string _str in _credtemp)
            {
                string[] _strarr = _str.Split(new string[] { "=" }, StringSplitOptions.None);
                if (_strarr.Length == 2)
                {
                    switch (_strarr[0].ToLower().Trim())
                    {
                        case "server":
                            Properties.Settings.Default.Server = _strarr[1].Trim();
                            break;
                        case "username":
                            Properties.Settings.Default.Username = _strarr[1].Trim();
                            break;
                        case "password":
                            Properties.Settings.Default.Password = _strarr[1].Trim();
                            break;

                    }

                }
            }
            Properties.Settings.Default.Save();
            

        }

        void CheckUpdate() {
            BackgroundWorker _worker = new BackgroundWorker();
            _worker.DoWork += (s,es)=> {
                TimeTrackClass t = new TimeTrackClass();
                es.Result = t.CheckUpdate();


            };

            _worker.RunWorkerCompleted += (s, es) => {
                //(string)es.Result;

            };       

        }

        

        private void MainFrm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                ShowInTaskbar = false;
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        internal void MainFrm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION,0);
            }
        
        }

       
        public void ShowCtrl()
        {
            BeginInvoke((MethodInvoker)delegate {
                ControlForm Userctrl = new ControlForm(this);
                Height = Userctrl.Height + 60;
                Width = Userctrl.Width+20;

                Rectangle workingArea = Screen.GetWorkingArea(this);
                Location = new Point(workingArea.Right - Size.Width - 20,
                                          workingArea.Bottom - Size.Height);


                Userctrl.Dock = DockStyle.Fill;
                groupBox2.Controls.Clear();              
                
                groupBox2.Controls.Add(Userctrl);

                notifyIcon1.Text = "Logged in";
                notifyIcon1.BalloonTipText = "Logged in";
                notifyIcon1.ShowBalloonTip(500);

                Focus();
            });
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.remember) {
                if (!String.IsNullOrEmpty(Properties.Settings.Default.Username) && !String.IsNullOrEmpty(Properties.Settings.Default.Password) && !String.IsNullOrEmpty(Properties.Settings.Default.Server)) {
                    string _writestr = "Server ="+Properties.Settings.Default.Server+ Environment.NewLine;
                    _writestr += "Username =" + Properties.Settings.Default.Username + Environment.NewLine;
                    _writestr += "Password =" + Properties.Settings.Default.Password + Environment.NewLine;

                    System.IO.File.WriteAllText(Application.StartupPath+"/data/Timetrack.ini",_writestr);
                    Properties.Settings.Default.Reset();
                    Properties.Settings.Default.Save();
                }

            }
        }

        private void splitContainer1_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            MainFrm_MouseDown(sender, e);

        }
    }
}
