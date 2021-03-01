using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MonitorDataSys.UtilTool
{
    public class SendHelper
    {
        //公共方法类

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendGetGZIP(string url)
        {
            try
            {
                //发送请求
                var request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = 1000 * 5000;//50分钟
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                var rep = (HttpWebResponse)request.GetResponse();//得到请求结果
                Stream stream = rep.GetResponseStream();
                if (stream != null)
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseHtml = reader.ReadToEnd();
                        rep.Close();
                        return responseHtml;
                    }
                rep.Close();
                return null;//如果结果流为空，则返回为空
            }
            catch (Exception exp)
            {
                Loghelper.WriteErrorLog("请求失败", exp);
                //var temp = exp.Message;
                throw exp;//抛出异常
                //return null;//未查询到数据时，404错误
            }
            //return ErrorRestResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public static string SendPostGZIP(string url, string dataStr)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = "POST";
            byte[] data = Encoding.UTF8.GetBytes(dataStr);
            request.CookieContainer = new CookieContainer();
            request.AllowAutoRedirect = true;
            //request.ContentType = "text/plain";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 1000 * 5000;//50分钟
            try
            {
                Stream newstream = request.GetRequestStream();
                newstream.Write(data, 0, data.Length);//传输post的数据
                newstream.Flush();
                newstream.Close();

                //获得返回数据
                var response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseHtml = reader.ReadToEnd();
                        response.Close();
                        return responseHtml;
                    }
                }
                response.Close();
                return "";
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("请求失败", e);
                throw e;
            }
        }

        /// <summary>
        /// 发送Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendPost(string url)
        {
            string result = "";
            try
            {
                //发送请求
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = false;
                req.CookieContainer = new CookieContainer();
                //req.Timeout = 1 * 60 * 1000;//5分钟
                var rep = (HttpWebResponse)req.GetResponse();//得到请求结果
                if (rep.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = rep.GetResponseStream();
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            string responseHtml = reader.ReadToEnd();
                            rep.Close();
                            result = responseHtml;
                        }
                    }
                }
                else
                {
                    rep.Close();
                    result = "";
                }
                return result;//如果结果流为空，则返回为空
            }
            catch (Exception exp)
            {
                Loghelper.WriteErrorLog("请求失败", exp);
                //var temp = exp.Message;
                //throw exp;//抛出异常
                return "";//未查询到数据时，404错误
            }
            //return ErrorRestResult;
        }

        public static Stream SendPost(string url, bool isStream)
        {

            try
            {
                //发送请求
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.KeepAlive = false;
                req.Timeout = 1000 * 5000;//50分钟
                var rep = (HttpWebResponse)req.GetResponse();//得到请求结果
                Stream stream = rep.GetResponseStream();
                rep.Close();
                return stream;//如果结果流为空，则返回为空
            }
            catch (Exception exp)
            {
                Loghelper.WriteErrorLog("请求失败", exp);
                //var temp = exp.Message;
                //throw exp;//抛出异常
                return null;//未查询到数据时，404错误
            }
        }
        /// <summary>
        /// 发送GET网络请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public static string SendPost(string url, string dataStr)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = "POST";
            byte[] data = Encoding.UTF8.GetBytes(dataStr);
            request.CookieContainer = new CookieContainer();
            request.AllowAutoRedirect = true;
            //request.ContentType = "text/plain";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 1000 * 5000;//50分钟
            try
            {
                Stream newstream = request.GetRequestStream();
                newstream.Write(data, 0, data.Length);//传输post的数据
                newstream.Flush();
                newstream.Close();

                //获得返回数据
                var response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseHtml = reader.ReadToEnd();
                        response.Close();
                        return responseHtml;
                    }
                }
                response.Close();
                return "";
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("请求失败", e);
                throw e;
            }
        }

        /// <summary>
        /// 发送GET网络请求 ContentType = "text/plain";
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public static string SendPostText(string url, string dataStr)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = "POST";
            byte[] data = Encoding.UTF8.GetBytes(dataStr);
            request.CookieContainer = new CookieContainer();
            request.AllowAutoRedirect = true;
            request.ContentType = "text/plain";
            //request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Timeout = 1000 * 5000;//50分钟
            try
            {
                Stream newstream = request.GetRequestStream();
                newstream.Write(data, 0, data.Length);//传输post的数据
                newstream.Flush();
                newstream.Close();

                //获得返回数据
                var response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseHtml = reader.ReadToEnd();
                        response.Close();
                        return responseHtml;
                    }
                }

                response.Close();
                return "";
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("请求失败", e);
                throw e;
            }
        }

        /// <summary>
        /// 发送GET网络请求 ContentType = "text/plain";
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dataStr"></param>
        /// <returns></returns>
        public static string SendPostByte(string url, byte[] dataByte, string contentType = "application/x-www-form-urlencoded")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.KeepAlive = false;
            request.Method = "POST";
            byte[] data = dataByte;
            request.CookieContainer = new CookieContainer();
            request.AllowAutoRedirect = true;
            request.ContentType = contentType;
            //request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.Timeout = 1000 * 5000;//50分钟
            try
            {
                Stream newstream = request.GetRequestStream();
                newstream.Write(data, 0, data.Length);//传输post的数据
                newstream.Flush();
                newstream.Close();

                //获得返回数据
                var response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string responseHtml = reader.ReadToEnd();
                        response.Close();
                        return responseHtml;
                    }
                }
                response.Close();
                return "";
            }
            catch (Exception e)
            {
                Loghelper.WriteErrorLog("请求失败", e);
                throw e;
            }
        }

    }
}
