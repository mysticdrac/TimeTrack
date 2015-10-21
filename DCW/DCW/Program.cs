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
        #region Pinvoke SetForgroundWindow
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion
        /// <summary>
        #region Main Entry Point For Application
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew = true;
            /* check for current assembly exits using mutex 
             * (Refs: https://msdn.microsoft.com/en-us/library/system.threading.mutex(v=vs.110).aspx)
             *
             */
            using (Mutex mutex = new Mutex(true, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, out createdNew))
            {
                /*
                if not Application Exists, Mutex will return true
                 */
                if (createdNew)
                {
                    CheckIni();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainFrm());
                }
                else 
                {
                    /*
                     * if Application Exists, select the process and set to foreground
                     */

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
        #endregion

        #region CheckIni --Check if Required Files / Folders Exists. if Not---Create it
        static void CheckIni()
        {
            string[] paths = new string[3];
            paths[0] = Application.StartupPath + "/data";
            paths[1] = paths[0] + "/Log";
            paths[2] = paths[0] + "/tmp";

            string file = paths[0] + "/Timetrack.ini";

            foreach (string path in paths)
            {
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
            }
            if (!System.IO.File.Exists(file))
            {

                System.IO.File.Create(file);
            }

            


        } 
        #endregion
    }
}
