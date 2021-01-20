using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using UtilsSharp.Standard;

namespace UtilsSharp
{
    /// <summary>
    /// Ip帮助类
    /// </summary>
    public class IpHelper
    {
        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            try
            {
                var headers = HttpContext.Current.Request.Headers;
                var arrays = new string[] { "HTTP_CLIENT_IP", "HTTP_X_FORWARDED_FOR", "X_FORWARDED_FOR", "X-REAL-IP", "HTTP_VIA", "HTTP_FROM", "REMOTE_ADDR" };
                foreach (var item in arrays)
                {
                    if (string.IsNullOrEmpty(headers[item].FirstOrDefault())) continue;
                    var ip = headers[item].FirstOrDefault();
                    return ip;
                }
                return "0.0.0.0";

            }
            catch
            {
                return "0.0.0.0";
            }
        }

        /// <summary>
        /// 获取服务端Ip
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            var ip = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList
                .FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                ?.ToString();
            return ip;
        }

        /// <summary>
        /// 获取Ip详细信息
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static IpInfo GetIpInfo(string ip)
        {
            var ipInfo = new IpInfo() { Status = "fail", Message = "invalid ip" };
            if (string.IsNullOrEmpty(ip))
            {
                ipInfo.Message = "IP cannot be empty";
                return ipInfo;
            }
            ip = ip.ToLower().Replace("https://", "").Replace("http://", "");
            if (!RegexHelper.IsIp(ip))
            {
                return ipInfo;
            }
            var webHelper=new WebHelper();
            var r=webHelper.DoGet<IpInfo>($"http://ip-api.com/json/{ip}?lang=zh-CN");
            return r.Result;
        }
    }

    /// <summary>
    /// Ip信息
    /// </summary>
    public class IpInfo
    {
        /// <summary>
        /// 状态 success fail
        /// </summary>
        public string Status { set; get; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { set; get; }
        /// <summary>
        /// 国家码
        /// </summary>
        public string CountryCode { set; get; }
        /// <summary>
        /// 地区码(省、州、自治区、直辖市)
        /// </summary>
        public string Region { set; get; }
        /// <summary>
        /// 地区名(省、州、自治区、直辖市)
        /// </summary>
        public string RegionName { set; get; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { set; get; }
        /// <summary>
        /// zip
        /// </summary>
        public string Zip { set; get; }
        /// <summary>
        /// lat
        /// </summary>
        public string Lat { set; get; }
        /// <summary>
        /// lon
        /// </summary>
        public string Lon { set; get; }
        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { set; get; }
        /// <summary>
        /// Isp
        /// </summary>
        public string Isp { set; get; }
        /// <summary>
        /// org
        /// </summary>
        public string Org { set; get; }
        /// <summary>
        /// as
        /// </summary>
        public string As { set; get; }
        /// <summary>
        /// 请求数据
        /// </summary>
        public string Query { set; get; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { set; get; }
    }
}
