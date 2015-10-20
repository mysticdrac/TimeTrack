using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
//using System.Security.Authentication;
namespace WebAsyncReq
{
    public class WebAsyncReq : MarshalByRefObject
    {
        ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 8192;
        bool aborted = false;
        int DefaultTimeout = 30000;
        bool SSL = false;
        CookieContainer Cookies;
        RequestState rstop;
        string _Postdata;
        bool _IsXMLRequest = false;
        bool _redirect = true;
        bool _byte = false;
        bool _ismultipart = false;
        string _boundary = "";
        string _filename = "";
        public bool SetSSL {

            set {
                SSL = value;
            }
        }
        public bool Getbyte
        {

            set
            {
                _byte = value;
            }
        }

        public bool _IsMultipart {
            set {

                _ismultipart = value;
            }

        }
        public string SetFilename {
            set {

                _filename = value;
            }

        }
        public CookieContainer SetCookies {
            set {
                Cookies = value;
            }
        
        }

        public int SetTimeOut {
            set {
                DefaultTimeout = value;
            
            }
        
        }
        public string SetPostData {
            set {
                _Postdata = value;
            }
        
        }

        public WebHeaderCollection GetResponseHeader {
            get {

                return rstop.Response.Headers; 
            }
        
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (error == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

          return false;
        }

        public bool IsXmlRequest {

            set {

                _IsXMLRequest = value;
            }
        }

        public string SetBoundary {
            set {

                _boundary = value;
            }

        }

        void Initialize(ref HttpWebRequest request) {
            if (SSL)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;

            }
            else
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }

            if (Cookies != null)
            {
                request.CookieContainer = Cookies;
                //request.Headers.Add("Cookie", Cookies);
            }
            if (!_redirect) {
                request.AllowAutoRedirect = false;
            }
            bool post = false;
            
            if (!string.IsNullOrEmpty(_Postdata))
            {
                if (_ismultipart)
                {


                    request.ContentType = "multipart/form-data; boundary=" + _boundary;
                    string footer = "\r\n--" + _boundary + "--\r\n";

                    request.ContentLength = _Postdata.Length + File.ReadAllBytes(_filename).Length+footer.Length;

                }
                else
                {

                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = _Postdata.Length;

                }
                request.Method = "POST";
                post = true;
            }
            if (_IsXMLRequest) {
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                
            }
           
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*,;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Dragon/33.1.0.0 Chrome/33.0.1750.152 Safari/537.36";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Timeout = 5000;
            request.ProtocolVersion = HttpVersion.Version11;
            request.ServicePoint.Expect100Continue = false;
            if (post) {

                    request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request );
                    allDone.WaitOne();
                    allDone.Reset();
                
            }

        }

        public bool SetRedirect {
            set {
                _redirect = value;
            }
        }

        void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
           

            //RequestState rs = (RequestState)asynchronousResult.AsyncState;
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            byte[] byteArray = Encoding.UTF8.GetBytes(_Postdata);

            postStream.Write(byteArray, 0, _Postdata.Length);

            if (_ismultipart)
            {
                byte[] file = File.ReadAllBytes(_filename);
                postStream.Write(file, 0, file.Length);
                string footer = "\r\n---" + _boundary + "--\r\n";
                postStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));
            }
            postStream.Close();
            allDone.Set();
           

        }

        public object Request(string url,WebProxy proxy)
        {
           
            object result = null;
            try
            {
                if (aborted)
                    throw new Exception();

                rstop = new RequestState();

               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                
                request.Proxy = proxy;
                request.KeepAlive = false;              
                
                Initialize(ref request);

                rstop.Request = request;
                Stopwatch st = new Stopwatch();
                st.Start();
                IAsyncResult r = (IAsyncResult)request.BeginGetResponse(new AsyncCallback(RespCallback), rstop);
                ThreadPool.RegisterWaitForSingleObject(r.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), rstop.Request, DefaultTimeout, true); 
                allDone.WaitOne();
                st.Stop();
                Debug.WriteLine(st.ElapsedMilliseconds);
                if (!_byte)
                {
                    if (rstop.RequestData != null)
                    {
                        if (!rstop.RequestData.ToString().Trim().Equals(string.Empty))
                        {

                            result = rstop.RequestData.ToString();

                        }
                    }
                }
                else
                {
                    if (rstop.RequestByte != null)
                    {

                        result = rstop.RequestByte;
                    }

                }
                if (rstop.ResponseStream != null)
                {
                    rstop.ResponseStream.Close();
                    Thread.Sleep(100);
                }
                

            }
            catch (Exception)
            {

                throw new Exception();
            }

            return result ;
        }
        
        void RespCallback(IAsyncResult ar)
        {
            RequestState rs = null;
            IAsyncResult iarRead = null;
            WebResponse resp = null;
            Stream ResponseStream = null;
            try
            {
                if (aborted)
                    throw new Exception();

                rs = (RequestState)ar.AsyncState;
                WebRequest req = rs.Request;
                try
                {
                    resp = req.EndGetResponse(ar);
                    ResponseStream = resp.GetResponseStream();
                    rs.ResponseStream = ResponseStream;
                    rs.Response = resp;
                    
                    
                    iarRead = ResponseStream.BeginRead(rs.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), rs);
                    
                    
                }
                catch (System.Net.Sockets.SocketException)
                {
                    throw new Exception();
                }
                catch (System.Exception)
                {
                    throw new Exception();
                }
            }
            catch (System.Exception)
            {

                if (!allDone.WaitOne(0))
                {
                    if (rs != null)
                    {
                        rs.RequestData = null;
                    }
                    allDone.Set();
                }

            }

        }
        
        void ReadCallBack(IAsyncResult asyncResult)
        {
            Stream responseStream = null;
            IAsyncResult ar = null;
            try
            {
                if (aborted)
                    throw new Exception();

                RequestState rs = (RequestState)asyncResult.AsyncState;
                responseStream = rs.ResponseStream;
                
                int read = responseStream.EndRead(asyncResult);

                if (read > 0)
                {
                    if (!_byte)
                    {
                        char[] charBuffer = new Char[BUFFER_SIZE];
                        int len = rs.StreamDecode.GetChars(rs.BufferRead, 0, read, charBuffer, 0);
                        string str = new String(charBuffer, 0, len);
                        rs.RequestData.Append(Encoding.ASCII.GetString(rs.BufferRead, 0, read));
                    }
                    else {

                        rs.RequestByte.Write(rs.BufferRead,0,read);
                    }
                    
                    try
                    {
                        ar = responseStream.BeginRead(rs.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), rs);
                    }
                    catch (System.Net.Sockets.SocketException)
                    {
                        throw new Exception();
                    }
                    catch (System.Exception)
                    {

                        throw new Exception();


                    }
                }
                else
                {

                    responseStream.Close();
                    allDone.Set();
                }

            }
            catch (System.Exception)
            {
                if (responseStream != null)
                    responseStream.Close();

                allDone.Set();

            }
            return;
        }
        
        public void Abort()
        {
            aborted = true;

        }

        void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut || aborted)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
    }
}
