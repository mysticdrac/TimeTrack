using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace DCW
{
    static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true,System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, out createdNew))
            {
                if (createdNew)
                {
                    CheckIni();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainFrm());
                }
                else
                {
                    Process current = Process.GetCurrentProcess();
                    foreach (Process process in Process.GetProcessesByName(current.ProcessName))
                    {
                        if (process.Id != current.Id)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            break;
                        }
                    }
                }
            }
        }

        static void CheckIni() {
            //load file
            string path = Application.StartupPath + "/data";
            string file = path + "/Timetrack.ini";

            if (!System.IO.File.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!System.IO.File.Exists(file))
            {

                System.IO.File.Create(file);
            }



        }
    }
}
