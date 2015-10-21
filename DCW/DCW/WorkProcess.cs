using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Threading;
using System.Diagnostics;
namespace DCW
{
    class WorkProcess
    {

        #region Initialize Variables
        ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 8192;
        bool aborted=false;
        int DefaultTimeout = 10000;
        WebAsyncReq.WebAsyncReq req;
        TimeTrackClass Pt;
        #endregion
        
        #region WorkProcess Constructor
        internal WorkProcess(TimeTrackClass parent)
        {
            Pt = parent;
        }
        #endregion

        #region Create new appdomain for sslrequest
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
            if (obj != null)
            {
                str = obj.ToString();
            }
            AppDomain.Unload(_domain);
            return str;
        }
        #endregion

        #region AsyncRequest method
        /*
           Method name: AsyncRequest
           Params 
           url : Link to post/get request
           requestdata : supplied data
           method : get/post
           _ismultipart : if is multipart
           param: additional array objects
       */

        internal object AsyncRequest(string url, string requestdata, string Method, bool _ismultipart, object[] param = null)
        {
            try
            {
                if (aborted)
                    throw new Exception();


                string _boundary = null;
                string _filename = null;
                string _referrer = "";

                //check if there is additional params
                if (param != null)
                {
                    if (param.Length > 0)
                    {
                        //referrer
                        _referrer = (string)param[0].ToString();

                        //if it is multipart , will need boundary
                        if (_ismultipart && param.Length >= 2)
                        {
                            _boundary = (string)param[1].ToString();

                        }

                        //if supply with _filename
                        if (param.Length >= 3)
                        {
                            _filename = (string)param[2].ToString();
                        }

                    }
                }


                //proxy set to null
                WebProxy proxy = null;

                //If uri is wrong, do nothing
                if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                {

                    return null;
                }
                req = new WebAsyncReq.WebAsyncReq();
                if (!string.IsNullOrEmpty(_filename))
                {
                    req.SetFilename = _filename;
                }
                req.SetBoundary = _boundary;

                req._IsMultipart = _ismultipart;
                req.SetSSL = false;
                req.SetTimeOut = DefaultTimeout;
                req.SetReferrer = _referrer;

                if (Method.ToLower().Equals("post"))
                {
                    req.SetPostData = requestdata;


                }


                object result = req.Request(url, proxy);
                if (result != null)
                {
                    return result;
                }
                else
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
        #endregion

        #region Abort Method

        internal void Abort()
        {
            aborted = true;
            if (req != null)
            {
                req.Abort();
            }


        } 
        #endregion

    }
}
