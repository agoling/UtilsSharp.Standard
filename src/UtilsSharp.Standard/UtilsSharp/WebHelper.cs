using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UtilsSharp.Standard;

namespace UtilsSharp
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    public partial class WebHelper : WebClient
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<string> DoGet(string address, object parameters)
        {
            var strDic = ObjToDictionary(parameters);
            return GetRequest<string>(address, strDic);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<T> DoGet<T>(string address, object parameters) where T : class, new()
        {
            var strDic = ObjToDictionary(parameters);
            return GetRequest<T>(address, strDic);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public BaseResult<string> DoGet(string address)
        {
            return GetRequest<string>(address, null);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public BaseResult<T> DoGet<T>(string address) where T : class, new()
        {
            return GetRequest<T>(address, null);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<string> DoGet(string address, Dictionary<string, string> parameters)
        {
            return GetRequest<string>(address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<T> DoGet<T>(string address, Dictionary<string, string> parameters) where T : class, new()
        {
            return GetRequest<T>(address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        private BaseResult<T> GetRequest<T>(string address, Dictionary<string, string> parameters) where T : class
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }

                var (item1, item2) = BuildUrlParameter(address, parameters);
                address = item1;
                if (!string.IsNullOrEmpty(item2))
                {
                    address = $"{item1}?{item2}";
                }

                var bytes = DownloadData(address);
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }

                if (typeof(T) == typeof(string))
                {
                    result.Result = (T) Convert.ChangeType(content, typeof(T));
                    return result;
                }

                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public BaseResult<string> DoPost(string address)
        {
            return PostRequest<string>(address, null);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public BaseResult<T> DoPost<T>(string address) where T : class, new()
        {
            return PostRequest<T>(address, null);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<string> DoPost(string address, Dictionary<string, string> parameters)
        {
            return PostRequest<string>(address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<T> DoPost<T>(string address, Dictionary<string, string> parameters) where T : class, new()
        {
            return PostRequest<T>(address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        private BaseResult<T> PostRequest<T>(string address, Dictionary<string, string> parameters) where T : class
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }

                var (item1, item2) = BuildUrlParameter(address, parameters);
                address = item1;
                var dataBytes = new byte[0];
                if (!string.IsNullOrEmpty(item2))
                {
                    dataBytes = Encoding.GetBytes(item2);
                }

                var bytes = UploadData(address, HttpMethod.Post.ToString(), dataBytes);
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }

                if (typeof(T) == typeof(string))
                {
                    result.Result = (T) Convert.ChangeType(content, typeof(T));
                    return result;
                }

                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">返回的时间格式</param>
        /// <returns></returns>
        public BaseResult<string> DoPost(string address, object parameters,
            string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return PostRequest<string>(address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">返回的时间格式</param>
        /// <returns></returns>
        public BaseResult<T> DoPost<T>(string address, object parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
            where T : class, new()
        {
            return PostRequest<T>(address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">返回的时间格式</param>
        /// <returns></returns>
        private BaseResult<T> PostRequest<T>(string address, object parameters,
            string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }

                Headers.Add("Content-Type", "application/json;charset=UTF-8");
                var @params = JsonConvert.SerializeObject(parameters);
                @params = Regex.Replace(@params, @"\\/Date\((\d+)\)\\/", match =>
                {
                    var dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString(dateTimeFormat);
                });
                var dataBytes = new byte[0];
                if (!string.IsNullOrEmpty(@params))
                {
                    dataBytes = Encoding.GetBytes(@params);
                }

                var bytes = UploadData(address, HttpMethod.Post.ToString(), dataBytes);
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }

                if (typeof(T) == typeof(string))
                {
                    result.Result = (T) Convert.ChangeType(content, typeof(T));
                    return result;
                }

                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }

        /// <summary>
        /// 对象转字典
        /// </summary>
        /// <param name="obj">参数</param>
        /// <returns></returns>
        private static Dictionary<string, string> ObjToDictionary(object obj)
        {
            var objDic = DictionaryHelper.ObjToDictionary(obj);
            var strDic = new Dictionary<string, string>();
            if (objDic == null) return strDic;
            foreach (var item in objDic)
            {
                if (string.IsNullOrEmpty(item.Key) || item.Value == null) continue;
                var key = item.Key;
                object objValue;
                try
                {
                    objValue = Convert.ChangeType(item.Value, typeof(string));
                }
                catch (Exception)
                {
                    objValue = JsonConvert.SerializeObject(item.Value);
                }

                var value = objValue.ToString();
                strDic.Add(key, value);
            }

            return strDic;
        }

        /// <summary>
        /// 调整url和参数
        /// </summary>
        /// <param name="address">url地址</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        private Tuple<string, string> BuildUrlParameter(string address, Dictionary<string, string> parameters)
        {
            var url = address;
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

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

            var parameterStr = BuildQuery(parameters);
            var tuple = new Tuple<string, string>(url, parameterStr);
            return tuple;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private string BuildQuery(IDictionary<string, string> parameters)
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
                postData.Append(System.Web.HttpUtility.UrlEncode(value, Encoding));
                hasParam = true;
            }

            return postData.ToString();
        }

    }

    /// <summary>
    /// 网络工具类(异步)
    /// </summary>
    public partial class WebHelper : WebClient
    {
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoGetAsync(string address, object parameters)
        {
            var strDic = await ObjToDictionaryAsync(parameters);
            return await GetRequestAsync<string>(address, strDic);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoGetAsync<T>(string address, object parameters) where T : class, new()
        {
            var strDic = await ObjToDictionaryAsync(parameters);
            return await GetRequestAsync<T>(address, strDic);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoGetAsync(string address)
        {
            return await GetRequestAsync<string>(address, null);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoGetAsync<T>(string address) where T : class, new()
        {
            return await GetRequestAsync<T>(address, null);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoGetAsync(string address, Dictionary<string, string> parameters)
        {
            return await GetRequestAsync<string>(address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoGetAsync<T>(string address, Dictionary<string, string> parameters)
            where T : class, new()
        {
            return await GetRequestAsync<T>(address, parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        private async Task<BaseResult<T>> GetRequestAsync<T>(string address, Dictionary<string, string> parameters)
            where T : class
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }

                var buildUrlParameterRequest = new BuildUrlParameterRequest
                {
                    Address = address,
                    Parameters = parameters
                };
                var (item1, item2) = await BuildUrlParameterAsync(buildUrlParameterRequest);
                address = item1;
                if (!string.IsNullOrEmpty(item2))
                {
                    address = $"{item1}?{item2}";
                }

                var bytes = await DownloadDataTaskAsync(address);
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }

                if (typeof(T) == typeof(string))
                {
                    result.Result = (T) Convert.ChangeType(content, typeof(T));
                    return result;
                }

                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoPostAsync(string address)
        {
            return await PostRequestAsync<string>(address, null);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoPostAsync<T>(string address) where T : class, new()
        {
            return await PostRequestAsync<T>(address, null);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoPostAsync(string address, Dictionary<string, string> parameters)
        {
            return await PostRequestAsync<string>(address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoPostAsync<T>(string address, Dictionary<string, string> parameters)
            where T : class, new()
        {
            return await PostRequestAsync<T>(address, parameters);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        private async Task<BaseResult<T>> PostRequestAsync<T>(string address, Dictionary<string, string> parameters)
            where T : class
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }

                var buildUrlParameterRequest = new BuildUrlParameterRequest
                {
                    Address = address, Parameters = parameters
                };
                var (item1, item2) = await BuildUrlParameterAsync(buildUrlParameterRequest);
                address = item1;
                var dataBytes = new byte[0];
                if (!string.IsNullOrEmpty(item2))
                {
                    dataBytes = Encoding.GetBytes(item2);
                }

                var bytes = await UploadDataTaskAsync(address, HttpMethod.Post.ToString(), dataBytes);
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }

                if (typeof(T) == typeof(string))
                {
                    result.Result = (T) Convert.ChangeType(content, typeof(T));
                    return result;
                }

                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">返回的时间格式</param>
        /// <returns></returns>
        public async Task<BaseResult<string>> DoPostAsync(string address, object parameters,
            string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            return await PostRequestAsync<string>(address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">返回的时间格式</param>
        /// <returns></returns>
        public async Task<BaseResult<T>> DoPostAsync<T>(string address, object parameters,
            string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class, new()
        {
            return await PostRequestAsync<T>(address, parameters, dateTimeFormat);
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="dateTimeFormat">返回的时间格式</param>
        /// <returns></returns>
        private async Task<BaseResult<T>> PostRequestAsync<T>(string address, object parameters,
            string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class
        {
            var result = new BaseResult<T>();
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    result.SetError("address不能为空！");
                    return result;
                }

                Headers.Add("Content-Type", "application/json;charset=UTF-8");
                var @params = JsonConvert.SerializeObject(parameters);
                @params = Regex.Replace(@params, @"\\/Date\((\d+)\)\\/", match =>
                {
                    var dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString(dateTimeFormat);
                });
                var dataBytes = new byte[0];
                if (!string.IsNullOrEmpty(@params))
                {
                    dataBytes = Encoding.GetBytes(@params);
                }

                var bytes = await UploadDataTaskAsync(address, HttpMethod.Post.ToString(), dataBytes);
                var content = Encoding.GetString(bytes);
                if (string.IsNullOrEmpty(content))
                {
                    return result;
                }

                if (typeof(T) == typeof(string))
                {
                    result.Result = (T) Convert.ChangeType(content, typeof(T));
                    return result;
                }

                result.Result = JsonConvert.DeserializeObject<T>(content);
                return result;
            }
            catch (Exception ex)
            {
                result.SetError(ex.Message, BaseStateCode.TryCatch异常错误);
                return result;
            }
        }

        /// <summary>
        /// 对象转字典
        /// </summary>
        /// <param name="obj">参数</param>
        /// <returns></returns>
        private static async Task<Dictionary<string, string>> ObjToDictionaryAsync(object obj)
        {
            return await Task.Factory.StartNew(o =>
            {
                var objDic = DictionaryHelper.ObjToDictionary(o);
                var strDic = new Dictionary<string, string>();
                if (objDic == null) return strDic;
                foreach (var item in objDic)
                {
                    if (string.IsNullOrEmpty(item.Key) || item.Value == null) continue;
                    var key = item.Key;
                    object objValue;
                    try
                    {
                        objValue = Convert.ChangeType(item.Value, typeof(string));
                    }
                    catch (Exception)
                    {
                        objValue = JsonConvert.SerializeObject(item.Value);
                    }

                    var value = objValue.ToString();
                    strDic.Add(key, value);
                }

                return strDic;
            }, obj);
        }

        /// <summary>
        /// 调整url和参数
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        private async Task<Tuple<string, string>> BuildUrlParameterAsync(BuildUrlParameterRequest request)
        {
            var url = request.Address;
            if (request.Parameters == null)
            {
                request.Parameters = new Dictionary<string, string>();
            }

            if (request.Address.Contains("?"))
            {
                var array = request.Address.Split('?');
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
                            if (!request.Parameters.ContainsKey(k))
                            {
                                request.Parameters.Add(k, v);
                            }
                        }
                    }
                    else
                    {
                        var kvArray = p.Split('=');
                        var k = kvArray[0];
                        var v = kvArray[1];
                        if (!request.Parameters.ContainsKey(k))
                        {
                            request.Parameters.Add(k, v);
                        }
                    }
                }
            }

            var parameterStr = await BuildQueryAsync(request.Parameters);
            var tuple = new Tuple<string, string>(url, parameterStr);
            return tuple;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private async Task<string> BuildQueryAsync(IDictionary<string, string> parameters)
        {
            return await Task.Factory.StartNew(o =>
            {
                var obj = (IDictionary<string, string>) o;
                if (obj == null || !obj.Any())
                {
                    return "";
                }

                var postData = new StringBuilder();
                var hasParam = false;
                using var dem = obj.GetEnumerator();
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
                    postData.Append(System.Web.HttpUtility.UrlEncode(value, Encoding));
                    hasParam = true;
                }

                return postData.ToString();
            }, parameters);
        }
    }

    /// <summary>
    /// 网络工具类(兼容超时)
    /// </summary>
    public partial class WebHelper : WebClient
    {
        /// <summary>
        /// CookieContainer
        /// </summary>
        public CookieContainer CookieContainer { get; set; }
        private Calculagraph _timer;
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
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = CookieContainer;
            }
            HttpWebRequest httpRequest = (HttpWebRequest)request;
            httpRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpRequest.Timeout = 1000 * Timeout;
            httpRequest.ReadWriteTimeout = 1000 * Timeout;
            return httpRequest;
        }

        /// <summary>
        /// GetWebResponse
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            String setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            //do something if needed to parse out the cookie.
            if (setCookieHeader != null)
            {
                Cookie cookie = new Cookie(); //create cookie
                this.CookieContainer.Add(cookie);
            }

            return response;
        }

        /// <summary>
        /// 带过期计时的下载
        /// </summary>
        public void DownloadFileAsyncWithTimeout(Uri address, string fileName, object userToken)
        {
            if (_timer == null)
            {
                _timer = new Calculagraph(this) {Timeout = Timeout};
                _timer.TimeOver += _timer_TimeOver;
                DownloadProgressChanged +=WebHelper_DownloadProgressChanged;
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
        /// <param name="userdata"></param>
        private void _timer_TimeOver(object userdata)
        {
            CancelAsync(); //取消下载
        }


    }

    /// <summary>
    ///  创建计时器监视响应情况，过期则取消下载
    /// </summary>
    public class Calculagraph
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
        public bool cnHasStarted;

        /// <summary>
        /// 用户数据
        /// </summary>
        public readonly object cnUserdata;

        /// <summary>
        /// 计时器构造方法
        /// </summary>
        /// <param name="userdata">计时结束时回调的用户数据</param>
        public Calculagraph(object userdata)
        {
            TimeOver += OnTimeOver;
            cnUserdata = userdata;
        }

        /// <summary>
        /// 超时退出
        /// </summary>
        /// <param name="userdata"></param>
        public virtual void OnTimeOver(object userdata)
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
        public bool HasStarted => cnHasStarted;

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            Reset();
            cnHasStarted = true;
            var th = new Thread(WaitCall) {IsBackground = true};
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
            cnHasStarted = false;
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
                while (cnHasStarted && !CheckTimeout())
                {
                    Thread.Sleep(1000);
                }

                TimeOver?.Invoke(cnUserdata);
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
    /// <param name="userdata">用户数据</param>
    public delegate void TimeoutCaller(object userdata);

    /// <summary>
    /// 创建url参数请求
    /// </summary>
    internal class BuildUrlParameterRequest
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Address { set; get; }

        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, string> Parameters { set; get; }
    }
}
