using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using UtilsSharp.Standard;

namespace UtilsSharp
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    public class WebHelper : WebClient
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
            return GetRequest<string>(address,null);
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
            return GetRequest<string>(address,parameters);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="address">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        public BaseResult<T> DoGet<T>(string address, Dictionary<string, string> parameters) where T : class,new()
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
                    result.Result = (T)Convert.ChangeType(content, typeof(T));
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
            return PostRequest<string>(address,null);
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
                    result.Result = (T)Convert.ChangeType(content, typeof(T));
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
        public BaseResult<string> DoPost(string address, object parameters,string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")
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
        public BaseResult<T> DoPost<T>(string address, object parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss")where T : class, new()
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
        private BaseResult<T> PostRequest<T>(string address, object parameters, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss") where T : class
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
                    result.Result = (T)Convert.ChangeType(content, typeof(T));
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
}
