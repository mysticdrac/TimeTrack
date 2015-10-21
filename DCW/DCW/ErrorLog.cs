using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DCW
{
    static class ErrorLog
    {
       internal static void _WriteLog(string text) {
            try
            {
                //Write Error Log to data/Log/{date and time Logging}.log
                File.AppendAllText(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/data/Log/" + DateTime.Now.ToString("dd-MM-yy") + ".log", DateTime.Now.ToString("HH:mm:ss")+"--"+text+"\n");
            }
            catch(System.Exception ex) {

            }

            

        }
    }
}
