using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UtilsSharp.Shared.Standard;

namespace UtilsSharp
{
    /// <summary>
    /// 网络请求工具帮助类
    /// </summary>
    public partial class WebHelper : WebClient
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<string> DoGet(string address, Dictionary<string, object> parameters = null)
        {
            return Request<Dictionary<string, object>, string>(HttpMethod.Get, address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<T> DoGet<T>(string address, Dictionary<string, object> parameters = null) where T : class
        {
            return Request<Dictionary<string, object>, T>(HttpMethod.Get, address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public BaseResult<string> DoGet<TP>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where TP : class, new()
        {
            return Request<TP, string>(HttpMethod.Get, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public BaseResult<T> DoGet<TP, T>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class where TP : class, new()
        {
            return Request<TP, T>(HttpMethod.Get, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<string> DoPost(string address, Dictionary<string, object> parameters = null)
        {
            return Request<Dictionary<string, object>, string>(HttpMethod.Post, address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<T> DoPost<T>(string address, Dictionary<string, object> parameters = null) where T : class
        {
            return Request<Dictionary<string, object>, T>(HttpMethod.Post, address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public BaseResult<string> DoPost<TP>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where TP : class, new()
        {
            return Request<TP, string>(HttpMethod.Post, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public BaseResult<T> DoPost<TP, T>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class where TP : class, new()
        {
            return Request<TP, T>(HttpMethod.Post, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="method">表示请求的http方法，大写， 如POST、GET、PUT</param>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public BaseResult<T> Request<TP, T>(HttpMethod method, string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class where TP : class, new()
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }
                var contentType = Headers["Content-Type"]?.ToLower();
                if (contentType == null)
                {
                    Headers.Add("Content-Type", "application/json");
                    contentType = Headers["Content-Type"].ToLower();
                }

                byte[] bytes;
                var @params = JsonConvert.SerializeObject(parameters);
                @params = Regex.Replace(@params, @"\\/Date\((\d+)\)\\/", match =>
                {
                    var dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString(dateTimeFormat);
                });

                if (method == HttpMethod.Get)
                {
                    var dicParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(@params);
                    var (item1, item2) = BuildUrlParameter(address, dicParameters);
                    address = item1;
                    var parametersStr = BuildQuery(item2);
                    if (!string.IsNullOrEmpty(parametersStr))
                    {
                        address = $"{item1}?{parametersStr}";
                    }
                    bytes = DownloadData(address);
                }
                else if (contentType.Contains("application/json"))
                {
                    var dataBytes = Encoding.GetBytes(@params);
                    bytes = UploadData(address, HttpMethod.Post.ToString(), dataBytes);
                }
                else
                {
                    var dicParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(@params);
                    var parametersStr = BuildQuery(dicParameters);
                    var dataBytes = Encoding.GetBytes(parametersStr);
                    bytes = UploadData(address, HttpMethod.Post.ToString(), dataBytes);
                }
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }
                if (typeof(T) == typeof(string))
                {
                    result.Result = (T)Convert.ChangeType(content, typeof(T));
                    return result;
                }
                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "WebException")
                {
                    var we = (WebException)ex;
                    using var hr = (HttpWebResponse)we.Response;
                    if (hr != null)
                    {
                        var statusCode = (int)hr.StatusCode;
                        var sb = new StringBuilder();
                        var responseStream = hr.GetResponseStream();
                        if (responseStream != null)
                        {
                            var sr = new StreamReader(responseStream, Encoding.UTF8);
                            sb.Append(sr.ReadToEnd());
                            result.SetError($"{sb}", statusCode);
                            return result;
                        }
                    }
                }
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
            finally
            {
                QueryString.Clear();
            }
        }

        /// <summary>
        /// 调整url和参数
        /// </summary>
        /// <param name="address">url地址</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        private static Tuple<string, Dictionary<string, string>> BuildUrlParameter(string address, Dictionary<string, string> parameters)
        {
            var url = address;
            parameters ??= new Dictionary<string, string>();
            if (address.Contains("?"))
            {
                var array = address.Split('?');
                url = array[0];
                var p = array[1];
                if (!string.IsNullOrEmpty(p))
                {
                    if (p.Contains("&"))
                    {
                        var pArray = p.Split('&');
                        foreach (var item in pArray)
                        {
                            if (string.IsNullOrEmpty(item)) continue;
                            var kvArray = item.Split('=');
                            var k = kvArray[0];
                            var v = kvArray[1];
                            if (!parameters.ContainsKey(k))
                            {
                                parameters.Add(k, v);
                            }
                        }
                    }
                    else
                    {
                        var kvArray = p.Split('=');
                        var k = kvArray[0];
                        var v = kvArray[1];
                        if (!parameters.ContainsKey(k))
                        {
                            parameters.Add(k, v);
                        }
                    }
                }
            }
            var tuple = new Tuple<string, Dictionary<string, string>>(url, parameters);
            return tuple;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private static string BuildQuery(IDictionary<string, string> parameters)
        {
            if (parameters == null || !parameters.Any())
            {
                return "";
            }

            var postData = new StringBuilder();
            var hasParam = false;
            using var dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                var name = dem.Current.Key;
                var value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value)) continue;
                if (hasParam)
                {
                    postData.Append("&");
                }

                postData.Append(name);
                postData.Append("=");
                postData.Append(value);
                hasParam = true;
            }

            return postData.ToString();
        }
    }

    /// <summary>
    /// 网络请求工具帮助类(异步)
    /// </summary>
    public partial class WebHelper
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoGetAsync(string address, Dictionary<string, object> parameters = null)
        {
            return await RequestAsync<Dictionary<string, object>, string>(HttpMethod.Get, address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoGetAsync<T>(string address, Dictionary<string, object> parameters = null) where T : class
        {
            return await RequestAsync<Dictionary<string, object>, T>(HttpMethod.Get, address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoGetAsync<TP>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where TP : class, new()
        {
            return await RequestAsync<TP, string>(HttpMethod.Get, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoGetAsync<TP, T>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class where TP : class, new()
        {
            return await RequestAsync<TP, T>(HttpMethod.Get, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoPostAsync(string address, Dictionary<string, object> parameters = null)
        {
            return await RequestAsync<Dictionary<string, object>, string>(HttpMethod.Post, address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoPostAsync<T>(string address, Dictionary<string, object> parameters = null) where T : class
        {
            return await RequestAsync<Dictionary<string, object>, T>(HttpMethod.Post, address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoPostAsync<TP>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where TP : class, new()
        {
            return await RequestAsync<TP, string>(HttpMethod.Post, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoPostAsync<TP, T>(string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class where TP : class, new()
        {
            return await RequestAsync<TP, T>(HttpMethod.Post, address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Request请求
        /// </summary>
        /// <typeparam name="TP">入参类型</typeparam>
        /// <typeparam name="T">出参类型</typeparam>
        /// <param name="method">表示请求的http方法，大写， 如POST、GET、PUT</param>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">入参的时间格式</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> RequestAsync<TP, T>(HttpMethod method, string address, TP parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class where TP : class, new()
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }
                var contentType = Headers["Content-Type"]?.ToLower();
                if (contentType == null)
                {
                    Headers.Add("Content-Type", "application/json");
                    contentType = Headers["Content-Type"].ToLower();
                }

                byte[] bytes;
                var @params = JsonConvert.SerializeObject(parameters);
                @params = Regex.Replace(@params, @"\\/Date\((\d+)\)\\/", match =>
                {
                    var dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString(dateTimeFormat);
                });

                if (method == HttpMethod.Get)
                {
                    var dicParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(@params);
                    var (item1, item2) = BuildUrlParameter(address, dicParameters);
                    address = item1;
                    var parametersStr = BuildQuery(item2);
                    if (!string.IsNullOrEmpty(parametersStr))
                    {
                        address = $"{item1}?{parametersStr}";
                    }
                    bytes = await DownloadDataTaskAsync(address);
                }
                else if (contentType.Contains("application/json"))
                {
                    var dataBytes = Encoding.GetBytes(@params);
                    bytes = await UploadDataTaskAsync(address, HttpMethod.Post.ToString(), dataBytes);
                }
                else
                {
                    var dicParameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(@params);
                    var parametersStr = BuildQuery(dicParameters);
                    var dataBytes = Encoding.GetBytes(parametersStr);
                    bytes = await UploadDataTaskAsync(address, HttpMethod.Post.ToString(), dataBytes);
                }
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }
                if (typeof(T) == typeof(string))
                {
                    result.Result = (T)Convert.ChangeType(content, typeof(T));
                    return result;
                }
                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "WebException")
                {
                    var we = (WebException)ex;
                    using var hr = (HttpWebResponse)we.Response;
                    if (hr != null)
                    {
                        var statusCode = (int)hr.StatusCode;
                        var sb = new StringBuilder();
                        var responseStream = hr.GetResponseStream();
                        if (responseStream != null)
                        {
                            var sr = new StreamReader(responseStream, Encoding.UTF8);
                            sb.Append(await sr.ReadToEndAsync());
                            result.SetError($"{sb}", statusCode);
                            return result;
                        }
                    }
                }
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
            finally
            {
                QueryString.Clear();
            }
        }
    }

    /// <summary>
    /// 网络请求工具帮助类(兼容超时)
    /// </summary>
    public partial class WebHelper
    {
        /// <summary>
        /// CookieContainer
        /// </summary>
        public CookieContainer CookieContainer { get; set; }
        private TimeCalculator _timer;
        private int _timeOut = 10;

        /// <summary>
        /// 过期时间(秒)
        /// </summary>
        public int Timeout
        {
            get => _timeOut;
            set
            {
                if (value <= 0)
                    _timeOut = 10;
                _timeOut = value;
            }
        }

        /// <summary>
        /// 重写GetWebRequest
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            if (request == null) return null;
            request.CookieContainer = CookieContainer;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 1000 * Timeout;
            request.ReadWriteTimeout = 1000 * Timeout;
            return request;
        }

        /// <summary>
        /// 带过期计时的下载
        /// </summary>
        public void DownloadFileAsyncWithTimeout(Uri address, string fileName, object userToken)
        {
            if (_timer == null)
            {
                _timer = new TimeCalculator(this) { Timeout = Timeout };
                _timer.TimeOver += _timer_TimeOver;
                DownloadProgressChanged += WebHelper_DownloadProgressChanged;
            }

            DownloadFileAsync(address, fileName, userToken);
            _timer.Start();
        }

        /// <summary>
        /// WebClient下载过程事件，接收到数据时引发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebHelper_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            _timer.Reset(); //重置计时器
        }

        /// <summary>
        /// 计时器过期
        /// </summary>
        /// <param name="userData"></param>
        private void _timer_TimeOver(object userData)
        {
            CancelAsync(); //取消下载
        }
    }

    /// <summary>
    /// 网络请求工具帮助类(处理返回实体)
    /// </summary>
    public static class WebHelperHandleResult
    {
        /// <summary>
        /// 处理返回实体
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="response">返回实体</param>
        /// <returns></returns>
        public static BaseResult<T> HandleResult<T>(this BaseResult<BaseResult<T>> response)
        {
            var result = new BaseResult<T>();
            if (response == null) return result;
            if (response.Code != 200)
            {
                result.Code = response.Code;
                result.Msg = response.Msg;
                return result;
            }
            result = response.Result;
            return result;
        }

        /// <summary>
        /// 处理返回实体
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="response">返回实体</param>
        /// <returns></returns>
        public static BasePagedResult<T> HandleResult<T>(this BaseResult<BasePagedResult<T>> response)
        {
            var result = new BasePagedResult<T>();
            if (response == null) return result;
            if (response.Code != 200)
            {
                result.Code = response.Code;
                result.Msg = response.Msg;
                return result;
            }
            result = response.Result;
            return result;
        }

        /// <summary>
        /// 处理返回实体(异步)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="response">返回实体</param>
        /// <returns></returns>
        public static async Task<BaseResult<T>> HandleResultAsync<T>(this Task<BaseResult<BaseResult<T>>> response)
        {
            return await Task.Run(() =>
            {
                var result = new BaseResult<T>();
                if (response == null) return result;
                if (response.Result.Code != 200)
                {
                    result.Code = response.Result.Code;
                    result.Msg = response.Result.Msg;
                    return result;
                }
                result = response.Result.Result;
                return result;
            });
        }

        /// <summary>
        /// 处理返回实体(异步)
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="response">返回实体</param>
        /// <returns></returns>
        public static async Task<BasePagedResult<T>> HandleResultAsync<T>(this Task<BaseResult<BasePagedResult<T>>> response)
        {
            return await Task.Run(() =>
            {
                var result = new BasePagedResult<T>();
                if (response == null) return result;
                if (response.Result.Code != 200)
                {
                    result.Code = response.Result.Code;
                    result.Msg = response.Result.Msg;
                    return result;
                }
                result = response.Result.Result;
                return result;
            });
        }
    }

    /// <summary>
    ///  创建计时器监视响应情况，过期则取消下载
    /// </summary>
    public class TimeCalculator
    {
        /// <summary>
        /// 时间到事件
        /// </summary>
        public event TimeoutCaller TimeOver;

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime _startTime;

        /// <summary>
        /// 时间戳
        /// </summary>
        private TimeSpan _timeout = new TimeSpan(0, 0, 10);

        /// <summary>
        /// 是否开始
        /// </summary>
        public bool CnHasStarted;

        /// <summary>
        /// 用户数据
        /// </summary>
        public readonly object CnUserData;

        /// <summary>
        /// 计时器构造方法
        /// </summary>
        /// <param name="userData">计时结束时回调的用户数据</param>
        public TimeCalculator(object userData)
        {
            TimeOver += OnTimeOver;
            CnUserData = userData;
        }

        /// <summary>
        /// 超时退出
        /// </summary>
        /// <param name="userData"></param>
        public virtual void OnTimeOver(object userData)
        {
            Stop();
        }

        /// <summary>
        /// 过期时间(秒)
        /// </summary>
        public int Timeout
        {
            get => _timeout.Seconds;
            set
            {
                if (value <= 0)
                    return;
                _timeout = new TimeSpan(0, 0, value);
            }
        }

        /// <summary>
        /// 是否已经开始计时
        /// </summary>
        public bool HasStarted => CnHasStarted;

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            Reset();
            CnHasStarted = true;
            var th = new Thread(WaitCall) { IsBackground = true };
            th.Start();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            CnHasStarted = false;
        }

        /// <summary>
        /// 检查是否过期
        /// </summary>
        /// <returns></returns>
        private bool CheckTimeout()
        {
            return (DateTime.Now - _startTime).Seconds >= Timeout;
        }

        private void WaitCall()
        {
            try
            {
                //循环检测是否过期
                while (CnHasStarted && !CheckTimeout())
                {
                    Thread.Sleep(1000);
                }

                TimeOver?.Invoke(CnUserData);
            }
            catch (Exception)
            {
                Stop();
            }
        }
    }

    /// <summary>
    /// 过期时回调委托
    /// </summary>
    /// <param name="userData">用户数据</param>
    public delegate void TimeoutCaller(object userData);
}
