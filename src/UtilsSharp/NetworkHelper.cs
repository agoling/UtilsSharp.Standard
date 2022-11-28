using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilsSharp
{
    /// <summary>
    /// 网络辅助帮助类
    /// </summary>
    public class NetworkHelper
    {
        /// <summary>
        /// 是否能Ping通指定主机
        /// </summary>
        public static bool Ping(string ip)
        {
            try
            {
                Ping ping = new Ping();
                PingOptions options = new PingOptions { DontFragment = true };
                string data = "Test Data";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 1000;
                PingReply reply = ping.Send(ip, timeout, buffer, options);
                return reply != null && reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }

        /// <summary>
        /// 网络是否畅通
        /// </summary>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public static bool IsInternetConnected()
        {
            int i;
            bool state = InternetGetConnectedState(out i, 0);
            return state;
        }

        [DllImport("wininet.dll")]
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        private static extern bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
    }
}
