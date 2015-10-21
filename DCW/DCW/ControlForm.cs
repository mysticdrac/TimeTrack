﻿using System;
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
            t.Start(txbx_task.Text.Trim());
        }
    }
}
