﻿using System;
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
        UInt32 _timeUpload = 0;
        UInt32 _tick = 0;
        string _task = "";
        string _user = "";
        string _pass = "";
        string newLine = Environment.NewLine;
        string boundary = "";
        string propFormat;
        string fileHeaderFormat;
        string[] _ERROR = new string[7] 
        {
            "Collecting Report.....",
            "Reporting.....",
            "Error Collecting Report",
            "Error Contacting Server",
            "Error while Reporting !",
            "Work Reported ! ",
             "Problem adjust Timer"
        };
        const string _referrer = "http://track.designcircuitworks.com/"; 
        public TimeTrackClass() {

            propFormat = "--{0}" + newLine +
                            "Content-Disposition: form-data; name=\"{1}\"" + newLine + newLine +
                            "{2}" + newLine;
            fileHeaderFormat = "--{0}" + newLine +
                                     "Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"" + newLine + "Content-Type: {3}" + newLine + newLine;

        }

        public TimeTrackClass(ControlForm p):this()
        {
            TmrScr = new System.Timers.Timer(1000);
            TmrScr.Elapsed += TmrScr_Elapsed;
            TmrScr.AutoReset = true;
            _timeUpload = Properties.Settings.Default.Interval;
            P = p;
            

        }

        private void TmrScr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _tick++;
            if (_tick ==_timeUpload)
            {
                uploadScreenshot();
            }
            DateTime tm = new DateTime(_tick * 1000);
            P.lbltime.Invoke((MethodInvoker)delegate { P.lbltime.Text = TimeSpan.FromSeconds(_tick).ToString(); });

        }

        internal UInt32 SetInterval {
            get {
                return _timeUpload;

            }
            set {

                _timeUpload = value;
            }

        }

        internal int Login(object[] obj)
        {
            string Username = (string)obj[0];
            string Password = (string)obj[1];
            string Server = (string)obj[2];
            WorkProcess wrkproc = new WorkProcess(this);
            boundary = "------WebKitFormBoundary" + Helper.GetUniqueKey(16);
           
            Hashtable _hash = new Hashtable();
            _hash.Add("user", Username);
            _hash.Add("pass", Password);
            _hash.Add("task","Login");
            _hash.Add("start", "start");

            string requestdata = "";
            foreach (DictionaryEntry pair in _hash)
            {

                requestdata += string.Format(propFormat,boundary, pair.Key, pair.Value);

            }
            //string url,string requestdata,string Method,bool _ismultipart,object[] param
            object result = wrkproc.AsyncRequest(Server,requestdata,"post",true,new object[] { _referrer,boundary});
            if (result != null)
            {
              
                if (result.ToString().ToLower().IndexOf("received") > -1)
                {


                    Properties.Settings.Default.Username = Username;
                    Properties.Settings.Default.Password = Password;
                    Properties.Settings.Default.Server = Server;
                    Properties.Settings.Default.Save();
                    try
                    {
                        string[] _resarr = result.ToString().Split(new string[] { ":" }, StringSplitOptions.None);
                        if (_resarr.Length > 0)
                        {


                            try
                            {
                                Properties.Settings.Default.Interval = UInt32.Parse(_resarr[1].ToString());
                                Properties.Settings.Default.Save();
                               
                           

                            }
                            catch
                            {
                               

                            }
                        }
                    }
                    catch { }

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

        void StopBtn_Capture(string _text) {

            P.lblinfo.Invoke((MethodInvoker)delegate {
                P.lblinfo.Text = _text;

            });
            P.btn_capture.Invoke((MethodInvoker)delegate {

                P.btn_capture.PerformClick();
                P.btn_capture.FlatAppearance.BorderSize = 2;
                P.btn_capture.FlatAppearance.BorderColor = Color.Red;
            });

        }

        internal void Start(string task) {
            if (P.btn_capture.FlatAppearance.BorderSize != 1) {
                P.btn_capture.Invoke((MethodInvoker)delegate {
                    P.btn_capture.FlatAppearance.BorderSize = 1;
                    P.btn_capture.FlatAppearance.BorderColor = Color.Black;


                });

            }


            if (!_isStart)
            {
                P.lblinfo.Invoke((MethodInvoker)delegate {
                    P.lblinfo.Text = "";
                });

                Properties.Settings.Default.Task = task;
                Properties.Settings.Default.Save();
                SetInterval = Properties.Settings.Default.Interval;
                _user = Properties.Settings.Default.Username;
                _pass = Properties.Settings.Default.Password;
                TmrScr.Start();
                P.btn_capture.Invoke((MethodInvoker)delegate { P.btn_capture.Text = "Stop Capture"; });
                _isStart = true;
            }
            else
            {
                _task = "";
                _tick = 0;
                
                TmrScr.Stop();
                P.btn_capture.Invoke((MethodInvoker)delegate { P.btn_capture.Text = "Start Capture"; });
                _isStart = false;
            }

        }

        string screenShot()
        {
            try {
                P.lblinfo.Invoke((MethodInvoker)delegate {

                    P.lblinfo.Text = _ERROR[0];
                });
                string _filename = null;
                using (var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb))
                {

                    using (var gfxScreenshot = Graphics.FromImage(bmpScreenshot))
                    {


                        gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                                    Screen.PrimaryScreen.Bounds.Y,
                                                    0,
                                                    0,
                                                    Screen.PrimaryScreen.Bounds.Size,
                                                    CopyPixelOperation.SourceCopy);

                        _filename = Properties.Settings.Default.TmpFolder+"\\" + _user + "--" + DateTime.Now.ToString("dd-MM-yyyy") + "--" + Guid.NewGuid().ToString("N") + ".jpg";
                        bmpScreenshot.Save(_filename, ImageFormat.Jpeg);
                    }
                }
                return _filename;
            }catch(System.Exception ex)
            {
                ErrorLog._WriteLog(ex.Message);
                return null;

            }
        }

        void uploadScreenshot()
        {
            P.lblinfo.Invoke((MethodInvoker)delegate {
                P.lblinfo.Text = _ERROR[1];

            });

            boundary = "------WebKitFormBoundary" + Helper.GetUniqueKey(16);

            Hashtable _hash = new Hashtable();
            _hash.Add("user", _user);
            _hash.Add("pass", _pass);
            _hash.Add("task", _task);
            _hash.Add("start", "start");
            
           
            string _filename = screenShot();
            if (_filename == null) {
                StopBtn_Capture(_ERROR[2]);
                return;
            }
            string requestdata = "";
            foreach (DictionaryEntry pair in _hash)
            {

                requestdata += string.Format(propFormat,boundary, pair.Key, pair.Value);

            }
            try
            {
                requestdata += string.Format(fileHeaderFormat, boundary,"picture", System.IO.Path.GetFileName(_filename), "image/" + ImageFormat.Jpeg.ToString());
            }
            catch { }
            WorkProcess wrkproc = new WorkProcess(this);

            //string url,string requestdata,string Method,bool _ismultipart,object[] param

            object result = wrkproc.AsyncRequest(Properties.Settings.Default.Server,requestdata,"post",true, new object[] { _referrer, boundary, _filename });
            if (result == null)
            {
                ErrorLog._WriteLog(_ERROR[3]);
                StopBtn_Capture(_ERROR[3]);


            }
            else {
                if (result.ToString().ToLower().IndexOf("received") < 0)
                {
                    ErrorLog._WriteLog(_ERROR[4]);
                    StopBtn_Capture(_ERROR[4]);
                }
                else { 
                    try
                    {
                        System.IO.File.Delete(_filename);
                       
                    }
                    catch(System.Exception ex) {
                        ErrorLog._WriteLog(ex.Message);
                   
                    }

                    string[] _resarr = result.ToString().Split(new string[] { ":"},StringSplitOptions.None);
                    if (_resarr.Length > 0) {


                        try
                        {
                            //Parsing the response and split it to get the second string (time interval)
                            UInt32 interval = UInt32.Parse(_resarr[1].ToString());

                            //set interval into time elapsed now + interval
                            SetInterval = _tick + interval;

                            //save interval incase error occured
                            Properties.Settings.Default.Interval = interval;
                            Properties.Settings.Default.Save();

                            
                            P.lblinfo.Invoke((MethodInvoker)delegate {
                                P.lblinfo.Text = _ERROR[5];

                            },_ERROR[5]);

                        }
                        catch {
                            ErrorLog._WriteLog(_ERROR[6]);
                            StopBtn_Capture(_ERROR[6]);

                        }
                    }
                }
            }
        }


        internal string CheckUpdate()
        {
            System.Reflection.Assembly CurrentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(CurrentAssembly.Location);

            WorkProcess wrkproc = new WorkProcess(this);

            //params: string url,string requestdata,string Method,bool _ismultipart,object[] param
            object result = wrkproc.AsyncRequest(Properties.Settings.Default.UpdateServer,"","get",false);
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
