using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
namespace DCW
{
    class TimeTrackClass
    {
        bool _isStart = false;
        System.Timers.Timer TmrScr;
        ControlForm P;
        int _timeUpload = 0;
        int _tick = 0;
        public TimeTrackClass() {


        }

        public TimeTrackClass(ControlForm p)
        {
            TmrScr = new System.Timers.Timer(1000);
            TmrScr.Elapsed += TmrScr_Elapsed;
            TmrScr.AutoReset = true;
            
            P = p;
            

        }

        private void TmrScr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _tick++;
            if (_tick %_timeUpload ==0)
            {
                uploadScreenshot();
            }
            DateTime tm = new DateTime(_tick * 1000);
            P.lbltime.Invoke((MethodInvoker)delegate { P.lbltime.Text = TimeSpan.FromSeconds(_tick).ToString(); });

        }

        internal int SetInterval {
            get {
                return _timeUpload;

            }
            set {

                _timeUpload = value * 60;
            }

        }

        internal int Login(object[] obj)
        {
            string Username = (string)obj[0];
            string Password = (string)obj[1];
            string Server = (string)obj[2];
            WorkProcess wrkproc = new WorkProcess(this);
            string requestdata = "username=" + Username + "&passwd=" + Password;
            object result = wrkproc.AsyncRequest(new object[] { Server, requestdata, "post" });
            if (result != null)
            {
              
                if (result.ToString().ToLower().IndexOf("received") > -1)
                {

                    Properties.Settings.Default.Username = Username;
                    Properties.Settings.Default.Password = Password;
                    Properties.Settings.Default.Server = Server;
                    Properties.Settings.Default.Save();
                    return 0;

                }
                else
                {
                    return 1;
                  
                }

            }
            else
            {

                return 2;
            }
        }



        internal void Start(int _interval) {
        
            if (!_isStart)
            {
                if (P.chkbx_capture.CheckState != CheckState.Checked) {
                    MessageBox.Show("Please check the auto capture");
                    return;
                }
                SetInterval = _interval;
                TmrScr.Start();
                P.btn_capture.Invoke((MethodInvoker)delegate { P.btn_capture.Text = "Stop Capture"; });
                _isStart = true;
            }
            else
            {
                TmrScr.Stop();
                P.btn_capture.Invoke((MethodInvoker)delegate { P.btn_capture.Text = "Start Capture"; });
                _isStart = false;
            }

        }

        string screenShot()
        {
            try {
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);

                string _filename = Application.StartupPath + "/data/tmp/" +Properties.Settings.Default.Username+"--"+DateTime.Now.ToString("dd-MM-yyyy")+"--"+ Guid.NewGuid().ToString("N") + ".jpg";
                bmpScreenshot.Save(_filename, ImageFormat.Jpeg);
                return _filename;
            }catch(System.Exception ex)
            {
                ErrorLog._WriteLog(ex.Message);
                return null;

            }
        }

        void uploadScreenshot()
        {
            Hashtable _hash = new Hashtable();
            _hash.Add("user", Properties.Settings.Default.Username);
            _hash.Add("pass", Properties.Settings.Default.Password);
            if (_isStart)
            {
                _hash.Add("start", "start");
            }
            else
            {
                _hash.Add("stop", "stop");
            }
            string _filename = screenShot();
            if (_filename == null) {

                return;
            }
            long ticks = DateTime.Now.Ticks;
            var boundary = "------WebKitFormBoundary" + Helper.GetUniqueKey(16);

            var newLine = Environment.NewLine;
            var propFormat = "--" + boundary + newLine +
                                "Content-Disposition: form-data; name=\"{0}\"" + newLine + newLine +
                                "{1}" + newLine;
            var fileHeaderFormat = "--" + boundary + newLine +
                                    "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" + newLine + "Content-Type: {2}" + newLine + newLine;

            string requestdata = "";
            foreach (DictionaryEntry pair in _hash)
            {

                requestdata += string.Format(propFormat, pair.Key, pair.Value);

            }

            requestdata += string.Format(fileHeaderFormat, "picture", System.IO.Path.GetFileName(_filename), "image/" + ImageFormat.Jpeg.ToString());
            WorkProcess wrkproc = new WorkProcess(this);


            object result = wrkproc.AsyncRequest(new object[] { Properties.Settings.Default.Server, requestdata, "post", boundary, _filename });
            if (result != null)
            {
                if (result.ToString().ToLower().IndexOf("received")>-1)
                {
                    try
                    {
                        System.IO.File.Delete(_filename);
                    }
                    catch(System.Exception ex) {
                        ErrorLog._WriteLog(ex.Message);

                    }
                }
                else
                {
                    ErrorLog._WriteLog("Error while uploading screenshot");
                }


            }
            else
            {
                ErrorLog._WriteLog("Server Response Null");
                

            }



        }


        internal string CheckUpdate()
        {
            System.Reflection.Assembly CurrentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(CurrentAssembly.Location);

            WorkProcess wrkproc = new WorkProcess(this);
            object result = wrkproc.AsyncRequest(new object[] { Properties.Settings.Default.UpdateServer, "", "get" });
            if (result == null)
            {
                return result.ToString();

            }
            else
            {
                return null;

            }

        }

    }
 }
    
//}
