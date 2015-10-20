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
    public partial class ControlForm : UserControl
    {
        MainFrm P;
        TimeTrackClass t;
        public ControlForm(MainFrm p)
        {
            InitializeComponent();
            P = p;
            t = new TimeTrackClass(this);

        }

       

        private void chkbx_capture_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkbx_capture.CheckState == CheckState.Checked)
            {
                cbbx_capture.Enabled = true;
            }
            else {
                cbbx_capture.Enabled = false;
            }
        }

        private void ControlForm_MouseDown(object sender, MouseEventArgs e)
        {
            P.MainFrm_MouseDown(sender,e);
        }

        private void btn_capture_Click(object sender, EventArgs e)
        {
            t.Start(int.Parse(cbbx_capture.Text));
        }

        private void splitContainer1_MouseDown(object sender, MouseEventArgs e)
        {
            P.MainFrm_MouseDown(sender, e);

        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
            cbbx_capture.SelectedIndex = 0;

        }
    }
}
