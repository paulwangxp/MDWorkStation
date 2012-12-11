using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Windows.Forms;

namespace MDWorkStation
{
    /*    
 * 作者：周公(zhoufoxcn)    
 * 日期：2011-05-08    
 * 原文出处：http://blog.csdn.net/zhoufoxcn 或http://zhoufoxcn.blog.51cto.com    
 * 版权说明：本文可以在保留原文出处的情况下使用于非商业用途，周公对此不作任何担保或承诺。    
 * */

    /// <summary>  
    /// 有关HTTP请求的辅助类  
    /// </summary>  
    public class HttpWebResponseUtility
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }

            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        public static HttpStatusCode getUrlRequestStatusCode(string methodUrl, string userCode/*上传人编号*/, string editCode/*采集人编号*/,
                                                                string uploadName/*上传文件名号*/, string uploadPath/*上传路径*/,string createTime/*文件创建时间*/,
                                                                string fileSize/*文件大小*/, string fileTime/*文件时长*/, out string responseText)
        {

            //return “0;/1/101/103/” 表示成功 或者 “1;失败信息”
            string tagUrl = methodUrl + "&userCode=" + userCode + "&editCode=" + editCode + "&uploadName=" + uploadName
                             + "&filePath=" + uploadPath + "&createTime=" + createTime + "&fileSize=" + fileSize + "&fileTime=" + fileTime;
            CookieCollection cookies = new CookieCollection();//如何从response.Headers["Set-Cookie"];中获取并设置CookieCollection的代码略  

            HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(tagUrl, null, null, cookies);

            Stream resStream = response.GetResponseStream();//返回从 Internet 资源返回数据流
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);//实例华一个流的读写器
            responseText = sr.ReadToEnd();//这就是百度首页的HTML哦 ,字符串形式的流的其余部分（从当前位置到末尾）。如果当前位置位于流的末尾，则返回空字符串 ("")
            resStream.Close();//关闭当前流并释放与之关联的所有资源
            sr.Close();


            return response.StatusCode; 

        }

        public static HttpStatusCode getFtpDirRequestStatusCode(string methodUrl, string userCode/*上传人编号*/, out string responseText)
        {
            //return “0;/1/101/103/” 表示成功 或者 “1;失败信息”

            string tagUrl = methodUrl + "&userCode=" + userCode;
            CookieCollection cookies = new CookieCollection();

            HttpWebResponse response = HttpWebResponseUtility.CreateGetHttpResponse(tagUrl, null, null, cookies);
            responseText = response.GetResponseStream().ToString();

            Stream resStream = response.GetResponseStream();//返回从 Internet 资源返回数据流
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);//实例华一个流的读写器
            responseText = sr.ReadToEnd();//这就是百度首页的HTML哦 ,字符串形式的流的其余部分（从当前位置到末尾）。如果当前位置位于流的末尾，则返回空字符串 ("")
            resStream.Close();//关闭当前流并释放与之关联的所有资源
            sr.Close();


            return response.StatusCode; 
        }

        private static string get_uft8(string unicodeString)
        {
            //UTF8Encoding utf8 =  new UTF8Encoding();
            Encoding utf8 = Encoding.GetEncoding("gb2312");
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }

        /*
         * 
         * （1）POST数据到HTTPS站点，用它来登录百度：
 
string loginUrl = "https://passport.baidu.com/?login";  
string userName = "userName";  
string password = "password";  
string tagUrl = "http://cang.baidu.com/"+userName+"/tags";  
Encoding encoding = Encoding.GetEncoding("gb2312");  
 
IDictionary<string, string> parameters = new Dictionary<string, string>();  
parameters.Add("tpl", "fa");  
parameters.Add("tpl_reg", "fa");  
parameters.Add("u", tagUrl);  
parameters.Add("psp_tt", "0");  
parameters.Add("username", userName);  
parameters.Add("password", password);  
parameters.Add("mem_pass", "1");  
HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(loginUrl, parameters, null, null, encoding, null);  
string cookieString = response.Headers["Set-Cookie"];
         * 
         * 
         * 
         * （2）发送GET请求到HTTP站点
 在cookieString中包含了服务器端返回的会话信息数据，从中提取了之后可以设置Cookie下次登录时带上这个Cookie就可以以认证用户的信息，假设我们已经登录成功并且获取了Cookie，那么发送GET请求的代码如下：
 
string userName = "userName";  
string tagUrl = "http://cang.baidu.com/"+userName+"/tags";  
CookieCollection cookies = new CookieCollection();//如何从response.Headers["Set-Cookie"];中获取并设置CookieCollection的代码略  
response = HttpWebResponseUtility.CreateGetHttpResponse(tagUrl, null, null, cookies);  
         * 
         * 
         * 
         * 
         * (3)发送POST请求到HTTP站点
 以登录51CTO为例：
 
string loginUrl = "http://home.51cto.com/index.php?s=/Index/doLogin";  
string userName = "userName";  
string password = "password";  
 
IDictionary<string, string> parameters = new Dictionary<string, string>();  
parameters.Add("email", userName);  
parameters.Add("passwd", password);  
 
HttpWebResponse response = HttpWebResponseUtility.CreatePostHttpResponse(loginUrl, parameters, null, null, Encoding.UTF8, null);  
         * 
         */
    }  
}
