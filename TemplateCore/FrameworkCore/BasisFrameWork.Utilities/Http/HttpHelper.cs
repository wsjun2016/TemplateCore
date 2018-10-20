using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace BasisFrameWork.Utilities.Http
{
    public static class HttpHelper {
        static HttpHelper() {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// 发送异步的Post请求，并返回Task<TEntity>对象实例
        /// </summary>
        /// <typeparam name="TEntity">承载返回结果的类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<TEntity> PostAsync<TEntity>(string url, string parameter = "", string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) {
            TEntity rc = default(TEntity);

            string message = await PostOnStringAsync(url, parameter, encoding, keepAlive, timeout, contentType, charset, headers);
            if (!string.IsNullOrWhiteSpace(message?.Trim()))
                rc = JsonConvert.DeserializeObject<TEntity>(message);

            return rc;
        }

        /// <summary>
        /// 发送异步的Post请求，并返回Task<string>对象实例
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<string> PostOnStringAsync(string url, string parameter = "", string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) {
            string rc = "";

            HttpResponseMessage response = await PostAsync(url, parameter, encoding, keepAlive, timeout, contentType, charset, headers);
            if (response != null && response.IsSuccessStatusCode) {
                using (response) {
                    rc = await response.Content.ReadAsStringAsync();
                }
            }

            return rc;
        }

        /// <summary>
        /// 发送异步的Post请求，并返回Task<HttpResponseMessage>对象实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数的对象实例</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync<TEntity>(string url, TEntity parameter = default(TEntity), string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) where TEntity : class, new() {
            string requestParameter = contentType?.ToLower() == "application/json" ? parameter.ToJson() : parameter.ToQueryString();
            return await PostAsync(url, requestParameter, encoding, keepAlive, timeout, contentType, charset, headers);
        }

        /// <summary>
        /// 发送异步的Post请求，并返回Task<HttpResponseMessage>对象实例
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync(string url, string parameter = "", string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) {
            return await SendAsync(url, parameter, HttpMethod.Post, encoding, keepAlive, timeout, contentType, charset, headers);
        }


        /// <summary>
        /// 发送异步的Get请求，并返回Task<TEntity>对象实例
        /// </summary>
        /// <typeparam name="TEntity">承载返回结果的类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<TEntity> GetAsync<TEntity>(string url, string parameter = "", string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) {
            TEntity rc = default(TEntity);

            string message = await GetOnStringAsync(url, parameter, encoding, keepAlive, timeout, contentType, charset, headers);
            if (!string.IsNullOrWhiteSpace(message?.Trim()))
                rc = JsonConvert.DeserializeObject<TEntity>(message);

            return rc;
        }

        /// <summary>
        /// 发送异步的Get请求，并返回Task<string>对象实例
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<string> GetOnStringAsync(string url, string parameter = "", string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) {
            string rc = "";

            HttpResponseMessage response = await GetAsync(url, parameter, encoding, keepAlive, timeout, contentType, charset, headers);
            if (response != null && response.IsSuccessStatusCode) {
                using (response) {
                    rc = await response.Content.ReadAsStringAsync();
                }
            }

            return rc;
        }

        /// <summary>
        /// 发送异步的Get请求，并返回Task<HttpResponseMessage>对象实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数的对象实例</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync<TEntity>(string url, TEntity parameter = default(TEntity), string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) where TEntity : class, new() {
            string requestParameter = parameter.ToQueryString();
            return await GetAsync(url, requestParameter, encoding, keepAlive, timeout, contentType, charset, headers);
        }

        /// <summary>
        /// 发送异步的Get请求，并返回Task<HttpResponseMessage>对象实例
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(string url, string parameter = "", string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) {
            return await SendAsync(url, parameter, HttpMethod.Get, encoding, keepAlive, timeout, contentType, charset, headers);
        }

        /// <summary>
        /// 发送异步的Post请求，并返回Task<HttpResponseMessage>对象实例
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="method">请求的方式</param>
        /// <param name="encoding">对parameter的编码格式</param>
        /// <param name="keepAlive">是否保持连接 true:是 false:否</param>
        /// <param name="timeout">超时时间，单位：秒</param>
        /// <param name="contentType">请求的内容类型</param>
        /// <param name="charset">请求的内容的charset字符集</param>
        /// <param name="headers">自定义的header集合</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendAsync(string url, string parameter, HttpMethod method, string encoding = "utf-8", bool keepAlive = false, int timeout = 120, string contentType = "application/x-www-form-urlencoded", string charset = "utf-8", Dictionary<string, string> headers = null) {
            HttpResponseMessage rc = null;

            if (string.IsNullOrWhiteSpace(url?.Trim()))
                throw new ArgumentNullException(nameof(url));

            using (HttpClient client = new HttpClient()) {
                if (headers == null)
                    headers = new Dictionary<string, string>();

                if (!(headers.Keys?.Any(it => it.ToUpper() == "ACCEPT-LANGUAGE") ?? false))
                    headers.Add("Accept-Language", "zh-cn");
                if (!(headers.Keys?.Any(it => it.ToUpper() == "ACCEPT") ?? false))
                    headers.Add("Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-silverlight, */*");
                if (!(headers.Keys?.Any(it => it.ToUpper() == "USER-AGENT") ?? false) && method == HttpMethod.Post)
                    headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)");
                if (!(headers.Keys?.Any(it => it.ToUpper() == "KEEPALIVE") ?? false))
                    headers.Add("KeepAlive", keepAlive.ToString());
                foreach (KeyValuePair<string, string> header in headers)
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);

                client.Timeout = TimeSpan.FromSeconds(timeout);

                if (url.IndexOf("http://") == -1 && url.IndexOf("https://") == -1)
                    url = "http://" + url;

                //Get方式
                if (method == HttpMethod.Get) {
                    if (!string.IsNullOrWhiteSpace(parameter?.Trim())) {
                        if (url.IndexOf("?") == -1)
                            url += "?" + parameter;
                        else if (url.IndexOf("&") == -1)
                            url += parameter;
                        else
                            url += "&" + parameter;
                    }

                    rc = await client.GetAsync(url);
                }
                //Post方式
                else {
                    byte[] buffer = Encoding.GetEncoding(encoding).GetBytes(parameter);
                    using (Stream stream = new MemoryStream(buffer)) {
                        using (HttpContent content = new StreamContent(stream)) {
                            content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                            content.Headers.ContentType.CharSet = charset;
                            rc = await client.PostAsync(url, content);
                        }
                    }
                }
            }

            return rc;
        }


        /// <summary>
        /// 将某个实体对象转换为Http查询字符串
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToQueryString<TEntity>(this TEntity entity) where TEntity : class, new() {
            string rc = "";

            if (entity == default(TEntity)) return rc;
            PropertyInfo[] properties = entity.GetType().GetProperties();
            if (properties?.Length > 0) {
                foreach (var property in properties)
                    rc += $"{property.Name}={property.GetValue(entity)}&";
                rc = rc.TrimEnd('&');
            }

            return rc;
        }

        /// <summary>
        /// 将某个实体对象转换为Json字符串
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string ToJson<TEntity>(this TEntity entity) where TEntity : class, new() {
            if (entity == default(TEntity)) return "";
            return Newtonsoft.Json.JsonConvert.SerializeObject(entity); ;
        }

        /// <summary>
        /// 从HttpResponseMessage获取cookie字符串
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetCookie(this HttpResponseMessage response) {
            return (response?.Headers?.Contains("Set-Cookie") ?? false)
                ? response.Headers.GetValues("Set-Cookie").FirstOrDefault()
                : "";
        }

        /// <summary>
        /// 获取HttpResponseMessage的字符串结果
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetResult(this HttpResponseMessage response) {
            string rc = "";

            if (response != null && response.IsSuccessStatusCode) {
                using (response) {
                    rc = response.Content.ReadAsStringAsync().Result;
                }
            }

            return rc;
        }

        /// <summary>
        /// 将HttpResponseMessage的Json字符串结果转换为TEntity类型的对象实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static TEntity GetResultFromJson<TEntity>(this HttpResponseMessage response) {
            TEntity rc = default(TEntity);

            string message = response.GetResult();
            if (!string.IsNullOrWhiteSpace(message?.Trim()))
                rc = JsonConvert.DeserializeObject<TEntity>(message);

            return rc;
        }

        /// <summary>
        /// 将HttpResponseMessage的xml字符串结果转换为TEntity类型的对象实例
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static TEntity GetResultFromXml<TEntity>(this HttpResponseMessage response) {
            TEntity rc = default(TEntity);

            string message = response.GetResult();
            if (!string.IsNullOrWhiteSpace(message?.Trim()))
                using (StringReader reader = new StringReader(message)) {
                    rc = (TEntity)new XmlSerializer(typeof(TEntity)).Deserialize(reader);
                }

            return rc;
        }

        /// <summary>
        /// Http方法
        /// </summary>
        public enum HttpMethod {
            Post = 1,
            Get = 2
        }

    }
}
