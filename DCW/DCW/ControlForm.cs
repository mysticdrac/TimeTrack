using System;
using System.Windows.Forms;

namespace DCW
{
    public partial class ControlForm : UserControl
    {
        MainFrm P;
        TimeTrackClass t;
        Project[] _project;
        internal JsonResp _response;
        public ControlForm(MainFrm p)
        {
            InitializeComponent();
            P = p;
            _response = P.resp;
            t = new TimeTrackClass(this);

        }


        private void ControlForm_MouseDown(object sender, MouseEventArgs e)
        {
            P.MainFrm_MouseDown(sender,e);
        }

      

        private void splitContainer1_MouseDown(object sender, MouseEventArgs e)
        {
            P.MainFrm_MouseDown(sender, e);

        }

        private void btn_capture_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbxproject.Text) || string.IsNullOrEmpty(cbbxtask.Text)) {

                MessageBox.Show("Please select Project /Task");
                return;
            }
            cbbxproject.Enabled = false;
            cbbxtask.Enabled = false;
            btn_capture.Enabled = false;
            t.Start(cbbxproject.Text,cbbxtask.Text);
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
            _project = P.setJson.Project;
            foreach(Project _p in _project)
            {
                cbbxproject.Items.Add(_p.name);
   
            }
        }

        private void cbbxproject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if it is start
            if (t._isStart) {
                cbbxproject.Enabled = false;
                cbbxtask.Enabled = false;
                //stop the process
                t.Start("", "");

            }
            lblhour.Text = "00:00:00";
            cbbxtask.Text = "";
            cbbxtask.Items.Clear();
            foreach (Project _p in _project) {
                if (cbbxproject.Text.Equals(_p.name)){
                    foreach (Task _t in _p.Task) {
                        cbbxtask.Items.Add(_t.name);
                    }
                }
            }
        }

        private void cbbxtask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (t._isStart)
            {
                cbbxproject.Enabled = false;
                cbbxtask.Enabled = false;
                //stop the process
                t.Start("", "");
            }
            if (string.IsNullOrEmpty(cbbxproject.Text)) {

                MessageBox.Show("Please select Project !");
                return;
            }
            //show the hours spent
            foreach(Project _p in _project)
            {
                if (cbbxproject.Text.Equals(_p.name))
                {
                    foreach (Task _t in _p.Task)
                    {
                        if (cbbxtask.Text.Equals(_t.name)) {
                            TimeSpan _totaltime = TimeSpan.FromSeconds(_t.time);
                            lblhour.Text = string.Format("{0:00}:{1:D2}:{2:D2}", Math.Floor(_totaltime.TotalHours), _totaltime.Minutes, _totaltime.Seconds);
                            break;
                        }
                    }
                }
                
            }
        }
    }
}
