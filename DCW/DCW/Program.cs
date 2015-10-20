using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DCW
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CheckIni();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
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
