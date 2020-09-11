using System;
using System.Collections.Generic;
using System.Linq;
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
            var ip = HttpContext.Current.Connection.RemoteIpAddress.ToString();
            return ip;
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
    }
}
