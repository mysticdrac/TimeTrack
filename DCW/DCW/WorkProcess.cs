﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Globalization;
namespace DCW
{
    class WorkProcess
    {

        #region Variables
        ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 8192;
        int Identifier;
        bool login=false;
        int Index;
        bool aborted=false;
        string urlproxy;
        int DefaultTimeout = 10000;
        WebAsyncReq.WebAsyncReq req;
        TimeTrackClass Pt;
        #endregion
        
        #region CONSTRUCTORDESTRUCTOR
        internal WorkProcess(TimeTrackClass parent)
        {
            Pt = parent;
        }
        #endregion     

        string SslRequest(string url, WebProxy proxy)
        {
            AppDomain _domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());

            WebAsyncReq.WebAsyncReq boundary = (WebAsyncReq.WebAsyncReq)
                          _domain.CreateInstanceAndUnwrap(
                             typeof(WebAsyncReq.WebAsyncReq).Assembly.FullName,
                             typeof(WebAsyncReq.WebAsyncReq).FullName);
            boundary.SetSSL = true;
            boundary.SetTimeOut = DefaultTimeout;
            object obj = boundary.Request(url, proxy);
            string str = null;
            if(obj != null)
            {
                str = obj.ToString();
            }
            AppDomain.Unload(_domain);
            return str;
        }

        internal object AsyncRequest(object obj) {
                try
                {
                    if (aborted)
                        throw new Exception();

                    object[] context = obj as object[];
                    string url = context[0] as string;
                    string requestdata = (string)context[1];
                    string Method=(string)context[2];
                    string _boundary = "";
                    string _filename = "";
                    if (context.Length > 3)
                     {
                        _boundary = (string)context[3].ToString();
                        _filename = (string)context[4];

                    }


                    WebProxy proxy = null;                

                   
                    if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute)) {
                   
                            return null;                    
                    }
                    req = new WebAsyncReq.WebAsyncReq();
                    if (!string.IsNullOrEmpty(_boundary) && !string.IsNullOrEmpty(_filename)) {
                        req.SetBoundary = _boundary;
                        req.SetFilename = _filename;
                        req._IsMultipart = true;
                    }


                    req.SetSSL = false;
                    req.SetTimeOut = DefaultTimeout;
                    if(Method.ToLower().Equals("post")){
                    req.SetPostData=requestdata;
                    
                    }
                    
                    
                    object result = req.Request(url,proxy);
                    if (result != null) {
                        return result;
                     }else
                    {
                        if (aborted)
                            throw new Exception();

                        return null;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("Thread Aborted");
                    return null;
                } 
           

        }
          
        internal void Abort()
        {
            aborted = true;
            if (req != null) {
                req.Abort();
            }


        }

    }
}
