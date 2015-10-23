using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Threading;
namespace DCW
{
    class TimeTrackClass
    {
        #region Initialize variables
        internal bool _isStart = false;
        System.Timers.Timer TmrScr;
        System.Timers.Timer WorkTmr;
        ControlForm P;
        uint _timeUpload = 0;
        uint _worktick = 0;
        uint _tick = 0;
        string _user = "";
        string _pass = "";
        string newLine = Environment.NewLine;
        string boundary = "";
        string propFormat;
        string fileHeaderFormat;
        Project _CurProject;
        Task _CurTask;
        JsonResp _response;
        string[] _ERROR = new string[8]
        {
            "Collecting Report.....",
            "Reporting.....",
            "Error Collecting Report",
            "Error Contacting Server",
            "Error while Reporting !",
            "Work Reported ! ",
             "Problem adjust Timer",
             "Pause Work because Idle detected"
        };
        const string _referrer = "http://track.designcircuitworks.com/";
        uint maxidletime = Properties.Settings.Default.IdleTime * 1000; //idle max is 5 minutes
        TimeSpan _timetotalWork;
        #endregion

        #region Constructor and initialize of PropFormat and FileheaderFormat
        public TimeTrackClass()
        {

            propFormat = "--{0}" + newLine +
                            "Content-Disposition: form-data; name=\"{1}\"" + newLine + newLine +
                            "{2}" + newLine;
            fileHeaderFormat = "--{0}" + newLine +
                                     "Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"" + newLine + "Content-Type: {3}" + newLine + newLine;

        }

        #endregion

        #region Constructor with params
        public TimeTrackClass(ControlForm p) : this()
        {
            TmrScr = new System.Timers.Timer(1000);
            TmrScr.Elapsed += TmrScr_Elapsed;
            TmrScr.AutoReset = true;

            WorkTmr = new System.Timers.Timer(1000);
            WorkTmr.Elapsed += WorkTmr_Elapsed;
            WorkTmr.AutoReset = true;

            _timeUpload = Properties.Settings.Default.Interval;
            P = p;
            _response = P._response;



        }

        private void WorkTmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            WorkTmr.Stop();
            //save the current daily worktime every minute has changed
            if (_worktick % 60 == 0)
            {
                Properties.Settings.Default.WorkTime = _worktick;
                Properties.Settings.Default.Save();
            }
            //if already 4 hours, will pop up message box for fill in work summary
            if (_worktick % 14400 == 0) {
                //pop up the messagebox from parent

            }


            _worktick++;


            _timetotalWork = TimeSpan.FromSeconds(_worktick);
            P.lbltime.Invoke((MethodInvoker)delegate { P.lbltime.Text = string.Format("{0:00}:{1:D2}:{2:D2}", Math.Floor(_timetotalWork.TotalHours), _timetotalWork.Minutes, _timetotalWork.Seconds); }, _timetotalWork);
            WorkTmr.Start();
        }
        #endregion


        #region ScreenShot Elapsed action
        private void TmrScr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DetectIdle.GetIdleTime() > maxidletime)
            {
                TmrScr.Stop();
                StopBtn_Capture(_ERROR[7]);
                return;
            }
            _tick++;
            if (_tick == _timeUpload)
            {
                uploadScreenshot(_worktick,true);
                
            }
            /*detect idle time*/
            TimeSpan _totalhourtask = TimeSpan.FromSeconds(_tick);

            P.lblhour.Invoke((MethodInvoker)delegate { P.lblhour.Text = string.Format("{0:00}:{1:D2}:{2:D2}", Math.Floor(_totalhourtask.TotalHours), _totalhourtask.Minutes, _totalhourtask.Seconds); }, _totalhourtask);

        }
        #endregion

        #region Set Interval of Upload time
        internal uint SetInterval
        {
            get
            {
                return _timeUpload;

            }
            set
            {

                _timeUpload = value;
            }

        }
        #endregion

        #region Login Method
        internal object Login(object[] obj)
        {
            string Username = (string)obj[0];
            string Password = (string)obj[1];
            string Server = (string)obj[2];
            WorkProcess wrkproc = new WorkProcess(this);
            boundary = "------WebKitFormBoundary" + Helper.GetUniqueKey(16);

            Hashtable _hash = new Hashtable();
            _hash.Add("user", Username);
            _hash.Add("pass", Password);
            _hash.Add("task", "Login");
            _hash.Add("start", "start");

            string requestdata = "";
            foreach (DictionaryEntry pair in _hash)
            {

                requestdata += string.Format(propFormat, boundary, pair.Key, pair.Value);

            }
            //string url,string requestdata,string Method,bool _ismultipart,object[] param
            object result = wrkproc.AsyncRequest(Server, requestdata, "post", true, new object[] { _referrer, boundary });
            if (result != null)
            {
                try
                {
                   
                    _response = Deserialize<JsonResp>(result.ToString());
                   
                    if (!_response.status.ToLower().Equals("ok"))
                    {
                       
                        return 1;
                    }
                    else {
                        Properties.Settings.Default.Username = Username;
                        Properties.Settings.Default.Password = Password;
                        Properties.Settings.Default.Server = Server;
                        Properties.Settings.Default.Interval = _response.time;
                        Properties.Settings.Default.Save();
                        return _response;
                    }

                }
                catch {

                    return 2;

                }

            }
            else
            {

                return 2;
            }
            
        }
        #endregion

        private T Deserialize<T>(string json)
        {
            var instance = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }


        #region Collection of Action when Stop button pressed
        void StopBtn_Capture(string _text)
        {

            P.lblinfo.Invoke((MethodInvoker)delegate
            {
                P.lblinfo.Text = _text;

            });
            P.btn_capture.Invoke((MethodInvoker)delegate
            {

                P.btn_capture.PerformClick();
                P.btn_capture.FlatAppearance.BorderSize = 2;
                P.btn_capture.FlatAppearance.BorderColor = Color.Red;
            });
            
        }
        #endregion

        #region Start Button Pressed Action

        internal void Start(string _project,string _task)
        {
            if (P.btn_capture.FlatAppearance.BorderSize != 1)
            {
                P.btn_capture.Invoke((MethodInvoker)delegate
                {
                    P.btn_capture.FlatAppearance.BorderSize = 1;
                    P.btn_capture.FlatAppearance.BorderColor = Color.Black;


                });

            }
           

            if (!_isStart)
            {
                P.lblinfo.Invoke((MethodInvoker)delegate
                {
                    P.lblinfo.Text = "";
                });
                //Get the current _project and _task
                foreach (Project _proj in _response.Project) {
                    if (_proj.name.Equals(_project))
                    {     _CurProject = _proj;
                        foreach (Task _t in _proj.Task) {
                            if (_t.name.Equals(_task)) {
                                _CurTask = _t;
                                break;

                            }

                        }
                    }

                }
                if (_CurProject == null || _CurTask == null) {
                    MessageBox.Show("Task / Project not found");
                    P.cbbxproject.Invoke((MethodInvoker)delegate {
                        P.cbbxproject.Enabled = true;


                    });
                    P.cbbxtask.Invoke((MethodInvoker)delegate {
                        P.cbbxtask.Enabled = true;


                    });
                    P.btn_capture.Invoke((MethodInvoker)delegate {
                        P.btn_capture.Enabled = true;

                    });
                    return;
                }
                //set the start timer to latest task time
                _tick = _CurTask.time;


                //set the daily work timer to the last time it stops
                _worktick = Properties.Settings.Default.WorkTime;


                P.lbltime.Invoke((MethodInvoker)delegate {

                    P.lbltime.Text = TimeSpan.FromSeconds(_worktick).ToString();

                });

                Properties.Settings.Default.Project = _CurProject.name;
                Properties.Settings.Default.Task = _CurTask.name;
                Properties.Settings.Default.Save();
                SetInterval = Properties.Settings.Default.Interval;
                _user = Properties.Settings.Default.Username;
                _pass = Properties.Settings.Default.Password;
                System.ComponentModel.BackgroundWorker _worker = new System.ComponentModel.BackgroundWorker();
                _worker.DoWork += (s,es)=>{
                    uint t = (uint)es.Argument;
                    uploadScreenshot(t, true);


                };
                _worker.RunWorkerAsync(_worktick);
                WorkTmr.Start();
                TmrScr.Start();
                
                P.btn_capture.Invoke((MethodInvoker)delegate {
                    P.btn_capture.Enabled = true;
                    P.btn_capture.Text = "Stop Capture";


                });
                _isStart = true;
                P.cbbxproject.Invoke((MethodInvoker)delegate {
                    P.cbbxproject.Enabled = true;


                });
                P.cbbxtask.Invoke((MethodInvoker)delegate {
                    P.cbbxtask.Enabled = true;


                });
            

            }
            else
            {
                _task = "";
                _tick = 0;
                WorkTmr.Stop();
                TmrScr.Stop();
                System.ComponentModel.BackgroundWorker _worker = new System.ComponentModel.BackgroundWorker();
                _worker.DoWork += (s, es) => {
                    uint t = (uint)es.Argument;
                    uploadScreenshot(t, false);


                };
                _worker.RunWorkerAsync(_worktick);
                Properties.Settings.Default.WorkTime = _worktick;
                Properties.Settings.Default.Save();
                P.btn_capture.Invoke((MethodInvoker)delegate {
                    P.btn_capture.Enabled = true;
                    P.btn_capture.Text = "Start Capture";

                });
                _isStart = false;
                P.cbbxproject.Invoke((MethodInvoker)delegate {
                    P.cbbxproject.Enabled = true;


                });
                P.cbbxtask.Invoke((MethodInvoker)delegate {

                    P.cbbxtask.Enabled = true;

                });
            }

        }


        #endregion



        #region Taking Screenshot
        string screenShot()
        {
            try
            {
                P.lblinfo.Invoke((MethodInvoker)delegate
                {

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

                        _filename = Properties.Settings.Default.TmpFolder + "\\" + _user + "--" + DateTime.Now.ToString("dd-MM-yyyy") + "--" + Guid.NewGuid().ToString("N") + ".jpg";
                        bmpScreenshot.Save(_filename, ImageFormat.Jpeg);
                    }
                }
                return _filename;
            }
            catch (System.Exception ex)
            {
                ErrorLog._WriteLog(ex.Message);
                return null;

            }
        }
        #endregion

        #region Process upload Screenshot to Server
        void uploadScreenshot(uint _workticktemp,bool _start)
        {
            P.lblinfo.Invoke((MethodInvoker)delegate
            {
                P.lblinfo.Text = _ERROR[1];

            });

            boundary = "------WebKitFormBoundary" + Helper.GetUniqueKey(16);

            Hashtable _hash = new Hashtable();
            _hash.Add("user", _user);
            _hash.Add("pass", _pass);
            _hash.Add("task", Properties.Settings.Default.Task);
            if (_start)
            {
                _hash.Add("start", "start");
            }
            else {

                _hash.Add("stop", "stop");

            }

            string _filename = screenShot();
            if (_filename == null)
            {
                StopBtn_Capture(_ERROR[2]);
                return;
            }
            string requestdata = "";
            foreach (DictionaryEntry pair in _hash)
            {

                requestdata += string.Format(propFormat, boundary, pair.Key, pair.Value);

            }
            try
            {
                requestdata += string.Format(fileHeaderFormat, boundary, "picture", System.IO.Path.GetFileName(_filename), "image/" + ImageFormat.Jpeg.ToString());
            }
            catch { }
            WorkProcess wrkproc = new WorkProcess(this);

            //string url,string requestdata,string Method,bool _ismultipart,object[] param

            object result = wrkproc.AsyncRequest(Properties.Settings.Default.Server, requestdata, "post", true, new object[] { _referrer, boundary, _filename });
            if (result == null)
            {
                ErrorLog._WriteLog(_ERROR[3]);
                StopBtn_Capture(_ERROR[3]);


            }
            else
            {
                ScResp resp = Deserialize<ScResp>(result.ToString());
                if (!resp.status.ToLower().Equals("ok"))
                {

                    ErrorLog._WriteLog(_ERROR[4]);
                    StopBtn_Capture(_ERROR[4]);
                }
                else {
                    try
                    {
                        System.IO.File.Delete(_filename);

                    }
                    catch (System.Exception ex)
                    {
                        ErrorLog._WriteLog(ex.Message);

                    }

                    //if stop will get the totaltime for current task from server and update it to show to user
                    if (!_start)
                    {
                        foreach (Project _p in _response.Project)
                        {
                            if (_p.name.Equals(_CurProject.name))
                            {
                                foreach (Task _t in _p.Task)
                                {
                                    if (_t.name.Equals(_CurTask))
                                    {
                                        _t.time = resp.tottime;
                                    }
                                }
                            }
                        }


                    }
                    //set interval into time elapsed now + interval
                    SetInterval = _tick + resp.time;

                    //save interval incase error occured
                    Properties.Settings.Default.Interval = resp.time;
                    Properties.Settings.Default.Save();

                    //set label info to show status upload
                    P.lblinfo.Invoke((MethodInvoker)delegate
                    {
                        P.lblinfo.Text = _ERROR[5] + " at " + TimeSpan.FromSeconds(_workticktemp).ToString();

                    }, _ERROR[5]);


                }
            }
        }
        #endregion


        #region CheckUpdate Module
        internal string CheckUpdate()
        {
            System.Reflection.Assembly CurrentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(CurrentAssembly.Location);

            WorkProcess wrkproc = new WorkProcess(this);

            //params: string url,string requestdata,string Method,bool _ismultipart,object[] param
            object result = wrkproc.AsyncRequest(Properties.Settings.Default.UpdateServer, "", "get", false);
            if (result == null)
            {
                return result.ToString();

            }
            else
            {
                return null;

            }

        } 
        #endregion

    }
 }
    
