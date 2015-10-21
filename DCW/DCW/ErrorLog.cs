using System;
using System.IO;

namespace DCW
{
    static class ErrorLog
    {
        #region Write Error Log
        internal static void _WriteLog(string text)
        {
            try
            {
                //Write Error Log to data/Log/{date and time Logging}.log
                File.AppendAllText(Properties.Settings.Default.LogFolder + "\\" + DateTime.Now.ToString("dd-MM-yy") + ".log", DateTime.Now.ToString("HH:mm:ss") + "--" + text + "\n");
            }
            catch (System.Exception ex)
            {

            }



        } 
        #endregion
    }
}
